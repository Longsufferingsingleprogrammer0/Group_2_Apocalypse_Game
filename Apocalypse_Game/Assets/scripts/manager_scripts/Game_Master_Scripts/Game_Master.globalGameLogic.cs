using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    private int day;

    [SerializeField] private float startingHealth;
    private float health;
    private bool invincible;

    [SerializeField] private float InvincibilityTime;
    private float elapsed;
    private bool injured;

    
    private LevelManager levelManagerScriptReference;


    private int food;
    private int water;

    [SerializeField] private int startingFood;
    [SerializeField] private int startingWater;



    public void giveLevelManagerReference(LevelManager reference)
    {
        levelManagerScriptReference= reference;

    }


    private IEnumerator temporaryInvinicibilty()
    {
        invincible = true;
       
        while (elapsed < InvincibilityTime)
        {
            switch (levelManagerScriptReference.getGameplayEnabled())
            {
                case 1:
                    elapsed += Time.deltaTime;
                    yield return null;
                    break;

                default:
                    yield return null;
                    break;
            }



        }
        elapsed = 0f;
        invincible = false;
      
    }

    public void damagePlayer(float hp)
    {
        if (!invincible)
        {
            health -= hp;
            injured = true;
           
        }
    }


    public void enemyKilled(int value)
    {
        updateScore(value);
    }


    private void collectFood(int collectedFood)
    {
        food += collectedFood;
        updateScore(collectedFood);
    }

    private void collectWater(int collectedWater)
    {
        water += collectedWater;
        updateScore(collectedWater);
    }

    private void updateScore(int value)
    {
        score += value;
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
        day = 0;
        food = 0;
        water = 0;
        health = startingHealth;
        invincible = false;
        injured = false;
        elapsed = 0f;
    }

    // Start is called before the first frame update
    private void StartGlobalGameLogic()
    {
        day = 0;
        resetGlobalGameLogicVariables();
    }


    private void fixedUpdateGlobalGameLogic()
    {
        switch (levelManagerScriptReference.getGameplayEnabled())
        {
            case 1:
                if (injured)
                {
                    injured = false;
                    StartCoroutine(temporaryInvinicibilty());
                }
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    private void UpdateGlobalGameLogic()
    {
        
    }
}
