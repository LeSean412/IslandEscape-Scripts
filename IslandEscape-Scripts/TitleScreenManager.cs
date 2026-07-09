using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject customizationPanel;
    public GameObject settingsPanel;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button loadButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("🌟 Panel Return Buttons")]
    public Button settingsBackButton;
    public Button customizationBackButton;

    [Header("🌟 Audio Stack Buttons")]
    public Button soundToggleButton;       // Drag your SoundToggleButton here
    public Button musicToggleButton;       // Drag your MusicToggleButton here

    [Header("Audio Components")]
    public AudioSource menuMusic;
    public AudioClip buttonClickSound;

    // Internal state tracking variables
    private bool soundEnabled = true;
    private bool musicEnabled = true;

    void Start()
    {
        // 1. Sync Panel Visibility on Startup
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (customizationPanel != null) customizationPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // 2. Set Up Main Menu Button Listeners
        if (startButton != null) startButton.onClick.AddListener(OnStartGameClicked);
        if (loadButton != null) loadButton.onClick.AddListener(OnLoadGameClicked);
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsClicked);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitClicked);

        // 3. Set Up Panel Return Button Listeners
        if (settingsBackButton != null) settingsBackButton.onClick.AddListener(OnSettingsBackClicked);
        if (customizationBackButton != null) customizationBackButton.onClick.AddListener(OnCustomizationBackClicked);

        // 🌟 4. Set Up Your Brand-New Button Stack Listeners
        if (soundToggleButton != null) soundToggleButton.onClick.AddListener(OnSoundToggleClicked);
        if (musicToggleButton != null) musicToggleButton.onClick.AddListener(OnMusicToggleClicked);

        // 5. Initialize Settings and UI States from Save Data
        LoadAudioSettings();

        // 6. Start Menu Theme
        if (menuMusic != null && !menuMusic.isPlaying && musicEnabled)
        {
            menuMusic.Play();
        }

        // 7. Autosave System Check: Disable Load button if no save exists yet
        if (loadButton != null)
        {
            loadButton.interactable = (PlayerPrefs.GetInt("HasSaveData", 0) == 1);
        }
    }

    public void OnStartGameClicked()
    {
        PlayButtonSound();

        int hasPlayedBefore = PlayerPrefs.GetInt("IsFirstPlay", 0);

        if (hasPlayedBefore == 0)
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (customizationPanel != null) customizationPanel.SetActive(true);

            // 🌟 WAKE UP THE CHARACTER PREVIEW SYSTEM:
            if (CustomizationManager.Instance != null)
            {
                if (CustomizationManager.Instance.brotherPreview != null)
                {
                    CustomizationManager.Instance.brotherPreview.SetActive(CustomizationManager.Instance.customizingBrother);
                }
                if (CustomizationManager.Instance.sisterPreview != null)
                {
                    CustomizationManager.Instance.sisterPreview.SetActive(!CustomizationManager.Instance.customizingBrother);
                }
            }
        }
        else
        {
            LoadGameplayScene();
        }
    }

    public void OnLoadGameClicked()
    {
        PlayButtonSound();
        LoadGameplayScene();
    }

    public void OnSettingsClicked()
    {
        PlayButtonSound();
        if (settingsPanel != null)
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }

    public void OnSettingsBackClicked()
    {
        PlayButtonSound();
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void OnCustomizationBackClicked()
    {
        PlayButtonSound();
        if (customizationPanel != null) customizationPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // 🌟 HIDE THE MODELS AGAIN IF A PLAYER BACKS OUT TO THE FRONT PAGE:
        if (CustomizationManager.Instance != null)
        {
            if (CustomizationManager.Instance.brotherPreview != null)
                CustomizationManager.Instance.brotherPreview.SetActive(false);
            if (CustomizationManager.Instance.sisterPreview != null)
                CustomizationManager.Instance.sisterPreview.SetActive(false);
        }
    }

    // 🌟 CLICK HANDLER: Toggle Sound effects and play a feedback click immediately
    public void OnSoundToggleClicked()
    {
        soundEnabled = !soundEnabled;
        PlayerPrefs.SetInt("SFXMuted", soundEnabled ? 0 : 1);
        PlayerPrefs.Save();
        UpdateAudioUI();
        PlayButtonSound(); // Plays immediate feedback if turned ON
    }

    // 🌟 CLICK HANDLER: Toggle Music on/off and instantly mute/unmute the source component
    public void OnMusicToggleClicked()
    {
        musicEnabled = !musicEnabled;
        PlayerPrefs.SetInt("MusicMuted", musicEnabled ? 0 : 1);
        PlayerPrefs.Save();

        if (menuMusic != null)
        {
            menuMusic.mute = !musicEnabled;
            if (musicEnabled && !menuMusic.isPlaying)
            {
                menuMusic.Play();
            }
        }

        UpdateAudioUI();
        PlayButtonSound();
    }

    // Call this function at the absolute end of your character customization confirmation button!
    public void ConfirmCharacterCreation()
    {
        PlayerPrefs.SetInt("IsFirstPlay", 1);
        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();

        LoadGameplayScene();
    }

    void LoadGameplayScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // --- NEW AUDIO SYSTEMS MANAGEMENT ---
    void LoadAudioSettings()
    {
        // Read binary mute integers (0 = Active/False, 1 = Muted/True)
        soundEnabled = (PlayerPrefs.GetInt("SFXMuted", 0) == 0);
        musicEnabled = (PlayerPrefs.GetInt("MusicMuted", 0) == 0);

        if (menuMusic != null)
        {
            menuMusic.mute = !musicEnabled;
        }

        UpdateAudioUI();
    }

    // 🌟 DYNAMIC TEXT SYSTEM: Automatically updates both normal text and TextMeshPro sub-elements
    void UpdateAudioUI()
    {
        if (soundToggleButton != null)
        {
            var txt = soundToggleButton.GetComponentInChildren<Text>();
            if (txt != null) txt.text = soundEnabled ? "SOUND: ON" : "SOUND: OFF";

            var tmp = soundToggleButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmp != null) tmp.text = soundEnabled ? "SOUND: ON" : "SOUND: OFF";
        }

        if (musicToggleButton != null)
        {
            var txt = musicToggleButton.GetComponentInChildren<Text>();
            if (txt != null) txt.text = musicEnabled ? "MUSIC: ON" : "MUSIC: OFF";

            var tmp = musicToggleButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmp != null) tmp.text = musicEnabled ? "MUSIC: ON" : "MUSIC: OFF";
        }
    }

    // 🌟 CLEAN COMPILER SPACE FIX:
    public void OnQuitClicked()
    {
        PlayButtonSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else                        
        Application.Quit();
#endif
    }

    // 🌟 DYNAMIC AUDIO LINK: Plays the click audio hook cleanly
    public void PlayButtonSound()
    {
        if (buttonClickSound != null && menuMusic != null && soundEnabled)
        {
            menuMusic.PlayOneShot(buttonClickSound);
        }
    }
}