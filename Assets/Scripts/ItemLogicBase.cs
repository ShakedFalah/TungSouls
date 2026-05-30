using UnityEngine;

public class ItemLogicBase : MonoBehaviour
{
    [Header("Connections")]
    private DifficultySettings difficultySettings;
    [SerializeField] private float despawnDistance = -8f;

    [SerializeField] protected string poolTag; // for returning objects to the pool
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

    private void updateDifficulty(DifficultySettings newDifficultySettings)
    {
        difficultySettings = newDifficultySettings;
    }

    private void OnEnable()
    {
        updateDifficulty(GameManager.Instance.CurrentDifficulty);
        GameManager.Instance.onDifficultyChange += updateDifficulty;
    }

    private void OnDisable()
    {
        GameManager.Instance.onDifficultyChange -= updateDifficulty;
    }

}
