using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Loading;

public partial class LevelManager : MonoBehaviour
{
    private int uimode;
    private int uiStage;
    [SerializeField] private GameObject fader;
    private transitionFaderScript faderController;
    [SerializeField] private float loadingFadeTime;

    //ui code

    

    private void setUIStage(int uiStage)
    {
        this.uiStage = uiStage;
    }

    private void setUiMode(int uiMode)
    {
        this.uiStage = uiMode;
    }



    private void uiLoadingMode()
    {
        switch (uiStage)
        {
            case 0:
                faderController.fadeIn(loadingFadeTime);
                uiStage++;
                
                break;
            case 1:
                if (faderController.isFadeFinished())
                {
                    uiStage++;
                    
                }
                break;
            case 2:
                if (mapSetupDone)
                {
                    uiStage++;
                    
                    faderController.fadeOut(loadingFadeTime/2);
                    
                }
                break;
            case 3:
                if (faderController.isFadeFinished())
                {
                    uiStage++;
                    
                    playerSprite.GetComponent<Rigidbody2D>().position = loadingDoneScreenPos;
                    
                    faderController.fadeIn(loadingFadeTime/2);
                }
                break;
            case 4:
                if (faderController.isFadeFinished())
                {
                    uiStage++;
                }
                break;
            case 5:
                if(Input.anyKeyDown)
                {
                    menuSound.Play();
                    uiStage++;
                }
                break;
            case 6:
                faderController.fadeTransition(loadingFadeTime,0.07f,loadingFadeTime);
                uiStage++;
                break;
            case 7:
                if (faderController.isFadeTransitionInPause())
                {
                    uiStage++;
                    playerSprite.GetComponent<Rigidbody2D>().position = calculateGridGlobalPosition(mapData.getPlayerStartPos().getX(), mapData.getPlayerStartPos().getY());
                    playerSprite.GetComponent<SpriteRenderer>().enabled = true;
                    startMusic();
                    musicSource.GetComponent<audioFaderScript>().fadeIn(loadingFadeTime);
                }
                break;
            case 8:
                playerSprite.GetComponent<Player>().setPlayerMovementEnabled(true);
                mapSetupStage = 0;
                uimode++;
                break;


        }
    }

    private void uiGameplayMode()
    {
        //placeholder
    }

    // Start is called before the first frame update
    void UIStart()
    {
        uiStage = 0;
        uimode = 0;
        faderController = fader.GetComponent<transitionFaderScript>();
    }



    // Update is called once per frame
    void UIUpdate()
    {
        switch (uimode)
        {
            case 0:
                uiLoadingMode();
                break;
            case 1:
                uiGameplayMode();
                break;
            case 2:
                break;
            default:
                throw new System.Exception("ui mode is set to invalid value of "+uimode.ToString()+" must be within 0 to 2 inclusive");
               

        }
    }
}
