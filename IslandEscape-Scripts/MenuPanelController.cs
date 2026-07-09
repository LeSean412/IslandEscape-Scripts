using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPanelController : MonoBehaviour
{
    [Header("Panel Windows")]
    public GameObject mainMenuPanel;
    public GameObject customizationPanel;

    [Header("Customization Settings")]
    public TextMeshProUGUI characterLabelText;
    public Slider volumeSlider;
    public AudioSource menuAmbientAudio;

    private bool editingBrother = true;
    private int shirtColorIndex = 0;

    void Start()
    {
        // 🌟 Ensure the game starts on the Main Menu, hiding the custom GUI
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (customizationPanel != null) customizationPanel.SetActive(false);

        // Link up your audio slider to your menu sound loops
        if (volumeSlider != null && menuAmbientAudio != null)
        {
            volumeSlider.value = menuAmbientAudio.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        UpdateCustomizationText();
    }

    // 🔄 NAVIGATION: Jump from Main Menu into Customization Panel
    public void OpenCustomizationMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (customizationPanel != null) customizationPanel.SetActive(true);
    }

    // 🔄 NAVIGATION: Back button if they want to return to Main Menu
    public void GoBackToMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (customizationPanel != null) customizationPanel.SetActive(false);
    }

    // 👥 GUI INTERACTION: Toggle between character profiles
    public void ToggleSelectedCharacter()
    {
        editingBrother = !editingBrother;
        UpdateCustomizationText();
    }

    // 👕 GUI INTERACTION: Cycle clothes options
    public void CycleClothingOption()
    {
        shirtColorIndex = (shirtColorIndex + 1) % 4; // Cycles 4 test variations
        Debug.Log($"Selected Clothing Style Index: {shirtColorIndex} for {(editingBrother ? "Brother" : "Sister")}");
    }

    // 🔊 AUDIO INTERACTION: Slider control
    public void SetVolume(float volume)
    {
        if (menuAmbientAudio != null)
        {
            menuAmbientAudio.volume = volume;
        }
    }

    void UpdateCustomizationText()
    {
        if (characterLabelText != null)
        {
            characterLabelText.text = editingBrother ? "CURRENT SURVIVOR: BROTHER" : "CURRENT SURVIVOR: SISTER";
        }
    }
}
