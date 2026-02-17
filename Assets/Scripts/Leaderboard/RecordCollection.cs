using System;
using System.Collections.Generic;

[Serializable]
public class RecordCollection {
    public RecordCollection(List<Record> records) {
        this.records = records;
    }
    
    readonly List<Record> records = new();

    public List<Record> GetRecords() {
        return records;
    }
}