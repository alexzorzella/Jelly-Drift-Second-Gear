using UnityEngine;

public class KeepRotation : MonoBehaviour {
    void Update() {
        var eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.x = 0f;
        transform.rotation = Quaternion.Euler(eulerAngles);
    }
}