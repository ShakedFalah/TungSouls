using UnityEngine;
using UnityEngine.Rendering;

public class ItemSpawner : MonoBehaviour
{

    [Header("Lane Settings")]
    [SerializeField] private float spawnZPosition = 40f ; // how far from the player
    [SerializeField] private float[] heightY = {1f, 5f};

    private float[] lanes; // will use the laneSpacing from PlayerController
    
    [Header("Connections")]
    private DifficultySettings difficultySettings;
    [SerializeField] private PlayerController playerController;

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
    }

    void Update()
    {
        if (difficultySettings == null) return;
        
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return; // stop spawning when game over
        
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            string randomItemTag = GetItemTag();
            
            if (!string.IsNullOrEmpty(randomItemTag))
            {
                SpawnRandomItem(randomItemTag);
            }
            
            spawnTimer = difficultySettings.spawnRate;
        }
    }

    private string GetItemTag() // item spawns are based on difficulty probabilities
    {
        if (difficultySettings == null ||
            difficultySettings.obstacleSettings.Count == 0) return null;

        float totalProbability = 0f;
        
        foreach (ObstacleSettings obstacle in difficultySettings.obstacleSettings) // adds every item's probability to the sum total of the probability 
        {
            totalProbability += obstacle.probability;
        }

        float roll = Random.Range(0f, totalProbability); // roll
        float currentProbability = 0f;

        foreach (ObstacleSettings obstacle in difficultySettings.obstacleSettings) // checks where the rolled number landed (which item)
        {
            currentProbability += obstacle.probability;
            if (roll <= currentProbability)
            {
                return obstacle.obstacleTag;
            }
        }
        
        return difficultySettings.obstacleSettings[0].obstacleTag; // if misses everthing, returns the first item
    }

    void SpawnRandomItem(string tagName)
    {
        int randomIndex = Random.Range(0, lanes.Length);
        float spawnXPosition = lanes[randomIndex];
        randomIndex = Random.Range(0, heightY.Length);
        float spawnYPosition = heightY[randomIndex];
        
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition ,spawnZPosition);
        
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
