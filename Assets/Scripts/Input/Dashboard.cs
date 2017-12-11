using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashboard : GazeSelectionTarget, IFadeTarget {

    public bool HasGaze = false;

    public Material DefaultMaterial; // When not looking at it
    // TODO modify this code for this case
    private Dictionary<string, float> defaultMaterialDefaults = new Dictionary<string, float>();
    private GameObject previewText;
    public Material SelectedMaterial; // When pointing at it
    private Dictionary<string, float> selectedMaterialDefaults = new Dictionary<string, float>();

    private float currentOpacity = 1;

    // TODO check if we can do it like this? ie: Hook up materials to provided fader
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
    }

    public override void OnGazeSelect()
    {
        HasGaze = true;
    }

    public override void OnGazeDeselect()
    {
        HasGaze = false;
    }
}
