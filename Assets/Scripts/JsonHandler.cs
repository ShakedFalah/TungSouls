using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public struct ObstacleData
{
    public string tag;
    public Vector3 position;
}

[Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public ObstacleData[] obstaclesData;
    public int difficultyIndex;
    public float time;
    public float distance;
    public int score;
    public string seed;
    public int timesSeedUsed;
}

public static class JsonHandler
{
    // This function will save a SaveData object as a JSON file
    public static void SaveToJson(SaveData data, string fileName)
    {
        // Convert the SaveData directly to JSON format using Newtonsoft.Json
        string json = JsonConvert.SerializeObject(data, Formatting.Indented); // 'Indented' makes the output human-readable

        // Create a file path (this saves the file in the persistent data path of the game)
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        // Write the JSON data to a file
        File.WriteAllText(path, json);

        Debug.Log("List of data saved as JSON to: " + path); // For debugging purposes
    }

    // Function to read SaveData from JSON
    public static SaveData ReadFromJson(string filePath)
    {
        try
        {
            // Read the JSON file content
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize the JSON to SaveData
            SaveData items = JsonConvert.DeserializeObject<SaveData>(jsonContent);

            Debug.Log($"Successfully read items from {filePath}");
            return items;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading JSON file: {e.Message}");
            return new SaveData();
        }
    }

    public static SaveData CreateSaveData()
    {
        SaveData data = new SaveData();
        data.score = 0;
        data.time = 0;
        data.distance = 0;
        data.difficultyIndex = 0;
        data.timesSeedUsed = 0;
        data.seed = "";
        data.playerPosition = Vector3.zero;
        data.obstaclesData = new ObstacleData[0];

        return data;
    }

    public static ObstacleData[] GetObstacleDataList()
    {
        MovingItemLogic[] movingItemLogics = GameObject.FindObjectsByType<MovingItemLogic>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        ObstacleData[] obstacleDatas = new ObstacleData[movingItemLogics.Length];
        for (int i = 0; i < movingItemLogics.Length; i++)
        {
            obstacleDatas[i] = new ObstacleData()
            {
                position = movingItemLogics[i].transform.position,
                tag = movingItemLogics[i].GetPoolTag(),
            };
        }

        return obstacleDatas;
    }
}