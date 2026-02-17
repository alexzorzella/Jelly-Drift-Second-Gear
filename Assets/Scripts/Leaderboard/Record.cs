using System;

[Serializable]
public class Record {
    public Record(int stageId, string username, string message, long date, int time) {
        this.stageId = stageId;
        this.username = username;
        this.message = message;
        this.date = date;
        this.time = time;
    }

    readonly int stageId;
    readonly string username;
    readonly string message;
    readonly long date;
    readonly int time;

    public int GetStageId() {
        return stageId;
    }
    
    public string GetUsername() {
        return username;
    }
    
    public string GetMessage() {
        return message;
    }
    
    public long GetDate() {
        return date;
    }
    
    public int GetTime() {
        return time;
    }
}