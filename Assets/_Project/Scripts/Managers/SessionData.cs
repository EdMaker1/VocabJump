/// <summary>
/// Datos de sesion que persisten entre escenas (clase estatica, no MonoBehaviour).
/// Transporta el nombre del jugador (login) y las estadisticas de cada
/// nivel hacia las pantallas de resumen (Requisito 10).
/// </summary>
public static class SessionData
{
    // Perfil (se llena en la escena Login)
    public static string PlayerName = "Player";

    // Estadisticas del ultimo nivel jugado
    public static int LastLevelCorrect;
    public static int LastLevelWrong;
    public static int LastLevelWords;

    // Acumulado de toda la partida (para el resumen final)
    public static int TotalCorrect;
    public static int TotalWrong;
    public static int TotalWords;

    // A que escena ir despues del resumen parcial
    public static string NextLevelName = "Level2";

    /// <summary>Guarda las estadisticas al terminar un nivel.</summary>
    public static void RegisterLevelResults(int correct, int wrong, int words, string nextLevel)
    {
        LastLevelCorrect = correct;
        LastLevelWrong = wrong;
        LastLevelWords = words;

        TotalCorrect += correct;
        TotalWrong += wrong;
        TotalWords += words;

        NextLevelName = nextLevel;
    }

    /// <summary>Porcentaje de aciertos del ultimo nivel (Requisito 10).</summary>
    public static int LastLevelPercentage()
    {
        int total = LastLevelCorrect + LastLevelWrong;
        return total == 0 ? 0 : UnityEngine.Mathf.RoundToInt(100f * LastLevelCorrect / total);
    }

    public static int TotalPercentage()
    {
        int total = TotalCorrect + TotalWrong;
        return total == 0 ? 0 : UnityEngine.Mathf.RoundToInt(100f * TotalCorrect / total);
    }

    /// <summary>Reinicia la partida (al volver al menu principal).</summary>
    public static void ResetRun()
    {
        LastLevelCorrect = LastLevelWrong = LastLevelWords = 0;
        TotalCorrect = TotalWrong = TotalWords = 0;
        NextLevelName = "Level2";
    }
}