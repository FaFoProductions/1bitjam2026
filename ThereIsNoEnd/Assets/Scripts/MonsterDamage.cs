using UnityEngine;

public class MonsterDamage : MonoBehaviour
{

    public int damage;
    public PlayerHealth playerHealth;
    public Rigidbody2D playerRB;
    public Transform playerTransform;
    [SerializeField] private float knockbackForce = 2f;
    
    private void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        playerRB = player.GetComponent<Rigidbody2D>();
        playerTransform = player.transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player takes damage");
            playerHealth.TakeDamage(damage);
            if (gameObject.tag == "Enemy") {
                collision.gameObject.GetComponent<Movement>().PlayTakingDamage();
            } else if (gameObject.tag == "Spike") {
                collision.gameObject.GetComponent<Movement>().PlaySpikeDamage();
            }
            KnockBackPlayer();
        }
    }

    private void KnockBackPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        playerRB.AddForce(direction * knockbackForce);
    }  


}
