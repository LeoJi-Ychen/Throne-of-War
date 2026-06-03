using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMove : MonoBehaviour
{
    CharacterAttack characterAttack;
    CharacterAnimation characterAnimation;
    [Header("Move")]
    public float runSpeed = 4.5f;
    public float walkSpeed = 3f;

    [Header("Jump Feel")]
    float jumpHeight = 1f;
    float gravity = -15f;
    float fallMultiplier = 1.2f;
    float lowJumpMultiplier = 1.0f;
    float groundedForce = -5f;

    [Header("Dodge")]
    float dodgeSpeed = 3f;
    float dodgeDuration = 1.2f;
    float dodgeCooldown = 0.5f;

    public MoveState moveState;
    public enum MoveState
    {
        Idle,
        WalkForward,
        RunForward,
        WalkBackward,
        RunBackward,
        Jump,
        DodgeForward,
        DodgeBackward
    }

    private CharacterController controller;

    private Vector3 velocity;

    private bool isDodging;
    private Vector3 dodgeDirection;
    private float dodgeTimer;
    private float dodgeCooldownTimer;
    private bool lastGround;
    private bool isLanding;
    private float landing_timer;
    private float stiffeningDuration;
    private bool dodgeFront;
    private bool endDodge;
    private float endDodge_timer;
    private float stiffeningDuration_dodge;

    void Start()
    {
        characterAttack = GetComponent<CharacterAttack>();
        characterAnimation = GetComponent<CharacterAnimation>();
        controller = GetComponent<CharacterController>();
        stiffeningDuration = 1.2f;
        stiffeningDuration_dodge = 0.6f;
    }

    void Update()
    {
        if (!characterAnimation.isAttacking)
        {
            if (characterAttack.isPress)
            {
                if (characterAttack.press_timer > 0.2f)
                {
                    moveState = MoveState.Idle;
                }
                else
                {
                    HandleMovement();
                }               
            }
            else
            {
                HandleMovement();
            }
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
            && dodgeCooldownTimer <= 0)
        {
            isDodging = true;
            dodgeTimer = dodgeDuration;
            if (input.y >= 0)
            {
                dodgeFront = true;
                dodgeDirection = transform.forward;
            }
            else
            {
                dodgeFront = false;
                dodgeDirection = -transform.forward/2;
            }

            dodgeCooldownTimer = dodgeCooldown;
        }

        if (isDodging)
        {
            GetComponent<Character>().isDodge = true;
            if (dodgeFront)
            {
                moveState = MoveState.DodgeForward;
            }
            else
            {
                moveState = MoveState.DodgeBackward;
            }
          
            controller.Move(
                dodgeDirection *
                dodgeSpeed *
                Time.deltaTime);

            dodgeTimer -= Time.deltaTime;

            if (dodgeTimer <= 0)
            {
                isDodging = false;
                endDodge = true;
            }
        }
        else
        {
            GetComponent<Character>().isDodge = false;
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
            if (!endDodge)
            {
                if (!isLanding)
                {
                    if (!grounded)
                    {
                        speed = 0.6f * speed;
                    }
                    controller.Move(
                   move *
                   speed *
                   Time.deltaTime);
                }
                else
                {
                    landing_timer += Time.deltaTime;
                    if (landing_timer > stiffeningDuration)
                    {
                        landing_timer = 0;
                        isLanding = false;
                    }
                }
            }
            else
            {
                endDodge_timer += Time.deltaTime;
                if (endDodge_timer > stiffeningDuration_dodge)
                {
                    endDodge_timer = 0;
                    endDodge = false;
                }
            }
          
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded&&!isLanding)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (!grounded)
        {
            moveState = MoveState.Jump;
        }
        if (lastGround != grounded)
        {
            lastGround = grounded;
            if (grounded)
            {
                landing_timer = 0;
                isLanding = true;
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

    void HandleDodgeCooldown()
    {
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
    }
}