using System;
using UnityEngine;

public enum DataOptionType
{
    SelectData,
    ShowBillboard,
    HideBillboard,
    SwitchView
}

/*
 * Class used for options from the data menu
 * */
public class DataMenuOption : MenuOption
{
    //DataManager data
    public DataOptionType type;

    private DataManager Data;

    public override void Start()
    {
        base.Start();
        Data = DataManager.Instance;
        if(type == DataOptionType.ShowBillboard)
        {
            HideOption(); //Data is shown by default, so show button is hidden
        }

        if (type == DataOptionType.SwitchView && ViewLoader.Instance.CurrentView != "SolarSystemView")
        {
            this.gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (type == DataOptionType.SwitchView && ViewLoader.Instance.CurrentView == "SolarSystemView")
        {
            this.gameObject.SetActive(true);
        }
    }



    public override void OptionAction()
    {
        switch (type)
        {
            case DataOptionType.SelectData:
                Data.ShowAvailableData(); // Show the available destinations
                break;

            case DataOptionType.HideBillboard:
            case DataOptionType.ShowBillboard:
                Data.ToggleData(); // Toggle visibility of data
                break;

            case DataOptionType.SwitchView:
                SwitchView(); // Not in Data Manager because this is related to the scene manager and view loader
                break;
        }
    }

    private void SwitchView()
    {
        OrbitScalePointOfInterest switchPoi = ViewLoader.Instance.GetCurrentContent().GetComponentInChildren<OrbitScalePointOfInterest>(true);
        switchPoi.callOrbitSelect(); // We use the POI from original project to switch between realistic and simplified view
    }
}