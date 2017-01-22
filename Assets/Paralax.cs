using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour {

    [SerializeField] Player player;
    [SerializeField] float maxX;
    [SerializeField] float factor;
    [SerializeField] Vector3 position;

    void Update() {
        position.x -= player.xSpeed * factor * Time.deltaTime;
        if(position.x < maxX)
            position.x = position.x - maxX;
        transform.localPosition = position;
    }
}
