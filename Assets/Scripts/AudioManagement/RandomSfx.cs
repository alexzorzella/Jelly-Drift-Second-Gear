using UnityEngine;

public class RandomSfx : MonoBehaviour {
    public AudioClip[] sounds;

    [Range(0f, 2f)] public float maxPitch = 0.8f;

    [Range(0f, 2f)] public float minPitch = 1.2f;

    public bool playOnAwake = true;

    AudioSource s;

    void Awake() {
        s = GetComponent<AudioSource>();
        if (playOnAwake) {
            Randomize();
        }
    }

    public void Randomize() {
        s.clip = sounds[Random.Range(0, sounds.Length - 1)];
        s.pitch = Random.Range(minPitch, maxPitch);
        s.Play();
    }
}