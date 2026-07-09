using Unity.Services.Analytics;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    private bool didFire100m = false;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        gameManager.onDistanceChanged += OnDistanceChange;
    }

    private void OnDistanceChange(float distance)
    {
        if (!didFire100m && distance >= 100)
        {
            OnReached100M(gameManager.currentTime, gameManager.currentScore, gameManager.difficultyLevel);
        }
    }
    public void OnReached100M(float time, int score, int difficultyLevel)
    {
        CustomEvent reached100mEvent = new("reached100m")
        {
            { "InGameTime", time },
            { "difficultyLevel", score },
            { "score", difficultyLevel }
        };

        AnalyticsService.Instance.RecordEvent(reached100mEvent);
    }

}
