using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContainer : MonoBehaviour {

    public Wall[] walls;
    public float width;
    public float maxX = 0;
    public int wallId;

    public void Init() {
        float minX = int.MaxValue;
        float maxX = 0;
        for (int i = 0; i < walls.Length; i++) {
            if (minX > walls[i].minX)
                minX = walls[i].minX;
            if (maxX < walls[i].maxX) {
                maxX = walls[i].maxX;
            }
        }
		width = maxX - minX;
    }

}
