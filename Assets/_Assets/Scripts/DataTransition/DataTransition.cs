// Script which save some data in the file inside the folder of game
using UnityEngine;
using System.IO;
public static class DataTransition
{
    public static void MapNameToFile(GameData gameData)
    {
        string path = Application.persistentDataPath + "/HuntTest.Heller.json";
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, jsonData);
    }
    public static GameData MapNameFromFile()
    {
        string path = Application.persistentDataPath + "/HuntTest.Heller.json";

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
            return gameData;
        }
        Debug.LogError("File doesn't exist");
        return null;
    }
}