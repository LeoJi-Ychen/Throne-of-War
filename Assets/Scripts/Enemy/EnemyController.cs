using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public bool elite;
    public GameObject player;
    private CharacterController controller;
    public GameObject target;
    bool stop;
    float speed;
    float state_timer;
    public List<SkinnedMeshRenderer> models = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(2.4f, 4.2f);
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player");
        if (elite)
        {
            GetComponent<Enemy>().maxblood = 9;
            speed = Random.Range(2f, 3.6f);
        }
        foreach (var model in models)
        {
            model.shadowCastingMode = ShadowCastingMode.Off;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (elite)
            {
                if (GetComponent<Enemy>().target != player)
                {
                    GetComponent<Enemy>().target = player;
                    target = player;
                }
            }
            state_timer += Time.deltaTime;
            if (distance(player) > 20)
            {
                if (GetComponent<Enemy>().enabled)
                {
                    GetComponent<Enemy>().Birth();
                    GetComponent<Enemy>().anim.Play("run");
                    GetComponent<Enemy>().enabled = false;
                    GetComponent<EnemyUI>().enabled = false;
                    GetComponent<EnemyUI>().canvas.SetActive(false);
                    GetComponent<Enemy>().weapon.GetComponent<EnemyWeapon>().isAttack = false;
                    foreach (var model in models)
                    {
                        model.shadowCastingMode = ShadowCastingMode.Off;
                    }
                }              
            }
            else
            {
                if (state_timer > 2)
                {
                    state_timer = 0;
                    if (!GetComponent<Enemy>().enabled)
                    {
                        GetComponent<Enemy>().enabled = true;
                        GetComponent<EnemyUI>().enabled = true;
                        GetComponent<Enemy>().behavior_state = 3;
                        stop = false;
                        foreach (var model in models)
                        {
                            model.shadowCastingMode = ShadowCastingMode.On;
                        }
                    }
                }                            
            }
        }
        if(!GetComponent<Enemy>().enabled)
        {
            if(target == null)
            {
                target = GetComponent<Enemy>().target;
            }
            else
            {
                if (distance(target) > 1.2f)
                {
                    if (stop)
                    {
                        stop = false;
                    }
                    transform.LookAt(target.transform.position);
                    controller.Move(transform.forward * speed * Time.deltaTime);
                }
                else
                {
                    if (!stop)
                    {
                        stop = true;
                        GetComponent<Enemy>().anim.Play("atkperformance");
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
