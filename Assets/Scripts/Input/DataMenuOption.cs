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
            this.gameObject.SetActive(false);
        }
        if (type == DataOptionType.HideBillboard)
        {
            if (Data.IsHidden)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        if (type == DataOptionType.ShowBillboard)
        {
            if (Data.IsHidden)
            {
                Hide();
            }
            else
            {
                Show();
            }
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
                Hide();
                //TODO show other
                break;

            case DataOptionType.ShowBillboard:
                Data.ShowBillboard();
                Hide();
                break;

            case DataOptionType.SwitchView:
                SwitchView(); // Not in Data Manager because this is related to the scene manager and view loader
                break;
        }
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    private void SwitchView()
    {
        OrbitScalePointOfInterest switchPoi = ViewLoader.Instance.GetCurrentContent().GetComponentInChildren<OrbitScalePointOfInterest>(true);
        switchPoi.callOrbitSelect();
    }
}