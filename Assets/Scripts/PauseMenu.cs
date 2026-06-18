using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public void UpdateMusicVolume(float volume)
    {
        SettingsManager.Instance.UpdateMusicVolume(volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        SettingsManager.Instance.UpdateSFXVolume(volume);
    }

    public void UpdateDifficultyLevel(int difficultyLevel)
    {
        SettingsManager.Instance.UpdateDifficulty(difficultyLevel);
    }

    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    public void UpdateSettings(GameSettings settings)
    {
        musicSlider.value = settings.musicVolume;
        sfxSlider.value = settings.sfxVolume;
    }

}
