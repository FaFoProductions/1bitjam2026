using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject characterName;
    [SerializeField] private GameObject dialogueObj;

    private float timer;

    private string[] localDialogue;

    private int i = 0;
    
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

    public void PlayDialogue(Dialogue dialogue)
    {
        i = 0;
        characterName.GetComponent<TextMeshProUGUI>().text = dialogue.characterName;
        localDialogue = dialogue.dialogue;
        LoadNextLine();
    }

    private void LoadNextLine()
    {
        float wordTime = localDialogue[i].Split(" ").Length / 3;
        if (wordTime < 3)
        {
            timer = 3; 
        } else
        {
            timer = wordTime;
        }
        
        dialogueObj.GetComponent<TextMeshProUGUI>().text = localDialogue[i];
        i++;
    }

    public void Update()
    {
        if(timer > 0 )
        {
            timer -= Time.deltaTime;
        } else
        {
            if(i < localDialogue.Length)
            {
                LoadNextLine();
            }
            
        }
    }
}
