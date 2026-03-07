using TMPro;
using UnityEngine;

public class ProgressController : MonoBehaviour {
    public Transform progress;
    public TextMeshProUGUI level;
    public TextMeshProUGUI money;
    public TextMeshProUGUI xpGained;
    public TextMeshProUGUI moneyGained;
    int currentLevel;
    float currentMoney;
    Vector3 defaultLevelScale;
    float desiredMoney;
    int desiredXp;
    bool ready;
    float xp;

    void Awake() {
        defaultLevelScale = level.transform.localScale;
    }

    void Update() {
        if (!ready || (UnlockManager.Instance && UnlockManager.Instance.gameObject.activeInHierarchy)) {
            return;
        }

        xp = Mathf.Lerp(xp, desiredXp, Time.fixedDeltaTime * 0.5f);
        currentMoney = Mathf.Lerp(currentMoney, desiredMoney, Time.fixedDeltaTime * 0.5f);
        var num = (float)((int)xp - SaveManager.i.state.XpForLevel(currentLevel));
        var num2 = SaveManager.i.state.XpForLevel(currentLevel + 1) -
                   SaveManager.i.state.XpForLevel(currentLevel);
        var x = num / num2;
        progress.localScale = new Vector3(x, 1f, 1f);
        money.text = "$" + Mathf.CeilToInt(currentMoney);
        if (SaveManager.i.state.GetLevel((int)xp) > currentLevel) {
            print("levelled up!");
            level.transform.localScale = defaultLevelScale * 1.5f;
            currentLevel++;
            level.text = "Lvl " + currentLevel;
        }

        ScaleLevel();
    }

    public void SetProgress(int oldXp, int newXp, int oldLevel, int oldMoney, int newMoney) {
        xp = oldXp;
        desiredXp = newXp;
        currentLevel = oldLevel;
        currentMoney = oldMoney;
        desiredMoney = newMoney;
        level.text = "Lvl " + oldLevel;
        money.text = "$" + oldMoney;
        xpGained.text = "+ " + (newXp - oldXp) + "xp";
        moneyGained.text = "+ " + (newMoney - oldMoney) + "$";
        var num = (float)((int)xp - SaveManager.i.state.XpForLevel(currentLevel));
        var num2 = SaveManager.i.state.XpForLevel(currentLevel + 1) -
                   SaveManager.i.state.XpForLevel(currentLevel);
        var x = num / num2;
        progress.localScale = new Vector3(x, 1f, 1f);
        Invoke("GetReady", 0.5f);
    }

    void GetReady() {
        ready = true;
    }

    void ScaleLevel() {
        level.transform.localScale = Vector3.Lerp(level.transform.localScale, defaultLevelScale, Time.deltaTime * 1f);
    }
}