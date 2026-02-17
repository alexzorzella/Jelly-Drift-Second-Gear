using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {
    public VerticalLayoutGroup verticalLayoutGroup;
    RectTransform entryParent;

    public TextMeshProUGUI timeText;
    
    public TMP_InputField usernameInput;
    public TMP_InputField messageInput;
    public GameObject submitButton;
    
    readonly List<GameObject> timeEntryPool = new();

    int timeMs = -1;
    
    void Awake() {
        entryParent = verticalLayoutGroup.GetComponent<RectTransform>();
        DisableInput();
        RecordUtil.Read();
        Refresh();
    }

    void Refresh() {
        while (timeEntryPool.Count > 0) {
            GameObject timeEntry = timeEntryPool[0];
            timeEntryPool.RemoveAt(0);
            Destroy(timeEntry);
        }

        float height = 0;
        
        int stageId = MapManager.i.GetSelectedMap().GetId();

        List<Record> records = new();

        foreach (var record in RecordUtil.records) {
            if (record.GetStageId() != stageId) {
                continue;
            }

            records.Add(record);
        }
        
        records.Sort((x, y) => x.GetTimeMs().CompareTo(y.GetTimeMs()));
        
        foreach (Record record in records) {
            GameObject timeEntry = ResourceLoader.InstantiateObject("TimeEntry");
            timeEntry.GetComponent<TimeEntry>().Initialize(record);
            timeEntry.transform.SetParent(entryParent);
            
            height += timeEntry.GetComponent<RectTransform>().rect.height;
            height += verticalLayoutGroup.spacing;
            
            timeEntryPool.Add(timeEntry);
        }
        
        entryParent.sizeDelta = new Vector2(entryParent.sizeDelta.x, height);
    }

    public void SubmitNewTime() {
        if (timeMs <= 0) {
            return;
        }
        
        string username = usernameInput.text;
        string message = messageInput.text;

        usernameInput.text = "";
        messageInput.text = "";

        int stageId = MapManager.i.GetSelectedMap().GetId();
        
        Record record = new Record(
            stageId,
            username, 
            message, 
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 
            timeMs);
        
        timeMs = -1;
        
        DisableInput();
        
        RecordUtil.records.Add(record);
        RecordUtil.Write();
        Refresh();
    }

    void EnableInput() {
        usernameInput.gameObject.SetActive(true);
        messageInput.gameObject.SetActive(true);
        submitButton.SetActive(true);
    }
    
    void DisableInput() {
        usernameInput.gameObject.SetActive(false);
        messageInput.gameObject.SetActive(false);
        submitButton.SetActive(false);
    }
    
    public void ClockTime() {
        timeMs = Timer.Instance.GetMilliseconds();
        timeText.text = TimeEntry.FormatMs(timeMs);
        EnableInput();
    }
}