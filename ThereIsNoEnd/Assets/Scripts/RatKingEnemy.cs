using UnityEngine;

public class RatKingEnemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject body;
    [SerializeField] private float accel;
    [SerializeField] private float topSpeed;
    [SerializeField] private float deAccel;

    private Transform playerTransform;

    public float chaseDistance = 5f;
    private bool isChasing;
    private Vector2 moveDir;

    private float counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate() {

        // Rotation system
        counter -= Time.deltaTime;
        Debug.Log(counter);
        if (counter < 0) {
            counter = 1.125f;
        }
        
        body.transform.rotation = Quaternion.Euler(0, 0, Mathf.Floor(counter * 40));

        // Chase system
        Vector2 dis = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dis, chaseDistance);
        isChasing = hit.collider != null && hit.collider.CompareTag("Player")
        
        if (isChasing) {
            moveDir = (playerTransform.position - transform.position).normalized;
            moveDir.y = 0;
        }

    }
}
