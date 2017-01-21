using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public float width {
        get { return spriteRenderer.sprite.bounds.max.x - spriteRenderer.sprite.bounds.min.x; }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if(coll.gameObject.name == "Player") {
            Debug.Log("EXPLODE!!!");
        }
    }
}
