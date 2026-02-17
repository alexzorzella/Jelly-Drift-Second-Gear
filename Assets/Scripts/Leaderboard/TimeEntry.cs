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
        TimeSpan time = TimeSpan.FromSeconds(record.GetTime());
        timeText.text = time.ToString(@"mm\:ss\:fff");
    }
}