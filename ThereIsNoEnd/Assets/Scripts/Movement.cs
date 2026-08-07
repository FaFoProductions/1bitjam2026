using UnityEngine;
using UnityEngine.InputSystem;


public class Movement : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] LayerMask playerLayer;

    [Header("Movement Settings")]
    public float topSpeed = 5f;
    public float horDrag = 0.9f;
    public float accel = 1f;
    public float jumpforce = 10;
    public float groundSensitivity = 0.52f;
    public bool isGrounded = true;
    public Vector2 moveDir;
    public int maxAirJumps = 0;
    public int airJumps = 0;
    // Float effect lower value is more floaty
    public float floatEffect = 0.2f;
    private bool jump;
    private bool glide;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        // Fix input assignment
        PlayerControls.PlayerMoveActions playerMove = InputManagerSingleton.Instance.playerControlActions.PlayerMove;
        InputManagerSingleton.Instance.Bind(playerMove.Move, MovePerformed);
        InputManagerSingleton.Instance.Bind(playerMove.Jump, JumpPerformed);
        InputManagerSingleton.Instance.Bind(playerMove.Dash, DashPerformed);
    }

    void Stop()
    {
        PlayerControls.PlayerMoveActions playerMove = InputManagerSingleton.Instance.playerControlActions.PlayerMove;
        InputManagerSingleton.Instance.UnBind(playerMove.Move, MovePerformed);
        InputManagerSingleton.Instance.UnBind(playerMove.Jump, JumpPerformed);
        InputManagerSingleton.Instance.UnBind(playerMove.Dash, DashPerformed);
    }

    void MovePerformed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 targetDir = context.ReadValue<Vector2>();
            moveDir = targetDir.normalized;
        } else if (context.phase == InputActionPhase.Canceled)
        {
            moveDir = new Vector2(0,0);
        }
    }
    
    void JumpPerformed(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started)
        {
            if (isGrounded || airJumps > 0)
            {
                if (!isGrounded && airJumps > 0 ) {airJumps--;}
                jump = true;
            }
        } else if (context.phase == InputActionPhase.Performed) {
            glide = true;
        } else if (context.phase == InputActionPhase.Canceled) {
            glide = false;
        }
    }

    void DashPerformed(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started)
        {
            topSpeed *= 2;
            accel *= 2;
        } else if (context.phase == InputActionPhase.Canceled) {
            topSpeed /= 2;
            accel /= 2;
        }
    }

    void FixedUpdate()
    {
        //Check if on ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundSensitivity, ~playerLayer);
        if (isGrounded) {airJumps = maxAirJumps;}

        // Velocity is previous velocity + the new input to a maximum in any given direction
        Vector2 newVelocity = rb.linearVelocity;
        
        // Check if moving
        if (moveDir.x != 0) {
            newVelocity.x = rb.linearVelocity.x + (moveDir.x * accel);
            newVelocity.x = Mathf.Clamp(newVelocity.x, -topSpeed, topSpeed);
        } else
        {
            newVelocity.x = rb.linearVelocity.x * horDrag;
            if (newVelocity.x <= 0.1f * topSpeed)
            {
                newVelocity.x = 0;
            }
        }

        if (jump) {
            newVelocity.y = jumpforce;
            jump = false;
        }

        if (glide) {
            newVelocity.y = Mathf.Clamp(newVelocity.y, -floatEffect, jumpforce);
        }

        rb.linearVelocity = newVelocity;
    }
}
