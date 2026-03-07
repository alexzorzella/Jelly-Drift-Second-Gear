using UnityEngine;

public class StartLight : MonoBehaviour, StartListener {
    Material[] colors;
    MultiAudioSource beepSource;

    MeshRenderer rend;

    void Start() {
        rend = GetComponent<MeshRenderer>();
        colors = rend.materials;
        SetColor(-1);

        beepSource = MultiAudioSource.FromResource(gameObject, "beep");
        
        FindFirstObjectByType<StartHandler>().RegsiterListener(this, false);
    }

    void SetColor(int c) {
        var array = new Material[colors.Length];
        for (var i = 0; i < array.Length; i++) {
            array[i] = colors[i];
        }

        for (var j = 0; j < array.Length; j++) {
            if (j == c + 1) {
                array[j] = colors[j];
            }
            else {
                array[j] = colors[0];
            }
        }

        rend.materials = array;
    }

    public void NotifyCountdownUpdated(int countdown) {
        beepSource.SetPitch(1f - countdown * 0.5f / 3f);
        beepSource.Play();
        
        SetColor(2 - countdown);
    }

    public void NotifyStartRace() {
        
    }
}