using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;

public static class RecordUtil {
    static readonly byte[] Key = {
        0x55, 0xFF, 0xD7, 0xB7, 0x0C, 0x7D, 0x11, 
        0x3E, 0x3D, 0xAB, 0xDC, 0x6A, 0x75, 0x02, 
        0xDB, 0x8B, 0xCD, 0xA1, 0x59, 0x98, 0x48, 
        0x47, 0xE8, 0xCD, 0x60, 0xFF, 0x77, 0x66, 
        0x0F, 0x09, 0xDD, 0x34
    };

    static readonly string path = Application.persistentDataPath + "/leaderboard.weaver";

    public static readonly List<Record> records = new();
    
    public static void Write() {
        FileStream stream = new FileStream(path, FileMode.Create);

        foreach (var record in records) {
            byte[] encrypted = Encrypt(record);
            stream.Write(encrypted);
        }

        stream.Close();
    }

    const int recordSize = 16 + 255 + 8 + 4;
    const int chunkSize = 12 + 16 + recordSize;
    
    public static bool Read() {
        if (!File.Exists(path)) {
            return false;
        }

        byte[] fileData = File.ReadAllBytes(path);

        for (int i = 0; i + chunkSize <= fileData.Length; i += chunkSize) {
            byte[] chunk = new byte[chunkSize];
            Array.Copy(fileData, i, chunk, 0, chunkSize);

            try {
                byte[] decrypted = Decrypt(chunk);
                Record record = DeserializeRecord(decrypted);
                records.Add(record);
            }
            catch (CryptographicException) {
                
            }
        }

        return true;
    }
    
    public static byte[] SerializeRecord(Record record) {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms, Encoding.UTF8);
        
        bw.Write(record.GetUsername().PadRight(16, '\0').ToCharArray());
        bw.Write(record.GetMessage().PadRight(255, '\0').ToCharArray());
        bw.Write(record.GetDate());
        bw.Write(record.GetTime());

        return ms.ToArray();
    }
    
    static byte[] Encrypt(Record record) {
        byte[] nonce = new byte[12];

        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(nonce);
        
        byte[] plainText = SerializeRecord(record);
        
        byte[] cypherText = new byte[plainText.Length];

        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = Key;
        aes.GenerateIV();
        
        var encryptor = aes.CreateEncryptor();
        byte[] cipherText = encryptor.TransformFinalBlock(plainText, 0, plainText.Length);

        var hmac = new HMACSHA256(Key);
        byte[] tag = hmac.ComputeHash(cipherText);
        
        MemoryStream ms = new MemoryStream();
        ms.Write(aes.IV);
        ms.Write(cipherText);
        ms.Write(tag);

        return ms.ToArray();
    }
    
    const int ivLength = 16;
    const int tagLength = 32;

    static byte[] Decrypt(byte[] data) {
        byte[] iv = data[..ivLength];
        byte[] tag = data[^tagLength..];
        byte[] cipherText = data[ivLength..^tagLength];
        
        var hmac = new HMACSHA256(Key);
        byte[] computedTag = hmac.ComputeHash(cipherText);

        if (!computedTag.SequenceEqual(tag)) {
            throw new CryptographicException("Tampered record");
        }

        var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.Key = Key;
        aes.IV = iv;
        
        var decryptor = aes.CreateDecryptor();
        byte[] plainText = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
        
        return plainText;
    }

    static Record DeserializeRecord(byte[] data) {
        MemoryStream ms = new MemoryStream(data);
        BinaryReader br = new BinaryReader(ms, Encoding.UTF8);
        
        string username = new string(br.ReadChars(16)).TrimEnd('\0');
        string message = new string(br.ReadChars(255)).TrimEnd('\0');
        
        long date = br.ReadInt64();
        int time = br.ReadInt32();

        Record result = new(username, message, date, time);

        return result;
    }
}