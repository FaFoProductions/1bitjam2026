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

    [Header("Animation Settings")]
    [SerializeField] public Animator animator;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip[] wetFootstepClips;
    [SerializeField] private AudioClip[] takingDamageClips;
    [SerializeField] private AudioClip spikeSound;
    [SerializeField] private AudioClip doingDamageSound;
    [SerializeField] private AudioClip jumpingSound;
    [SerializeField] private AudioClip floatSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] AudioSource footstepSource;
    [SerializeField] AudioSource dashSource;
    [SerializeField] AudioSource floatSource;
    [SerializeField] public float dashAnimLength;

    [Header("Particle Settings")]
    [SerializeField] GameObject dashParticlePrefab;
    [SerializeField] GameObject jumpParticlePrefab;
    [SerializeField] Transform jumpParticlePos;

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
    public int airJumps = 0;
    public float wallJumpMod;
    public float floatEffect = 0.2f;
    public float climbEffect = 0.6f;
    public bool inWater = false;
    public bool canGoWater = false;
    public bool canGoFloat = false;
    public bool canGoDash = false;
    public int maxAirJumps = 0;
    public bool canGoClimb = false;
    private float graceTime;
    private float dashTime;
    private bool jump;
    private bool glide;
    private bool dash;
    private bool dashAvailable = true;
    private bool glideSoundPlayed = false;
    private float speedMod = 1;
    private bool gliding = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
            glideSoundPlayed = false;
        }
    }

    void DashPerformed(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started)
        {
            dash = true;
            if (canGoDash) {
                dashTime = dashAnimLength;
            }
        }
    }

    void UpdateAnimVariables(Vector2 dir) {
        animator.SetFloat("xSpeed", Mathf.Abs(dir.x));
        animator.SetFloat("ySpeed", Mathf.Abs(dir.y));
        animator.SetBool("isGrounded", grounded);
        animator.SetBool("walledLeft", walledLeft);
        animator.SetBool("walledRight", walledRight);
        animator.SetBool("dashing", (dashTime > 0));
        animator.SetBool("gliding", gliding);

        if (Mathf.Sign(dir.y) >= 0) {
            animator.SetBool("ySpeedIsUp", true);
        } else {
            animator.SetBool("ySpeedIsUp", false);
        }

    }

    public void PlayFootstepSound()
    {   
        if (inWater) {
            int randomIndex = Random.Range(0, wetFootstepClips.Length);
            footstepSource.clip = wetFootstepClips[randomIndex];
        } else {
            int randomIndex = Random.Range(0, footstepClips.Length);
            footstepSource.clip = footstepClips[randomIndex];
        }
        
        
        footstepSource.Play();
    }

    public void PlayJumpingSound()
    {
        footstepSource.clip = jumpingSound;
        
        footstepSource.Play();
    }

    public void PlayFloatSound()
    {
        floatSource.clip = floatSound;
        
        floatSource.Play();
    }

    public void PlayDashSound()
    {
        dashSource.clip = dashSound;
        
        dashSource.Play();
    }

    public void PlayTakingDamage() {
        int randomIndex = Random.Range(0, takingDamageClips.Length);
        dashSource.clip = takingDamageClips[randomIndex];
    }
    public void PlayDoingDamage() {
        dashSource.clip = doingDamageSound;
        
        dashSource.Play();
    }
    public void PlaySpikeDamage() {
        dashSource.clip = spikeSound;
        
        dashSource.Play();
    }

    public void PlayJumpParticles(Transform t) {
        Instantiate(jumpParticlePrefab, t.position, Quaternion.Euler(90,0,0));
    }

    public void PlayDashParticles(Transform t, Vector2 moveDir) {
        GameObject dashParticle = Instantiate(dashParticlePrefab, t.position, Quaternion.Euler(0,90 * Mathf.Sign(moveDir.x) * -1,0));
        dashParticle.GetComponent<ParticleSystem>().startSpeed = dashforce * 0.5f;
    }

    private bool isGrounded(Transform pos) {
        return Physics2D.OverlapCircle(pos.position, groundSensitivity, groundLayer);
    }

    public void EnteredWater() {
        speedMod = 0.6f;
        animator.speed = speedMod;
    }
    public void ExitWater() {
        speedMod = 1;
        animator.speed = speedMod;
    }

    void FixedUpdate()
    {
        // Count down speed grace if active
        if (graceTime > 0) {
            graceTime -= Time.deltaTime;
        }

        if (dashTime > 0) {
            dashTime -= Time.deltaTime;
        }

        // Check if on ground
        grounded = isGrounded(groundPos);
        if (grounded) {dashAvailable = true;}

        // Check if on wall
        walledLeft = isGrounded(leftWallPos);
        walledRight = isGrounded(rightWallPos);

        if (grounded || ((walledLeft || walledRight) && canGoClimb)) {airJumps = maxAirJumps;}

        // Velocity is previous velocity + the new input to a maximum in any given direction
        Vector2 newVelocity = rb.linearVelocity;
        
        // Check if moving
        if (moveDir.x != 0) {
            newVelocity.x = rb.linearVelocity.x + (moveDir.x * accel * speedMod);
            if (graceTime > 0) {
                newVelocity.x = Mathf.Clamp(newVelocity.x, 
                Mathf.Min(-topSpeed * speedMod, (rb.linearVelocity.x * speedMod)), 
                Mathf.Max(topSpeed * speedMod, (rb.linearVelocity.x * speedMod))
                );
            } else {
                newVelocity.x = Mathf.Clamp(newVelocity.x, 
                Mathf.Min(-topSpeed * speedMod, (rb.linearVelocity.x * decayMod * speedMod)), 
                Mathf.Max(topSpeed * speedMod, (rb.linearVelocity.x * decayMod * speedMod))
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
            if (!grounded && walledLeft && canGoClimb) {
                // Walljump from wall on the left
                newVelocity.x = jumpforce * wallJumpMod;
                graceTime = highSpeedGraceTimeInS;
                PlayJumpParticles(leftWallPos);
                // Normal jump
                newVelocity.y = jumpforce;
                PlayJumpingSound();
            } else if (!grounded && walledRight && canGoClimb) {
                // Walljump from wall on the right
                newVelocity.x = -(jumpforce * wallJumpMod);
                graceTime = highSpeedGraceTimeInS;
                PlayJumpParticles(rightWallPos);
                // Normal jump
                newVelocity.y = jumpforce;
                PlayJumpingSound();
            } else if (grounded) {
                PlayJumpParticles(jumpParticlePos);
                // Normal jump
                newVelocity.y = jumpforce;
                PlayJumpingSound();
            } else if (airJumps > 0) {
                // Normal jump
                newVelocity.y = jumpforce;
                PlayJumpingSound();
            }
            jump = false;
        }

        if (dash) {
            if (dashAvailable && canGoDash) {
                if (moveDir.x != 0) {
                    newVelocity.x = dashforce * Mathf.Sign(moveDir.x);
                    dashAvailable = false;
                    PlayDashSound();
                    if (Mathf.Sign(moveDir.x) > 0) {
                        PlayDashParticles(leftWallPos, moveDir);
                    } else if (Mathf.Sign(moveDir.x) < 0) {
                        PlayDashParticles(rightWallPos, moveDir);
                    }
                }
            }
            dash = false;
        }

        if (glide) {
            if (canGoFloat) {
                newVelocity.y = Mathf.Clamp(newVelocity.y, -floatEffect, jumpforce);
                if (Mathf.Sign(newVelocity.y) < 0) {
                    if (glideSoundPlayed == false) {
                        PlayFloatSound();
                        glideSoundPlayed = true;
                    }
                    gliding = true;
                } else {
                    gliding = false;
                }
            }
        } else if ((walledLeft || walledRight) && canGoClimb) {
            newVelocity.y = Mathf.Clamp(newVelocity.y, -climbEffect, jumpforce);
        }

        UpdateAnimVariables(newVelocity.normalized);
        

        // Flip sprite if new movement is to the left instead of default (right) or for wall scenario
        if ((walledLeft || walledRight) && !grounded) {
            if (walledRight) {
                GetComponent<SpriteRenderer>().flipX = true;
            } else if (walledLeft) {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        } else {
            if (newVelocity.x < 0) {
                GetComponent<SpriteRenderer>().flipX = true;
            } else if (newVelocity.x > 0){
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }


        rb.linearVelocity = newVelocity;
    }
}
