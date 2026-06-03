using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class CharacterAnimation : MonoBehaviour
{
    HitEffect hitEffect;
    Character character;
    CharacterMove characterMove;
    CharacterAttack characterAttack;
    [Header("Animation")]
    GameObject model;
    public Animator anim;
    private Vector3 originLoction;
    private Quaternion originRotation;
    private Vector3 originScale;
    private int anim_state_move;
    private int anim_laststate_move;
    private Queue<int> anim_state_attack = new Queue<int>();
    public bool isAttacking;
    float movestatechange_timer; 
    private void Awake()
    {
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
        hitEffect = GetComponent<HitEffect>() ? GetComponent<HitEffect>() : this.gameObject.AddComponent<HitEffect>();
        character = GetComponent<Character>() ? GetComponent<Character>() : this.gameObject.AddComponent<Character>();
        characterMove
            = GetComponent<CharacterMove>() ? GetComponent<CharacterMove>() : this.gameObject.AddComponent<CharacterMove>();
        characterAttack
            = GetComponent<CharacterAttack>()? GetComponent<CharacterAttack>(): this.gameObject.AddComponent<CharacterAttack>();
    }
    void Start()
    {
        
    }
    public void SetAttack(int mode)
    {
        if (anim_state_attack.Count < 1)
        {
            anim_state_attack.Enqueue(mode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        character.isAttacking = isAttacking;
        if (anim != null)
        {
            switch (characterMove.moveState)
            {
                case CharacterMove.MoveState.Idle:
                    anim_state_move = 0;
                    break;
                case CharacterMove.MoveState.WalkForward:
                    anim_state_move = 1;
                    break;
                case CharacterMove.MoveState.WalkBackward:
                    anim_state_move = 2;
                    break;
                case CharacterMove.MoveState.RunForward:
                    anim_state_move = 3;
                    break;
                case CharacterMove.MoveState.RunBackward:
                    anim_state_move = 4;
                    break;
                case CharacterMove.MoveState.Jump:
                    anim_state_move = 5;
                    break;
                case CharacterMove.MoveState.DodgeForward:
                    anim_state_move = 6;
                    break;
                case CharacterMove.MoveState.DodgeBackward:
                    anim_state_move = 7;
                    break;
            }
            //test
/*            if (Keyboard.current.qKey.isPressed)
            {
                character.hitted = true;
            }*/
            //
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (character.nearDeath)
            {
                anim.Play("death");
            }
            else if (character.hitted)
            {
                character.hitted = false;
                //anim.Play("hitted");
                hitEffect.TakeDamage();
            }
            else
            {
                if (anim_state_attack.Count > 0)
                {
                    ChangeAnimationState_Attack();
                }
                else
                {
                    state = anim.GetCurrentAnimatorStateInfo(0);
                    if (isAttacking)
                    {
                        if (state.normalizedTime >= 1f)
                        {
                            isAttacking = false;
                        }
                    }
                    if (!isAttacking)
                    {
                        movestatechange_timer += Time.deltaTime;
                        if (anim_laststate_move != anim_state_move)
                        {                         
                            if(anim_state_move==0)
                            {
                                if (movestatechange_timer > 0.15f)
                                {
                                    movestatechange_timer = 0;
                                    ChangeAnimationState_Move();
                                }                       
                            }
                            else
                            {
                                movestatechange_timer = 0;
                                ChangeAnimationState_Move();
                            }
                        }
                    }
                }
                state = anim.GetCurrentAnimatorStateInfo(0);
                if (state.IsName("death"))
                {
                    return;
                }
                if (state.IsName("dodge"))
                {
                    if (state.normalizedTime >= 1f)
                    {
                        model.transform.localPosition = originLoction;
                        model.transform.localRotation = originRotation;
                        model.transform.localScale = originScale;
                        anim_laststate_move = 0;
                    }
                }
                else if(state.IsName("dodgeback"))
                {
                    model.transform.localPosition = originLoction;
                    if (state.normalizedTime >= 1f)
                    {
                        model.transform.localPosition = originLoction;
                        model.transform.localRotation = originRotation;
                        model.transform.localScale = originScale;
                        anim_laststate_move = 0;
                    }
                }
                else if (state.IsName("jump"))
                {
                    model.transform.localPosition = originLoction;
                    model.transform.localRotation = originRotation;
                    model.transform.localScale = originScale;
                    if (state.normalizedTime >= 1f)
                    {
                        anim_laststate_move = 0;
                    }
                }
                else
                {
                    model.transform.localPosition = originLoction;
                    model.transform.localRotation = originRotation;
                    model.transform.localScale = originScale;
                }
            }           
        }
    }
    void ChangeAnimationState_Move()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("jump") && state.normalizedTime < 1f)
        {
            return;
        }
        else if (state.IsName("dodge") && state.normalizedTime < 1f)
        {
            return;
        }
        else if (state.IsName("dodgeback") && state.normalizedTime < 1f)
        {
            return;
        }
        anim_laststate_move = anim_state_move;
        switch (anim_state_move)
        {
            case 0:
                anim.Play("idle01",0,0);
                break;
            case 1:
                anim.Play("walk",0,0);
                break;
            case 2:
                anim.Play("walkbackward", 0, 0);
                break;
            case 3:
                anim.Play("run", 0, 0);
                break;
            case 4:
                anim.Play("runbackward", 0, 0);
                break;
            case 5:
                anim.Play("jump", 0, 0);
                break;
            case 6:
                anim.Play("dodge", 0, 0);
                break;
            case 7:
                anim.Play("dodgeback", 0, 0);
                break;
        }
    }
    void ChangeAnimationState_Attack()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (isAttacking && state.normalizedTime < 1f)
        {
            return;
        }
        switch (anim_state_attack.Dequeue())
        {
            case 1:
                anim.Play("atk01", 0, 0);
                character.atkmode = 0;
                break;
            case 2:
                anim.Play("atk02", 0, 0);
                character.atkmode = 0;
                break;
            case 3:
                anim.Play("atk03", 0, 0);
                character.atkmode = 1;
                break;
            case 4:
                anim.Play("atk04", 0, 0);
                character.atkmode = 2;
                break;
            case 5:
                anim.Play("encourage", 0, 0);
                break;
        }
        isAttacking = true;
        character.EndAttack();
        anim_laststate_move = -1;
    }
}
