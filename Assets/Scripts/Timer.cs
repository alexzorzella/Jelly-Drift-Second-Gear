using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
    bool stop;

    TextMeshProUGUI text;
    float timer;
    public static Timer Instance { get; set; }

    void Awake() {
        Instance = this;
        text = GetComponent<TextMeshProUGUI>();
        stop = false;
        StartTimer();
    }

    void Update() {
        if (!GameController.Instance) {
            return;
        }

        if (!GameController.Instance.playing || stop) {
            return;
        }

        timer += Time.deltaTime;
        AutoSplitterData.inGameTime = timer;
        text.text = GetFormattedTime(timer);
    }

    public void StartTimer() {
        stop = false;
        timer = 0f;
    }

    public static string GetFormattedTime(float f) {
        if (f == 0f) {
            return "no time";
        }

        var arg = Mathf.Floor(f / 60f).ToString("00");
        var arg2 = Mathf.Floor(f % 60f).ToString("00");
        (f * 1000f % 1000f).ToString("000");
        var text = (f * 100f % 100f).ToString("00");
        if (text.Equals("100")) {
            text = "99";
        }

        return string.Format("{0}:{1}:{2}", arg, arg2, text);
    }

    public float GetTimer() {
        return timer;
    }

    public void Stop() {
        stop = true;
    }

    public int GetMinutes() {
        return (int)Mathf.Floor(timer / 60f);
    }

    public int GetMilliseconds() {
        return (int)Mathf.Floor(timer * 1000F);
    }
}