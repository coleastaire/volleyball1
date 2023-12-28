using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallPathManager : MonoBehaviour
{
    public PathType PathSystem = PathType.CatmullRom;

    public Transform[] WPObjects = new Transform[3];

    private Vector3[] Waypoints = new Vector3[3];

    // Start is called before the first frame update
    void Start()
    {
        UpdateWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateWaypoints()
    {
        for(int i = 0; i < 3; i++)
        {
            Waypoints[i] = WPObjects[i].position;
        }
    }

    public void RestartPlayPath()
    {
        gameObject.transform.DOKill();
        gameObject.transform.position = Waypoints[0];
        //gameObject.transform.DORestart();
        gameObject.transform.DOPath(Waypoints, 3, PathSystem);
    }

    
    public void SetWBObjectStart(Vector3 newPosition)
    {
        WPObjects[0].position = newPosition;
        Debug.Log("Ball Path Start: " + newPosition);
    }

    public void SetWPObjectMidPoint(float height)
    {
        //get current and destination, then flatten them to y = 0
        Vector3 a = WPObjects[0].position;
        a.y = 0.0f;

        Vector3 c = WPObjects[2].position;
        c.y = 0.0f;

        //find mp and set object
        Vector3 b = (a + c) / 2.0f;
        b.y = height;

        WPObjects[1].position = b;

        Debug.Log("Ball Midpoint: " + b);
    }

    public Vector3 SetRandomDestinationWPObject(bool team1Ball)
    {
        //generate position
        float x = Random.Range(0.0f, 15);
        if(team1Ball) { x *= -1; }
        float z = Random.Range(-(7.5f), 7.5f);
        Vector3 newPos = new Vector3(x, 0.0f, z);

        WPObjects[2].position = newPos;

        Debug.Log("Ball Destination: " + newPos);
        return newPos;
    }

    public Vector3 GetCurrentBallDestination()
    {
        return Waypoints[2];
    }
}
