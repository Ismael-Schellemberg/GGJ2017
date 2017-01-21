﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera camera;
    float ySpeed;
    float xSpeed;

    public float minXSpeed;
    public float maxXSpeed;
    public float maxYSpeed;

    [SerializeField] TrailRenderer trailRend;
    [SerializeField] Color fixedColor;
    [SerializeField] Color freeColor;
    int movingSign;
    public float amp;
    float lastY;
    [SerializeField] float accelSpeed;

    public float timeOffset;
    float breakCooldown;

    Vector3 movement = Vector2.zero;
	Vector3 cameraPosition = Vector2.zero;
	float cameraDeltaX;

    bool isMovingFree;
    bool isPressing;

    void Start() {
        isPressing = false;
        isMovingFree = false;
        movingSign = 1;
        amp = 0.1f;
		cameraPosition = Camera.main.transform.position;
		cameraDeltaX = Camera.main.transform.position.x - transform.position.x;
		Debug.Log ("" + cameraPosition);
        UpdateTrailColor();
        ySpeed = maxXSpeed;
    }

    void Update() {
        isPressing = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if (!isMovingFree && isPressing) {
            ySpeed = Mathf.Sign(lastY * transform.position.y) * transform.position.y;

            amp += Mathf.Sign(lastY - transform.position.y) * 1;
        }

        isMovingFree = isPressing;
        lastY = transform.position.y;
        if(isMovingFree) {
            amp += ySpeed * Time.deltaTime;
        }
        movement.y = Mathf.Sin(Time.time * (1f / amp)) * amp;
        UpdateTrailColor();

        xSpeed = Mathf.Clamp(maxYSpeed * (1 - (Mathf.Abs(transform.position.y) / 4f)), 0.1f, maxYSpeed);

        float deltaX = xSpeed * Time.deltaTime;
        movement.x += deltaX;
		transform.position = movement;


		cameraPosition.x = movement.x + cameraDeltaX;
		camera.transform.position = cameraPosition;
    }

    void UpdateTrailColor() {
        if(isMovingFree)
            trailRend.materials[0].SetColor("_TintColor", freeColor);
        else
            trailRend.materials[0].SetColor("_TintColor", fixedColor);
    }
}
