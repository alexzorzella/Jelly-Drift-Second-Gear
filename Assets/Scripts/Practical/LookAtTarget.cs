using UnityEngine;

public class LookAtTarget : MonoBehaviour {
    public Transform target;
    readonly float targetFov = 15f;
    Camera cam;

    void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * 10f);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, Time.deltaTime * 5.5f);
    }
}