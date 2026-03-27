using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceDetails : MonoBehaviour {
    public Image[] pbStars;
    public TextMeshProUGUI text;

    public void UpdateStars(int map) {
        var num = SaveManager.i.state.races[map] + 1;

        if (num <= 0) {
            text.text = "None";
        } else {
            text.text = ((DifficultyCycle.Difficulty)(num - 1)).ToString();
        }

        for (var i = 0; i < pbStars.Length; i++) {
            if (i < num) {
                pbStars[i].color = Color.yellow;
            } else {
                pbStars[i].color = Color.gray;
            }
        }
    }
}