using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wide_horizontal_shelf : MonoBehaviour
{
    //number of texture options
    private const int textureCount = 3;
    //the starting texture
    [SerializeField] private int startingTexture;

    //the current texture index
    private int currentTextureIndex;



    public int getTextureCount()
    {
        return textureCount;
    }

    public void setTexture(int textureIndex)
    {
        if ((textureIndex >= 0) && (textureIndex < textureCount))
        {
            currentTextureIndex = textureIndex;
        }
        else
        {
            throw new System.Exception("invalid texture index for a wide horizontal shelf. acceptable values are between 0 and "+textureCount.ToString()+". argument value was "+textureIndex.ToString());
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        currentTextureIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
