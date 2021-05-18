using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDown : MonoBehaviour
{
    //OLD SCRIPT
    //WRITTEN FOR MOVING PANEL IN THE SECOND FLOOR
    //SCRIPT CHANGED TO THE "MOVEPANEL" 

    private bool isDown = false;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDown)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 1f);
        }
        
    }

    public void Down()
    {
        isDown = true;
    }


}
