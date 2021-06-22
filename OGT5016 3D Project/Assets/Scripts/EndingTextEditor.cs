using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingTextEditor : MonoBehaviour
{
    //Text editor for robot serial number change
    
    [SerializeField] private TMP_Text finalText;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        
        int serialNumber =  PlayerPrefs.GetInt("SerialNumber");

        string name = "Virtus-" + CreateSerialNumber(serialNumber);
        
        if (finalText.text.Contains("nameText"))
        {
            //int index = dialog.sentences[i].IndexOf("nameText");
            finalText.text = finalText.text.Replace("nameText", name);


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private string CreateSerialNumber(int number)
    {
        if (number >= 1000)
        {
            return number.ToString();
        }
        else if(number >= 100)
        {
            return "0" + number.ToString();
        }
        else if(number >= 10)
        {
            return "00" + number.ToString();
        }
        else
        {
            return "000" + number.ToString();
        }
        
    }
}
