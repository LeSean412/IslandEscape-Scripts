using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    public float cameraTurnSpeed = 60f; // 🌟 How fast the camera pivots when you hold A/D or Left/Right
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    [SerializeField] private bool controlEnabled = false;
    private Transform cameraTransform;
    private Transform visualMeshTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (transform.childCount > 0)
        {
            visualMeshTransform = transform.GetChild(0);
        }
    }

    void Update()
    {
        if (!controlEnabled)
        {
            velocity = Vector3.zero;
            return;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 moveInput = new Vector3(moveX, 0f, moveZ);

        // 🌟 THE CAMERA ORBIT FIX:
        // If you are pressing Left/Right (A/D) and NOT moving forward, 
        // gently rotate the main camera transform around the character so the viewpoint turns!
        if (Mathf.Abs(moveX) > 0.1f && cameraTransform != null)
        {
            float orbitDirection = moveX; // Positive for Right, Negative for Left
            cameraTransform.RotateAround(transform.position, Vector3.up, orbitDirection * cameraTurnSpeed * Time.deltaTime);
        }

        if (moveInput.sqrMagnitude > 0.01f && cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = (camForward * moveZ) + (camRight * moveX);
            if (moveDirection.sqrMagnitude > 1f) moveDirection.Normalize();

            // Move the character capsule smoothly along the terrain
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Align the visual 3D art mesh
            if (visualMeshTransform != null)
            {
                Quaternion targetRotation;

                // Moving forward or diagonally forward -> face the heading direction
                if (moveZ > 0.1f)
                {
                    float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                    targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                }
                // Strafing left/right or backing up -> stay locked flush with the turning camera frame
                else
                {
                    targetRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
                }

                visualMeshTransform.rotation = Quaternion.RotateTowards(visualMeshTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Keep the visual mesh looking forward when standing completely still
            if (visualMeshTransform != null && cameraTransform != null)
            {
                Quaternion idleRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
                visualMeshTransform.rotation = Quaternion.RotateTowards(visualMeshTransform.rotation, idleRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void EnableControl()
    {
        controlEnabled = true;
    }

    public void DisableControl()
    {
        controlEnabled = false;
    }
}