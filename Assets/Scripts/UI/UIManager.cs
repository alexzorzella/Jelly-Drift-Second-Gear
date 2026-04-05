using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    public TextMeshProUGUI splitText;

    public RectTransform spedometerIndicator;
    public TextMeshProUGUI spedometerText;

    Car car;
    
    void Awake() {
        Instance = this;

        splitText.text = "";
    }

    public void SetCar(Car car) {
        this.car = car;
    }
    
    void Update() {
        if (car != null) {
            UpdateSpedometer(car.GetDisplaySpeedValue(), car.GetGear());
        }
    }

    const float maxSpeed = 210;

    float targetSpedometerRotation = 0;
    
    void UpdateSpedometer(float currentSpeed, int currentGear) {
        spedometerText.text = $"{currentSpeed.ToString("0")} km/h";
        float speedPercentage = (currentSpeed / maxSpeed);
        float rotation = speedPercentage * -360F;
        targetSpedometerRotation = rotation;
        spedometerIndicator.localEulerAngles = 
            new Vector3(0, 0, 
                Mathf.LerpAngle(
                    spedometerIndicator.localEulerAngles.z,
                    targetSpedometerRotation, 
                    Time.deltaTime * 15F * Mathf.Clamp(speedPercentage + 0.2F, 0, 1)));
    }

    public void DisplaySplit() {
        // splitText.text = Timer.GetFormattedTime(Timer.Instance.GetTimer());
        
        LeanTween.cancel(splitText.gameObject);
        
        Color color = splitText.color;

        splitText.transform.localScale = Vector2.one;
        // splitText.color = new Color(color.r, color.g, color.b, 0);
        
        LeanTween.value(splitText.gameObject, splitText.transform.localScale.x, 1.45F, 0.05F).
            setEase(LeanTweenType.easeOutExpo).
            setOnUpdate((xScale) => { splitText.transform.localScale = new Vector2(xScale, splitText.transform.localScale.y); }).
            setEase(LeanTweenType.easeOutBounce).setOnComplete(() => {
                LeanTween.value(splitText.gameObject, splitText.transform.localScale.x, 1, 0.45F).setOnUpdate((xScale) => { splitText.transform.localScale = new Vector2(xScale, splitText.transform.localScale.y); }).
                setEase(LeanTweenType.easeOutBounce);
            });
        
        LeanTween.value(splitText.gameObject, splitText.transform.localScale.y, 1.65F, 0.05F).
            setEase(LeanTweenType.easeOutExpo).
            setOnUpdate((yScale) => { splitText.transform.localScale = new Vector2(splitText.transform.localScale.x, yScale); }).
            setEase(LeanTweenType.easeOutBounce).setOnComplete(() => {
                LeanTween.value(splitText.gameObject, splitText.transform.localScale.y, 1, 0.4F).setOnUpdate((yScale) => { splitText.transform.localScale = new Vector2(splitText.transform.localScale.x, yScale); }).
                setEase(LeanTweenType.easeOutBounce);
            });
        
        // LeanTween.value(splitText.gameObject, 0, 1, 0.15F).setOnUpdate((alpha) => {
        //     splitText.color = new Color(color.r, color.g, color.b, alpha);
        // });
        
        // LeanTween.delayedCall(splitText.gameObject, 4F, () => {
        //     LeanTween.value(splitText.gameObject, splitText.transform.localScale.x, 0.75F, 3F).
        //         setOnUpdate((scale) => { splitText.transform.localScale = new Vector2(scale, scale); }).
        //         setEase(LeanTweenType.easeOutExpo);
        //     LeanTween.value(splitText.gameObject, 1, 0, 0.5F).setOnUpdate((alpha) => {
        //         splitText.color = new Color(color.r, color.g, color.b, alpha);
        //     }).setEase(LeanTweenType.easeOutExpo);
        // });
    }
}