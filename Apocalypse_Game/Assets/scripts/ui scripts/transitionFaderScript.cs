

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

    [SerializeField] private bool startingState;


    private void resetVariables()
    {
        newAlpha = 0f;
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
        faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 0f);
        resetVariables();
    }

    public void setStateFadedOut()
    {
        faderRenderer.color = new Color(faderRenderer.color.r, faderRenderer.color.g, faderRenderer.color.b, 1f);
        resetVariables();
    }



    // Start is called before the first frame update
    void Start()
    {
        
        faderRenderer = GetComponent<Image>();
        resetVariables();
        if(startingState)
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
