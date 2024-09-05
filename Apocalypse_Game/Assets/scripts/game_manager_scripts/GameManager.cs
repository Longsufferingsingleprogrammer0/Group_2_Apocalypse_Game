using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    //singleton variable 
    private static GameManager instance;


    //ui code
    public TextMeshProUGUI collectableText;
    private int itemsCollected;


    //getter for the instance, needed for singletons
    public static GameManager Instance => instance;
    


    //is the level running, used to prevent anything form happening durring loading and level init
    private bool levelRunning = false;
    //problably won't need and will be deleted later, use to keep track of if we are starting up.
    private bool settingUpLevel;


    
    




    void Awake()
    {
        //startup code for singletons
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //starting code goes here?


    }

    [ContextMenu("resetCollectedItemsValue")]
    public void resetCollectedItems()
    {
        itemsCollected = 0;
        collectableText.SetText(itemsCollected.ToString());
    }

    
    private void setCollectedItems(int value)
    {
        itemsCollected = value;
        collectableText.SetText(itemsCollected.ToString());
    }

    public void incrimentCollectedItems()
    {
        itemsCollected++;
        collectableText.SetText(itemsCollected.ToString());
    }

    public bool isLevelRunning() 
    {
        return levelRunning; 
    }

    public bool isSettingUpLevel()
    {
        return settingUpLevel;
    }

    private void startLevelRunning()
    {
        levelRunning = true;
    }

    private void finishedSettingUp()
    {
        settingUpLevel = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        levelRunning = false;
        resetCollectedItems();
        //settingUpLevel=true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
