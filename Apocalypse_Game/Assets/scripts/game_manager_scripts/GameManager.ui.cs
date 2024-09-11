using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public partial class GameManager : MonoBehaviour
{


    //ui code
    public TextMeshProUGUI collectableText;
    private int itemsCollected;





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
    void UIStart()
    {
        
        resetCollectedItems();
        
    }

    // Update is called once per frame
    void UIUpdate()
    {

    }
}
