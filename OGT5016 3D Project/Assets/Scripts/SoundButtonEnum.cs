using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonEnum : MonoBehaviour
{
    public enum Order
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

    public Order buttonOrder;

    public bool isTurn;
    public bool isPressed = false;
    private bool playerNear;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isTurn)
                {
                    isPressed = true;
                    //TO DO
                    //sound play
                }
                else
                {
                    CanvasController.instance.FailLoseState();
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            CanvasController.instance.CustomInteractiveText(true, "'E' to play sound");
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
}
