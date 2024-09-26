
using UnityEngine;


public class Setpeice_Script : MonoBehaviour
{
    #region variables

    //an array of all the texures used by the sprite
    [SerializeField] private Sprite[] textures;

    //the current texture index
    private int currentTextureIndex;

    //the type of setpeice
    [SerializeField] private string setpeiceType;

    //get our sprite renderer
    private SpriteRenderer spriteRenderer;
    #endregion
    [SerializeField] private GridVector2 gridPosition;
    [SerializeField] private Vector2 GridOffset;
    [SerializeField] private GridIllegalSpawnZone[] gridSize;

    [SerializeField] private int element;

    public int ElementIndex
    {
        get => element;
    }
    public void setElelment(int element)
    {
        this.element = element;
    }

    public int getElement() { return element; }

    public void setRandomTexture()
    {
        setTexture(Random.Range(0, textures.Length));
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


    public int getTextureCount()
    {
        return textures.Length;
    }


    public Vector2 getPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public Vector2 getGridOffset()
    {
        return new Vector2(GridOffset.x, GridOffset.y);
    }

    public GridVector2 getGridPosition()
    {
        return gridPosition.clone();
    }


    public void setPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void setGridOffset(Vector2 offset)
    {
        transform.Translate(new Vector3(-GridOffset.x, GridOffset.y, 0f));
        GridOffset = new Vector2(offset.x, offset.y);
        transform.Translate(new Vector3(offset.x, -offset.y, 0f));

    }
    public void setGridPosition(Vector2 gridZeroZeroPoint, int x, int y)
    {
        float newX = gridZeroZeroPoint.x + x + GridOffset.x;
        float newY = gridZeroZeroPoint.y + (-y) - GridOffset.y;
        gridPosition = new GridVector2(x, y);
        Vector2 newPosition = new Vector2(newX, newY);
        transform.position = newPosition;
    }


    public void setTexture(int textureIndex)
    {
        if ((textureIndex >= 0) && (textureIndex < textures.Length))
        {
            currentTextureIndex = textureIndex;
        }
        else
        {
            throw new System.Exception("invalid texture index for a " + setpeiceType + ". acceptable values are between 0 and " + textures.Length.ToString() + ". argument value was " + textureIndex.ToString());
        }

        spriteRenderer.sprite = textures[textureIndex];

    }




    // Start is called before the first frame update
    void Start()
    {
        //get our sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        //generate a texture index to use
        currentTextureIndex = Random.Range(0, textures.Length);

        //set our sprite to said index
        spriteRenderer.sprite = textures[currentTextureIndex];


    }

}
