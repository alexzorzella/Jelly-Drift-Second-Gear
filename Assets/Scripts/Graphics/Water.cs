using UnityEngine;

public class Water : MonoBehaviour {
    public Material bad;

    void Start() {
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            GetComponent<MeshRenderer>().material = bad;
        }
    }
}