using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{

    //singleton variable 
    private static LevelManager instance;


    //getter for the instance, needed for singletons
    public static LevelManager Instance => instance;


    void Awake()
    {
        //startup code for singletons
        if (instance == null)
        {
            instance = this;
            //do this if you want it immortal
            //DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //starting code goes here?


    }


    void quitter()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void startAllFiles()
    {
        UIStart();
        MapInitStart();
    }


    void updateAllFiles()
    {
        quitter();
        UIUpdate();
        MapInitUpdate();
    }


    // Start is called before the first frame update
    void Start()
    {
        startAllFiles();
        
    }

    // Update is called once per frame
    void Update()
    {
        updateAllFiles();
        
    }
}
