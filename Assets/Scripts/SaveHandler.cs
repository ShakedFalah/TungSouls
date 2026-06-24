using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public struct ObstacleData
{
    public string tag;
    public float positionX;
    public float positionY;
    public float positionZ;
}

[Serializable]
public class SaveData
{
    public float playerPositionX;
    public ObstacleData[] obstaclesData;
    public int difficultyIndex;
    public float time;
    public float distance;
    public int score;
    public string seed;
    public int timesSeedUsed;
    public float magnetDuration;
    public float multiplierDuration;
    public float invincibilityDuration;
    public float multiplierValue;
}

public static class SaveHandler
{
    private readonly static string savePath = Path.Combine(Application.persistentDataPath, "Saves");
    private readonly static string saveFileName = "data.json";
    private readonly static string thumbnailName = "thumbnail.png";

    // This function will save a SaveData object as a JSON file
    public static void SaveToJson(SaveData data, string directoryName)
    {
        // Convert the SaveData directly to JSON format using Newtonsoft.Json
        string json = JsonConvert.SerializeObject(data, Formatting.Indented); // 'Indented' makes the output human-readable

        // Create a file path (this saves the file in the persistent data path of the game)
        string path = Path.Combine(savePath,  directoryName);

        Directory.CreateDirectory(path);

        // Write the JSON data to a file
        File.WriteAllText(Path.Combine(path, saveFileName), json);

        Debug.Log("List of data saved as JSON to: " + path); // For debugging purposes
    }

    // Function to read SaveData from JSON
    public static SaveData ReadFromJson(string directoryName)
    {
        try
        {
            string path = Path.Combine(savePath, directoryName, saveFileName);

            // Read the JSON file content
            string jsonContent = File.ReadAllText(path);

            // Deserialize the JSON to SaveData
            SaveData data = JsonConvert.DeserializeObject<SaveData>(jsonContent);

            Debug.Log($"Successfully read items from {path}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading JSON file: {e.Message}");
            return new SaveData();
        }
    }

    public static ObstacleData[] GetObstacleDataList()
    {
        MovingItemLogic[] movingItemLogics = GameObject.FindObjectsByType<MovingItemLogic>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        ObstacleData[] obstacleDatas = new ObstacleData[movingItemLogics.Length];
        for (int i = 0; i < movingItemLogics.Length; i++)
        {
            obstacleDatas[i] = new ObstacleData()
            {
                positionX = movingItemLogics[i].transform.position.x,
                positionY = movingItemLogics[i].transform.position.y,
                positionZ = movingItemLogics[i].transform.position.z,
                tag = movingItemLogics[i].GetPoolTag(),
            };
        }

        return obstacleDatas;
    }

    public static void SaveThumbnail(Texture2D image, string directoryName)
    {
        var bytes = image.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(savePath, directoryName, thumbnailName), bytes);
    }

    public static Texture2D LoadThumbnail(string directoryName)
    {
        Texture2D tex = null;
        byte[] fileData;
        string path = Path.Combine(savePath, directoryName, thumbnailName);

        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return tex;
    }

    public static string[] GetSaveNames()
    {
        string[] paths = Directory.GetDirectories(savePath);
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = Path.GetFileName(paths[i]);
        }
        return paths;
    }
}