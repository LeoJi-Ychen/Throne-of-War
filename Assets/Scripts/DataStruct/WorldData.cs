using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[Serializable]

public class WorldData
{
    public Vector3 camPos;
    public List<TroopData> playertroops = new();
    public List<TroopData> emytroops  = new();
    public List<int> castles = new List<int>();
    public bool[] explored;
    public bool[] visible;

    public int width;
    public int height;
    public static WorldData LoadStructFromJson()
    {
        string path = Application.dataPath + @"/StreamingAssets/Data/" + "World" + ".json";
        if (!File.Exists(path))
        {
            return default;
        }
        string jsonString = File.ReadAllText(path);
        WorldData data = JsonUtility.FromJson<WorldData>(jsonString);
        return data;
    }
    public void SaveStructToJson()
    {
        string path = Application.dataPath + @"/StreamingAssets/Data/" + "World" + ".json";
        string jsonString = JsonUtility.ToJson(this);
        File.WriteAllText(path, jsonString);
    }
}

[Serializable]
public class TroopData
{
    public int id;
    public Vector3 pos;
    public int state;
}
