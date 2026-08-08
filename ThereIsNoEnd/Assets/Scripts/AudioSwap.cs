using UnityEngine;

public class AudioSwap : MonoBehaviour
{
    [SerializeField] public AudioClip zoneTrack;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            AudioManager.Instance.SwapTrack(zoneTrack);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            AudioManager.Instance.SwapToDefault();
        }
    }
}
