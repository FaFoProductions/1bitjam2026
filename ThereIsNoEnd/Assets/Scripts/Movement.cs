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
    }

    void Stop()
    {
        PlayerControls.PlayerMoveActions playerMove = InputManagerSingleton.Instance.playerControlActions.PlayerMove;
        InputManagerSingleton.Instance.UnBind(playerMove.Move, MovePerformed);
    }

    void MovePerformed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 targetDir = context.ReadValue<Vector2>();
            Debug.Log($"Target input dir{targetDir}");
            moveDir = targetDir.normalized;
        } else if (context.phase == InputActionPhase.Canceled)
        {
            moveDir = new Vector2(0,0);
        }
    }

    void FixedUpdate()
    {
        //Check if on ground
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundSensitivity, ~playerLayer);

        // Velocity is previous velocity + the new input to a maximum in any given direction of
        // Check if moving
        Vector2 newVelocity = rb.linearVelocity;
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

        if (moveDir.y != 0 && isGrounded)
        {
            newVelocity.y = jumpforce;
        }

        // Apply horizontal drag and clamp newVelocity x dir to topSpeed

        rb.linearVelocity = newVelocity;
    }
}
