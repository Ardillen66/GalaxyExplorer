using System;
using UnityEngine;

public enum DataOptionType
{
    SelectData,
    ShowBillboard,
    HideBillboard,
    SwitchView
    //TODO: include possible actions for this menu here
}

public class DataMenuOption : MenuOption
{
    //DataManager data  TODO make manager to handle showing/hiding data and selecting which type of data to show
    DataOptionType type;

    public override void OptionAction()
    {
        switch (type)
        {
            case DataOptionType.SelectData:
                //TODO show list of available data
                break;

            case DataOptionType.HideBillboard:
                //TODO hide the data billboard 
                //data.HideBillboard();
                break;

            case DataOptionType.ShowBillboard:
                //TODO show data billboard
                //data.ShowBillboard();
                break;

            case DataOptionType.SwitchView:
                //TODO get POI for switching views and simulate tap
                break;
        }
    }
}