using System.Resources;
using UnityEngine;

public class Suspension : MonoBehaviour {
    public Transform wheelObject;
    public bool rearWheel;

    [HideInInspector] public bool skidding;

    [HideInInspector] public float grip;

    public bool showFx = true;
    public AudioSource skidSfx;
    public ParticleSystem smokeFx;
    public ParticleSystem spinFx;
    public float steeringAngle;
    public float traction;
    public bool spinning;
    public LayerMask whatIsGround;
    public Vector3 hitPos;
    public Vector3 hitNormal;
    public float hitHeight;
    public bool grounded;
    public float lastCompression;
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;
    readonly float steerTime = 15f;
    Rigidbody bodyRb;

    Car car;
    float damperForce;
    float lastLength;
    int lastSkid;
    float maxEmission;
    float maxLength;
    MeshRenderer mesh;
    float minLength;
    float raycastOffset;
    ParticleSystem.EmissionModule smokeEmitting;
    ParticleSystem.EmissionModule spinEmitting;
    float springForce;
    float springLength;
    float springVelocity;
    float wheelAngleVelocity;
    public bool terrain { get; set; }

    public void Initialize(Car car) {
        this.car = car;
        bodyRb = car.GetComponent<Rigidbody>();
        raycastOffset = car.GetCarData().GetSuspensionLength() * 0.5f;

        if (smokeFx != null) {
            smokeEmitting = smokeFx.emission;
        }

        if (spinFx != null) {
            spinEmitting = spinFx.emission;
        }
        
        wheelObject = ResourceLoader.InstantiateObject("Wheel").transform;
            
        wheelObject.SetParent(transform);
        wheelObject.localPosition = Vector3.zero;
        wheelObject.localRotation = Quaternion.identity;
        wheelObject.localScale = Vector3.one * car.GetCarData().GetSuspensionLength() * 2f;
    }

    void Update() {
        if (car == null) {
            return;
        }
        
        DebugTraction();
        if (rearWheel) {
            return;
        }

        wheelAngleVelocity = Mathf.Lerp(wheelAngleVelocity, steeringAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngleVelocity);
    }

    void FixedUpdate() {
        if (car == null) {
            return;
        }
        
        NewSuspension();
    }

    void LateUpdate() {
        if (car == null) {
            return;
        }
        
        if (!showFx) {
            return;
        }

        if (traction > 0.05f && hitPos != Vector3.zero && grounded) {
            smokeEmitting.enabled = true;
            if (Skidmarks.Instance) {
                lastSkid = Skidmarks.Instance.AddSkidMark(hitPos + bodyRb.linearVelocity * Time.fixedDeltaTime,
                    hitNormal, traction * 0.9f, lastSkid);
            }
        }
        else {
            smokeEmitting.enabled = false;
            lastSkid = -1;
        }

        if (skidSfx) {
            var num = 1f;
            if (bodyRb.linearVelocity.magnitude < 2f) {
                num = 0f;
            }

            skidSfx.volume = traction * num;
            skidSfx.pitch = 0.3f + 0.4f * Mathf.Clamp(traction * 0.5f, 0f, 1f);
        }

        if (!rearWheel) {
            return;
        }

        if (traction > 0.15f && grounded) {
            spinEmitting.enabled = true;
            spinEmitting.rateOverTime = Mathf.Clamp(traction * 60f, 20f, 400f);
            return;
        }

        spinEmitting.enabled = false;
    }

    void DebugTraction() {
    }

    void NewSuspension() {
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
        var suspensionLength = car.GetCarData().GetSuspensionLength();
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, -transform.up, out raycastHit, maxLength + suspensionLength)) {
            lastLength = springLength;
            springLength = raycastHit.distance - suspensionLength;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;
            var force = (springForce + damperForce) * transform.up;
            bodyRb.AddForceAtPosition(force, raycastHit.point);
            terrain = raycastHit.collider.gameObject.CompareTag("Terrain");
            hitPos = raycastHit.point;
            hitNormal = raycastHit.normal;
            hitHeight = raycastHit.distance;
            grounded = true;
            return;
        }

        grounded = false;
        hitHeight = car.GetCarData().GetSuspensionLength() + car.GetCarData().GetRestHeight();
    }
}