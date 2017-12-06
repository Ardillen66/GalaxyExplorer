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
    public DataOptionType type;

    private void Awake()
    {
        if(type == null)
        {
            //TODO find better way to check if type wa correctly assigned from unity
            Debug.LogWarning("Please assign an option type to " + gameObject.name + " Menu Option");
        }
        else if(type == DataOptionType.SwitchView)
        {
            //TODO Find a way to be aware of current view, only show this option for solar system en get access to the old POI that had this functionality
        }
    }

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