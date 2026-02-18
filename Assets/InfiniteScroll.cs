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

    List<TextMeshProUGUI> textComponents;
    RectTransform rect;

    float width;

    public void Start() {
        rect = GetComponent<RectTransform>();
        width = rect.sizeDelta.x;

        textComponents.Add(GetComponent<TextMeshProUGUI>());
        textComponents.AddRange(GetComponentsInChildren<TextMeshProUGUI>());
    }

    public void SetText(string text) {
        foreach(var textComponent in textComponents) {
            textComponent.text = text;
            textComponent.ForceMeshUpdate();
            textComponent.GetComponent<RectTransform>().sizeDelta = textComponent.GetRenderedValues() + new Vector2(0, 20);
        }
    }
    
    public void Update() {
        rect.Translate(
            new Vector2(direction == Direction.HORIZONTAL ? 1 : 0, direction == Direction.VERTICAL ? 1 : 0) * 
            Time.deltaTime * speed);

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x % width, rect.anchoredPosition.y);
    }
}