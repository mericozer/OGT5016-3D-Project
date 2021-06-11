using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    
    public Dialog dialog;
    
    [SerializeField] private TMP_Text NPCName; 

    [SerializeField] private TMP_Text dialogBox;
    
    [SerializeField] private GameObject dialogPanel;

   // [SerializeField] private Transform lookAtPoint; 

    private bool playerNear = false;

    [SerializeField] private List<int> serialNumberDialogIndex = new List<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        
        NPCName.text = dialog.name;

        int serialNumber =  PlayerPrefs.GetInt("SerialNumber");

        string name = "Virtus-" + CreateSerialNumber(serialNumber);
        //string name = "test";
        for (int i = 0; i < dialog.sentences.Length; i++)
        {
            if (dialog.sentences[i].Contains("nameText"))
            {
                //int index = dialog.sentences[i].IndexOf("nameText");
                dialog.sentences[i] = dialog.sentences[i].Replace("nameText", name);


            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.E)) //sends the dialog to the dialog manager
            {
                CanvasController.instance.CustomInteractiveText(false, "Press E to talk");
                PlayerController.instance.StopSound();
                dialogPanel.SetActive(true);
                
                DialogManager.instance.StartDialog(dialog, dialogBox);
            }  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(true, "Press E to talk");
            playerNear = true;
        }
    }
    
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(false, "Press E to talk");
            dialogPanel.SetActive(false);
            //change
            dialogBox.text = "";
            CanvasController.instance.levelStarts = true;
            playerNear = false;
            PlayerPrefs.SetInt("Talk", 1);
        }
    }
    
    private string CreateSerialNumber(int number)
    {
        if (number >= 1000)
        {
            return number.ToString();
        }
        else if(number >= 100)
        {
            return "0" + number.ToString();
        }
        else if(number >= 10)
        {
            return "00" + number.ToString();
        }
        else
        {
            return "000" + number.ToString();
        }
        
    }
    
   
}
