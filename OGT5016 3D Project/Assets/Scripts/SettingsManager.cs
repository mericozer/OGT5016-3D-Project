using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle fullScreen;

    [SerializeField] private Sprite fullScreenOn; //full screen sprite for button
    [SerializeField] private Sprite fullScreenOff; //full screen off sprite for the button

    [SerializeField] private Button screenButton; //screen adjustment button
    
    // Start is called before the first frame update
    void Start()
    {
       CheckFullScreen(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //changes screen to fullscreen
    public void FullScreen()
    {
        if (PlayerPrefs.GetInt("FullScreen") == 0)
        {
            Screen.fullScreen = false;
            //fullScreen.isOn = false;
            PlayerPrefs.SetInt("FullScreen" , 1);
            screenButton.GetComponent<Image>().sprite = fullScreenOff;
            
        }
        else
        {
            Screen.fullScreen = true;
            //fullScreen.isOn = true;
            PlayerPrefs.SetInt("FullScreen" , 0);
            screenButton.GetComponent<Image>().sprite = fullScreenOn;
        }
    }

    //checks what is the last state of the screen
    //adjust the sprite 
    private void CheckFullScreen()
    {
        if (!PlayerPrefs.HasKey("FullScreen"))
        {
            PlayerPrefs.SetInt("FullScreen" , 0);
        }
        else
        {
            if (PlayerPrefs.GetInt("FullScreen") == 0)
            {
                //Screen.fullScreen = true;
                //fullScreen.isOn = true;
                screenButton.GetComponent<Image>().sprite = fullScreenOn;
            }
            else
            {
                //Screen.fullScreen = false;
                //fullScreen.isOn = false;
                screenButton.GetComponent<Image>().sprite = fullScreenOff;
            }
            
        }
    }
}
