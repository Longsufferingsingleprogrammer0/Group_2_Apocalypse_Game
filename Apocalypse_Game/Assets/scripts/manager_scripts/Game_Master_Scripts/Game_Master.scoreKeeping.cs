using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    private int score;
    private int food;
    private int water;

    [SerializeField] private int startingFood;
    [SerializeField] private int startingWater;

    [SerializeField]private int foodMultiplyer;
    [SerializeField]private int waterMultiplyer;


    public void collectItem(int ammount, int id)
    {
        switch (id)
        {
            case 0:
                collectFood(ammount);
                break;
            case 1:
                collectWater(ammount);
                break;
            default:
                throw new System.Exception("invalid item id given to collected item. invalid id:"+id.ToString());
        }
    }

    private void collectFood(int collectedFood)
    {
        food += collectedFood;
        score += collectedFood * foodMultiplyer;
    }

    private void collectWater(int collectedWater)
    {
        water += collectedWater;
        score += collectedWater * waterMultiplyer;
    }


    // Start is called before the first frame update
    private void startScoreKeeping()
    {
        score = 0;
        food = startingFood;
        water = startingWater;
    }

    // Update is called once per frame
    private void UpdateScoreKeeping()
    {
        
    }
}
