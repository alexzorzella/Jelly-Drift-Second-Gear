using UnityEngine;

public class PlayerSave {
    readonly float x = 0.07f;
    readonly float y = 1.55f;
    public bool[] carsUnlocked = new bool[15];
    public float[] daniTimes = new float[100];
    public int lastCar;
    public int lastDifficulty;
    public int lastGhost;
    public int lastMap;
    public int[] lastSkin;
    public bool[] mapsUnlocked = new bool[15];
    public int money;
    public int[] races = new int[100];
    public bool[][] skins;
    public float[] times = new float[100];
    public int xp;

    public PlayerSave() {
        lastSkin = new int[15];
        skins = new bool[carsUnlocked.Length][];
        for (var i = 0; i < skins.Length; i++) {
            if (i == 5) {
                skins[i] = new bool[2];
            }
            else if (i > 5) {
                skins[i] = new bool[1];
            }
            else {
                skins[i] = new bool[5];
            }

            skins[i][0] = true;
        }

        if (SystemInfo.deviceType == DeviceType.Handheld) {
            graphics = 0;
            quality = 0;
            motionBlur = 0;
            dof = 0;
        }

        daniTimes[0] = 42.11238f;
        daniTimes[1] = 51.41264f;
        daniTimes[2] = 76.41264f;
        daniTimes[3] = 79.27263f;
        daniTimes[4] = 114.1815f;
        mapsUnlocked[0] = true;
        carsUnlocked[0] = true;
        for (var j = 0; j < races.Length; j++) {
            races[j] = -1;
        }
    }

    public int graphics { get; set; } = 1;
    public int quality { get; set; } = 2;
    public int motionBlur { get; set; } = 1;
    public int dof { get; set; } = 1;
    public int cameraMode { get; set; }
    public int cameraShake { get; set; } = 1;
    public int muted { get; set; }
    public int volume { get; set; } = 3;
    public int music { get; set; } = 4;

    public int GetLevel(int xp) {
        return Mathf.FloorToInt(NthRoot(xp, y) * x);
    }

    public int GetLevel() {
        return Mathf.FloorToInt(NthRoot(xp, y) * x);
    }

    public int XpForLevel(int level) {
        return (int)Mathf.Pow(level / x, y);
    }

    public float LevelProgress() {
        var num = (float)(xp - XpForLevel(GetLevel()));
        var num2 = XpForLevel(GetLevel() + 1) - XpForLevel(GetLevel());
        return num / num2;
    }

    static float NthRoot(float A, float N) {
        return Mathf.Pow(A, 1f / N);
    }

    public static int GetSkinPrice(int car, int skin) {
        if (car < 5) {
            return 500 * (skin - 2) + car * 200;
        }

        return 9999;
    }
}