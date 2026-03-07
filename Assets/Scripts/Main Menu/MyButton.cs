using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler {
    public int value;

    public void OnPointerDown(PointerEventData eventData) {
        value = 1;
    }

    public void OnPointerUp(PointerEventData eventData) {
        value = 0;
    }
}