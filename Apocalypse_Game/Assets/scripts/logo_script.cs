using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class logo_script : MonoBehaviour
{
    // Start is called before the first frame update

    private float elsapsed;
    [SerializeField] private int waitTimeSeconds;
    [SerializeField] private string SceneToSwitchTo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        elsapsed += Time.deltaTime;

        if (elsapsed >= ((float)waitTimeSeconds))
        {
            SceneManager.LoadScene(sceneName: SceneToSwitchTo);
        }else if (Input.anyKey)
        {
            SceneManager.LoadScene(sceneName: SceneToSwitchTo);
        }

    }
}
