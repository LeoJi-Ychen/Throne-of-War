using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static WarManager;
public class WarManager : MonoBehaviour
{
    Camera camera;
    public GameObject LoadingPage;
    public GameObject fog;
    List<GameObject> castles = new();
    public GameObject winscreen;
    public GameObject losescreen;
    public string sceneName = "Battlefield";
    public static bool War;
    public static float Clock;
    public static float TriggerRange;
    public static WorldData Data;
    public static bool HasData;
    public static List<Confrontation> Confrontations = new();
    public static List<int> defeatedTroop_player = new();
    public static List<int> defeatedTroop_enemy = new();
    public static int Result;
    string keyName = "saved";
    public static float MainGameTimer;

   public class Confrontation
    {
        public Vector3 location;
        public List<GameObject> playerUnits = new();
        public List<GameObject> emyUnits = new();
        public float timer;      
    }
    private void Awake()
    {
        EnemyUnit.eid = 0;
        PlayerUnit.pid = 0;
        camera = Camera.main;
        foreach (var c in Confrontations)
        {
            c.playerUnits.Clear();
            c.emyUnits.Clear();
        }
        HasData = false;
        if (PlayerPrefs.HasKey(keyName))
        {
            if (PlayerPrefs.GetInt(keyName) == 1)
            {
                Data = WorldData.LoadStructFromJson();
                if (Data != null)
                {
                    HasData = true;
                    Camera camera = Camera.main;
                    camera.transform.parent.transform.position = Data.camPos;
                    fog.GetComponent<FogOfWar>().explored = new bool[Data.width, Data.height];
                    fog.GetComponent<FogOfWar>().visible = new bool[Data.width, Data.height];
                    for (int x = 0; x < Data.width; x++)
                    {
                        for (int y = 0; y < Data.height; y++)
                        {
                            int index = x + y * Data.width;

                            fog.GetComponent<FogOfWar>().explored[x, y] = Data.explored[index];
                            fog.GetComponent<FogOfWar>().visible[x, y] = Data.visible[index];
                        }
                    }
                }
            }
            else
            {
                PlayerPrefs.SetInt(keyName, 1);
            }
        }
        else
        {
            PlayerPrefs.SetInt(keyName, 1);
        }  
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Clock = 3;
        TriggerRange = 6;
    }
    private void Save()
    {
        WorldData data = new WorldData();
        
        data.camPos = camera.transform.parent.transform.position;
        foreach(GameObject p in PlayerUnit.AllPlayerUnit)
        {
            if( p != null && p.activeSelf)
            {
                TroopData td = new TroopData();
                td.id = p.GetComponent<PlayerUnit>().ID;
                td.pos = p.transform.position;
                td.state = p.GetComponent<PlayerUnit>().troopState;
                data.playertroops.Add(td);
            } 
        }
        foreach (GameObject e in EnemyUnit.AllEnemyUnit)
        {
            if (e != null && e.activeSelf)
            {
                TroopData td = new TroopData();
                td.id = e.GetComponent<EnemyUnit>().ID;
                td.pos = e.transform.position;
                td.state = e.GetComponent<EnemyUnit>().troopState;
                data.emytroops.Add(td);
            }
        }
        int width = fog.GetComponent<FogOfWar>().explored.GetLength(0);
        int height = fog.GetComponent<FogOfWar>().explored.GetLength(1);

        data.width = width;
        data.height = height;

        data.explored = new bool[width * height];
        data.visible = new bool[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + y * width;

                data.explored[index] =
                    fog.GetComponent<FogOfWar>().explored[x, y];

                data.visible[index] =
                    fog.GetComponent<FogOfWar>().visible[x, y];
            }
        }
        data.SaveStructToJson();
        Data = data;
        EnemyUnit.eid = 0;
        PlayerUnit.pid = 0;

    }
    // Update is called once per frame
    void Update()
    {
        if (Result != 0)
        {
            return;
        }
        int playerPoint = 0;
        int enemyPoint = 0;
        MainGameTimer += Time.deltaTime;
        castles = new List<GameObject>(Castle.AllCastle);
        foreach(GameObject c in castles)
        {
            if (c.GetComponent<Castle>().camp == 0)
            {
                enemyPoint++;
            }
            else
            {
                playerPoint++;
            }         
        }
        if (enemyPoint == 0)
        {
            Result = 1;
            PlayerPrefs.SetInt(keyName, 0);
            winscreen.SetActive(true);
        }
        else if (playerPoint == 0)
        {
            Result = 2;
            PlayerPrefs.SetInt(keyName, 0);
            losescreen.SetActive(true);
        }
        for (int i = 0; i < Confrontations.Count; i++)
        {
            foreach(var g in PlayerUnit.AllPlayerUnit)
            {
                if (!Confrontations[i].playerUnits.Contains(g))
                {
                    if (distanceToTarget(Confrontations[i].location, g) < TriggerRange)
                    {
                        Confrontations[i].playerUnits.Add(g);
                        g.GetComponent<PlayerUnitMove>().preWar = true;
                    }
                }                  
            }
            foreach (var g in EnemyUnit.AllEnemyUnit)
            {
                if (!Confrontations[i].emyUnits.Contains(g))
                {
                    if (distanceToTarget(Confrontations[i].location, g) < TriggerRange)
                    {
                        Confrontations[i].emyUnits.Add(g);
                        g.GetComponent<EnemyUnit>().preWar = true;
                    }
                }
            }
        }
        for (int i=0;i<Confrontations.Count;i++)
        {
            Confrontations[i].timer += Time.deltaTime;
            if (Confrontations[i].timer > Clock)
            {
                War = true;
                CombatData data = new CombatData();
                data.playerforce = Confrontations[i].playerUnits.Count * 1000;
                data.emyforce = Confrontations[i].emyUnits.Count * 1000;
                foreach(GameObject p in Confrontations[i].playerUnits)
                {
                    data.playerIdList.Add(p.GetComponent<PlayerUnit>().ID);
                }
                foreach (GameObject e in Confrontations[i].emyUnits)
                {
                    data.emyIdList.Add(e.GetComponent<EnemyUnit>().ID);
                }
                data.SaveStructToJson();
                Confrontations.RemoveAt(i);
                break;
            }
        }
        if(War)
        {
            War = false;
            Save();
            LoadingPage.SetActive(true);
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
    public static void NewWar(GameObject emy,GameObject player)
    {
        Vector3 p = (player.transform.position + emy.transform.position)/2;
        bool b = false;
        for (int i = 0; i < Confrontations.Count; i++)
        {
            if (Vector3.Distance(Confrontations[i].location, p) < 1.5f*TriggerRange)
            {
                b = true;
                break;
            }
        }
        if (!b)
        {
            Confrontation war = new Confrontation();
            war.location = p;
            Confrontations.Add(war);
        }
    }
    float distanceToTarget(Vector3 pos, GameObject target)
    {
        Vector3 t = target.transform.position;
        t.y = pos.y;
        float res = (t - pos).magnitude;
        return res;
    }
}
