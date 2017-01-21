using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour {

    public float zJump;
    public float zMax;
    public float zSpeed;
    [SerializeField] Transform textures;

    Vector3 position;

    void Start() {
        position = transform.localPosition;
    }

    void Update() {
        position.z += zSpeed * Time.deltaTime;
        if(position.z > zMax) {
            position.z = position.z - zJump;
        }
        transform.localPosition = position;
    }
}
