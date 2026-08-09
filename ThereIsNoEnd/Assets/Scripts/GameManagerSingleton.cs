using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance { get; private set; }

    public Transform SpawnPoint;
    public GameObject player;
    [SerializeField] public AudioClip newDefault;
    private bool firstDialogue = true;

    [SerializeField] private List<Dialogue> thingsToSay = new List<Dialogue>(); 

    [SerializeField] private Dialogue[] dashDia;
    [SerializeField] private Dialogue[] doubleDia;
    [SerializeField] private Dialogue[] floatDia;
    [SerializeField] private Dialogue[] swimDia;
    [SerializeField] private Dialogue[] climbDia;

    [SerializeField] private Dialogue spikeDia;
    [SerializeField] private Dialogue waterDia;
    [SerializeField] private Dialogue enemyDia;

    [SerializeField] private Dialogue ralphDia;

    [SerializeField] private GameObject narratorObj;

    public RespawnReason reason;

    public enum RespawnReason
    {
        None,
        Swim,
        Float,
        Dash,
        DoubleJump,
        Climb,
        Water,
        Spike,
        Enemy
    }
    
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
        Invoke(nameof(SwapDefaultTrack),15f);
    }
    private void SwapDefaultTrack() {
        AudioManager.Instance.defaultAmbience = newDefault;
        AudioManager.Instance.SwapToDefault();
    }

    public void RespawnPlayer() {
        StopAllCoroutines();
        player.transform.position = SpawnPoint.position;
        Camera.main.transform.position = player.transform.position;
        player.GetComponent<PlayerHealth>().health = player.GetComponent<PlayerHealth>().maxHealth;
        AudioManager.Instance.SwapToDefault();

        // Respawn dialogue loading
        thingsToSay = new List<Dialogue>();
        if (firstDialogue) {
            thingsToSay.Add(ralphDia);
            firstDialogue = false;
        }

        switch (reason)
        {
            case RespawnReason.None: ;
            break;
            case RespawnReason.Swim: foreach (Dialogue d in swimDia) {thingsToSay.Add(d);};
            break;
            case RespawnReason.Float: foreach (Dialogue d in floatDia) {thingsToSay.Add(d);};
            break;
            case RespawnReason.Dash: foreach (Dialogue d in dashDia) {thingsToSay.Add(d);};
            break;
            case RespawnReason.DoubleJump: foreach (Dialogue d in doubleDia) {thingsToSay.Add(d);};
            break;
            case RespawnReason.Climb: foreach (Dialogue d in climbDia) {thingsToSay.Add(d);};
            break;
            case RespawnReason.Spike: thingsToSay.Add(spikeDia);
            break;
            case RespawnReason.Water: thingsToSay.Add(waterDia);
            break;
            case RespawnReason.Enemy: thingsToSay.Add(enemyDia);
            break;
        }

        if (thingsToSay.Count > 0) {StartCoroutine(LoadDia());}
        reason = RespawnReason.None;
    }

    IEnumerator LoadDia() {
        while (thingsToSay.Count > 0) {
            narratorObj.GetComponent<NPCDialogue>().dialogue = thingsToSay[0];
            narratorObj.GetComponent<NPCDialogue>().DialogueStart();
            thingsToSay.RemoveAt(0);
            float waitTime = DialogueManager.Instance.totalDiaTime + 1.5f;
            if (waitTime <= 3) {
                waitTime = 3;
            }
            yield return new WaitForSeconds(waitTime);
        }
        narratorObj.GetComponent<NPCDialogue>().DialogueStop();
    }
}
