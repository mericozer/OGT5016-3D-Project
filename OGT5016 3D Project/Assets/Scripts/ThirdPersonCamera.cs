using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    [SerializeField] private Transform player;

    private Vector3 offset;
    
    private float x;
    private float y;
    private float z;
    // Start is called before the first frame update
    void Start()
    {
        //checks if there is a saved game
        //if there is puts the camera to saved position
        
       /* if (!PlayerPrefs.HasKey("cameraX"))
        {
            offset = transform.position - player.position;
            PlayerPrefs.SetFloat("cameraX",offset.x);
            PlayerPrefs.SetFloat("cameraY",offset.y);
            PlayerPrefs.SetFloat("cameraZ",offset.z);
            
        }
        else
        {
            x = PlayerPrefs.GetFloat("cameraX");
            y = PlayerPrefs.GetFloat("cameraY");
            z = PlayerPrefs.GetFloat("cameraZ");
            offset = new Vector3(x, y, z);
        }*/

       //without save close it after development
       offset = transform.position - player.position;
      
        
    }

    // Update is called once per frame
    void Update()
    {
        float nextAngle = player.transform.eulerAngles.y;
        
        Quaternion nextRotation = Quaternion.Euler(0, nextAngle, 0);

        transform.position = player.position + (nextRotation * offset);
        
        transform.LookAt(player);
    }
}
