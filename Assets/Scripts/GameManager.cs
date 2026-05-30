using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // to restart the game

public class GameManager : SingletonPersistent<GameManager> // making it a singleton
{
    [SerializeField] private DifficultyManager difficultyManager;
    private HUDManager hudManager;

    private int currentDifficultyIndex;
    private float currentTime = 0f;
    private float currentDistance = 0f;
    private int currentScore = 0;

    public event Action<DifficultySettings> onDifficultyChange;
    public event Action onGameOver;
    public DifficultySettings CurrentDifficulty => difficultyManager.difficultyList[currentDifficultyIndex];

    [Header("Game State")]
    [SerializeField] private bool isGameOver = false;
    public bool IsGameOver => isGameOver;
    private Coroutine difficultyCoroutine;


    void Update()
    {
        if (isGameOver) return;

        currentTime += Time.deltaTime; // time

        currentDistance += CurrentDifficulty.movementSpeed * Time.deltaTime; // distance

        if (hudManager != null)
        {
            hudManager.UpdateHUDFields(currentTime, currentDistance, currentDifficultyIndex + 1);
        }

    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (hudManager != null)
        {
            hudManager.UpdateScoreDisplay(currentScore); // displays the score
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        onGameOver.Invoke();

        Time.timeScale = 0;


        StopAllCoroutines();
        Debug.Log("Game Over");

        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence()
    {
        yield return new WaitForSecondsRealtime(2f);

        // disabling any active item
        ItemLogic[] activeItems = FindObjectsByType<ItemLogic>(FindObjectsSortMode.None);
        foreach (ItemLogic item in activeItems)
        {
            item.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload level
        Time.timeScale = 1;
    }


    IEnumerator ChangeDifficulty()
    {
        while (currentDifficultyIndex < difficultyManager.difficultyList.Count - 1)
        {
            yield return new WaitForSeconds(difficultyManager.difficultyChangeTime);

            if (isGameOver) yield break;

            if (currentDifficultyIndex < difficultyManager.difficultyList.Count - 1)
            {
                currentDifficultyIndex++;
                if (onDifficultyChange != null)
                {
                    onDifficultyChange.Invoke(CurrentDifficulty);
                }
            }
        }
    }

    void Restart()
    {
        // Reset our variables for the fresh scene
        isGameOver = false;
        currentDifficultyIndex = 0;
        currentTime = 0f;
        currentDistance = 0f;
        currentScore = 0;
        Time.timeScale = 1f;

        hudManager = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();

        // Restart the difficulty loop safely
        if (difficultyCoroutine != null)
        {
            StopCoroutine(difficultyCoroutine);
        }
        difficultyCoroutine = StartCoroutine(ChangeDifficulty());

    }

    void OnEnable()
    {
        // Listen for scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to clean up memory
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Restart();
    }
}

