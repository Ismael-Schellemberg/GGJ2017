using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] Collider2D colider;
    [SerializeField] Animator anim;

    void Start() {
        colider.enabled = true;
        anim.speed = Random.Range(0.8f, 1.2f);
    }

    public void Hit() {
        if(!colider.enabled)
            return;

        colider.enabled = false;
        anim.SetBool("hit", true);
    }
}
