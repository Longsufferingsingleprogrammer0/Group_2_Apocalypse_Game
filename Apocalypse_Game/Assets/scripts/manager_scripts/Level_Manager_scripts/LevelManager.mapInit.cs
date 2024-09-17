using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
    private bool[] mapGrid;
    

    private Vector2 calculateGridGlobalPosition(int gridX,int gridY, Vector2 gridOffset)
    {
        float globalX = mapData.getGridZeroPoint().x + ((float)gridX) + gridOffset.x;
        float globalY = mapData.getGridZeroPoint().y + (-(float)gridY) + gridOffset.y;

        return new Vector2(globalX, globalY);        

    }

    private Vector2 calculateGridGlobalPosition(int gridX, int gridY)
    {
        float globalX = mapData.getGridZeroPoint().x + ((float)gridX);
        float globalY = mapData.getGridZeroPoint().y + (-(float)gridY);

        return new Vector2(globalX, globalY);

    }


    private void addTakenSpaceToMap(GridIllegalSpawnZone takenSpace)
    {

    }



    private void spawnSetPeice(SetpeiceSpawnPosition spawnPosition, GameObject[] varientTable)
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

        //spawn the set peice
        GameObject newPeice = Instantiate(varientTable[spawnPosition.getPrefabVariant()], spawnpos, new Quaternion(0f,0f,0f,0f));

        //add the setpeice to the setpeices list
        Setpeices.Add(newPeice);


    }

    private IEnumerator placeSetpeiceSet(SetpeiceObjectSpawnTable spawnTable) 
    {

        int counter = 0;
        GameObject[] varients = spawnTable.getActorPrefabVariants();
        


        for (int setpeice = 0; setpeice < spawnTable.getSpawnPositionCount(); setpeice++)
        {

            spawnSetPeice(spawnTable.GetSetpeiceSpawnPosition(setpeice), varients);

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
            Debug.Log(openIndexes.Count.ToString());
            Debug.Log(chosenIndex.ToString());


            SetpeiceSpawnPosition toBePlaced = openIndexes[chosenIndex];

            openIndexes.RemoveAt(chosenIndex);

            spawnSetPeice(toBePlaced,varients);

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
        


        yield return null;
    }


    private IEnumerator initializeMap()
    {
        
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
            playerSprite.GetComponent<Rigidbody2D>().position = calculateGridGlobalPosition(mapData.getPlayerStartPos().getX(),mapData.getPlayerStartPos().getX());
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
