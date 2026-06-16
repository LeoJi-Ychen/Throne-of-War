using UnityEngine;
using System.Collections.Generic;
public class Castle : MonoBehaviour
{
    public static List<GameObject> AllCastle = new List<GameObject>();
    public GameObject flag_red;
    public GameObject flag_yellow;
    public int camp;//1 player
    public float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 10;
    }
    private void OnEnable()
    {
        if (!AllCastle.Contains(this.gameObject))
        {
            AllCastle.Add(this.gameObject);
        }
    }
    private void OnDisable()
    {
        AllCastle.Remove(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (camp == 0)
        {
            flag_red.SetActive(false);
            flag_yellow.SetActive(true);
        }
        else
        {
            flag_red.SetActive(true);
            flag_yellow.SetActive(false);
        }
        if (timer > 10)
        {
            if (camp == 0)
            {
                foreach (GameObject obj in PlayerUnit.AllPlayerUnit)
                {
                    if (distanceToTarget(obj) < 4)
                    {
                        camp = 1;
                        timer = 0;
                        break;
                    }
                }
            }
            else
            {
                foreach (GameObject obj in EnemyUnit.AllEnemyUnit)
                {
                    if (distanceToTarget(obj) < 4)
                    {
                        camp = 0;
                        timer = 0;
                        break;
                    }
                }
            }
        }      
    }
    float distanceToTarget(GameObject target)
    {
        Vector3 t = target.transform.position;
        t.y = transform.position.y;
        float res = (t - transform.position).magnitude;
        return res;
    }
}
