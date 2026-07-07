using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton que muestra las preguntas en pantalla (Requisito 3),
/// gestiona respuestas correctas/incorrectas y el reintento con
/// intervalo de tiempo (Requisito 5).
/// </summary>
public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Image wordImage;
    [SerializeField] private Button[] answerButtons;   // exactamente 4
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Configuracion (Requisito 5)")]
    [SerializeField] private float retryDelay = 3f;    // segundos de espera tras fallar
    [SerializeField] private float closeDelay = 1f;    // segundos antes de cerrar al acertar

    private WordData currentWord;
    private Action onCorrectCallback;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        questionPanel.SetActive(false);
    }

    /// <summary>Muestra una pregunta y pausa el juego.</summary>
    public void ShowQuestion(WordData word, Action onCorrect)
    {
        currentWord = word;
        onCorrectCallback = onCorrect;

        Time.timeScale = 0f;               // pausa el gameplay
        questionPanel.SetActive(true);
        feedbackText.text = "";

        questionText.text = string.IsNullOrEmpty(word.questionText)
            ? $"How do you say \"{word.spanishTranslation}\" in English?"
            : word.questionText;

        if (word.image != null)
        {
            wordImage.sprite = word.image;
            wordImage.gameObject.SetActive(true);
        }
        else
        {
            wordImage.gameObject.SetActive(false);
        }

        SetupButtons();
    }

    /// <summary>Baraja la respuesta correcta con los 3 distractores.</summary>
    private void SetupButtons()
    {
        List<string> options = new List<string>(currentWord.distractors);
        options.Add(currentWord.englishWord);
        Shuffle(options);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            string option = options[i];
            answerButtons[i].interactable = true;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = option;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(option));
        }
    }

    private void OnAnswerSelected(string option)
    {
        bool isCorrect = option == currentWord.englishWord;
        if (isCorrect) StartCoroutine(HandleCorrect());
        else StartCoroutine(HandleWrong());
    }

    private IEnumerator HandleCorrect()
    {
        SetButtonsInteractable(false);
        feedbackText.color = new Color(0.2f, 0.75f, 0.3f);
        feedbackText.text = "Correct! Well done!";

        GameEvents.RaiseCorrectAnswer(currentWord);   // Observer: notifica al HUD, etc.

        // WaitForSecondsRealtime porque Time.timeScale esta en 0
        yield return new WaitForSecondsRealtime(closeDelay);

        questionPanel.SetActive(false);
        Time.timeScale = 1f;                          // reanuda el juego
        onCorrectCallback?.Invoke();                  // avisa a la QuestionZone
    }

    private IEnumerator HandleWrong()
    {
        SetButtonsInteractable(false);
        feedbackText.color = new Color(0.85f, 0.25f, 0.25f);

        GameEvents.RaiseWrongAnswer(currentWord);     // Observer: restara vida en Fase 4

        // Requisito 5: reintento tras un intervalo, con cuenta regresiva visible
        float remaining = retryDelay;
        while (remaining > 0f)
        {
            feedbackText.text = $"Incorrect. Try again in {Mathf.CeilToInt(remaining)}...";
            yield return new WaitForSecondsRealtime(0.1f);
            remaining -= 0.1f;
        }

        feedbackText.text = "";
        SetupButtons();   // rebaraja las opciones y reactiva los botones
    }

    private void SetButtonsInteractable(bool state)
    {
        foreach (Button b in answerButtons) b.interactable = state;
    }

    /// <summary>Barajado Fisher-Yates.</summary>
    private void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}