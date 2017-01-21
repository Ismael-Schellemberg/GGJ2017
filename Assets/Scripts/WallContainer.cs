using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContainer : MonoBehaviour {

    public Wall topWall;
    public Wall bottomWall;

    public int wallId;
	public float maxX;

	public float width {
		get { return topWall.width; }
    }

}
