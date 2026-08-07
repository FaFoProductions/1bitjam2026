using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


public class Movement : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundPos;
    [SerializeField] Transform leftWallPos;
    [SerializeField] Transform rightWallPos;


    [Header("Movement Settings")]
    public float topSpeed = 5f;
    public float horDrag = 0.9f;
    public float accel = 1f;
    public float jumpforce = 10;
    public float dashforce = 30;
    public float highSpeedGraceTimeInS = 2f;
    public float groundSensitivity = 0.2f;
    public float decayMod = 0.9f;
    public bool grounded = true;
    public bool walledLeft;
    public bool walledRight;
    public Vector2 moveDir;
    public int maxAirJumps = 0;
    public int airJumps = 0;
    public float wallJumpMod;
    // Float effect lower value is more floaty
    public float floatEffect = 0.2f;
    private float graceTime;
    private bool jump;
    private bool glide;
    private bool dash;
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
            if (grounded || walledLeft || walledRight || airJumps > 0)
            {
                if ((!grounded && !walledLeft && !walledRight) && airJumps > 0 ) {airJumps--;}
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
            dash = true;
        }
    }

    private bool isGrounded(Transform pos) {
        return Physics2D.OverlapCircle(pos.position, groundSensitivity, groundLayer);
    }

    void FixedUpdate()
    {
        // Count down speed grace if active
        if (graceTime > 0) {
            graceTime -= Time.deltaTime;
        }

        // Check if on ground
        grounded = isGrounded(groundPos);

        // Check if on wall
        walledLeft = isGrounded(leftWallPos);
        walledRight = isGrounded(rightWallPos);

        if (grounded || walledLeft || walledRight) {airJumps = maxAirJumps;}

        // Velocity is previous velocity + the new input to a maximum in any given direction
        Vector2 newVelocity = rb.linearVelocity;
        
        // Check if moving
        if (moveDir.x != 0) {
            newVelocity.x = rb.linearVelocity.x + (moveDir.x * accel);
            if (graceTime > 0) {
                newVelocity.x = Mathf.Clamp(newVelocity.x, 
                Mathf.Min(-topSpeed, (rb.linearVelocity.x)), 
                Mathf.Max(topSpeed, (rb.linearVelocity.x))
                );
            } else {
                newVelocity.x = Mathf.Clamp(newVelocity.x, 
                Mathf.Min(-topSpeed, (rb.linearVelocity.x * decayMod)), 
                Mathf.Max(topSpeed, (rb.linearVelocity.x * decayMod))
                );
            }

        } else
        {
            newVelocity.x = rb.linearVelocity.x * horDrag;
            if (Mathf.Abs(newVelocity.x) <= 0.1f * topSpeed)
            {
                newVelocity.x = 0;
            }
        }

        if (jump) {
            if (!grounded && walledLeft) {
                // Walljump from wall on the left
                newVelocity.x = jumpforce * wallJumpMod;
                graceTime = highSpeedGraceTimeInS;

            } else if (!grounded && walledRight) {
                // Walljump from wall on the right
                newVelocity.x = -(jumpforce * wallJumpMod);
                graceTime = highSpeedGraceTimeInS;
            }
            // Normal jump
            newVelocity.y = jumpforce;
            jump = false;
        }

        if (dash) {
            newVelocity.x = dashforce * Mathf.Sign(rb.linearVelocity.x);
            dash = false;
        }

        if (glide) {
            newVelocity.y = Mathf.Clamp(newVelocity.y, -floatEffect, jumpforce);
        }

        rb.linearVelocity = newVelocity;
    }
}
