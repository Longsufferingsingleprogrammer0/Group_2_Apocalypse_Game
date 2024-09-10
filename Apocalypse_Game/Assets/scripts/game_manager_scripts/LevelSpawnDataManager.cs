using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSpawnDataManager", menuName = "SpawnDataManager", order = 1)]
public class LevelSpawnData : ScriptableObject
{
    public string LevelName;
    public ObjectSpawnTable[] spawnData;
}

[System.Serializable]
public class ObjectSpawnTable
{
    public string actorType;
    public GameObject[] ActorPrefabVariants;
    public bool randomizedSpawning;
    public int minimumSpawnNumber;
    public int maximumSpawnNumber;

    public spawnPosition[] possibleSpawnPositions;
    //store the data for each prefab here
}

[System.Serializable]
public class spawnPosition
{
    public Vector2 position;
    public int prefabVariant;
}