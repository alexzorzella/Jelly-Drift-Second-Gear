using TMPro;

public class SkinCycle : ItemCycle {
    public TextMeshProUGUI text;
    public CarCycle carCycle;
    public CarButton carBtn;
    public MenuStats menuStats;
    public UnlockManager unlockManager;

    void Start() {
        max = SaveManager.i.state.skins[carCycle.selected].Length;
        UpdateColor();
    }

    public void SetCarToCycle(int n) {
        // selected = SaveManager.Instance.state.lastSkin[n];
        // max = SaveManager.Instance.state.skins[n].Length;
    }

    public override void Cycle(int n) {
        base.Cycle(n);
        UpdateColor();
    }

    public void UpdateColor() {
        // if (!SaveManager.Instance.state.carsUnlocked[carCycle.selected]) {
        //     return;
        // }

        CarDisplay.Instance.SetSkin(selected);
        
        // var num = 0;
        // if (SaveManager.Instance.state.skins[carCycle.selected][selected]) {
        //     num = selected;
        //     SaveManager.Instance.state.lastSkin[carCycle.selected] = num;
        //     SaveManager.Instance.Save();
        // }

        // GameState.Instance.skin = num;
        // UpdateText(num == selected);
    }

    public void UpdateText(bool unlocked) {
        var selected = carCycle.selected;
        var selected2 = this.selected;
        carBtn.SetState(CarButton.ButtonState.Next);
        var text = "<size=60%>";
        if (!SaveManager.i.state.skins[selected][selected2]) {
            if (selected < 5) {
                if (selected2 == 1) {
                    // text = text + "Complete " + MapManager.i.GetSelectedMap().GetName() + " on hard difficulty";
                }
                else if (selected2 == 2) {
                    // text = text + "Complete " + MapManager.GetSelectedMa.name + " 3-star time";
                }
                else {
                    carBtn.SetState(CarButton.ButtonState.BuySkin);
                    var skinPrice = PlayerSave.GetSkinPrice(selected, selected2);
                    text = string.Concat(text, "<size=80%><font=\"Ubuntu-Bold SDF\">Buy (", skinPrice, "$)");
                }
            }
        
            if (selected == 5) {
                text += "Beat the ghost of Dani on all maps..";
            }
        }
        else {
            // text = CarDisplay.Instance.currentCar.GetComponent<CarSkin>().GetSkinName(this.selected);
        }

        this.text.text = "| " + text;
    }

    public void BuySkin() {
        var skinPrice = PlayerSave.GetSkinPrice(carCycle.selected, selected);
        if (SaveManager.i.state.money >= skinPrice) {
            SaveManager.i.state.money -= skinPrice;
            SaveManager.i.state.skins[carCycle.selected][selected] = true;
            SaveManager.i.Save();
            UpdateText(true);
            unlockManager.unlocks.Add(new UnlockManager.Unlock(UnlockManager.UnlockType.Skin, carCycle.selected,
                selected));
            unlockManager.gameObject.SetActive(true);
            menuStats.UpdateStats();
            UpdateColor();
            SoundManager.i.PlayMoney();
            return;
        }

        SoundManager.i.PlayError();
    }
}