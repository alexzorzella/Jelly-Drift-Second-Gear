using UnityEngine;

public class LowPassFilterController : MonoBehaviour {
    float desiredFreq;

    AudioLowPassFilter lowpass;

    void Awake() {
        lowpass = GetComponent<AudioLowPassFilter>();
        // print("got lowpass: " + lowpass);
    }

    void Update() {
        if (Pause.Instance.paused) {
            desiredFreq = 200f;
        }
        else if (Time.timeScale < 1f) {
            desiredFreq = 500f;
        }
        else {
            desiredFreq = 22000f;
        }

        lowpass.cutoffFrequency = Mathf.Lerp(lowpass.cutoffFrequency, desiredFreq, Time.fixedDeltaTime * 4f);
    }
}