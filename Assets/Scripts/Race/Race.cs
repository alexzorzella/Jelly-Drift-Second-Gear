using UnityEngine;

public class Race : MonoBehaviour {
    public GameObject enemyCarPrefab;
    GameController gameController;
    public GameObject enemyCarObject { get; set; }

    void Awake() {
        if (GameState.i.gamemode != Gamemode.Race) {
            Destroy(this);
            return;
        }

        gameController = gameObject.GetComponent<GameController>();
        var startPos = gameController.startPos;
        
        enemyCarObject = ResourceLoader.InstantiateObject("Car", startPos.position + startPos.right * 2F, startPos.rotation);

        Car enemyCar = enemyCarObject.GetComponent<Car>();
        
        enemyCar.Initialize(CarCatalogue.GetSelectedOpponentCarData(), true);
        
        CarAi carAi = enemyCarObject.AddComponent<CarAi>();
        carAi.Initialize(enemyCar);
        carAi.SetPath(gameController.path);
    }

    void Start() {
        enemyCarObject.AddComponent<CheckpointUser>().player = false;
        GameController.Instance.currentCar.AddComponent<CheckpointUser>();
    }
}