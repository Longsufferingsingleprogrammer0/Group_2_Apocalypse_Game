using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class title_screen_worker : MonoBehaviour
{

    [SerializeField] private string gameStartScene;
    [SerializeField] private GameObject fader;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private float fadeTime;
    [SerializeField] private GameObject startButton;
    private transitionFaderScript faderController;
    private audioFaderScript musicController;
    private bool InputEnable;

    private int state;

    public void startGame()
    {
        if (InputEnable) 
        {
            state++;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        InputEnable = false;
        faderController = fader.GetComponent<transitionFaderScript>();
        musicController = musicPlayer.GetComponent<audioFaderScript>();
        startButton.GetComponent<Button>().enabled = false;
        faderController.fadeIn(fadeTime);
        musicController.fadeIn(fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                if (Input.anyKeyDown)
                {
                    faderController.skipTransition();
                    musicController.skipTransition();

                }
                if (faderController.isFadeFinished() && musicController.isFadeFinished())
                {
                    state++;
                    InputEnable = true;
                    startButton.GetComponent<Button>().enabled = true;
                    
                }
                break;
            case 1:
                break;
            case 2:
                startButton.GetComponent<Button>().enabled = false;
                InputEnable = false;
                faderController.fadeOut(fadeTime);
                musicController.fadeOut(fadeTime);
                state++;
                break;
            case 3:
                if (Input.anyKeyDown)
                {
                    faderController.skipTransition();
                    musicController.skipTransition();

                }
                if (faderController.isFadeFinished() && musicController.isFadeFinished())
                {
                    state++;

                }
                break;
            case 4:
                SceneManager.LoadScene(gameStartScene);
                break;
        }
    }
}
