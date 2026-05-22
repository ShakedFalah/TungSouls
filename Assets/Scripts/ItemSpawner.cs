using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [Header("Lane Settings")]
    [SerializeField] private float spawnZPosition = 40f ; // how far from the player
    [SerializeField] private float[] heightY = {1f, 5f};

    private float[] lanes; // will use the laneSpacing from PlayerController
    
    [Header("Connections")]
    [SerializeField] private DifficultySettings difficultySettings;
    [SerializeField] private PlayerController playerController;

    private float spawnTimer;
    
    void Start()
    {
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
    }

    void Update()
    {
        if (difficultySettings == null) return;
        
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnRandomItem();
            spawnTimer = difficultySettings.spawnRate;
        }
    }

    void SpawnRandomItem()
    {
        int randomIndex = Random.Range(0, lanes.Length);
        float spawnXPosition = lanes[randomIndex];
        randomIndex = Random.Range(0, heightY.Length);
        float spawnYPosition = heightY[randomIndex];
        
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnYPosition ,spawnZPosition);
        
        GameObject coin = TaggedObjectPooler.Instance.GetPooledObject("Coin");

        if (coin != null)
        {
            coin.transform.position = spawnPosition;
            coin.SetActive(true);
        }
    }
}
