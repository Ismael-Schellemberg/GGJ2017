using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera camera;
    float xSpeed;
    float ySpeed;

    public float maxXSpeed;
    public float maxYSpeed;

    [SerializeField] TrailRenderer trailRend;
    [SerializeField] Color fixedColor;
    [SerializeField] Color freeColor;
    int movingSign;
    bool isMovingFree;
    float fixedDeltaX;
    [SerializeField] float accelSpeed;

    bool isBreaking;

    float breakCooldown;

    Vector2 movement = Vector2.zero;
    Vector2 cameraMovement = Vector2.zero;

    void Start() {
        movingSign = 1;
        isMovingFree = false;
        isBreaking = false;
        fixedDeltaX = 0.1f;
        UpdateTrailColor();
        xSpeed = maxXSpeed;
    }

    void Update() {
        if(isMovingFree) {
            if(xSpeed < maxXSpeed) {
                xSpeed += accelSpeed * Time.deltaTime;
            }
        } else {
            if(isBreaking) {
                xSpeed -= accelSpeed * Time.deltaTime;
                if(xSpeed <= 0.01f) {
                    isBreaking = false;
                    movingSign = -movingSign;
                }
            } else if(xSpeed < maxXSpeed) {
                xSpeed += accelSpeed * Time.deltaTime;
            }

            if((transform.position.x < -fixedDeltaX && movingSign < 0) ||
               (transform.position.x > fixedDeltaX && movingSign > 0)) {

                isBreaking = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            if(isMovingFree) {
                fixedDeltaX = Mathf.Abs(transform.position.x);
                if((transform.position.x < 0 && movingSign < 0) ||
                   (transform.position.x > 0 && movingSign > 0)) {

                    isBreaking = true;
                }
            }
            isMovingFree = !isMovingFree;

            UpdateTrailColor();
        }

        ySpeed = Mathf.Clamp(maxYSpeed * (1 - (Mathf.Abs(transform.position.x) / 4f)), 0.1f, maxYSpeed);

        float deltaX = movingSign * xSpeed * Time.deltaTime;
        float deltaY = ySpeed * Time.deltaTime;
        movement.x = deltaX;
        movement.y = deltaY;
        cameraMovement.y = deltaY;

        transform.Translate(movement);
        camera.transform.Translate(cameraMovement);
    }

    void UpdateTrailColor() {
        if(isMovingFree)
            trailRend.materials[0].SetColor("_TintColor", freeColor);
        else
            trailRend.materials[0].SetColor("_TintColor", fixedColor);
    }
}
