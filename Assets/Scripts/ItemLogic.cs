using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    [Header("Connections")]
    private DifficultySettings difficultySettings;
    [SerializeField] private float despawnDistance = -8f;
    
    [SerializeField] private string poolTag; // for returning objects to the pool
    private void Start()
    {
        difficultySettings = GameManager.Instance.CurrentDifficulty;
    }
    void Update()
    {
        if (difficultySettings == null) return;
        
        transform.position += Vector3.back * difficultySettings.movementSpeed * Time.deltaTime; // moves towards the screen / player

        if (transform.position.z < despawnDistance)
        {
            gameObject.SetActive(false);
            TaggedObjectPooler.Instance.ReturnObject(gameObject, poolTag);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            OnPlayerHit(other.gameObject);
        }
    }

    protected virtual void OnPlayerHit(GameObject player) // will change depending on the item / obstacle collided
    {
        TaggedObjectPooler.Instance.ReturnObject(gameObject, poolTag);
    }

    private void updateDifficulty(DifficultySettings newDifficultySettings)
    {
        this.difficultySettings = newDifficultySettings;
    }

    private void OnEnable()
    {
        difficultySettings = GameManager.Instance.CurrentDifficulty;
        GameManager.Instance.onDifficultyChange += updateDifficulty;
    }

    private void OnDisable()
    {
        GameManager.Instance.onDifficultyChange -= updateDifficulty;
    }
}
