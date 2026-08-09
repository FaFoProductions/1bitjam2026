using UnityEngine;

public class EndingScript : MonoBehaviour
{

    [SerializeField] private GameObject endingTimeline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            endingTimeline.SetActive(true);
        }
    }
}
