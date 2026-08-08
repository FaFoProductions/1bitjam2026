using Unity.VisualScripting;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination; // the patrolpoint the monster is currently moving towards

    private Transform playerTransform;
    public bool isChaching;
    public float chaseDistance;

    public int playerLayerMask;


    private void Start() {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Can we see the player?
        
        //if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        Vector2 dis = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dis, chaseDistance);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("We are chasing!!!");
            isChaching = true;

 
        } else
        {
            Debug.Log("No chase");
            isChaching = false;
        }

        if (isChaching) {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            // We need to look at the player
            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            } else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        } else {
            if(patrolDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                if(Vector2.Distance(transform.position, patrolPoints[0].position) < 0.5f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    patrolDestination = 1;
                }
            }
            if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.5f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    patrolDestination = 0;
                }
            }
        }
    }

    public void DestroyMonster()
    {
        Destroy(gameObject);
    }

}


