using UnityEngine;


public class Player : MonoBehaviour
{

    #region localVariables:
    //the compoents we need
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D SpritePhysics;
    private Animator spriteAnimator;
    //never give serialized values a default value, it breaks things

    #region movementVariables:
    //distance moved per second
    [SerializeField] private float movementSpeed;

    #endregion


    #region modeVariables:
    //enable movement of sprite at startup    
    [SerializeField] private bool movementEnabledAtStartup;
    
    //whether the sprite is visible at startup
    [SerializeField] private bool spriteRendererEnabledAtStartup;
    
    //enables and disables movement
    private bool movementEnabled;

    #endregion

    #region animationVariables:
    [SerializeField] private string animationControlParamater;
    [SerializeField] private Sprite[] idleSprites;
    
    private int direction;
    #endregion
    #region audioVariables;
    [SerializeField] private AudioSource footsteps;
    private bool playingWalkingSound;
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

        spriteAnimator = GetComponent<Animator>();
        //init movement enableing
        movementEnabled = movementEnabledAtStartup;
        //init render enabling at at startup
        spriteRenderer.enabled = spriteRendererEnabledAtStartup;

        spriteAnimator.enabled = false;
        direction = 1;
    }


    public void setPlayerMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }


    private void playerAudioHandler(bool moving)
    {
        if (moving != playingWalkingSound)
        {
            if (moving)
            {
                footsteps.Play();
                playingWalkingSound = true;
            }
            else
            {
                footsteps.Stop();
                playingWalkingSound = false;
            }
        }

    }

    private void playerAnimationHandler(int toMoveX, int toMoveY, bool moving)
    {
        if (moving)
        {
            if (!spriteAnimator.enabled)
            {
                spriteAnimator.enabled = true;
            }
            int newDirection = 0;
            if (toMoveY == 0)
            {
                switch (toMoveX + 1)
                {
                    case (0):
                        newDirection = 2;
                        break;
                    case (2):
                        newDirection = 4;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (toMoveY + 1)
                {
                    case (0):
                        newDirection = 3;
                        break;
                    case (2):
                        newDirection = 1;
                        break;
                    default:
                        break;
                }
                
                
            }
            if (newDirection != direction)
            {
                direction = newDirection;
                spriteRenderer.sprite = idleSprites[direction - 1];
            }
            if (spriteAnimator.GetInteger(animationControlParamater) != direction)
            {
                spriteAnimator.SetInteger(animationControlParamater, direction);
            }

        }
        else if (spriteAnimator.enabled)
        {
            spriteRenderer.sprite = idleSprites[direction - 1];
            spriteAnimator.enabled = false;
            spriteAnimator.SetInteger(animationControlParamater, 0);
            
        }
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

            bool moving = (toMoveX != 0) || (toMoveY != 0);
            playerAnimationHandler(toMoveX, toMoveY,moving);
            playerAudioHandler(moving);
            //if we need to move
            if (moving)
            {              
                //i did a lot of research, and this is what the internet said for the style i wanted
                //this makes the movment consistant, which is what i wanted
                //to clairify, the internet gave the general math outline and told me the unity stuff i needed to use, i did the coding
                //my tutor then fixed my code

                //create a vector for our movement and adjust it to make sure its the same distance, even when moving horizontally
                Vector3 spriteMovement = new Vector3(toMoveX, toMoveY, 0f).normalized * movementSpeed * Time.deltaTime;

                //TODO: changing the sprite direction and animation 

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
