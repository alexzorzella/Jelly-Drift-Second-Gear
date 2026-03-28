using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishController : MonoBehaviour {
    public static FinishController Instance;
    public TextMeshProUGUI victoryText;
    public GameObject racePanel;

    public Leaderboard leaderboard;
    
    void Awake() {
        Instance = this;
    }

    public void Open(bool victory) {
        racePanel.SetActive(true);
            
        if (GameState.i.gameMode == GameMode.TIME_TRIAL) {
            leaderboard.ClockTime();
        } else if (GameState.i.gameMode == GameMode.RACE) {
            if (victory) {
                victoryText.text = "Victory!";
                leaderboard.ClockTime();
            }
            else {
                victoryText.text = "Defeat...";
            }
        }

        SaveManager.i.Save();
    }

    public void Restart() {
        if (leaderboard.AwaitingSubmission()) {
            return;
        }
        
        GameController.Instance.RestartGame();
        Time.timeScale = 1f;
    }

    public void Menu() {
        if (leaderboard.AwaitingSubmission()) {
            return;
        }
        
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}