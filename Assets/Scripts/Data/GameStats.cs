public class GameStats {
    static GameStats _i = null;

    public static GameStats i {
        get {
            if (_i == null) {
                _i = new();
            }

            return _i;
        }
    }

    float sfxVolume = 0F;
    bool sfxMuted = false;
    
    float musicVolume = 0F;
    bool musicMuted = false;

    const float quiet = -80F;
    
    public float _SfxVol() { return sfxMuted ? quiet : sfxVolume; }
    public float _MusicVol() { return musicMuted ? quiet : musicVolume; }
    public void SetSfxVol(float volume) { sfxVolume = volume; }
    public void ToggleSfxMute() { sfxMuted = !sfxMuted; }
    public void SetMusicVol(float volume) { musicVolume = volume; }
    public void ToggleMusicMute() { musicMuted = !musicMuted; }
    public bool SfxMuted() { return sfxMuted; }
    public bool MusicMuted() { return musicMuted; }
    public void UnmuteSfx() { sfxMuted = false; }
    public void UnmuteMusic() { musicMuted = false; }
}