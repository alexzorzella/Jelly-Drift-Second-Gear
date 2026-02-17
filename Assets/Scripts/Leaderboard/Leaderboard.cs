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
        
        foreach (var record in RecordUtil.records) {
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
            1000);
        
        RecordUtil.records.Add(record);
        
        RecordUtil.Write();

        Refresh();
    }
}