using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {
    void Start() {
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 0;
        // MonoBehaviour.print("counting");
        // if (InitializeAds.Instance)
        // {
        // 	InitializeAds.Instance.MenuCount();
        // }
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() {
        Application.Quit(1);
    }
}