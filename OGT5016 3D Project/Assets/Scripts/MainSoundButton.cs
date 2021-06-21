using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class MainSoundButton : MonoBehaviour
{
    [SerializeField] private Material greenMat;
    private Material brownMat;

    [SerializeField] private GameObject button;

    [SerializeField] private PuzzleRoomController roomController;

    [SerializeField] private List<int> soundOrder;

    public AudioSource soundSource;
    public AudioClip[] audioList;
    
    private bool playerNear = false;
    private bool isActive = false;

    private bool playing = false;

    private float delayTime = 1f;

    private int soundNumber =-1;
    // Start is called before the first frame update
    void Start()
    {
        soundSource = GetComponent<AudioSource>();
        soundOrder = roomController.orderNumber;

        brownMat = button.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            /*if (!isActive)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //StartCoroutine(PlaySound(0));
                    
                    delayTime = 0f;
                    soundNumber = -1;
                    playing = true;
                    
                    button.GetComponent<MeshRenderer>().material = greenMat;
                    Debug.Log("Main Sound played");
                }
            
            }
            else
            {
                //play sound
                CanvasController.instance.UpdateBatteryPercentage(-10, true);
                Debug.Log("Main Sound played multiple");
            }*/

            if (Input.GetKeyDown(KeyCode.E))
            {
                delayTime = 0f;
                soundNumber = -1;
                playing = true;
                    
                button.GetComponent<MeshRenderer>().material = greenMat;
                
                if (isActive)
                {
                    CanvasController.instance.UpdateBatteryPercentage(-10, true);
                }
                else
                {
                    isActive = true;
                }
            }
        }

        if (playing)
        {
            delayTime += Time.deltaTime;
            if (delayTime >= 1f)
            {
                soundNumber++;
                
                if (soundNumber < 4)
                {
                    
                    soundSource.PlayOneShot(audioList[soundOrder[soundNumber] - 1]);
                    delayTime = 0f;
                }
                else
                {
                    delayTime = 0f;
                    soundNumber = -1;
                    playing = false;
                    button.GetComponent<MeshRenderer>().material = brownMat;
                }
                
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
    
    
   
    /*private IEnumerator PlaySound(int i)
    {
       /* if (i < soundOrder.Count)
        {
            Debug.Log("playing sound index is : " + i);
            soundSource.PlayOneShot(audioList[soundOrder[i]]);
            yield return new WaitForSeconds(1f);
            StartCoroutine(PlaySound(i + 1));
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            //yield return null;
        }

       if (i == 3)
       {
           soundSource.PlayOneShot(audioList[soundOrder[i]]);
           
       }
       else
       {
           StartCoroutine(PlaySound(i + 1));
       }

    }*/
}
