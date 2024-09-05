
using System.Runtime.CompilerServices;
using UnityEngine;


public class test_player_script : MonoBehaviour
{

#region localVariables:
    //the compoents we need
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D SpritePhysics;




    //never give serialized values a default value, it breaks things

#region movementVariables:
    //distance moved per second
    [SerializeField] private float movementSpeed;
    

#endregion




#region modeVariables:
    //enable movement of sprite at startup
            
    [SerializeField] private bool movementEnabledAtStartup;

    //enbables and disables movement
    private bool movementEnabled;
#endregion

#endregion






    // Start is called before the first frame update
    void Start()
    {
        //this gets the framerate if you want that
        //int targetFramerate = Application.targetFrameRate;

        //get our main components
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        SpritePhysics = GetComponent<Rigidbody2D>();

        //init movement enableing
        movementEnabled = movementEnabledAtStartup;

    }





    //handles key input and moving the player around
    private bool playerMovementHandler2D()
    {
        //my tutor helped out and helped me fix this as well

        //only check for keys if we can move
        if (movementEnabled)
        {
            //how much to move by in each direction
            int toMoveX = 0;
            int toMoveY = 0;
            int distance = 1;
            //self explanatory
            if (Input.GetKey(KeyCode.W))
            {
                toMoveY += distance;
            }

            if (Input.GetKey(KeyCode.S))
            {
                toMoveY -= distance;
            }

            if (Input.GetKey(KeyCode.A))
            {
                toMoveX -= distance;
            }

            if (Input.GetKey(KeyCode.D))
            {
                toMoveX += distance;
            }
            

            //if we need to move
            if ((toMoveX!=0)||(toMoveY!=0))
            {              
                //i did a lot of research, and this is what the internet said for the style i wanted
                //this makes the movment consistant, which is what i wanted
                //to clairify, the internet gave the general math outline and told me the unity stuff i needed to use, i did the coding
                //my tutor then fixed my code

                //create a vector for our movement and adjust it to make sure its the same distance, even when moving horizontally
                Vector3 spriteMovement = new Vector3(toMoveX, toMoveY, 0f).normalized * movementSpeed * Time.deltaTime;


                //flip test sprite, will need to be replaced with anim swap
                if (toMoveX < 0)
                {
                    transform.localScale = new Vector3(-1,1,1);
                }else if (toMoveX > 0)
                {
                    transform.localScale = new Vector3(1,1,1);
                }


                //actually do the moving
                SpritePhysics.MovePosition(spriteMovement+transform.position);
                //transform.Translate(spriteMovement);
                
                return true;
            }

        }
        
        return false;
    }


    //called at a set rate
    private void FixedUpdate()
    {
        //movement handling is functioned out for clairity
        playerMovementHandler2D();
    }






    // Update is called once per frame
    void Update()
    {
        
     
    }
}
