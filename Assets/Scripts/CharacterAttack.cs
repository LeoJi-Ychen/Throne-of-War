using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAttack : MonoBehaviour
{
    CharacterMove characterMove;
    int attack_state;
    float timer;
    float duration;
    public AttackState attackState;
    public enum AttackState
    {
        None,
        NormalAttack,
        ChargedAttack,
        NormalAttack01,
        ChargedAttack01,
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
        duration = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(characterMove.moveState != CharacterMove.MoveState.Jump
            && characterMove.moveState != CharacterMove.MoveState.Dodge)
        {
            Attack();
        }      
    }
    void Attack()
    {
        if(attack_state == 0)
        {
            if (Keyboard.current.leftShiftKey.isPressed && Mouse.current.leftButton.wasPressedThisFrame)
            {
                attack_state = 3;
            }
            else if (Keyboard.current.leftShiftKey.isPressed && Mouse.current.rightButton.wasPressedThisFrame)
            {
                attack_state = 4;
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                attack_state = 1;
            }
            else if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                attack_state = 2;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                timer = 0;
                attack_state = 0;
            }
        }
        switch (attack_state)
        {
            case 0:
                attackState = AttackState.None;
                break;
            case 1:
                attackState = AttackState.NormalAttack;
                break;
            case 2:
                attackState = AttackState.ChargedAttack;
                break;
            case 3:
                attackState = AttackState.NormalAttack01;
                break;
            case 4:
                attackState = AttackState.ChargedAttack01;
                break;
        }
    }
}
