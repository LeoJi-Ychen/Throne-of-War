using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
public class CombatData
{
    public List<int> playerIdList = new List<int>();
    public List<int> emyIdList = new List<int>();

    public int playerforce;
    public int emyforce;
    public static CombatData LoadStructFromJson()
    {
        string path = Application.dataPath + @"/StreamingAssets/Data/" + "Combat" + ".json";
        if (!File.Exists(path))
        {
            return default;
        }
        string jsonString = File.ReadAllText(path);
        CombatData data = JsonUtility.FromJson<CombatData>(jsonString);
        return data;
    }
    public void SaveStructToJson()
    {
        string path = Application.dataPath + @"/StreamingAssets/Data/" + "Combat" + ".json";
        string jsonString = JsonUtility.ToJson(this);
        File.WriteAllText(path, jsonString);
    }
}
