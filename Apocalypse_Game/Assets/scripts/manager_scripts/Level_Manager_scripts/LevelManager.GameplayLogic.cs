using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{

    private int gameplayEnabled=0;
    [SerializeField] private bool gameplayEnabledAtStart;

    [SerializeField] private float startingHealth;
    private float health;
    private bool invincible;

    [SerializeField] private float InvincibilityTime;
    private float elapsed;
    private bool injured;





    public int getGameplayEnabled()
    {
        return gameplayEnabled;
    }

    private IEnumerator temporaryInvinicibilty()
    {
        invincible = true;

        while (elapsed < InvincibilityTime)
        {
            switch (gameplayEnabled)
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




    // Start is called before the first frame update
    private void GamePlayLogicStart()
    {
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
    private void GameplayLogicUpdate()
    {
        switch (gameplayEnabled)
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
