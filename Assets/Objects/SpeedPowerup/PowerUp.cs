using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    float amplitude = 2f;
    float periodDuration;
    float periodTimer = 0;

    Vector2 position = new Vector2(15f, 0f);
    public Player player;

    bool moving;

    [SerializeField] Collider2D colider;
    [SerializeField] Animator anim;

    void Start() {
        moving = true;
        transform.localPosition = position;
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
        float periodCos = Mathf.Cos(periodTime);

        position.y = periodSin * amplitude;
        position.x += player.xSpeed * 0.8f * Time.deltaTime;

        transform.localPosition = position;
    }

    public void Hit() {
        if(!colider.enabled)
            return;

        colider.enabled = false;
        anim.SetTrigger("hit");
    }

    protected void OnHit() {
    }
}
