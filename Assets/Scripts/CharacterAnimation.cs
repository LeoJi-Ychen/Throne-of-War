using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    CharacterMove characterMove;
    CharacterAttack characterAttack;
    [Header("Animation")]
    GameObject model;
    public Animator anim;
    private Vector3 originLoction;
    private Quaternion originRotation;
    private Vector3 originScale;
    private int anim_state;
    private int anim_laststate;
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
        characterMove
            = GetComponent<CharacterMove>() ? GetComponent<CharacterMove>() : this.gameObject.AddComponent<CharacterMove>();
        characterAttack
            = GetComponent<CharacterAttack>()? GetComponent<CharacterAttack>(): this.gameObject.AddComponent<CharacterAttack>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim != null)
        {
            switch (characterMove.moveState)
            {
                case CharacterMove.MoveState.Idle:
                    anim_state = 0;
                    break;
                case CharacterMove.MoveState.WalkForward:
                    anim_state = 1;
                    break;
                case CharacterMove.MoveState.WalkBackward:
                    anim_state = 2;
                    break;
                case CharacterMove.MoveState.RunForward:
                    anim_state = 3;
                    break;
                case CharacterMove.MoveState.RunBackward:
                    anim_state = 4;
                    break;
                case CharacterMove.MoveState.Jump:
                    anim_state = 5;
                    break;
                case CharacterMove.MoveState.Dodge:
                    anim_state = 6;
                    break;
            }
            if(anim_state!=5 && anim_state != 6)
            {
                switch (characterAttack.attackState)
                {
                    case CharacterAttack.AttackState.NormalAttack:
                        anim_state = 7;
                        break;
                    case CharacterAttack.AttackState.ChargedAttack:
                        anim_state = 8;
                        break;
                    case CharacterAttack.AttackState.NormalAttack01:
                        anim_state = 9;
                        break;
                    case CharacterAttack.AttackState.ChargedAttack01:
                        anim_state = 10;
                        break;
                }
            }          
            if (anim_laststate!= anim_state)
            {
                ChangeAnimationState();
            }
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("dodge"))
            {
                model.transform.localPosition = originLoction;
                model.transform.localRotation = originRotation;
                model.transform.localScale = originScale;           
            }
            else if (state.normalizedTime >= 1f)
            {
                model.transform.localPosition = originLoction;
                model.transform.localRotation = originRotation;
                model.transform.localScale = originScale;
            }
            if (state.IsName("atk01")|| state.IsName("atk02") || state.IsName("atk03") || state.IsName("atk04"))
            {
                if(state.normalizedTime >= 1f)
                {
                    anim_laststate = 0;
                }             
            }
        }
    }
    void ChangeAnimationState()
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
        else if (state.IsName("atk01") && state.normalizedTime < 1f)
        {
            return;
        }
        else if (state.IsName("atk02") && state.normalizedTime < 1f)
        {
            return;
        }
        else if (state.IsName("atk03") && state.normalizedTime < 1f)
        {
            return;
        }
        else if (state.IsName("atk04") && state.normalizedTime < 1f)
        {
            return;
        }
        anim_laststate = anim_state;
        switch (anim_state)
        {
            case 0:
                anim.Play("idle",0,0);
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
                anim.Play("atk01", 0, 0);
                break;
            case 8:
                anim.Play("atk02", 0, 0);
                break;
            case 9:
                anim.Play("atk03", 0, 0);
                break;
            case 10:
                anim.Play("atk04", 0, 0);
                break;
        }
    }
}
