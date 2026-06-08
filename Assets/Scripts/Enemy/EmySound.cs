using UnityEngine;
using System.Collections.Generic;
public class EmySound : MonoBehaviour
{
    public List<AudioSource> audio_list = new List<AudioSource>();
    GameObject player;
    int sound_state;
    float sound_timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        sound_timer = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (GetComponent<Enemy>().nearDeath)
        {
            sound_timer = 0;
        }
        if (distance(player) < 8)
        {
            if (sound_state == 0)
            {
                sound_timer += Time.deltaTime;
                if(sound_timer > 6)
                {
                    sound_state = 1;
                    sound_timer = Random.Range(0f,2f);
                }
            }
        }
        if(sound_state == 1)
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
