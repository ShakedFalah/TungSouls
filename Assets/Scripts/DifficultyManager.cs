using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyManager", menuName = "Scriptable Objects/DifficultyManager")]
public class DifficultyManager : ScriptableObject
{
    public List<DifficultySettings> difficultyList;
    public float difficultyChangeTime;
}
