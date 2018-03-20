using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySlowRB : MonoBehaviour {
    public GameObject spot;
    float length;
    float timer = 0;
    Vector3 pivot;
    float speed, angleRotated;
    bool isMoving;
    Rigidbody rbCube;
	// Use this for initialization
	void Start () {
        rbCube = GetComponent<Rigidbody>();
        isMoving = false;
        angleRotated = 0;
        speed = 1;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        //transform.Rotate(new Vector3(100,0,0), Vector3.forward, 1.0f);
        //transform.RotateAround(new Vector3(100, 0, 0), 1.0f);
        //transform.Rotate(new Vector3(100, 0, 0), 1.0f, new Space());
        //if (!isMoving)
        //    return;
        //if (angleRotated < 90) {
        //    float degree = speed;// *Time.deltaTime;
        //    //transform.RotateAround(pivot, Vector3.forward, degree);
        //    angleRotated += degree;
        //}
        //else {
        //    angleRotated = 0;
        //    pivot = transform.position + new Vector3(-length / 2f, -length / 2f, 0);
        //}
        //if(timer > 0.1f) {
        //    transform.position = snap(transform.position);
        //    transform.rotation = Quaternion.identity;
        //}
        if(timer > 3f)
        {
            startMoving();
            timer = 0;
        }

        // snap to grid after moving
        if (rbCube != null)
            if (rbCube.velocity.magnitude == 0)
            {
                Debug.Log("not moving");
                transform.position = snap(transform.position);
                transform.rotation = Quaternion.identity;
            }
    }

    Vector3 snap(Vector3 value)
    {
        Vector3 snappedValue;
        //snappedValue.x = (float)Math.Round(value.x, MidpointRounding.AwayFromZero) / 2;
        //snappedValue.y = (float)Math.Round(value.y, MidpointRounding.AwayFromZero) / 2;
        //snappedValue.z = (float)Math.Round(value.z, MidpointRounding.AwayFromZero) / 2;
        snappedValue.x = (float)Math.Round(value.x * 2) / 2;
        snappedValue.y = (float)Math.Round(value.y * 2) / 2;
        snappedValue.z = (float)Math.Round(value.z * 2) / 2;
        return snappedValue;
    }

    void startMoving() {
        //length = GameObject.Find("Cube").GetComponent<Renderer>().bounds.size.x;
        //length = 1;
        //pivot = transform.position + new Vector3(-length / 2f, -length / 2f, 0);
        //isMoving = true;
        length = (int)GetComponent<BoxCollider>().bounds.size.x;
        Debug.Log(length);
        pivot = transform.position + new Vector3(length / 2f, length / 2f, 0);
        Vector3 landPoint = transform.position + new Vector3(-length / 2f, length / 2f, 0);
        Instantiate(spot, landPoint, Quaternion.identity).transform.SetParent(transform);
        //rbCube.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
        rbCube.AddForceAtPosition(Vector3.left * 120, pivot, ForceMode.Force);
    }
}
