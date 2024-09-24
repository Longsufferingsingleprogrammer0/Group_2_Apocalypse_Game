using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioFaderScript : MonoBehaviour
{
    #region timerVariables
    //how long we have waited
    private float elsapsed;

    private float timer;
    #endregion

    //if the user skips the volume lerp
    private bool skip;

    //our mode
    private int mode;

    //just premaking a variable for later
    private float newVolume;

   

    private AudioSource audioPlayer;

    #region controlVariables

    private float fadeInTimeSeconds;


    private float fadeOutTimeSeconds;
    #endregion

    private bool finishedTransition;

    [SerializeField] private bool startingState;

    

    public void setVolume(float volume)
    {
        audioPlayer.volume = volume;
    }

    public void startPlayback()
    {
        audioPlayer.Play();
    }

    public void stopPlayback() 
    { 
        audioPlayer.Stop();
    }

    public void setLooping(bool looping)
    {
        audioPlayer.loop=looping;
    }



    private void resetVariables()
    {
        newVolume = 0f;
        mode = 0;
        finishedTransition = true;
        elsapsed = 0;
        timer = 0;
        skip = false;
    }

    public bool isFadeFinished()
    {
        return finishedTransition;
    }

    public void setStatefadedIn()
    {
        audioPlayer.volume = 1f;
        resetVariables();
    }

    public void setStateFadedOut()
    {
        audioPlayer.volume=0f;
        resetVariables();
    }



    // Start is called before the first frame update
    void Start()
    {

        audioPlayer = GetComponent<AudioSource>();
        resetVariables();
        if (startingState)
        {
            setStatefadedIn();
        }
        else
        {
            setStateFadedOut();
        }


    }

    public void skipTransition()
    {
        if (mode != 0)
        {
            skip = true;
        }

    }

    public void fadeIn(float duration)
    {
        resetVariables();
        timer = duration;
        mode = 1;
        finishedTransition = false;


    }

    public void fadeOut(float duration)
    {
        resetVariables();
        timer = duration;
        mode = 2;
        finishedTransition = false;
    }

    private void fadeInWorker()
    {
        elsapsed += Time.fixedDeltaTime;
        if (skip)
        {
            elsapsed = timer;
        }

        if (elsapsed >= timer)
        {
            //reset and next stage
            resetVariables();
            audioPlayer.volume = 1f;
        }
        else
        {
            newVolume = Mathf.Lerp(0f, 1f, elsapsed / timer);
            audioPlayer.volume = newVolume;
        }
    }

    private void fadeOutWorker()
    {
        elsapsed += Time.fixedDeltaTime;
        if (skip)
        {
            elsapsed = timer;
        }

        if (elsapsed >= timer)
        {
            //reset and next stage
            resetVariables();
            audioPlayer.volume = 0f;
        }
        else
        {
            newVolume = Mathf.Lerp(1f, 0f, elsapsed / timer);
            audioPlayer.volume = newVolume;
        }
    }

    public void fadeWorker()
    {

        switch (mode)
        {
            case 1:
                fadeInWorker();
                break;
            case 2:
                fadeOutWorker();
                break;
            default:
                throw new System.Exception("fade worker set to undefined mode of:" + mode.ToString() + " must be 0-2 inclusive");

        }
    }


    void FixedUpdate()
    {
        switch (mode)
        {
            case 0:

                break;
            default:
                fadeWorker();
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
