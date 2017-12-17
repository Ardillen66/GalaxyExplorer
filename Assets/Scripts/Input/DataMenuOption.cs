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
    //DataManager data
    public DataOptionType type;

    private DataManager Data;

    private void Awake()
    {
        
        if(type == DataOptionType.SwitchView && ViewLoader.Instance.CurrentView != "SolarSystemView")
        {
            this.gameObject.SetActive(false);
        }

    }

    public override void Start()
    {
        base.Start();
        Data = DataManager.Instance;
        if(type == DataOptionType.ShowBillboard)
        {
            HideOption(); //Data is shown by default, so show button is hidden
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
            case DataOptionType.ShowBillboard:
                Data.ToggleData();
                break;

            case DataOptionType.SwitchView:
                SwitchView(); // Not in Data Manager because this is related to the scene manager and view loader
                break;
        }
    }

    private void SwitchView()
    {
        OrbitScalePointOfInterest switchPoi = ViewLoader.Instance.GetCurrentContent().GetComponentInChildren<OrbitScalePointOfInterest>(true);
        switchPoi.callOrbitSelect();
    }
}