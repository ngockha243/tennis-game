using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameObject player;
    private GameObject target;
    public float force = 18f;
    private float bounceForce = .8f;
    private Vector3 Direction;
    private Vector3 startPos;
    private float timeUpdate;
    private float radius;
    void Start()
    {
        startPos = transform.position;
        player = GameObject.Find("Player");
        target = GameObject.Find("Target");
        radius = gameObject.GetComponent<SphereCollider>().radius;
        Direction = (target.transform.position - player.transform.position)/(target.transform.position - player.transform.position).magnitude;
    }

    void Update()
    {
        /*
        if player hit the ball --> add direction to the ball
        if ball hit wall and ground --> reflect direction of the ball
        if ball go out --> destroy
        */

        timeUpdate += Time.deltaTime;
        transform.position = updatePositionOverTime(timeUpdate);

        int numberOfFrameToPredict = 2;

        // Use OverlapSphere to detect if the ball collide in the next frame
        Collider[] colliders = Physics.OverlapSphere(updatePositionOverTime(timeUpdate + Time.deltaTime * numberOfFrameToPredict), radius, 1);
        if(colliders.Length != 0)
        {
            startPos = transform.position;
            timeUpdate = 0;

            // Player hit the ball
            if(colliders[0].transform.CompareTag("Player"))
            {
                // add direction
                Direction = (target.transform.position - player.transform.position)/(target.transform.position - player.transform.position).magnitude;
            }
            // The ball go out
            else if(colliders[0].transform.CompareTag("destroy"))
            {
                Destroy(gameObject);
                GameController.isBallActive = false;
            }
            // Ball hit wall and ground
            else{
                Vector3 normal = Vector3.zero;
                // Check ball hit
                if(colliders[0].transform.CompareTag("wallFront"))
                {
                    // Get normal vector of front wall
                    normal = new Vector3(0, 0, -1);
                }
                else if(colliders[0].transform.CompareTag("wallSide"))
                {
                    // Get normal vector of side wall
                    normal = new Vector3(1, 0, 0);
                }
                else if(colliders[0].transform.CompareTag("ground"))
                {
                    // Get normal vector of ground
                    normal = new Vector3(0, 1, 0);
                }
                // Reflect vector of the ball
                Direction = Vector3.Reflect(Direction, normal) * bounceForce;
            }
        }
    }

    // update position of the ball over time by Time.deltaTime
    Vector3 updatePositionOverTime(float t)
    {
        return startPos + Direction * force * t + 0.5f * Physics.gravity * t * t;
    }
}
