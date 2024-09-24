

using UnityEngine;
using UnityEngine.UI;



public class transitionFaderScript : MonoBehaviour
{
    #region timerVariables
    //how long we have waited
    private float elsapsed;

    private float timer;
    #endregion

    //if the user skips the animation
    private bool skip;

    //what stage of the animation we are on
    private int mode;

    //just premaking a variable for later
    private float newAlpha;

    //sprite renderer and logo sprite
   
    private Image faderRenderer;

    #region controlVariables
    
    private float fadeInTimeSeconds;
    

    private float fadeOutTimeSeconds;
    #endregion

    private bool finishedTransition;
    private bool fadeTransitionPause;
    private int transitionStage;
    [SerializeField] private bool startingState;

    private float fadeIntime;
    private float pauseTime;
    private float fadeOutTime;


    private void resetVariables()
    {
        newAlpha = 0f;
        mode = 0;
        finishedTransition = true;
        elsapsed = 0;
        timer = 0;
        skip = false;
        fadeTransitionPause = false;
        transitionStage = 0;
        fadeIntime = 0;
        pauseTime = 0;
        fadeOutTime = 0;
    }

    public bool isFadeTransitionInPause()
    {
        return fadeTransitionPause;
    }
    public bool isFadeFinished()
    {
        return finishedTransition;
    }

    public void setStateFadedIn()
    {
        faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 0f);
        resetVariables();
    }

    public void setStateFadedOut()
    {
        faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 1f);
        resetVariables();
    }

    public void fadeTransition(float fadeOutDurration, float pauseDurration, float fadeInDurration)
    {
        setStateFadedIn();
        resetVariables();
        finishedTransition = false;
        fadeTransitionPause = false;
        fadeIntime = fadeInDurration;
        pauseTime = pauseDurration;
        fadeOutTime = fadeOutDurration;
        timer=fadeOutTime;
        mode = 3;
    }



    private void fadeTransitionWorker()
    {

        switch (transitionStage)
        {
            case 0:
                elsapsed += Time.fixedDeltaTime;
                if (skip)
                {
                    elsapsed = timer;
                }

                if (elsapsed >= timer)
                {
                    //reset and next stage
                    timer = pauseTime;
                    faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 1f);
                    elsapsed = 0f;
                    transitionStage++;
                    fadeTransitionPause=true;
                }
                else
                {
                    newAlpha = Mathf.Lerp(0f, 1f, elsapsed / timer);
                    faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, newAlpha);
                }
                break;


            case 1:
                elsapsed += Time.fixedDeltaTime;
                if (skip)
                {
                    elsapsed = timer;
                }

                if (elsapsed >= timer)
                {
                    //reset and next stage
                    timer = fadeIntime;
                    fadeTransitionPause = false;
                    transitionStage++;
                    elsapsed = 0f;
                }
                
                break;

            case 2:
                elsapsed += Time.fixedDeltaTime;
                if (skip)
                {
                    elsapsed = timer;
                }

                if (elsapsed >= timer)
                {
                    //reset and next stage
                    
                    faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 0f);
                    transitionStage++;
                    elsapsed = 0f;
                    resetVariables();
                }
                else
                {
                    newAlpha = Mathf.Lerp(1f, 0f, elsapsed / timer);
                    faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, newAlpha);
                }
                break;
        }   
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        faderRenderer = GetComponent<Image>();
        resetVariables();
        if(startingState)
        {
            setStateFadedIn();
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
        setStateFadedOut();
        resetVariables();
        timer = duration;
        finishedTransition = false;
        mode = 1;
        
        
        

    }

    public void fadeOut(float duration)
    {
        setStateFadedIn();
        resetVariables();
        timer = duration;
        finishedTransition = false;
        mode = 2;
        
        
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
            faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 0f);
        }
        else
        {
            newAlpha = Mathf.Lerp(1f, 0f, elsapsed / timer);
            faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, newAlpha);
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
            faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 1f);
        }
        else
        {
            newAlpha = Mathf.Lerp(0f, 1f, elsapsed / timer);
            faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, newAlpha);
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
            case 3:
                fadeTransitionWorker();
                break;
            default:
                throw new System.Exception("fade worker set to undefined mode of:"+mode.ToString()+" must be 0-2 inclusive");

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
