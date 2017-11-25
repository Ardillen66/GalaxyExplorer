using System;
using UnityEngine;

public enum ToolOptionType
{
    Grab,
    Rotate,
    Reset,
    Zoom,
    Tilt
    //TODO: include possible actions for this menu here

}

public class ToolMenuOption : MenuOption
{
    ToolOptionType toolType;

    //Reuse tool script
    Tool tool;

    private void Awake()
    {
        tool = new Tool(); //TODO correctly initialize script and modify to ignore gaze and such
    }

    public override void OptionAction()
    {
        tool.Select();
        //TODO: How to unselect
    }
}
