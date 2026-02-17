using System;

[Serializable]
public class Record {
    public Record(string username, string message, long date, int time) {
        this.username = username;
        this.message = message;
        this.date = date;
        this.time = time;
    }

    readonly string username;
    readonly string message;
    readonly long date;
    readonly int time;
    
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