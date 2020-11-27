using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    //For testing
    //private static string SavePath = Application.dataPath + "/Scripts/TMP Saving/Data.txt";
    private static string SavePath = Application.persistentDataPath + "/Data.txt";
    public static void SaveData(Savedata data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath, FileMode.Create);

        GameData gamedata =  new GameData(data);

        formatter.Serialize(stream, gamedata);
        stream.Close();
    }

    public static GameData LoadData()
    {
        if (File.Exists(SavePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePath, FileMode.Open);

            GameData data =  formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }
        else
        {
         //   Debug.Log("Path not found, creating new one");
            return null;
        }
    }

    //JSON
    public static void SaveDataJson(Savedata data)
    {
        using (StreamWriter stream = new StreamWriter(SavePath))
        {
            GameData gamedata = new GameData(data);

            string json = JsonUtility.ToJson(gamedata);
            stream.Write(json);
        }
    }
    public static GameData LoadDataJson()
    {
        if (File.Exists(SavePath))
        {
            using (StreamReader stream = new StreamReader(SavePath))
            {
                string json = stream.ReadToEnd();
                GameData ps = JsonUtility.FromJson<GameData>(json);
           //     Debug.Log("Data Loaded");
                return ps;
            }
        }
        else
        {
        //    Debug.Log("Save file not found in " + SavePath);
            return null;
        }
    }
}
