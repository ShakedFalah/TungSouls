using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Scriptable Objects/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    public float spawnRate;
    public float movementSpeed;
    public int maxObstacles;
    public List<ObstacleSettings> obstacleSettings;
    public Material groundMaterial;
    public List<ObstacleSettings> environmentObjects;
}

[System.Serializable]
public struct ObstacleSettings
{
    public string obstacleTag;
    public float probability;
}
