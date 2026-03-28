using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUi : MonoBehaviour {
    public AudioMixer sfx;
    public AudioMixer music;

    public Slider sfxSlider;
    public Slider musicSlider;

    public Image sfxMuteImage;
    public Image musicMuteImage;
    
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
    }

    void ReflectSfxVolume(bool updateVolume = true) {
        if (updateVolume) {
            float sfxVolume = GameStats.i._SfxVol();
            sfx.SetFloat("Volume", sfxVolume);
            sfxSlider.value = sfxVolume;
        }
        
        sfxMuteImage.sprite = ResourceLoader.LoadSprite(GameStats.i.SfxMuted() ? "volume_off" : "volume_on");
    }
    
    void ReflectMusicVolume(bool updateVolume = true) {
        if (updateVolume) {
            float musicVolume = GameStats.i._MusicVol();
            music.SetFloat("Volume", musicVolume);
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
        SaveManager.i.state.motionBlur = n;
        SaveManager.i.Save();
        SaveState.i.motionBlur = n;
    }

    public void _SetDoF(int n) {
        SaveManager.i.state.dof = n;
        SaveManager.i.Save();
        SaveState.i.dof = n;
    }

    public void _SetGraphics(int n) {
        SaveManager.i.state.graphics = n;
        SaveManager.i.Save();
        SaveState.i.graphics = n;
    }

    public void _SetQuality(int n) {
        SaveManager.i.state.quality = n;
        SaveManager.i.Save();
        SaveState.i.quality = n;
        QualitySettings.SetQualityLevel(n + Mathf.Clamp(2 * n - 1, 0, 10));
        if (CameraCulling.Instance) {
            CameraCulling.Instance.UpdateCulling();
        }
    }

    public void _SetCamMode(int n) {
        SaveManager.i.state.cameraMode = n;
        SaveManager.i.Save();
        SaveState.i.cameraMode = n;
    }

    public void _SetCamShake(int n) {
        SaveManager.i.state.cameraShake = n;
        SaveManager.i.Save();
        SaveState.i.cameraShake = n;
    }

    public void _SetResetSave() {
        SaveManager.i.NewSave();
        SaveManager.i.Save();
    }
}