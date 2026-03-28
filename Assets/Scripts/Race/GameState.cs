using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState {
    static GameState _i;
	
    public static GameState i {
        get {
            if (_i == null) {
                _i = new GameState();
            }
            
            return _i;
        }
    }
    
    public GhostCycle.Ghost ghost;
    public DifficultyCycle.Difficulty difficulty = DifficultyCycle.Difficulty.Normal;
    public int car { get; set; } = 1;
    public int map { get; set; }
    public GameMode gameMode { get; set; }
    public int skin { get; set; } = 1;

    public void LoadMap() {
        SceneManager.LoadScene(MapManager.i.GetSelectedMap().GetNameFormatted());
    }
}