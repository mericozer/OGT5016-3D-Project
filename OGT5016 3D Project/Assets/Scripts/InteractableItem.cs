using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    //At first, designed for multiple objects
    //In use for just the cubes
    
    //object type select
    [System.Serializable]
     public enum ItemType
    {
        Cube
    }

     //select the color of the cube
     public enum CubeColor
     {
         None,
         Red,
         Green,
         Yellow,
         Blue
     }

     private SphereCollider coll;

     public ItemType type;

     public CubeColor cubeColor;

     private bool hold = false; //cube is in hold state or not
     private bool playerNear = false;

     private float cubeValueY = 15.66925f; //y position value for platform

     public bool Hold
     {
         get => hold;
         set => hold = value;
     }

     public bool PlayerNear
     {
         get => playerNear;
         set => playerNear = value;
     }

     // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear && !hold)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HoldState();
            }  
        }
        
        if (hold)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                float x = PlayerController.instance.gameObject.transform.position.x;
                float z = PlayerController.instance.gameObject.transform.position.z;
                gameObject.transform.position = new Vector3(x, cubeValueY, z + 1); //player puts back the object
                
                gameObject.transform.parent = null;
                
                hold = false;
                playerNear = true;
                coll.enabled = true;
                
                PlayerController.instance.PutObject();
            }
        }
    }
    
    
   private void OnTriggerEnter(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           CanvasController.instance.InteractableText(true);
           playerNear = true;
       }
   }
   
   private void OnTriggerExit(Collider other)
   {
       if (other.CompareTag("Player"))
       {
           CanvasController.instance.InteractableText(false);
           playerNear = false;
       }
   }
   
   public void HoldState()
   {
       PlayerController.instance.HoldObject(gameObject);
       coll.enabled = false;
       CanvasController.instance.InteractableText(false);
       StartCoroutine(HoldDelay());
   }

   //if player press e, object gets to the players hands and goes back so there is delay for not making work E button twice while holding
   IEnumerator HoldDelay()
   {
       yield return new WaitForSeconds(0.1f);
       hold = true;
   }

}
