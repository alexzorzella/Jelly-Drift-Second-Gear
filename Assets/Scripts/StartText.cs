using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartText : MonoBehaviour, StartListener {
    CanvasGroup canvasGroup;

    public TextMeshProUGUI cutoutText;
    public TextMeshProUGUI overlayText;
    public Image backgroundImage;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        FindFirstObjectByType<StartHandler>().RegsiterListener(this);
    }

    public void NotifyCountdownUpdated(int countdown) {
        if (countdown == 2) {
            canvasGroup.alpha = 1;
            
            overlayText.alpha = 0;
            cutoutText.alpha = 1;
            Color color = backgroundImage.color;
            backgroundImage.color = new Color(color.r, color.g, color.b, 1);
            
            cutoutText.transform.localScale = Vector3.one;
        } else if (countdown == 1) {
            cutoutText.alpha = 0;
            overlayText.alpha = 1;
            Color color = backgroundImage.color;
            backgroundImage.color = new Color(color.r, color.g, color.b, 0);
            
            // LeanTween.value(gameObject, 1, 0, 1F).setOnUpdate((value) => {
            //     overlayText.alpha = value;
            // });
        } else if (countdown <= 0) {
            // canvasGroup.alpha = 0;

            LeanTween.value(gameObject, 1, 10, 10F).setOnUpdate((value) => {
                cutoutText.transform.localScale = Vector3.one * value;
            }).setEase(LeanTweenType.easeOutExpo);
            
            // LeanTween.value(gameObject, 1, 0, 15F).setOnUpdate((value) => {
            //      canvasGroup.alpha = value;
            // }).setEase(LeanTweenType.easeOutExpo);
        }
    }

    public void NotifyStartRace() {
        
    }
}