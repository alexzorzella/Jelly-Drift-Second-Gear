using UnityEngine;

public class Ambience : MonoBehaviour {
    public string ambienceSoundName;
    
    void Start() {
        if (!string.IsNullOrWhiteSpace(ambienceSoundName)) {
            MultiAudioSource audioSource = MultiAudioSource.FromResource(gameObject, ambienceSoundName, loop: true);
            audioSource.Play();
        }
    }
}