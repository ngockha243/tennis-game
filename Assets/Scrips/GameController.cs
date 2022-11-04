using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool isBallActive = true;
    public GameObject ball;
    public static Transform spawnPos;
    public static Transform spawnPosPlayer;
    public static Transform spawnPosBot;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI win;
    public float time;
    public static int scorePlayer = 0;
    public static int scoreBot = 0;
    public GameObject panel;
    public Button back;

    void Start()
    {
        // get spawn position of ball in player court and bot court
        spawnPosPlayer = GameObject.Find("SpawnPosPlayer").transform;
        spawnPosBot = GameObject.Find("SpawnPosBot").transform;
        
        // spawn in player court first
        spawnPos = spawnPosPlayer;
        Instantiate(ball, spawnPos.position, Quaternion.identity);

        // get difficulty
        difficultyText.text = StateName.difficulty;
        time = 90f;
        win.text = "";
        back.onClick.AddListener(delegate() {LoadMenu(); });
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
    void Update()
    {
        // check ball is active or not
        if(!isBallActive)
        {
            // delay and init ball
            StartCoroutine(delayForSpawn());
            isBallActive = true;
        }
        // check difficulty to return menu scene
        if(StateName.difficulty == "none")
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
        // score
        scoreText.text = "Player: " + scorePlayer + " - " + scoreBot + " : Bot";
        if(time > 0f)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 0;
            panel.SetActive(true);
            back.gameObject.SetActive(true);
            if(scoreBot < scorePlayer)
            {
                win.text = "You win!";
            }
            else if(scoreBot > scorePlayer){
                win.text = "You lose!";
            }
            else
            {
                win.text = "";
            }
            GameObject b = GameObject.FindWithTag("ball");
            Destroy(b);
        }
        timer.text = "Time: " + Mathf.Round(time);
    }

    IEnumerator delayForSpawn()
    {
        yield return new WaitForSeconds(1.5f);
        Instantiate(ball, spawnPos.position, Quaternion.identity);
    }
}
