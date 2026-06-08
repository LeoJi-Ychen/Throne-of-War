using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public bool isAttack;
    public int atkmode;
    public int damage;
    float timer;
    int stage;
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> targets_npc = new List<GameObject>();
    public void EndAttack()
    {
        isAttack = false;
        atkmode = 0;
        stage = 0;
        timer = 0;
        targets.Clear();

    }
    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            GetComponent<BoxCollider>().enabled = true;
            timer += Time.deltaTime;
            if (atkmode == 1)
            {
                if (timer > 1.9f * (1 - 0.3f * (float)stage) && stage < 2)
                {
                    timer = 0;
                    foreach (GameObject target in targets)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Character>().BeAttacked(damage);
                        }
                    }
                    foreach (GameObject target in targets_npc)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Npc>().BeAttacked(1);
                        }
                    }
                    targets.Clear();
                    stage++;
                }
            }
            else if (atkmode == 2)
            {
                if (timer > (1.2f - 0.4f * (float)stage) && stage < 3)
                {
                    timer = 0;
                    foreach (GameObject target in targets)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Character>().BeAttacked(damage);
                        }
                    }
                    foreach (GameObject target in targets_npc)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Npc>().BeAttacked(1);
                        }
                    }
                    targets.Clear();
                    stage++;
                }
            }
            else
            {
                if (timer > 1.1f && stage < 1)
                {
                    timer = 0;
                    foreach (GameObject target in targets)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Character>().BeAttacked(damage);
                        }
                    }
                    foreach (GameObject target in targets_npc)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Npc>().BeAttacked(1);
                        }
                    }
                    targets.Clear();
                    targets_npc.Clear();
                    stage++;
                }
            }
        }
        else
        {
            atkmode = 0;
            stage = 0;
            timer = 0;
            targets.Clear();
            targets_npc.Clear();
            GetComponent<BoxCollider>().enabled = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!targets.Contains(other.gameObject))
            {
                targets.Add(other.gameObject);
            }
            //other.GetComponent<Enemy>().BeAttacked(damage);
        }
        if (other.tag == "NPC")
        {
            if (targets_npc.Count < 1)
            {
                targets_npc.Add(other.gameObject);
            }
            //other.GetComponent<Enemy>().BeAttacked(damage);
        }
    }
}
