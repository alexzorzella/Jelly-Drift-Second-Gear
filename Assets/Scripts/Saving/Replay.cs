using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Replay : MonoBehaviour {
    public TextAsset[] daniTimes;
    int currentFrame;
    string filePath;
    Vector3 nextPos;
    Rigidbody rb;

    List<ReplayController.ReplayFrame> replay;
    float replayDeltaTime;

    void Start() {
        if (GameState.i.gamemode != Gamemode.TimeTrial || GameState.i.ghost == GhostCycle.Ghost.Off) {
            Destroy(this);
            return;
        }

        var ghost = GameState.i.ghost;
        replay = new List<ReplayController.ReplayFrame>();
        replayDeltaTime = 1f / ReplayController.Instance.hz;
        var text = "pb";
        text += GameState.i.map;
        if (ghost == GhostCycle.Ghost.PB) {
            filePath = Application.persistentDataPath + "/replays/" + text + ".txt";
            if (!File.Exists(filePath)) {
                Destroy(this);
                print("couldnt find destroying");
                return;
            }
        }

        // print("path: " + filePath);
        if (ghost == GhostCycle.Ghost.Dani) {
            ReadTextAsset();
            return;
        }

        var streamReader = new StreamReader(filePath);
        var text2 = streamReader.ReadLine();
        text2 = text2.Replace("(", string.Empty).Replace(")", string.Empty);
        var array = Array.ConvertAll(text2.Split(new[] {
            ','
        }), int.Parse);
        print(string.Concat("lna: ", array[0], ", ", array[1]));

        var ghostCarObject = Instantiate(ResourceLoader.LoadObject("Car"));
        ghostCarObject.GetComponent<Car>().Initialize(CarCatalogue.GetCarAtIndex(array[0]));
        
        CarSkin skin = ghostCarObject.GetComponent<CarSkin>();
        if (skin != null) {
            skin.SetSkin(array[1]);
        }
        
        while ((text2 = streamReader.ReadLine()) != null) {
            text2 = text2.Replace("(", string.Empty).Replace(")", string.Empty);
            var array2 = Array.ConvertAll(text2.Split(new[] {
                ','
            }), float.Parse);
            var item = new ReplayController.ReplayFrame(new Vector3(array2[0], array2[1], array2[2]),
                new Vector3(array2[3], array2[4], array2[5]), array2[6], array2[7]);
            replay.Add(item);
        }

        rb = ghostCarObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(ghostCarObject.GetComponentInChildren<Collider>());
        Destroy(ghostCarObject.GetComponent<PlayerInput>());
        var componentsInChildren = ghostCarObject.GetComponentsInChildren<ParticleSystem>();
        for (var i = 0; i < componentsInChildren.Length; i++) {
            componentsInChildren[i].gameObject.SetActive(false);
        }

        var componentsInChildren2 = ghostCarObject.GetComponentsInChildren<Suspension>();
        for (var i = 0; i < componentsInChildren2.Length; i++) {
            componentsInChildren2[i].showFx = false;
        }

        var componentsInChildren3 = ghostCarObject.GetComponentsInChildren<AudioSource>();
        for (var i = 0; i < componentsInChildren3.Length; i++) {
            componentsInChildren3[i].enabled = false;
        }

        ghostCarObject.AddComponent<Ghost>();
    }

    void FixedUpdate() {
        if (currentFrame >= replay.Count - 1) {
            return;
        }

        rb.MovePosition(Vector3.Lerp(rb.transform.position, replay[currentFrame].pos, Time.deltaTime * 30f));
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.Euler(replay[currentFrame].rot), Time.deltaTime * 30f));
        currentFrame++;
    }

    void ReadTextAsset() {
        var textAsset = daniTimes[GameState.i.map];
        if (!textAsset) {
            Destroy(this);
            return;
        }

        var array = textAsset.text.Split(new[] {
            Environment.NewLine
        }, StringSplitOptions.None);
        var i = 0;
        var array2 = Array.ConvertAll(array[i++].Replace("(", string.Empty).Replace(")", string.Empty).Split(new[] {
            ','
        }), int.Parse);
        
        var ghostCarObject = Instantiate(ResourceLoader.LoadObject("Car"));
        ghostCarObject.GetComponent<Car>().Initialize(CarCatalogue.GetCarAtIndex(array2[0]));

        CarSkin skin = ghostCarObject.GetComponent<CarSkin>();
        if (skin != null) {
            skin.SetSkin(array2[1]);  
        }
        
        while (i < array.Length - 1) {
            var array3 = Array.ConvertAll(array[i++].Replace("(", string.Empty).Replace(")", string.Empty).Split(new[] {
                ','
            }), float.Parse);
            var item = new ReplayController.ReplayFrame(new Vector3(array3[0], array3[1], array3[2]),
                new Vector3(array3[3], array3[4], array3[5]), array3[6], array3[7]);
            replay.Add(item);
        }

        rb = ghostCarObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Destroy(ghostCarObject.GetComponentInChildren<Collider>());
        Destroy(ghostCarObject.GetComponent<PlayerInput>());
        var componentsInChildren = ghostCarObject.GetComponentsInChildren<ParticleSystem>();
        for (var j = 0; j < componentsInChildren.Length; j++) {
            componentsInChildren[j].gameObject.SetActive(false);
        }

        var componentsInChildren2 = ghostCarObject.GetComponentsInChildren<Suspension>();
        for (var j = 0; j < componentsInChildren2.Length; j++) {
            componentsInChildren2[j].showFx = false;
        }

        var componentsInChildren3 = ghostCarObject.GetComponentsInChildren<AudioSource>();
        for (var j = 0; j < componentsInChildren3.Length; j++) {
            componentsInChildren3[j].enabled = false;
        }

        ghostCarObject.AddComponent<Ghost>();
    }
}