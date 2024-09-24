using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title_screen_worker : MonoBehaviour
{

    [SerializeField] private string gameStartScene;
    [SerializeField] private GameObject fader;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private float fadeTime;
    private fader_script faderController;
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
        faderController = fader.GetComponent<fader_script>();
        musicController = musicPlayer.GetComponent<audioFaderScript>();
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
                    InputEnable= true;
                    state++;
                }
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                SceneManager.LoadScene(gameStartScene);
                break;
        }
    }
}
