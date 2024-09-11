using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public partial class GameManager : MonoBehaviour
{

    //whether the map setup is done, an int to allow for switch case optomisation
    private int mapSetupStage = 0;

    //the spawndata object for this level
    [SerializeField] private LevelSpawnData mapData;

    //position of the loading screen
    [SerializeField] private Vector2 loadingScreenPos;
    [SerializeField] private Vector2 loadingDoneScreenPos;


    //temporary variable until we figure things out
    [SerializeField] private Vector2 playerStartPos;

    private GameObject playerSprite;

    [SerializeField] private string playerTag;

    //put the variables for spawning actors here

    [SerializeField] private int maxSetpeicesSpawnedPerFrame;


    private void spawnSetPeice(SetpeiceSpawnPosition spawnPosition, GameObject[] varientTable)
    {
        if((varientTable==null)||(varientTable.Length == 0))
        {
            throw new System.Exception("setpeice to spawn has an empty varient table");
        }

        Vector3 spawnpos = new Vector3(spawnPosition.getPosition().x, spawnPosition.getPosition().y,0f);
        Instantiate(varientTable[spawnPosition.getPrefabVariant()], spawnpos,new Quaternion(0f,0f,0f,0f));
        


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
    
    private IEnumerator initialzeMapActors()
    {
        //when adding enemies and items, put them here
        //yield return null;
        yield return null;
    }


    private IEnumerator initializeMap()
    {
        
        //set up the dynamic parts of the map
        StartCoroutine(initializeMapSetpeices());
        yield return null;
        StartCoroutine(initialzeMapActors());
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
            playerSprite.GetComponent<Rigidbody2D>().position = playerStartPos;
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
        playerSprite = GameObject.FindWithTag(playerTag);
        playerSprite.GetComponent<Rigidbody2D>().position = loadingScreenPos;
        StartCoroutine(initializeMap());
        
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        mapSetupStageManager();
        
    }
}
