using UnityEngine;

public class RandomBGColor : MonoBehaviour {
    readonly Color[] colors = {
        new(1f, 0.65f, 0.4f),
        new(1f, 0.4f, 0.41f),
        new(1f, 0.4f, 0.66f),
        new(0.95f, 0.48f, 1f),
        new(0.45f, 0.45f, 1f),
        new(0.316f, 0.7123f, 1f),
        new(0.35f, 1f, 0.48f)
    };

    Camera camera;

    void Awake() {
        camera = GetComponentInChildren<Camera>();
    }

    void OnEnable() {
        RandomColor();
    }

    void RandomColor() {
        var backgroundColor = colors[Random.Range(0, colors.Length)];
        camera.backgroundColor = backgroundColor;
    }
}