using UnityEngine;
using System.IO;
public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem instance;
    public GameData CurrentData;
    private void Awake()
    {
        instance = this;
        if (File.Exists(Application.persistentDataPath + "/HuntTest.Heller.json"))
        {
            TransitDataToCurrent();
        }
        else
        {
            NewData();
            SaveDataIntoFile();
        }
        
        //NewData();
        //SaveDataIntoFile();
    }
    public void NewData()
    {
        CurrentData = new GameData();
    }
    public GameData TransitDataToCurrent()
    {
        CurrentData = new GameData(DataTransition.MapNameFromFile());
        return CurrentData;
    }
    public void SaveDataIntoFile()
    {
        DataTransition.MapNameToFile(CurrentData);
    }
}