using UnityEngine;

public class About : MonoBehaviour {
    public void OpenUrl(string url) {
        Application.OpenURL(url);
    }
}