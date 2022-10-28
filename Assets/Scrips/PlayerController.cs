using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public static Vector3 direction;

    // draw direction
    
	public GameObject PointPrefab;
	private GameObject[] Points;
	public int numberOfPoints = 10;

    void Start()
    {
        Points = new GameObject[numberOfPoints];
		for(int i = 0; i<numberOfPoints; i++){
			Points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
		}
    }
    void Update()
    {
        // Move player
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if((h != 0 || v != 0)){
            transform.Translate(new Vector3(h , 0, v) * speed * Time.deltaTime);
        }

        for (int i = 0; i < Points.Length; i++){
            Points[i].transform.position = PointPosition(i * 0.01f);
        }
    }

    
	Vector3 PointPosition(float t){
		return transform.position + direction * 18f * t + 0.5f * Physics.gravity * t * t;
	}
}
