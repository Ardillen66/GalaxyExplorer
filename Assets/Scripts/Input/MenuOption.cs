using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class MenuOption : MonoBehaviour
{
    private static bool Selected = false;

    //list of indices to keep track of the position of an option in th menu grid (see Menu for details)
    public List<int> GridIndex;

    //Code below is reproduced from the Button clqss and assumes the same meshes and shaders are being used
    public Material DefaultMaterial;
    public Dictionary<string, float> defaultMaterialDefaults = new Dictionary<string, float>();
    public Material HightlightMaterial;
    public Dictionary<string, float> highlightMaterialDefaults = new Dictionary<string, float>();

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
    private Bounds objectBounds;

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

    public virtual void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false; // Always start hidden

        //Mesh mesh = GetComponent<MeshFilter>().mesh;
        //objectBounds = meshRenderer.bounds;
    }

    public bool IsSelected()
    {
        return Selected;
    }


    //public Bounds GetBounds()
    //{
    //    return objectBounds;
    //}

    public void HideOption()
    {
        meshRenderer.enabled = false;
    }

    public void ShowOption()
    {
        meshRenderer.enabled = true;
        meshRenderer.material = DefaultMaterial;
    }

    public bool IsHidden()
    {
        return meshRenderer.enabled;
    }

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