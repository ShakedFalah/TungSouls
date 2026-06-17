using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void UpdateMusicVolume(float volume)
    {
        AudioManager.Instance.UpdateMusicVolume(volume);
    }

    public void UpdateSFXVolume(float volume)
    {
        AudioManager.Instance.UpdateSFXVolume(volume);
    }

    public void UpdateDifficultyLevel(int difficultyLevel)
    {
        GameManager.Instance.UpdateDifficutlyLevel(difficultyLevel);
    }

    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

}
