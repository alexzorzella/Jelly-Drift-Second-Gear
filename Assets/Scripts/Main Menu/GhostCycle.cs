using TMPro;

public class GhostCycle : ItemCycle {
    public enum Ghost {
        PB,
        Dani,
        Off
    }

    public TextMeshProUGUI ghostText;
    public MapCycle mapCycle;

    Ghost ghost;

    void Awake() {
        max = 3;
    }

    void Start() {
        UpdateText();
    }

    public override void Cycle(int n) {
        if (mapCycle.lockUi.activeInHierarchy) {
            return;
        }

        base.Cycle(n);
        UpdateText();
    }

    public void UpdateText() {
        var selected = (Ghost)this.selected;
        ghost = selected;
        ghostText.text = " (" + selected + ")";
        var str = " (" + selected + ")";
        var str2 = "| ";
        if (selected == Ghost.Dani) {
            str2 += Timer.GetFormattedTime(SaveManager.i.state.daniTimes[mapCycle.selected]);
        }
        else if (selected == Ghost.PB) {
            str2 += Timer.GetFormattedTime(SaveManager.i.state.times[mapCycle.selected]);
        }

        ghostText.text = str2 + str;
        GameState.i.ghost = ghost;
    }
}