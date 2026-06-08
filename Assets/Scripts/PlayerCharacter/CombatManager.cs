using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Data;
public class CombatManager : MonoBehaviour
{
    public static bool Fighting;
    public GameObject audio_win;
    public GameObject audio_loss;
    public GameObject audio_horn;
    public int playerforces;
    public int emyforces;
    public int battleSituation;
    int state;
    int laststate;
    public GameObject emyEliteTroop_0;
    public GameObject emyEliteTroop_1;
    public List<BattleGroup> battleGroups = new List<BattleGroup>();
    public List<BattleGroup> battleGroups_0 = new List<BattleGroup>();
    public List<BattleGroup> battleGroups_p = new List<BattleGroup>();
    public List<BattleGroup> battleGroups_e = new List<BattleGroup>();
    public int gameRes;// 1-win 2-lose
    public bool InitReady;
    public bool battlefieldChange;
    float combat_timer;

    [Serializable]
    public  struct BattleGroup
    {
        public GameObject npctroop;
        public GameObject emytroop;
        public void SetActive()
        {
            npctroop.SetActive(true);
            emytroop.SetActive(true);
        }
        public void SetFalse()
        {
            npctroop.SetActive(false);
            emytroop.SetActive(false);
        }
        public void EmyRetreat()
        {
            emytroop.GetComponent<EmyTroop>().retreat = true;
        }
        public void NpcRetreat()
        {
            npctroop.GetComponent<NpcTroop>().retreat = true;
        }
        public void Init()
        {
            npctroop.GetComponent<NpcTroop>().targetTroop = emytroop;
            emytroop.GetComponent<EmyTroop>().targetTroop = npctroop;
        }
    }
    private void Awake()
    {
        CombatData data = LoadStructFromJson<CombatData>("combatdata");
        if(data != null )
        {

        }
        else
        {
            playerforces = 1000;
            emyforces = 1000;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laststate = -2;
        foreach (BattleGroup battleGroup in battleGroups)
        {
            battleGroup.Init();
        }
        foreach (BattleGroup battleGroup in battleGroups_0)
        {
            battleGroup.Init();
        }
        foreach (BattleGroup battleGroup in battleGroups_p)
        {
            battleGroup.Init();
        }
        foreach (BattleGroup battleGroup in battleGroups_e)
        {
            battleGroup.Init();
        }
        StateManage();
        SetBattleTroop();
        InitReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InitReady)
        {
            return;
        }
        if (playerforces / Mathf.Max(1, emyforces) > 2)
        {
            battleSituation = 100;
        }
        else if (emyforces / Mathf.Max(1,playerforces) > 2)
        {
            battleSituation = -100;
        }
        else
        {
            battleSituation = 0;
        }
        //battleSituation = (playerforces - emyforces) / 10;
        StateManage();
        SetBattleTroopInCombat();
        Judgement();
        Combat();
    }
    void StateManage()
    {
        if (battleSituation < -50)
        {
            state = -1;
        }
        else if (battleSituation > 50)
        {
            state = 1;
        }
        else
        {
            state = 0;
        }
    }
    void SetBattleTroop()
    {
        if (state != laststate)
        {
            if (state == -1)
            {
                emyEliteTroop_0.SetActive(true);
                emyEliteTroop_1.SetActive(true);
                foreach (BattleGroup battleGroup in battleGroups_e)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetFalse();
                }
                foreach (BattleGroup battleGroup in battleGroups_p)
                {
                    battleGroup.SetFalse();
                }
            }
            else if (state == 1)
            {
                emyEliteTroop_0.SetActive(true);
                emyEliteTroop_1.SetActive(false);
                foreach (BattleGroup battleGroup in battleGroups_e)
                {
                    battleGroup.SetFalse();
                }
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetFalse();
                }
                foreach (BattleGroup battleGroup in battleGroups_p)
                {
                    battleGroup.SetActive();
                }
            }
            else
            {
                emyEliteTroop_0.SetActive(false);
                emyEliteTroop_1.SetActive(true);
                foreach (BattleGroup battleGroup in battleGroups_e)
                {
                    battleGroup.SetFalse();
                }
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_p)
                {
                    battleGroup.SetFalse();
                }
            }
            laststate = state;
        }
    }
    void SetBattleTroopInCombat()
    {
        if (state != laststate)
        {
            battlefieldChange = true;
            audio_horn.GetComponent<AudioSource>().Stop();
            audio_horn.GetComponent<AudioSource>().Play();
            if (state == -1)
            {
                emyEliteTroop_0.SetActive(true);
                emyEliteTroop_1.SetActive(true);
                foreach (BattleGroup battleGroup in battleGroups_e)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.NpcRetreat();
                }
                foreach (BattleGroup battleGroup in battleGroups_p)
                {
                    battleGroup.NpcRetreat();
                }
            }
            else if (state == 1)
            {
                emyEliteTroop_0.SetActive(true);
                emyEliteTroop_1.SetActive(false);
                foreach (BattleGroup battleGroup in battleGroups_e)
                {
                    battleGroup.EmyRetreat();
                }
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.EmyRetreat();
                }
                foreach (BattleGroup battleGroup in battleGroups_p)
                {
                    battleGroup.SetActive();
                }
            }
            else
            {
                emyEliteTroop_0.SetActive(false);
                emyEliteTroop_1.SetActive(true);
                foreach (BattleGroup battleGroup in battleGroups_e)
                {
                    battleGroup.EmyRetreat();
                }
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_p)
                {
                    battleGroup.NpcRetreat();
                }
            }
            laststate = state;
        }
    }
    void Judgement()
    {
        if (gameRes==0)
        {
            if (playerforces <= 0)
            {
                gameRes = 2;
                audio_loss.GetComponent<AudioSource>().Play();
            }
            else if(emyforces<=0)
            {
                gameRes = 1;
                audio_win.GetComponent<AudioSource>().Play();
            }
        }
    }
    public T LoadStructFromJson<T>(string filePath)
    {
        string path = Application.dataPath + @"/StreamingAssets/Data" + filePath + ".json";
        if (!File.Exists(path))
        {
            return default;
        }
        string jsonString = File.ReadAllText(path);
        T data = JsonUtility.FromJson<T>(jsonString);
        return data;
    }
    public void SaveStructToJson<T>(T data, string filePath)
    {
        string path = Application.dataPath + @"/StreamingAssets/Data" + filePath + ".json";
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonString);
    }
    void Combat()
    {
        if (Fighting)
        {
            combat_timer += Time.deltaTime;
        }      
        if (state == -1)
        {        
            if(combat_timer > 1)
            {
                combat_timer = 0;
                playerforces -= 2;
                emyforces -= 1;
            }
        }
        else if(state == 1)
        {
            if (combat_timer > 1)
            {
                combat_timer = 0;
                playerforces -= 1;
                emyforces -= 2;
            }
        }
        else
        {
            if (combat_timer > 1)
            {
                combat_timer = 0;
                playerforces -= 1;
                emyforces -= 1;
            }
        }
    }
}
