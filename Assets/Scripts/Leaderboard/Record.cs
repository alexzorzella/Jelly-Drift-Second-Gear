using System;

[Serializable]
public class Record {
    public Record(
        int stageId,
        int carId,
        string username,
        string message,
        long date,
        int time,
        string phoneNumber,
        int carVisualsId,
        string gameVersion) {
        this.stageId = stageId;
        this.carId = stageId;
        this.username = username;
        this.message = message;
        this.date = date;
        this.time = time;
        this.phoneNumber = phoneNumber;
        this.carVisualsId = carVisualsId;
        this.gameVersion = gameVersion;
    }

    readonly int stageId;
    readonly int carId;
    readonly string username;
    readonly string message;
    readonly long date;
    readonly int time;
    readonly string phoneNumber;
    readonly int carVisualsId;
    readonly string gameVersion;

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

    public string GetPhoneNumber() {
        return phoneNumber;
    }

    public int GetCarVisualsId() {
        return carVisualsId;
    }

    public string GameVersion() {
        return gameVersion;
    }

    public string GetFormattedDate() {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(GetDate());
        string result = dateTimeOffset.ToLocalTime().DateTime.ToShortDateString();
        return result;
    }
    
    public int GetTimeMs() {
        return time;
    }

    public override string ToString() {
        return $"Username: '{username}', " +
               $"Message: '{message}', " +
               $"Date: {GetFormattedDate()}, " +
               $"Time: {TimeEntry.FormatMs(time)}, " +
               $"Stage: {MapManager.GetStageById(stageId).GetName()}, " +
               $"Car: {CarCatalogue.GetCarById(carId).GetCarName()} (visual {carVisualsId}), " +
               $"Phone: {phoneNumber}, " +
               $"Game Version: {gameVersion}";
    }
}