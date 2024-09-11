using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Android;

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

            spawnSetPeice(toBePlaced,varients);

            if (counter >= maxSetpeicesSpawnedPerFrame)
            {
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
            if (spawnTable.isSpawningRandomized())
            {
                randomPlaceSetpeiceSet(spawnTable);
            }
            else
            {
                placeSetpeiceSet(spawnTable);
            }
            yield return null;
            
        }
        

        this.mapSetupStage++;
        
    }
    
    private IEnumerator initialzeMapActors()
    {
        //when adding enemies and items, put them here
        //yield return null;
        return null;
    }


    private IEnumerator initializeMap()
    {
        //set up the dynamic parts of the map
        initializeMapSetpeices();
        yield return null;
        initialzeMapActors();
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
        initializeMap();
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        mapSetupStageManager();
    }
}
