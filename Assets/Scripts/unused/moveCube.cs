using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCube : MonoBehaviour
{
    #region Variables
    private bool ismoving = false;
    private float startY;
    public float cubeSpeed;
    float cubeSize;
    #endregion

    // Use this for initialization
    void Start() {
        startY = transform.position.y;
        cubeSize = GetComponent<Renderer>().bounds.size.x;
		GameObject targetPoint = new GameObject("targetpoint");
		targetPoint.transform.SetParent(transform);

		// move the targetpoint to the center of the cube
		targetPoint.transform.position = transform.position;
		targetPoint.transform.rotation = Quaternion.identity;
	}

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown("up") && ismoving == false)
        {
            ismoving = true;
            transform.Find("targetpoint").Translate(0, -cubeSize / 2, cubeSize / 2);
            StartCoroutine(DoRoll(transform.Find("targetpoint").position, Vector3.right, 90.0f, cubeSpeed));
        }
        if (Input.GetKeyDown("down") && ismoving == false)
        {
            ismoving = true;
            transform.Find("targetpoint").Translate(0, -cubeSize / 2, -cubeSize / 2);
            StartCoroutine(DoRoll(transform.Find("targetpoint").position, -Vector3.right, 90.0f, cubeSpeed));
        }
        if (Input.GetKeyDown("left") && ismoving == false)
        {
            ismoving = true;
            transform.Find("targetpoint").Translate(-cubeSize / 2, -cubeSize / 2, 0);
            StartCoroutine(DoRoll(transform.Find("targetpoint").position, Vector3.forward, 90.0f, cubeSpeed));
        }
        if (Input.GetKeyDown("right") && ismoving == false)
        {
            ismoving = true;
            transform.Find("targetpoint").Translate(cubeSize / 2, -cubeSize / 2, 0);
            StartCoroutine(DoRoll(transform.Find("targetpoint").position, -Vector3.forward, 90.0f, cubeSpeed));
        }
    }

    IEnumerator DoRoll(Vector3 aPoint, Vector3 aAxis, float aAngle, float speed) {
        float aDuration = 1 / speed;
        float tSteps = Mathf.Ceil(aDuration * 100.0f);
        float tAngle = aAngle / tSteps;
        Vector3 pos; // declare variable to fix the y position

        // Rotate the cube by the point, axis and angle
        for (var i = 1; i <= tSteps; i++) {
            transform.RotateAround(aPoint, aAxis, tAngle);
            yield return new WaitForSeconds(0.01f);
        }
    

        // move the targetpoint to the center of the cube
        transform.Find("targetpoint").position = transform.position;
		transform.Find("targetpoint").rotation = Quaternion.identity;

		// Make sure the y position is correct
		pos = transform.position;
           pos.y = startY;
           transform.position = pos;
    
        // Make sure the angles are snaping to 90 degrees.    
           Vector3 vec = transform.eulerAngles;
           vec.x = Mathf.Round(vec.x / 90) * 90;
           vec.y = Mathf.Round(vec.y / 90) * 90;
           vec.z = Mathf.Round(vec.z / 90) * 90;
           transform.eulerAngles = vec;
    
        // The cube is stoped
           ismoving = false;    
        }
}
