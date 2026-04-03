using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CycleMenu : MonoBehaviour {
    public TextMeshProUGUI[] correspondingText;
    public int startSelect;

    List<ItemCycle> cycles;
    List<TextMeshProUGUI> cycleText;
    int selected;

    MenuControls menuControls;

    readonly Color unselectedColor = Color.white;
    readonly Color selectedColor = new Color(0.9F, 0.58F, 0.01F, 1);
    
    void Awake() {
        selected = startSelect;
        cycles = new List<ItemCycle>();
        cycleText = new List<TextMeshProUGUI>();
        for (var i = 0; i < transform.childCount; i++) {
            var component = transform.GetChild(i).GetComponent<ItemCycle>();
            
            if (component) {
                cycles.Add(component);
                var componentInChildren = component.GetComponentInChildren<TextMeshProUGUI>();
                cycleText.Add(componentInChildren);
                componentInChildren.color = unselectedColor;
            }
        }

        cycleText[selected].color = selectedColor;

        InitializeInput();
    }

    void InitializeInput() {
        menuControls = new();

        menuControls.User.ScrollUp.performed += ScrollUp;
        menuControls.User.ScrollDown.performed += ScrollDown;

        menuControls.User.Select.performed += Select;
    }

    void Start() {
        SaveManager.i.state.skins[5][1] = true;
    }

    void OnEnable() {
        selected = startSelect;
        UpdateSelected();

        menuControls.Enable();
    }

    void OnDisable() {
        menuControls.Disable();
    }

    void UpdateSelected() {
        foreach (var textMeshProUGUI in cycleText) {
            textMeshProUGUI.color = unselectedColor;
            if (correspondingText.Length != 0 && !correspondingText[selected].gameObject.CompareTag("Ignore")) {
                correspondingText[selected].color = unselectedColor;
            }
        }

        cycleText[selected].color = selectedColor;
        if (correspondingText.Length != 0 && !correspondingText[selected].gameObject.CompareTag("Ignore")) {
            correspondingText[selected].color = selectedColor;
        }
    }

    void ScrollUp(InputAction.CallbackContext context) {
        Navigate(new Vector2(0, 1));
    }

    void ScrollDown(InputAction.CallbackContext context) {
        Navigate(new Vector2(0, -1));
    }

    void Select(InputAction.CallbackContext context) {
        if (cycles[selected].activeCycle) {
            cycles[selected].Cycle(1);
            SoundManager.i.PlayCycle();
        }
        else {
            SoundManager.i.PlayError();
        }
    }

    void Navigate(Vector2 input) {
        int verticalInput = -(int)input.y;

        cycleText[selected].color = unselectedColor;
        if (correspondingText.Length != 0 && !correspondingText[selected].gameObject.CompareTag("Ignore")) {
            correspondingText[selected].color = selectedColor;
        }

        selected = IncrementWithOverflow.Run(selected, cycles.Count, verticalInput);
        
        cycleText[selected].color = selectedColor;
        if (correspondingText.Length != 0 && !correspondingText[selected].gameObject.CompareTag("Ignore")) {
            correspondingText[selected].color = selectedColor;
        }

        SoundManager.i.PlayMenuNavigate();
    }
}