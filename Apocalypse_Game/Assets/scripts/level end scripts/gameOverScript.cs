using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameOverScript : MonoBehaviour
{

    private GameObject gameMaster;
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
        finalScoreUI.GetComponent<TextMeshProUGUI>().SetText("Final Score: "+gameMaster.GetComponent<Game_Master>().getScore().ToString());
        deathDayUI.GetComponent<TextMeshProUGUI>().SetText("Days Survived: " + gameMaster.GetComponent<Game_Master>().getDay().ToString());
        
        //delete this for level transition
        gameMaster.GetComponent<Game_Master>().resetAllVariables();
        
        
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
                break;
        }
    }
}
