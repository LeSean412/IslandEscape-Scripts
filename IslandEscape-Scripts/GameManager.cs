using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // 🌟 REMOVED THE OLD DUPLICATE LINE FROM HERE!

    public enum GameState { Cutscene, Playing, BossFight, Paused, GameOver, Victory }

    [Header("Game State")]
    public GameState currentState = GameState.Cutscene;

    [Header("Players")]
    public ThirdPersonController brother;
    public ThirdPersonController sister;
    public ThirdPersonController activePlayer;

    [Header("Boat Building")]
    public int boatPiecesCollected = 0;
    public int totalBoatPieces = 5;
    public List<string> collectedPieces = new List<string>();

    [Header("Environment")]
    public Light mainLight;
    public ParticleSystem rainEffect;
    public GameObject finalBossPrefab;
    public Transform bossSpawnPoint;

    [Header("Audio")]
    public AudioSource ambientAudio;
    public AudioSource musicAudio;
    public AudioClip jungleAmbience;
    public AudioClip rainSound;
    public AudioClip bossMusic;

    [Header("Dependencies")]
    public CutsceneManager cutsceneManager;

    private bool gameStarted = false;

    // 🌟 THIS IS THE ONLY INSTANCE VARIABLE WE KEEP:
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ... everything else below stays EXACTLY the same as your code!
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameScene" && !gameStarted)
        {
            gameStarted = true;
            StartCoroutine(GameSequence());
        }
    }

    IEnumerator GameSequence()
    {
        yield return StartCoroutine(cutsceneManager.PlayPlaneCrashCutscene());
        yield return StartCoroutine(cutsceneManager.PlayIslandLandingCutscene());
        StartGameplay();
    }

    void StartGameplay()
    {
        currentState = GameState.Playing;

        // Ensure the brother starts as the main active character
        activePlayer = brother;
        brother.gameObject.SetActive(true);
        sister.gameObject.SetActive(false);

        // 🌟 Turn controls ON for brother, and OFF for sister
        brother.EnableControl();
        sister.DisableControl();

        SetupDarkForest();
        if (rainEffect != null) rainEffect.Play();
    }
    void SetupDarkForest()
    {
        RenderSettings.ambientLight = new Color(0.15f, 0.15f, 0.2f);
        RenderSettings.fogColor = new Color(0.1f, 0.12f, 0.15f);
        RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.08f;

        if (mainLight != null)
        {
            mainLight.intensity = 0.4f;
            mainLight.color = new Color(0.7f, 0.75f, 0.85f);
        }
    }

    public void CollectBoatPiece(string pieceName)
    {
        if (!collectedPieces.Contains(pieceName))
        {
            collectedPieces.Add(pieceName);
            boatPiecesCollected++;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowNotification($"Found {pieceName}! ({boatPiecesCollected}/{totalBoatPieces})");
            }

            if (boatPiecesCollected >= totalBoatPieces)
            {
                StartCoroutine(TriggerFinalBoss());
            }
        }
    }

    IEnumerator TriggerFinalBoss()
    {
        currentState = GameState.BossFight;
        yield return StartCoroutine(cutsceneManager.PlayFinalBossIntroCutscene());

        Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : new Vector3(0, 0, 20);
        GameObject boss = Instantiate(finalBossPrefab, spawnPos, Quaternion.identity);

        if (musicAudio != null && bossMusic != null)
        {
            musicAudio.clip = bossMusic;
            musicAudio.loop = true;
            musicAudio.Play();
        }

        EnemyBase bossScript = boss.GetComponent<EnemyBase>();
        yield return new WaitUntil(() => bossScript == null || bossScript.health <= 0);

        StartCoroutine(VictorySequence());
    }

    IEnumerator VictorySequence()
    {
        currentState = GameState.Victory;
        yield return StartCoroutine(cutsceneManager.PlayVictoryCutscene());
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.PlayerVictorious();
        }
    }

    void Update()
    {
        // Press the Tab key to switch between the brother and sister
        if (Input.GetKeyDown(KeyCode.Tab) && currentState == GameState.Playing)
        {
            SwapPlayers();
        }
    }

    void SwapPlayers()
    {
        if (activePlayer == brother)
        {
            // Switch to Sister
            brother.DisableControl();
            brother.gameObject.SetActive(false);

            activePlayer = sister;
            sister.gameObject.SetActive(true);
            sister.EnableControl();

            if (UIManager.Instance != null) UIManager.Instance.ShowNotification("Controlling Sister");
        }
        else
        {
            // Switch back to Brother
            sister.DisableControl();
            sister.gameObject.SetActive(false);

            activePlayer = brother;
            brother.gameObject.SetActive(true);
            brother.EnableControl();

            if (UIManager.Instance != null) UIManager.Instance.ShowNotification("Controlling Brother");
        }
    }
}