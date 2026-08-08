using Unity.VisualScripting;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
     public Dialogue dialogue;
     private Transform player;

     private DialogueManager dialogueManager;

     private bool dialogueInitiated;

     [SerializeField] public GameObject dialogbox; 
     [SerializeField] public GameObject alertBubble;


    void Start()
    {
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Entered trigger");
            dialogbox.SetActive(true);
            alertBubble.SetActive(false);
            DialogueManager.Instance.PlayDialogue(dialogue);
            player = collision.gameObject.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
        
            dialogbox.SetActive(false);
            alertBubble.SetActive(true);

        }
    }
}

