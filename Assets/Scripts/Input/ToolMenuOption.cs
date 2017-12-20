using System;
using UnityEngine;

public enum ToolOptionType
{
    Grab,
    Rotate,
    Reset,
    Zoom,
    Tilt

}

/*
 * Options used in the tools menu
 * */
public class ToolMenuOption : MenuOption
{
    public ToolOptionType toolType;

    //Reuse tool script
    private Tool tool;
    private Button orButton; // Use tool or button to perform the action;

    private void Awake()
    {
        //TODO find better way to initialize, but we use this method for now
        if(toolType == ToolOptionType.Grab) // Assign tool if functionality was originally in a tool 
        {
            orButton = gameObject.AddComponent<Button>();
            orButton.type = this.OptionToButton();
        }
        else // Otherwise it is a button
        {
            tool = gameObject.AddComponent<Tool>();
            tool.type = this.OptionToTool();
        }
        
    }

    /*
     * Here we just call on the tool or button that contained the requested functionality in th original project
     * */
    public override void OptionAction()
    {
        if(tool == null)
        {
            // If there is no tool registered it must be a button
            if(orButton == null)
            {
                Debug.LogError(this + "No tool or button registered");
                //TODO error handling
            }
            else
            {
                orButton.ButtonAction();
            }
        }
        else
        {
            tool.ToolAction();
        }
    }

    /*
     * Converts enum types we use in this class to equivalent types in the Tool script from the original project
     * */
    private ToolType OptionToTool()
    {
        ToolType tType = ToolType.None;
        switch (toolType)
        {
            case ToolOptionType.Reset:
                tType = ToolType.Reset;
                break;
            case ToolOptionType.Rotate:
                tType = ToolType.Rotate;
                break;
            case ToolOptionType.Tilt:
                tType = ToolType.Pan;
                break;
            case ToolOptionType.Zoom:
                tType = ToolType.Zoom;
                break;
            default:
                Debug.LogError("Invalid Tool Type");
                break;

        }
        return tType;
    }

    /*
     * Coverts from the internally used type enum to the equivalent in the original Button script
     * */
    private ButtonType OptionToButton()
    {
        ButtonType bType = ButtonType.None;
        switch (toolType)
        {
            case ToolOptionType.Grab:
                bType = ButtonType.MoveCube;
                break;
            default:
                Debug.LogError("Invalid button type");
                break;
        }
        return bType;
    }
}
