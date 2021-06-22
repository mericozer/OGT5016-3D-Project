using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ShootProjection : MonoBehaviour
{
    //creates a line to show where to shoot
    //it only shows a projection line of a correct shooting power
    private LineRenderer lineRenderer;

    [SerializeField] private int pointCount = 50;

    [SerializeField] private float timeBetweenPoints = 0.1f;

    [SerializeField] private LayerMask CollidableLayers;

    [SerializeField] private Transform shotPoint;

    [SerializeField] private float shootPower = 20;

    private bool shooting = false;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            lineRenderer.positionCount = (int) pointCount; 
            List<Vector3> points = new List<Vector3>();
        
            //cannon shot point
            Vector3 startingPosition = shotPoint.position;
            //cannon power
            Vector3 startingVelocity = shotPoint.forward * shootPower;

            for (float t = 0; t < pointCount; t += timeBetweenPoints) //creates a line with fixed points
            {
                Vector3 newPoint = startingPosition + t * startingVelocity;
                newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t; //calculates the points position according to velocity
                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0) //points stop if a selected layer is touched by the line
                {
                    lineRenderer.positionCount = points.Count;
                    break;
                }
                //lineRenderer.positionCount = points.Count;
            }
        
            lineRenderer.SetPositions(points.ToArray());
        }
       
    }
    
    public void CleanLine() //makes point vector values zero to delete the line
    {
        shooting = false;
        lineRenderer.positionCount = 0;
    }

    public void ActivateLine()
    {
        shooting = true;
        lineRenderer.enabled = true;
    }
}
