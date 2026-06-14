using UnityEngine;

public enum InputType
{
    Buttons,
    Gestures
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float bgmVolume;
    public float sfxVolume;
    public InputType inputType;
    public Difficulty difficulty;
}
