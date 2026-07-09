using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomizationManager : MonoBehaviour
{
    public static CustomizationManager Instance;
    public bool customizationComplete = false;

    [Header("Character Selection")]
    public GameObject brotherPreview;
    public GameObject sisterPreview;
    public bool customizingBrother = true;

    [Header("Clothing Options")]
    public List<Material> shirtMaterials = new List<Material>();
    public List<Material> pantsMaterials = new List<Material>();
    public List<Material> shoesMaterials = new List<Material>();
    public List<Color> hairColors = new List<Color>();

    // 👥 SEPARATE TRACKING INDEXES FOR BOTH CHARACTERS
    private int brotherChoiceIndex = 0;
    private int sisterChoiceIndex = 0;
    private string brotherLastType = "shirt";
    private string sisterLastType = "shirt";

    [Header("Preview Model Parts")]
    public SkinnedMeshRenderer previewShirt;
    public SkinnedMeshRenderer previewPants;
    public SkinnedMeshRenderer previewShoes;
    public SkinnedMeshRenderer previewHair;

    [Header("UI Interactive Elements")]
    public Text characterNameText;
    public Button nextShirtBtn;
    public Button nextPantsBtn;
    public Button nextShoesBtn;
    public Button startButton;

    public static ClothingData brotherClothing = new ClothingData();
    public static ClothingData sisterClothing = new ClothingData();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (startButton) startButton.onClick.AddListener(CompleteCustomization);
        UpdatePreviewVisuals();
    }

    public void CycleShirt()
    {
        if (shirtMaterials.Count == 0) return;

        if (customizingBrother)
        {
            brotherChoiceIndex = (brotherChoiceIndex + 1) % shirtMaterials.Count;
            brotherLastType = "shirt";
        }
        else
        {
            sisterChoiceIndex = (sisterChoiceIndex + 1) % shirtMaterials.Count;
            sisterLastType = "shirt";
        }
        UpdatePreviewVisuals();
    }

    public void CyclePants()
    {
        if (pantsMaterials.Count == 0) return;

        if (customizingBrother)
        {
            brotherChoiceIndex = (brotherChoiceIndex + 1) % pantsMaterials.Count;
            brotherLastType = "pants";
        }
        else
        {
            sisterChoiceIndex = (sisterChoiceIndex + 1) % pantsMaterials.Count;
            sisterLastType = "pants";
        }
        UpdatePreviewVisuals();
    }

    public void CycleShoes()
    {
        if (shoesMaterials.Count == 0) return;

        if (customizingBrother)
        {
            brotherChoiceIndex = (brotherChoiceIndex + 1) % shoesMaterials.Count;
            brotherLastType = "shoes";
        }
        else
        {
            sisterChoiceIndex = (sisterChoiceIndex + 1) % shoesMaterials.Count;
            sisterLastType = "shoes";
        }
        UpdatePreviewVisuals();
    }

    void UpdatePreviewVisuals()
    {
        // 1. Update the Brother Capsule using his independent choices
        if (brotherPreview != null)
        {
            Renderer brotherRenderer = brotherPreview.GetComponent<Renderer>();
            if (brotherRenderer != null)
            {
                if (brotherLastType == "shirt" && shirtMaterials.Count > brotherChoiceIndex)
                    brotherRenderer.material = shirtMaterials[brotherChoiceIndex];
                else if (brotherLastType == "pants" && pantsMaterials.Count > brotherChoiceIndex)
                    brotherRenderer.material = pantsMaterials[brotherChoiceIndex];
                else if (brotherLastType == "shoes" && shoesMaterials.Count > brotherChoiceIndex)
                    brotherRenderer.material = shoesMaterials[brotherChoiceIndex];
            }
        }

        // 2. Update the Sister Capsule using her independent choices
        if (sisterPreview != null)
        {
            Renderer sisterRenderer = sisterPreview.GetComponent<Renderer>();
            if (sisterRenderer != null)
            {
                if (sisterLastType == "shirt" && shirtMaterials.Count > sisterChoiceIndex)
                    sisterRenderer.material = shirtMaterials[sisterChoiceIndex];
                else if (sisterLastType == "pants" && pantsMaterials.Count > sisterChoiceIndex)
                    sisterRenderer.material = pantsMaterials[sisterChoiceIndex];
                else if (sisterLastType == "shoes" && shoesMaterials.Count > sisterChoiceIndex)
                    sisterRenderer.material = shoesMaterials[sisterChoiceIndex];
            }
        }
    }

    void CompleteCustomization()
    {
        // Save the choices into our global data right before shifting scenes
        brotherClothing.shirtIndex = brotherChoiceIndex;
        sisterClothing.shirtIndex = sisterChoiceIndex;

        customizationComplete = true;
        SceneManager.LoadScene("GameScene");
    }

    public void SetActiveCharacter(bool lookAtBrother)
    {
        customizingBrother = lookAtBrother;

        if (brotherPreview != null) brotherPreview.SetActive(true);
        if (sisterPreview != null) sisterPreview.SetActive(true);

        // Instantly forces both meshes to display their own correct colors!
        UpdatePreviewVisuals();
        Debug.Log("Switched customization target. Customizing Brother: " + customizingBrother);
    }

    [System.Serializable]
    public class ClothingData
    {
        public int shirtIndex;
        public int pantsIndex;
        public int shoesIndex;
        public int hairColorIndex;
    }
}