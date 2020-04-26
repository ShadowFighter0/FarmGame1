using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveLoad
{
    public static void Save<T>(T objectToSave, string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Create))
        {
            formatter.Serialize(fileStream, objectToSave);
        }
        Debug.Log(key + "saved!");
    }

    public static T Load<T>(string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default;
        using (FileStream fileStream = new FileStream(path + key + ".txt", FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(fileStream);
        }
        return returnValue;
    }

    public static bool SaveExists(string key)
    {
        string path = Application.persistentDataPath + "/saves/" + key + ".txt";
        return File.Exists(path);
    }

    public static void DeleteAllData()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }

    public static bool HasSaves()
    {
        return Directory.EnumerateFileSystemEntries(Application.persistentDataPath + "/saves/").Any();
    }
}