using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public GameObject notificationPanel;
    public Text notificationText;

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
            return;
        }
    }

    public void ShowNotification(string message)
    {
        notificationPanel.SetActive(true);
        notificationText.text = message;
        Invoke("HideNotification", 2f); // Hides notification after 2 seconds
    }

    void HideNotification()
    {
        notificationPanel.SetActive(false);
    }
}
