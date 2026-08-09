using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 10;
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage, string thing)
    {
        health -= damage;
        Debug.Log("The players health is: " + health);
        if (health <= 0) { 
            if (
                    GameManagerSingleton.Instance.reason != GameManagerSingleton.RespawnReason.Swim ||
                    GameManagerSingleton.Instance.reason != GameManagerSingleton.RespawnReason.Float ||
                    GameManagerSingleton.Instance.reason != GameManagerSingleton.RespawnReason.DoubleJump ||
                    GameManagerSingleton.Instance.reason != GameManagerSingleton.RespawnReason.Dash || 
                    GameManagerSingleton.Instance.reason != GameManagerSingleton.RespawnReason.Climb
                    ) {
                        if (thing == "Spike") {
                            GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Spike;
                        } else if (thing == "Enemy") {
                            GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Enemy;
                        } else if (thing == "Water") {
                            GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Water;
                        }
                    
                }
            GameManagerSingleton.Instance.RespawnPlayer();
        }
    }
}
