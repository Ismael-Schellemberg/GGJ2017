using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Camera camera;
	public float xSpeed;
	public float ySpeed;


    void Start() {
    }

    void Update() {
		float deltaX = xSpeed * Time.deltaTime;
		float deltaY = ySpeed * Time.deltaTime;
		Vector3 movement = new Vector3 (deltaX, deltaY, 0f);
		Vector3 cameraMovement = new Vector3 (0f, deltaY, 0f);



		transform.Translate (movement);
		camera.transform.Translate (cameraMovement);

		Debug.Log ("movement = " + movement + ", newPos = " + transform.position);

		if (xSpeed < 0 && transform.position.x < -2f)
			xSpeed = -xSpeed;
		else if (xSpeed > 0 && transform.position.x > 2f)
			xSpeed = -xSpeed;	
			
		if (Input.GetKeyDown(KeyCode.Space))
            xSpeed = -xSpeed;


		if (Input.GetKeyDown (KeyCode.UpArrow))
			ySpeed++;
		else if (Input.GetKeyDown (KeyCode.DownArrow))
			ySpeed--;
    }
}
