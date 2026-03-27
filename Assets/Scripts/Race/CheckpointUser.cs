using UnityEngine;

public class CheckpointUser : MonoBehaviour {
    public bool player = true;

    bool[] checkedPoints;

    void Awake() {
        checkedPoints = new bool[GameController.Instance.checkPoints.childCount];
    }

    public int GetCurrentCheckpoint(bool loopMap) {
        var num = 0;
        var num2 = 1;
        if (!loopMap) {
            num2 = 0;
        }

        var num3 = num2;
        while (num3 < checkedPoints.Length && checkedPoints[num3]) {
            num++;
            num3++;
        }

        if (!loopMap) {
            return num - 1;
        }

        return num;
    }

    public bool CheckPoint(CheckPoint p) {
        if (p.siblingIndex != GameController.Instance.finalCheckpoint) {
            ClearCheckpoint(p.siblingIndex);
            return true;
        }

        if (ReadyToFinish()) {
            GameController.Instance.FinishRace(player, transform);
            ClearCheckpoint(p.siblingIndex);
            return true;
        }

        return false;
    }

    void ClearCheckpoint(int n) {
        if (checkedPoints[n]) {
            return;
        }

        if (GameController.Instance.finalCheckpoint > 0 && n == 0) {
            checkedPoints[0] = true;
            return;
        }

        checkedPoints[n] = true;
        if (!player) {
            return;
        }

        UIManager.Instance.DisplaySplit();
    }

    public void ForceCheckpoint(int n) {
        checkedPoints[n] = true;
    }

    bool ReadyToFinish() {
        var num = 0;
        for (var i = 0; i < checkedPoints.Length; i++) {
            if (checkedPoints[i]) {
                num++;
            }
        }

        return num >= checkedPoints.Length - 1;
    }
}