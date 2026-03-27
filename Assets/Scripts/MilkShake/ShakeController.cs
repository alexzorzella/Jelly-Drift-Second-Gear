using MilkShake;
using UnityEngine;

public class ShakeController : MonoBehaviour {
    public static ShakeController Instance;
    public Car car;
    public ShakePreset preset;
    public ShakePreset crashShake;
    ShakeInstance shakeInstance;
    Shaker shaker;

    void Awake() {
        Instance = this;
    }

    void Start() {
        shaker = CameraController.Instance.transform.GetComponentInChildren<Shaker>();
        shakeInstance = shaker.Shake(preset);
        shakeInstance.StrengthScale = 0f;
    }

    void FixedUpdate() {
        if (!car) {
            return;
        }

        var magnitude = car.acceleration.magnitude;
        var num = 0f;
        foreach (var suspension in car.GetWheelPositions()) {
            if (suspension.traction > num) {
                num = suspension.traction;
            }
        }

        if (car.speed < 2f) {
            num = 0f;
        }

        shakeInstance.StrengthScale = Mathf.Clamp(num * 0.5f, 0f, 1f);
    }

    public void Shake() {
        shaker.Shake(crashShake);
    }
}