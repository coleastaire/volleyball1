using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public GameObject Ball;
    public BallPathManager BallPath;

    public List<GameObject> Players = new List<GameObject>();

    public bool isTeam1Ball = true;

    public Vector3 CurrentBallDestination = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BumpBall()
    {
        //set new waypoints
        BallPath.SetWBObjectStart(Ball.transform.position);
        BallPath.SetRandomDestinationWPObject(isTeam1Ball);
        BallPath.SetWPObjectMidPoint(5.0f);

        BallPath.UpdateWaypoints();

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

}
