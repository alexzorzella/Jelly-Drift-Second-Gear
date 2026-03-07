using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MapManager {
    static MapManager _i;

    public static MapManager i {
        get {
            if (_i == null) {
                _i = new MapManager();
            }
            return _i;
        }
    }

    int selectedMapIndex = 0;

    public void CycleSelectedMap(int by) {
        selectedMapIndex = IncrementWithOverflow.Run(selectedMapIndex, maps.Count, by);
    }
    
    public MapData GetSelectedMap() {
        return maps[selectedMapIndex];
    }
    
    readonly List<MapData> maps = new() {
        new MapData(0, "Dusty Desert", new Color(0.811F, 0.38F, 0.09F, 0.75F)),
        new MapData(1, "Sneaky Snow", new Color(0, 0.43F, 1, 0.75F)),
        new MapData(2, "Pink Plains", new Color(1, 0.16F, 0.33F, 0.75F)),
        new MapData(3, "Akina Downhill", new Color(1, 0.12F, 0, 0.75F)),
        new MapData(4, "Flapjack Raceway", new Color(1, 0.12F, 0, 0.75F))
    };

    public MapData GetMapAtIndex(int index) {
        return maps[index];
    }
    
    public int MapCount() {
        return maps.Count;
    }
    
    public int GetStars(int map, float time) {
        var result = 0;
        // if (time <= maps[map].times[2]) {
        //     result = 3;
        // }
        // else if (time <= maps[map].times[1]) {
        //     result = 2;
        // }
        // else if (time <= maps[map].times[0]) {
        //     result = 1;
        // }
        //
        // if (time <= 0f) {
        //     result = 0;
        // }
        
        return result;
    }

    public class MapData {
        int id;
        string name;
        Color themeColor;
        Sprite sprite;
        float[] times;
        
        public MapData(int id, string name, Color themeColor) {
            this.id = id;
            this.name = name;
            this.themeColor = themeColor;

            string nameFormatted = GetNameFormatted();
            string path = $"Sprites/{nameFormatted}";
            sprite = Resources.Load<Sprite>(path);

            if (sprite == null) {
                Debug.LogError($"No image called {nameFormatted} found in Resources/Sprites.");
            }
        }

        public int GetId() {
            return id;
        }
        
        public string GetName() {
            return name;
        }

        public Color GetColor() {
            return themeColor;
        }

        public Sprite GetSprite() {
            return sprite;
        }

        public string GetNameFormatted() {
            string result = name.Replace(" ", "_").ToLower();
            return result;
        }
    }
}