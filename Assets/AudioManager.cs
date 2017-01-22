using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;

    public AudioSource source;

    public AudioClip coin;
    public AudioClip spawn;
    public AudioClip dead;

    void Awake() {
        Instance = this;
    }

    public void PlayCoin() {
        source.volume = 1f;
        source.PlayOneShot(coin);
    }

    public void PlaySpawn() {
        source.volume = 0.8f;
        source.PlayOneShot(spawn);
    }

    public void PlayDead() {
        source.volume = 1f;
        source.PlayOneShot(dead);
    }
}
