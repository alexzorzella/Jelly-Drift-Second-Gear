using System.Collections.Generic;

public class HcRecordUtil : HCommand {
    public List<string> AutocompleteOptions() {
        return new List<string>() {
            "list",
            "delete",
            "write"
        };
    }

    public string CommandFunction(params string[] parameters) {
        if (parameters.Length > 2 && parameters[1] == "write") {
            RecordUtil.Write();
            return $"Wrote {RecordUtil.records.Count} record(s) to {RecordUtil.path}";
        }
        
        RecordUtil.Read();

        List<Record> records = RecordUtil.records;

        records.Sort((x, y) => x.GetTimeMs().CompareTo(y.GetTimeMs()));
        records.Sort((x, y) => x.GetStageId().CompareTo(y.GetStageId()));

        if (parameters.Length < 2) {
            int index = 0;
            foreach(var record in records) {
                JConsole.i.WriteLine($"[{index}] {record}");
                index++;
            }
        } else if(parameters.Length > 1){
            if (parameters.Length > 2 && parameters[1] == "delete") {
                int recordToDeleteIndex = -1;
    
                int.TryParse(parameters[2], out recordToDeleteIndex);
    
                if (recordToDeleteIndex < 0 || recordToDeleteIndex >= records.Count) {
                    return "Invalid record index";
                }
                
                Record removeRecord = RecordUtil.records[recordToDeleteIndex];
                RecordUtil.records.Remove(removeRecord);
    
                return $"Removed the following record: {removeRecord}. You must write back manually.";
            }

            if (parameters[1] == "list") {
                int index = 0;
                foreach (var record in records) {
                    JConsole.i.WriteLine($"[{index}] {record}");
                    index++;
                }
            }
        }
        
        return "";
    }

    public string CommandHelp() {
        return "'list' to list records, 'delete (int index)' to delete a record, 'write' to write the current list of records to the disk";
    }

    public string Keyword() {
        return "recordUtil";
    }
}