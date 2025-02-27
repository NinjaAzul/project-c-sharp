namespace Project_C_Sharp.Common.Constants.Regex;

public static class RegexPatterns
{
    /// <summary>
    /// Regex para validar nomes que contém apenas letras e espaços
    /// </summary>
    public const string OnlyLettersAndSpaces = "^[a-zA-Z ]*$";

    /// <summary>
    /// Regex para validar se contém pelo menos uma letra maiúscula
    /// </summary>
    public const string HasUpperCase = "[A-Z]";

    /// <summary>
    /// Regex para validar se contém pelo menos um número
    /// </summary>
    public const string HasNumber = "[0-9]";
}
