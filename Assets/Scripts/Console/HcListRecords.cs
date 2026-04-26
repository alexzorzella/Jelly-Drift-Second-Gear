using System.Collections.Generic;

public class HcListRecords : HCommand {
    public List<string> AutocompleteOptions() {
        return new List<string>();
    }

    public string CommandFunction(params string[] parameters) {
        RecordUtil.Read();

        List<Record> records = RecordUtil.records;
        
        records.Sort((x, y) => x.GetTimeMs().CompareTo(y.GetTimeMs()));
        records.Sort((x, y) => x.GetStageId().CompareTo(y.GetStageId()));

        int index = 0;
        foreach(var record in records) {
            JConsole.i.WriteLine($"[{index}] {record}");
            index++;
        }
        
        return "";
    }

    public string CommandHelp() {
        return "Lists the records saved on the device";
    }

    public string Keyword() {
        return "listRecords";
    }
}