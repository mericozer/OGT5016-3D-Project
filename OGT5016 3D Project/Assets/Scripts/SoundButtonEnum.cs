using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonEnum : MonoBehaviour
{
    //buttons order in the sequence can be selected
    public enum Order
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

    public Order buttonOrder;

    public AudioSource soundSource;
    //public AudioClip[] audioList;

    [SerializeField] private Material whiteMat; //material while button is not active
    [SerializeField] private Material activeMat; //while button is active
    

    [SerializeField] private GameObject button;
    public bool isTurn; //if button have the turn
    public bool isPressed = false; //if button is pressed
    private bool playerNear;
    private bool isActive = false;
    private bool firstListen = true; //if not first listen player losts 10 units from batery
    
    
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            //the difference between activate and listen is
            //player can listen many times
            //but if player activates the button and it is the wrong button 
            //test fails
            
            if (Input.GetKeyDown(KeyCode.E)) //activate
            {
                if (isTurn)
                {
                    isPressed = true;
                    soundSource.Play();
                    isActive = true;
                    button.GetComponent<MeshRenderer>().material = activeMat;
                }
                else
                {
                    CanvasController.instance.FailLoseState();
                }

            }

            if (Input.GetKeyDown(KeyCode.F)) //listen
            {
                button.GetComponent<MeshRenderer>().material = activeMat;
                StartCoroutine(ListenDelay());
                
                if (firstListen)
                {
                    firstListen = false;
                    soundSource.Play();
                }
                else
                {
                    CanvasController.instance.UpdateBatteryPercentage(-10, true);
                    soundSource.Play();
                }
                
            }
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (!isActive)
            {
                CanvasController.instance.CustomInteractiveText(true, "'E' to activate | 'F' to listen");
            }
            else
            {
                CanvasController.instance.CustomInteractiveText(true, "");
            }
            
        }
    }
    
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            CanvasController.instance.CustomInteractiveText(false, "'E' to play sound");
        }
    }

    IEnumerator ListenDelay()
    {
        yield return new WaitForSeconds(1f);
        button.GetComponent<MeshRenderer>().material = whiteMat;
    }
}
