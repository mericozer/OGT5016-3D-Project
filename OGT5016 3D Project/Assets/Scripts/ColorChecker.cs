using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChecker : MonoBehaviour
{
    public enum BallColor
    {
        None,
        Red,
        Green,
        Yellow,
        Blue
    }

    public BallColor currentColor;

    private List<int> selectedColors = new List<int>();

    private int counter;
    
    [SerializeField] private Material redMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Material yellowMat;
    [SerializeField] private Material blueMat;
    
    // Start is called before the first frame update
    void Start()
    {
        AdjustColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter >= 4)
        {
            //TO DO
            //OPEN DOOR
            Debug.Log("DOOR OPENED");
        }
    }

    void AdjustColor()
    {
        int color = Random.Range(1, 5);

        while (selectedColors.Contains(color))
        {
            color = Random.Range(1, 5);
        }
       
        selectedColors.Add(color);
        switch (color)
        {
            case 1:
                gameObject.GetComponent<MeshRenderer>().material = redMat;
                currentColor = BallColor.Red;
                break;
            case 2:
                gameObject.GetComponent<MeshRenderer>().material = greenMat;
                currentColor = BallColor.Green;
                break;
            case 3:
                gameObject.GetComponent<MeshRenderer>().material = yellowMat;
                currentColor = BallColor.Yellow;
                break;
            case 4:
                gameObject.GetComponent<MeshRenderer>().material = blueMat;
                currentColor = BallColor.Blue;
                break;

        }

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
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
