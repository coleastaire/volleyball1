using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public enum PlayerRecentTouch
    {
        none,
        teammate1,
        teammate2
    }

    public Transform ServeTransform;
    public float HitHeight = 5.0f;
    public GameObject Ball;
    public BallPathManager BallPath;

    public GameObject Team1Ground;
    public GameObject Team2Ground;

    public List<PlayerController> PlayersTeam1 = new List<PlayerController>();
    public List<PlayerController> PlayersTeam2 = new List<PlayerController>();

    private PlayerRecentTouch Touch = PlayerRecentTouch.none;

    public bool isTeam1Ball = true;
    private bool isServe = true;
    public int CurrentTouches = 0;

    public Vector3 CurrentBallDestination = new Vector3();

    void Start()
    {
        Collider ballCollider = Ball.GetComponent<Collider>();
        if (ballCollider == null)
            Debug.Log("Ball Collider not found.");

        Collider team1GroundCollider = Team1Ground.GetComponent<Collider>();
        if (team1GroundCollider == null)
            Debug.Log("Team 1 Ground Collider not found.");

        Collider team2GroundCollider = Team2Ground.GetComponent<Collider>();
        if (team2GroundCollider == null)
            Debug.Log("Team 2 Ground Collider not found.");

        ResetGame();
    }

    void Update()
    {
        //when the ball collides with a player, step the game flow
        //if the ball collides with the ground, reset the game
        if(CheckPlayerBallCollision() == true)
        {
            GameStepEvent();
        }

        if(CheckFloorBallCollision() == true)
        {
            ResetGame();
        }
    }

    public void GameStepEvent()
    {
        if(isServe)
        {
            ServeBall();
        } else
        {
            CountHit();
            BumpBall();
        }

        SelectAndMoveActivePlayer();
    }

    public void SelectAndMoveActivePlayer()
    {
        float min_distance = 1000.0f; //arbitrary large length
        int closestPlayerIndex;
        Vector3 ballPos = BallPath.GetCurrentBallDestination();

        if(isTeam1Ball)
        {
            if(Touch == PlayerRecentTouch.none)
            {
                //no touch, check distance
                if(PlayersTeam1[0].DistanceTo(ballPos) >= PlayersTeam1[1].DistanceTo(ballPos))
                {
                    //p1 closest or equal, give to p1
                    PlayersTeam1[0].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate1;
                } else
                {
                    PlayersTeam1[1].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate2;
                }
            } else
            {
                //ball has been touched, doesnt matter. give to player who hasnt touched.
                if(Touch == PlayerRecentTouch.teammate1)
                {
                    //p1 touched, give to p2
                    PlayersTeam1[1].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate2;
                } else
                {
                    PlayersTeam1[0].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate1;
                }
            }
        } else
        {
            if(Touch == PlayerRecentTouch.none)
            {
                //no touch, check distance
                if(PlayersTeam2[0].DistanceTo(ballPos) >= PlayersTeam2[1].DistanceTo(ballPos))
                {
                    //p1 closest or equal, give to p1
                    PlayersTeam2[0].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate1;
                } else
                {
                    PlayersTeam2[1].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate2;
                }
            } else
            {
                //ball has been touched, doesnt matter. give to player who hasnt touched.
                if(Touch == PlayerRecentTouch.teammate1)
                {
                    //p1 touched, give to p2
                    PlayersTeam2[1].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate2;
                } else
                {
                    PlayersTeam2[0].SetDestination(ballPos);
                    Touch = PlayerRecentTouch.teammate1;
                }
            }
        }
    }

    public void ServeBall()
    {
        //set new waypoints, starting from serve position
        BallPath.SetWBObjectStart(ServeTransform.position);
        BallPath.SetRandomDestinationWPObject(isTeam1Ball);
        BallPath.SetWPObjectMidPoint(HitHeight);

        BallPath.UpdateWaypoints();

        isServe = false;

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    public void BumpBall()
    {
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

        int isFirstTouch = (CurrentTouches % 3);

        if(isFirstTouch == 1)
        {
            //dont change team
        } else
        {

        }

        if(CurrentTouches == 3)
        {
            isTeam1Ball = !isTeam1Ball;
            //reset touches
            CurrentTouches = 0;
            //reset hits
            Touch = PlayerRecentTouch.none;
        }
        else
        {
            //dont change teams
        }
    }

    // Function to check collisions between players and the ball
    private bool CheckPlayerBallCollision()
    {
        foreach (PlayerController player in PlayersTeam1)
        {
            if (player.GetTouchedBall() == true)
            {
                // Player successfully reached the ball
                Debug.Log("Player from Team 1 reached the ball!");
                player.SetTouchedBall(false);
                return true;
            }
        }

        foreach (PlayerController player in PlayersTeam2)
        {
            if (player.GetTouchedBall() == true)
            {
                // Player successfully reached the ball
                Debug.Log("Player from Team 2 reached the ball!");
                player.SetTouchedBall(false);
                return true;
            }
        }

        return false;
    }

    private bool CheckFloorBallCollision()
    {
        if(Ball.transform.position == BallPath.GetCurrentBallDestination())
        {
            return true;
        }

        return false;
    }

    public void StartGame()
    {
        GameStepEvent();
    }

    public void ResetGame()
    {
        isTeam1Ball = true;
        CurrentTouches = 0;
        isServe = true;
    }

}
