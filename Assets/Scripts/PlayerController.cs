using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public bool isTeam1 = true;
    private NavMeshAgent Agent;

    private bool touchedBall = false;

    // Start is called before the first frame update
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if colliding with ball
        if(other.CompareTag("Ball"))
        {
            Debug.Log("Player touched Ball.");
            SetTouchedBall(true);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        Agent.destination = destination;
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
}
