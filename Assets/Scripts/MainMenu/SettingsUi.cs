using UnityEngine;

public class SettingsUi : MonoBehaviour {
    public SettingCycle motionBlur;
    public SettingCycle graphics;
    public SettingCycle quality;
    public SettingCycle camMode;
    public SettingCycle camShake;
    public SettingCycle dof;
    public SliderSettingCycle volume;
    public SliderSettingCycle music;
    Color deselected = new(0f, 0f, 0f, 0.3f);
    Color selected = Color.white;

    void Start() {
        LoadAllSettings();
    }

    void LoadAllSettings() {
        LoadSetting(motionBlur, SaveState.i.motionBlur);
        LoadSetting(dof, SaveState.i.dof);
        LoadSetting(graphics, SaveState.i.graphics);
        LoadSetting(quality, SaveState.i.quality);
        LoadSetting(camMode, SaveState.i.cameraMode);
        LoadSetting(camShake, SaveState.i.cameraShake);
        LoadSettingSlider(volume, SaveState.i.volume);
        LoadSettingSlider(music, SaveState.i.musicVolume);
    }

    void LoadSetting(SettingCycle s, int n) {
        s.selected = n;
        s.UpdateOptions();
    }

    void LoadSettingSlider(SliderSettingCycle s, int f) {
        s.selected = f;
        s.UpdateOptions();
    }

    public void UpdateSettings() {
        MotionBlur(motionBlur.selected);
        DoF(dof.selected);
        Graphics(graphics.selected);
        Quality(quality.selected);
        CamMode(camMode.selected);
        CamShake(camShake.selected);
        Volume();
        Music();
    }

    public void MotionBlur(int n) {
        SaveManager.i.state.motionBlur = n;
        SaveManager.i.Save();
        SaveState.i.motionBlur = n;
        // PPController.Instance.LoadSettings();
    }

    public void DoF(int n) {
        SaveManager.i.state.dof = n;
        SaveManager.i.Save();
        SaveState.i.dof = n;
        // PPController.Instance.LoadSettings();
    }

    public void Graphics(int n) {
        SaveManager.i.state.graphics = n;
        SaveManager.i.Save();
        SaveState.i.graphics = n;
        // PPController.Instance.LoadSettings();
    }

    public void Quality(int n) {
        SaveManager.i.state.quality = n;
        SaveManager.i.Save();
        SaveState.i.quality = n;
        QualitySettings.SetQualityLevel(n + Mathf.Clamp(2 * n - 1, 0, 10));
        if (CameraCulling.Instance) {
            CameraCulling.Instance.UpdateCulling();
        }
    }

    public void CamMode(int n) {
        SaveManager.i.state.cameraMode = n;
        SaveManager.i.Save();
        SaveState.i.cameraMode = n;
    }

    public void CamShake(int n) {
        SaveManager.i.state.cameraShake = n;
        SaveManager.i.Save();
        SaveState.i.cameraShake = n;
    }

    public void Volume() {
        SaveManager.i.state.volume = volume.selected;
        SaveManager.i.Save();
        SaveState.i.volume = volume.selected;
        AudioListener.volume = volume.selected / 10f;
    }

    public void Music() {
        SaveManager.i.state.music = music.selected;
        SaveManager.i.Save();
        SaveState.i.musicVolume = music.selected;
        MusicController.i.UpdateVolume(music.selected);
    }

    public void ResetSave() {
        SaveManager.i.NewSave();
        SaveManager.i.Save();
    }
}