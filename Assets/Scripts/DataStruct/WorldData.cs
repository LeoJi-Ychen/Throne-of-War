using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

[Serializable]

public class WorldData
{
    public Vector3 camPos;
    public List<Vector3> playtroops = new List<Vector3>();
    public List<Vector3> emytroops  = new List<Vector3>();
    public List<int> playtroopsState = new List<int>();
    public List<int> emytroopsState = new List<int>();
    public List<int> castles = new List<int>();
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
