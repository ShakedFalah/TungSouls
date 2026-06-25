using UnityEngine;

[CreateAssetMenu(fileName = "NewPattern", menuName = "Scriptable Objects/Level Pattern")]
public class LevelPatternSO : ScriptableObject
{
    public float distanceBetweenRows = 8f; 
    public PatternRow[] rows; 
}

[System.Serializable]
public struct PatternRow 
{
    [Header("Lane 1 (Left)")]
    public ItemType lane1Low;
    public ItemType lane1High;

    [Header("Lane 2 (Middle)")]
    public ItemType lane2Low;
    public ItemType lane2High;

    [Header("Lane 3 (Right)")]
    public ItemType lane3Low;
    public ItemType lane3High;
}