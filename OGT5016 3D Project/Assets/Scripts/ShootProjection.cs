using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class ShootProjection : MonoBehaviour
{
    //cannon example
    private LineRenderer lineRenderer;

    [SerializeField] private int pointCount = 50;

    [SerializeField] private float timeBetweenPoints = 0.1f;

    [SerializeField] private LayerMask CollidableLayers;

    [SerializeField] private Transform shotPoint;

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
            Vector3 startingVelocity = shotPoint.forward * 60 * 0.5f;

            for (float t = 0; t < pointCount; t += timeBetweenPoints)
            {
                Vector3 newPoint = startingPosition + t * startingVelocity;
                newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
                {
                    lineRenderer.positionCount = points.Count;
                    break;
                }
                //lineRenderer.positionCount = points.Count;
            }
        
            lineRenderer.SetPositions(points.ToArray());
        }
       
    }
    
    public void CleanLine()
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
