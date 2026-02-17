using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProfanityFilter;
using UnityEngine.InputSystem;

public class Leaderboard : MonoBehaviour {
    public VerticalLayoutGroup verticalLayoutGroup;
    RectTransform entryParent;

    public TextMeshProUGUI timeText;
    
    public TMP_InputField usernameInput;
    public TMP_InputField messageInput;
    public TextMeshProUGUI charCounterText;
    public GameObject submitButton;
    
    readonly List<GameObject> timeEntryPool = new();

    int timeMs = -1;

    const int usernameCharLimit = 16;
    const int messageCharLimit = 180;
    
    void Awake() {
        entryParent = verticalLayoutGroup.GetComponent<RectTransform>();
        EnableInput();
        RecordUtil.Read();
        Refresh();
        UpdateCharCounterText("");
    }

    void Update() {
        if (Keyboard.current.enterKey.wasPressedThisFrame) {
            SubmitNewTime();
        } else if (Keyboard.current.tabKey.wasPressedThisFrame) {
            if (usernameInput.isFocused) {
                messageInput.Select();
            } else if (messageInput.isFocused) {
                usernameInput.Select();
            } else {
                usernameInput.Select();
            }
        }
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

        List<string> displayedUsers = new();
        
        int count = 0;
        foreach (Record record in records) {
            if (displayedUsers.Contains(record.GetUsername())) {
                continue;
            }
            
            GameObject timeEntry = ResourceLoader.InstantiateObject("TimeEntry");
            timeEntry.GetComponent<TimeEntry>().Initialize(record);
            timeEntry.transform.SetParent(entryParent);
            timeEntry.transform.localScale = Vector2.one;
            
            height += timeEntry.GetComponent<RectTransform>().rect.height;
            height += verticalLayoutGroup.spacing;
            
            timeEntryPool.Add(timeEntry);
            displayedUsers.Add(record.GetUsername());
            
            count++;

            if (count >= 5) {
                break;
            }
        }
        
        entryParent.sizeDelta = new Vector2(entryParent.sizeDelta.x, height);
    }

    public void SubmitNewTime() {
        if (timeMs <= 0) {
            return;
        }
        
        string username = usernameInput.text;
        string message = messageInput.text;

        if (username == "" || message == "") {
            return;
        }
            
        if (username.Length > 16) {
            username = username.Substring(0, usernameCharLimit);
        }

        if (message.Length > 255) {
            message = message.Substring(0, messageCharLimit);
        }
        
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

    public void ConcatName(string newName) {
        if (newName.Length > usernameCharLimit) {
            newName = newName.Substring(0, usernameCharLimit);
            usernameInput.text = newName;
        }
    }
    
    public void ConcatMessage(string newMessage) {
        if (newMessage.Length > messageCharLimit) {
            newMessage = newMessage.Substring(0, messageCharLimit);
            messageInput.text = newMessage;
        }
    }

    public void UpdateCharCounterText(string message) {
        int charCount = message.Length;
        charCounterText.text = $"{charCount}/{messageCharLimit}";

        if (charCount > messageCharLimit) {
            charCounterText.color = Color.red;
        } else {
            charCounterText.color = Color.grey;
        }
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