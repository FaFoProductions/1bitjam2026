using UnityEngine;

public class RatKingEnemy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject body;

    private Transform playerTransform;

    public float chaseDistance = 5f;
    private bool isChasing;

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

        Vector2 dis = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dis, chaseDistance);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isChasing = true;

        } else
        {
            isChasing = false;
        }
    }
}
