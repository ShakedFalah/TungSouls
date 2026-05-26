using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> // making it a singleton
{
    [SerializeField] private DifficultyManager difficultyManager;
    private int currentDifficultyIndex;
    public event Action<DifficultySettings> onDifficultyChange;
    public DifficultySettings CurrentDifficulty {
        get 
        {
            return difficultyManager.difficultyList[currentDifficultyIndex];
        }
    }

    private float currentTime = 0f;
    private float currentDistance = 0f;
    private int currentScore = 0;
    protected override void Awake() 
    {
        base.Awake();
    }

    void Start()
    {
        StartCoroutine(ChangeDifficulty());
    }

    void Update()
    {
        currentTime += Time.deltaTime; // time
        
        currentDistance += CurrentDifficulty.movementSpeed * Time.deltaTime; // distance

        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHUDFields(currentTime, currentDistance, currentDifficultyIndex + 1);
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
            yield return new WaitForSeconds(difficultyManager.difficultyChangeTime);
            if (currentDifficultyIndex < difficultyManager.difficultyList.Count - 1) { 
                currentDifficultyIndex++;
                if (onDifficultyChange != null)
                {
                    onDifficultyChange.Invoke(CurrentDifficulty);
                }
            }
        }
    }
}

