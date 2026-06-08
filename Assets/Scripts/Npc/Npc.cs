using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class Npc : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> emylist = new List<GameObject>();
    public List<GameObject> bosslist = new List<GameObject>();
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
    public int behavior_state;
    float behaviorDuration;
    float behavior_timer;
    private CharacterController controller;
    private Vector3 velocity;
    float moveSpeed;
    public int atk_mode;
    [Header("Jump Feel")]
    public float gravity = -25f;
    public float fallMultiplier = 2.2f;
    public float lowJumpMultiplier = 1.5f;
    public float groundedForce = -5f;
    bool isRunning;
    int atk_count;
    bool hitted_state;
    public float rotateSpeed = 360f;
    float resettarget_timer;
    bool cheer;
    Vector3 startPostion;
    Vector3 modelparent_location;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        behaviorDuration = 3;
        maxblood = 10;
        damage = 2;
        moveSpeed = 4.2f;
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
    void Start()
    {
        startPostion = transform.position;
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player");
        //ResetTarget();
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
    void ResetTarget()
    {
        if (target != null)
        {
            cheer = false;
        }
        if (!cheer)
        {
            emylist = new List<GameObject>(GameObject.FindGameObjectsWithTag("Emy"));
            bosslist = new List<GameObject>(GameObject.FindGameObjectsWithTag("Boss"));
            if (Random.Range(0, 10) < 8)
            {
                if (emylist.Count > 0)
                {
                    target = emylist[Random.Range(0, emylist.Count)];
                }
                else
                {
                    if (bosslist.Count > 0)
                    {
                        target = bosslist[Random.Range(0, bosslist.Count)];
                    }
                }
            }
            else
            {
                if (bosslist.Count > 0)
                {
                    target = bosslist[Random.Range(0, bosslist.Count)];
                }
            }
        }      
        if (target == null)
        {
            cheer = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target==null)
        {
            resettarget_timer += Time.deltaTime;
            if (resettarget_timer > 1)
            {
                resettarget_timer = 0;
                ResetTarget();
            }
        }
        else if(target.activeSelf == false)
        {
            resettarget_timer += Time.deltaTime;
            if (resettarget_timer > 1)
            {
                resettarget_timer = 0;
                target = null;
            }
        }
        if (cheer)
        {
            anim.Play("npcencourage");
        }
        if (death)
        {
            Birth();
            return;
        }
        weapon.GetComponent<NpcWeapon>().damage = damage;
        if (blood <= 0)
        {
            if (!nearDeath)
            {
                modelparent_location = model.transform.parent.position;
                nearDeath = true;
                player.GetComponent<CombatManager>().playerforces -= 3;
                controller.excludeLayers = LayerMask.GetMask("Role");
            }
        }
        if (nearDeath)
        {
            behavior_state = 0;
            behavior_timer = 0;
            dead_timer += Time.deltaTime;
            Revive();
            if (dead_timer > 3)
            {
                dead_timer = 0;
                death = true;
                //this.gameObject.SetActive(false);
            }
        }
        if (!nearDeath)
        {
            if (target != null)
            {
                NpcBehavior();
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
    public void BeAttacked(int damage,GameObject attacker = null)
    {
        blood -= damage;
        hitted = true;
        behavior_timer = 2;
        behavior_state = 0;
        atk_mode = 0;
        player.GetComponent<CombatManager>().playerforces -= 1;
        if (attacker != null )
        {
            target = attacker;
        }
    }
    public void BeHealed(int heal)
    {
        blood += heal;
    }
    void NpcBehavior()
    {
        if (atk_mode == 0)
        {
            behavior_timer += Time.deltaTime;
        }
        if (behavior_timer >= behaviorDuration)
        {
            behavior_timer = 0;
            atk_count = 0;
            if (distanceToTarget() > 6)
            {
                behavior_state = 2;
            }
            else
            {
                behavior_state = Random.Range(0, 5);
            }
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
                    behavior_timer = 2;
                    controller.Move(Vector3.zero);
                    isRunning = false;
                }     
                break;
            case 2:
                if (distanceToTarget() > 1.2f && atk_mode == 0 && atk_count == 0)
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
                if (distanceToTarget() > 1.2f && atk_mode == 0 && atk_count == 0)
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
                if (distanceToTarget() > 1.2f && atk_mode==0 && atk_count == 0)
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
        else if (hitted||hitted_state)
        {
            if (hitted)
            {
                hitted = false;
                hitted_state = true;
                //hitEffect.TakeDamage();
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
                weapon.GetComponent<NpcWeapon>().isAttack = false;
                isAttacking = false;
            }
            else
            {
                if (atk_mode == 0)
                {
                    anim.Play("idle");
                    weapon.GetComponent<NpcWeapon>().isAttack = false;
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
                                weapon.GetComponent<NpcWeapon>().isAttack = true;
                                weapon.GetComponent<NpcWeapon>().atkmode = 0;
                                break;
                            case 2:
                                anim.Play("atk03");
                                isAttacking = true;
                                weapon.GetComponent<NpcWeapon>().isAttack = true;
                                weapon.GetComponent<NpcWeapon>().atkmode = 1;
                                break;
                            case 3:
                                anim.Play("atk04");
                                isAttacking = true;
                                weapon.GetComponent<NpcWeapon>().isAttack = true;
                                weapon.GetComponent<NpcWeapon>().atkmode = 2;
                                break;
                        }
                    }
                    else
                    {
                        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
                        if (state.normalizedTime >= 1f)
                        {
                            weapon.GetComponent<NpcWeapon>().isAttack = false;
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
    void Revive()
    {
        if ((startPostion - player.transform.position).magnitude < 6)
        {
            startPostion = player.transform.position - player.transform.forward * 6;
        }
        Vector3 dir = (startPostion - transform.position);
        dir.y = 0;
        float dis = dir.magnitude;
        dir.Normalize();
        if (dis > 0.5f)
        {
            controller.Move(dir * 10 * Time.deltaTime);
        }
        model.transform.parent.position = modelparent_location;
    }
    public void Birth()
    {
        model.transform.parent.localPosition = Vector3.zero;
        model.transform.localPosition = originLoction;
        model.transform.localRotation = originRotation;
        model.transform.localScale = originScale;
        blood = maxblood;
        nearDeath = false;
        death = false;
        hitted = false;
        hitted_state = false;
        controller.excludeLayers = 0;
    }
}
