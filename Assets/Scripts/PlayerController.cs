using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private Animator anim;

    private TouchType currentTouchType;

    // Start is called before the first frame update
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();

        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("Player Animator not found.");
    }

    // Update is called once per frame
    void Update()
    {
        if (activeReceiver)
        {
            //check distance from destination against time for ball to arrive
            if (DistanceTo(Agent.destination) <= 2.0f)
            {
                //if there in time, play one animation
                switch (currentTouchType)
                {
                    case TouchType.pass:
                        //anim.SetTrigger("StartBump");
                        anim.Play("Bump");
                        break;

                    case TouchType.set:
                        //anim.SetTrigger("StartSet");
                        anim.Play("HandSet");
                        break;

                    case TouchType.attack:
                        //anim.SetTrigger("StartBump");
                        anim.Play("Attack");
                        break;
                    default:
                        break;
                }
            }
        }
        //if not there in time, play another
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if colliding with ball
        if(other.CompareTag("Ball"))
        {
            Debug.Log("Player touched Ball.");
            SetTouchedBall(true);
            //set inactive touch

            anim.SetTrigger("Finish");
        }
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
}
