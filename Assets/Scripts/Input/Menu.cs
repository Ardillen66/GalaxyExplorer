using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;

public enum MenuType
{
    Navigation,
    Data,
    Tools
}

public class Menu : GazeSelectionTarget, IFadeTarget
{
    private static bool hasGaze = false;

    public GameObject NameObject;

    public Material DefaultMaterial; // When not looking at it
    public Material DefaultName;
    // TODO cache default attributes
    private Dictionary<string, float> defaultMaterialDefaults = new Dictionary<string, float>();
    public Material HighlightMaterial; // When looking at it
    public Material HighlightName;
    private Dictionary<string, float> highlightMaterialDefaults = new Dictionary<string, float>();
    public Material SelectedMaterial; // When pointing at it
    private Dictionary<string, float> selectedMaterialDefaults = new Dictionary<string, float>();
    public MenuType type;
    
    private bool selected = false;
    private MeshRenderer meshRenderer;
    private MeshRenderer nameRenderer;

    private float currentOpacity = 1;

    // TODO check if we can do it like this?
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
            ApplyOpacity(HighlightMaterial, value);
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

    //Until here

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

        if (HighlightMaterial == null)
        {
            Debug.LogWarning(gameObject.name + " Menu has no highlight material.");
        }
        else
        {
            CacheMaterialDefaultAttributes(ref highlightMaterialDefaults, HighlightMaterial);
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

        nameRenderer = NameObject.GetComponent<MeshRenderer>();
        
    }

    private void Start()
    {
        //TODO startup methods here
        
    }

    private void OnDestroy()
    {
        // TODO eventual additional cleanup
        
        //clear shader cahce
        RestoreMaterialDefaultAttributes(ref defaultMaterialDefaults, DefaultMaterial);
        RestoreMaterialDefaultAttributes(ref highlightMaterialDefaults, HighlightMaterial);
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
            Debug.Log("Showing preview");
            meshRenderer.material = HighlightMaterial;
            nameRenderer.material = HighlightName;
            
        }
    }

    public void HidePreview()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound
            meshRenderer.material = DefaultMaterial;
            nameRenderer.material = DefaultName;
        }
    }

    /**
     * Called when user starts a Navigation gesture
     **/
    public void ShowMenu()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound and activate children
            meshRenderer.material = SelectedMaterial;
            nameRenderer.material = HighlightName;

        }
    }

    public void HideMenu()
    {
        if (!ToolManager.Instance.IsLocked)
        {
            //TODO add sound and deactivate children
            meshRenderer.material = DefaultMaterial;
            nameRenderer.material = DefaultName;

        }
    }

    //TODO delegate action to currently selected tool/button
    public void OptionAction()
    {

    }

    public override void OnGazeSelect()
    {
        Debug.Log("Gaze detected");
        hasGaze = true;
        ShowPreview();
    }

    public override void OnGazeDeselect()
    {
        hasGaze = false;
        HidePreview();
    }

    public override bool OnNavigationStarted(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO: Show menu buttons when holding select, potential checks to return true when correctly handeled
        ShowMenu();
        return true;
    }

    public override bool OnNavigationUpdated(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO: Higlight currently selected option
        return true;
    }

    public override bool OnNavigationCompleted(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO: Select currently higlighted option and activate callback
        HideMenu();
        return true;
    }

    public override bool OnNavigationCanceled(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO restore default/highlight state (dependent on gaze)
        HideMenu();
        return true;
    }

    protected override void VoiceCommandCallback(string command)
    {
        if (!TransitionManager.Instance.InTransition)
        {
            OptionAction();
        }
    }

}
