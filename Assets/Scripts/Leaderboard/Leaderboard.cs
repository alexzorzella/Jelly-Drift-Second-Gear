using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Leaderboard : MonoBehaviour {
    public VerticalLayoutGroup verticalLayoutGroup;
    RectTransform entryParent;

    public TextMeshProUGUI timeText;
    
    public TMP_InputField usernameInput;
    public TMP_InputField messageInput;
    public TextMeshProUGUI charCounterText;
    public GameObject submitButton;
    
    readonly List<GameObject> timeEntryPool = new();

    bool awaitingSubmission = false;
    int timeMs = -1;

    const int usernameCharLimit = 16;
    const int messageCharLimit = 180;

    public TextMeshProUGUI systemMessageText;

    public GameObject championMessageObject;
    public TextMeshProUGUI championMessageText;
    public RectTransform championMessageBgRect;
    
    void Awake() {
        entryParent = verticalLayoutGroup.GetComponent<RectTransform>();
        DisableInput();
        RecordUtil.Read();
        Refresh();
        UpdateCharCounterText("");
    }

    void Update() {
        if (Keyboard.current.enterKey.wasPressedThisFrame) {
            // SubmitNewTime();
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

        championMessageObject.SetActive(records.Count > 0);
        
        List<string> displayedUsers = new();
        
        int count = 0;
        foreach (Record record in records) {
            if (displayedUsers.Contains(record.GetUsername())) {
                continue;
            }

            if (count == 0) {
                championMessageText.text = record.GetMessage();
                championMessageText.ForceMeshUpdate();
                championMessageBgRect.sizeDelta = championMessageText.GetRenderedValues() + Vector2.one * 25F;

                LeanTween.cancel(championMessageObject);
                championMessageObject.transform.localScale = Vector2.zero;
                
                LeanTween.value(championMessageObject, 0, 1, 0.25F).
                    setOnUpdate((scale) => { championMessageObject.transform.localScale = new Vector2(scale, scale); }).
                    setEase(LeanTweenType.easeOutExpo);
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
            DisplaySystemMessage("Your time is impossible.");
            return;
        }
        
        string username = usernameInput.text;
        string message = messageInput.text;

        if (username == "") {
            DisplaySystemMessage("You haven't entered a display name.");
            return;
        }
        
        if(message == "") {
            DisplaySystemMessage("You haven't entered a message.");
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
    
    void PromptInput() {
        awaitingSubmission = true;
        usernameInput.gameObject.SetActive(true);
        messageInput.gameObject.SetActive(true);
        submitButton.SetActive(true);
    }
    
    void DisableInput() {
        awaitingSubmission = false;
        usernameInput.gameObject.SetActive(false);
        messageInput.gameObject.SetActive(false);
        submitButton.SetActive(false);
    }
    
    public void ClockTime() {
        timeMs = Timer.Instance.GetMilliseconds();
        timeText.text = TimeEntry.FormatMs(timeMs);
        PromptInput();
    }

    void DisplaySystemMessage(string message) {
        systemMessageText.text = message;
        
        LeanTween.cancel(systemMessageText.gameObject);
        
        Color color = systemMessageText.color;
        
        systemMessageText.color = new Color(color.r, color.g, color.b, 0);
        
        LeanTween.value(systemMessageText.gameObject, 0, 1, 0.2F).setOnUpdate((alpha) => {
            systemMessageText.color = new Color(color.r, color.g, color.b, alpha);
        });
        
        LeanTween.delayedCall(systemMessageText.gameObject, 4F, () => {
             LeanTween.value(systemMessageText.gameObject, 1, 0, 0.75F).setOnUpdate((alpha) => {
                 systemMessageText.color = new Color(color.r, color.g, color.b, alpha);
             }).setEase(LeanTweenType.easeOutExpo);
        });
    }

    public bool AwaitingSubmission() {
        if (awaitingSubmission) {
            DisplaySystemMessage("Submit a display name and message!");
        }
        
        return awaitingSubmission;
    }
}