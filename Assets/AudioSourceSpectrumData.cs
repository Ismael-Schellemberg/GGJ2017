using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceSpectrumData : MonoBehaviour {
    public CameraEffects effects;
    public float[] samples;
    public LineRenderer line;

    Vector2 tempVec = Vector2.zero;

    void Start() {  
        samples = new float[64];
        line.SetVertexCount(samples.Length);  
    }

    void Update() {
        AudioListener.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        for(int i = 1; i < samples.Length - 1; i++) {
            Debug.Log(samples[i]);
            tempVec.x = transform.position.x + i * 0.3f;
            tempVec.y = Mathf.Clamp(samples[i] * (50 + i * i), 0, 50);
            line.SetPosition(i, tempVec); 
        }
    }
}