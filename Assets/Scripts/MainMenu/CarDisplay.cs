using UnityEngine;

public class CarDisplay : MonoBehaviour {
    public static CarDisplay Instance;
    public GameObject currentCar;
    public int nCars { get; set; }

    void Awake() {
        if (!(Instance != null) || !(Instance != this)) {
            Instance = this;
        }

        nCars = CarCatalogue.TotalCars();
        SpawnCar(0);
    }

    public void SetSkin(int n) {
        // skin.SetSkin(n);
    }

    public void Hide() {
        currentCar.gameObject.SetActive(false);
    }

    public void Show() {
        currentCar.gameObject.SetActive(true);
    }

    public void SpawnCar(int n) {
        if (currentCar != null) {
            Destroy(currentCar);
        }
        
        currentCar = ResourceLoader.InstantiateObject("Car", transform.position, transform.rotation);
        currentCar.GetComponent<Car>().Initialize(CarCatalogue.GetSelectedCarData(), carType: CarType.DISPLAY);
    }
}