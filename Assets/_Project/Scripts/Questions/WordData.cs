using UnityEngine;

/// <summary>
/// Requisito 6: Banco de palabras organizado por categorias tematicas.
/// Cada asset de este tipo representa una palabra del vocabulario.
/// Los datos estan separados de la logica (buena practica de diseño).
/// </summary>
public enum WordCategory { Colores, Numeros, Animales, ObjetosDelHogar }
public enum WordDifficulty { A1, A2 }

[CreateAssetMenu(fileName = "Word_", menuName = "VocabJump/Word Data")]
public class WordData : ScriptableObject
{
    [Header("Palabra")]
    public string englishWord;          // ej: "red"
    public string spanishTranslation;   // ej: "rojo"

    [Tooltip("Si se deja vacio, se genera automaticamente: How do you say '...' in English?")]
    [TextArea] public string questionText;

    [Header("Clasificacion (Requisito 6)")]
    public WordCategory category;
    public WordDifficulty difficulty = WordDifficulty.A1;

    [Header("Opciones incorrectas (exactamente 3)")]
    public string[] distractors = new string[3];

    [Header("Opcional")]
    public Sprite image;                // imagen ilustrativa de la palabra
}