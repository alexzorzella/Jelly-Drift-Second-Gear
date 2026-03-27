using UnityEngine;

public class CheckPoint : MonoBehaviour {
    bool done;
    public int siblingIndex { get; set; }

    void Awake() {
        siblingIndex = transform.GetSiblingIndex();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car")) {
            var component = other.gameObject.transform.root.GetComponent<CheckpointUser>();
            if (component) {
                component.CheckPoint(this);
            }
        }
    }
}