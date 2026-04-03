using System.Collections.Generic;
using UnityEngine;

public class CarData {
    readonly int id = -1;
    
    readonly string name;
    
    readonly float mass;
    readonly float linearDamping;
    readonly float angularDamping;
    
    // Suspension Variables (Customizable)

    readonly float suspensionLength;
    readonly float restHeight;
    readonly float suspensionForce;
    readonly float suspensionDamping;

    // Car Specs (Customizable)
    
    readonly float engineForce = 5000f;
    readonly float steerForce = 1f;
    readonly float antiRoll = 5000f;
    readonly float stability;

    // Drift Specs (Customizable)
    
    readonly float driftMultiplier = 1f;
    readonly float driftThreshold = 0.5f;

    // Audio

    readonly string accelerateSoundName;
    readonly string decelerateSoundName;
    
    // Model and Materials

    readonly GameObject model;
    readonly List<Material> materials = new();
    
    CarData(
        int id,
        string name,
        float mass,
        float linearDamping,
        float angularDamping,
        float suspensionLength,
        float restHeight,
        float suspensionForce,
        float suspensionDamping,
        float engineForce,
        float steerForce,
        float antiRoll,
        float stability,
        float driftMultiplier,
        float driftThreshold,
        string accelerateSoundName,
        string decelerateSoundName,
        GameObject model,
        List<Material> materials) {
        this.id = id;
        this.name = name;
        
        this.mass = mass;
        this.linearDamping = linearDamping;
        this.angularDamping = angularDamping;
        
        this.suspensionLength = suspensionLength;
        this.restHeight = restHeight;
        this.suspensionForce = suspensionForce;
        
        this.suspensionDamping = suspensionDamping;
        this.engineForce = engineForce;
        this.steerForce = steerForce;
        this.antiRoll = antiRoll;
        this.stability = stability;
        
        this.driftMultiplier = driftMultiplier;
        this.driftThreshold = driftThreshold;
        
        this.accelerateSoundName = accelerateSoundName;
        this.decelerateSoundName = decelerateSoundName;

        this.model = model;
        this.materials = materials;
    }

    public class Builder {
        readonly int id = -1;
        
        readonly string name;
        
        float mass = 1000F;
        float linearDamping;
        float angularDamping;
        
        // Suspension Variables

        float suspensionLength = 0.35F;
        float restHeight = 0.17F;
        float suspensionForce = 17000;
        float suspensionDamping = 1000;

        // Car Specs

        float engineForce = 3000;
        float steerForce = 1;
        float antiRoll = 5000;
        float stability = 0.6F;

        // Drift Specs

        float driftMultiplier = 1.33F;
        float driftThreshold = 0.4F;

        // Audio

        string accelerateSoundName = "180xs_accel";
        string decelerateSoundName = "decelerate";
        
        // Model and Materials

        GameObject model;
        List<Material> materials = new();
        
        public Builder(int id, string name, string modelName = "") {
            this.id = id;
            this.name = name;

            if (string.IsNullOrWhiteSpace(modelName)) {
                modelName = name.ToLower().Replace(" ", "");
            }
            
            GameObject loadedModel = Resources.Load<GameObject>(modelName);

            if (loadedModel == null) {
                Debug.LogError($"Couldn't find model in Resources named {modelName}.");
            } else {
                model = loadedModel;
            }
        }
        
        public Builder WithPhysicsSpecs(
            float mass = 1000, 
            float linearDamping = 0, 
            float angularDamping = 0) {
            this.mass = mass;
            this.linearDamping = linearDamping;
            this.angularDamping = angularDamping;
            
            return this;
        }
        
        public Builder WithSuspensionSpecs(
            float suspensionLength = 0.35F,
            float restHeight = 0.17F,
            float suspensionForce = 17000,
            float suspensionDamping = 1000) {
            this.suspensionLength = suspensionLength;
            this.restHeight = restHeight;
            this.suspensionForce = suspensionForce;
            this.suspensionDamping = suspensionDamping;
            
            return this;
        }

        public Builder WithCarSpecs(
            float engineForce = 3000,
            float steerForce = 1,
            float antiRoll = 5000,
            float stability = 0.6F) {
            this.engineForce = engineForce;
            this.steerForce = steerForce;
            this.antiRoll = antiRoll;
            this.stability = stability;
            
            return this;
        }

        public Builder WithDriftSpecs(
            float driftMultiplier = 1.33F,
            float driftThreshold = 0.4F) {
            this.driftMultiplier = driftMultiplier;
            this.driftThreshold = driftThreshold;

            return this;
        }

        public Builder WithAudio(
            string accelerateSoundName = "180xs_accel",
            string decelerateSoundName = "decelerate") {
            this.accelerateSoundName = accelerateSoundName;
            this.decelerateSoundName = decelerateSoundName;

            return this;
        }

        public Builder WithMaterials(params string[] materialNames) {
            foreach (var materialName in materialNames) {
                materials.Add(Resources.Load<Material>(materialName));
            }
            
            return this;
        }
        
        public CarData Build() {
            return new CarData(
                id,
                name,
                mass,
                linearDamping,
                angularDamping,
                suspensionLength, 
                restHeight , 
                suspensionForce, 
                suspensionDamping, 
                engineForce, steerForce, 
                antiRoll, 
                stability, 
                driftMultiplier, 
                driftThreshold,
                accelerateSoundName,
                decelerateSoundName,
                model,
                materials);
        }
    }
    
    // Constants
    
    public const float brakeForce = 3000f;
    public const float dragForce = 3.5f;
    public const float rollFriction = 105f;
    public const float yawGripMultiplier = 0.15f;
    public const float yawGripThreshold = 0.6f;
    
    public string GetCarName() { return name; }
    
    public float GetMass() { return mass; }
    public float GetLinearDamping() { return linearDamping; }
    public float GetAngularDamping() { return angularDamping; }
    
    public float GetSuspensionLength() { return suspensionLength; }
    public float GetRestHeight() { return restHeight; }
    public float GetSuspensionForce() { return suspensionForce; }
    public float GetSuspensionDamping() { return suspensionDamping; }
    
    public float GetEngineForce() { return engineForce; }
    public float GetSteerForce() { return steerForce; }
    public float GetAntiRoll() { return antiRoll; }
    public float GetStability() { return stability; }
    
    public float GetDriftMultiplier() { return driftMultiplier; }
    public float GetDriftThreshold() { return driftThreshold; }
    
    public string GetAccelerateSoundName() { return accelerateSoundName; }
    public string GetDecelerateSoundName() { return decelerateSoundName; }
    
    public GameObject GetModel() { return model; }
    public List<Material> GetMaterials() { return materials; }

    public int GetId() { return id; }
}