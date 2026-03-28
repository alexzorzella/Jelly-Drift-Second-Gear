using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapCycle : ItemCycle {
    public Image mapImg;
    public Image overlay;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI time;
    public GhostCycle ghostCycle;
    public DifficultyCycle difficultyCycle;
    public TextMeshProUGUI ghostText;
    public Button nextButton;
    public Transform starsDetails;
    public TextMeshProUGUI[] starTimes;
    public Image[] pbStars;
    public GameObject lockUi;
    [FormerlySerializedAs("gamemode")] public GameMode gameMode;
    public RaceDetails raceDetails;

    void Awake() {
        selected = SaveManager.i.state.lastMap;
    }

    void Start() {
        if (starsDetails) {
            starTimes = starsDetails.GetComponentsInChildren<TextMeshProUGUI>();
        }

        UpdateUI();
        max = MapManager.i.MapCount();
        // CarDisplay.Instance.Hide();
    }

    void Update() {
        if (lockUi.activeInHierarchy) {
            overlay.color = Color.Lerp(overlay.color, new Color(1f, 1f, 1f, 0.55f), Time.deltaTime * 1.2f);
            return;
        }

        overlay.color = Color.Lerp(overlay.color, MapManager.i.GetSelectedMap().GetColor(), Time.deltaTime * 0.9f);
    }

    void OnEnable() {
        if (!CarDisplay.Instance || !CarDisplay.Instance.currentCar) {
            return;
        }

        // CarDisplay.Instance.Hide();
        selected = SaveManager.i.state.lastMap;
        UpdateUI();
    }

    public override void Cycle(int n) {
        base.Cycle(n);
        MapManager.i.CycleSelectedMap(n);
        UpdateUI();
    }

    void UpdateUI() {
        if (raceDetails) {
            raceDetails.UpdateStars(selected);
        }

        lockUi.SetActive(false);

        MapManager.MapData mapData = MapManager.i.GetSelectedMap();
        
        mapImg.sprite = mapData.GetSprite();
        name.text = "| " + mapData.GetName();
        // time.text = "PB - " + Timer.GetFormattedTime(SaveManager.Instance.state.times[n]);
        if (ghostCycle) {
            ghostCycle.UpdateText();
        }
        
        if (difficultyCycle) {
            difficultyCycle.UpdateTextOnly();
        }
        
        if (starsDetails) {
            UpdateStars();
        }
        
        GameState.i.map = selected;
        nextButton.enabled = true;
        nextButton.GetComponent<ItemCycle>().activeCycle = true;
        SaveManager.i.state.lastMap = selected;
        SaveManager.i.Save();
    }

    void UpdateStars() {
        // for (var i = 0; i < starTimes.Length; i++) {
        //     starTimes[i].text = Timer.GetFormattedTime(MapManager.Instance.maps[selected].times[i]);
        // }
        //
        // var stars = MapManager.Instance.GetStars(selected, SaveManager.Instance.state.times[selected]);
        // for (var j = 0; j < pbStars.Length; j++) {
        //     if (j < stars) {
        //         pbStars[j].color = Color.yellow;
        //     }
        //     else {
        //         pbStars[j].color = Color.gray;
        //     }
        // }
    }

    public void SaveMap() {
        GameState.i.map = selected;
        GameState.i.gameMode = gameMode;
    }
}