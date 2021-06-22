using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleRoomController : MonoBehaviour
{
    //level 2 sound puzzle room controller
    //it checks whether correct sequence is played or not
    
    private int current = 0; //current button index

    [SerializeField] private GameObject hintText;
    
    private SoundButtonEnum currentButton;
    
    [SerializeField] List<SoundButtonEnum> buttonList = new List<SoundButtonEnum>(); //all buttons
    [SerializeField] private List<SoundButtonEnum> orderList = new List<SoundButtonEnum>(); //ordered sequence
    public List<int> orderNumber = new List<int>();
    
    public UnityEvent OnPressEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private bool playerNear;

    private bool completed = false;
    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        int counter = 1;
        for (int i = 0; i < 5; i++) 
        {
            counter = 1;
            foreach (var button in buttonList) //checks every button for every turn to order them
            {
                Debug.Log("turning");
                
                if ((int)button.buttonOrder == i)
                {
                    Debug.Log((int)button.buttonOrder);
                    orderList.Add(button);
                    orderNumber.Add(counter);
                    break;
                }

                counter++;
            }
        }

        orderList[current].isTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear && !completed)
        {
            if (orderList[current].isPressed) //if the correct button pressed change to the next button
            {
                orderList[current].isTurn = false;
                current++;
                Debug.Log("order list count: " + orderList.Count);
                if (current < orderList.Count)
                {
                    orderList[current].isTurn = true;
                    Debug.Log("heree");
                }
                else
                {
                    Debug.Log("orr here");
                    completed = true;
                    OnPressEvent.Invoke();
                    IncreaseTextAlpha();
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
            playerNear = true;
        }
    }
    
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
