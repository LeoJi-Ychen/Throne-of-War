using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmyTroop : MonoBehaviour
{
    public GameObject targetTroop;
    public GameObject player;
    public List<GameObject> emys = new List<GameObject>();
    public bool state;
    public bool order;
    public GameObject home;
    bool exit;
    public bool retreat;
    float retreat_timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        home.transform.parent = null;
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        for (int i = 0; i < transform.childCount; i++)
        {
            emys.Add(transform.GetChild(i).gameObject);
        }        
        state = true;
        foreach (GameObject n in emys)
        {
            n.GetComponent<Enemy>().enabled = true;
            n.GetComponent<Enemy>().weapon.SetActive(true);
            n.GetComponent<EnemyController>().enabled = true;
            n.GetComponent<EnemyUI>().enabled = true;
            n.GetComponent<SimpleMove>().enabled = false;
            n.GetComponent<CharacterController>().excludeLayers = 0;
            n.GetComponent<Enemy>().army = this.gameObject;
        }
    }
    private void OnEnable()
    {
        foreach (GameObject n in emys)
        {
            n.GetComponent<Enemy>().enabled = true;
            n.GetComponent<Enemy>().weapon.SetActive(true);
            n.GetComponent<EnemyController>().enabled = true;
            n.GetComponent<EnemyUI>().enabled = true;
            n.GetComponent<SimpleMove>().enabled = false;
            n.GetComponent<CharacterController>().excludeLayers = 0;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (targetTroop == null)
        {
            return;
        }
        if (retreat)
        {
            if (!exit)
            {
                exit = true;
                foreach (GameObject n in emys)
                {
                    n.GetComponent<Enemy>().enabled = false;
                    n.GetComponent<Enemy>().weapon.SetActive(false);
                    n.GetComponent<Enemy>().anim.Play("run");
                    n.GetComponent<EnemyController>().enabled = false;
                    n.GetComponent<EnemyUI>().enabled = false;
                    n.GetComponent<EnemyUI>().canvas.SetActive(false);
                    n.GetComponent<SimpleMove>().enabled = true;
                    n.GetComponent<SimpleMove>().aim = home;
                    n.GetComponent<CharacterController>().excludeLayers = LayerMask.GetMask("Role");
                    CombatManager cm = player.GetComponent<CombatManager>();
                    targetTroop.GetComponent<NpcTroop>().targetTroop =
                        cm.battleGroups_0[Random.Range(0, cm.battleGroups_0.Count)].emytroop;
                    targetTroop.GetComponent<NpcTroop>().SetSoldier();
                }
            }
            retreat_timer += Time.deltaTime;
            if (retreat_timer > 10)
            {
                this.gameObject.SetActive(false);
            }
            return;
        }
        if (!order)
        {
            if (state && targetTroop.GetComponent<NpcTroop>().state)
            {
                order = true;
                int j = 0;
                for (int i = 0; i < emys.Count; i++)
                {
                    if (j >= targetTroop.GetComponent<NpcTroop>().Npcs.Count)
                    {
                        j = 0;
                    }
                    if (j < targetTroop.GetComponent<NpcTroop>().Npcs.Count)
                    {
                        emys[i].GetComponent<Enemy>().target = targetTroop.GetComponent<NpcTroop>().Npcs[j];
                        j++;
                    }
                }
            }
        }
    }
    public void SetSoldier()
    {
        if (state && targetTroop.GetComponent<NpcTroop>().state)
        {
            int j = 0;
            for (int i = 0; i < emys.Count; i++)
            {
                if (j >= targetTroop.GetComponent<NpcTroop>().Npcs.Count)
                {
                    j = 0;
                }
                if (j < targetTroop.GetComponent<NpcTroop>().Npcs.Count)
                {
                    emys[i].GetComponent<Enemy>().target = targetTroop.GetComponent<NpcTroop>().Npcs[j];
                    j++;
                }
            }
        }
    }
    float distance(GameObject obj)
    {
        float res = (obj.transform.position - transform.position).magnitude;
        return res;
    }
}
