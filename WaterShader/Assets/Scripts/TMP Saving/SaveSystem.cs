using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    private static string SavePath = Application.dataPath + "/Scripts/TMP Saving/Data.txt";
    public static void SaveData(Data data)
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
            Debug.Log("Path not found, creating new one");
            return null;
        }
    }
}
