using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementCheck : MonoBehaviour
{
    //Checks all the holders if they are filled correctly
    //Checks the box color sequence if it is correct
    
    public static PlacementCheck instance;
    
    [SerializeField] private List<HolderController> holders;

    private List<string> colorOrder = new List<string> ();

    [SerializeField] private MovePanel door;
    private int counter = 0;

    private bool done = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
      colorOrder = new List<string> ();
    
      //correct color order sequence
      //from left to right
        colorOrder.Add("Blue");
        colorOrder.Add("Yellow");
        colorOrder.Add("Green");
        colorOrder.Add("Red");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == 4) //checks if all the holders are filled
        {
            if (ColorCheck() && !done)
            {
                //Debug.Log("TRUE COLORS!!!");
                door.ActivateObject();
                done = true;

            }
        }
    }

    //sequence check
    bool ColorCheck()
    {
        for (int i = 0; i < holders.Count; i++)
        {
            if (colorOrder[i] != holders[i].boxColor)
            {
                return false;
            }
        }

        return true;
    }

    //holder filled counter 
    public void UpdateCounter(int c)
    {
        counter += c;
        Debug.Log("Counter is: " + counter);
    }


}
