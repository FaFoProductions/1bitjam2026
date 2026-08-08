using UnityEngine;

public class MonsterStomp : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("We are entering");
        if (collision.gameObject.tag == "Weak Point")
        {
            Debug.Log("DESTROY MONSTER: " + collision.gameObject.name);
            collision.gameObject.GetComponentInParent<MonsterMovement>().DestroyMonster();
        } 
    }
}
