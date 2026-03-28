using TMPro;
using UnityEngine.UI;

public class CarCycle : ItemCycle {
    public SkinCycle skinCycle;
    public new TextMeshProUGUI name;
    public Button nextBtn;
    public CarStats carStats;

    void Start() {
        max = CarDisplay.Instance.nCars;
    }

    void OnEnable() {
        if (CarDisplay.Instance) {
            var lastCar = SaveManager.i.state.lastCar;
            selected = lastCar;
            CarDisplay.Instance.SpawnCar(lastCar);
            name.text = CarCatalogue.GetSelectedCarData().GetCarName();
            CarDisplay.Instance.SetSkin(SaveManager.i.state.lastSkin[lastCar]);
            carStats.SetStats(selected);
            skinCycle.selected = SaveManager.i.state.lastSkin[lastCar];
        }
    }

    public override void Cycle(int n) {
        base.Cycle(n);
        
        CarCatalogue.CycleSelectedCar(n);
        skinCycle.SetCarToCycle(selected);
        
        CarDisplay.Instance.SpawnCar(selected);
        name.text = CarCatalogue.GetSelectedCarData().GetCarName();

        carStats.SetStats(selected);
    }

    public void BuyCar() {
    }

    public void SaveCar() {
        SaveManager.i.state.lastCar = selected;
        SaveManager.i.Save();
        GameState.i.car = selected;
        GameState.i.LoadMap();
    }
}