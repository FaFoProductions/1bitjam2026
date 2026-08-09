using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private GameObject characterName;
    [SerializeField] private GameObject dialogueObj;
    [SerializeField] private GameObject sprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingSound;

    private float timer;

    [SerializeField] private float waitTime = 0.05f;

    private string[] localDialogue;

    private int i = 0;
    public float wordTime;
    public float totalDiaTime;
    
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
        audioSource.clip = typingSound;
    }

    public void PlayDialogue(Dialogue dialogue)
    {
        i = 0;
        characterName.GetComponent<TextMeshProUGUI>().text = dialogue.characterName;
        localDialogue = dialogue.dialogue;
        sprite.GetComponent<Image>().sprite = dialogue.characterSprite;

        // Calculate total time
        totalDiaTime = 0;
        for (int i = 0; i < localDialogue.Length; i++) {
            totalDiaTime += calcDiaTime(localDialogue[i]);
        }
        LoadNextLine();
    }

    private float calcDiaTime(string s) {
        return s.ToCharArray().Length * waitTime * 1.5f;
    }

    private void StopTyping() {
        audioSource.Stop();
    }

    private void LoadNextLine()
    {
        audioSource.Play();
        Invoke(nameof(StopTyping), (totalDiaTime * 0.75f));
        wordTime = calcDiaTime(localDialogue[i]);
        if (wordTime < 3)
        {
            timer = 3; 
        } else
        {
            timer = wordTime;
        }
        
        dialogueObj.GetComponent<TextMeshProUGUI>().text = "";
        string theLine = localDialogue[i];
        StopAllCoroutines();
        StartCoroutine(WriteText(theLine));
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

    IEnumerator WriteText(string text)
    {
        foreach(char c in text.ToCharArray())
        {
            dialogueObj.GetComponent<TextMeshProUGUI>().text += c;
            yield return new WaitForSeconds(waitTime);
        }
    }
    
}
