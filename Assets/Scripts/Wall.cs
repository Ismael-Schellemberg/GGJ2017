using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	public SpriteRenderer spriteRenderer;

	public float height {
		get { return spriteRenderer.sprite.bounds.max.y - spriteRenderer.sprite.bounds.min.y; }
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Player") {
			Debug.Log ("EXPLODE!!!");
		}
	}
}
