using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    private float gravity = -9.81f;
    private float velocityY;
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
    private void OnEnable()
    {
        stop = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Enemy>().enabled)
        {
            Gravity();
        }
        target = GetComponent<Enemy>().target;
        if(player != null)
        {
            float sq_dis = sqdistance(player);
            if (elite)
            {
                if (GetComponent<Enemy>().target != player)
                {
                    GetComponent<Enemy>().target = player;
                    target = player;
                }
            }
            state_timer += Time.deltaTime;
            if (sq_dis > 225)
            {
                if (GetComponent<Enemy>().enabled)
                {
                    GetComponent<Enemy>().Birth();
                    GetComponent<Enemy>().anim.Play("run");
                    GetComponent<Enemy>().enabled = false;
                    GetComponent<EnemyUI>().enabled = false;
                    GetComponent<EnemyUI>().canvas.SetActive(false);
                    GetComponent<Enemy>().weapon.GetComponent<EnemyWeapon>().isAttack = false;
                    GetComponent<EmySound>().enabled = false;
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
                        GetComponent<EmySound>().enabled = true;
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
                if (sqdistance(target) > 1.44f)
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
    float sqdistance(GameObject obj)
    {
        float res = (obj.transform.position - transform.position).sqrMagnitude;
        return res;
    }
    void Gravity()
    {
        velocityY += gravity * Time.deltaTime;

        controller.Move(
            Vector3.up * velocityY * Time.deltaTime
        );
    }
}
