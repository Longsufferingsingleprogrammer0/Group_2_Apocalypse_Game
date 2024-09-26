
using System.Collections;
using System.Collections.Generic;

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

    //debug variables
    [SerializeField] private GameObject takenTileObject;
    [SerializeField] private bool showTakenTiles;


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

    private int currentItem;
    private int currentEnemy;

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








    private IEnumerator spawnSetPeice(SetpeiceSpawnPosition spawnPosition, GameObject[] varientTable, int element)
    {
        //check to make sure there is something to spawn
        if ((varientTable == null) || (varientTable.Length == 0))
        {
            throw new System.Exception("setpeice to spawn has an empty varient table");
        }

        //convert the grid position to unity coordinates
        Vector2 globalPosition = calculateGridGlobalPosition(spawnPosition.getPosition().getX(), spawnPosition.getPosition().getY(), spawnPosition.getGridPositionOffset());

        //convert the global position vector 2 to a vector 3
        Vector3 spawnpos = new Vector3(globalPosition.x, globalPosition.y, 0f);



        //spawn the set peice
        GameObject newPeice = Instantiate(varientTable[spawnPosition.getPrefabVariant()], spawnpos, new Quaternion(0f, 0f, 0f, 0f));


        Setpeice_Script newPeiceController = newPeice.GetComponent<Setpeice_Script>();

        if (newPeiceController == null)
        {
            Debug.Log(varientTable[spawnPosition.getPrefabVariant()].name);

            throw new System.Exception("newPeice has no controller");
        }


        newPeiceController.setElelment(element);

        //add the setpeice to the setpeices list
        Setpeices.Add(newPeice);


        yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), spawnPosition.getPosition()));
        yield return null;


        //account for offsets (not at all efficent, but i dont want to waste time on that

        int angleOffset = 0;

        if (spawnPosition.getVerticalOffsetNoSpawnCompensation())
        {
            if (spawnPosition.getGridPositionOffset().y > 0f)
            {
                angleOffset += 1;
                yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX(), spawnPosition.getPosition().getY() + 1)));
            } else if (spawnPosition.getGridPositionOffset().y < 0f)
            {
                angleOffset += 2;
                yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX(), spawnPosition.getPosition().getY() - 1)));
            }

            yield return null;
        }



        if (spawnPosition.getHorizontalOffsetNoSpawnCompensation())
        {
            if (spawnPosition.getGridPositionOffset().x > 0f)
            {
                angleOffset += 10;
                yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX() + 1, spawnPosition.getPosition().getY())));
            } else if (spawnPosition.getGridPositionOffset().x < 0f)
            {
                angleOffset += 20;
                yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX() - 1, spawnPosition.getPosition().getY())));
            }

            yield return null;
        }



        if (spawnPosition.getHorizontalOffsetNoSpawnCompensation() && spawnPosition.getVerticalOffsetNoSpawnCompensation())
        {
            switch (angleOffset)
            {
                case 11:
                    yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX() + 1, spawnPosition.getPosition().getY() + 1)));
                    break;

                case 12:
                    yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX() + 1, spawnPosition.getPosition().getY() - 1)));
                    break;

                case 21:
                    yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX() - 1, spawnPosition.getPosition().getY() + 1)));
                    break;

                case 22:
                    yield return StartCoroutine(addTakenSpaceToMap(newPeice.GetComponent<Setpeice_Script>().getGridSize(), new GridVector2(spawnPosition.getPosition().getX() - 1, spawnPosition.getPosition().getY() - 1)));
                    break;
                default:
                    break;
            }



        }


    }



    
    private void debugNoSpawnZoneMaskGenerator()
    {
        for(int y = 0; y < mapGrid.Length; y++)
        {
            int counter = 0;
            for(int x = 0; x < mapGrid[y].Length; x++)
            {
                if (mapGrid[y][x])
                {
                    Vector2 spawnPoint = calculateGridGlobalPosition(x, y);
                    Vector3 spawnPoint3D = new Vector3(spawnPoint.x, spawnPoint.y, 0f);
                    Instantiate(takenTileObject, spawnPoint3D, new Quaternion(0f, 0f, 0f, 0f));
                    if (counter > maxSetpeicesSpawnedPerFrame)
                    {
                        counter = 0;
                        
                    }
                }
            }

            
        }
    }


    private IEnumerator placeSetpeiceSet(SetpeiceObjectSpawnTable spawnTable) 
    {

        int counter = 0;
        GameObject[] varients = spawnTable.getActorPrefabVariants();
        


        for (int setpeice = 0; setpeice < spawnTable.getSpawnPositionCount(); setpeice++)
        {

            yield return StartCoroutine(spawnSetPeice(spawnTable.GetSetpeiceSpawnPosition(setpeice), varients, setpeice));

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

            yield return StartCoroutine(spawnSetPeice(toBePlaced, varients,chosenIndex));

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
                yield return StartCoroutine(randomPlaceSetpeiceSet(spawnTable));
            }
            else
            {
                yield return StartCoroutine(placeSetpeiceSet(spawnTable));
            }
            yield return null;
            
        }
        

        this.mapSetupStage++;
        
    }
    

    private bool checkSpawnPositionSafety(GridIllegalSpawnZone spawnZone, int x, int y)
    {
        if (spawnZone == null)
        {
            throw new System.Exception("spawn position safety check error! spawnzone is null!");
        }

        for(int testx=x+spawnZone.getTopLeftCorner().getX(); testx<=spawnZone.getBottomRightCorner().getX()+x; testx++)
        {
            for(int testy=y+spawnZone.getTopLeftCorner().getY(); testy<=spawnZone.getBottomRightCorner().getY()+y; testy++)
            {
                if (testx<0 || testy<0 || testy > (mapGrid.Length-1)|| testx > (mapGrid[testy].Length - 1))
                {
                    return false;
                }
                if (mapGrid[testy][testx])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private IEnumerator spawnCollectableItem(GameObject Item)
    {
        Collectable_Item_Script itemController=Item.GetComponent<Collectable_Item_Script>();
        if(itemController == null)
        {
            throw new System.Exception("collectable item spawn error, collectable item has no script!");
        }
        GridVector2 prospectiveSpawnPostion=null;
        bool success = false;
        while (!success)
        {
            prospectiveSpawnPostion = new GridVector2(Random.Range(0, mapData.getGridSize().getX() - 1), Random.Range(0, mapData.getGridSize().getY() - 1));

            bool allPass = true;
            for(int zone=0; zone< itemController.getGridSize().Length; zone++)
            {
                if (!checkSpawnPositionSafety(itemController.getGridSize()[zone], prospectiveSpawnPostion.getX(), prospectiveSpawnPostion.getY())){
                    allPass = false;
                }
                yield return null;
            }
            if (allPass)
            {

                success = true;

            }
            yield return null;
        }
        if (success)
        {
            GridVector2 spawnPosition = prospectiveSpawnPostion;
            Vector2 globalPosition = calculateGridGlobalPosition(spawnPosition.getX(), spawnPosition.getY());

            //convert the global position vector 2 to a vector 3
            Vector3 spawnpos = new Vector3(globalPosition.x, globalPosition.y, 0f);



            //spawn the set peice
            GameObject newPeice = Instantiate(Item, spawnpos, new Quaternion(0f, 0f, 0f, 0f));

            Items.Add(newPeice);
            Collectable_Item_Script newPeiceController = newPeice.GetComponent<Collectable_Item_Script>();

            if (newPeiceController == null)
            {
                Debug.Log(Item.name);

                throw new System.Exception("newPeice has no controller");
            }
            

            newPeiceController.setGridPosition(mapData.getGridZeroPoint(), spawnPosition.getX(), spawnPosition.getY());
            for(int zone=0; zone<newPeiceController.getGridSize().Length; zone++)
            {
                yield return StartCoroutine(addTakenSpaceToMap(newPeiceController.getGridSize()[zone]));
            }
            
            newPeiceController.setElelment(currentItem);
            currentItem++;
        }
        
    }

    private IEnumerator spawnCollectableItemSet(int count, GameObject type)
    {
        for(int item=0; item<count; item++)
        {
            yield return StartCoroutine(spawnCollectableItem(type));
            
        }
    }

    private IEnumerator initializeCollectableItems()
    {
        if (mapData.getItemSpawnTable() == null)
        {
            throw new System.Exception("item spawn table error, the table is null!");
        }
        for(int item = 0; item<mapData.getItemSpawnTableLength(); item++)
        {
            
            ItemSpawnEntry entry=mapData.getItemSpawnEntry(item);
            if (entry.getIsSpawnCountRandomized())
            {
                yield return StartCoroutine(spawnCollectableItemSet(Random.Range(entry.getSpawnCountMin(),entry.getSpawnCountMax()),entry.getItemType()));
            }
            else
            {
                yield return StartCoroutine(spawnCollectableItemSet(entry.getSpawnCountMax(), entry.getItemType()));
            }

        }
        


        yield return null;
    }

    private IEnumerator spawnEnemy(GameObject Item)
    {
        Enemy_Script enemyController = Item.GetComponent<Enemy_Script>();
        if (enemyController == null)
        {
            throw new System.Exception("collectable item spawn error, collectable item has no script!");
        }
        GridVector2 prospectiveSpawnPostion = null;
        bool success = false;
        while (!success)
        {
            prospectiveSpawnPostion = new GridVector2(Random.Range(0, mapData.getGridSize().getX() - 1), Random.Range(0, mapData.getGridSize().getY() - 1));

            bool allPass = true;
            for (int zone = 0; zone < enemyController.getGridSize().Length; zone++)
            {
                if (!checkSpawnPositionSafety(enemyController.getGridSize()[zone], prospectiveSpawnPostion.getX(), prospectiveSpawnPostion.getY()))
                {
                    allPass = false;
                }
                yield return null;
            }
            if (allPass)
            {

                success = true;

            }
            yield return null;
        }
        if (success)
        {
            GridVector2 spawnPosition = prospectiveSpawnPostion;
            Vector2 globalPosition = calculateGridGlobalPosition(spawnPosition.getX(), spawnPosition.getY());

            //convert the global position vector 2 to a vector 3
            Vector3 spawnpos = new Vector3(globalPosition.x, globalPosition.y, 0f);



            //spawn the set peice
            GameObject newPeice = Instantiate(Item, spawnpos, new Quaternion(0f, 0f, 0f, 0f));

            

            Items.Add(newPeice);
            Enemy_Script newPeiceController = newPeice.GetComponent<Enemy_Script>();

            if (newPeiceController == null)
            {
                Debug.Log(Item.name);

                throw new System.Exception("newPeice has no controller");
            }


            
            


            newPeiceController.setGridPosition(mapData.getGridZeroPoint(), spawnPosition.getX(), spawnPosition.getY());
            yield return StartCoroutine(addTakenSpaceToMap(newPeiceController.getGridSize(), spawnPosition));
            newPeiceController.setElelment(currentEnemy);
            currentEnemy++;
        }

    }

    private IEnumerator spawnEnemySet(int count, GameObject type)
    {
        for (int enemy = 0; enemy < count; enemy++)
        {
            yield return StartCoroutine(spawnEnemy(type));

        }
    }

    private IEnumerator initializeEnemies()
    {
        if (mapData.getEnemySpawnTable() == null)
        {
            throw new System.Exception("item spawn table error, the table is null!");
        }
        for (int enemy = 0; enemy < mapData.getEnemySpawnTableLength(); enemy++)
        {

            EnemySpawnEntry entry = mapData.getEnemySpawnEntry(enemy);
            if (entry.getIsSpawnCountRandomized())
            {
                yield return StartCoroutine(spawnEnemySet(Random.Range(entry.getSpawnCountMin(), entry.getSpawnCountMax()), entry.getEnemyType()));
            }
            else
            {
                yield return StartCoroutine(spawnEnemySet(entry.getSpawnCountMax(), entry.getEnemyType()));
            }

        }



        yield return null;
    }

    private IEnumerator initializeMapActors()
    {
        //when adding enemies and items, put them here
        //yield return null;
        yield return StartCoroutine(initializeCollectableItems());
        yield return StartCoroutine(initializeEnemies());
    }



    private IEnumerator initializeMapNoSpawnZones()
    {
        if (!(mapData.getOutOfMapGridAreas() == null || mapData.getOutOfMapGridAreaListLength() <= 0)) { 
            for (int zone = 0; zone < mapData.getOutOfMapGridAreaListLength(); zone++)
            {

                yield return StartCoroutine(addTakenSpaceToMap(mapData.getOutOfMapGridArea(zone)));
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
        yield return StartCoroutine(initializeMapNoSpawnZones());
        yield return null;
        //set up the dynamic parts of the map
        yield return StartCoroutine(initializeMapSetpeices());
        yield return null;
        //yield return StartCoroutine(initializeMapActors());
        yield return null;
        //setup done, go to confirm screen
        mapSetupStage++;
        
    }




    private void debugOpps()
    {
       debugNoSpawnZoneMaskGenerator();
       
        mapSetupStage++;
    }


    private bool mapSetupDone = false;

    private void mapSetupStageManager()
    {
        
        switch (mapSetupStage)
        {
            case 4:
                if (showTakenTiles)
                {
                    debugOpps();
                    mapSetupStage++;
                }
                else
                {
                    mapSetupStage += 2;
                }
                
                break;
            case 6:
                mapSetupDone = true;
                mapSetupStage++;
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

        //coroutines will not wait for each other unless you chain yeild returns
        
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        mapSetupStageManager();
        
    }
}
