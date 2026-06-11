using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public static Action<float> getDamage;
    public GameObject audio_hitted;
    public GameObject audio_hit;
    public GameObject weapon;
    public bool isAttacking;
    public int atkmode;
    public bool isDodge;
    public int maxblood;
    public int blood;
    public int damage;
    public bool hitted;
    public bool hitted_state;
    public bool nearDeath;
    public bool death;
    public float dead_timer;
    public bool invincible;
    float invincible_timer;
    float skill_timer;
    float skill_cd;
    public bool single;
    private void Awake()
    {
        maxblood = 100;
        blood = maxblood;
        damage = 10;
        skill_cd = 30;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapon.GetComponent<CharacterWeapon>().audio_hit = audio_hit;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CombatManager.isDuel)
        {
            skill_timer += Time.deltaTime;
        }
        if (skill_timer > skill_cd) 
        {
            if (CombatManager.boss != null&&!CombatManager.isDuel)
            {
                single = true;
                if (Keyboard.current.vKey.wasPressedThisFrame)
                {
                    skill_timer = 0;
                    CombatManager.isDuel = true;
                    GetComponent<CombatManager>().AllTroop.SetActive(false);
                    GetComponent<CombatManager>().Arena.SetActive(true);
                }
            }
            else
            {
                single = false;
            }
        }
        else
        {
            single = false;
        }
        if (invincible)
        {
            invincible_timer += Time.deltaTime;
            if (invincible_timer > 3.8f)
            {
                invincible_timer = 0;
                invincible = false;
            }
        }
        if (blood <= 0)
        {
            if(CombatManager.isDuel)
            {
                blood = 1;
                CombatManager.isDuel = false;
                GetComponent<CombatManager>().AllTroop.SetActive(true);
                GetComponent<CombatManager>().Arena.SetActive(false);
                GetComponent<CombatManager>().emyforces += (GetComponent<CombatManager>().maxemyforces / 10);
                GetComponent<CombatManager>().playerforces -= (GetComponent<CombatManager>().maxplayerforces / 10);
            }
            else
            {
                blood = 1;
                GetComponent<CombatManager>().playerforces -= 10;
            }         
            if (false)
            {
                if (!nearDeath)
                {
                    GetComponent<CombatManager>().playerforces -= 100;
                    nearDeath = true;
                    weapon.SetActive(false);
                }
            }                 
        }
        if(nearDeath)
        {
            dead_timer += Time.deltaTime;
            if (dead_timer > 5)
            {
                dead_timer = 0;
                weapon.SetActive(true);
                blood = maxblood;
                nearDeath = false;
            }
        }
        weapon.GetComponent<CharacterWeapon>().damage = damage;
        weapon.GetComponent<CharacterWeapon>().isAttack = isAttacking;
        weapon.GetComponent<CharacterWeapon>().atkmode = atkmode;
    }
    public void BeAttacked(int damage)
    {
        if (!isDodge)
        {
            blood -= damage;
            isAttacking = false;
            weapon.GetComponent<CharacterWeapon>().isAttack = false;
            GetComponent<CombatManager>().playerforces -= 1;
            if (!invincible)
            {
                hitted = true;
                GetComponent<CharacterAnimation>().anim_laststate_move = -1;
            }
            //audio_hitted.GetComponent<AudioSource>().Stop();
            audio_hitted.GetComponent<AudioSource>().PlayOneShot(audio_hitted.GetComponent<AudioSource>().clip);
            getDamage?.Invoke(damage);
        }
    }
    public void BeHealed(int heal)
    {
        blood += heal;
    }
    public void EndAttack()
    {
        weapon.GetComponent<CharacterWeapon>().EndAttack();
    }
}
