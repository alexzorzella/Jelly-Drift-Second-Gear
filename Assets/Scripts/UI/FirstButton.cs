using UnityEngine;
using UnityEngine.UI;

public class FirstButton : MonoBehaviour {
    Button btn;

    void Awake() {
        btn = GetComponent<Button>();
    }

    void Start() {
        btn.Select();
    }

    public void SelectButton() {
        btn.Select();
    }
}