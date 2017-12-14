using System;
using UnityEngine;

public enum DataOptionType
{
    SelectData,
    ShowBillboard,
    HideBillboard,
    SwitchView
}

public class DataMenuOption : MenuOption
{
    //DataManager data  TODO make manager to handle showing/hiding data and selecting which type of data to show
    public DataOptionType type;

    private DataManager Data;

    private void Awake()
    {
        Data = DataManager.Instance;
        if(type == DataOptionType.SwitchView && ViewLoader.Instance.CurrentView != "SolarSystemView")
        {
            //TODO only show this option for solar system en get access to the POI that had this functionality
        }
    }

    public override void OptionAction()
    {
        switch (type)
        {
            case DataOptionType.SelectData:
                Data.ShowAvailableData();
                break;

            case DataOptionType.HideBillboard:
                Data.HideBillboard();
                break;

            case DataOptionType.ShowBillboard:
                Data.ShowBillboard();
                break;

            case DataOptionType.SwitchView:
                SwitchView(); // Not in Data Manager because this is related to the scene manager and view loader
                break;
        }
    }

    private void SwitchView()
    {
        throw new NotImplementedException();
    }
}