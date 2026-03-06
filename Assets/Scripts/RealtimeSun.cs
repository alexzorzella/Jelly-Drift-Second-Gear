using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RealtimeSun : MonoBehaviour {
    const int secondsInADay = 86400;

    const float nightPostExposureFactor = -8F;
    const float lightIntensity = 5F;

    public Volume volume;
    ColorAdjustments colorAdjustments;
    
    Light lightSource;

    public List<Light> nighttimeLights;

    float rotY;
    float rotZ;
    
    const float sunlightTweenSpeed = 120F;
    const float postExposureTweenSpeed = 120F;
    const float lightsTweenSpeed = 120F;

    bool isNight = false;
    float sunAngle;
    float percentageOfDayPassed = 0;
    
    void Start() {
        lightSource = GetComponent<Light>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        rotY = transform.rotation.eulerAngles.y;
        rotZ = transform.rotation.eulerAngles.z;

        foreach (var light in nighttimeLights) {
            light.intensity = 0;
        }
    }

    void Update() {
        DateTime now = DateTime.Now;
        TimeSpan timeOfDay = now.TimeOfDay;
        int secondsPassedToday = (int)timeOfDay.TotalSeconds;

        percentageOfDayPassed = ((float)secondsPassedToday / secondsInADay) % 1;
        
        sunAngle = (360 * (percentageOfDayPassed % 1) + 270) % 360;
        
        transform.rotation = Quaternion.Euler(sunAngle, rotY, rotZ);

        bool night = sunAngle > 183 && sunAngle < 357;

        if (isNight != night) {
            isNight = night;
            
            float currentPostExposure = isNight ? nightPostExposureFactor : 0;
            currentPostExposure = Mathf.Clamp(currentPostExposure, nightPostExposureFactor, 0);

            float currentLightIntensity = !isNight ? lightIntensity : 0;
            
            LeanTween.value(gameObject, colorAdjustments.postExposure.value, currentPostExposure, sunlightTweenSpeed).setOnUpdate((value) => { colorAdjustments.postExposure.value = value; });
            LeanTween.value(gameObject, lightSource.intensity, currentLightIntensity, postExposureTweenSpeed).setOnUpdate((value) => { lightSource.intensity = value; });
            
            foreach (var light in nighttimeLights) {
                int nighttimeLightIntensity = isNight ? 1000 : 0;
                LeanTween.value(gameObject, light.intensity, nighttimeLightIntensity, lightsTweenSpeed).setOnUpdate((value) => { light.intensity = value; });
            }
        }
    }

    void OnDestroy() {
        LeanTween.cancel(gameObject);
    }
}