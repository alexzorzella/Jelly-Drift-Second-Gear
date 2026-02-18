using System.Collections.Generic;
using UnityEngine;

public class MainMenuNewsScroll : MonoBehaviour {
    void Start() {
        RecordUtil.Read();
        
        List<Record> records = new();

        records.AddRange(RecordUtil.records);

        records.Sort((x, y) => x.GetTimeMs().CompareTo(y.GetTimeMs()));

        List<int> stageIds = new();

        string finalMessage = "Our reigning champions: ";
        
        foreach (var record in records) {
            if (stageIds.Contains(record.GetStageId())) {
                continue;
            }

            finalMessage += $"{record.GetUsername()} ({TimeEntry.FormatMs(record.GetTimeMs())}) '{record.GetMessage()}' / ";
        }
        
        GetComponent<InfiniteScroll>().SetText(finalMessage);
    }
}