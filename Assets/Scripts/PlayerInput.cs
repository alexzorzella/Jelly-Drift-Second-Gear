using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, StartListener {
    public Car car;

    PXN pxn;

    bool started = false;

    void Start() {
        FindFirstObjectByType<StartHandler>().RegsiterListener(this);
    }

    public void Initialize(Car car) {
        this.car = car;
        
        pxn = new PXN();
        
        pxn.Car.Throttle.performed += Throttle;
        pxn.Car.Wheel.performed += Steering;
        pxn.Car.Brake.performed += Brake;
        
        pxn.Car.Reverse.performed += Reverse;
        pxn.Car.First.performed += First;
        pxn.Car.Second.performed += Second;
        pxn.Car.Third.performed += Third;
        pxn.Car.Fourth.performed += Fourth;
        pxn.Car.Fifth.performed += Fifth;

        pxn.Car.Horn.performed += Horn;

        pxn.Car.WheelKeyboard.performed += WheelKeyboard;

        pxn.Car.ThrottleKey.performed += ThrottleKey;
        pxn.Car.ReverseKey.performed += ReverseKey;
        
        pxn.Enable();
    }

    void ThrottleKey(InputAction.CallbackContext context) {
        if (!started) {
            return;
        }
        
        if (context.action.IsPressed()) {
            car.throttle = 1;
        } else {
            car.throttle = 0;
        }
    }

    void ReverseKey(InputAction.CallbackContext context) {
        if (context.action.IsPressed()) {
            car.SetGear(5);
            car.throttle = 1;
        } else {
            car.SetGear(2);
            car.throttle = 0;
        }
    }
    
    void WheelKeyboard(InputAction.CallbackContext context) {
        car.steering = context.ReadValue<float>();
    }
    
    void Menu(InputAction.CallbackContext context) {
        Debug.Log("Menu");
    }

    void Throttle(InputAction.CallbackContext context) {
        car.throttle = -(context.ReadValue<float>() - 1) / 2;
    }

    const float steeringMultiplier = 1.5F;
    const float exponential = 1;
    
    void Steering(InputAction.CallbackContext context) {
        float steering = context.ReadValue<float>();

        if (steering > 0) {
            steering = Mathf.Pow((1 - steering) * steeringMultiplier, exponential);
        }
        else {
            steering = -Mathf.Pow((1 + steering) * steeringMultiplier,exponential);
        }

        steering = Mathf.Clamp(steering, -1F, 1F);
        
        car.steering = steering;
    }

    void Brake(InputAction.CallbackContext context) {
        car.braking = context.ReadValue<float>() > 0.02F;
    }

    void Horn(InputAction.CallbackContext context) {
        if (context.action.IsPressed()) {
            car.PlayHorn();
        } else {
            car.StopHorn();
        }
    }

    void First(InputAction.CallbackContext context) { car.SetGear(0); }
    void Second(InputAction.CallbackContext context) { car.SetGear(1); }
    void Third(InputAction.CallbackContext context) { car.SetGear(2); }
    void Fourth(InputAction.CallbackContext context) { car.SetGear(3); }
    void Fifth(InputAction.CallbackContext context) { car.SetGear(4); }
    void Reverse(InputAction.CallbackContext context) { car.SetGear(5); }

    void OnEnable() {
        if (pxn != null) {
            pxn.Enable();
        }
    }

    void OnDisable() {
        pxn.Disable();
    }

    public void NotifyCountdownUpdated(int countdown) {
        
    }

    public void NotifyStartRace() {
        started = true;
    }
}