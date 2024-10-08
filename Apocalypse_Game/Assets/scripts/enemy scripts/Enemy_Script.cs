
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    //item identifers
    [SerializeField] private string enemyName;
    [SerializeField] private int enemyID;

    //variable used for debugging
    [SerializeField] private int element;

    //used for spawning 
    [SerializeField] private GridVector2 gridStartingPosition;
    [SerializeField] private GridIllegalSpawnZone[] gridSize;

    //health vars
    [SerializeField] private bool randomizedHealth;
    [SerializeField] private float maxhealth;
    [SerializeField] private float minhealth;
    [SerializeField] private float health;


    //speed
    [SerializeField] private bool randomSpeed;
    [SerializeField] private float MinSpeed;
    [SerializeField] private float MaxSpeed;
    private float speed;

    //attack vars
    [SerializeField] private bool randomizedAttackPoints;
    [SerializeField] private float maxAttack;
    [SerializeField] private float minAttack;
    [SerializeField] private float playerDetectionDistance;


    //movement var
    [SerializeField] private float minDirectionChangeTime;
    [SerializeField] private float maxDirectionChangeTime;
    private float changeDirectionTimer;
    private float elaspedDirectionChangeTime;
    private int direction;
    private bool sawPlayer;


    //systemVars
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D collisionBody;
    private GameObject GameManager;
    private GameObject LevelManager;
    private GameObject player;
    private Player playerScript;


    public int ElementIndex
    {
        get => element;
    }
    public void setElelment(int element)
    {
        this.element = element;
    }

    public string getName()
    {
        return enemyName;
    }

    public int getElement() { return element; }


    
    private bool canSeePlayer()
    {
        return (getPlayerDistance() < playerDetectionDistance);
    }
    

    private bool randomBool()
    {
        return Random.Range(0, 2) == 1;
    }


    public GridIllegalSpawnZone[] getGridSize()
    {
        GridIllegalSpawnZone[] copy = new GridIllegalSpawnZone[gridSize.Length];
        for (int box = 0; box < gridSize.Length; box++)
        {
            GridVector2 bottomRightCorner = gridSize[box].getBottomRightCorner();
            GridVector2 topLeftCorner = gridSize[box].getTopLeftCorner();
            bottomRightCorner = new GridVector2(bottomRightCorner.getX(), bottomRightCorner.getY());
            topLeftCorner = new GridVector2(topLeftCorner.getX(), topLeftCorner.getY());
            copy[box] = new GridIllegalSpawnZone(topLeftCorner, bottomRightCorner);

        }
        return copy;
    }


    public Vector2 getPosition()
    {
        return new Vector2(transform.position.x+0.5f, transform.position.y-1.3f);
    }


    public void damage(float damage)
    {
        health-=damage;
        if(health < 0)
        {
            Destroy(gameObject);
        }
    }


    
    public void chaseMove()
    {
        if (getPlayerDistance() > 0.5)
        {
            Vector3 chaseDirection = (playerScript.getPosition() - getPosition()).normalized;
            Vector3 spriteMovement = new Vector3(chaseDirection.x, chaseDirection.y, 0f).normalized * speed * Time.deltaTime;
            collisionBody.MovePosition(spriteMovement + new Vector3(collisionBody.position.x, collisionBody.position.y, 0));
        }

    }

    private void wanderMove()
    {
        int toMoveX = 0;
        int toMoveY = 0;
        int distance = 1;

        switch (direction)
        {
            case 0:
                break;
            case 1:
                toMoveY += distance;
                break;
            case 2:
                toMoveX -= distance;
                toMoveY += distance;
                break;
            case 3:
                toMoveX -= distance;
                break;
            case 4:
                toMoveY -= distance;
                toMoveX-= distance;
                break;
            case 5:
                toMoveY -= distance;
                break;
            case 6:
                toMoveX += distance;
                toMoveY -= distance;
                break;
            case 7:
                toMoveX += distance;
                break;
            case 8:
                toMoveY += distance;
                toMoveX += distance;
                break;



        }

        bool moving = (toMoveX != 0) || (toMoveY != 0);

        if (moving)
        {
            if(getPlayerDistance() >0.5)
            {
                Vector2 currentPos = getPosition();
                Vector3 spriteMovement = new Vector3(toMoveX, toMoveY, 0f).normalized * speed * Time.deltaTime;
                collisionBody.MovePosition(spriteMovement + new Vector3(collisionBody.position.x,collisionBody.position.y,0));
            }
            
        }
    }


    private void movementController2D()
    {
   
        if(elaspedDirectionChangeTime>= changeDirectionTimer)
        {
            elaspedDirectionChangeTime = 0;
            changeDirectionTimer = Random.Range(minDirectionChangeTime, maxDirectionChangeTime);
            if (randomSpeed)
            {
                speed=Random.Range(MinSpeed, MaxSpeed);
            }
            else
            {
                speed = MaxSpeed;
            }
            sawPlayer = canSeePlayer();
            if (!sawPlayer)
            {
                direction = Random.Range(0, 9);
            }
            
        }
        else
        {
            elaspedDirectionChangeTime += Time.deltaTime;
        }
        if (sawPlayer)
        {
            chaseMove();
        }
        else
        {
            wanderMove();
        }
    }






    public GridVector2 getGridStartingPosition()
    {
        return gridStartingPosition.clone();
    }


    public void setPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private float attack()
    {
        if (randomizedAttackPoints)
        {
            return Random.Range(minAttack, maxAttack);
        }
        else
        {
            return maxAttack;
        }
    }

    public void setGridPosition(Vector2 grid00Point, int x, int y)
    {
        float newX = grid00Point.x + x;
        float newY = grid00Point.y + (-y);
        gridStartingPosition = new GridVector2(x, y);
        Vector2 newPosition = new Vector2(newX, newY);
        transform.position = newPosition;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
   
        
          
        if (collision.CompareTag("Player"))
        {
            LevelManager.GetComponent<LevelManager>().damagePlayer(attack());
        }

        if (collision.CompareTag("attack"))
        {
            GameObject.FindWithTag("attack").GetComponent<knifeAttackScript>();
        }
        
    }

    
    private float getPlayerDistance()
    {
        return Vector2.Distance(getPosition(),playerScript.getPosition());
    }

    

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collisionBody = GetComponent<Rigidbody2D>();
        
        GameManager = GameObject.FindWithTag("game_master");
        LevelManager = GameObject.FindWithTag("Level_Master");
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Player>();

        if (GameManager == null)
        {
            throw new System.Exception("enemy reference to game manager is null");
        }
         if (LevelManager == null)
        {
            throw new System.Exception("enemy reference to level manager is null");
        }
        if (player == null)
        {
            throw new System.Exception("enemy reference to player is null");
        }
        if (playerScript == null)
        {
            throw new System.Exception("enemy reference to player script is null");
        }



        if (randomizedHealth)
        {
            health = Random.Range(minhealth, maxhealth);
        }
        else
        {
            health = maxhealth;
        }
    }


    private void FixedUpdate()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        movementController2D();
    }
}
