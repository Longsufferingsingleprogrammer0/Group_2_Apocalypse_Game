using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{

    private int health;
    private bool invincible;

    [SerializeField] private float InvincibilityTime;
    private float elasped;


    private IEnumerator temporaryInvinicibilty()
    {
        invincible = true;

        while (elasped < InvincibilityTime)
        {
            elasped += Time.deltaTime;
            yield return null;
        }
        elasped = 0f;
        invincible = false;
    }



    // Start is called before the first frame update
    private void StartGameLogic()
    {
        
    }

    // Update is called once per frame
    private void UpdateGameLogic()
    {
        
    }
}
