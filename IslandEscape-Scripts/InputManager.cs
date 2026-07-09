using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

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

    public Vector3 GetMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        return new Vector3(moveX, 0, moveZ);
    }

    public bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool GetInteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
