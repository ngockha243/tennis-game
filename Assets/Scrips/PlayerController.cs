using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // Move player
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if((h != 0 || v != 0)){
            transform.Translate(new Vector3(h , 0, v) * speed * Time.deltaTime);
        }
    }
}
