using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Loading;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public partial class LevelManager : MonoBehaviour
{
    private int uimode;
    private int uiStage;
    private GameObject gameManager;
    [SerializeField] private GameObject fader;
    [SerializeField] private GameObject fadeOutFader;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject exitButton;
    private transitionFaderScript faderController;
    [SerializeField] private float loadingFadeTime;
    [SerializeField] private string menuScene;
    private bool paused;
    private bool pauseTransition;

    //ui code

    private void PauseHandler()
    {
        if (!pauseTransition)
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseTransition = true;
                uiStage = 0;
              
                if (paused)
                {
                    
                    setUiMode(5);
                }
                else
                {
                   
                    setUiMode(3);
                }
            }
        }
        
    }


    public void resumeClick()
    {
        pauseTransition = true;
        uiStage = 0;
        setUiMode(5);
    }


    private void setUIStage(int uiStage)
    {
        this.uiStage = uiStage;
    }

    private void setUiMode(int uiMode)
    {
        this.uimode = uiMode;
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
                pauseTransition = false;
                mapSetupStage = 0;
                uimode++;
                uiStage = 0;
                break;


        }
    }

    private void startGamePlayMode()
    {
        playerSprite.GetComponent<Player>().setPlayerMovementEnabled(true);
    }


    private void fadeInPauseMenu()
    {
        
        switch (uiStage)
        {
            case 0:
                pauseTransition = true;
                pauseGamePlayLogic();
                uiStage++;
                break;
            case 1:
                faderController.fadeOut(loadingFadeTime / 2);
                uiStage++;
                break;
            case 2:
                if (faderController.isFadeFinished())
                {
                    uiStage++;
                }
                break;
            case 3:
                resumeButton.SetActive(true);
                exitButton.SetActive(true);
                uimode=4;
                uiStage = 0;
                paused = true;
                pauseTransition = false;
                
                break;

        }
    }

    private void fadeOutPauseMenu()
    {
        switch (uiStage)
        {
            case 0:
                pauseTransition = true;
                resumeButton.SetActive(false);
                exitButton.SetActive(false);
                uiStage++;
                break;
            case 1:
                faderController.fadeIn(loadingFadeTime / 2);
                uiStage++;
                break;
            case 2:
                if (faderController.isFadeFinished())
                {
                    uiStage++;
                }
                break;
            case 3:
                paused = false;
                pauseTransition = false;
                resumeGamePlayLogic();
                uimode = 2;
                uiStage = 0;
                break;

        }
    }


    private void uiGameplayMode()
    {
        //placeholder
    }

    private void exitMode()
    {
        switch (uiStage)
        {
            case 0:
                pauseTransition = true;
                gameManager.GetComponent<Game_Master>().resetAllVariables();
                resumeButton.GetComponent<Button>().enabled = false;
                exitButton.GetComponent<Button>().enabled = false;
                uiStage++;
                break;
            case 1:
                fadeOutFader.GetComponent<transitionFaderScript>().fadeOut(loadingFadeTime);
                uiStage++;
                break;
            case 2:
                if (fadeOutFader.GetComponent<transitionFaderScript>().isFadeFinished())
                {
                    uiStage++;
                }
                break;
            case 3:
                SceneManager.LoadScene(menuScene);
                uiStage++;
                uiStage = 0;
                break;
        }
    }


    // Start is called before the first frame update
    void UIStart()
    {
        pauseTransition = true;
        paused = false;
        uiStage = 0;
        uimode = 0;
        gameManager=  GameObject.FindWithTag("game_master");
        faderController = fader.GetComponent<transitionFaderScript>();
        resumeButton.SetActive(false);
        exitButton.SetActive(false);
    }

    public void exitClick()
    {
        uimode = 6;
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
                startGamePlayMode();
                break;
            case 2:
                uiGameplayMode();
                break;
            case 3:
                fadeInPauseMenu();
                break;
            case 4:
                break;
            case 5:
                fadeOutPauseMenu();
                break;
            case 6:
                exitMode();
                break;
            default:
                throw new System.Exception("ui mode is set to invalid value of "+uimode.ToString()+" must be within 0 to 2 inclusive");

            
        }
        PauseHandler();
    }
}
