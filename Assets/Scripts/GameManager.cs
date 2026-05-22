using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> // making it a singleton
{
    [SerializeField] private DifficultyManager difficultyManager; // the manager 
    [SerializeField] private DifficultySettings difficultySettings; // the manager 
    [SerializeField] private float changeDifficultyTime;

    private float currentTime = 0f;
    private float currentDistance = 0f;
    private int currentScore = 0;
    private int currentDifficulty;

    [SerializeField] private float movementspeed;

    protected override void Awake() 
    {
        base.Awake();
    }

    void Start()
    {
        movementspeed = difficultySettings.movementSpeed;
        StartCoroutine(ChangeDifficulty());
    }

    void Update()
    {
        currentTime += Time.deltaTime; // time
        
        currentDistance += movementspeed * Time.deltaTime; // distance

        if (difficultyManager != null)
        {
            currentDifficulty = difficultyManager.currentDifficulty; // difficulty
        }

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHUDFields(currentTime, currentDistance, currentDifficulty);
        }
        
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateScoreDisplay(currentScore); // displays the score
        }
    }

    IEnumerator ChangeDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDifficultyTime);
            difficultyManager.nextDifficulty();
            
            movementspeed = difficultySettings.movementSpeed;
        }
    }
}
