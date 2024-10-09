using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelFinishScreenScript : MonoBehaviour
{

    private GameObject gameMaster;
    [SerializeField] private string sceneToGoTo;
    [SerializeField] private GameObject finalScoreUI;
    [SerializeField] private GameObject deathDayUI;
    [SerializeField] private AudioSource menuSoundSource;
    [SerializeField] private GameObject fader;
    private transitionFaderScript faderController;
  
    private int uiStage;
    [SerializeField] float fadeTime;
    // Start is called before the first frame update
    void Start()
    {
        uiStage = 0;
        gameMaster = GameObject.FindWithTag("game_master");

        //change the text here for level transitions
        gameMaster.GetComponent<Game_Master>().updateScore(gameMaster.GetComponent<Game_Master>().getDay()*100);
        finalScoreUI.GetComponent<TextMeshProUGUI>().SetText("Score: "+gameMaster.GetComponent<Game_Master>().getScore().ToString());
        deathDayUI.GetComponent<TextMeshProUGUI>().SetText("Now Starting Day: " + gameMaster.GetComponent<Game_Master>().getDay().ToString());
        
        //delete this for level transition
    
        
        
        faderController=fader.GetComponent<transitionFaderScript>();
        uiStage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (uiStage)
        {
            case 0:
                break;
            case 1:
                faderController.fadeIn(fadeTime);
                uiStage++;
                break;
            case 2:
                if (faderController.isFadeFinished())
                {
                    uiStage++;
                }
                break;
            case 3:
                if (Input.anyKeyDown)
                {
                    menuSoundSource.Play();
                    faderController.fadeOut(fadeTime);
                    uiStage++;
                }
                break;
            case 4:
                if (faderController.isFadeFinished())
                {
                    if (faderController.isFadeFinished())
                    {
                        uiStage++;
                    }
                }
                break;
            case 5:
                SceneManager.LoadScene(sceneToGoTo);
                break;
        }
    }
}
