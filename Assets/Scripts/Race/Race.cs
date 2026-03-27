using UnityEngine;

public class Race : MonoBehaviour {
    public GameController gameController;
    public GameObject enemyCarObject { get; set; }

    void Awake() {
        if (GameState.i.gameMode != GameMode.RACE) {
            Destroy(this);
            return;
        }

        gameController = gameObject.GetComponent<GameController>();
        var startPos = gameController.startPos;
        
        enemyCarObject = ResourceLoader.InstantiateObject("Car", startPos.position + startPos.right * 2F, startPos.rotation);

        Car enemyCar = enemyCarObject.GetComponent<Car>();
        
        enemyCar.Initialize(CarCatalogue.GetSelectedOpponentCarData(), carType: CarType.AGENT);
    }

    void Start() {
        enemyCarObject.AddComponent<CheckpointUser>().player = false;
        GameController.Instance.currentCar.AddComponent<CheckpointUser>();
    }
}