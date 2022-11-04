using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;    // speed of player
    // draw direction
    public Vector3 direction;   // direction of hitting the ball
	public GameObject PointPrefab;
	private GameObject[] Points;
	public int numberOfPoints = 10;
    // animation
    public Animator anim;
    // Get mouse down and mouse up --> to detect direction of mouse swipe
    
    public Vector2 firstPressPos;      // first position (when mouse down)
    public Vector2 secondPressPos;     // second position (when mouse up)

    void Start()
    {
        // init draw direction
        Points = new GameObject[numberOfPoints];
		for(int i = 0; i < numberOfPoints; i++){
			Points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
		}
        // animation
        anim = GetComponent<Animator>();
        // init direction of hitting the ball -> straight
        direction = new Vector3(0, 1f, 1f);
    }
    void Update()
    {
        // Move player
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if((h != 0 || v != 0)){
            transform.Translate(new Vector3(h , 0, v) * speed * Time.deltaTime);
        }

        // draw direction of hitting the ball
        for (int i = 0; i < numberOfPoints; i++){
            Points[i].transform.position = PointPosition(i * 0.01f);
        }

        // get direction by swipe
        direction = Swipe();
    }

    
	Vector3 PointPosition(float t){
		return transform.position + direction * 18f * t + 0.5f * Physics.gravity * t * t;
	}

    // function get direction when swipe screen
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
        // Get direction of mouse swipe --> direction of ball Axis X, Z (direction of mouse Axis X, Y)
        Vector2 dir = (secondPressPos - firstPressPos).normalized;
        // if haven't swipe the mouse or swipe backward --> hit the ball straight
        if(firstPressPos == Vector2.zero || firstPressPos == secondPressPos || dir.y < 0)
        {
            return new Vector3(0, 1f, 1f);
        }
        // return direction of mouse swipe
        return new Vector3(dir.x, 1f, dir.y);
    }
}
