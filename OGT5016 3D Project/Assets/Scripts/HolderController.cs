using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderController : MonoBehaviour
{
    public enum CubeColor
    {
        None,
        Red,
        Green,
        Yellow,
        Blue
    }

    public CubeColor rightColor;
    
    //Script for cube placement holders
    
    [SerializeField] private Transform holderPoint; //cubes stands in this point

    private BoxCollider coll; 

    private GameObject obj;
    
    private bool filled = false; //holder is filled or not
    private bool done = false; //filled action is done or not
    private bool playerNear = false; 

    public string boxColor; //important to know box color for puzzle
    
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
              HoldDropAction();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.HolderText(filled);
            playerNear = true;
        }
    }
    
   
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.instance.HolderText(true);
            playerNear = false;
        }
    }

    //it gets activated with a player and a box
    //takes the box form players hand and puts it to the hold point
    
    private void HoldDropAction()
    {
        if (!filled && !done)
        {
            obj = PlayerController.instance.holdingObject; //takes object

            if (obj.GetComponent<InteractableItem>().cubeColor.Equals(rightColor))
            {
                obj.transform.position = holderPoint.position; //change the position of the object
            
                obj.transform.parent = gameObject.transform; //makes the box object a child of the holder
            
                obj.GetComponent<InteractableItem>().Hold = false; //checks if box is in hold state
            
                obj.GetComponent<InteractableItem>().PlayerNear = false; //checks if player is holding
            
                PlayerController.instance.holdingObject = null; //player doesnt have the box object as child
            
                CanvasController.instance.HolderText(true); 
            
                PlayerController.instance.PutObject(); //makes player put the object
            
                PlacementCheck.instance.UpdateCounter(1); //all holders connected to one controller and it looks if all the holders are filled or not
            
                boxColor = obj.GetComponent<InteractableItem>().cubeColor.ToString(); //takes box color
           
                done = true;
            
                StartCoroutine(Delay());
            }
            else
            {
                CanvasController.instance.FailLoseState();
            }
            
           
        }
        /*
        else if(filled)
        {
            //makes object players child again
            //holder is now empty
            
            filled = false;
            done = false;
            
            CanvasController.instance.HolderText(filled);
            obj.GetComponent<InteractableItem>().HoldState();
            PlayerController.instance.HoldObject(obj);
            PlacementCheck.instance.UpdateCounter(-1);
        }*/

    }

    //when object goes to the holder a time needs to finish all the actions
    IEnumerator Delay()
    {
        CanvasController.instance.HolderText(false);
        yield return new WaitForSeconds(1f);
        filled = true;
    }
}
