using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void SaveData(string dataPath, Player player,List<CoinData> coins, List<MovableObjectData> movableObj)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(dataPath, FileMode.Create);

        SaveData data = new SaveData(player, coins, movableObj);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadData(string dataPath)
    {
        if(File.Exists(dataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(dataPath, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }else{
            Debug.Log("Save file not found!");
            return null;
        }
    }

    public static void deleteData(string dataPath)
    {
        File.Delete(dataPath);
        File.Delete(dataPath + ".meta");
    }
}
