using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class KeygenWindow: EditorWindow {
    string keyAsString = "";
    
    void OnGUI() {
        if (keyAsString == "") {
            byte[] key = new byte[32];
        
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(key);
            
            keyAsString = string.Join(", ", key.Select(b => $"0x{b:X2}"));
        }
        
        GUILayout.TextArea($"{keyAsString}");
    }

    [MenuItem("Socrates/Keygen")]
    public static void ShowWindow() {
        GetWindow<KeygenWindow>("Hex Code Converter");
    }
}