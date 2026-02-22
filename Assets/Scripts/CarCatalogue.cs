using System.Collections.Generic;
using UnityEngine;

public static class CarCatalogue {
    static int _selectedCar = 0;
    static int _selectedOpponent = 0;
    
    public static void CycleSelectedCar(int amount) {
        IncrementWithOverflow.Run(_selectedCar, cars.Count, amount, out _selectedCar);
    }
    
    public static CarData GetSelectedCarData() {
        return cars[_selectedCar];
    }
    
    public static CarData GetCarAtIndex(int i) {
        return cars[i];
    }

    public static int TotalCars() {
        return cars.Count;
    }
    
    public static CarData GetSelectedOpponentCarData() {
        return opponentCars[_selectedOpponent];
    }
    
    static readonly List<CarData> cars = new() {
        new CarData.Builder(0, "Nissaan 2SX", "180sx").
            WithMaterials("Gray", "Yellow", "Purple", "Blue", "Shadow").Build(),
        new CarData.Builder(1, "Nissaan Silva", "s14").
            WithCarSpecs(3070, stability: 0.5F).
            WithDriftSpecs(1.37F).
            WithMaterials("Gray", "Blue", "Crimson", "Midnight", "Dream").
            WithAudio("s14_accel").Build(),
        new CarData.Builder(11, "Denva", "rx7").
            WithSuspensionSpecs(0.37F).
            WithCarSpecs(3150, stability: 0.1F).
            WithDriftSpecs(1.3F).
            WithMaterials("Scuffed", "Blue", "Shadow", "Sakura", "The Original").
            WithAudio("rx7_accel").Build(),
        new CarData.Builder(20, "Off-Roader", "truck").
            WithSuspensionSpecs(0.45F).
            WithCarSpecs(stability: 0).
            WithDriftSpecs(1.2F).
            WithMaterials("Midnight").Build(),
        new CarData.Builder(20, "Automobile").
            WithSuspensionSpecs(0.45F).
            WithCarSpecs(engineForce: 3200, stability: 0).
            WithDriftSpecs(1.2F).
            WithPhysicsSpecs(mass: 1200).
            WithMaterials("Midnight").Build(),
        new CarData.Builder(20, "Mom Van", "van").
            WithPhysicsSpecs(mass: 1200).
            WithSuspensionSpecs(0.4F).
            WithCarSpecs(engineForce: 3100, stability: 0.1F).
            WithDriftSpecs(1.3F).Build(),
        new CarData.Builder(30, "Accurate Integral", "integra").
            WithCarSpecs(3030, stability: 0.7F).
            WithDriftSpecs(1.4F).
            WithMaterials("Gray", "Midnight", "Crimson", "Yellow", "OJ").
            WithAudio("db8_accel").Build(),
        new CarData.Builder(2, "Nissaan Freshpine", "skyline").
            WithCarSpecs(2850, stability: 0.9F).
            WithDriftSpecs(1, 0.45F).
            WithMaterials("Gray", "Midnight", "Crimson", "Sakura", "Shadow").
            WithAudio("r32_accel").Build(),
        new CarData.Builder(3, "Nissaan Silva HQR", "s14").
            WithPhysicsSpecs(1350, 0, 0.4F).
            WithCarSpecs(antiRoll: 8000, stability: 0).
            WithDriftSpecs(1.2F, 0.8F).
            WithMaterials("Beach").
            WithAudio("s14_accel").Build(),
        new CarData.Builder(35, "Fuji TFM", "ae86").
            WithCarSpecs(3050, stability: 0.25F).
            WithDriftSpecs(1.43F).
            WithMaterials("4th Stage", "Tofu", "Tofu_1", "tofu_2", "ToduHidden", "Tofu").Build(),
        new CarData.Builder(50, "Banana").
            WithSuspensionSpecs(0.55F).
            WithCarSpecs(3000, 1, 5000, 0).
            WithDriftSpecs(1.2F, 0.4F).
            WithMaterials("Banana").Build()
    };

    static readonly List<CarData> opponentCars = new() {
        new CarData.Builder(90, "T54 Grand Turismo", "skyline").
            WithPhysicsSpecs(1400).
            WithCarSpecs(2850, stability: 0).
            WithDriftSpecs(1, 1).
            WithMaterials("Gray", "Midnight", "Crimson", "Sakura", "Shadow").Build(),
        new CarData.Builder(91, "FD8 Grand Turismo", "integra").
            WithPhysicsSpecs(1400).
            WithSuspensionSpecs(0.3F, 0.25F, 20000, 1600).
            WithCarSpecs(3030, antiRoll: 8000).
            WithDriftSpecs(1.4F, 1F).
            WithMaterials("Gray", "Midnight", "Crimson", "Yellow", "OJ").Build(),
        new CarData.Builder(92, "TZ9 Grand Turismo", "rx7").
            WithPhysicsSpecs(1400, 0.6F, 1).
            WithSuspensionSpecs(0.37F).
            WithDriftSpecs(1.3F, 1F).
            WithCarSpecs(0.37F, 1, 6000, 0).
            WithMaterials("Scuffed", "Blue", "Shadow", "Sakura", "The Original").Build()
    };
    
    public static readonly float[] gearEngineForceMultipliers = { 0.6F, 0.75F, 0.9F, 1, 1.15F, -1F };
    public static readonly float[] gearEngineDriftThresholdMultipliers = { 1.2F, 1.1F, 1F, 0.9F, 0.85F, 1F };
}