using System;

[Serializable]
public class Record {
    public Record(int stageId, int carId, string username, string message, long date, int time) {
        this.stageId = stageId;
        this.carId = stageId;
        this.username = username;
        this.message = message;
        this.date = date;
        this.time = time;
    }

    readonly int stageId;
    readonly int carId;
    readonly string username;
    readonly string message;
    readonly long date;
    readonly int time;

    public int GetStageId() {
        return stageId;
    }

    public int GetCarId() {
        return carId;
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

    public string GetFormattedDate() {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(GetDate());
        string result = dateTimeOffset.ToLocalTime().DateTime.ToShortDateString();
        return result;
    }
    
    public int GetTimeMs() {
        return time;
    }
}