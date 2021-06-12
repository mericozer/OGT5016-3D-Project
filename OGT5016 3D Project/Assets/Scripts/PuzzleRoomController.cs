using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleRoomController : MonoBehaviour
{
    private int current = 0;

    [SerializeField] private GameObject hintText;
    
    private SoundButtonEnum currentButton;
    
    [SerializeField] List<SoundButtonEnum> buttonList = new List<SoundButtonEnum>();
    [SerializeField] private List<SoundButtonEnum> orderList = new List<SoundButtonEnum>();
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
            foreach (var button in buttonList)
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
            if (orderList[current].isPressed)
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
