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
    
    // Start is called before the first frame update
    void Start()
    {
        
        NPCName.text = dialog.name;
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
    
   
}
