using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour {
    public PlayerSave state;
    
    static SaveManager _i;
	
    public static SaveManager i {
        get {
            if (_i == null) {
                SaveManager x = Resources.Load<SaveManager>("SaveManager");

                _i = Instantiate(x);
            }
            return _i;
        }
    }
    
    void Awake() {
        Load();
    }

    public void Save() {
        PlayerPrefs.SetString("save", Serialize(state));
    }

    public void Load() {
        if (PlayerPrefs.HasKey("save")) {
            state = Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
            return;
        }

        NewSave();
    }

    public void NewSave() {
        state = new PlayerSave();
        Save();
        print("Creating new save file");
    }

    public string Serialize<T>(T toSerialize) {
        var xmlSerializer = new XmlSerializer(typeof(T));
        var stringWriter = new StringWriter();
        xmlSerializer.Serialize(stringWriter, toSerialize);
        return stringWriter.ToString();
    }

    public T Deserialize<T>(string toDeserialize) {
        var xmlSerializer = new XmlSerializer(typeof(T));
        var textReader = new StringReader(toDeserialize);
        return (T)xmlSerializer.Deserialize(textReader);
    }
}