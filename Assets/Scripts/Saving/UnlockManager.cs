using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockManager : MonoBehaviour {
    public enum UnlockType {
        Car,
        Skin,
        Map
    }

    public static UnlockManager Instance;
    public TextMeshProUGUI text;
    public GameObject unlockDisplayPrefab;
    CarDisplay carDisplay;
    Vector3 defaultSize;
    Vector3 desiredSize;
    Transform maps;
    int n;
    bool readyToSkip;
    public List<Unlock> unlocks = new();

    void Start() {
        Instance = this;
        defaultSize = transform.localScale;
        desiredSize = Vector3.zero;
        transform.localScale = Vector3.zero;
        carDisplay = Instantiate(unlockDisplayPrefab).GetComponentInChildren<CarDisplay>();
        maps = carDisplay.transform.parent.GetChild(2);
        NextDisplay();
    }

    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, desiredSize, Time.unscaledDeltaTime * 3f);
        if (Input.anyKey && readyToSkip) {
            NextDisplay();
        }
    }

    public void NextDisplay() {
        readyToSkip = false;
        Invoke("SetSkipReady", 0.4f);
        if (n >= unlocks.Count) {
            gameObject.SetActive(false);
            return;
        }

        var list = unlocks;
        var num = n;
        n = num + 1;
        // DisplayUnlock(list[num]);
        SoundManager.i.PlayUnlock();
    }

    public void DisplayUnlock(Unlock u) {
        // for (var i = 0; i < maps.childCount; i++) {
        //     maps.GetChild(i).gameObject.SetActive(false);
        // }
        //
        // transform.localScale = Vector3.zero;
        // desiredSize = defaultSize;
        // var str = "";
        // if (u.type == UnlockType.Car) {
        //     SaveManager.Instance.state.carsUnlocked[u.index] = true;
        //     SaveManager.Instance.Save();
        //     str = "\"" + PrefabManager.Instance.cars[u.index].name + "\"";
        //     carDisplay.SpawnCar(u.index);
        //     carDisplay.SetSkin(SaveManager.Instance.state.skins[u.index].Length - 1);
        // }
        // else if (u.type == UnlockType.Skin) {
        //     SaveManager.Instance.state.skins[u.index][u.subIndex] = true;
        //     SaveManager.Instance.Save();
        //     carDisplay.SpawnCar(u.index);
        //     carDisplay.SetSkin(u.subIndex);
        //     str = PrefabManager.Instance.cars[u.index].name + " \"" +
        //           carDisplay.currentCar.GetComponent<CarSkin>().GetSkinName(u.subIndex) + "\"";
        // }
        // else if (u.type == UnlockType.Map) {
        //     SaveManager.Instance.state.mapsUnlocked[u.index] = true;
        //     SaveManager.Instance.Save();
        //     maps.GetChild(u.index).gameObject.SetActive(true);
        //     str = "\"" + MapManager.Instance.maps[u.index].name + "\"";
        //     carDisplay.Hide();
        // }
        //
        // text.text = "<size=100%>unlocked:\n<b><size=80%>" + str;
    }

    void SetSkipReady() {
        readyToSkip = true;
    }

    public class Unlock {
        public int index;
        public int subIndex;
        public UnlockType type;

        public Unlock(UnlockType t, int index, int subIndex) {
            type = t;
            this.index = index;
            this.subIndex = subIndex;
        }
    }
}