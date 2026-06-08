using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterController))]
public class EnemyBoss : MonoBehaviour
{
    public GameObject audio_hitted;
    public GameObject player;
    public List<GameObject> npclist = new List<GameObject>();
    public GameObject target;
    public GameObject weapon;
    HitEffect hitEffect;
    GameObject model;
    public Animator anim;
    private Vector3 originLoction;
    private Quaternion originRotation;
    private Vector3 originScale;
    public bool isAttacking;
    public int maxblood;
    public int blood;
    public int damage;
    public bool hitted;
    public bool nearDeath;
    public bool death;
    public float dead_timer;
    int behavior_state;
    float behaviorDuration;
    float behavior_timer;
    private CharacterController controller;
    private Vector3 velocity;
    float moveSpeed;
    int atk_mode;
    [Header("Jump Feel")]
    public float gravity = -25f;
    public float fallMultiplier = 2.2f;
    public float lowJumpMultiplier = 1.5f;
    public float groundedForce = -5f;
    bool isRunning;
    int atk_count;
    bool hitted_state;
    public bool sitting;
    float sitting_timer;
    int life;
    public float rotateSpeed = 360f;
    bool redflash;
    float hatred_timer;
    bool invincible;
    float invincible_timer;
    public Vector3 startPostion;
    bool aimplayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        life = 10;
        behaviorDuration = 3;
        maxblood = 120;
        damage = 10;
        moveSpeed = 4.5f;
        blood = maxblood;
        if (anim != null)
        {
            model = anim.gameObject;
        }
        if (model != null)
        {
            originLoction = model.transform.localPosition;
            originRotation = model.transform.localRotation;
            originScale = model.transform.localScale;
        }
    }
    void ResetTarget()
    {
        npclist = new List<GameObject>(GameObject.FindGameObjectsWithTag("NPC"));
        List<GameObject> nearnpclist = new List<GameObject>();
        foreach(GameObject n in npclist)
        {
            if (distance(n) < 5)
            {
                nearnpclist.Add(n);
            }
        }
        if (nearnpclist.Count > 0)
        {
            if (distance(player) < 8)
            {
                if (Random.Range(0, 10) < 8)
                {
                    target = nearnpclist[Random.Range(0, nearnpclist.Count)];
                }
                else
                {
                    target = player;
                }
            }
            else
            {
                target = nearnpclist[Random.Range(0, nearnpclist.Count)];
            }
        }
        else
        {
            if (Random.Range(0, 10) < 8)
            {
                if (npclist.Count > 0)
                {
                    target = npclist[Random.Range(0, npclist.Count)];
                }
                else
                {
                    target = player;
                }
            }
            else
            {
                target = player;
            }
        }   
    }
    float distance(GameObject obj)
    {
        float res = (obj.transform.position - transform.position).magnitude;
        return res;
    }
    void Start()
    {
        startPostion = transform.position;
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player");
        ResetTarget();
        hitEffect = GetComponent<HitEffect>() ? GetComponent<HitEffect>() : this.gameObject.AddComponent<HitEffect>();
    }
    void Rotate()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(dir);

        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (invincible)
        {
            invincible_timer += Time.deltaTime;
            if(invincible_timer > 3)
            {
                invincible_timer = 0;
                invincible = false;
            }
        }
        if (!aimplayer)
        {
            if (player.GetComponent<CombatManager>().battlefieldChange)
            {
                aimplayer = true;
                target = player;
                hatred_timer = 0;
            }
        }
        if (target == null)
        {
            ResetTarget();
        }
        else if (target.activeSelf == false)
        {
            ResetTarget();
        }
        else
        {
            if (distanceToTarget() > 9)
            {
                hatred_timer += Time.deltaTime;
                if (hatred_timer > 6)
                {
                    hatred_timer = 0;
                    target = null;
                }
            }
        }
        if (death)
        {
            return;
        }
        weapon.GetComponent<EnemyWeapon>().damage = damage;
        if (blood <= 0)
        {
            if (life > 0)
            {
                life--;
                blood = maxblood;
                hitted = false;
                hitted_state = false;
                sitting = true;
                sitting_timer = 0;
                weapon.GetComponent<EnemyWeapon>().isAttack = false;
                isAttacking = false;
                atk_mode = 0;
                atk_count = 0;
                behavior_state = 0;
                behaviorDuration = 2;
                player.GetComponent<CombatManager>().emyforces -= 200;
            }
            else
            {
                nearDeath = true;
                player.GetComponent<CombatManager>().emyforces -= 10000;
            }
        }
        if (nearDeath)
        {
            dead_timer += Time.deltaTime;
            if (dead_timer > 3)
            {
                death = true;
                this.gameObject.SetActive(false);
            }
        }
        if (!nearDeath&&!sitting)
        {          
            if (target != null)
            {
                EnemyBehavior();
                Rotate();
            }
            else
            {
                behavior_state = 0;
                behavior_timer = 0;
                isRunning = false;
            }
            HandleGravity();
        }
        if (sitting)
        {
            sitting_timer += Time.deltaTime;
            if (sitting_timer > 10)
            {
                sitting_timer = 0;
                sitting = false;
                ResetTarget();
            }
        }
        if (anim != null)
        {
            AnimationControll();
            if (!nearDeath)
            {
                model.transform.localPosition = originLoction;
                model.transform.localRotation = originRotation;
                model.transform.localScale = originScale;
            }
        }
        //Debug.Log(distanceToPlayer());
    }
    public void BeAttacked(int damage, int mode = 0)
    {
        blood -= damage;
        if (!invincible||mode>=1)
        {
            hitted = true;
            target = player;
            weapon.GetComponent<EnemyWeapon>().isAttack = false;
            audio_hitted.GetComponent<AudioSource>().Play();
        }
        if (mode > 2)
        {
            redflash = true;
        }
        behavior_timer = 2;
        behavior_state = 0;
        atk_mode = 0;
        player.GetComponent<CombatManager>().emyforces -= 1;
    }
    public void BeHealed(int heal)
    {
        blood += heal;
    }
    void EnemyBehavior()
    {
        if (atk_mode == 0)
        {
            behavior_timer += Time.deltaTime;
        }
        if (behavior_timer >= behaviorDuration)
        {
            behavior_timer = 0;
            atk_count = 0;
            behavior_state = Random.Range(0, 5);
        }
        switch (behavior_state)
        {
            case 0:
                controller.Move(Vector3.zero);
                isRunning = false;
                break;
            case 1:
                if (distanceToTarget() > 4)
                {
                    controller.Move(dirToTarget() * moveSpeed * Time.deltaTime);
                    isRunning = true;
                }
                else
                {
                    behavior_state = 0;
                    controller.Move(Vector3.zero);
                    isRunning = false;
                }     
                break;
            case 2:
                if (distanceToTarget() > 1.6f && atk_mode == 0 && atk_count == 0)
                {
                    controller.Move(dirToTarget() * moveSpeed * 1.2f * Time.deltaTime);
                    isRunning = true;
                }
                else
                {
                    isRunning = false;
                    if (atk_count < 1)
                    {
                        atk_mode = 1;
                        atk_count++;
                    }                   
                }
                break;
            case 3:
                if (distanceToTarget() > 1.6f && atk_mode == 0 && atk_count == 0)
                {
                    controller.Move(dirToTarget() * moveSpeed * 1.2f * Time.deltaTime);
                    isRunning = true;
                }
                else
                {
                    isRunning = false;
                    if (atk_count < 1)
                    {
                        atk_mode = 2;
                        atk_count++;
                    }
                }
                break;
            case 4:
                if (distanceToTarget() > 1.6f && atk_mode==0 && atk_count==0)
                {
                    controller.Move(dirToTarget() * moveSpeed * 1.2f * Time.deltaTime);
                    isRunning = true;
                }
                else
                {
                    isRunning = false;
                    if (atk_count < 1)
                    {
                        atk_mode = 3;
                        atk_count++;
                    }
                }
                break;
        }
    }
    float distanceToTarget()
    {
        float res = 100;
        if (target != null)
        {
            res = (target.transform.position - transform.position).magnitude;
        }
        return res;
    }
    Vector3 dirToTarget()
    {
        Vector3 v = transform.forward;
        if (target != null)
        {
            v = (target.transform.position - transform.position);
            v.y = 0;
        }
        return v.normalized;
    }
    void AnimationControll()
    {
        if (nearDeath)
        {
            anim.Play("death");
        }
        else if (sitting)
        {
            anim.Play("sit");
        }
        else if (hitted || hitted_state)
        {
            if (hitted)
            {
                hitted = false;
                hitted_state = true;
                invincible = true;
                if (redflash)
                {
                    redflash = false;
                    hitEffect.TakeDamage();
                }
                anim.Play("hitted", 0, 0f);
            }
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("hitted") && state.normalizedTime >= 1f)
            {
                hitted_state = false;
            }
        }
        else
        {
            if (isRunning)
            {
                anim.Play("run");
                weapon.GetComponent<EnemyWeapon>().isAttack = false;
                isAttacking = false;
            }
            else
            {
                if (atk_mode == 0)
                {
                    anim.Play("idle");
                    weapon.GetComponent<EnemyWeapon>().isAttack = false;
                    isAttacking = false;
                }
                else
                {
                    if (!isAttacking)
                    {
                        switch (atk_mode)
                        {
                            case 1:
                                anim.Play("atk02");
                                isAttacking = true;
                                weapon.GetComponent<EnemyWeapon>().isAttack = true;
                                weapon.GetComponent<EnemyWeapon>().atkmode = 0;
                                break;
                            case 2:
                                anim.Play("atk03");
                                isAttacking = true;
                                weapon.GetComponent<EnemyWeapon>().isAttack = true;
                                weapon.GetComponent<EnemyWeapon>().atkmode = 1;
                                break;
                            case 3:
                                anim.Play("atk04");
                                isAttacking = true;
                                weapon.GetComponent<EnemyWeapon>().isAttack = true;
                                weapon.GetComponent<EnemyWeapon>().atkmode = 2;
                                break;
                        }
                    }
                    else
                    {
                        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
                        if (state.normalizedTime >= 1f)
                        {
                            weapon.GetComponent<EnemyWeapon>().isAttack = false;
                            isAttacking = false;
                            atk_mode = 0;
                        }
                    }                
                }
            }
        }
    }
    void HandleGravity()
    {
        bool grounded = controller.isGrounded;

        if (grounded && velocity.y < 0f)
        {
            velocity.y = groundedForce;
        }

        if (velocity.y < 0f)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else if (velocity.y > 0f && !Keyboard.current.spaceKey.isPressed)
        {
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
