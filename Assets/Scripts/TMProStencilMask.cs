using System;
using TMPro;
using UnityEngine;

public class TMProStencilMask : MonoBehaviour {
    void Awake() {
        TMP_Text tmpText = GetComponent<TMP_Text>();
        Material material = tmpText.materialForRendering;
        
        material.SetInt("_StencilID", 1);
        material.SetInt("_StencilComp", 8);
        material.SetInt("_StencilOp", 2);
        material.SetInt("_ColorMask", 0);
    }
}