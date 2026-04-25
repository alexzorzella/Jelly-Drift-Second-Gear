using System.Collections.Generic;
using UnityEngine;

public class HcTimeScale : HCommand {
    public List<string> AutocompleteOptions() {
        return new List<string>();
    }

    public string CommandFunction(params string[] parameters) {
        float timeScale = 1.0F;

        bool successful = float.TryParse(parameters[1], out timeScale);

        if (!successful) {
            return "Invalid time scale inputted";
        }
        
        Time.timeScale = timeScale;
        
        return $"Set the time scale to {timeScale}";
    }

    public string CommandHelp() {
        return "Sets the time scale";
    }

    public string Keyword() {
        return "timeScale";
    }
}