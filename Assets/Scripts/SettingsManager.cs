
using System;
using UnityEngine;

public enum InputType
{
    Buttons = 0,
    Swipe = 1
}

public class SettingsManager : SingletonPersistent<SettingsManager>
{
    GameSettings settings;
    public event Action<float> onMusicVolumeChanged;
    public event Action<float> onSFXVolumeChanged;
    public event Action<int> onDifficultyChanged;
    public event Action<InputType> onInputChanged;

    public override void Awake()
    {
        base.Awake();

        settings = new GameSettings();
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", settings.musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", settings.sfxVolume);
        PlayerPrefs.SetInt("Difficulty", (int)settings.difficulty);
        PlayerPrefs.SetInt("InputType", (int)settings.inputType);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        UpdateMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        UpdateSFXVolume(PlayerPrefs.GetFloat("SfxVolume", 1f));
        UpdateDifficulty(PlayerPrefs.GetInt("Difficulty", 0));
        UpdateInput(PlayerPrefs.GetInt("InputType", 0));
    }

    public void UpdateMusicVolume(float volume)
    {
        settings.musicVolume = volume;
        onMusicVolumeChanged?.Invoke(volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        settings.sfxVolume = volume;
        onSFXVolumeChanged?.Invoke(volume);
    }

    public void UpdateDifficulty(int difficultyValue)
    {
        settings.difficulty = difficultyValue;
        onDifficultyChanged?.Invoke(settings.difficulty);
    }

    public void UpdateInput(int inputValue)
    {
        settings.inputType = (InputType)inputValue;
        onInputChanged?.Invoke(settings.inputType);
    }

}


public class GameSettings
{
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public int difficulty = 0;
    public InputType inputType = InputType.Buttons;
}