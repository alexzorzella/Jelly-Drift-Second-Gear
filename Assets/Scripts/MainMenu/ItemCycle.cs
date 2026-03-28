using UnityEngine;

public class ItemCycle : MonoBehaviour {
    public int selected { get; set; }
    public int max { get; set; }
    public bool activeCycle { get; set; } = true;

    public virtual void Cycle(int n) {
        if (!gameObject.activeInHierarchy) {
            return;
        }

        selected += n;
        if (selected >= max) {
            selected = 0;
        }

        if (selected < 0) {
            selected = max - 1;
        }
    }
}