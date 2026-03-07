using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class WheelSkid : MonoBehaviour {
    const float SKID_FX_SPEED = 0.5f;
    const float MAX_SKID_INTENSITY = 20f;
    const float WHEEL_SLIP_MULTIPLIER = 10f;
    [SerializeField] Rigidbody rb;
    [SerializeField] Skidmarks skidmarksController;
    float lastFixedUpdateTime;
    int lastSkid = -1;
    WheelCollider wheelCollider;
    WheelHit wheelHitInfo;

    protected void Awake() {
        wheelCollider = GetComponent<WheelCollider>();
        lastFixedUpdateTime = Time.time;
    }

    protected void FixedUpdate() {
        lastFixedUpdateTime = Time.time;
    }

    protected void LateUpdate() {
        if (!wheelCollider.GetGroundHit(out wheelHitInfo)) {
            lastSkid = -1;
            return;
        }

        var num = Mathf.Abs(transform.InverseTransformDirection(rb.linearVelocity).x);
        var num2 = wheelCollider.radius * (6.2831855f * wheelCollider.rpm / 60f);
        var num3 = Vector3.Dot(rb.linearVelocity, transform.forward);
        var num4 = Mathf.Abs(num3 - num2) * 10f;
        num4 = Mathf.Max(0f, num4 * (10f - Mathf.Abs(num3)));
        num += num4;
        if (num >= 0.5f) {
            var opacity = Mathf.Clamp01(num / 20f);
            var pos = wheelHitInfo.point + rb.linearVelocity * (Time.time - lastFixedUpdateTime);
            lastSkid = skidmarksController.AddSkidMark(pos, wheelHitInfo.normal, opacity, lastSkid);
            return;
        }

        lastSkid = -1;
    }
}