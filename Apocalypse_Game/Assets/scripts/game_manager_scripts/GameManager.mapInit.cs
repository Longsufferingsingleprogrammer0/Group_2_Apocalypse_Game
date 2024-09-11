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
    [SerializeField] private float loadingScreenX;
    [SerializeField] private float loadingScreenY;

    [SerializeField] private string playerTag;

    private GameObject playerSprite;





    private IEnumerator initializeMap()
    {
        
        

        mapSetupDone = 1;
        return null;
    }



    // Start is called before the first frame update
    void MapInitStart()
    {
        playerSprite = GameObject.FindWithTag(playerTag);
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        
    }
}
