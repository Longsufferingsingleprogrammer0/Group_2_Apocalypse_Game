using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSpawnDataManager", menuName = "SpawnDataManager", order = 1)]
public class LevelSpawnData : ScriptableObject
{
    [SerializeField]private string LevelName;
    [SerializeField]private SetpeiceObjectSpawnTable[] setPeiceSpawnList;
    
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

    public int getObjectArrayLength() {  return setPeiceSpawnList.Length; }

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

    //getters for the variables
    public Vector2 getPosition() { return new Vector2(position.x, position.y); }
    public int getPrefabVariant() { return prefabVariant; }

    public SetpeiceSpawnPosition(Vector2 position, int prefabVariant)
    {
        this.position = position;
        this.prefabVariant = prefabVariant;
    }

    //clone function to make life easier
    public SetpeiceSpawnPosition clone()
    {
        return new SetpeiceSpawnPosition(this.position, this.prefabVariant);
    }
}