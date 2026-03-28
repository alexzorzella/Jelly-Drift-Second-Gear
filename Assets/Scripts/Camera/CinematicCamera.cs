using UnityEngine;

public class CinematicCamera : MonoBehaviour {
    public float rotationSpeed;
    public Transform target;
    public Vector3 offset;

    void Update() {
        transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        transform.LookAt(target.position + offset);
    }
}