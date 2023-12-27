using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public BallController Ball;

    public List<GameObject> Players = new List<GameObject>();

    public bool isTeam1Ball = true;

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
        //generate a new destination

        //find the midpoint

        //set the new waypoints in the ball path

        //restart the ball animation
    }
}
