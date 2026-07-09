using UnityEngine;

public class CharacterSwapManager : MonoBehaviour
{
    [Header("Character References")]
    public ThirdPersonController sisterController;
    public ThirdPersonController brotherController;

    [Header("Camera Reference")]
    public Transform mainCameraTransform;

    [Header("Custom Over-The-Shoulder Settings")]
    public float cameraHeight = 2.0f;
    public float cameraDistance = 3.5f;
    public float rightOffset = 0.5f;

    private bool isControllingSister = true;
    private Transform activeTarget;

    void Start()
    {
        if (sisterController == null || brotherController == null)
        {
            Debug.LogError("Please assign Sister and Brother in the Inspector!");
            return;
        }

        if (mainCameraTransform == null && Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }

        if (mainCameraTransform != null)
        {
            mainCameraTransform.SetParent(null);
        }

        SetCharacterState(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isControllingSister = !isControllingSister;
            SetCharacterState(isControllingSister);
        }
    }

    void LateUpdate()
    {
        if (mainCameraTransform != null && activeTarget != null)
        {
            // Calculate standard over-the-shoulder placement relative to target heading
            Vector3 targetPosition = activeTarget.position
                                   - (activeTarget.forward * cameraDistance)
                                   + (activeTarget.up * cameraHeight)
                                   + (activeTarget.right * rightOffset);

            // Fast interpolation prevents lag or mathematical drift offsets
            mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, targetPosition, Time.deltaTime * 20f);

            // Forces the camera lens to look right at their upper back/neck area uniformly
            mainCameraTransform.LookAt(activeTarget.position + Vector3.up * 1.5f);
        }
    }

    void SetCharacterState(bool controlSister)
    {
        if (controlSister)
        {
            sisterController.EnableControl();
            brotherController.DisableControl();
            activeTarget = sisterController.transform;

            // 🌟 FORCE CAMERA ALIGNMENT: Snap camera rotation to match the Sister's exact rotation immediately
            if (mainCameraTransform != null)
            {
                mainCameraTransform.rotation = sisterController.transform.rotation;
            }

            Debug.Log("Switched control to: Older Sister");
        }
        else
        {
            sisterController.DisableControl();
            brotherController.EnableControl();
            activeTarget = brotherController.transform;

            // 🌟 FORCE CAMERA ALIGNMENT: Snap camera rotation to match the Brother's exact rotation immediately
            if (mainCameraTransform != null)
            {
                mainCameraTransform.rotation = brotherController.transform.rotation;
            }

            Debug.Log("Switched control to: Younger Brother");
        }
    }
}