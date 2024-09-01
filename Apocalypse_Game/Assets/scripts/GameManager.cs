using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public TextMeshProUGUI collectableText;
    private int itemsCollected;

    //getter for the instance
    public static GameManager Instance => instance;

    void Awake()
    {
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

    

    // Start is called before the first frame update
    void Start()
    {
        resetCollectedItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
