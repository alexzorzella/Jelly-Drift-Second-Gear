public class ConsoleUtilityDeprecated {
    public static string ParameterParse(string[] parameters) {
        var result = "";

        for (var i = 1; i < parameters.Length; i++)
            if (parameters.Length < 2)
                result += parameters[i];
            else if (i < parameters.Length - 1)
                result += parameters[i] + " ";
            else
                result += parameters[i];

        return result;
    }
}