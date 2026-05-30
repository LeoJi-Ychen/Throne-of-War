using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMove : MonoBehaviour
{
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
    public float dodgeDuration = 0.5f;
    public float dodgeCooldown = 0.5f;

    [Header("Dodge")]
    GameObject model;
    private Vector3 originLoction;
    private Quaternion originRotation;
    private Vector3 originScale;
    public Animator anim;
    private int anim_state = 0;

    private CharacterController controller;

    private Vector3 velocity;

    private bool isDodging;
    private Vector3 dodgeDirection;
    private float dodgeTimer;
    private float dodgeCooldownTimer;

    private void Awake()
    {
        if(anim != null)
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
        controller = GetComponent<CharacterController>();      
    }

    void Update()
    {
        HandleMovement();
        HandleGravity();
        HandleDodgeCooldown();
        if(anim != null)
        {
            anim.SetInteger("state", anim_state);
            model.transform.localPosition = originLoction;
            model.transform.localRotation = originRotation;
            model.transform.localScale = originScale;
        }
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

        anim_state = 0;

        float speed = Keyboard.current.leftShiftKey.isPressed
            ? walkSpeed
            : runSpeed;

        if (input.y > 0|| input.x !=0)
        {
            if (speed == walkSpeed)
            {
                anim_state = 2;
            }
            else
            {
                anim_state = 1;
            }      
        }
        if (input.y < 0)
        {
            anim_state = 3;
        }

        if (Keyboard.current.leftCtrlKey.wasPressedThisFrame
            && !isDodging
            && dodgeCooldownTimer <= 0
            && move != Vector3.zero)
        {
            isDodging = true;
            anim_state = 6;
            dodgeTimer = dodgeDuration;
            dodgeDirection = move;

            dodgeCooldownTimer = dodgeCooldown;
        }

        if (isDodging)
        {
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
            controller.Move(
                move *
                speed *
                Time.deltaTime);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
        {
            anim_state = 4;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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