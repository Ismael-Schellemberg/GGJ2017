using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] Collider2D colider;
    [SerializeField] Animator anim;

    void Start() {
        anim.speed = Random.Range(0.8f, 1.2f);
    }

    public void Hit() {
        colider.enabled = false;
        anim.SetTrigger("hit");
    }
}
