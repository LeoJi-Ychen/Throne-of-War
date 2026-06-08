using System.Collections.Generic;
using UnityEngine;

public class NpcWeapon : MonoBehaviour
{
    public bool isAttack;
    public int atkmode;
    public int damage;
    float timer;
    int stage;
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> targets_boss = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void EndAttack()
    {
        isAttack = false;
        atkmode = 0;
        stage = 0;
        timer = 0;
        targets.Clear();
        targets_boss.Clear();

    }
    // Update is called once per frame
    void Update()
    {
        if (isAttack)
        {
            GetComponent<BoxCollider>().enabled = true;
            timer += Time.deltaTime;
            if (atkmode == 1 && (targets.Count > 0 || targets_boss.Count > 0))
            {
                if (timer > 2f * (1 - 0.3f * (float)stage) && stage < 2)
                {
                    timer = 0;
                    foreach (GameObject target in targets)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Enemy>().BeAttacked(damage);
                        }
                    }
                    foreach (GameObject target in targets_boss)
                    {
                        if (target != null)
                        {
                            target.GetComponent<EnemyBoss>().BeAttacked(1);
                        }
                    }
                    stage++;
                }
            }
            else if (atkmode == 2 && (targets.Count > 0 || targets_boss.Count > 0))
            {
                if (timer > (1.2f - 0.4f * (float)stage) && stage < 3)
                {
                    timer = 0;
                    foreach (GameObject target in targets)
                    {
                        if (target != null)
                        {
                            target.GetComponent<Enemy>().BeAttacked(damage);
                        }
                    }
                    foreach (GameObject target in targets_boss)
                    {
                        if (target != null)
                        {
                            target.GetComponent<EnemyBoss>().BeAttacked(1);
                        }
                    }
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
                            target.GetComponent<Enemy>().BeAttacked(damage);
                        }
                    }
                    foreach (GameObject target in targets_boss)
                    {
                        if (target != null)
                        {
                            target.GetComponent<EnemyBoss>().BeAttacked(1);
                        }
                    }
                    targets.Clear();
                    targets_boss.Clear();
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
            targets_boss.Clear();
            GetComponent<BoxCollider>().enabled = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Emy")
        {
            if (targets.Count < 1)
            {
                targets.Add(other.gameObject);
            }
        }
        if (other.tag == "Boss")
        {
            if (targets_boss.Count < 1)
            {
                targets_boss.Add(other.gameObject);
            }
        }
    }
}
