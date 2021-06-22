using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Game Manager script for saving the game data and loading scenes
    
    public static GameManager instance; //Singleton

    //starting point of the player
    private float initialPositionX = 0; 
    private float initialPositionY = 0;
    private float initialPositionZ = -6.1f;
    private float initialBatteryPercentage = 300;
   
    //saved starting point of the player
    private float savedPositionX = 0;
    private float savedPositionY = 0;
    private float savedPositionZ = -6.1f;
    
    //saved batter value and current level
    private float savedBatteryPercentage = 300;
    private int currentLevel = 1;

    public int serialNumber;

    //saved position
    public Vector3 savedPos;

    [SerializeField] private Button continueButton;
    [SerializeField] private Text buttontext;

    [SerializeField] private GameObject player;

    [SerializeField] private bool isMenu;

    //continue button colors
    private Color enabled;
    private Color disabled;

    void Awake()
    {
        instance = this;
        
        ColorUtility.TryParseHtmlString("#6A6A6A", out disabled); 
        ColorUtility.TryParseHtmlString("#8A8A8A", out enabled);
        
        //if it is not a menu 
        //take the level related data
        if (!isMenu)
        {
            //if a saved game has take the saved one, if there arent any, create as the level 1
            if (!PlayerPrefs.HasKey("CurrentLevel"))
            {
               
                PlayerPrefs.SetInt("CurrentLevel", 1);
                currentLevel = PlayerPrefs.GetInt("CurrentLevel");

                 PlayerPrefs.SetInt("SerialNumber", 1);
                 serialNumber = PlayerPrefs.GetInt("SerialNumber");

            }
            else
            {
                int scene = SceneManager.GetActiveScene().buildIndex;
                PlayerPrefs.SetInt("CurrentLevel", scene);
            }

            //if some percentage is saved, also position is saved
            //checks if there is a saved one
            if (!PlayerPrefs.HasKey("BatteryPercentage"))
            {
                PlayerPrefs.SetFloat("BatteryPercentage", initialBatteryPercentage);
                PlayerPrefs.SetFloat("XPosition", initialPositionX);
                PlayerPrefs.SetFloat("YPosition", initialPositionY);
                PlayerPrefs.SetFloat("ZPosition", initialPositionZ);
            }
            /* else //load the saved percentage and positions
            {
               
                savedBatteryPercentage = PlayerPrefs.GetFloat("BatteryPercentage");
                savedPositionX = PlayerPrefs.GetFloat("XPosition");
                savedPositionY = PlayerPrefs.GetFloat("YPosition");
                savedPositionZ = PlayerPrefs.GetFloat("ZPosition");
               

                savedPos = new Vector3(savedPositionX, savedPositionY, savedPositionZ);

                player.transform.position = savedPos;
            }*/

        }
        //if it is a menu
        //check if there is a saved game already and activate the continue button
        else
        {
            
            if (!PlayerPrefs.HasKey("CurrentLevel"))
            {
                
                buttontext.color = disabled;
                continueButton.GetComponent<Button>().interactable = false;
            }
            else
            {

                buttontext.color = enabled;
                continueButton.GetComponent<Button>().interactable = true;
            }
                
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Deletes all preferences and opens a new game
    public void NewGame()
    {
        DeleteSaves();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //continue to the saved game 
    public void ContinueGame()
    {
       int scene = PlayerPrefs.GetInt("CurrentLevel");
       Debug.Log("CURRENT LEVEL NO IS" + scene);
       SceneManager.LoadScene(scene);
    }

    //restarts the level with deleting saved data
    public void Restart()
    {
        int serial = PlayerPrefs.GetInt("SerialNumber") + 1;
        int scene = SceneManager.GetActiveScene().buildIndex;
        DeleteSaves();
        PlayerPrefs.SetInt("CurrentLevel", scene);
        PlayerPrefs.SetInt("SerialNumber", serial);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //loads the next scene
    public void NextScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("CurrentLevel", scene + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
       
    }

    //opens main menu
    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

    //save game data
    //batter percentage
    //player position in the current level
    public void SaveGame()
    {
        PlayerPrefs.SetFloat("BatteryPercentage", CanvasController.instance.BatteryPercantage);
        PlayerPrefs.SetFloat("XPosition", player.transform.position.x);
        PlayerPrefs.SetFloat("YPosition", player.transform.position.y);
        PlayerPrefs.SetFloat("ZPosition", player.transform.position.z);
        
        //int scene = SceneManager.GetActiveScene().buildIndex;
        //PlayerPrefs.SetFloat("CurrentLevel", scene);
    }

    //finishing a level makes game saves and updates the current level data to next one
    public void LevelCompleteSave()
    {
        int scene = SceneManager.GetActiveScene().buildIndex + 1;
        PlayerPrefs.SetInt("CurrentLevel", scene);
        PlayerPrefs.SetFloat("BatteryPercentage", initialBatteryPercentage);
        PlayerPrefs.SetFloat("XPosition", initialPositionX);
        PlayerPrefs.SetFloat("YPosition", initialPositionY);
        PlayerPrefs.SetFloat("ZPosition", initialPositionZ);
        
        PlayerPrefs.SetInt("Talk" , 0);
    }

    //deletes all the preferences
    //except volume, screen setting
    public void DeleteSaves()
    {
        if (isMenu)
        {
            buttontext.color = disabled;
            continueButton.GetComponent<Button>().interactable = false;
        }
        
        int screen = PlayerPrefs.GetInt("FullScreen");
        float volume = PlayerPrefs.GetFloat("Volume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Fullscreen", screen);
        PlayerPrefs.SetInt("Talk" , 0);
        PlayerPrefs.SetFloat("Volume" , volume);
    }

    //cloese the game
    public void Exit()
    {
        Application.Quit();
    }


}
