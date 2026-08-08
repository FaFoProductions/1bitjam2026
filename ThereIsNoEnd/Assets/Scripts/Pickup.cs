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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player picked powerup "+upgrade);
            Movement playerStats = collision.gameObject.GetComponent<Movement>();
            switch (upgrade)
            {
                case Power.Swim: playerStats.canGoWater = true;
                return;
                case Power.Float: playerStats.canGoFloat = true;
                return;
                case Power.Dash: playerStats.canGoDash = true;
                return;
                case Power.DoubleJump: playerStats.maxAirJumps = 1;
                return;
                case Power.Climb: playerStats.canGoClimb = true;
                return;
            }
            Destroy(gameObject);
        }
    }

}
