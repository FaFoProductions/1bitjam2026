using UnityEngine;

public class RatKingEnemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject body;
    [SerializeField] private float accel;
    [SerializeField] private float topSpeed;
    [SerializeField] private float deAccel;
    [SerializeField] private AudioClip rollSound;
    [SerializeField] AudioSource rollSource;

    private Transform playerTransform;

    public int health;
    public float chaseDistance = 5f;
    private bool isChasing;
    private Vector2 moveDir;
    private Vector2 newVelocity;
    private bool rollingPlaying = false;

    private float counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        rollSource.clip = rollSound;
    }

    private void FixedUpdate() {

        // Rotation system
        counter -= Time.deltaTime;
        if (counter < 0) {
            counter = 0.5625f;
        }

        
        body.transform.rotation = Quaternion.Euler(0, 0, Mathf.Floor(counter * 80));

        // Chase system
        Vector2 dis = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dis, chaseDistance);
        isChasing = hit.collider != null && hit.collider.CompareTag("Player");
        

        if (isChasing) {
            moveDir = (playerTransform.position - transform.position).normalized;
            if (Mathf.Sign(moveDir.x) > 0) {
                moveDir.x = 1;
            } else {
                moveDir.x = -1;
            }
            newVelocity.x = rb.linearVelocity.x + (moveDir.x * accel);
            newVelocity.x = Mathf.Clamp(newVelocity.x, 
                Mathf.Min(-topSpeed, rb.linearVelocity.x), 
                Mathf.Max(topSpeed, rb.linearVelocity.x)
                );
        } else {
            newVelocity = rb.linearVelocity * deAccel;
        }

        // Sound system
        if (Mathf.Abs(newVelocity.x) > 0.1 && !rollingPlaying) {
            rollSource.Play();
            rollingPlaying = true;
        } else if (Mathf.Abs(newVelocity.x) < 0.1 && rollingPlaying) {
            rollSource.Stop();
            rollingPlaying = false;
        }

        if (moveDir.x > 0) {
            sp.flipX = false;
        } else if (moveDir.x < 0) {
            sp.flipX = true;
        }
        rb.linearVelocity = newVelocity;
    }

    public void DamageRat(int i) {
        health -= i;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
}
