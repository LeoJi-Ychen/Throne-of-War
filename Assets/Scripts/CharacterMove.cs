using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMove : MonoBehaviour
{
    CharacterAttack characterAttack;
    [Header("Move")]
    public float runSpeed = 6f;
    public float walkSpeed = 3f;

    [Header("Jump Feel")]
    public float jumpHeight = 3f;
    public float gravity = -25f;
    public float fallMultiplier = 2.2f;
    public float lowJumpMultiplier = 1.5f;
    public float groundedForce = -5f;

    [Header("Dodge")]
    public float dodgeSpeed = 20f;
    public float dodgeDuration = 1f;
    public float dodgeCooldown = 0.5f;

    public MoveState moveState;
    public enum MoveState
    {
        Idle,
        WalkForward,
        RunForward,
        WalkBackward,
        RunBackward,
        Jump,
        Dodge
    }

    private CharacterController controller;

    private Vector3 velocity;

    private bool isDodging;
    private Vector3 dodgeDirection;
    private float dodgeTimer;
    private float dodgeCooldownTimer;

    void Start()
    {
        characterAttack = GetComponent<CharacterAttack>();
        controller = GetComponent<CharacterController>();      
    }

    void Update()
    {
        if (characterAttack.attackState == CharacterAttack.AttackState.None)
        {
            HandleMovement();
        }
        HandleGravity();
        HandleDodgeCooldown();
    }

    void HandleMovement()
    {
        bool grounded = controller.isGrounded;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            input.y += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            input.y -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            input.x -= 1;
        }           
        if (Keyboard.current.dKey.isPressed)
        {
            input.x += 1;
        }           
        Vector3 move =
            transform.right * input.x +
            transform.forward * input.y;

        move = move.normalized;

        float speed = Keyboard.current.leftShiftKey.isPressed
            ? walkSpeed
            : runSpeed;


        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame
            && !isDodging
            && dodgeCooldownTimer <= 0
            && move != Vector3.zero)
        {
            isDodging = true;
            dodgeTimer = dodgeDuration;
            dodgeDirection = move;

            dodgeCooldownTimer = dodgeCooldown;
        }

        if (isDodging)
        {
            moveState = MoveState.Dodge;
            controller.Move(
                dodgeDirection *
                dodgeSpeed *
                Time.deltaTime);

            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0)
            {
                isDodging = false;
            }
        }
        else
        {
            moveState = MoveState.Idle;

            if (input.y > 0 || input.x != 0)
            {
                if (speed == walkSpeed)
                {
                    moveState = MoveState.WalkForward;
                }
                else
                {
                    moveState = MoveState.RunForward;
                }
            }
            if (input.y < 0)
            {
                if (speed == walkSpeed)
                {
                    moveState = MoveState.WalkBackward;
                }
                else
                {
                    moveState = MoveState.RunBackward;
                }
            }
            controller.Move(
                move *
                speed *
                Time.deltaTime);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (!grounded)
        {
            moveState = MoveState.Jump;
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

    void HandleDodgeCooldown()
    {
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
    }
}