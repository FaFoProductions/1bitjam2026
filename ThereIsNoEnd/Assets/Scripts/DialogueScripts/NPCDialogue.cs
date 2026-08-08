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


    public void DialogueStart()
    {
        dialogbox.SetActive(true);
        alertBubble.SetActive(false);
        DialogueManager.Instance.PlayDialogue(dialogue);
    }

    public void DialogueStop() {
        Debug.Log("Dialog start");
        dialogbox.SetActive(false);
        alertBubble.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Entered trigger");
            DialogueStart();
            //player = collision.gameObject.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            DialogueStop();
        }
    }

    // The function must be public and take 0 or 1 parameters
    public void TriggerTimelineAction()
    {
        Debug.Log("The Unity Timeline just triggered this script!");
        // Add your custom gameplay logic here (e.g., spawn enemies, change UI)
    }
}

