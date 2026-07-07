using System;

/// <summary>
/// Patron Observer: canal central de eventos del juego.
/// Los sistemas (HUD, vidas, plataformas) se suscriben sin acoplarse entre si.
/// </summary>
public static class GameEvents
{
    public static event Action<WordData> OnCorrectAnswer;
    public static event Action<WordData> OnWrongAnswer;

    public static void RaiseCorrectAnswer(WordData word) => OnCorrectAnswer?.Invoke(word);
    public static void RaiseWrongAnswer(WordData word) => OnWrongAnswer?.Invoke(word);
}