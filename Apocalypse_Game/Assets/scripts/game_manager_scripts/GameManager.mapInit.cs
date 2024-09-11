using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{

    //whether the map setup is done, an int to allow for switch case optomisation
    private int mapSetupDone = 0;

    //the spawndata object for this level
    [SerializeField] private LevelSpawnData mapData;

    //position of the loading screen
    [SerializeField] private Vector2 loadingScreenPos;
    [SerializeField] private Vector2 loadingDoneScreenPos;

    [SerializeField] private string playerSpriteTag;

    //temporary variable until we figure things out
    [SerializeField] private Vector2 playerStartPos;

    private GameObject playerSprite;

    [SerializeField] private string playerTag;


    private void spawnSetPeice(SetpeiceSpawnPosition spawnPosition, GameObject[] varientTable)
    {

    }

    private IEnumerator placeSetpeiceSet(SetpeiceObjectSpawnTable spawnTable) 
    {

        return null;
    }


    private IEnumerator randomPlaceSetpeiceSet(SetpeiceObjectSpawnTable spawnTable)
    {

        return null;
    }

    
    private IEnumerator initializeMapSetpeices()
    {
        for(int setPeiceType=0; setPeiceType<mapData.getSpawnTableArrayLength(); setPeiceType++)
        {

        }
        

        mapSetupDone = 1;
        return null;
    }
    
    private IEnumerator initialzeMapActors()
    {
        return null;
    }


    private IEnumerator initializeMap()
    {

        return null;
    }

    // Start is called before the first frame update
    void MapInitStart()
    {
        playerSprite = GameObject.FindWithTag(playerTag);
        playerSprite.GetComponent<Rigidbody2D>().position = loadingScreenPos;
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        
    }
}
