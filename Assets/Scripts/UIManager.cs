using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    public TextMeshProUGUI splitText;

    void Awake() {
        Instance = this;

        splitText.text = "";
    }

    public void DisplaySplit() {
        splitText.text = Timer.GetFormattedTime(Timer.Instance.GetTimer());
        
        LeanTween.cancel(splitText.gameObject);
        
        Color color = splitText.color;
        
        splitText.transform.localScale = Vector2.one * 1.25F + new Vector2(0, 0.75F);
        splitText.color = new Color(color.r, color.g, color.b, 0);

        LeanTween.value(splitText.gameObject, splitText.transform.localScale.x, 1, 0.5F).
            setOnUpdate((xScale) => { splitText.transform.localScale = new Vector2(xScale, splitText.transform.localScale.y); }).
            setEase(LeanTweenType.easeOutBounce);
        LeanTween.value(splitText.gameObject, splitText.transform.localScale.y, 1, 0.5F).
            setOnUpdate((yScale) => { splitText.transform.localScale = new Vector2(splitText.transform.localScale.x, yScale); }).
            setEase(LeanTweenType.easeOutBounce);
        LeanTween.value(splitText.gameObject, 0, 1, 0.15F).setOnUpdate((alpha) => {
            splitText.color = new Color(color.r, color.g, color.b, alpha);
        });
        
        LeanTween.delayedCall(splitText.gameObject, 4F, () => {
            LeanTween.value(splitText.gameObject, splitText.transform.localScale.x, 0.75F, 3F).
                setOnUpdate((scale) => { splitText.transform.localScale = new Vector2(scale, scale); }).
                setEase(LeanTweenType.easeOutExpo);
            LeanTween.value(splitText.gameObject, 1, 0, 0.5F).setOnUpdate((alpha) => {
                splitText.color = new Color(color.r, color.g, color.b, alpha);
            }).setEase(LeanTweenType.easeOutExpo);
        });
    }
}