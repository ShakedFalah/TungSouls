using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // to restart the game

public class GameManager : Singleton<GameManager> // making it a singleton
{
    [SerializeField] private DifficultyManager difficultyManager;
    
    private int currentDifficultyIndex;
    private float currentTime = 0f;
    private float currentDistance = 0f;
    private int currentScore = 0;
    
    public event Action<DifficultySettings> onDifficultyChange;
    public DifficultySettings CurrentDifficulty => difficultyManager.difficultyList[currentDifficultyIndex];

    [Header("Game State")]
    [SerializeField] private bool isGameOver = false;
    public bool IsGameOver => isGameOver;
    
    
    void Start()
    {
        isGameOver = false;
        currentDifficultyIndex = 0;
        currentTime = 0f;
        currentDistance = 0f;
        
        StartCoroutine(ChangeDifficulty());
    }

    void Update()
    {
        if (isGameOver) return;
        
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

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        
        StopAllCoroutines();
        Debug.Log("Game Over");
        
        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence()
    {
        yield return new WaitForSeconds(2f);
        
        // disabling any active item
        ItemLogic[] activeItems = FindObjectsByType<ItemLogic>(FindObjectsSortMode.None);
        foreach (ItemLogic item in activeItems)
        {
            item.gameObject.SetActive(false);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload level
    }
    
    
    IEnumerator ChangeDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyManager.difficultyChangeTime);
            
            if(isGameOver) yield break;
            
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

