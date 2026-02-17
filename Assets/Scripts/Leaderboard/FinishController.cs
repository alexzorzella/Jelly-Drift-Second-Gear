using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishController : MonoBehaviour {
    public static FinishController Instance;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI pbTimer;
    public TextMeshProUGUI mapName;
    public TextMeshProUGUI newBest;
    public TextMeshProUGUI victoryText;
    public GameObject timePanel;
    public GameObject racePanel;

    void Awake() {
        Instance = this;
    }

    public void Open(bool victory) {
        if (GameState.i.gamemode == Gamemode.TimeTrial) {
            timePanel.SetActive(true);
            mapName.text = MapManager.i.GetSelectedMap().GetName();
            var num = Timer.Instance.GetTimer();

            timer.text = Timer.GetFormattedTime(num);
        }
        else if (GameState.i.gamemode == Gamemode.Race) {
            racePanel.SetActive(true);
            if (victory) {
                victoryText.text = "Victory!";
            }
            else {
                victoryText.text = "Defeat...";
            }
        }

        SaveManager.i.Save();
    }

    public void Restart() {
        GameController.Instance.RestartGame();
        Time.timeScale = 1f;
    }

    public void Menu() {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}