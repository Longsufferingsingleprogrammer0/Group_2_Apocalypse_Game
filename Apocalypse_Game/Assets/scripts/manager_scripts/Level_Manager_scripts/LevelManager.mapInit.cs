using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

//need to heavily modify for new map spawning system
public partial class LevelManager : MonoBehaviour
{

    //whether the map setup is done, an int to allow for switch case optomisation
    private int mapSetupStage = 0;

    //the spawndata object for this level
    [SerializeField] private LevelSpawnData mapData;

    //position of the loading screens
    [SerializeField] private Vector2 loadingScreenPos;
    [SerializeField] private Vector2 loadingDoneScreenPos;

    

   

    private GameObject playerSprite;

    //the player's tag
    [SerializeField] private string playerTag;

    //put the variables for spawning actors here

    [SerializeField] private int maxSetpeicesSpawnedPerFrame;

    private List<GameObject> Setpeices;

    private List<GameObject> Items;

    private List<GameObject> Collectables;

    private List<GameObject> Enemies;

    //the grid used to figure out and store what parts of the map are taken or not
    private bool[][] mapGrid;
    



    private IEnumerator addTakenSpaceToMap(GridIllegalSpawnZone[] takenSpace, GridVector2 objectPosition)
    {
        if (!(takenSpace == null || takenSpace.Length <= 0))
        {
            for (int section = 0; section < takenSpace.Length; section++)
            {
                //cache variables
                int x1 = takenSpace[section].getTopLeftCorner().getX();
                int x2 = takenSpace[section].getBottomRightCorner().getX();
                int y1 = takenSpace[section].getTopLeftCorner().getY();
                int y2 = takenSpace[section].getBottomRightCorner().getY();
                int mapLimitX = mapData.getGridSize().getX();
                int mapLimitY = mapData.getGridSize().getY();
                int objectX = objectPosition.getX();
                int objectY = objectPosition.getY();

                if (x2 < x1)
                {
                    throw new System.Exception("illegal spawn zone error, corners in incorrect x order (x2 must be greater than or equal to x1)");
                }
                else if (y2 < y1)
                {
                    throw new System.Exception("illegal spawn zone error, corners in incorrect y order (y2 must be greater than or equal to y1)");
                }

                int yDistance = (y2 - y1);
                int xDistance = (x2 - x1);

                for (int y = 0; y <= yDistance; y++)
                {
                    for (int x = 0; x <= xDistance; x++)
                    {
                        //calculate the position to update
                        int selectedX = x + x1 + objectX;
                        int selectedY = y + y1 + objectY;
                        //if we are in bounds
                        if ((selectedX >= 0) && (selectedX < mapLimitX) && (selectedY >= 0) && (selectedY < mapLimitY))
                        {
                            mapGrid[selectedY][selectedX] = true;
                        }
                    }
                    yield return null;
                }
                yield return null;
            }
        }
    }

    private IEnumerator addTakenSpaceToMap(GridIllegalSpawnZone takenSpace)
    {
        //cache variables
        int x1 = takenSpace.getTopLeftCorner().getX();
        int x2 = takenSpace.getBottomRightCorner().getX();
        int y1 = takenSpace.getTopLeftCorner().getY();
        int y2 = takenSpace.getBottomRightCorner().getY();
        int mapLimitX = mapData.getGridSize().getX();
        int mapLimitY = mapData.getGridSize().getY();


        int yDistance = (y2 - y1);
        int xDistance = (x2 - x1);

        for (int y = 0; y <= yDistance; y++)
        {
            for (int x = 0; x <= xDistance; x++)
            {
                //calculate the position to update
                int selectedX = x + x1;
                int selectedY = y + y1;

                if ((selectedX >= 0) && (selectedX < mapLimitX) && (selectedY >= 0) && (selectedY < mapLimitY))
                {
                    mapGrid[selectedY][selectedX] = true;
                }
            }
            yield return null;
        }
    }






    private Vector2 calculateGridGlobalPosition(int gridX, int gridY, Vector2 gridOffset)
    {
        float globalX = mapData.getGridZeroPoint().x + gridOffset.x;
        globalX += (float)gridX;
        float globalY = mapData.getGridZeroPoint().y - gridOffset.y;
        globalY -= (float)gridY;

        return new Vector2(globalX, globalY);

    }

    private Vector2 calculateGridGlobalPosition(int gridX, int gridY)
    {
        float globalX = mapData.getGridZeroPoint().x;
        globalX += gridX;
        float globalY = mapData.getGridZeroPoint().y;
        globalY -= gridY;


        return new Vector2(globalX, globalY);

    }








    private IEnumerator spawnSetPeice(SetpeiceSpawnPosition spawnPosition, GameObject[] varientTable)
    {
        //check to make sure there is something to spawn
        if((varientTable==null)||(varientTable.Length == 0))
        {
            throw new System.Exception("setpeice to spawn has an empty varient table");
        }

        //convert the grid position to unity coordinates
        Vector2 globalPosition = calculateGridGlobalPosition(spawnPosition.getPosition().getX(), spawnPosition.getPosition().getY(), spawnPosition.getGridPositionOffset());

        //convert the global position vector 2 to a vector 3
        Vector3 spawnpos = new Vector3(globalPosition.x,globalPosition.y,0f);

        Debug.Log(spawnpos.ToString());

        //spawn the set peice
        GameObject newPeice = Instantiate(varientTable[spawnPosition.getPrefabVariant()], spawnpos, new Quaternion(0f,0f,0f,0f));

        

        //add the setpeice to the setpeices list
        Setpeices.Add(newPeice);
        yield return null;

        StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Logic>().getGridSize(),spawnPosition.getPosition()));

        

        //account for offsets
        Vector2 offset = spawnPosition.getGridPositionOffset();
        GridVector2 gridspawn = spawnPosition.getPosition();
        bool third = false;

        if (Mathf.CeilToInt(Mathf.Abs(offset.x)) + Mathf.CeilToInt(Mathf.Abs(offset.y)) > 1)
        {
            third = true;
        }
        yield return null;
        //are we offset left or right
        switch (Mathf.CeilToInt(offset.x)+2)
        {
            case 1:
                StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Logic>().getGridSize(), new GridVector2(gridspawn.getX() - 1, gridspawn.getY())));
                break;
            case 2:
                break;
            case 3:
                StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Logic>().getGridSize(), new GridVector2(gridspawn.getX() + 1, gridspawn.getY())));
                break;

        }

        yield return null;
        switch (Mathf.CeilToInt(offset.y) + 2)
        {
            case 1:
                StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Logic>().getGridSize(), new GridVector2(gridspawn.getX(), gridspawn.getY()-1)));
                break;
            case 2:
                break;
            case 3:
                StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Logic>().getGridSize(), new GridVector2(gridspawn.getX(), gridspawn.getY()+1)));
                break;

        }
        yield return null;

        if (third)
        {
            
        }

    }



    



    private IEnumerator placeSetpeiceSet(SetpeiceObjectSpawnTable spawnTable) 
    {

        int counter = 0;
        GameObject[] varients = spawnTable.getActorPrefabVariants();
        


        for (int setpeice = 0; setpeice < spawnTable.getSpawnPositionCount(); setpeice++)
        {

            StartCoroutine(spawnSetPeice(spawnTable.GetSetpeiceSpawnPosition(setpeice), varients));

            if (counter >= maxSetpeicesSpawnedPerFrame)
            {
                counter = 0;
                yield return null;
            }
            else
            {
                counter++;
            }
        }
    }


    private IEnumerator randomPlaceSetpeiceSet(SetpeiceObjectSpawnTable spawnTable)
    {
        int number= Random.Range(spawnTable.getMinimumSpawnNumber(), spawnTable.getMaximumSpawnNumber()+1);
        int counter = 0;
        GameObject[] varients = spawnTable.getActorPrefabVariants();

        List<SetpeiceSpawnPosition> openIndexes = new List<SetpeiceSpawnPosition>(spawnTable.getPossibleSpawnPositions());

        for (int setpeice=0; setpeice<number; setpeice++)
        {
            
            int chosenIndex=Random.Range(0, openIndexes.Count);



            SetpeiceSpawnPosition toBePlaced = openIndexes[chosenIndex];

            openIndexes.RemoveAt(chosenIndex);

            StartCoroutine(spawnSetPeice(toBePlaced,varients));

            if (counter >= maxSetpeicesSpawnedPerFrame)
            {
                counter = 0;
                yield return null;
            }
            else
            {
                counter++;
            }
            
        }

        
    }

    
    private IEnumerator initializeMapSetpeices()
    {
        for(int setPeiceType=0; setPeiceType<mapData.getSpawnTableArrayLength(); setPeiceType++)
        {
            SetpeiceObjectSpawnTable spawnTable = mapData.getObjectSpawnTable(setPeiceType);

            if (spawnTable == null)
            {
                throw new System.Exception("null object spawn table error");
            }
            else if (spawnTable.getPossibleSpawnPositions()==null)
            {
                throw new System.Exception("null possible spawn position array error");
            }else if (spawnTable.getSpawnPositionCount() == 0)
            {
                throw new System.Exception("empty object position array error");
            }else if (spawnTable.getMaximumSpawnNumber() > spawnTable.getSpawnPositionCount() && spawnTable.isSpawningRandomized())
            {
                throw new System.Exception("maximum number of setpeices spawnable is greater than possible positions error");
            }




            if (spawnTable.isSpawningRandomized())
            {
                StartCoroutine(randomPlaceSetpeiceSet(spawnTable));
            }
            else
            {
                StartCoroutine(placeSetpeiceSet(spawnTable));
            }
            yield return null;
            
        }
        

        this.mapSetupStage++;
        
    }
    
    private IEnumerator initializeMapActors()
    {
        //when adding enemies and items, put them here
        //yield return null;
        yield return null;
    }



    private IEnumerator initializeMapNoSpawnZones()
    {
        if (!(mapData.getOutOfMapGridAreas() == null || mapData.getOutOfMapGridAreaListLength() <= 0)) { 
            for (int zone = 0; zone < mapData.getOutOfMapGridAreaListLength(); zone++)
            {

                StartCoroutine(addTakenSpaceToMap(mapData.getOutOfMapGridArea(zone)));
                yield return null;
            }
        }
        mapSetupStage++;
    }


    private void initializeGridArray(int x, int y)
    {
        bool[][] yArray = new bool[y][];
        for(int i=0; i<y; i++)
        {
            yArray[i] = new bool[x];
            for(int j=0; j < yArray[i].Length; j++)
            {
                yArray[i][j] = false;
            }
        }
        mapGrid = yArray;
        mapSetupStage++;
    }


    private IEnumerator initializeMap()
    {

        
        initializeGridArray(mapData.getGridSize().getX(), mapData.getGridSize().getY());
        yield return null;
        StartCoroutine(initializeMapNoSpawnZones());
        yield return null;
        //set up the dynamic parts of the map
        StartCoroutine(initializeMapSetpeices());
        yield return null;
        StartCoroutine(initializeMapActors());
        yield return null;
        //setup done, go to confirm screen
        mapSetupStage++;
        playerSprite.GetComponent<Rigidbody2D>().position = loadingDoneScreenPos;
    }


    private void waitScreen()
    {
       
        if (Input.anyKeyDown)
        {
           
            mapSetupStage++;
            playerSprite.GetComponent<Rigidbody2D>().position = calculateGridGlobalPosition(mapData.getPlayerStartPos().getX(),mapData.getPlayerStartPos().getY());
            playerSprite.GetComponent<SpriteRenderer>().enabled = true;
            playerSprite.GetComponent<Player>().setPlayerMovementEnabled(true);
        }
    }

    private void mapSetupStageManager()
    {
        
        switch (mapSetupStage)
        {
            case 0:
         
                break;
            case 1:
            
                break;
            case 2:

                break;
            case 3:

                break;
            case 4:
              
                waitScreen();
                break;
            default:
                break;
        }
    }


    // Start is called before the first frame update
    void MapInitStart()
    {
        //get the player and put them on the loading spot
        playerSprite = GameObject.FindWithTag(playerTag);
        playerSprite.GetComponent<Rigidbody2D>().position = loadingScreenPos;

        //initialize our actor lists
        Setpeices = new List<GameObject>();
        Items = new List<GameObject>();
        Collectables = new List<GameObject>();
        Enemies = new List<GameObject>();

        //start the initialisation process
        StartCoroutine(initializeMap());
        
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        mapSetupStageManager();
        
    }
}
