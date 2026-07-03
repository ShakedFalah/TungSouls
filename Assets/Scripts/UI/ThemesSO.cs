using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemesSo", menuName = "Scriptable Objects/ThemesSo")]
public class ThemesSo : ScriptableObject
{
    public int currentThemeIndex = 0;
    
    public List<Material> skyThemes = new List<Material>();
}
