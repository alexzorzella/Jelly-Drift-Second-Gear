using UnityEngine;

public class MenuUI : MonoBehaviour {
    public void SetGameModeToRace() {
        GameState.i.gameMode = GameMode.RACE;
    }
    
    public void SetGameModeToTimeTrial() {
        GameState.i.gameMode = GameMode.TIME_TRIAL;
    }

    public void Quit() {
        Application.Quit(1);
    }
}