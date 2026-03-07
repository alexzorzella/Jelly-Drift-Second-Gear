using TMPro;
using UnityEngine;

public class MenuStats : MonoBehaviour {
    public static MenuStats Instance;
    public TextMeshProUGUI level;
    public TextMeshProUGUI money;
    public Transform currentXp;
    float currMoney;

    void Start() {
        Instance = this;
        UpdateStats();
        currMoney = SaveManager.i.state.money;
        money.text = "$" + currMoney;
    }

    void Update() {
        currMoney = Mathf.Lerp(currMoney, SaveManager.i.state.money, Time.deltaTime * 3f);
        money.text = "$" + Mathf.CeilToInt(currMoney);
    }

    public void UpdateStats() {
        var x = SaveManager.i.state.LevelProgress();
        currentXp.transform.localScale = new Vector3(x, 1f, 1f);
        level.text = "Lvl" + SaveManager.i.state.GetLevel();
    }
}