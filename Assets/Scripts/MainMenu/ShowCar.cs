using UnityEngine;

public class ShowCar : MonoBehaviour {
    public bool show;

    void OnEnable() {
        if (!CarDisplay.Instance || !CarDisplay.Instance.currentCar) {
            return;
        }

        if (show) {
            CarDisplay.Instance.Show();
        }
    }
}