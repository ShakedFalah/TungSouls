using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyManager", menuName = "Scriptable Objects/DifficultyManager")]
public class DifficultyManager : VariableChangeEventSO<ScriptableObject>
{
    [SerializeField]
    private List<DifficultySettings> difficultyList;

    private int currentDifficulty;

    public void nextDifficulty()
    {
        if (currentDifficulty < difficultyList.Count - 1)
        {
            currentDifficulty++;
            this.Value = difficultyList[currentDifficulty];
        }
    }
}
