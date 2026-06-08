using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NpcController : MonoBehaviour
{
    public GameObject audio_encourage;
    public GameObject audio_charge;
    public GameObject player;
    private CharacterController controller;
    public GameObject target;
    bool stop;
    float speed;
    int sound_state;
    float sound_looptimer;
    float sound_timer;
    float state_timer;
    public List<SkinnedMeshRenderer> models = new();
    bool charge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(2.1f, 3.6f);
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player");
        sound_looptimer = Random.Range(0.5f, 2f);
        sound_looptimer -= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        state_timer += Time.deltaTime;
        if (player != null)
        {
            if (player.GetComponent<Commander>().OrderID != 1)
            {
                GetComponent<Npc>().enabled = false;
                GetComponent<Npc>().anim.Play("encourage");
                if (distance(player) > 20)
                {
                    if (GetComponent<NpcUI>().enabled)
                    {
                        GetComponent<NpcUI>().enabled = false;
                        GetComponent<NpcUI>().canvas.SetActive(false);
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
                        if (!GetComponent<NpcUI>().enabled)
                        {
                            GetComponent<NpcUI>().enabled = true;
                            foreach (var model in models)
                            {
                                model.shadowCastingMode = ShadowCastingMode.On;
                            }
                        }
                    }                   
                }
                if (sound_state == 0)
                {
                    sound_looptimer += Time.deltaTime;
                    if (sound_looptimer > 2)
                    {
                        sound_looptimer = 0;
                        audio_encourage.GetComponent<AudioSource>().Play();
                    }
                }
                return;
            }
            if (sound_state==0)
            {
                sound_state = 1;
                audio_encourage.GetComponent<AudioSource>().Stop();
            }
            else if (sound_state == 1)
            {
                sound_timer += Time.deltaTime;
                if (sound_timer > 2)
                {
                    sound_state = 2;
                    audio_charge.GetComponent<AudioSource>().Play();
                }
            }
            if (distance(player) > 20)
            {
                if (!charge)
                {
                    charge = true;
                    GetComponent<Npc>().anim.Play("run");
                }
                if (GetComponent<Npc>().enabled)
                {
                    GetComponent<Npc>().Birth();
                    GetComponent<Npc>().anim.Play("run");
                    GetComponent<Npc>().enabled = false;
                    GetComponent<NpcUI>().enabled = false;
                    GetComponent<NpcUI>().canvas.SetActive(false);
                    GetComponent<Npc>().weapon.GetComponent<NpcWeapon>().isAttack = false;
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
                    if (!charge)
                    {
                        charge = true;
                        GetComponent<Npc>().anim.Play("run");
                    }
                    if (!GetComponent<Npc>().enabled)
                    {
                        GetComponent<Npc>().behavior_state = 3;
                        GetComponent<Npc>().enabled = true;
                        GetComponent<NpcUI>().enabled = true;
                        stop = false;
                        foreach (var model in models)
                        {
                            model.shadowCastingMode = ShadowCastingMode.On;
                        }
                    }
                }                               
            }
        }
        if (!GetComponent<Npc>().enabled)
        {
            if (target == null)
            {
                target = GetComponent<Npc>().target;
            }
            else
            {
                if (distance(target) > 1.5f)
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
                        GetComponent<Npc>().anim.Play("atkperformance");
                        CombatManager.Fighting = true;
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
