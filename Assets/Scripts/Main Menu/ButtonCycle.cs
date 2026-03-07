using UnityEngine.UI;

public class ButtonCycle : ItemCycle {
    Button btn;

    void Awake() {
        btn = GetComponent<Button>();
    }

    public override void Cycle(int n) {
        if (!btn.enabled) {
            return;
        }

        btn.onClick.Invoke();
    }
}