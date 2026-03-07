using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {
    public static Pause Instance;
    public GameObject pauseMenu;
    public bool paused { get; set; }

    void Awake() {
        Instance = this;
    }

    public void PauseGame() {
        if (paused) {
            return;
        }

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        paused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        paused = false;
        print("resuiming game");
    }

    public void Recover() {
        Time.timeScale = 1f;
        GameController.Instance.Recover();
        ResumeGame();
    }

    public void RestartGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        paused = false;
        GameController.Instance.RestartGame();
    }

    public void Options() {
    }

    public void TogglePause() {
        if (!GameController.Instance.playing) {
            return;
        }

        if (!paused) {
            PauseGame();
            return;
        }

        ResumeGame();
    }

    public void Quit() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
        paused = false;
    }
}