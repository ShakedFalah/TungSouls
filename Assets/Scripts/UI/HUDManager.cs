using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour // singleton
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText; // score
    [SerializeField] private TextMeshProUGUI timeText; // time
    [SerializeField] private TextMeshProUGUI distanceText; // distance
    [SerializeField] private TextMeshProUGUI difficultyText; // difficulty
    [SerializeField] private TextMeshProUGUI gameOverText; // game over
    [SerializeField] private GameObject inputButtons;

    void Start()
    {
        gameOverText.enabled = false;
        UpdateScoreDisplay(0);
        UpdateHUDFields(0f, 0f, 1);
        GameManager.Instance.onGameOver += ShowGameOver;

        SettingsManager.Instance.onInputChanged += UpdateInputType;
        UpdateInputType(SettingsManager.Instance.settings.inputType);
    }

    public void UpdateScoreDisplay(int scoreToDisplay)
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + scoreToDisplay;
        }
    }

    public void UpdateHUDFields(float time, float distance, int difficulty)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        if (timeText != null)
        {
            timeText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds); // shortcut for making a real looking timer
        }

        if (distanceText != null)
        {
            distanceText.text = "Distance: " + Mathf.FloorToInt(distance) +  "m";
        }

        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty: " + difficulty;
        }
    }

    private void ShowGameOver()
    {
        gameOverText.enabled = true;
    }

    private void UpdateInputType(InputType inputType)
    {
        if (inputType != InputType.Buttons)
        {
            inputButtons.SetActive(false);
        } else
        {
            inputButtons.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameOver -= ShowGameOver;
        SettingsManager.Instance.onInputChanged -= UpdateInputType;
    }
}
