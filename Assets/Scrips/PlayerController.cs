using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float positionY = 1.5f;
    public float positionZ = -8f;
    void Start()
    {
        transform.position = new Vector3(0, positionY, positionZ);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        // Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // transform.position = new Vector3(mousePos.x * speed, positionY, mousePos.y * speed);
        if(h != 0 || v != 0){
            transform.Translate(new Vector3(h , 0, v) * speed * Time.deltaTime);
        }
    }
}
