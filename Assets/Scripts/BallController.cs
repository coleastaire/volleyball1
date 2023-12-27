using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public Vector3 ArenaHalfDimensions = new Vector3(15,0,15);

    public Vector3 CurrentDestination = new Vector3();
    public Vector3 CurrentPathMidpoint = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GenerateRandomBallDestination(bool team1Ball)
    {
        //generate position
        float x = Random.Range(0.0f, ArenaHalfDimensions.x);
        if(team1Ball) { x *= -1; }
        float z = Random.Range(-(ArenaHalfDimensions.z), ArenaHalfDimensions.z);
        Vector3 newPos = new Vector3(x, 0.0f, z);

        CurrentDestination = newPos;

        Debug.Log("Ball Destination: " + newPos);
        return newPos;
    }

    public Vector3 GenerateBallPathMidPoint()
    {
        //get current and destination, then flatten them to y = 0
        Vector3 a = gameObject.transform.position;
        a.y = 0.0f;

        Vector3 c = CurrentDestination;
        c.y = 0.0f;

        //find mp
        Vector3 b = (a + c) / 2.0f;

        Debug.Log("Ball Midpoint: " + b);
        return b;
    }

    public void SetBallWaypoints(Vector3 destination, Vector3 midpoint)
    {
        
    }
}
