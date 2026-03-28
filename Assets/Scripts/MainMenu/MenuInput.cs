using UnityEngine;

public class MenuInput : MonoBehaviour {
    public static MenuInput Instance;
    public bool horizontalDone;
    public bool verticalDone;
    public int horizontal;
    public int vertical;
    public bool select;
    public int wat;

    void Awake() {
        if (Instance) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update() {
        PlayerInput();
    }

    void PlayerInput() {
    }
}