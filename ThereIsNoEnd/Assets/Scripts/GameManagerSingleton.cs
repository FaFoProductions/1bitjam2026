using UnityEngine;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance { get; private set; }

    public Transform SpawnPoint;
    public GameObject player;
    [SerializeField] public AudioClip newDefault;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        RespawnPlayer();
        AudioManager.Instance.defaultAmbience = newDefault;
        AudioManager.Instance.SwapToDefault();
    }

    public void RespawnPlayer() {
        player.transform.position = SpawnPoint.position;
        Camera.main.transform.position = player.transform.position;
        player.GetComponent<PlayerHealth>().health = player.GetComponent<PlayerHealth>().maxHealth;
        AudioManager.Instance.SwapToDefault();
    }
}
