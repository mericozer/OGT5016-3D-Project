using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dialog object for dialog manager

[System.Serializable]

public class Dialog
{
    public string name;

    [TextArea(2, 5)] 
    public string[] sentences;
}
