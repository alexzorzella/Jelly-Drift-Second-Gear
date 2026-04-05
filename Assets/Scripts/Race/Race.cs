using UnityEngine;

public class Race : MonoBehaviour {
    GameController gameController;
    GameObject enemyCarObject { get; set; }
    
    void Awake() {
        if (GameState.i.gameMode != GameMode.RACE) {
            Destroy(this);
            return;
        }
        
        Debug.Log("Game is a race");

        gameController = gameObject.GetComponent<GameController>();
        var startPos = gameController.StartTransform();
        
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