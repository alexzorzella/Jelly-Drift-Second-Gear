using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour, StartListener {
    int gear = 3;

    bool started = false;

    void Start() {
        FindFirstObjectByType<StartHandler>()?.RegsiterListener(this);
    }
    
    public void NotifyCountdownUpdated(int countdown) { }
    public void NotifyStartRace() { started = true; }
    
    public void SetGear(int gear) { this.gear = gear; }
    
    float overrideEngineForce = 0;

    public void SetOverrideEngineForce(float newOverrideEngineForce) {
        overrideEngineForce = newOverrideEngineForce;
    }
    
    Transform centerOfMass;
    
    readonly List<Suspension> wheelPositions = new();

    CarData carData;
    
    public CarData GetCarData() {
        return carData;
    }
    
    bool isCpu;
    
    Collider carCollider;
    Vector3 centerOfGravity;
    
    float dir;
    bool grounded;
    Vector3 lastVelocity;
    
    float yawRate;
    
    public Rigidbody rb { get; set; }
    public float steering { get; set; }
    public float throttle { get; set; }
    public bool braking { get; set; }
    public float speed { get; private set; }
    public float steerAngle { get; set; }
    public Vector3 acceleration { get; private set; }
    
    MultiAudioSource accelerationSource;
    MultiAudioSource decelerationSource;
    MultiAudioSource horn;

    bool isDisplayCar = false;
    
    public void Initialize(CarData carData, bool isCpu = false, bool isDisplayCar = false) {
        this.carData = carData;
        this.isCpu = isCpu;
        this.isDisplayCar = isDisplayCar;

        gameObject.name = carData.GetCarName();

        GameObject carModel = Instantiate(carData.GetModel(), transform);
        carModel.transform.localPosition = Vector3.zero;

        if (!isCpu) {
            gameObject.AddComponent<PlayerInput>().Initialize(this);
        }
        else {
            Camera.main.gameObject.SetActive(false);
            Destroy(transform.Find("XR Interaction Manager"));
            Destroy(transform.Find("XR Origin (VR)"));
        }
        
        // Materials are set here

        centerOfMass = carModel.transform.Find("CenterOfMass");
        centerOfMass.transform.SetParent(transform);
        
        rb = GetComponent<Rigidbody>();

        rb.mass = carData.GetMass();
        rb.linearDamping = carData.GetLinearDamping();
        rb.angularDamping = carData.GetAngularDamping();

        Suspension frontLeft = carModel.transform.Find("FrontLeft").GetComponent<Suspension>();
        Suspension frontRight = carModel.transform.Find("FrontRight").GetComponent<Suspension>();
        Suspension rearLeft = carModel.transform.Find("RearLeft").GetComponent<Suspension>();
        Suspension rearRight = carModel.transform.Find("RearRight").GetComponent<Suspension>();

        wheelPositions.Add(frontLeft);
        wheelPositions.Add(frontRight);
        wheelPositions.Add(rearLeft);
        wheelPositions.Add(rearRight);
        
        foreach (var suspension in wheelPositions) {
            suspension.Initialize(this);
            suspension.gameObject.transform.SetParent(transform);
        }
        
        gameObject.AddComponent<AntiRoll>().Initialize(
            carData.GetAntiRoll(), 
            frontLeft, 
            frontRight, 
            rearLeft, 
            rearRight);

        if (centerOfMass) {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        carCollider = GetComponentInChildren<Collider>();

        accelerationSource = MultiAudioSource.FromResource(gameObject, carData.GetAccelerateSoundName(), loop: true, spatialBlend: 1, minDistance: 5, maxDistance: 15);
        decelerationSource = MultiAudioSource.FromResource(gameObject, carData.GetDecelerateSoundName(), loop: true, spatialBlend: 1, minDistance: 5, maxDistance: 15);
        horn = MultiAudioSource.FromResource(gameObject, "car_horn", loop: true, spatialBlend: 1, minDistance: 5, maxDistance: 15);
        
        accelerationSource.SetVolume(0);
        decelerationSource.SetVolume(0);
        
        accelerationSource.PlayRoundRobin();
        decelerationSource.PlayRoundRobin();
    }

    void Update() {
        if (carData == null) {
            return;
        }
        
        MoveWheels();
        HandleAudio();
        CheckGrounded();
        Steering();
    }

    void FixedUpdate() {
        if (carData == null) {
            return;
        }
        
        Movement();
    }
    
    void HandleAudio() {
        accelerationSource.SetVolume(Mathf.Lerp(accelerationSource.GetVolume(),
            Mathf.Abs(throttle) + Mathf.Abs(speed / 80f),
            Time.deltaTime * 6f));
        decelerationSource.SetVolume(Mathf.Lerp(decelerationSource.GetVolume(), speed / 40f - throttle * 0.5f, Time.deltaTime * 4f));
        accelerationSource.SetPitch(Mathf.Lerp(accelerationSource.GetPitch(), 0.65f + Mathf.Clamp(Mathf.Abs(speed / 160f) * (1 + (Mathf.Abs((float)gear) / 5)), 0f, 1f),
            Time.deltaTime * 5f));
        if (!grounded) {
            accelerationSource.SetPitch(Mathf.Lerp(accelerationSource.GetPitch(), 1.5f, Time.deltaTime * 8f));
        }
        
        decelerationSource.SetPitch(Mathf.Lerp(decelerationSource.GetPitch(), 0.5f + speed / 40f, Time.deltaTime * 2f));
    }

    void Movement() {
        if (!started) {
            return;
        }
        
        var linearVelocity = XZVector(rb.linearVelocity);
        var inverseTransformDir = transform.InverseTransformDirection(XZVector(rb.linearVelocity));
        
        acceleration = (lastVelocity - inverseTransformDir) / Time.fixedDeltaTime;
        dir = Mathf.Sign(transform.InverseTransformDirection(linearVelocity).z);
        speed = linearVelocity.magnitude * 3.6f * dir;
        
        var absYVel = Mathf.Abs(rb.angularVelocity.y);
        
        foreach (var suspension in wheelPositions) {
            if (suspension.grounded) {
                var vector3 = XZVector(rb.GetPointVelocity(suspension.hitPos));
                transform.InverseTransformDirection(vector3);
                var a = Vector3.Project(vector3, suspension.transform.right);
                var d = 1f;
                var num2 = 1f;
                
                if (suspension.terrain) {
                    num2 = 0.6f;
                    d = 0.75f;
                }

                var f = Mathf.Atan2(inverseTransformDir.x, inverseTransformDir.z);
                if (braking) {
                    num2 -= 0.6f;
                }

                var currentThreshold = carData.GetDriftThreshold() * CarCatalogue.gearEngineDriftThresholdMultipliers[gear];
                
                if (absYVel > 1f) {
                    currentThreshold -= 0.2f;
                }

                var flag = false;
                if (Mathf.Abs(f) > currentThreshold) {
                    var num4 = Mathf.Clamp(Mathf.Abs(f) * 2.4f - currentThreshold, 0f, 1f);
                    num2 = Mathf.Clamp(1f - num4, 0.05f, 1f);
                    var magnitude = rb.linearVelocity.magnitude;
                    flag = true;
                    if (magnitude < 8f) {
                        num2 += (8f - magnitude) / 8f;
                    }

                    if (absYVel < CarData.yawGripThreshold) {
                        var num5 = (CarData.yawGripThreshold - absYVel) / CarData.yawGripThreshold;
                        num2 += num5 * CarData.yawGripMultiplier;
                    }

                    if (Mathf.Abs(throttle) < 0.3f) {
                        num2 += 0.1f;
                    }

                    num2 = Mathf.Clamp(num2, 0f, 1f);
                }

                var d2 = 1f;
                if (flag) {
                    d2 = carData.GetDriftMultiplier();
                }

                Vector3 forceAtPosition = 
                    suspension.transform.forward * 
                    throttle * 
                    GetEngineForce() *
                    CarCatalogue.gearEngineForceMultipliers[gear] *
                    d2 * 
                    d;
                
                rb.AddForceAtPosition(forceAtPosition, suspension.hitPos);

                var a2 = a * rb.mass * d * num2;
                rb.AddForceAtPosition(-a2, suspension.hitPos);
                rb.AddForceAtPosition(suspension.transform.forward * a2.magnitude * 0.25f, suspension.hitPos);
                var num6 = Mathf.Clamp(1f - num2, 0f, 1f);
                if (Mathf.Sign(dir) != Mathf.Sign(throttle) && speed > 2f) {
                    num6 = Mathf.Clamp(num6 + 0.5f, 0f, 1f);
                }

                suspension.traction = num6;
                var force = -CarData.dragForce * linearVelocity;
                rb.AddForce(force);
                var force2 = -CarData.rollFriction * linearVelocity;
                rb.AddForce(force2);
            }
        }

        StandStill();
        lastVelocity = inverseTransformDir;
    }

    float GetEngineForce() {
        float result = isCpu ? overrideEngineForce : carData.GetEngineForce();
        return result;
    }

    void StandStill() {
        if (Mathf.Abs(speed) >= 1f || !grounded || throttle != 0f) {
            rb.linearDamping = 0f;
            return;
        }

        var flag = true;
        var array = wheelPositions;
        for (var i = 0; i < array.Count; i++) {
            if (Vector3.Angle(array[i].hitNormal, Vector3.up) > 1f) {
                flag = false;
                break;
            }
        }

        if (flag) {
            rb.linearDamping = (1f - Mathf.Abs(speed)) * 30f;
            return;
        }

        rb.linearDamping = 0f;
    }

    void Steering() {
        foreach (var suspension in wheelPositions) {
            if (!suspension.rearWheel) {
                suspension.steeringAngle = steering * (37f - Mathf.Clamp(speed * 0.35f - 2f, 0f, 17f));
                steerAngle = suspension.steeringAngle;
            }
        }
    }

    Vector3 XZVector(Vector3 v) {
        return new Vector3(v.x, 0f, v.z);
    }

    void MoveWheels() {
        foreach (var suspension in wheelPositions) {
            var suspensionLength = carData.GetSuspensionLength();
            var hitHeight = suspension.hitHeight;
            var y = Mathf.Lerp(suspension.wheelObject.transform.localPosition.y, -hitHeight + suspensionLength, Time.deltaTime * 20f);
            suspension.wheelObject.transform.localPosition = new Vector3(0f, y, 0f);
            suspension.wheelObject.Rotate(Vector3.right, XZVector(rb.linearVelocity).magnitude * 1f * dir);
            suspension.wheelObject.localScale = Vector3.one * (suspensionLength * 2f);
            suspension.transform.localScale = Vector3.one / transform.localScale.x;
        }
    }

    void CheckGrounded() {
        grounded = false;
        var array = wheelPositions;
        for (var i = 0; i < array.Count; i++) {
            if (array[i].grounded) {
                grounded = true;
                break;
            }
        }
    }

    public List<Suspension> GetWheelPositions() {
        return wheelPositions;
    }
    
    public void PlayHorn() {
        horn.PlayRoundRobin();
    }

    public void StopHorn() {
        horn.Stop();
    }
}