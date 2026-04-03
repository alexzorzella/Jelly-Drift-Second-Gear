using System.Collections.Generic;
using UnityEngine;

public class MainMenuNewsScroll : MonoBehaviour {
    void Start() {
        RecordUtil.Read();
        
        List<Record> records = new();

        records.AddRange(RecordUtil.records);

        records.Sort((x, y) => x.GetTimeMs().CompareTo(y.GetTimeMs()));
        records.Sort((x, y) => x.GetStageId().CompareTo(y.GetStageId()));

        List<int> stageIds = new();

        string finalMessage = "Our reigning champions: ";

        foreach (var record in records) {
            if (stageIds.Contains(record.GetStageId())) {
                continue;
            }

            string date = record.GetFormattedDate();
            string username = record.GetUsername();
            string time = TimeEntry.FormatMs(record.GetTimeMs());
            string message = record.GetMessage();

            string map = "Unknown";
            
            MapManager.MapData mapData = MapManager.GetStageById(record.GetStageId());

            if (mapData != null) {
                map = mapData.GetName();
            }

            string car = "Unknown";
            CarData carData = CarCatalogue.GetCarById(record.GetCarId());

            if (carData != null) {
                car = carData.GetCarName();
            }
            
            finalMessage += $"On {date}, {username} achieved a time of {time} on {map} using a {car}. \"{message}\" • ";
            
            stageIds.Add(record.GetStageId());
        }
        
        GetComponent<InfiniteScroll>().SetText(finalMessage);
    }
}