using UnityEngine;

public class CameraCulling : MonoBehaviour {
    public static CameraCulling Instance;
    Camera cam;

    void Awake() {
        Instance = this;
        cam = GetComponent<Camera>();
        UpdateCulling();
    }

    public void UpdateCulling() {
        var array = new float[32];
        var quality = SaveState.i.quality;
        if (quality == 0) {
            array[12] = 120f;
        }
        else if (quality == 1) {
            array[12] = 300f;
        }
        else {
            array[12] = 1000f;
        }

        cam.layerCullDistances = array;
        cam.layerCullSpherical = true;
    }
}