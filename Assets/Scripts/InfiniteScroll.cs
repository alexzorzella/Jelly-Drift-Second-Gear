using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfiniteScroll : MonoBehaviour {
    public enum Direction {
        HORIZONTAL,
        VERTICAL
    }

    public Direction direction;
    public float speed = 1F;

    TextMeshProUGUI[] textComponents;
    RectTransform rect;

    float width = -1;

    public void Awake() {
        rect = GetComponent<RectTransform>();
        textComponents = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text) {
        float totalWidth = 0;

        foreach(var textComponent in textComponents) {
            textComponent.text = text;
            textComponent.ForceMeshUpdate();

            RectTransform localRect = textComponent.GetComponent<RectTransform>();
            rect.sizeDelta = textComponent.GetRenderedValues(false);

            float localWidth = rect.rect.width;

            if (width <= 0) {
                width = localWidth;
            }

            localRect.anchoredPosition = new Vector2(totalWidth, 0);
            
            totalWidth += localWidth;
        }
    }
    
    public void Update() {
        rect.Translate(
            new Vector2(direction == Direction.HORIZONTAL ? 1 : 0, direction == Direction.VERTICAL ? 1 : 0) * 
            Time.deltaTime * speed);

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x % width, rect.anchoredPosition.y);
    }
}