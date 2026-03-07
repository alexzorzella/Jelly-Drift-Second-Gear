using UnityEngine;

public class SaveState : MonoBehaviour {
    public int quality { get; set; }
    public int dof { get; set; }
    public int motionBlur { get; set; }
    public int cameraMode { get; set; }
    public int cameraShake { get; set; }
    public int muted { get; set; }
    public int volume { get; set; }
    public int musicVolume { get; set; }
    public int graphics { get; set; }
    
    static SaveState _i;
	
    public static SaveState i {
        get {
            if (_i == null) {
                _i = new SaveState();
            }
            return _i;
        }
    }

    SaveState() {
        LoadSettings();
    }

    void LoadSettings() {
        graphics = SaveManager.i.state.graphics;
        quality = SaveManager.i.state.quality;
        motionBlur = SaveManager.i.state.motionBlur;
        dof = SaveManager.i.state.dof;
        cameraMode = SaveManager.i.state.cameraMode;
        cameraShake = SaveManager.i.state.cameraShake;
        muted = SaveManager.i.state.muted;
        volume = SaveManager.i.state.volume;
        musicVolume = SaveManager.i.state.music;
    }
}