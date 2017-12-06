using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class MenuOption : MonoBehaviour
{
    private static bool Selected = false;

    //Code below is reproduced from the Button clqss and assumes the same meshes and shaders are being used
    public Material DefaultMaterial;
    public Dictionary<string, float> defaultMaterialDefaults = new Dictionary<string, float>();
    public Material HightlightMaterial;
    public Dictionary<string, float> highlightMaterialDefaults = new Dictionary<string, float>();

    public event Action<MenuType> OptionSelected; //TODO also include which option was selected

    private MeshRenderer meshRenderer;

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
            ApplyOpacity(HightlightMaterial, value);
        }
    }

    public string VoiceCommand;// { get; internal set; }

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

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false; // Always start hidden
    }

    public void HideOption()
    {
        meshRenderer.enabled = false;
    }

    public void ShowOption()
    {
        meshRenderer.enabled = true;
        meshRenderer.material = DefaultMaterial;
    }


    //TODO register and manage actions when option is selected
    public abstract void OptionAction();

    /*
     * Highlights this option and register as selected
     * */
    public void Highlight()
    {
        meshRenderer.material = HightlightMaterial;
        Selected = true;
    }

    public void RemoveHighlight()
    {
        meshRenderer.material = DefaultMaterial;
        Selected = false;
    }
}