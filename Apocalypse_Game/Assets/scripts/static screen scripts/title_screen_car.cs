using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class title_screen_car : MonoBehaviour
{
    [SerializeField] private float roadY;
    [SerializeField] private float startPositionX;
    [SerializeField] private float loopPosiotionX;
    [SerializeField] private float movementSpeed;

    private SpriteRenderer carRenderer;



    private void updateCar()
    {
        if(carRenderer.transform.position.x >= loopPosiotionX)
        {
            carRenderer.transform.position = new Vector2(startPositionX, roadY);
        }
        else
        {
            Vector3 spriteMovement = new Vector3(1, 0f, 0f).normalized * movementSpeed * Time.deltaTime;
            carRenderer.transform.Translate(spriteMovement);
        }
    }


    
    // Start is called before the first frame update
    void Start()
    {
        carRenderer=GetComponent<SpriteRenderer>();
        carRenderer.transform.position =new Vector2(startPositionX, roadY);
    }

    
    private void FixedUpdate()
    {
        updateCar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
