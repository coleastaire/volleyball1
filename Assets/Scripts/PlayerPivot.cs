using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPivot : MonoBehaviour
{
    public UnityEvent OnBallCollision;

    // Start is called before the first frame update
    void Start()
    {
        
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
            Debug.Log("Player Collided with Ball.");
            OnBallCollision.Invoke();
        }
    }
}
