using UnityEngine;
using UnityEngine.Rendering;

public class ItemSpawner : MonoBehaviour
{

    [Header("Lane Settings")]
    [SerializeField] private float spawnZPosition = 40f ; // how far from the player
    [SerializeField] private float[] heightY = {1f, 5f};

    private float[] lanes; 
    
    [Header("Connections")]
    private DifficultySettings difficultySettings;
    [SerializeField] private PlayerController playerController;

    [Header("Pattern Settings")]
    [SerializeField] private LevelPatternSO[] availablePatterns;
    
    private LevelPatternSO currentPattern;
    private int currentPatternIndex = 0;
    private int currentRowIndex = 0;
    
    private float spawnTimer;
    
    void Start()
    {
        updateDifficulty(GameManager.Instance.CurrentDifficulty);
        if (playerController != null)
        {
            lanes = playerController.lanesPositions; // takes the array of lanes from the player
        }
        else
        {
            Debug.LogError("You forgot to drag the player into the Spawner slot");
        }

        if (difficultySettings != null)
        {
            spawnTimer = difficultySettings.spawnRate;
        }

        GameManager.Instance.onDifficultyChange += updateDifficulty;

        PickNextPattern();
    }

    void Update()
    {
        if (difficultySettings == null || currentPattern == null) return;
        
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return; // stop spawning when game over
        
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnPatternRow(currentPattern.rows[currentRowIndex]);
            currentRowIndex++;

            if (currentRowIndex >= currentPattern.rows.Length)
            {
                PickNextPattern();
            }
            
            // faster row spawning
            float calculatedSpawnRate = currentPattern.distanceBetweenRows / difficultySettings.movementSpeed;
            spawnTimer = calculatedSpawnRate;
        }
    }

    private void PickNextPattern()
    {
        if (availablePatterns == null || availablePatterns.Length == 0)
        {
            Debug.LogWarning("No Level Patterns assigned to the ItemSpawner!");
            return;
        }

        // pick random pattern
        int patternRoll = Random.Range(0, availablePatterns.Length);
        currentPattern = availablePatterns[patternRoll];
        
        currentRowIndex = 0;
    }
    
    private void SpawnPatternRow(PatternRow rowData)
    {
        // left lane
        TrySpawnPooledItem(rowData.lane1Low, lanes[0], heightY[0]);
        TrySpawnPooledItem(rowData.lane1High, lanes[0], heightY[1]);
        
        // middle lane
        TrySpawnPooledItem(rowData.lane2Low, lanes[1], heightY[0]);
        TrySpawnPooledItem(rowData.lane2High, lanes[1], heightY[1]);
        
        // right lane
        TrySpawnPooledItem(rowData.lane3Low, lanes[2], heightY[0]);
        TrySpawnPooledItem(rowData.lane3High, lanes[2], heightY[1]);
    }

    private void TrySpawnPooledItem(ItemType tagName, float xPos, float yPos)
    {
        if(tagName == ItemType.None) return; // skip if blank
        
        Vector3 spawnPosition = new Vector3(xPos, yPos, playerController.transform.position.z + spawnZPosition);
        GameObject spawnedItem = TaggedObjectPooler.Instance.GetPooledObject(tagName);

        if (spawnedItem != null)
        {
            spawnedItem.transform.position = spawnPosition;
            spawnedItem.SetActive(true);
        }
    }

    private void updateDifficulty(DifficultySettings newDifficultySettings)
    {
        this.difficultySettings = newDifficultySettings;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onDifficultyChange -= updateDifficulty;
    }
}
