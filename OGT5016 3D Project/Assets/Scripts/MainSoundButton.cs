using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSoundButton : MonoBehaviour
{
    [SerializeField] private Material greenMat;

    [SerializeField] private GameObject button;


    private bool playerNear = false;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (!isActive)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //play sound
                    button.GetComponent<MeshRenderer>().material = greenMat;
                    Debug.Log("Main Sound played");
                }
            
            }
            else
            {
                //play sound
                CanvasController.instance.UpdateBatteryPercentage(-10, true);
                Debug.Log("Main Sound played multiple");
            }
        }
       
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(true, "'E' to play sound");
            playerNear = true;
        }
    }
    
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(false, "'E' to play sound");
            playerNear = false;
        }
    }
}
