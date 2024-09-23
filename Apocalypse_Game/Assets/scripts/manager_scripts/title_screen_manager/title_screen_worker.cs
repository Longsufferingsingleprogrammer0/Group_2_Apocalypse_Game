using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class title_screen_worker : MonoBehaviour
{

    [SerializeField] private string gameStartScene;
    public void startGame()
    {
        SceneManager.LoadScene(gameStartScene);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
