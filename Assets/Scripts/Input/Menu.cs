using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;
using System;
using System.Linq;

public enum MenuType
{
    Navigation,
    Data,
    Tools
}

public class Menu : GazeSelectionTarget, IFadeTarget
{
    private static bool hasGaze = false;

    private static bool IsNavigating = false;

    // Options for a menu will, be kept in the list and called accordingly
    private List<MenuOption> MenuOptions = new List<MenuOption>();
    private MenuOption SelectedOption;

    // Values for the name gameobject
    public GameObject NameObject;
    private TextMesh nameRenderer;
    public Color DefaultNameColor;
    public Color HighlightNameColor;

    public Material DefaultMaterial; // When not looking at it
    private Dictionary<string, float> defaultMaterialDefaults = new Dictionary<string, float>();
    private GameObject previewText;
    public Material SelectedMaterial; // When pointing at it
    private Dictionary<string, float> selectedMaterialDefaults = new Dictionary<string, float>();

    public MenuType type;
    
    private MeshRenderer meshRenderer;
    
    // Fade target attributes. Code Recycled from Tool/Button equivalents
    private float currentOpacity = 1;
    
    public float Opacity
    {
        get
        {
            return currentOpacity;
        }

        set
        {
            currentOpacity = value;

            ApplyOpacity(DefaultMaterial, value);
            ApplyOpacity(SelectedMaterial, value);
        }
    }

    private void ApplyOpacity(Material material, float value)
    {
        value = Mathf.Clamp01(value);

        if (material)
        {
            material.SetFloat("_TransitionAlpha", value);
            material.SetInt("_SRCBLEND", value < 1 ? (int)UnityEngine.Rendering.BlendMode.SrcAlpha : (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DSTBLEND", value < 1 ? (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha : (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWRITE", value < 1 ? 0 : 1);
        }
    }

    private void CacheMaterialDefaultAttributes(ref Dictionary<string, float> dict, Material mat)
    {
        dict.Add("_TransitionAlpha", mat.GetFloat("_TransitionAlpha"));
        dict.Add("_SRCBLEND", (float)mat.GetInt("_SRCBLEND"));
        dict.Add("_DSTBLEND", (float)mat.GetInt("_DSTBLEND"));
        dict.Add("_ZWRITE", (float)mat.GetInt("_ZWRITE"));
    }

    private void RestoreMaterialDefaultAttributes(ref Dictionary<string, float> dict, Material mat)
    {
        mat.SetFloat("_TransitionAlpha", dict["_TransitionAlpha"]);
        mat.SetInt("_SRCBLEND", (int)dict["_SRCBLEND"]);
        mat.SetInt("_DSTBLEND", (int)dict["_DSTBLEND"]);
        mat.SetInt("_ZWRITE", (int)dict["_ZWRITE"]);
    }
    

    private void Awake()
    {
        if (DefaultMaterial == null)
        {
            Debug.LogWarning(gameObject.name + " Menu has no active material.");
        }
        else
        {
            CacheMaterialDefaultAttributes(ref defaultMaterialDefaults, DefaultMaterial);
        }

        if (SelectedMaterial == null)
        {
            Debug.LogWarning(gameObject.name + " Menu has no selected material.");
        }
        else
        {
            CacheMaterialDefaultAttributes(ref selectedMaterialDefaults, SelectedMaterial);
        }

        meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogWarning(gameObject.name + " Menu has no renderer.");
        }


        if(NameObject == null)
        {
            Debug.LogWarning(gameObject.name + "Menu has nu associated name object");
        }

        nameRenderer = NameObject.GetComponent<TextMesh>();

        if(DefaultNameColor == null) //TODO always false, find better way to check for unasigned color
        {
            Debug.LogWarning(gameObject.name + "Menu has no default color for the name");
        }
        else
        {
            HighlightNameColor = new Color(DefaultNameColor.r, DefaultNameColor.g, DefaultNameColor.b); // Same color, but alpha = 1
        }
        
    }

    private void Start()
    {
        // Get all options assigned from unity
        MenuOption[] options = GetComponentsInChildren<MenuOption>(true);
        VoiceCommands = new string[options.Length];
        int idx = 0; //We need to keep track of an array index to add voicecommands
        foreach(MenuOption option in options)
        {
            MenuOptions.Add(option);
            VoiceCommands[idx++] = option.VoiceCommand;
        }

        previewText = this.gameObject.transform.Find("UI").Find("UITextPrefab").gameObject; // retrieve the preview text
        previewText.SetActive(false); // Ensure the preview is hidden by default
        

    }

    private void OnDestroy()
    { 
        //clear shader defaults cahce
        RestoreMaterialDefaultAttributes(ref defaultMaterialDefaults, DefaultMaterial);
        RestoreMaterialDefaultAttributes(ref selectedMaterialDefaults, SelectedMaterial);
    }

    /**
     * Called when user looks at menu
     */
    public void ShowPreview()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound
            previewText.SetActive(true);
            nameRenderer.color = HighlightNameColor;
            
        }
    }

    public void HidePreview()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound
            //meshRenderer.material = DefaultMaterial;
            previewText.SetActive(false);
            if (!IsNavigating)
            {
                nameRenderer.color = DefaultNameColor;
            } 
        }
    }

    /**
     * Called when user starts a Navigation gesture
     **/
    public void ShowMenu()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound
            meshRenderer.material = SelectedMaterial;
            nameRenderer.color = HighlightNameColor;
            foreach(MenuOption opt in MenuOptions)
            {
                opt.ShowOption();
            }
        }
    }

    public void HideMenu()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound
            meshRenderer.material = DefaultMaterial;
            nameRenderer.color = DefaultNameColor;
            foreach(MenuOption opt in MenuOptions)
            {
                opt.HideOption();
            }
        }
    }

    public override void OnGazeSelect()
    {
        hasGaze = true;
        if (!IsNavigating)
        {
            
            ShowPreview();
        }
        
    }

    public override void OnGazeDeselect()
    {
        hasGaze = false;
        if (!IsNavigating)
        {
            HidePreview();
        }
    }

    /**
     * Extract the index from a position if we order options in a 3x3 grid as such:
     * 0 is the startingPositoion, set to be (0,0,0) when a Navigation gesture starts
     * -------
     * |1|2|3|
     * -------
     * |8|0|4|
     * -------
     * |7|6|5|
     * -------
     * */
    private int OptionIndex(Vector3 position)
    {
        float xPos = position.x;
        float yPos = position.y;
        if (xPos < -0.25)
        {
            if (yPos > 0.25) return 1;
            else if (yPos < -0.25) return 7;
            else return 8;
        }
        else if (xPos > 0.25)
        {
            if (yPos > 0.25) return 3;
            else if (yPos < -0.25) return 5;
            else return 4;
        }
        else
        {
            if (yPos > 0.25) return 2;
            else if (yPos < -0.25) return 6;
            else return 0;
        }
    }

    /**
     * Retrieves a MenuOption given the current position of the user's hand
     * Note: Assumes pre-determined indices for now according to the grid structure described above. Should be improved on in later versions
     * */
    private MenuOption GetSelectedOption(Vector3 sel)
    {
        try
        {
            return MenuOptions.Single<MenuOption>(opt => (opt.GridIndex.Contains(OptionIndex(sel)) && !opt.IsHidden()));
        }
        catch (InvalidOperationException)
        {
            return null; // return null if no option with index at given grid position
        }
       
        //return MenuOptions[OptionIndex(sel)];
    }
    

    public override bool OnNavigationStarted(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        if (hasGaze)
        {
            IsNavigating = true;
            HidePreview(); // hide the preview menu wihch should be active when gaze selected
            ShowMenu();
            return true;
        }
        return false;
    }

    public override bool OnNavigationUpdated(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        if (IsNavigating)
        {
            if(SelectedOption != null)
            {
                SelectedOption.RemoveHighlight();
            }
            SelectedOption = GetSelectedOption(relativePosition);
            SelectedOption.Highlight();
            return true;
        }
        return false;
    }

    public override bool OnNavigationCompleted(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        HideMenu();
        if (IsNavigating)
        {
            IsNavigating = false;
            //Select currently higlighted option and activate callback
            if (SelectedOption == null)
            {
                SelectedOption = GetSelectedOption(relativePosition);
            }
            SelectedOption.OptionAction(); // Perform action from this option
            SelectedOption.RemoveHighlight();
            
            SelectedOption = null;
            return true;
        }
        
        return false;
    }

    public override bool OnNavigationCanceled(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        HideMenu();
        if (IsNavigating)
        {
            if(SelectedOption != null)
            {
                SelectedOption.RemoveHighlight();
            }
            IsNavigating = false;
            SelectedOption = null;
            
            return true;
        }
        return false;
    }

    protected override void VoiceCommandCallback(string command)
    {
        if (!TransitionManager.Instance.InTransition)
        {

            foreach(MenuOption opt in MenuOptions)
            {
                if (command.Equals(opt.VoiceCommand))
                {
                    opt.OptionAction();
                }
            }
        }
    }

}
