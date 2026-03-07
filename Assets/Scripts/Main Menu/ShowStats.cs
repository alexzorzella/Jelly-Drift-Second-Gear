using UnityEngine;

public class ShowStats : MonoBehaviour {
    public bool show;

    void OnEnable() {
        if (MenuStats.Instance) {
            MenuStats.Instance.gameObject.SetActive(show);
        }
    }
}