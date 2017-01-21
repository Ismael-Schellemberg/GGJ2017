﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public SpriteRenderer spriteRenderer;

    public float maxX {
        get { return spriteRenderer.sprite.bounds.max.x; }
    }

    public float minX {
        get { return spriteRenderer.sprite.bounds.min.x; }
    }
}
