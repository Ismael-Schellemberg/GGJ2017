using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float xSpeed;

    Vector2 position;

    void Start() {
        position = Vector2.zero;
    }

    void Update() {
        position.x += xSpeed * Time.deltaTime;
        transform.position = position;

        if(Input.GetKeyDown(KeyCode.Space))
            xSpeed = -xSpeed;
    }
}
