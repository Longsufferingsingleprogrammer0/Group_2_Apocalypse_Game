using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Android;



//need to heavily modify for new spawning system



[CreateAssetMenu(fileName = "LevelSpawnDataManager", menuName = "SpawnDataManager", order = 1)]
public class LevelSpawnData : ScriptableObject
{
    //map definition variables
    [SerializeField] private string LevelName;
    [SerializeField] private Vector2 gridZeroPoint;
    [SerializeField] private GridVector2 gridSize;
    //temporary variable until we figure out how player spawning should work
    [SerializeField] private GridVector2 playerStartPos;
    [SerializeField] private GridIllegalSpawnZone[] outOfMapGridAreas;

    //the array of setpeices to spawn
    [SerializeField] private SetpeiceObjectSpawnTable[] setPeiceSpawnList;
    [SerializeField] private EnemySpawnEntry[] enemySpawnTable;
    [SerializeField] private ItemSpawnEntry[] itemSpawnTable;

    public Vector2 getGridZeroPoint()
    {
        return new Vector2(gridZeroPoint.x, gridZeroPoint.y);
    }

    public GridIllegalSpawnZone[] getOutOfMapGridAreas()
    {
        GridIllegalSpawnZone[] arrayCopy = new GridIllegalSpawnZone[outOfMapGridAreas.Length];

        for(int zone = 0; zone < outOfMapGridAreas.Length; zone++)
        {
            arrayCopy[zone]=outOfMapGridAreas[zone].clone();
        }

        return arrayCopy;
    }

    public GridVector2 getPlayerStartPos()
    {
        return playerStartPos.clone();
    }

    public GridIllegalSpawnZone getOutOfMapGridArea(int index)
    {
        return outOfMapGridAreas[index].clone();
    }

    public int getOutOfMapGridAreaListLength()
    {
        return outOfMapGridAreas.Length;
    }

    public GridVector2 getGridSize()
    {
        return gridSize.clone();
    }
    public ItemSpawnEntry getItemSpawnEntry(int index)
    {
        return itemSpawnTable[index].clone();
    }

    public int getItemSpawnTableLength() { return itemSpawnTable.Length; }

    public ItemSpawnEntry[] getItemSpawnTable()
    {
        ItemSpawnEntry[] tableCopy = new ItemSpawnEntry[enemySpawnTable.Length];

        for (int i = 0; i < tableCopy.Length; i++)
        {
            tableCopy[i] = itemSpawnTable[i];
        }
        return tableCopy;
    }
    public EnemySpawnEntry getEnemySpawnEntry(int index)
    {
        return enemySpawnTable[index].clone();
    }

    public int getEnemySpawnTableLength() { return enemySpawnTable.Length; }

    public EnemySpawnEntry[] getEnemySpawnTable()
    {
        EnemySpawnEntry[] tableCopy = new EnemySpawnEntry[enemySpawnTable.Length];

        for (int i = 0; i < tableCopy.Length; i++)
        {
            tableCopy[i] = enemySpawnTable[i];
        }
        return tableCopy;
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
public class EnemySpawnEntry
{
    [SerializeField] private string enemyName;
    [SerializeField] private GameObject enemyType;
    [SerializeField] private bool isRandomizedNumber;
    [SerializeField] private int spawnCountMin;
    [SerializeField] private int spawnCountMax;

    public EnemySpawnEntry(string enemyName, GameObject enemyType, bool isRandomizedNumber, int spawncountMin, int spawnCountMax)
    {
        this.enemyType = enemyType;
        this.enemyName = enemyName;
        this.isRandomizedNumber = isRandomizedNumber;
        this.spawnCountMin = spawncountMin;
        this.spawnCountMax = spawnCountMax;
    }

    public string getEnemyName()
    {
        return enemyName;
    }

    public GameObject getEnemyType()
    {
        return enemyType;
    }

    public bool getIsSpawnCountRandomized()
    {
        return isRandomizedNumber;
    }

    public int getSpawnCountMax() { return spawnCountMax; }

    public int getSpawnCountMin() { return spawnCountMin; }


    public EnemySpawnEntry clone()
    {
        return new EnemySpawnEntry(enemyName,enemyType, isRandomizedNumber, spawnCountMin, spawnCountMax);
    }
}


[System.Serializable]
public class ItemSpawnEntry
{
    [SerializeField] private string itemName;
    [SerializeField] private GameObject itemType;
    [SerializeField] private bool isRandomizedNumber;
    [SerializeField] private int spawnCountMin;
    [SerializeField] private int spawnCountMax;

    public ItemSpawnEntry(string itemName, GameObject itemType, bool isRandomizedNumber, int spawncountMin, int spawnCountMax)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.isRandomizedNumber = isRandomizedNumber;
        this.spawnCountMin = spawncountMin;
        this.spawnCountMax = spawnCountMax;
    }

    public string getItemName()
    {
        return itemName;
    }

    public GameObject getItemType()
    {
        return itemType;
    }

    public bool getIsSpawnCountRandomized()
    {
        return isRandomizedNumber;
    }

    public int getSpawnCountMax() { return spawnCountMax; }

    public int getSpawnCountMin() { return spawnCountMin; }


    public ItemSpawnEntry clone()
    {
        return new ItemSpawnEntry(itemName, itemType, isRandomizedNumber, spawnCountMin, spawnCountMax);
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
    [SerializeField] private GridVector2 position;
    [SerializeField] private int prefabVariant;
    [SerializeField] private Vector2 gridPositionOffset;
    [SerializeField] private bool horizontalOffsetNoSpawnZoneCompensation;
    [SerializeField] private bool verticalOffsetNoSpawnZoneCompensation;

    //getters for the variables
    public GridVector2 getPosition() { return position.clone(); }

    public Vector2 getGridPositionOffset()
    {
        return new Vector2(gridPositionOffset.x, gridPositionOffset.y);
    }
    public int getPrefabVariant() { return prefabVariant; }

    

    public SetpeiceSpawnPosition(GridVector2 position, Vector2 gridPositionOffset, int prefabVariant, bool horizontalOffsetNoSpawnCompensation, bool verticalOffsetNoSpawnCompensation)
    {
        this.position = position;
        this.prefabVariant = prefabVariant;
        this.gridPositionOffset = gridPositionOffset;
        this.horizontalOffsetNoSpawnZoneCompensation = horizontalOffsetNoSpawnCompensation;
        this.verticalOffsetNoSpawnZoneCompensation= verticalOffsetNoSpawnCompensation;
    }



    public bool getHorizontalOffsetNoSpawnCompensation()
    {
        return horizontalOffsetNoSpawnZoneCompensation;
    }

    public bool getVerticalOffsetNoSpawnCompensation()
    {
        return verticalOffsetNoSpawnZoneCompensation;
    }


    //clone function to make life easier
    public SetpeiceSpawnPosition clone()
    {
        return new SetpeiceSpawnPosition(this.getPosition(), this.getGridPositionOffset(), this.getPrefabVariant(),this.getHorizontalOffsetNoSpawnCompensation(), this.getVerticalOffsetNoSpawnCompensation());
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
    [SerializeField]private int x;
    [SerializeField]private int y;

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