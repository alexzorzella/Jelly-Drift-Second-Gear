using UnityEngine;

public class AnimateUi : MonoBehaviour {
    public float rotSpeed;
    public float rotStrength;
    public float rotSmooth;
    public float scaleSpeed;
    public float scaleStrength;
    public float scaleSmooth;

    Vector3 defaultScale;
    Vector3 desiredScale;
    float rot;
    float rotVel;
    Vector3 scaleVel;

    void Awake() {
        defaultScale = transform.localScale;
        desiredScale = defaultScale;
    }

    void Update() {
        var d = 1f + (Mathf.PingPong(Time.time * scaleSpeed, scaleStrength) - scaleStrength / 2f);
        var target = Mathf.PingPong(Time.time * rotSpeed, rotStrength) - rotStrength / 2f;
        desiredScale = defaultScale * d;
        transform.localScale = Vector3.SmoothDamp(transform.localScale, desiredScale, ref scaleVel, scaleSmooth);
        rot = Mathf.SmoothDamp(rot, target, ref rotVel, rotSmooth);
        transform.localRotation = Quaternion.Euler(0f, 0f, rot);
    }
}