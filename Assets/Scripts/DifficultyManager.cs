using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyManager", menuName = "Scriptable Objects/DifficultyManager")]
public class DifficultyManager : VariableChangeEventSO<ScriptableObject>
{
    [SerializeField]
    private List<DifficultySettings> difficultyList;

    [SerializeField] private int currentDifficultyIndex = 0; // for the HUD

    public int currentDifficulty => currentDifficultyIndex + 1; // for nextDifficulty
    
    private void OnEnable()
    {
        currentDifficultyIndex = 0; // Force it to start fresh at index 0 (Level 1)
    }
    
    public void nextDifficulty()
    {
        if (currentDifficultyIndex < difficultyList.Count - 1)
        {
            currentDifficultyIndex++;
            this.Value = difficultyList[currentDifficultyIndex];
        }
    }
}
