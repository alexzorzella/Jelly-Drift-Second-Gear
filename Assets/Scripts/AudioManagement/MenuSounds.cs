using UnityEngine;

public class MenuSounds : MonoBehaviour {
    public void Play() {
        SoundManager.i.PlayMenuNavigate();
    }
}