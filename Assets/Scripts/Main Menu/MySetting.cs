using System.Linq;
using TMPro;
using UnityEngine;

public class MySetting : MonoBehaviour {
    public TextMeshProUGUI[] options { get; set; }

    void Awake() {
        options = (from r in GetComponentsInChildren<TextMeshProUGUI>()
            where r.tag != "Ignore"
            select r).ToArray();
    }
}