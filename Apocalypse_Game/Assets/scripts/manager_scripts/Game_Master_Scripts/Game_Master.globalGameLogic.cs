using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    







    private int food;
    private int water;

    [SerializeField] private int startingFood;
    [SerializeField] private int startingWater;




    



    private void collectFood(int collectedFood)
    {
        food += collectedFood;
        score += collectedFood;
    }

    private void collectWater(int collectedWater)
    {
        water += collectedWater;
        score += collectedWater;
    }




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
                throw new System.Exception("invalid item id given to collected item. invalid id:" + id.ToString());
        }
    }

    private void resetGlobalGameLogicVariables()
    {

        food = 0;
        water = 0;
    }

    // Start is called before the first frame update
    private void StartGlobalGameLogic()
    {
        
        resetGlobalGameLogicVariables();
    }

    // Update is called once per frame
    private void UpdateGlobalGameLogic()
    {
        
    }
}
