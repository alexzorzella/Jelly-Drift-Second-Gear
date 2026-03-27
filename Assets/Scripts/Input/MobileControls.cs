using UnityEngine;

public class MobileControls : MonoBehaviour {
    public MyButton left;
    public MyButton right;
    public MyButton throttle;
    public MyButton breakPedal;
    Car car;

    void Start() {
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            car = GameController.Instance.currentCar.GetComponent<Car>();
            Destroy(car.GetComponent<PlayerInput>());
            return;
        }

        Destroy(gameObject);
    }

    void Update() {
        if (!GameController.Instance.playing) {
            return;
        }

        var steering = 0f;
        var num = 0f;
        if (left.value > 0) {
            steering = -1f;
        }

        if (right.value > 0) {
            steering = 1f;
        }

        if (throttle.value > 0) {
            num = 1f;
        }

        if (breakPedal.value > 0) {
            num = -1f;
        }

        car.steering = steering;
        car.throttle = num;
    }
}