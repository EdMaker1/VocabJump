using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Login local: captura el nombre del jugador, lo guarda en la sesion
/// y en PlayerPrefs (persistencia local entre ejecuciones del juego).
/// </summary>
public class LoginController : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI welcomeText;

    private void Start()
    {
        // Si ya jugo antes en esta PC, precarga su nombre
        string savedName = PlayerPrefs.GetString("PlayerName", "");
        nameInput.text = savedName;
        welcomeText.text = string.IsNullOrEmpty(savedName)
            ? "Enter your name to start"
            : $"Welcome back, {savedName}!";

        ValidateInput(nameInput.text);
        nameInput.onValueChanged.AddListener(ValidateInput);
        playButton.onClick.AddListener(ConfirmLogin);
    }

    private void ValidateInput(string value)
    {
        // El boton solo se habilita con un nombre valido (min. 2 caracteres)
        playButton.interactable = value.Trim().Length >= 2;
    }

    private void ConfirmLogin()
    {
        string playerName = nameInput.text.Trim();
        SessionData.PlayerName = playerName;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Level1");
    }
}