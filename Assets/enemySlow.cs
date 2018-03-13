using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySlow : MonoBehaviour {
    float length;
    Vector3 pivot;
    float speed, angleRotated;
	// Use this for initialization
	void Start () {
        angleRotated = 0;
        speed = 10;
        length = GameObject.Find("Cube").GetComponent<Renderer>().bounds.size.x;
        pivot = transform.position + new Vector3(-0.5f, -0.5f, 0);
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(new Vector3(100,0,0), Vector3.forward, 1.0f);
        //transform.RotateAround(new Vector3(100, 0, 0), 1.0f);
        //transform.Rotate(new Vector3(100, 0, 0), 1.0f, new Space());
        if (angleRotated < 90) {
            float degree = speed;// *Time.deltaTime;
            transform.RotateAround(pivot, Vector3.forward, degree);
            angleRotated += degree;
        }
        else {
            angleRotated = 0;
            pivot = transform.position + new Vector3(-0.5f, -0.5f, 0);
        }
    }
}
