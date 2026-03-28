using UnityEngine;

public class CollisionSfx : MonoBehaviour {
    public RandomSfx crashAudio;
    bool ready = true;

    void OnCollisionEnter(Collision other) {
        if (!crashAudio || !ready) {
            return;
        }

        if (other.relativeVelocity.magnitude < 6f) {
            return;
        }

        if (other.contacts.Length != 0) {
            var rotation = Quaternion.LookRotation(other.contacts[0].normal);
            var gameObject = ResourceLoader.InstantiateObject("CrashParticles", other.contacts[0].point, rotation);
            var component = other.gameObject.GetComponent<Renderer>();
            if (component) {
                var material = component.materials[0];
                gameObject.GetComponent<ParticleSystem>().GetComponent<Renderer>().material = material;
            }
        }

        crashAudio.Randomize();
        ready = false;
        Invoke("GetReady", 0.5f);
    }

    void GetReady() {
        ready = true;
    }
}