using UnityEngine;

public class MusicController : MonoBehaviour {
    static MusicController _i;
    MultiAudioSource music;
    
    void Start() {
        DontDestroyOnLoad(gameObject);

        music = MultiAudioSource.FromResource(gameObject, "synthwave", loop: true);
        
        UpdateVolume(SaveState.i.musicVolume);
        
        music.PlayRoundRobin();
    }
	
    public static MusicController i {
        get {
            if (_i == null) {
                MusicController x = Resources.Load<MusicController>("MusicController");

                _i = Instantiate(x);
            }
            return _i;
        }
    }

    public void UpdateVolume(float newVolume) {
        music.SetVolume(newVolume / 10f);
    }
}