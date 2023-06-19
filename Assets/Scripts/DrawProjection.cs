using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawProjection : MonoBehaviour
{
    SlingController slingController;
    LineRenderer lineRenderer;
    [SerializeField] GameObject pointPref;

    // Number of points on the line
    public int numPoints = 50;

    // distance between those points on the line
    public float timeBetweenPoints = 0.1f;

    // The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    void Start()
    {
        slingController = GetComponent<SlingController>();
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        lineRenderer.positionCount = (int)numPoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = slingController.ShotPoint.position;
        Vector3 startingVelocity = slingController.ShotPoint.up * slingController.BlastPower + new Vector3(slingController.diffPosX, 0, 0);
        for (float t = 0; t < numPoints; t += timeBetweenPoints)
        {
            //Debug.Log(t);
            
            Vector3 newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y/2f * t * t;
            //GameObject createdPoint = Instantiate(pointPref, newPoint, slingController.ShotPoint.rotation);
            points.Add(newPoint);
            //Debug.Log(t + "  x = " + newPoint.x + " y = " + newPoint.y + " z = " + newPoint.z);

            if(Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
            
        }
        

        lineRenderer.SetPositions(points.ToArray());
    }
}