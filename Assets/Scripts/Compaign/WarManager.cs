using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static WarManager;
public class WarManager : MonoBehaviour
{
    Camera camera;
    public List<GameObject> castles = new();
    public GameObject winscreen;
    public GameObject losescreen;
    public string sceneName = "Battlefield";
    public static bool War;
    public static float Clock;
    public static float TriggerRange;
    public static WorldData data;
    public static bool HasData;
    public static List<Confrontation> Confrontations = new();
    public static int Result;
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
        data = WorldData.LoadStructFromJson();
        if(data != null )
        {
            HasData = true;
            Camera camera = Camera.main;
            camera.transform.parent.transform.position = data.camPos;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Clock = 10;
        TriggerRange = 5;
    }
    private void Save()
    {
        WorldData data = new WorldData();
        
        data.camPos = camera.transform.parent.transform.position;
        foreach(GameObject p in PlayerUnit.AllPlayerUnit)
        {
            if( p != null && p.activeSelf)
            {
                data.playtroops.Add(p.transform.position);
                data.playtroopsState.Add(p.GetComponent<PlayerUnit>().troopState);
            } 
        }
        foreach (GameObject e in EnemyUnit.AllEnemyUnit)
        {
            if (e != null && e.activeSelf)
            {
                data.emytroops.Add(e.transform.position);
                data.emytroopsState.Add(e.GetComponent<EnemyUnit>().troopState);
            }
        }
        data.SaveStructToJson();
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
            if (enemyPoint == 0)
            {
                Result = 1;
                winscreen.SetActive(true);
            }
            if (playerPoint == 0)
            {
                Result = 2;
                losescreen.SetActive(true);
            }
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
                data.SaveStructToJson();
                Confrontations.RemoveAt(i);
                break;
            }
        }
        if(War)
        {
            War = false;
            Save();
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
