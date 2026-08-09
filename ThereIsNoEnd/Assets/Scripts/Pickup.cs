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
                case Power.Swim: playerStats.canGoWater = true;
                break;
                case Power.Float: playerStats.canGoFloat = true;
                break;
                case Power.Dash: playerStats.canGoDash = true;
                break;
                case Power.DoubleJump: playerStats.maxAirJumps = 1;
                break;
                case Power.Climb: playerStats.canGoClimb = true;
                break;
            }
            Destroy(gameObject);
        }
    }

}
