using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Furnature_Setpeice : MonoBehaviour
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








    public void setRandomTexture()
    {
        setTexture(Random.Range(0, textures.Length));
    }




    public int getTextureCount()
    {
        return textures.Length;
    }


    public Vector2 getPosition()
    {
        return new Vector2(transform.position.x,transform.position.y);
    }

    public void setPosition(float x, float y)
    {
        transform.position = new Vector3(x,y,transform.position.z);
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
