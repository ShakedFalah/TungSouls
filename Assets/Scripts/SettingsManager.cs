
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private PauseMenu pauseMenu;

    public override void Awake()
    {
        base.Awake();

        if (Instance != this)
        {
            return;
        }

        settings = new GameSettings();
        Load();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        pauseMenu.UpdateSettings(settings);
    }
}


public class GameSettings
{
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public int difficulty = 0;
    public InputType inputType = InputType.Buttons;
}