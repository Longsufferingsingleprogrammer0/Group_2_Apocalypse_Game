using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    private int day;

    [SerializeField] private float startingHealth;
    private float health=1;
    private bool invincible;

    [SerializeField] private float InvincibilityTime;
    private float elapsed;
    private bool injured;

    
    private LevelManager levelManagerScriptReference;


    private int food;
    private int water;

    [SerializeField] private int startingFood;
    [SerializeField] private int startingWater;

    private bool gameplayMode=false;

    public void setGameplayMode(bool mode)
    {
        gameplayMode = mode;
    }

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

    
    public int getDay()
    {
        return day;
    }

    public int getScore()
    {
        return score;
    }

    public float getHealth()
    {
        return health;
    }

    public void setDay(int Newday)
    {
        day = Newday;
        if (gameplayMode)
        {
            levelManagerScriptReference.updateDay(day);
        }
    }


    public void damagePlayer(float hp)
    {
        if (!invincible)
        {
            health -= hp;
            injured = true;

            if (gameplayMode)
            {
                levelManagerScriptReference.playPlayerHitSound();
                if(health <= 0)
                {
                   
                    levelManagerScriptReference.updateHealth(0);
                }
                else
                {
                    levelManagerScriptReference.updateHealth(health);
                }
                

            }

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

    public void updateScore(int value)
    {
        score += value;
 
        if (gameplayMode)
        {
            levelManagerScriptReference.updateScore(score);
        }
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
        day = 1;
        food = 0;
        water = 0;
        health = startingHealth;
        invincible = false;
        injured = false;
        elapsed = 0f;
        score = 0;
        gameplayMode = false;
        singleSet = true;
    }

    // Start is called before the first frame update
    private void StartGlobalGameLogic()
    {
        day = 1;
        resetGlobalGameLogicVariables();
        if (gameplayMode)
        {

            levelManagerScriptReference.updateDay(day);
            levelManagerScriptReference.updateHealth(health);
            levelManagerScriptReference.updateScore(score);
        }
    }


    
    
    private void fixedUpdateGlobalGameLogic()
    {
        if (gameplayMode)
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
        
    }


    private bool  singleSet=true;
    // Update is called once per frame
    private void UpdateGlobalGameLogic()
    {
        if (gameplayMode && (health <= 0))
        {
            if (singleSet)
            {
                levelManagerScriptReference.externalSetUiMode(7);
                levelManagerScriptReference.updateHealth(0);
                singleSet = false;
            }
            
        }
    }
}
