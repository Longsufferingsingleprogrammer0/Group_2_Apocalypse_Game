using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    //singleton variable 
    private static Game_Master instance;


    //getter for the instance, needed for singletons
    public static Game_Master Instance => instance;



    //this game was origonally planned for so much more, but due to time constraints, we had to cut most of it all that is left is a shell of its former self




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

        awakeAllFiles();
    }


    private void FixedUpdateAllFiles()
    {
        fixedUpdateGlobalGameLogic();
    }

    private void FixedUpdate()
    {
        FixedUpdateAllFiles();
    }


    private void awakeAllFiles()
    {

    }


    public void quitGame()
    {
        Application.Quit();
    }


    private void startAllFiles()
    {
        
        StartGlobalGameLogic();
        startScoreKeeping();
    }


    private void updateAllFiles()
    {

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
