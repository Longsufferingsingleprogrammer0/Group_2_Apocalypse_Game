
using UnityEngine;


public class test_player_script : MonoBehaviour
{

    #region localVariables:
        //the compoents we need
        private SpriteRenderer spriteRenderer;
        private Transform spriteTransform;



        //never give serialized values a default value, it breaks things



        #region movementVariables:
            //distance moved on every update
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
        spriteTransform = GetComponent<Transform>();


        //init movement enableing
        movementEnabled = movementEnabledAtStartup;

        

    }






    //handles key input and moving the player around
    private bool playerMovementHandler2D()
    {
        //did movement occur
        bool changedPos = false;

        //only check for keys if we can move
        if (movementEnabled)
        {
            //how much to move by in each direction
            float toMoveX = 0;
            float toMoveY = 0;
            float distance = 1f;
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
                //set that we moved
                changedPos = true;
                
                //i did a lot of research, and this is what the internet said for the style i wanted
                //this makes the movment consistant, which is what i wanted
                //to clairify, the internet gave the general math outline and told me the unity stuff i needed to use, i did the coding

                //adjust how much to move for our speed and delta time
                toMoveX = toMoveX * movementSpeed * Time.deltaTime;
                toMoveY = toMoveY * movementSpeed * Time.deltaTime;

                //create a vector for our movement and adjust it to make sure its the same distance, even when moving horizontally
                Vector3 spriteMovement = new Vector3(toMoveX, toMoveY, 0f).normalized;
                    
                //actually do the moving
                spriteTransform.Translate(spriteMovement);
            }

        }
        
        return changedPos;
    }

    // Update is called once per frame
    void Update()
    {
        //movement handling is functioned out for clairity
        playerMovementHandler2D();
     
    }
}
