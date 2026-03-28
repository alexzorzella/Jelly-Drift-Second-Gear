using TMPro;
using UnityEngine;

public class SettingCycle : ItemCycle {
    public GameObject optionsParent;
    public SettingsUi settings;

    readonly Color deselectedC = new(1f, 1f, 1f, 0.3f);
    readonly Color selectedC = new(1f, 1f, 1f);
    public TextMeshProUGUI[] options { get; private set; }

    void Awake() {
        options = optionsParent.transform.GetChild(transform.GetSiblingIndex())
            .GetComponentsInChildren<TextMeshProUGUI>();
        max = options.Length;
        UpdateOptions();
    }

    public override void Cycle(int n) {
        base.Cycle(n);
        UpdateOptions();
        settings.UpdateSettings();
    }

    public void UpdateOptions() {
        for (var i = 0; i < max; i++) {
            options[i].color = deselectedC;
            if (i == selected) {
                options[i].color = selectedC;
            }
        }
    }
}