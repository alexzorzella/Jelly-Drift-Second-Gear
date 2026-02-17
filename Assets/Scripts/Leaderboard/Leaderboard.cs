using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {
    public VerticalLayoutGroup verticalLayoutGroup;
    RectTransform entryParent;

    public TMP_InputField usernameInput;
    public TMP_InputField messageInput;

    readonly List<GameObject> timeEntryPool = new();

    int timeMs = -1;
    
    void Awake() {
        entryParent = verticalLayoutGroup.GetComponent<RectTransform>();

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
        
        records.Sort((x, y) => x.GetTime().CompareTo(y.GetTime()));
        
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
        
        RecordUtil.records.Add(record);
        
        RecordUtil.Write();

        Refresh();

        timeMs = -1;
    }

    public void ClockTime() {
        timeMs = Timer.Instance.GetMilliseconds();
    }
}