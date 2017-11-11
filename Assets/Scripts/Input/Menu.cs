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
    //TODO get children and store them in menu options
    private Dictionary<string, MenuOption> options = new Dictionary<string, MenuOption>();

    public GameObject TooltipObject;

    public Material DefaultMaterial; // When not looking at it
    private Dictionary<string, float> defaultMaterialDefaults = new Dictionary<string, float>();
    public Material HighlightMaterial; // When looking at it
    private Dictionary<string, float> highlightMaterialDefaults = new Dictionary<string, float>();
    public Material SelectedMaterial; // When pointing at it
    private Dictionary<string, float> selectedMaterialDefaults = new Dictionary<string, float>();
    public MenuType type;
    
    private bool selected = false;
    private MeshRenderer meshRenderer;

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
        //TODO cache material attributes
    }

    private void Start()
    {
        //TODO hook up to player input manager
    }

    private void OnDestroy()
    {
        // TODO unhook from input manager and clear cahce
    }

    /**
     * Called when user looks at menu
     */
    public void ShowPreview()
    {

    }

    public void HidePreview()
    {

    }

    /**
     * Called when user starts a Navigation gesture
     **/
    public void ShowMenu()
    {

    }

    public void HideMenu()
    {

    }

    //TODO delegate action to currently selected tool/button
    public void OptionAction()
    {

    }

    public override void OnGazeSelect()
    {
        //TODO show related voice commands and menu preview (same?)
        base.OnGazeSelect();
    }

    public override void OnGazeDeselect()
    {
        //TODO restore default state
        base.OnGazeDeselect();
    }

    public override bool OnNavigationStarted(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO: Show menu buttons when holding select
        return base.OnNavigationStarted(source, relativePosition, ray);
    }

    public override bool OnNavigationUpdated(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO: Higlight currently selected option
        return base.OnNavigationUpdated(source, relativePosition, ray);
    }

    public override bool OnNavigationCompleted(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO: Select currently higlighted option
        return base.OnNavigationCompleted(source, relativePosition, ray);
    }

    public override bool OnNavigationCanceled(InteractionSourceKind source, Vector3 relativePosition, Ray ray)
    {
        //TODO restore default/highlight state (dependent on gaze)
        return base.OnNavigationCanceled(source, relativePosition, ray);
    }

    protected override void VoiceCommandCallback(string command)
    {
        if (!TransitionManager.Instance.InTransition)
        {
            OptionAction();
        }
    }

}
