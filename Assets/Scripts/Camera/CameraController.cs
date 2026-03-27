using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController Instance;
    public Transform target;
    public float moveSpeed;
    public float rotationSpeed;
    public float distFromTarget;
    public float camHeight;
    public float offsetSpeed = 1.5f;
    readonly float shakeThreshold = 16f;
    Quaternion desiredLook;
    Vector3 desiredPosition;
    float fov;
    Camera mainCam;
    Vector3 offset;
    bool readyToOffset = true;
    Car targetCar;
    Rigidbody targetRb;

    void Awake() {
        Instance = this;
        if (target != null) {
            AssignTarget(target);
        }

        mainCam = GetComponentInChildren<Camera>();
    }

    void Update() {
        if (!target) {
            return;
        }

        var normalized = new Vector3(target.forward.x, 0f, target.forward.z).normalized;
        var a = new Vector3(targetRb.linearVelocity.x, 0f, targetRb.linearVelocity.z).normalized;
        if ((targetCar.speed < 5f && targetCar.speed > -15f) || SaveState.i.cameraMode == 1) {
            a = Vector3.zero;
        }

        var a2 = normalized * 0.2f + a * 0.8f;
        a2.Normalize();
        desiredPosition = target.position + -a2 * distFromTarget + Vector3.up * camHeight + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        var d = targetRb.linearVelocity.magnitude * 0.25f;
        var forward = target.position - desiredPosition + d * a2 + d * Vector3.down * 0.3f;
        desiredLook = Quaternion.LookRotation(forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredLook, Time.deltaTime * rotationSpeed);
        var b = (float)Mathf.Clamp(70 + (int)(targetRb.linearVelocity.magnitude * 0.35f), 70, 85);
        fov = Mathf.Lerp(fov, b, Time.deltaTime * 5f);
        mainCam.fieldOfView = fov;
        offset = Vector3.Lerp(offset, Vector3.zero, Time.deltaTime * offsetSpeed);
        if (targetCar.acceleration.y > shakeThreshold) {
            var d2 = (Mathf.Clamp(targetCar.acceleration.y, shakeThreshold, 50f) - shakeThreshold / 2f) * 0.14f;
            OffsetCamera(Vector3.down * d2);
        }
    }

    public void AssignTarget(Transform target) {
        // print("assinging target");
        this.target = target;
        targetRb = target.GetComponent<Rigidbody>();
        targetCar = target.GetComponent<Car>();
    }

    public void OffsetCamera(Vector3 offset) {
        if (!readyToOffset) {
            return;
        }

        this.offset += offset;
        readyToOffset = false;
        Invoke("GetReady", 0.5f);
        ShakeController.Instance.Shake();
    }

    void GetReady() {
        readyToOffset = true;
    }
}