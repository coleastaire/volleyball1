using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public bool isTeam1 = true;
    private NavMeshAgent Agent;

    // Start is called before the first frame update
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDestination(Vector3 destination)
    {
        Agent.destination = destination;
    }

    public float DistanceTo(Vector3 target)
    {
        return Vector3.Distance(target, gameObject.transform.position);
    }
}
