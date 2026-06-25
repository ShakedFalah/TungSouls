using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Scriptable Objects/DifficultySettings")]
public class DifficultySettings : ScriptableObject
{
    public float spawnRate;
    public float movementSpeed;
    public int maxObstacles;
    public PatternSettings[] obstacleSettings;
    public Material groundMaterial;
    public List<ItemType> environmentObjects;
}

[System.Serializable]
public struct PatternSettings
{
    public LevelPatternSO pattern;
    public float probability;
}
