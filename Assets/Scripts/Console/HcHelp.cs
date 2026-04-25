using System.Collections.Generic;

public class HcHelp : HCommand {
    readonly List<string> options = new();

    public string CommandFunction(params string[] parameters) {
        foreach (var command in JConsole.i.commandList) {
            JConsole.i.WriteLine($"'{command.Keyword()}': {command.CommandHelp()}");
        }

        return $"Listed {JConsole.i.commandList.Count} commands.";
    }

    public string CommandHelp() {
        return "Lists all commands and their parameters";
    }

    public string Keyword() {
        return "help";
    }

    public List<string> AutocompleteOptions() {
        return options;
    }
}