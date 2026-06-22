using UnityEngine;

public class MovingItemLogic : MonoBehaviour
{
    [Header("Connections")]
    private DifficultySettings difficultySettings;
    [SerializeField] private float despawnDistance = -8f;
    [SerializeField] protected string poolTag; // for returning objects to the pool
    
    [Header("Magnet Pull Settings")]
    [SerializeField] private bool isMagnetable = false; // only on for items that arent affected by magnet the player
    
    private PlayerController playerScript;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerController>();
        }
    }
    
    void Update()
    {
        if (isMagnetable && playerScript != null && playerScript.magnetDuration > 0) // activating the magnet pull
        {
            float zDistanceToPlayer = transform.position.z - playerScript.transform.position.z;

            if (zDistanceToPlayer > 0 && zDistanceToPlayer <= playerScript.magnetSettings.pullDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerScript.transform.position, playerScript.magnetSettings.pullSpeed * Time.deltaTime);
                return; // skips the forward movement
            }
        }
        
        if (difficultySettings == null) return;
        
        transform.position += Vector3.back * difficultySettings.movementSpeed * Time.deltaTime; // moves towards the screen / player

        if (transform.position.z < despawnDistance)
        {
            Despawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerHit(other.gameObject);
        }
    }

    // collision
    protected virtual void OnPlayerHit(GameObject player) // will change depending on the item / obstacle collided
    {
        Despawn();
    }
    
    // despawn logic
    protected void Despawn()
    {
        gameObject.SetActive(false);
        if (TaggedObjectPooler.Instance != null)
        {
            TaggedObjectPooler.Instance.ReturnObject(gameObject, poolTag);
        }
    }
    
    private void updateDifficulty(DifficultySettings newDifficultySettings)
    {
        if (newDifficultySettings == null) return; // safety check
        difficultySettings = newDifficultySettings;
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentDifficulty != null)
        {
            updateDifficulty(GameManager.Instance.CurrentDifficulty);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.onDifficultyChange += updateDifficulty;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onDifficultyChange -= updateDifficulty;
        }
    }

    public string GetPoolTag()
    {
        return poolTag;
    }

}
