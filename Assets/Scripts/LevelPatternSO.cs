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
    public string lane1Low;
    public string lane1High;

    [Header("Lane 2 (Middle)")]
    public string lane2Low;
    public string lane2High;

    [Header("Lane 3 (Right)")]
    public string lane3Low;
    public string lane3High;
}