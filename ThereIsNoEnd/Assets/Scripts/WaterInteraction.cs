using Unity.VisualScripting;
using UnityEngine;

public class Waterinteraction : MonoBehaviour
{ 
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            if (!player.GetComponent<Movement>().canGoWater) {
                player.GetComponent<PlayerHealth>().TakeDamage(player.GetComponent<PlayerHealth>().maxHealth);
            } else {
                player.GetComponent<Movement>().inWater = true;
                player.GetComponent<Movement>().EnteredWater();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            player.GetComponent<Movement>().inWater = false;
            player.GetComponent<Movement>().ExitWater();
        }
    }
}
