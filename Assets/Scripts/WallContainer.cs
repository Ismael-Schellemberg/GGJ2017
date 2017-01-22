using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContainer : MonoBehaviour {

    private Wall[] walls;
	private Coin[] coins;
    public float width;
    public int wallId;
	public float maxX {
		get { 
			return transform.position.x + width / 2f;
		}
	}

    public void Init() {
        float leftEdge = 0f;
		float rightEdge = 0f;

		walls = gameObject.GetComponentsInChildren<Wall> ();
		coins = gameObject.GetComponentsInChildren<Coin> ();

        for (int i = 0; i < walls.Length; i++) {
			if (i == 0 || leftEdge > walls[i].minX)
				leftEdge = walls[i].minX;
			if (i == 0 || rightEdge < walls[i].maxX)
				rightEdge = walls[i].maxX;
        }
		width = rightEdge - leftEdge;
    }

	public void reset() {
		foreach (Coin coin in coins) {
			coin.reset ();
		}
	}
}
