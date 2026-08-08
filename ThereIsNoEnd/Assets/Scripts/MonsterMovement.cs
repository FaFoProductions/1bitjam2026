using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination; // the patrolpoint the monster is currently moving towards

    public Transform playerTransform;
    public bool isChaching;
    public float chaseDistance;

    public int playerLayerMask;
    private bool firstChase = true;

    private void Update()
    {
        // Can we see the player?
        //RaycastHit2D hit;
        //if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
        Vector2 dis = (playerTransform.position - transform.position).normalized;
        if (Physics2D.Raycast(transform.position, dis, chaseDistance, playerLayerMask))
        {
            Debug.Log("We are chasing!!!");
            isChaching = true;
            // Flip character to towards the player
    
            {
                // If the player is on the left

                // If the player is on the right
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                firstChase = false;
            }
        } else
        {
            isChaching = false;
        }

        if (isChaching) {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            // We need to look at the player
            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            } else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        } else {
            if(patrolDestination == 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                if(Vector2.Distance(transform.position, patrolPoints[0].position) < .5f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    patrolDestination = 1;
                }
            }
            if (patrolDestination == 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, patrolPoints[1].position) < .5f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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


