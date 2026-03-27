using TMPro;
using UnityEngine;

public class CarButton : MonoBehaviour {
    public enum ButtonState {
        NEXT,
        BUY_SKIN,
        BUY_CAR
    }

    public TextMeshProUGUI text;
    public CarCycle carCycle;
    public SkinCycle skinCycle;

    ButtonState state;

    void Awake() {
        SetState(ButtonState.NEXT);
    }

    public void SetState(ButtonState state) {
        this.state = state;
        if (state == ButtonState.NEXT) {
            text.text = "Next";
            return;
        }

        text.text = "Buy";
    }

    public void Use() {
        if (state == ButtonState.NEXT) {
            carCycle.SaveCar();
            return;
        }

        if (state == ButtonState.BUY_SKIN) {
            skinCycle.BuySkin();
            return;
        }

        if (state == ButtonState.BUY_CAR) {
            carCycle.BuyCar();
        }
    }
}