using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public Transform ServeTransform;
    public float HitHeight = 5.0f;
    public GameObject Ball;
    public BallPathManager BallPath;

    public List<GameObject> Players = new List<GameObject>();

    public bool isTeam1Ball = true;
    public int CurrentTouches = 0;

    public Vector3 CurrentBallDestination = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ServeBall()
    {
        //set new waypoints, starting from serve position
        BallPath.SetWBObjectStart(ServeTransform.position);
        BallPath.SetRandomDestinationWPObject(isTeam1Ball);
        BallPath.SetWPObjectMidPoint(HitHeight);

        BallPath.UpdateWaypoints();

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    public void BumpBall()
    {
        CountHit();

        //set new waypoints
        BallPath.SetWBObjectStart(Ball.transform.position);
        BallPath.SetRandomDestinationWPObject(isTeam1Ball);
        BallPath.SetWPObjectMidPoint(HitHeight);

        BallPath.UpdateWaypoints();

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    private void CountHit()
    {
        //ball starts on team 1
        //every 2 hits, switch teams

        CurrentTouches++;

        int isFirstTouch = (CurrentTouches % 2);

        if(isFirstTouch == 1)
        {
            //dont change team
        } else
        {
            isTeam1Ball = !isTeam1Ball;
        }
    }

    public void StartGame()
    {
        ServeBall();
    }

    public void ResetGame()
    {
        isTeam1Ball = true;
        CurrentTouches = 0;
    }

}
