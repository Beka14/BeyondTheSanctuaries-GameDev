using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : PlayerSubsystem
{
    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform orientation;

    Vector3 moveDirection;
    Vector3 rootMotion;
    Vector3 velocity;
    bool isJumping = false;
    bool firstJump = false;
    bool issueJump = false;
    bool disableMovement = false;

    public float jumpHeight = 1.5f;
    public float gravity = 9.81f;
    public float stepDown = 0.01f;
    public float airControl = 0.2f;
    
    public static PlayerMovement instance;
    public Action<bool> OnSprintAction;

    public CharacterController Controller { get { return controller; } }
    
    private void Start()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Controls.OnJump += Jump;
        Controls.OnDuckPressed += Duck;
        Controls.OnDuckReleased += Stand;
        Controls.OnSprintPressed += Sprint;
        Controls.OnSprintReleased += StopSprint;

        UIManager.OnUIOpen += Disable;
        UIManager.OnUIClose += Enable;
    }


    private void OnDisable()
    {
        Controls.OnJump -= Jump;
        Controls.OnDuckPressed -= Duck;
        Controls.OnDuckReleased -= Stand;
        Controls.OnSprintPressed -= Sprint;
        Controls.OnSprintReleased -= StopSprint;

        UIManager.OnUIOpen -= Disable;
        UIManager.OnUIClose -= Enable;
    }

    void ResetState()
    {
        Stand();
        StopSprint();

        animator.SetBool("Moving", false);
        animator.SetFloat("InputX", 0.0f);
        animator.SetFloat("InputY", 0.0f);
    }

    void Enable()
    {
        disableMovement = false;
    }
    void Disable()
    {
        ResetState();
        disableMovement = true;
    }

    private void Input()
    {
        Vector2 movement = disableMovement ? Vector2.zero : Controls.GetMovement();
        animator.SetBool("Moving", movement != Vector2.zero);
        moveDirection = orientation.right * movement.x + orientation.forward * movement.y;

        animator.SetFloat("InputX", movement.x, 0.05f, Time.deltaTime);
        animator.SetFloat("InputY", movement.y, 0.05f, Time.deltaTime);
    }

    private void Update()
    {
        Input();
    }
    private void FixedUpdate()
    {
        if (issueJump && !isJumping)
        {
            issueJump = false;
            isJumping = true;
            velocity = controller.velocity;
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }


        if (isJumping)
        {
            velocity.y -= gravity * Time.deltaTime;
            Vector3 motion = velocity * Time.deltaTime;
            motion += CalculateAirControl();

            controller.Move(motion);

            if (!firstJump && controller.isGrounded)
                firstJump = true;
            else
                isJumping = !controller.isGrounded;
        }
        else
        {
            firstJump = false;
            controller.Move(rootMotion + Vector3.down * stepDown);

            if (controller.isGrounded)
            {
                isJumping = true;
                velocity = animator.velocity;
                velocity.y = 0.0f;
            }
        }
        rootMotion = Vector3.zero;
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    void Jump(InputAction.CallbackContext context)
    {
        issueJump = controller.isGrounded && !disableMovement;
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * moveDirection.y) + (transform.right * moveDirection.x)) * airControl / 100;
    }

    void Duck()
    {
        if(disableMovement)
            return;

        animator.SetBool("Crouch", true);
        controller.height = 1.2f;
        controller.center = new Vector3(0, 0.6f, 0);
    }

    void Stand()
    {
        if (disableMovement)
            return;

        animator.SetBool("Crouch", false);
        controller.height = 1.7f;
        controller.center = new Vector3(0, 0.92f, 0);
    }

    void Sprint()
    {
        if (disableMovement)
            return;

        animator.SetBool("Sprint", true);
        OnSprintAction?.Invoke(true);
    }

    void StopSprint()
    {
        if (disableMovement)
            return;

        animator.SetBool("Sprint", false);
        OnSprintAction?.Invoke(false);
    }
}