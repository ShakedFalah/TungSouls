using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private Vector3 spawnPosition;
    private float distanceLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceLeft <= 0)
        {
            distanceLeft = distanceToSpawn;
            SpawnRandomObject();
        }

        distanceLeft -= GameManager.Instance.CurrentDifficulty.movementSpeed * Time.deltaTime;
    }

    void SpawnRandomObject()
    {
        List<ObstacleSettings> environmentObjects = GameManager.Instance.CurrentDifficulty.environmentObjects;
        float totalProbability = 0f;
        for (int i = 1; i < environmentObjects.Count; i++)
        {
            totalProbability += environmentObjects[i].probability;
        }

        float randomNumber = Random.Range(0, totalProbability);

        string selectedTag = environmentObjects[0].obstacleTag;
        float currentWeight = 0f;

        foreach (var evnironmentObject in environmentObjects)
        {
            currentWeight += evnironmentObject.probability;

            if (randomNumber <= currentWeight)
            {
                selectedTag = evnironmentObject.obstacleTag;
                break;
            }
        }

        GameObject environmentObjectInstance = TaggedObjectPooler.Instance.GetPooledObject(selectedTag);
        environmentObjectInstance.transform.position = spawnPosition;
        environmentObjectInstance.SetActive(true);
    }
}
