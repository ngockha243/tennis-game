using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool isBallActive = true;
    public GameObject ball;
    public Transform spawnPos;
    void Start()
    {
        Instantiate(ball, spawnPos.position, Quaternion.identity);
    }
    void Update()
    {
        if(!isBallActive)
        {
            Instantiate(ball, spawnPos.position, Quaternion.identity);
            isBallActive = true;
        }
    }
}
