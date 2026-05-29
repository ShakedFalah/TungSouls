using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [SerializeField] private float timeToSpawn;
    private float spawnCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown <= 0)
        {
            spawnCooldown = timeToSpawn;
            SpawnRandomObject();
        }

        spawnCooldown -= Time.deltaTime;
    }

    void SpawnRandomObject()
    {
        List<ObstacleSettings> environmentObjects = GameManager.Instance.CurrentDifficulty.environmentObjects;
        float[] totalProbabilities = new float[environmentObjects.Count];
        totalProbabilities[0] = environmentObjects[0].probability;
        for (int i = 1; i < environmentObjects.Count; i++)
        {
            totalProbabilities[i] = environmentObjects[i].probability + totalProbabilities[i - 1];
        }
    }
}
