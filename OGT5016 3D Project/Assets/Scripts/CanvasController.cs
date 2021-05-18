using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;

    public bool levelStarts = false; //makes battery start to drain
    
    public float batteryCost = -1f; //each second battery drains 1 unit

    private int batteryColorValue = 0;
    
    private bool isGameRunning = true; //checks if game is running
    
     [SerializeField] private Slider battery; //slider for battery(health)
     
     private float batteryPercantage; //holds battery value;
     [SerializeField] private float maxBatteryValue = 100f; //max value for battery
     
     [SerializeField] private TMP_Text interactableText; //Shows up when player gets near to the interactible object
     [SerializeField] private TMP_Text pausedText; //pause screen text

     [SerializeField] private GameObject pausePanel; 
     [SerializeField] private GameObject winPanel;
     [SerializeField] private GameObject losePanel;
     [SerializeField] private GameObject filler; //batter slider filler image
     
     private Color red; //battery low percentage color
     private Color yellow; //battery medium percentage color
     private Color green; //battery high percentage color
     
     public float BatteryPercantage
     {
         get => batteryPercantage;
         set => batteryPercantage = value;
     }

     void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        
        battery.maxValue = maxBatteryValue;
        //save system will be added
        batteryPercantage = PlayerPrefs.GetFloat("BatteryPercentage"); //assign the saved value for battery
        battery.value = batteryPercantage;
        
        
        ColorUtility.TryParseHtmlString("#CE1212", out red); //assign red color
        ColorUtility.TryParseHtmlString("#184D47", out green); //assign green color
        ColorUtility.TryParseHtmlString("#FDCA40", out yellow); //assign yellow clor
        
        ColorValueChecker();

        //if player talked with NPC, battery starts to drain
        if (PlayerPrefs.GetInt("Talk") == 1)
        {
            levelStarts = true;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //makes battery start to drain
        if (levelStarts)
        {
            UpdateBatteryPercentage(batteryCost, false);
        }

        //pause the game. If press while game is paused, game continues
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameRunning)
            {
                Time.timeScale = 0;
                isGameRunning = false;
                pausePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pausedText.text = "PAUSED"; //pause text can be changed to "game saved", it reverts
                ContinueGame();
            }
        }
        
    }

    //updates battery percentage every second
    //if it is just draining the battery it calculates with time
    //if it is a damage it is an external update and it works seperately
    public void UpdateBatteryPercentage(float value, bool external)
    {
        if (!external)
        {
            batteryPercantage += (value * Time.deltaTime);
            battery.value = batteryPercantage;
        }
        else
        {
            batteryPercantage += value;
            battery.value = batteryPercantage;
        }

        if (battery.value <= 0)
        {
            LoseState();
        }
        
        //battery color change based on its percantage, it is a checker for that action
        ColorValueChecker();
        

    }

    //win panel opens, game world stops
    public void WinState()
    {
        Time.timeScale = 0;
        isGameRunning = false;
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        
    }
    
    //lose panel opens, game world stops
    public void LoseState()
    {
        Time.timeScale = 0;
        isGameRunning = false;
        losePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        
    }

    //makes unpause the game
    public void ContinueGame()
    {
        Time.timeScale = 1f;
        isGameRunning = true;
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    //function for interactable objects
    //it can be replaced with CustomInteractiveText function
    public void InteractableText(bool open)
    {
        if (open)
        {
            interactableText.text = "Press E to take";
        }
        else
        {
            interactableText.text = "";
        }
    }

    //function for placement objects
    //it can be replaced with CustomInteractiveText function
    public void HolderText(bool filled)
    {
        if (!filled)
        {
            interactableText.text = "Press F to place the box";
        }
        else
        {
            interactableText.text = "";
        }
    }
    
    //shows a text to player for direction when activated
    //it deletes text when it is deactivated
    public void CustomInteractiveText(bool enter, string s)
    {
        if (enter)
        {
            interactableText.text = s;
        }
        else
        {
            interactableText.text = "";
        }
    }
    
    //checks the battery(health) percantage
    //if percentage is medium changes color to yellow
    //if percantage is low changes color to red
    private void ColorValueChecker()
    {
        int c;
        
        if (battery.value >= battery.maxValue/2)
        {
            c = 0;
            if (c != batteryColorValue)
            {
              
                filler.GetComponent<Image>().color = green;
                batteryColorValue = c;
            }
        }
        else if (battery.value > battery.maxValue/4 )
        {
            c = 1;
            if (c != batteryColorValue)
            {
              
                filler.GetComponent<Image>().color = yellow;
                batteryColorValue = c;
            }
        }
        else
        {
            c = 2;
            if (c != batteryColorValue)
            {
                filler.GetComponent<Image>().color = red;
                batteryColorValue = c;
            }
        }
    }
    

}
