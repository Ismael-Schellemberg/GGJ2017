using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContainer : MonoBehaviour {

    public Wall leftWall;
    public Wall rightWall;

    public int wallId;
    public float minX;

    public float widht {
        get { return leftWall.width; }
    }

}
