using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChecker : MonoBehaviour
{
    //Level 3 pipe color checker
    
    //colors kept as enum 
    public enum BallColor
    {
        None,
        Red,
        Green,
        Yellow,
        Blue
    }

    
    public BallColor currentColor; //holds current color of pipe

    private List<int> selectedColors = new List<int>(); //keeps the track of colors which is already selected for the pipe

    
    private int counter; //keeps the count of the balls in the pipe

    //materials for pipe color
    private Material currentMaterial;
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material blueMat;

    [SerializeField] private MovePanel door;

    private float lerp;
    private float colorChangeDuration = 0.3f;

    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        AdjustColor();
    }

    // Update is called once per frame
    void Update()
    {
        //color change slowly
        /*lerp = (Time.time - startTime) * colorChangeDuration;
        gameObject.GetComponent<MeshRenderer>().material.Lerp(currentMaterial, blueMat, lerp);
        if (lerp >= 1)
        {
            Debug.Log("birbirbirbir");
        }*/
        
    }

    void AdjustColor()
    {
        if (counter < 4) //if four balls are in level completed
        {
            int color = Random.Range(1, 5); //takes a value for 

            while (selectedColors.Contains(color))
            {
                color = Random.Range(1, 5);
            }
       
            //lerp = Mathf.PingPong(Time.time, colorChangeDuration) / colorChangeDuration;
            selectedColors.Add(color);
            switch (color)
            {
                case 1:
                    gameObject.GetComponent<MeshRenderer>().material = redMat;
                    //gameObject.GetComponent<MeshRenderer>().material.Lerp(currentMaterial, redMat, lerp);
                    currentMaterial = redMat;
                    currentColor = BallColor.Red;
                    break;
                case 2:
                    gameObject.GetComponent<MeshRenderer>().material = greenMat;
                    //gameObject.GetComponent<MeshRenderer>().material.Lerp(currentMaterial, greenMat, lerp);
                    currentMaterial = greenMat;
                    currentColor = BallColor.Green;
                    break;
                case 3:
                    gameObject.GetComponent<MeshRenderer>().material = yellowMat;
                    //gameObject.GetComponent<MeshRenderer>().material.Lerp(currentMaterial, yellowMat, lerp);
                    currentMaterial = yellowMat;
                    currentColor = BallColor.Yellow;
                    break;
                case 4:
                    gameObject.GetComponent<MeshRenderer>().material = blueMat;
                    //gameObject.GetComponent<MeshRenderer>().material.Lerp(currentMaterial, blueMat, lerp);
                    currentMaterial = blueMat;
                    currentColor = BallColor.Blue;
                    break;

            }

        }
        else
        {
            door.ActivateObject();
        }
       
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) //checks if ball is in or not
        {
            if (other.gameObject.GetComponent<ShootingBall>().color.ToString().Equals(currentColor.ToString()))
            {
                counter++;
                AdjustColor();
                Destroy(other.gameObject, 0.5f);
            }
            else
            {
                CanvasController.instance.FailLoseState();
            }
        }
    }
}
