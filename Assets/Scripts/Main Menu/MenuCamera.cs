using UnityEngine;

public class MenuCamera : MonoBehaviour {
    public float rotationSpeed;
    public Transform target;
    public Vector3 offset;
    public CarDisplay carDisplay;

    void Update() {
        if (!CarDisplay.Instance || !CarDisplay.Instance.currentCar) {
            return;
        }

        transform.RotateAround(carDisplay.currentCar.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        transform.LookAt(carDisplay.currentCar.transform.position + offset);
    }
}