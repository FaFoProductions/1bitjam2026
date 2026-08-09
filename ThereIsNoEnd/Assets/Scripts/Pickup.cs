using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum Power
    {
        Swim,
        Float,
        Dash,
        DoubleJump,
        Climb
    }

    [SerializeField] public Power upgrade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player picked powerup "+upgrade);
            Movement playerStats = collision.gameObject.GetComponent<Movement>();
            switch (upgrade)
            {
                case Power.Swim: playerStats.canGoWater = true; GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Swim;
                break;
                case Power.Float: playerStats.canGoFloat = true; GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Float;
                break;
                case Power.Dash: playerStats.canGoDash = true; GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Dash;
                break;
                case Power.DoubleJump: playerStats.maxAirJumps = 1; GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.DoubleJump;
                break;
                case Power.Climb: playerStats.canGoClimb = true; GameManagerSingleton.Instance.reason = GameManagerSingleton.RespawnReason.Climb;
                break;
            }
            Destroy(gameObject);
        }
    }

}
