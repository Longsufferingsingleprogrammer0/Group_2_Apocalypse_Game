using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{


    private int mapSetupDone = 0;

    [SerializeField] private LevelSpawnData mapData;









    private IEnumerator initializeMap()
    {
        
        

        mapSetupDone = 1;
        return null;
    }



    // Start is called before the first frame update
    void MapInitStart()
    {
        
    }

    // Update is called once per frame
    void MapInitUpdate()
    {
        
    }
}
