using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    float amplitude = 2f;
    float periodDuration;
    float periodTimer = 0;

    Vector2 position;
    public Player player;

    bool moving;

    [SerializeField] Collider2D colider;
    [SerializeField] Animator anim;

    void Start() {
        moving = true;
        position = transform.localPosition;
        periodDuration = amplitude * 2f;

        colider.enabled = true;
        anim.speed = Random.Range(0.8f, 1.2f);
    }

    void Update() {
        if(!moving)
            return;

        periodTimer += Time.deltaTime;

        float frequencyMultiplier = Player.TWO_PI / periodDuration;
        float periodTime = periodTimer * frequencyMultiplier;

        float periodSin = Mathf.Sin(periodTime);

        position.y = periodSin * amplitude;
        position.x += player.xSpeed * 0.8f * Time.deltaTime;

        transform.localPosition = position;
    }

    public void Hit() {
        if(!moving)
            return;
        moving = false;
        colider.enabled = false;
        anim.SetTrigger("hit");
        OnHit();
    }

    protected virtual void OnHit() {
    }
}
