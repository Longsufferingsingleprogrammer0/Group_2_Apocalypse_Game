using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public partial class Game_Master : MonoBehaviour
{
    

    [SerializeField]private float health;
    private bool invincible;

    [SerializeField] private float InvincibilityTime;
    private float elasped;
    private bool injured;

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

    public void damagePlayer(float hp)
    {
        if (!invincible)
        {
            health -= hp;
            injured = true;
        }
    }

    // Start is called before the first frame update
    private void StartGameLogic()
    {
        invincible=false;
        injured=false;
    }

    // Update is called once per frame
    private void UpdateGameLogic()
    {
        if (injured)
        {
            injured = false;
            StartCoroutine(temporaryInvinicibilty());
        }
    }
}
