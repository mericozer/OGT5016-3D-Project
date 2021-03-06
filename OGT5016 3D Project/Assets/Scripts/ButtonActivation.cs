using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActivation : MonoBehaviour
{
    //Button Activate script
    
    //Events helps to create a flexibility for button actions
    public UnityEvent OnPressEvent;

    private bool playerNear = false;
    private bool isPressed = false;
    
    public AudioSource soundSource; //buttons have their own activation sound
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    
    // Start is called before the first frame update
    void Start()
    {
        if (OnPressEvent == null)
        {
            OnPressEvent = new UnityEvent();
        }

        soundSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks if player is near to the button
        if (playerNear)
        {
            //Works with pressing E when near to the button
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isPressed)
                {
                    OnPressEvent.Invoke();
                    isPressed = true;
                    soundSource.Play();
                }
                
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed) //if button is pressed once do not activate again
        {
            CanvasController.instance.CustomInteractiveText(true, "Press E for Button");
            playerNear = true;
        }
    }
    
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.CustomInteractiveText(false, "Press E for Button");
            playerNear = false;
        }
    }

}
