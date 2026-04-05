using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SettingsUi : MonoBehaviour {
    public AudioMixer sfx;
    public AudioMixer music;

    public Slider sfxSlider;
    public Slider musicSlider;

    public Image sfxMuteImage;
    public Image musicMuteImage;

    public TextMeshProUGUI motionBlurText;
    public TextMeshProUGUI depthOfFieldText;
    public TextMeshProUGUI graphicsText;
    public TextMeshProUGUI qualityText;
    public TextMeshProUGUI cameraModeText;
    public TextMeshProUGUI cameraShakeText;

    public GameObject mainMenu;
    
    void Start() {
        sfxSlider.maxValue = 20F;
        sfxSlider.minValue = -80F;

        musicSlider.maxValue = 20F;
        musicSlider.minValue = -80F;

        sfx.GetFloat("Volume", out var sfxVolume);
        GameStats.i.SetSfxVol(sfxVolume);
        music.GetFloat("Volume", out var musicVolume);
        GameStats.i.SetMusicVol(musicVolume);
        
        ReflectSfxVolume();
        ReflectMusicVolume();
        
        motionBlurText.text = BoolIntToOnOffFormat(SaveState.i.motionBlur);
        depthOfFieldText.text = BoolIntToOnOffFormat(SaveState.i.depthOfField);
        graphicsText.text = BoolIntToQualityFormat(SaveState.i.graphics);
        qualityText.text = BoolIntToQualityFormat(SaveState.i.quality);
        cameraModeText.text = BoolIntToCameraMode(SaveState.i.cameraMode);
        cameraShakeText.text = BoolIntToOnOffFormat(SaveState.i.cameraShake);
    }

    void ReflectSfxVolume(bool updateSlider = true) {
        float sfxVolume = GameStats.i._SfxVol();
        sfx.SetFloat("Volume", sfxVolume);

        if (updateSlider) {
            sfxSlider.value = sfxVolume;
        }
        
        sfxMuteImage.sprite = ResourceLoader.LoadSprite(GameStats.i.SfxMuted() ? "volume_off" : "volume_on");
    }
    
    void ReflectMusicVolume(bool updateSlider = true) {
        float musicVolume = GameStats.i._MusicVol();
        music.SetFloat("Volume", musicVolume);

        if (updateSlider) {
            musicSlider.value = musicVolume;
        }

        musicMuteImage.sprite = ResourceLoader.LoadSprite(GameStats.i.MusicMuted() ? "volume_off" : "volume_on");
    }
    
    public void _SetSfxVolume(float volume) {
        GameStats.i.SetSfxVol(volume);
        GameStats.i.UnmuteSfx();
        ReflectSfxVolume();
    }

    public void _ToggleSfxMute() {
        GameStats.i.ToggleSfxMute();
        ReflectSfxVolume(false);
    }
    
    public void _SetMusicVolume(float volume) {
        GameStats.i.SetMusicVol(volume);
        GameStats.i.UnmuteMusic();
        ReflectMusicVolume();
    }
    
    public void _ToggleMusicMute() {
        GameStats.i.ToggleMusicMute();
        ReflectMusicVolume(false);
    }

    public void _SetMotionBlur(int n) {
        int newValue = IncrementWithOverflow.Run(SaveManager.i.state.motionBlur, 2, n);
        
        SaveManager.i.state.motionBlur = newValue;
        SaveManager.i.Save();
        SaveState.i.motionBlur = newValue;
        motionBlurText.text = BoolIntToOnOffFormat(SaveState.i.motionBlur);
    }

    public void _SetDoF(int n) {
        int newValue = IncrementWithOverflow.Run(SaveManager.i.state.depthOfField, 2, n);
        
        SaveManager.i.state.depthOfField = newValue;
        SaveManager.i.Save();
        SaveState.i.depthOfField = newValue;
        depthOfFieldText.text = BoolIntToOnOffFormat(SaveState.i.depthOfField);
    }

    public void _SetGraphics(int n) {
        int newValue = IncrementWithOverflow.Run(SaveManager.i.state.graphics, 2, n);
        
        SaveManager.i.state.graphics = newValue;
        SaveManager.i.Save();
        SaveState.i.graphics = newValue;
        graphicsText.text = BoolIntToQualityFormat(SaveState.i.graphics);
    }

    public void _SetQuality(int n) {
        int newValue = IncrementWithOverflow.Run(SaveManager.i.state.quality, 2, n);
        
        SaveManager.i.state.quality = newValue;
        SaveManager.i.Save();
        SaveState.i.quality = newValue;
        QualitySettings.SetQualityLevel(newValue);
        if (CameraCulling.Instance) {
            CameraCulling.Instance.UpdateCulling();
        }
        qualityText.text = BoolIntToQualityFormat(SaveState.i.quality);
    }

    public void _SetCamMode(int n) {
        int newValue = IncrementWithOverflow.Run(SaveManager.i.state.cameraMode, 2, n);
        
        SaveManager.i.state.cameraMode = newValue;
        SaveManager.i.Save();
        SaveState.i.cameraMode = newValue;
        cameraModeText.text = BoolIntToCameraMode(SaveState.i.cameraMode);
    }

    public void _SetCamShake(int n) {
        int newValue = IncrementWithOverflow.Run(SaveManager.i.state.cameraShake, 2, n);
        
        SaveManager.i.state.cameraShake = newValue;
        SaveManager.i.Save();
        SaveState.i.cameraShake = newValue;
        cameraShakeText.text = BoolIntToOnOffFormat(SaveState.i.cameraShake);
    }

    public void _CloseMenu() {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    
    public void _SetResetSave() {
        SaveManager.i.NewSave();
        SaveManager.i.Save();
    }

    string BoolIntToOnOffFormat(int value) {
        return value == 1 ? "On" : "Off";
    }
    
    string BoolIntToCameraMode(int value) {
        return value == 0 ? "Static" : "Dynamic";
    }
    
    string BoolIntToQualityFormat(int value) {
        return value == 0 ? "Low" : "Normal";
    }
}