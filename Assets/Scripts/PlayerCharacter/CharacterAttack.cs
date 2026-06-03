using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAttack : MonoBehaviour
{
    CharacterMove characterMove;
    CharacterAnimation characterAnimation;
    int attack_state;
    public float press_timer;
    float timer;
    float duration;
    bool canInput;
    public bool isPress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
        characterAnimation = GetComponent<CharacterAnimation>();
        duration = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(characterMove.moveState != CharacterMove.MoveState.Jump
            && characterMove.moveState != CharacterMove.MoveState.DodgeForward
            && characterMove.moveState != CharacterMove.MoveState.DodgeBackward)
        {
            Attack();
        }      
    }
    void Attack()
    {
        timer += Time.deltaTime;
        if(timer > duration)
        {
            canInput = true;
        }
        if (isPress)
        {
            press_timer += Time.deltaTime;
            if (attack_state == 0 && canInput)
            {
                if (press_timer > 0.8f)
                {
                    if (Mouse.current.leftButton.isPressed)
                    {
                        isPress = false;
                        attack_state = 3;
                        press_timer = 0;
                    }
                    else if (Mouse.current.rightButton.isPressed)
                    {
                        isPress = false;
                        attack_state = 4;
                        press_timer = 0;
                    }
                }
                else
                {
                    if (Mouse.current.leftButton.wasReleasedThisFrame)
                    {
                        isPress = false;
                        attack_state = 1;
                        press_timer = 0;
                    }
                    else if (Mouse.current.rightButton.wasReleasedThisFrame)
                    {
                        isPress = false;
                        attack_state = 2;
                        press_timer = 0;
                    }
                }             
            }
        }
        else
        {
            if (attack_state == 0 && canInput)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    isPress = true;
                }
                else if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    isPress = true;
                }
            }
            if (attack_state == 0 && canInput)
            {
                if (Keyboard.current.kKey.wasPressedThisFrame)
                {
                    attack_state = 5;
                }
            }
        }      
        if (attack_state != 0)
        {
            characterAnimation.SetAttack(attack_state);
            attack_state = 0;
            canInput = false;
            timer = 0;
        }
    }
}
