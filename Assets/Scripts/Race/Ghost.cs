using UnityEngine;

public class Ghost : MonoBehaviour {
    public Material ghost;

    Renderer[] renderers;

    void Start() {
        ghost = Resources.Load<Material>("Ghost 1");
        Renderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
        renderers = componentsInChildren;
        foreach (var renderer in renderers) {
            var materials = renderer.materials;
            for (var j = 0; j < materials.Length; j++) {
                var material = new Material(ghost);
                material.color = materials[j].color;
                material.color = new Color(material.color.r, material.color.g, material.color.b, 0.2f);
                materials[j] = material;
            }

            renderer.materials = materials;
        }
    }
}