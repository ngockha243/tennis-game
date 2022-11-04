using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private float speed;    // speed of bot
    private GameObject ball;    // ball
    public Vector3 nextPos; // next position of ball in next 1 second
    public Vector3 dir;     // direction of hitting the ball
    public string difficulty; // difficulty
    public Animator anim;
    void Start()
    {
        // get animation
        anim = GetComponent<Animator>();

        // get difficulty --> set speed
        difficulty = StateName.difficulty;
        if(difficulty == "easy")
        {
            speed = 1f;
        }
        else
        {
            speed = 5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            // move bot by next position of ball (next position of ball in next 1 second) by x Axis
            ball = GameObject.FindWithTag("ball");
            float timeUpdate = ball.GetComponent<BallController>().timeUpdate;
            nextPos = updatePositionOverTime(timeUpdate + 50 * Time.deltaTime);
            // move
            // set difficulty
            if(difficulty == "easy")
            {
                // Bot move x Axis in easy mode
                transform.position = Vector3.Lerp(transform.position, new Vector3(nextPos.x, transform.position.y, transform.position.z), Time.deltaTime * speed);  
                
                // if easy --> bot hit straight
                dir = new Vector3(0, 1f, -1f);
            }
            else
            {
                // Predict next position of ball in next 1 second to move bot in z axis (get next position from ball when ball hit ground)
                Vector3 nextPosHeight = ball.GetComponent<BallController>().nextPos;
                // bot move x, z Axis in hard mode
                transform.position = Vector3.Lerp(transform.position, new Vector3(nextPos.x, transform.position.y, nextPosHeight.z), Time.deltaTime * speed);  
                
                // if hard --> bot hit random in range player court
                Vector3 randomPos = new Vector3(Random.Range(-9f, 9f), 0f, -9);
                dir = (randomPos - transform.position).normalized;
                dir.y = 1f;
            }
        }
        catch{

        }
    }
    Vector3 updatePositionOverTime(float t)
    {
        // get ball property to predict next position of ball
        Vector3 startPos = ball.GetComponent<BallController>().startPos;
        Vector3 direction = ball.GetComponent<BallController>().direction;
        float force = ball.GetComponent<BallController>().force;

        return startPos + direction * force * t + 0.5f * Physics.gravity * t * t;
    }
}
