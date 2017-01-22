using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    float amplitude = 2f;
    float periodDuration;
    float periodTimer = 0;

    Vector2 position = new Vector2(-3f, 0f);
    public Player player;

    void Start() {
        transform.position = position;
        periodDuration = amplitude * 2f;
    }

    void Update() {
        periodTimer += Time.deltaTime;

        float frequencyMultiplier = Player.TWO_PI / periodDuration;
        float periodTime = periodTimer * frequencyMultiplier;

        float periodSin = Mathf.Sin(periodTime);
        float periodCos = Mathf.Cos(periodTime);

        position.y = periodSin * amplitude;
        position.x += player.xSpeed * 0.8f * Time.deltaTime;

        transform.position = position;
    }
}
