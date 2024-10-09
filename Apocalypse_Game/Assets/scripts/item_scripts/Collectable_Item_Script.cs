
using UnityEngine;

public class Collectable_Item_Script : MonoBehaviour
{
    //item identifers
    [SerializeField] private string itemName;
    [SerializeField] private int itemID;

    //variable used for debugging
    [SerializeField] private int element;

    //used for spawning 
    [SerializeField] private GridVector2 gridPosition;
    [SerializeField] private GridIllegalSpawnZone[] gridSize;

    //relevant information to object
    [SerializeField] private bool randomizedValue;
    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;
    private int value;


    //may need later
    private SpriteRenderer spriteRenderer;
    private GameObject GameManager;

    public int ElementIndex
    {
        get => element;
    }
    public void setElelment(int element)
    {
        this.element = element;
    }

    public int getElement() { return element; }


    public string getName()
    {
        return itemName;
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
        return new Vector2(transform.position.x, transform.position.y);
    }



    public GridVector2 getGridPosition()
    {
        return gridPosition.clone();
    }


    public void setPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }


    
    public void setGridPosition(Vector2 grid00Point, int x, int y)
    {
        float newX = grid00Point.x + x;
        float newY = grid00Point.y + (-y);
        gridPosition = new GridVector2(x, y);
        Vector2 newPosition = new Vector2(newX, newY);
        transform.position = newPosition;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject.FindWithTag("Level_Master").GetComponent<LevelManager>().playCollectItemSound();
            GameManager.GetComponent<Game_Master>().collectItem(value, itemID);
            GameObject.FindWithTag("Level_Master").GetComponent<LevelManager>().collectItem(gameObject);
            Destroy(gameObject);
        }
    }


    


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager = GameObject.FindWithTag("game_master");

        if (randomizedValue)
        {
            value = Random.Range(minValue, maxValue);
        }
        else
        {
            value = maxValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
