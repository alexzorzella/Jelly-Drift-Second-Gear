using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RealtimeSun : MonoBehaviour {
    const int secondsInADay = 86400;

    const float nightPostExposureFactor = -8F;
    const float lightIntensity = 5F;

    public Volume volume;
    ColorAdjustments colorAdjustments;
    
    float percentageOfDayPassed = 0;

    Light lightSource;

    public GameObject nighttimeLights;

    bool isNight = false;

    float rotY;
    float rotZ;
    
    void Start() {
        lightSource = GetComponent<Light>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        rotY = transform.rotation.eulerAngles.y;
        rotZ = transform.rotation.eulerAngles.z;
    }

    void Update() {
        DateTime now = DateTime.Now;
        TimeSpan timeOfDay = now.TimeOfDay;
        int secondsPassedToday = (int)timeOfDay.TotalSeconds;
        
        percentageOfDayPassed = (float)secondsPassedToday / secondsInADay;
        float sunAngle = 360 * percentageOfDayPassed - 90;

        transform.rotation = Quaternion.Euler(sunAngle, rotY, rotZ);

        bool night = sunAngle > 190 || sunAngle < 80;

        if (isNight != night) {
            isNight = night;
            
            float currentPostExposure = isNight ? nightPostExposureFactor : 0;
            currentPostExposure = Mathf.Clamp(currentPostExposure, nightPostExposureFactor, 0);
    
            colorAdjustments.postExposure.value = currentPostExposure;
            
            float currentLightIntensity = !isNight ? lightIntensity : 0;
            
            lightSource.intensity = currentLightIntensity;
            
            nighttimeLights.SetActive(isNight);
        }
        
    }
}