using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    public List<AudioSource> audio_list = new List<AudioSource>();
    public GameObject audio_charge;
    GameObject player;
    bool play;
    int sound_state;
    float sound_timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (!play)
        {
            if (distance(player) < 20)
            {
                play = true;
                audio_charge.GetComponent<AudioSource>().Stop();
                audio_charge.GetComponent<AudioSource>().Play();
            }
        }
        if (GetComponent<EnemyBoss>().nearDeath|| GetComponent<EnemyBoss>().sitting)
        {
            sound_timer = 0;
        }
        if (distance(player) < 6)
        {
            if (sound_state == 0)
            {
                sound_timer += Time.deltaTime;
                if (sound_timer > 8)
                {
                    sound_state = 1;
                    sound_timer = Random.Range(0f, 2f);
                }
            }
        }
        if (sound_state == 1)
        {
            sound_state = 0;
            if (audio_list.Count > 0)
            {
                audio_list[Random.Range(0, audio_list.Count)].Play();
            }
        }
    }
    float distance(GameObject obj)
    {
        float res = (obj.transform.position - transform.position).magnitude;
        return res;
    }
}
