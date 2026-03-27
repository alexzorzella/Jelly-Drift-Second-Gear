using UnityEngine;
using UnityEngine.UI;

public class SliderSettingCycle : ItemCycle {
    public Color deselectedC = new(1f, 1f, 1f, 0.3f);
    public Color selectedC = new(1f, 1f, 1f);
    public GameObject optionsParent;
    public SettingsUi settings;
    public Image[] options { get; private set; }

    void Awake() {
        options = optionsParent.transform.GetChild(transform.GetSiblingIndex()).GetComponentsInChildren<Image>();
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
            if (i <= selected) {
                options[i].color = selectedC;
            }
            else {
                options[i].color = deselectedC;
            }
        }
    }
}