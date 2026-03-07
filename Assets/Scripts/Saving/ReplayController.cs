using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ReplayController : MonoBehaviour {
    public static ReplayController Instance;
    float endTime;
    string filePath;

    List<ReplayFrame> replay;
    float startTime;
    public int hz { get; set; } = 30;
    public Car car { get; set; }

    void Awake() {
        Instance = this;
    }

    void Start() {
        var path = Application.persistentDataPath + "/replays";
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        filePath = string.Concat(Application.persistentDataPath, "/replays/", GameState.i.map, ".txt");
        replay = new List<ReplayFrame>();
        startTime = Time.time;
    }

    void FixedUpdate() {
        if (!GameController.Instance || !car) {
            return;
        }

        replay.Add(new ReplayFrame(car.rb.position, car.rb.rotation.eulerAngles, car.steerAngle, Time.time));
    }

    public void Save() {
        print("saving");
        var streamWriter = StreamWriter.Null;
        try {
            streamWriter = new StreamWriter(filePath, false);
            streamWriter.WriteLine(GameState.i.car + ", " + GameState.i.skin);
            foreach (var replayFrame in replay) {
                streamWriter.WriteLine(string.Concat(replayFrame.pos, ", ", replayFrame.rot, ", ",
                    replayFrame.steerAngle, ",", replayFrame.time));
            }
        }
        catch (Exception ex) {
            Debug.Log(ex.Message);
        }
        finally {
            streamWriter.Close();
        }
    }

    [Serializable]
    public class Replay {
        readonly string player;
        readonly string message;
        readonly DateTime dateTime;
        readonly ReplayFrame[] frames;
        
        static string Path(string specificPath) {
            var path = Application.persistentDataPath + $"/replays/{specificPath}.txt";
            return path;
        }
        
        public void Save() {
            BinaryFormatter formatter = new();

            FileStream stream = new FileStream(Path(GetDateAndTimeFormatted()), FileMode.Create);

            formatter.Serialize(stream, this);

            stream.Close();
        }
        
        public static void DeleteSave(string name) {
            if (File.Exists(Path(name)))
                File.Delete(Path(name));
            else
                Debug.LogError($"Tried to delete file in {Path(name)}.");
        }

        public static bool FileExists(string specificFile) {
            return File.Exists(Path(specificFile));
        }

        public static Replay LoadPlayer(string path) {
            if (File.Exists(Path(path))) {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(Path(path), FileMode.Open);

                var data = formatter.Deserialize(stream) as Replay;
                stream.Close();

                return data;
            }

            Debug.LogWarning($"Save file not found in {Path(path)}.");
            return null;
        }
        
        public static string GetDateAndTimeFormatted() {
            DateTime today = DateTime.Now;
            string result = $"{today.Year}-{today.Month}-{today.Day}_{today.Hour}-{today.Minute}-{today.Second}";

            return result;
        }
    }
    
    [Serializable]
    public class ReplayFrame {
        public Vector3 pos;
        public Vector3 rot;
        public float steerAngle;
        public float time;

        public ReplayFrame(Vector3 pos, Vector3 rot, float steerAngle, float time) {
            this.pos = pos;
            this.rot = rot;
            this.steerAngle = steerAngle;
            this.time = time;
        }
    }
}