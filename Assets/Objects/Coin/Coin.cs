using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] Collider2D colider;
    [SerializeField] Animator anim;

    Vector2 originPosition;

    public void Init() {
        originPosition = transform.localPosition;
        Debug.Log(originPosition);
        colider.enabled = true;
        anim.speed = Random.Range(0.8f, 1.2f);
    }

    public void Hit() {
        if(!colider.enabled)
            return;

        colider.enabled = false;
        anim.SetBool("hit", true);
    }

    public void reset() {
        colider.enabled = true;
        anim.SetBool("hit", false);
        transform.localPosition = originPosition;
    }

    void Update() {
        if(Player.isMagnetActivated) {
            float distance = Vector2.Distance(Player.Instance.transform.position, transform.position);

            if(distance < 3) {
                transform.position = 
                Vector2.Lerp(transform.position, 
                    Player.Instance.transform.position,
                    (1f / distance) * 5f * Time.deltaTime);
            }
        }
    }
}
