using TMPro;

public class DifficultyCycle : ItemCycle {
    public enum Difficulty {
        Easy,
        Normal,
        Hard
    }

    public TextMeshProUGUI ghostText;
    public MapCycle mapCycle;

    void Awake() {
        max = 3;
        selected = SaveManager.i.state.lastDifficulty;
        // print("loaded selected: " + selected);
    }

    void Start() {
        UpdateText();
        // print("in start method: " + selected);
    }

    public override void Cycle(int n) {
        if (mapCycle.lockUi.activeInHierarchy) {
            return;
        }

        base.Cycle(n);
        UpdateText();
    }

    public void UpdateText() {
        var selected = (Difficulty)this.selected;
        ghostText.text = "| " + selected;
        GameState.i.difficulty = selected;
        SaveManager.i.state.lastDifficulty = this.selected;
        SaveManager.i.Save();
        // print("saved last difficulty as:  " + this.selected);
    }

    public void UpdateTextOnly() {
        var selected = (Difficulty)this.selected;
        ghostText.text = "| " + selected;
    }
}