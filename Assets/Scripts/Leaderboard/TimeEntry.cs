using System;
using TMPro;
using UnityEngine;

public class TimeEntry : MonoBehaviour {
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI timeText;

    public void Initialize(Record record) {
        // DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(record.GetTime());
        
        nameText.text = record.GetUsername();
        // timeText.text = dateTimeOffset.LocalDateTime.ToShortTimeString();
        timeText.text = FormatMs(record.GetTimeMs());
    }
    
    public static string FormatMs(int millseconds) {
        TimeSpan time = TimeSpan.FromMilliseconds(millseconds);
        string result = time.ToString(@"mm\:ss\:fff");

        return result;
    }
}