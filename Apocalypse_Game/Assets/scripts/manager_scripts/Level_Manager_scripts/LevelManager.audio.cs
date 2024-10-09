using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource menuSound;
    [SerializeField] AudioSource enemyHit;
    [SerializeField] AudioSource playerHit;
    [SerializeField] AudioSource CollectItem;
    private void audioSourcePresenceCheck()
    {
        if (musicSource == null)
        {
            throw new System.Exception("music source not set!");
        }
    }

    private void startMusic()
    {
        
        musicSource.Play();

    }

    private void startManagerAudio()
    {
        audioSourcePresenceCheck();
    }

    private void audioManagerUpdate()
    {

    }

    public void playEnemyHitSound()
    {
        enemyHit.Play();
    }

    public void playPlayerHitSound()
    {
        playerHit.Play();

    }


    public void playCollectItemSound()
    {
      
        CollectItem.Play();
    }

    

}
