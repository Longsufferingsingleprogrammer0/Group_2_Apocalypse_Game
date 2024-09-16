using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;



//need to heavily modify for new spawning system



[CreateAssetMenu(fileName = "LevelSpawnDataManager", menuName = "SpawnDataManager", order = 1)]
public class LevelSpawnData : ScriptableObject
{
    [SerializeField] private string LevelName;
    [SerializeField] private SetpeiceObjectSpawnTable[] setPeiceSpawnList;
    [SerializeField] private Vector2 GridTopLeftCorner;
    [SerializeField] private int GridSizeX;
    [SerializeField] private int GridSizeY;
    [SerializeField] private GridIllegalSpawnZone[] outOfMapGridAreas;



    public GridIllegalSpawnZone[] getOutOfMapGridAreas()
    {
        GridIllegalSpawnZone[] arrayCopy = new GridIllegalSpawnZone[outOfMapGridAreas.Length];

        for(int zone = 0; zone < outOfMapGridAreas.Length; zone++)
        {
            arrayCopy[zone]=outOfMapGridAreas[zone].clone();
        }

        return arrayCopy;
    }



    public GridIllegalSpawnZone getOutOfMapGridArea(int index)
    {
        return outOfMapGridAreas[index].clone();
    }


    public GridVector2 getGridSize()
    {
        return new GridVector2(GridSizeX, GridSizeY);
    }

    public Vector2 getGridTopLeftCorner()
    {
        return new Vector2(GridTopLeftCorner.x, GridTopLeftCorner.y);
    }




    
    //getter for levelName;
    public string getLevelName() { return LevelName; }


    public SetpeiceObjectSpawnTable[] getSpawnData()
    {
        //clone array to store the data
        SetpeiceObjectSpawnTable[] cloneArray = new SetpeiceObjectSpawnTable[setPeiceSpawnList.Length];

        for(int table=0; table < setPeiceSpawnList.Length; table++)
        {
            cloneArray[table] = setPeiceSpawnList[table].clone();
        }

        return cloneArray;
    }

    public int getSpawnTableArrayLength() {  return setPeiceSpawnList.Length; }

    public SetpeiceObjectSpawnTable getObjectSpawnTable(int index)
    {
        return setPeiceSpawnList[index].clone();
    }
   
}

[System.Serializable]
public class SetpeiceObjectSpawnTable
{
    [SerializeField] private string actorType;
    [SerializeField] private GameObject[] ActorPrefabVariants;
    [SerializeField] private bool randomizedSpawning;
    [SerializeField] private int minimumSpawnNumber;
    [SerializeField] private int maximumSpawnNumber;
    [SerializeField] private SetpeiceSpawnPosition[] possibleSpawnPositions;

    public SetpeiceObjectSpawnTable(String actorType, GameObject[] ActorPrefabVariants, bool randomizedSpawning, int minimumSpawnNumber, int maximumSpawnNumber, SetpeiceSpawnPosition[] possibleSpawnPositions)
    {
        this.actorType = actorType;
        this.ActorPrefabVariants = ActorPrefabVariants;
        this.randomizedSpawning = randomizedSpawning;
        this.minimumSpawnNumber = minimumSpawnNumber;
        this.maximumSpawnNumber = maximumSpawnNumber;
        this.possibleSpawnPositions = possibleSpawnPositions;
    }


    #region simpleGetters
    public string getActorType() { return actorType; }
    public bool isSpawningRandomized() { return randomizedSpawning; }
    public int getMinimumSpawnNumber() { return minimumSpawnNumber; }
    public int getMaximumSpawnNumber() { return maximumSpawnNumber; }
    public int getActorPrefabVariantCount()
    {
        return ActorPrefabVariants.Length;
    }

    public int getSpawnPositionCount() {  return possibleSpawnPositions.Length; }
    #endregion


    public SetpeiceSpawnPosition GetSetpeiceSpawnPosition(int position)
    {
        return possibleSpawnPositions[position].clone();
    }


    #region cloneGetters
    //getter for actor prefab variants
    public GameObject[] getActorPrefabVariants() 
    {

        //clone array
        GameObject[] listClone = new GameObject[ActorPrefabVariants.Length];


        //loop to populate the new array 
        for(int variant=0; ActorPrefabVariants.Length > variant; variant++)
        {
            listClone[variant] = ActorPrefabVariants[variant];
        }

        //return the clone
        return listClone;
    }

    //getter for possible spawn positions
    public SetpeiceSpawnPosition[] getPossibleSpawnPositions()
    {
        //clone array
        SetpeiceSpawnPosition[] cloneArray = new SetpeiceSpawnPosition[possibleSpawnPositions.Length];

        //loop to populate the new array
        for(int position = 0;  position < possibleSpawnPositions.Length; position++)
        {
            cloneArray[position] = possibleSpawnPositions[position].clone();
        }

        return cloneArray;
    }

    #endregion


    //clone function to make life easier
    public SetpeiceObjectSpawnTable clone() 
    {
        return new SetpeiceObjectSpawnTable(this.getActorType(), this.getActorPrefabVariants(), this.isSpawningRandomized(), this.getMinimumSpawnNumber(), this.getMaximumSpawnNumber(),this.getPossibleSpawnPositions());
    }


    //store the data for each prefab here
}

[System.Serializable]
public class SetpeiceSpawnPosition
{
    [SerializeField] private Vector2 position;
    [SerializeField] private int prefabVariant;
    //used for spawn collision
    [SerializeField] private GridIllegalSpawnZone TakenGridSpace;

    //getters for the variables
    public Vector2 getPosition() { return new Vector2(position.x, position.y); }
    public int getPrefabVariant() { return prefabVariant; }

    public SetpeiceSpawnPosition(Vector2 position, int prefabVariant, GridIllegalSpawnZone takenSpaces)
    {
        this.position = position;
        this.prefabVariant = prefabVariant;
        this.TakenGridSpace = takenSpaces;
    }

    //clone function to make life easier
    public SetpeiceSpawnPosition clone()
    {
        return new SetpeiceSpawnPosition(this.getPosition(), this.getPrefabVariant(), this.getTakenGridSpace());
    }

    public GridIllegalSpawnZone getTakenGridSpace()
    {
        

        return TakenGridSpace.clone();

    }
}

[System.Serializable]
public class GridIllegalSpawnZone
{
    [SerializeField] private int TopLeftCornerGridX;
    [SerializeField] private int TopLeftCornerGridY;
    [SerializeField] private int BottomRightCornerGridX;
    [SerializeField] private int BottomRightCornerGridY;

    public GridIllegalSpawnZone(int x1, int y1, int x2, int y2)
    {
        TopLeftCornerGridX = x1;
        TopLeftCornerGridY = y1;
        BottomRightCornerGridX = x2;
        BottomRightCornerGridY = y2;
    }

    public GridIllegalSpawnZone(GridVector2 topLeftCorner, GridVector2 bottomRightCorner)
    {
        TopLeftCornerGridX = topLeftCorner.getX();
        TopLeftCornerGridY = topLeftCorner.getY();
        BottomRightCornerGridX = bottomRightCorner.getX();
        BottomRightCornerGridY = bottomRightCorner.getY();
    }


    public GridVector2 getTopLeftCorner()
    {
        return new GridVector2(TopLeftCornerGridX, TopLeftCornerGridY);
    }

    public GridVector2 getBottomRightCorner()
    {
        return new GridVector2(BottomRightCornerGridX, BottomRightCornerGridY);
    }
    


    public GridIllegalSpawnZone clone()
    {
        return new GridIllegalSpawnZone(TopLeftCornerGridX, TopLeftCornerGridY, BottomRightCornerGridX, BottomRightCornerGridY);
    }
}



[System.Serializable]
public class GridVector2
{
    private int x;
    private int y;

    public GridVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }



    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public GridVector2 clone()
    {
        return new GridVector2(x, y);
    }
}