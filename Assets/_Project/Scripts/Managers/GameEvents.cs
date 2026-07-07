using System;

/// <summary>
/// Patron Observer: canal central de eventos del juego.
/// Los sistemas (HUD, vidas, plataformas, flash) se suscriben sin acoplarse entre si.
/// </summary>
public static class GameEvents
{
    // Preguntas
    public static event Action<WordData> OnCorrectAnswer;
    public static event Action<WordData> OnWrongAnswer;

    // Vidas (Requisito 7)
    public static event Action<int> OnLivesChanged;    // vidas restantes
    public static event Action OnPlayerDamaged;        // para feedback visual
    public static event Action OnGameOver;

    // Progreso (Requisito 8)
    public static event Action<int> OnWordsLearnedChanged;

    public static void RaiseCorrectAnswer(WordData word) => OnCorrectAnswer?.Invoke(word);
    public static void RaiseWrongAnswer(WordData word) => OnWrongAnswer?.Invoke(word);
    public static void RaiseLivesChanged(int lives) => OnLivesChanged?.Invoke(lives);
    public static void RaisePlayerDamaged() => OnPlayerDamaged?.Invoke();
    public static void RaiseGameOver() => OnGameOver?.Invoke();
    public static void RaiseWordsLearnedChanged(int count) => OnWordsLearnedChanged?.Invoke(count);
}
