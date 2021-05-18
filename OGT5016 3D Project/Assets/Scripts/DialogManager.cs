using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    
    //Takes dialog sounds if NPC have
    public AudioSource soundSource;
    public AudioClip[] audioList;
    
    public Queue<string> sentenceQueue; //takes all the dialog sentences as a queue

    private TMP_Text dialogText; 

    private bool onDialog = false; //checks if dialog continues

    //dialogs forms as sentences
    //each sentence has a unique sound clip
    //with each sentence an audio clip plays
    private int formerSound = 0; //holds the former sound index
    private int currentSound = 0; //holds the current sound index
    
    //when dialog event ends, game starts
    public UnityEvent OnDialogEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    
    void Awake()
    {
        instance = this;
        
        if (OnDialogEvent == null)
        {
            OnDialogEvent = new UnityEvent();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        sentenceQueue = new Queue<string>();
        
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onDialog)
        {
            //makes skip to next sentence
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GetNextSentence();
            }
        }
        
    }

    public void StartDialog(Dialog dialog, TMP_Text dialogBox)
    {
        Time.timeScale = 0f;
        onDialog = true;
        
        //clears the queue from former dialogs if they exist
        sentenceQueue.Clear();

        //takes the current dialog sentences and puts in a queue
        foreach (string sentence in dialog.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }

        
        dialogText = dialogBox;
        GetNextSentence();
        
        
    }

    //makes sentences show up letter by letter in dialog box
    IEnumerator LetterByLetter(string sentence)
    {
        dialogText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
           
            dialogText.text += letter;
            
            yield return null;
        }
        
    }
   
    private void GetNextSentence()
    {
        if (sentenceQueue.Count == 0)
        {
            EndDialog();
        }
        else
        {
            string sentence = sentenceQueue.Dequeue();
            StopAllCoroutines();
            StartCoroutine(LetterByLetter(sentence));

            //checks if former sentences audio clip plays or not
            //if it plays, stops the audio clip and gets the next one
            if (formerSound != currentSound)
            {
                soundSource.Stop();
            }

            soundSource.PlayOneShot(audioList[currentSound]);
            formerSound = currentSound;
            currentSound++;
        }
    }

    //when dialog ends 
    //game starts
    private void EndDialog()
    {
        Time.timeScale = 1f;
        onDialog = false;
        OnDialogEvent.Invoke();
        soundSource.Stop();
        
    }
}
