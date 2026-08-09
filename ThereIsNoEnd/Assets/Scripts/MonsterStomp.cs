using UnityEngine;

public class MonsterStomp : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weak Point")
        {
            Debug.Log("DESTROY MONSTER: " + collision.gameObject.name);
            collision.gameObject.GetComponentInParent<MonsterMovement>().DestroyMonster();
            GetComponentInParent<Movement>().PlayDoingDamage();
        } 
    }
}
