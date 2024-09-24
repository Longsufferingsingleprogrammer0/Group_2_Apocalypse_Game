
using UnityEngine;
using UnityEngine.SceneManagement;

public class logo_script : MonoBehaviour
{
    // Start is called before the first frame update

    #region timerVariables
    //how long we have waited
    private float elsapsed;
    //how long to wait
    private float timer;
    #endregion

    //if the user skips the animation
    private bool skip;

    //what stage of the animation we are on
    private int stage;

    //just premaking a variable for later
    private float newAlpha;

    //sprite renderer and logo sprite
    private GameObject logo;
    private SpriteRenderer logoRenderer;

    #region controlVariables
    [SerializeField] private string whatActorTagToFade;
    [SerializeField] private float fadeInTimeSeconds;
    [SerializeField] private float holdTimeSeconds;
    [SerializeField] private float fadeOutTimeSeconds;
    [SerializeField] private string SceneToSwitchTo;
    #endregion


    void Start()
    {
        //initialize our variables and start the fade in process
        timer = fadeInTimeSeconds;
        elsapsed = 0f;
        stage = 0;
        skip= true;
        newAlpha = 0f;


        //get the logo
        logo = GameObject.FindWithTag(whatActorTagToFade);
        if( logo == null)
        {
            Debug.Log("null logo");
        }
        
        logoRenderer = logo.GetComponent<SpriteRenderer>();
        if( logoRenderer == null)
        {
            Debug.Log("null renderer");
        }
        logoRenderer.color = new Color(logoRenderer.color.r, logoRenderer.color.g, logoRenderer.color.b, 0f);
    }



    private void FixedUpdate()
    {
        //keep track of the time
        elsapsed += Time.fixedDeltaTime;

        //if we hit our timer
        if (elsapsed >= timer)
        {
            //reset and next stage
            elsapsed -= timer;

            stage++;
            switch (stage)
            {
                case 1:
                    //just finished fade in, switch to wait
                    logoRenderer.color = new Color(logoRenderer.color.r, logoRenderer.color.g, logoRenderer.color.b, 1f);
                    timer = holdTimeSeconds; 
                    break;

                case 2:
                    //just finished wait, switch to fade out
                    skip = true;
                    timer = fadeOutTimeSeconds;
                    break;

                default:
                    //change to next scene
                    logoRenderer.color = new Color(logoRenderer.color.r, logoRenderer.color.g, logoRenderer.color.b, 0f);
                    SceneManager.LoadScene(SceneToSwitchTo);
                    break;
            }
            
        }
        else
        {
            //if no stage transiton, perform the stage animation
            switch (stage)
            {
                case 0:
                    //fade in
                    //update the logorenderer to a new alpha based on the current period
                    newAlpha = Mathf.Lerp(0f, 1f, elsapsed / timer);
                    logoRenderer.color = new Color(logoRenderer.color.r, logoRenderer.color.g, logoRenderer.color.b, newAlpha);
                    break;

                case 1:
                    break;

                case 2:
                    newAlpha = Mathf.Lerp(1f, 0f, elsapsed / timer);
                    logoRenderer.color = new Color(logoRenderer.color.r, logoRenderer.color.g, logoRenderer.color.b, newAlpha);
                    break;

                default:
                    Debug.Log("unintended switch state error");
                    break;

            }


            
        }

        //allow the user to skip directly to fade out
        if (skip && Input.anyKeyDown)
        {
            skip = false;
            
            logoRenderer.color = new Color(logoRenderer.color.r, logoRenderer.color.g, logoRenderer.color.b, 1f);
            stage = 2;
            elsapsed = 0f;
            timer = fadeOutTimeSeconds;
        }


    }
}
