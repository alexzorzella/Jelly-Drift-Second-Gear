using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

public static class RecordUtil {
    static readonly byte[] Key = new byte[32] {
        0x55, 0xFF, 0xD7, 0xB7, 0x0C, 0x7D, 0x11, 
        0x3E, 0x3D, 0xAB, 0xDC, 0x6A, 0x75, 0x02, 
        0xDB, 0x8B, 0xCD, 0xA1, 0x59, 0x98, 0x48, 
        0x47, 0xE8, 0xCD, 0x60, 0xFF, 0x77, 0x66, 
        0x0F, 0x09, 0xDD, 0x34
    };
    
    public static byte[] SerializeRecords(Record record) {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8);
        
        bw.Write(record.GetUsername().PadRight(16, '\0').ToCharArray());
        bw.Write(record.GetMessage().PadRight(255, '\0').ToCharArray());
        bw.Write(record.GetDate());
        bw.Write(record.GetTime());

        return ms.ToArray();
    }

    public static List<Record> DeserializeScores(byte[] rawData) {
        string json = System.Text.Encoding.UTF8.GetString(rawData);
        RecordCollection recordCollection = JsonUtility.FromJson<RecordCollection>(json);
        return recordCollection.GetRecords();
    }
}