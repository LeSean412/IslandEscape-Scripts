using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;

    public UnityEvent onPlayerDeath;
    public UnityEvent onPlayerVictory;

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

    public void PlayerDied()
    {
        Debug.Log("Player has died.");
        onPlayerDeath.Invoke();
    }

    public void PlayerVictorious()
    {
        Debug.Log("Player has won!");
        onPlayerVictory.Invoke();
    }
}
