using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{


    [SerializeField] private float startingHealth;
    private float health;
    private bool invincible;

    [SerializeField] private float InvincibilityTime;
    private float elapsed;
    private bool injured;



    private IEnumerator temporaryInvinicibilty()
    {
        invincible = true;

        while (elapsed < InvincibilityTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
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
        
    }

    // Update is called once per frame
    private void GameplayLogicUpdate()
    {
        if (injured)
        {
            injured = false;
            StartCoroutine(temporaryInvinicibilty());
        }
    }
}
