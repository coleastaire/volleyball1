using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public enum PlayerRecentTouch
    {
        none,
        teammate1,
        teammate2
    }

    public Transform ServeStartTransform;
    public Transform PassDestinationTransform;
    public Transform AttackDestinationTransform;

    public float PassHeight = 7.0f;
    public float PassDuration = 2.5f;
    public float SetHeight = 5.0f;
    public float SetDuration = 2.0f;
    public float AttackHeight = 2.0f;
    public float AttackDuration = 1.0f;

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
    private int touchType = 0;

    public Vector3 CurrentBallDestination = new Vector3();

    public UnityEvent OnBump;
    public UnityEvent OnSet;
    public UnityEvent OnAttack;

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
            switch (CurrentTouches % 3)
            {
                case 0:
                    BumpBall(); 
                    OnBump.Invoke();
                    break;
                case 1:
                    SetBall(); 
                    OnSet.Invoke();
                    break;
                case 2:
                    AttackBall(); 
                    OnAttack.Invoke();
                    break;
                default:
                    break;
            }
            CountHit();
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
                    PlayersTeam1[0].SetReceiveDestination(ballPos);
                    PlayersTeam1[0].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate1;

                    PlayersTeam1[1].ResetPosition();
                } else
                {
                    PlayersTeam1[1].SetReceiveDestination(ballPos);
                    PlayersTeam1[1].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate2;

                    PlayersTeam1[0].ResetPosition();
                }
            } else
            {
                //ball has been touched, doesnt matter. give to player who hasnt touched.
                if(Touch == PlayerRecentTouch.teammate1)
                {
                    //p1 touched, give to p2
                    PlayersTeam1[1].SetReceiveDestination(ballPos);
                    PlayersTeam1[1].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate2;

                    PlayersTeam1[0].ResetPosition();
                } else
                {
                    PlayersTeam1[0].SetReceiveDestination(ballPos);
                    PlayersTeam1[0].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate1;

                    PlayersTeam1[1].ResetPosition();
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
                    PlayersTeam2[0].SetReceiveDestination(ballPos);
                    PlayersTeam2[0].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate1;
                    
                    PlayersTeam2[1].ResetPosition();
                } else
                {
                    PlayersTeam2[1].SetReceiveDestination(ballPos);
                    PlayersTeam2[1].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate2;

                    PlayersTeam2[0].ResetPosition();
                }
            } else
            {
                //ball has been touched, doesnt matter. give to player who hasnt touched.
                if(Touch == PlayerRecentTouch.teammate1)
                {
                    //p1 touched, give to p2
                    PlayersTeam2[1].SetReceiveDestination(ballPos);
                    PlayersTeam2[1].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate2;
                    PlayersTeam2[0].ResetPosition();
                } else
                {
                    PlayersTeam2[0].SetReceiveDestination(ballPos);
                    PlayersTeam2[0].SetTouchType(CurrentTouches % 3);
                    Touch = PlayerRecentTouch.teammate1;
                    PlayersTeam2[1].ResetPosition();
                }
            }
        }
    }

    public void ServeBall()
    {
        //set new waypoints, starting from serve position
        BallPath.SetBallPathRandomDestination(isTeam1Ball, ServeStartTransform.position, PassHeight);

        BallPath.UpdateWaypoints();

        isServe = false;

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    public void BumpBall()
    {
        //set new waypoints
        BallPath.SetBallPath(isTeam1Ball, Ball.transform.position, PassDestinationTransform.position, PassHeight);

        BallPath.UpdateWaypoints();

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    public void SetBall()
    {
        //set new waypoints
        BallPath.SetBallPath(isTeam1Ball, Ball.transform.position, PassDestinationTransform.position, SetHeight);

        BallPath.UpdateWaypoints();

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    public void AttackBall()
    {
        //set new waypoints
        BallPath.SetBallPathRandomDestination(!isTeam1Ball, Ball.transform.position, AttackHeight);//DONT FORGET ABOUT THIS ! SYMBOL IN THE PARAMETERRRRR HACKYYYYYy:w

        BallPath.UpdateWaypoints();

        //restart the ball animation
        BallPath.RestartPlayPath();
    }

    private void CountHit()
    {
        //ball starts on team 1
        //every 2 hits, switch teams

        CurrentTouches++;

        touchType = (CurrentTouches % 3);

        if(touchType == 1)
        {
            //dont change team
        } else
        {

        }

        if((CurrentTouches % 3) == 0)
        {
            isTeam1Ball = !isTeam1Ball;
            //reset touches
            CurrentTouches = 0;
            //reset hits
            Touch = PlayerRecentTouch.none;

            //reset position of players without posession
            if (isTeam1Ball)
            {
                foreach (PlayerController n in PlayersTeam2)
                {
                    n.ResetPosition();
                }
            } else
            {
                foreach (PlayerController n in PlayersTeam1)
                {
                    n.ResetPosition();
                }
            }
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
