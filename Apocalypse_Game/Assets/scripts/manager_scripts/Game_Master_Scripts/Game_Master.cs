using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    //singleton variable 
    private static Game_Master instance;


    //getter for the instance, needed for singletons
    public static Game_Master Instance => instance;


    void Awake()
    {
        //startup code for singletons
        if (instance == null)
        {
            instance = this;
            //do this if you want it immortal
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //starting code goes here?


    }


    public void quitGame()
    {
        Application.Quit();
    }


    private void quitter()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    private void startAllFiles()
    {
        
        StartGlobalGameLogic();
        startScoreKeeping();
    }


    private void updateAllFiles()
    {
        quitter();
        UpdateGlobalGameLogic();
        UpdateScoreKeeping();
        
    }

    public void resetAllVariables()
    {
        resetScoreKeepingVariables();
        resetGlobalGameLogicVariables();
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
