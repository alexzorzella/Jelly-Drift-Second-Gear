using UnityEngine;
using UnityEngine.UI;

public class CarStats : MonoBehaviour {
    public Image speed;
    public Image drift;
    public Image stability;
    public float minSpeed;
    public float maxSpeed;
    public float minDrift;
    public float maxDrift;
    public float minStab;
    public float maxStab;
    float dDrift;
    float dSpeed;
    float dStability;

    void Update() {
        speed.transform.localScale =
            Vector3.Lerp(speed.transform.localScale, new Vector3(dSpeed, 1f, 1f), Time.deltaTime * 4f);
        drift.transform.localScale =
            Vector3.Lerp(drift.transform.localScale, new Vector3(dDrift, 1f, 1f), Time.deltaTime * 4f);
        stability.transform.localScale = Vector3.Lerp(stability.transform.localScale, new Vector3(dStability, 1f, 1f),
            Time.deltaTime * 4f);
    }

    public void SetStats(int car) {
        CarData carData = CarCatalogue.GetSelectedCarData();
        
        var engineForce = carData.GetEngineForce();
        var driftMultiplier = carData.GetDriftMultiplier();
        var num = carData.GetStability();
        dSpeed = (engineForce - minSpeed) / (maxSpeed - minSpeed);
        dDrift = (driftMultiplier - minDrift) / (maxDrift - minDrift);
        dStability = (num - minStab) / (maxStab - minStab);
    }
}