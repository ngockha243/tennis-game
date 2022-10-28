using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private GameObject player;
    private GameObject target;
    public float force = 18f;
    private float bounceForce = .9f;
    private Vector3 direction;
    private Vector3 startPos;
    private float timeUpdate;
    private float radius;
    // Get mouse down and mouse up --> to detect direction of mouse swipe
    
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    void Start()
    {
        startPos = transform.position;
        player = GameObject.Find("Player");
        target = GameObject.Find("Target");
        radius = gameObject.GetComponent<SphereCollider>().radius;
        direction = (target.transform.position - player.transform.position)/(target.transform.position - player.transform.position).magnitude;
    }

    void Update()
    {
        /*
        if player hit the ball --> add direction to the ball
        if ball hit wall and ground --> reflect direction of the ball
        if ball go out --> destroy
        */

        timeUpdate += Time.deltaTime;
        gameObject.transform.position = updatePositionOverTime(timeUpdate);

        int numberOfFrameToPredict = 2;
        
        // Get direction before hit ball
        Vector3 dir = Swipe();

        // Use OverlapSphere to detect if the ball collide in the next frame
        Collider[] colliders = Physics.OverlapSphere(updatePositionOverTime(timeUpdate + Time.deltaTime * numberOfFrameToPredict), radius, 1);
        if(colliders.Length != 0)
        {
            startPos = transform.position;
            timeUpdate = 0;

            // Player hit the ball
            if(colliders[0].transform.CompareTag("Player"))
            {
                direction = dir;
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
                direction = Vector3.Reflect(direction, normal) * bounceForce;
            }
        }
    }

    // update position of the ball over time by Time.deltaTime
    Vector3 updatePositionOverTime(float t)
    {
        return startPos + direction * force * t + 0.5f * Physics.gravity * t * t;
    }

    
    public Vector3 Swipe()
    {
        // Get position mouse down and mouse up
        if(Input.GetMouseButtonDown(0))
        {
            firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        if(Input.GetMouseButtonUp(0))
        {
            secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
        // Get direction of ball axis Y
        Vector3 heigh = (target.transform.position - player.transform.position)/(target.transform.position - player.transform.position).magnitude;
        // Get direction of mouse swipe --> direction of ball Axis X, Z (direction of mouse Axis X, Y)
        Vector2 dir = (secondPressPos - firstPressPos)/(secondPressPos - firstPressPos).magnitude;
        // if haven't swipe the mouse --> return direction from player to target
        if(firstPressPos == Vector2.zero)
        {
            return (target.transform.position - player.transform.position)/(target.transform.position - player.transform.position).magnitude;
        }
        // return direction of mouse swipe
        return new Vector3(dir.x, heigh.y, dir.y);
    }
}
