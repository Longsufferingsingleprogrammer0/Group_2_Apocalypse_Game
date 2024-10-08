using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{


    //the compoents we need
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D SpritePhysics;
    private Animator spriteAnimator;
    //never give serialized values a default value, it breaks things


    //distance moved per second
    [SerializeField] private float movementSpeed;




    //mode vars:
    //enable movement of sprite at startup    
    [SerializeField] private bool movementEnabledAtStartup;
    //whether the sprite is visible at startup
    [SerializeField] private bool spriteRendererEnabledAtStartup;
    //enables and disables movement
    private bool movementEnabled;

    //attack vars
    [SerializeField] private bool attackEnabledAtStartup;
    private bool attackEnabled;
    [SerializeField] private GameObject knifeAttack;
    private knifeAttackScript knifeController;
    [SerializeField] private float attackCooldownTime;
    private float cooldownElapsed;
    private bool playerAttacking;


    //anim vars
  
    private int lastAnimMode;
    [SerializeField] private string animationDirectionVarName;
    [SerializeField] private string walkModeVarName;
    [SerializeField] private string attackModeVarName;
    [SerializeField] private Sprite[] idleSprites;
    private int lastDirection;

    //audio vars
    [SerializeField] private AudioSource footsteps;
    [SerializeField] private AudioSource attackSound;
    private bool playingWalkingSound;



    public void setAttackEnable(bool state)
    {
        attackEnabled = state;
    }
    public void resetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = idleSprites[0];
    }


    // Start is called before the first frame update
    void Start()
    {
        //this gets the framerate if you want that
        //int targetFramerate = Application.targetFrameRate;

        //get our main components


        attackEnabled = attackEnabledAtStartup;

        playerAttacking = false;

        lastAnimMode = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        SpritePhysics = GetComponent<Rigidbody2D>();

        spriteAnimator = GetComponent<Animator>();

        knifeController = knifeAttack.GetComponent<knifeAttackScript>();

        //init movement enableing
        movementEnabled = movementEnabledAtStartup;
        //init render enabling at at startup
        spriteRenderer.enabled = spriteRendererEnabledAtStartup;

        spriteAnimator.enabled = false;
        lastDirection = 0;
    }


    
    public Vector2 getPosition()
    {
        return new Vector2(transform.position.x+0.5f, transform.position.y-1.2f);
    }
    


    public void setPlayerMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }


    private void attackCoolDownWorker()
    {

        
        
        if (playerAttacking)
        {

            if(attackCooldownTime <= cooldownElapsed )
            {
                playerAttacking=false;
                cooldownElapsed=0;
           
            }
            else
            {
            
                cooldownElapsed += Time.deltaTime;
            }
        }

        
        
    }


    private void playerAttackHandler(int direction)
    {
        //put code here

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {

            if (!playerAttacking)
            {

                knifeController.attack(getPosition(), lastDirection);
                playerAttacking = true;
                attackSound.Play();
            }
       
        }

            //remember to trigger the sound effect
        

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



    private int findDirection(int toMoveX, int toMoveY)
    {
        int newDirection = 0;
        if (toMoveY == 0)
        {
            switch (toMoveX + 1)
            {
                case (0):
                    newDirection = 1;
                    break;
                case (2):
                    newDirection = 3;
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
                    newDirection = 2;
                    break;
                case (2):
                    newDirection = 0;
                    break;
                default:
                    break;
            }


        }
        return newDirection;
    }


    private void playerAnimationHandler(int currentDirection, bool moving)
    {
        int animMode = 0;
        if (moving)
        {
            animMode = 1;
        }else if(playerAttacking)
        {
            animMode = 2;
        }

        if (animMode != lastAnimMode)
        {
            switch (animMode)
            {
                case 0:
         
                    spriteAnimator.enabled = false;

                    
                    spriteAnimator.SetBool(walkModeVarName, false);
                    spriteAnimator.SetBool(attackModeVarName, false);
                    spriteRenderer.sprite = idleSprites[currentDirection];
                    break;
                case 1:
                  
                    spriteAnimator.enabled = true;

                    spriteAnimator.SetBool(walkModeVarName, true);
                    spriteAnimator.SetBool(attackModeVarName, false);

                    break;
                case 2:
                    spriteAnimator.enabled = true;
                    spriteAnimator.SetBool(walkModeVarName, false);
                    spriteAnimator.SetBool(attackModeVarName, true);
                    
                    
                    break;
                default:
                    throw new System.Exception("player anim error 1");

            }
            lastAnimMode = animMode;
        }

        if(currentDirection != lastDirection)
        {
            spriteAnimator.SetInteger(animationDirectionVarName, currentDirection);
            lastDirection = currentDirection;
            if(animMode>0 && animMode < 3)
            {
                spriteRenderer.sprite = idleSprites[currentDirection];
            }
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
            int direction = lastDirection;


            if (moving)
            {
                if (playerAttacking)
                {
                    moving = false;
                }
                else
                {
                    direction=findDirection(toMoveX, toMoveY);

                }
                

            }



            

            playerAnimationHandler(direction,moving);
            playerAudioHandler(moving);
            //if we need to move
            if (moving)
            {              
                //i did a lot of research, and this is what the internet said for the style i wanted
                //this makes the movment consistant, which is what i wanted
                //to clairify, the internet gave the general math outline and told me the unity stuff i needed to use, i did the coding
                //my tutor then fixed my code

                //create a vector for our movement and adjust it to make sure its the same distance, even when moving horizontally
                Vector3 spriteMovement = new Vector3(toMoveX, toMoveY, 0f).normalized * movementSpeed * Time.fixedDeltaTime;

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
        attackCoolDownWorker();
        if (attackEnabled)
        {
            playerAttackHandler(lastDirection);
        }
        
    }
}
