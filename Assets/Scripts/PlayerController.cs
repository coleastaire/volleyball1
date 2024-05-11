using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class PlayerController : MonoBehaviour
{
    public enum TouchType
    {
        pass,
        set,
        attack
    }

    public Transform IdlePosition;

    public BallPathManager ballPathMgr;

    public bool isTeam1 = true;
    private NavMeshAgent Agent;

    private bool touchedBall = false;
    private bool activeReceiver = false;

    public Animator anim;

    private TouchType currentTouchType;

    public GameObject PlayerPivot;
    public float AttackDistanceToJump = 0.25f;
    public float attackJumpHeight = 1f; // Height of the jump
    public float attackJumpDuration = 1f; // Duration of the jump
    private bool isAttackJumping = false; // Flag to track if the character is currently jumping
    private float startY; // Initial y-position of the character
    private float jumpStartTime; // Time when the jump started
    public AnimationCurve AttackJumpCurve;

    public Transform[] WPObjects = new Transform[3];

    private Vector3[] Waypoints = new Vector3[3];

    // Start is called before the first frame update
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();

        //anim = GetComponent<Animator>();
        //if (anim == null)
        //Debug.Log("Player Animator not found.");

        UpdateWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeReceiver)
        {
            if (DistanceTo(Agent.destination) <= 2.0f)
            {
                //if there in time, play one animation
                switch (currentTouchType)
                {
                    case TouchType.pass:
                        if (DistanceTo(Agent.destination) <= 2.0f)
                        {
                            anim.Play("Bump");
                        }
                        break;

                    case TouchType.set:
                        if (DistanceTo(Agent.destination) <= 2.0f)
                        {
                            anim.Play("HandSet");
                        }
                        break;

                    case TouchType.attack:
                        if (DistanceTo(Agent.destination) <= AttackDistanceToJump)
                        {
                            anim.Play("Attack");
                            StartJump();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        //if not there in time, play another

        //update jump
        // If the character is currently jumping
        if (isAttackJumping)
        {
            // Calculate the elapsed time since the jump started
            float elapsedTime = Time.time - jumpStartTime;

            // If the jump is still in progress
            if (elapsedTime < attackJumpDuration)
            {
                // Calculate the jump progress (0 to 1)
                float t = Mathf.Clamp01(elapsedTime / attackJumpDuration);

                // Evaluate the jump curve at the current progress
                float curveValue = AttackJumpCurve.Evaluate(t);

                // Calculate the new y-position using the evaluated curve value
                float newY = startY + attackJumpHeight * curveValue;

                // Update the character's position with the new y-position
                Vector3 newPosition = transform.position;
                newPosition.y = newY;
                PlayerPivot.transform.position = newPosition;
            }
            else
            {
                // Jump completed, reset jump state
                isAttackJumping = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void SetReceiveDestination(Vector3 destination)
    {
        Agent.destination = destination;
        activeReceiver = true;
    }

    public float DistanceTo(Vector3 target)
    {
        return Vector3.Distance(target, gameObject.transform.position);
    }

    public void SetTouchedBall(bool touched)
    {
        touchedBall = touched;
    }

    public bool GetTouchedBall()
    {
        return touchedBall;
    }

    public void SetActiveTouch(bool a) { activeReceiver = a; }
    public bool GetActiveTouch() { return activeReceiver; }

    public void ResetPosition()
    {
        Agent.destination = IdlePosition.position;
        activeReceiver = false;
    }

    public void SetTouchType(int typeIndex)
    {
        currentTouchType = (TouchType)typeIndex;
    }

    public void OnBallCollision()
    {
        //set inactive touch
        SetTouchedBall(true);
        anim.SetTrigger("Finish");
    }
    public void UpdateWaypoints()
    {
        for(int i = 0; i < 3; i++)
        {
            Waypoints[i] = WPObjects[i].position;
        }
    }

    public void StartJump()
    {
        // Only start the jump if the character is not already jumping
        if (!isAttackJumping)
        {
            // Set jump state to true and record jump start time
            isAttackJumping = true;
            jumpStartTime = Time.time;
        }
    }
}
