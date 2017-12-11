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

public class ToolMenuOption : MenuOption
{
    public ToolOptionType toolType;

    //Reuse tool script
    private Tool tool;
    private Button orButton; // Use tool or button to perform the action;

    private void Awake()
    {
        //TODO find better way to initialize, but we use this method for now
        if(toolType == ToolOptionType.Grab)
        {
            orButton = new Button();
            orButton.type = this.OptionToButton();
        }
        else
        {
            tool = new Tool();
            tool.type = this.OptionToTool();
        }
        
    }

    public override void OptionAction()
    {
        if(tool == null)
        {
            // If there is no tool registerer it must be a button
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
