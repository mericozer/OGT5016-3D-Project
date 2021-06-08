using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    //DONT FORGET AUDIO SOURCE
    
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject button;

    [SerializeField] private GameObject hintText;
    //[SerializeField] private TMP_Text hintText;

    [SerializeField] private Material greenMat;

    public bool isTurn = false;
    public bool isLast;
    [SerializeField] private bool isFirst;
    private bool playerNear = false;
    private bool isActive = false;
    
    public UnityEvent OnPressEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    
    // Start is called before the first frame update
    void Start()
    {
        if (isFirst)
        {
            isTurn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isActive)
                {
                        
                    if (!isLast)
                    {
                        if (isTurn)
                        {
                            nextButton.GetComponent<SoundButton>().isTurn = true;
                            //play sound
                            Debug.Log("Sound played");
                        }
                        else
                        {
                            CanvasController.instance.FailLoseState();
                        }
                            
                    }
                    else
                    {
                        if (isTurn)
                        {
                            OnPressEvent.Invoke();
                            IncreaseTextAlpha();
                            Debug.Log("Door Opened");
                        }
                        else
                        {
                            CanvasController.instance.FailLoseState();
                        }
                    }
                
                }
                else
                {
                    if (isLast && isTurn)
                    {
                        OnPressEvent.Invoke();
                        IncreaseTextAlpha();
                        Debug.Log("Door Opened");
                    }
                    button.GetComponent<MeshRenderer>().material = greenMat;
                    isActive = true;
                    Debug.Log("Sound played first time");
                }
            }
        }
        
       
       
    }

    private void IncreaseTextAlpha()
    {
        hintText.SetActive(true);
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
