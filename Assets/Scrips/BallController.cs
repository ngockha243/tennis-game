using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // player, bot
    private GameObject player;
    public GameObject bot;
    // bounce
    public float force = 18f;
    private float bounceForce = .8f;    // force loss when collide
    public Vector3 direction;   // direction of ball
    public Vector3 startPos;
    public float timeUpdate;
    private float radius;       // radius of ball
    // difficulty
    public string difficulty;   // difficulty
    private float speed;        // speed of ball

    // law tennis
    private bool turn = true;   // turn = true ---> player // turn = false ---> bot
    private int bounce = 1;     // only bouce 1 in tennis area
    // predict next position of ball when ball hit ground
    public Vector3 nextPos;
    void Start()
    {
        player = GameObject.Find("Player");
        bot = GameObject.FindWithTag("bot");
        startPos = transform.position;

        radius = gameObject.GetComponent<SphereCollider>().radius;
        // direction when initial (player side or bot side)
        if(GameController.spawnPos == GameController.spawnPosPlayer){
            turn = true;
            direction = new Vector3(0, 1f, 1f);
        }
        else{
            turn = false;
            direction = new Vector3(0, 1f, -1f);
        }

        // set difficulty
        difficulty = StateName.difficulty;
        if(difficulty == "easy")
        {
            speed = 1f;
        }
        else{
            speed = 1.5f;
        }

        // set next position in bot position
        nextPos = new Vector3(0, 1.5f, 18f);
    }

    void Update()
    {
        /*
        if player hit the ball --> add direction to the ball
        if ball hit wall and ground --> reflect direction of the ball
        if ball go out --> destroy
        */
        
        timeUpdate += Time.deltaTime;
        gameObject.transform.position = updatePositionOverTime(timeUpdate * speed);

        int numberOfFrameToPredict = 2;
        
        // Get direction before hit ball
        Vector3 dir = player.GetComponent<PlayerController>().direction;
        // PlayerController.direction = dir;

        // Use OverlapSphere to detect if the ball collide in the next frame
        Collider[] colliders = Physics.OverlapSphere(updatePositionOverTime(timeUpdate * speed + Time.deltaTime * numberOfFrameToPredict), radius, 1);
        if(colliders.Length != 0)
        {
            startPos = transform.position;
            timeUpdate = 0;

            // Player hit the ball
            if(colliders[0].transform.CompareTag("Player"))
            {
                // reset bounce to 1 and turn to true
                turn = true;
                bounce = 1;

                // set direction of ball by swipe screen
                direction = dir;

                // reset direction of mouse
                player.GetComponent<PlayerController>().firstPressPos = Vector2.zero;
                player.GetComponent<PlayerController>().secondPressPos = Vector2.zero;

                // Player animation: get ball direction to play forehand or backhand animation
                Vector3 ballDir = transform.position - player.transform.position;
                if(ballDir.x >= 0)
                {
                    player.GetComponent<PlayerController>().anim.Play("forehand");
                }
                else
                {
                    player.GetComponent<PlayerController>().anim.Play("backhand");
                }
                
            }
            // Bot hit the ball
            else if(colliders[0].transform.CompareTag("bot"))
            {
                // reset bounce to 1 and turn to false
                turn = false;
                bounce = 1;

                // get hit direction of bot
                direction = bot.GetComponent<BotController>().dir;

                // Bot animation: get ball direction to play forehand or backhand animation
                Vector3 ballDir = transform.position - bot.transform.position;
                if(ballDir.x >= 0)
                {
                    bot.GetComponent<BotController>().anim.Play("forehand");
                }
                else
                {
                    bot.GetComponent<BotController>().anim.Play("backhand");
                }
            }
            // The ball go out
            else if(colliders[0].transform.CompareTag("destroy"))
            {
                NewTurn();
            }
            // Ball hit wall, ground and net
            else{
                // create normal vector
                Vector3 normal = Vector3.zero;

                // Check ball hit
                // Ball hit wall (back and front)
                if(colliders[0].transform.CompareTag("wall"))
                {
                    // Get normal vector of front wall
                    normal = new Vector3(0, 0, -1);
                }
                // Ball hit wall (left and right)
                else if(colliders[0].transform.CompareTag("wallSide"))
                {
                    // Get normal vector of side wall
                    normal = new Vector3(1, 0, 0);
                }
                // Ball hit outside the tennis court
                else if(colliders[0].transform.CompareTag("ground"))
                {
                    // Get normal vector of ground
                    normal = new Vector3(0, 1, 0);
                    
                    // Check condition
                    if(bounce == 1){
                        if(turn)
                        {
                            AddScoreBot(1);
                            SetSpawnPositionBall(GameController.spawnPosBot);
                        }
                        else{
                            AddScorePlayer(1);
                            SetSpawnPositionBall(GameController.spawnPosPlayer);
                        }
                    }
                    else if (bounce == 0)
                    {
                        if(turn)
                        {
                            AddScorePlayer(1);
                            SetSpawnPositionBall(GameController.spawnPosPlayer);
                        }
                        else{
                            AddScoreBot(1);
                            SetSpawnPositionBall(GameController.spawnPosBot);
                        }
                    }
                    NewTurn();
                }
                // Ball hit player tennis court
                else if(colliders[0].transform.CompareTag("tennis-court-player"))
                {
                    
                    // Check condition
                    if(bounce == 1)
                    {
                        bounce --;
                        if (turn)
                        {
                            SetSpawnPositionBall(GameController.spawnPosBot);
                            NewTurn();
                        }
                        else{
                            turn = false;
                        }
                    }
                    else if(bounce == 0)
                    {
                        SetSpawnPositionBall(GameController.spawnPosBot);
                        NewTurn();
                        AddScoreBot(1);
                    }
                }
                // Ball hit bot tennis court
                else if(colliders[0].transform.CompareTag("tennis-court-bot"))
                {
                    // when ball hit ground --> predict position of ball in next 1 second to move Bot
                    nextPos = updatePositionOverTime(timeUpdate + Time.deltaTime * 50);

                    // Check condition
                    if(bounce == 1)
                    {
                        bounce --;
                        if (!turn)
                        {
                            SetSpawnPositionBall(GameController.spawnPosPlayer);
                            NewTurn();
                        }
                        else{
                            turn = true;
                        }
                    }
                    else if(bounce == 0)
                    {
                        SetSpawnPositionBall(GameController.spawnPosPlayer);
                        NewTurn();
                        AddScorePlayer(1);
                    }
                }
                // Ball hit net
                else if(colliders[0].transform.CompareTag("net"))
                {
                    // Check condition
                    if(turn)
                    {
                        SetSpawnPositionBall(GameController.spawnPosBot);
                        AddScoreBot(1);
                    }
                    else
                    {
                        SetSpawnPositionBall(GameController.spawnPosPlayer);
                        AddScorePlayer(1);
                    }
                    NewTurn();
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

    //function set spawn position of ball
    void SetSpawnPositionBall(Transform position)
    {
        GameController.spawnPos = position;
    }
    // function set create new ball and destroy old ball
    void NewTurn()
    {
        GameController.isBallActive = false;
        Destroy(gameObject);
    }
    // add score player
    void AddScorePlayer(int score)
    {
        GameController.scorePlayer += score;
    }
    // add score bot
    void AddScoreBot(int score)
    {
        GameController.scoreBot += score;
    }
}
