using Unity.Services.Analytics;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    private bool didCall100m = false;
    private bool didCall50Score = false;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.onDistanceChanged += OnDistanceChange;
        gameManager.onScoreChanged += OnScoreChanged;
        gameManager.onGameOver += OnGameOver;
    }

    private void OnDistanceChange(float distance)
    {
        if (!didCall100m && distance >= 100)
        {
            OnReached100M(gameManager.currentTime, gameManager.currentScore, gameManager.difficultyLevel);
        }
    }
    public void OnReached100M(float time, int score, int difficultyLevel)
    {
        CustomEvent reached100mEvent = new("reached100m")
        {
            { "InGameTime", time },
            { "score", score },
            { "difficultyLevel", difficultyLevel }
        };

        AnalyticsService.Instance.RecordEvent(reached100mEvent);
    }

    public void OnScoreChanged(int score)
    {
        if (!didCall50Score && score >= 50)
        {
            On50Score(gameManager.currentTime, gameManager.currentDistance, gameManager.difficultyLevel);
        }
    }

    public void On50Score(float time, float distance, int difficultyLevel)
    {
        CustomEvent reached100mEvent = new("got50Score")
        {
            { "InGameTime", time },
            { "distance", distance },
            { "difficultyLevel", difficultyLevel }
        };

        AnalyticsService.Instance.RecordEvent(reached100mEvent);
    }

    public void OnGameOver()
    {
        didCall100m = false;
        didCall50Score = false;
        CallGameOverEvent(gameManager.currentTime, gameManager.currentScore, gameManager.currentDistance, gameManager.difficultyLevel);
    }

    public void CallGameOverEvent(float time, int score, float distance, int difficultyLevel)
    {
        CustomEvent reached100mEvent = new("gameOver")
        {
            { "InGameTime", time },
            { "distance", distance },
            { "score", score },
            { "difficultyLevel", difficultyLevel }
        };

        AnalyticsService.Instance.RecordEvent(reached100mEvent);

    }

}
