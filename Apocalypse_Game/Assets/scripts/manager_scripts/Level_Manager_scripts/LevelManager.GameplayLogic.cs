using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{

    private int gameplayEnabled=0;
    [SerializeField] private bool gameplayEnabledAtStart;

    


    public void pauseGamePlayLogic()
    {
        gameplayEnabled = 0;
        for(int enemy=0; enemy<Enemies.Count; enemy++)
        {
            Enemies[enemy].GetComponent<Enemy_Script>().enabled = false;
        }
        GameObject.FindWithTag(playerTag).GetComponent<Player>().enabled=false;

    }

    public void resumeGamePlayLogic()
    {
        GameObject.FindWithTag(playerTag).GetComponent<Player>().enabled = true;
        for (int enemy = 0; enemy < Enemies.Count; enemy++)
        {
            Enemies[enemy].GetComponent<Enemy_Script>().enabled = true;
        }
        
        gameplayEnabled = 1;
    }


    private void gamePlayLogicAwake()
    {
        GameObject mastertemp = GameObject.FindGameObjectWithTag("game_master");
        mastertemp.GetComponent<Game_Master>().giveLevelManagerReference(this);
        mastertemp.GetComponent<Game_Master>().setGameplayMode(true);
    }

    public int getGameplayEnabled()
    {
        return gameplayEnabled;
    }


    // Start is called before the first frame update
    private void GamePlayLogicStart()
    {
        GameObject.FindWithTag("game_master").GetComponent<Game_Master>().setGameplayMode(true);
        if (gameplayEnabledAtStart)
        {
            gameplayEnabled = 1;

        }
        else
        {
            gameplayEnabled = 0;
        }
    }

    // Update is called once per frame
    private bool testWinTemp = false;
    private void GameplayLogicUpdate()
    {
        if (uimode == 2)
        {
            if (Enemies.Count <= 0 && (!testWinTemp))
            {
                Debug.Log("you win");
                testWinTemp = true;
            }
        }
        
    }
}
