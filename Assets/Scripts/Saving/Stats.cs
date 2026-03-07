using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour {
    public TextMeshProUGUI text;

    void OnEnable() {
        print("text:  " + text);
        text.text = "<size=110%>Times\n<size=75%>";
        // for (var i = 0; i < MapManager.Instance.maps.Length; i++) {
        //     var name = MapManager.Instance.maps[i].name;
        //     var formattedTime = Timer.GetFormattedTime(SaveManager.Instance.state.times[i]);
        //     var textMeshProUGUI = text;
        //     textMeshProUGUI.text = string.Concat(textMeshProUGUI.text, name, " - ", formattedTime, "\n");
        // }
    }

    public void DeleteSave() {
        SaveManager.i.NewSave();
        SaveManager.i.Save();
    }
}