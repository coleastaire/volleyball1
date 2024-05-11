using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallPathManager : MonoBehaviour
{
    public PathType PathSystem = PathType.CatmullRom;
    public float BallFlightTime = 2.0f;
    public Ease EaseType = Ease.Linear;

    public bool UseCustomBallSpeedCurve = false;
    public AnimationCurve BallSpeedCurve;

    public Transform[] WPObjects = new Transform[3];

    private Vector3[] Waypoints = new Vector3[3];

    public Transform BallDestinationDecal;

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

        //set decal
        SetDestinationDecalPosition();
    }

    public void RestartPlayPath()
    {
        gameObject.transform.DOKill();
        gameObject.transform.position = Waypoints[0];
        //gameObject.transform.DORestart();
        if (UseCustomBallSpeedCurve)
            gameObject.transform.DOPath(Waypoints, BallFlightTime, PathSystem).SetEase(BallSpeedCurve);
        else
            gameObject.transform.DOPath(Waypoints, BallFlightTime, PathSystem).SetEase(EaseType);
    }

    
    public void SetWPObjectStart(Vector3 newPosition)
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
        float x = Random.Range(0.0f, 8f);
        if(team1Ball) { x *= -1; }
        float z = Random.Range(-(8f), 8f);
        Vector3 newPos = new Vector3(x, 0.0f, z);

        WPObjects[2].position = newPos;

        Debug.Log("Ball Destination: " + newPos);
        return newPos;
    }

    public Vector3 SetDestinationWPObject (bool team1Ball, Vector3 pos)
    {
        //generate position
        float x = pos.x; 
        if(team1Ball) { x *= -1; }
        float z = pos.z;
        Vector3 newPos = new Vector3(x, 0.0f, z);

        WPObjects[2].position = newPos;

        Debug.Log("Ball Destination: " + newPos);
        return newPos;
    }

    public void SetBallPathRandomDestination(bool team1Ball, Vector3 startPos, float height, float duration)
    {
        SetWPObjectStart(startPos);
        SetRandomDestinationWPObject(team1Ball);
        SetWPObjectMidPoint(height);
        BallFlightTime = duration;
    }

    public void SetBallPath(bool team1Ball, Vector3 startPos, Vector3 endPos, float height, float duration)
    {
        SetWPObjectStart(startPos);
        SetDestinationWPObject(team1Ball, endPos);
        SetWPObjectMidPoint(height);
        BallFlightTime = duration;
    }
    public void SetBallPathRandomDestination(bool team1Ball, Vector3 startPos, float height)
    {
        SetWPObjectStart(startPos);
        SetRandomDestinationWPObject(team1Ball);
        SetWPObjectMidPoint(height);
    }

    public void SetBallPath(bool team1Ball, Vector3 startPos, Vector3 endPos, float height)
    {
        SetWPObjectStart(startPos);
        SetDestinationWPObject(team1Ball, endPos);
        SetWPObjectMidPoint(height);
    }

    public Vector3 GetCurrentBallDestination()
    {
        return Waypoints[2];
    }

    public void SetDestinationDecalPosition()
    {
        BallDestinationDecal.position = GetCurrentBallDestination();
    }
}
