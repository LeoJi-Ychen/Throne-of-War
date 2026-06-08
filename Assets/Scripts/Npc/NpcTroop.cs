using UnityEngine;
using System.Collections.Generic;
using System;
public class NpcTroop : MonoBehaviour
{
    public GameObject targetTroop;
    public GameObject player;
    public List<GameObject> Npcs = new List<GameObject>();
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
            Npcs.Add(transform.GetChild(i).gameObject);
        }
        state = true;
    }
    private void OnEnable()
    {
        foreach (GameObject n in Npcs)
        {
            n.GetComponent<Npc>().enabled = true;
            n.GetComponent<Npc>().weapon.SetActive(true);
            n.GetComponent<NpcController>().enabled = true;
            n.GetComponent<NpcUI>().enabled = true;
            n.GetComponent<SimpleMove>().enabled = false;
            n.GetComponent<CharacterController>().excludeLayers = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (retreat)
        {
            if (!exit)
            {
                exit = true;
                foreach(GameObject n in Npcs) 
                {
                    n.GetComponent<Npc>().enabled = false;
                    n.GetComponent<Npc>().weapon.SetActive(false);
                    n.GetComponent<Npc>().anim.Play("run");
                    n.GetComponent<NpcController>().enabled = false;
                    n.GetComponent<NpcUI>().enabled = false;
                    n.GetComponent<NpcUI>().canvas.SetActive(false);
                    n.GetComponent<SimpleMove>().enabled = true;
                    n.GetComponent<SimpleMove>().aim = home;
                    n.GetComponent<CharacterController>().excludeLayers = LayerMask.GetMask("Role");
                }
                foreach (GameObject n in targetTroop.GetComponent<EmyTroop>().emys)
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
            if (state && targetTroop.GetComponent<EmyTroop>().state)
            {
                order = true;
                int j = 0;
                for(int i =0;i<Npcs.Count;i++)
                {
                    if(j>= targetTroop.GetComponent<EmyTroop>().emys.Count)
                    {
                        j = 0;
                    }
                    if (j < targetTroop.GetComponent<EmyTroop>().emys.Count)
                    {
                        Npcs[i].GetComponent<Npc>().target = targetTroop.GetComponent<EmyTroop>().emys[j];
                        j++;
                    }  
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
