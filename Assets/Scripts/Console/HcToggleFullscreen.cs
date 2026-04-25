using UnityEngine;
using System.Collections.Generic;

public class HcToggleFullscreen : HCommand {
    public string CommandFunction(params string[] parameters) {
		Screen.fullScreen = !Screen.fullScreen;
        
        return "Toggled fullscreen";
    }

    public string Keyword() {
        return "fullscreen";
    }

    public string CommandHelp() {
        return "Toggles whether the application is fullscreen";
    }

    public List<string> AutocompleteOptions() {
        return new List<string>();
    }
}