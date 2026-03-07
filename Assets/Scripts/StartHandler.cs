using System.Collections.Generic;
using UnityEngine;

public class StartHandler : MonoBehaviour {
    readonly List<StartListener> listeners = new();
    
    int countdown = 3;

    public void RegsiterListener(StartListener listener, bool notifyOnRegister=true) {
        listeners.Add(listener);

        if (notifyOnRegister) {
            if (countdown >= 0) {
                listener.NotifyCountdownUpdated(countdown);
            } else {
                listener.NotifyStartRace();
            }
        }
    }

    void NotifyListeners() {
        foreach (var listener in listeners) {
            if (countdown >= 0) {
                listener.NotifyCountdownUpdated(countdown);
            } else {
                listener.NotifyStartRace();
            }
        }
    }
    
    void Start() {
        Invoke(nameof(CountDown), GameController.Instance.startTime / 3F);
    }

    void CountDown() {
        countdown--;
        NotifyListeners();

        if (countdown >= 0) {
            Invoke(nameof(CountDown), GameController.Instance.startTime / 3F);
        }
        
    }
}