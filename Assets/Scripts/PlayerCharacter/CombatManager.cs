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
    public int maxplayerforces;
    public int maxemyforces;
    public int playerforces;
    public int emyforces;
    public int battleSituation;
    int state;
    int laststate;
    public static GameObject boss;  
    public GameObject Arena;
    public GameObject AllTroop;
    public GameObject emyEliteTroop_0;
    public GameObject emyEliteTroop_1;
    //public List<BattleGroup> battleGroups = new List<BattleGroup>();
    public List<BattleGroup> battleGroups_0 = new List<BattleGroup>();
    public List<BattleGroup> battleGroups_1 = new List<BattleGroup>();
    public int gameRes;// 1-win 2-lose
    public bool InitReady;
    public bool battlefieldChange;
    float combat_timer;
    public static bool isDuel;
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
        public void EmySetFalse()
        {
            emytroop.SetActive(false);
        }
        public void NpcSetFalse()
        {
            npctroop.SetActive(false);
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
            maxplayerforces = 1900;
            maxemyforces = 1000;        
        }
        playerforces = maxplayerforces;
        emyforces = maxemyforces;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laststate = -2;
        foreach (BattleGroup battleGroup in battleGroups_0)
        {
            battleGroup.Init();
        }
        foreach (BattleGroup battleGroup in battleGroups_1)
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
        if (emyforces > maxemyforces)
        {
            emyforces = maxemyforces;
        }
        if(playerforces > maxplayerforces)
        {
            playerforces = maxplayerforces;
        }
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
        if (!isDuel)
        {
            Combat();
        }   
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
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_1)
                {
                    battleGroup.SetActive();
                    battleGroup.NpcSetFalse();
                }
            }
            else if (state == 1)
            {
                emyEliteTroop_0.SetActive(true);
                emyEliteTroop_1.SetActive(false);
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_1)
                {
                    battleGroup.SetActive();
                    battleGroup.EmySetFalse();
                }
            }
            else
            {
                emyEliteTroop_0.SetActive(false);
                emyEliteTroop_1.SetActive(true);
                foreach (BattleGroup battleGroup in battleGroups_0)
                {
                    battleGroup.SetActive();
                }
                foreach (BattleGroup battleGroup in battleGroups_1)
                {
                    battleGroup.SetActive();
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
                foreach (BattleGroup battleGroup in battleGroups_1)
                {
                    battleGroup.NpcRetreat();
                }
            }
            else if (state == 1)
            {
                emyEliteTroop_0.SetActive(true);
                emyEliteTroop_1.SetActive(false);
                foreach (BattleGroup battleGroup in battleGroups_1)
                {
                    battleGroup.EmyRetreat();
                }
            }
            else
            {
                emyEliteTroop_0.SetActive(false);
                emyEliteTroop_1.SetActive(true);
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
