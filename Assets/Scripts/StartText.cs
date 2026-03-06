using TMPro;
using UnityEngine;

public class StartText : MonoBehaviour, StartListener {
    CanvasGroup canvasGroup;

    public TextMeshProUGUI cutoutText;
    public TextMeshProUGUI overlayText;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        FindFirstObjectByType<StartHandler>().RegsiterListener(this);
    }

    public void NotifyCountdownUpdated(int countdown) {
        if (countdown == 2) {
            canvasGroup.alpha = 1;
        } else if (countdown == 1) {
            overlayText.alpha = 1;
            cutoutText.transform.localScale = Vector3.one;
            
            LeanTween.value(gameObject, 1, 0, 1F).setOnUpdate((value) => {
                overlayText.alpha = value;
            });
        } else if (countdown <= 0) {
            canvasGroup.alpha = 0;

            LeanTween.value(gameObject, 1, 5, 1F).setOnUpdate((value) => {
                cutoutText.transform.localScale = Vector3.one * value;
            });
        }
    }

    public void NotifyStartRace() {
        
    }
}