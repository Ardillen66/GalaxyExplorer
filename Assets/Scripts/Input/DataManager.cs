using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class that manages all data aspects of the simulation. 
 * Such as loading/caching of data related to a certain scene and attatching billboards to show data to the objects in a scene
 * If no spoken explanation is available, generates a new audio file from text to speech
 * 
 */
public class DataManager : Singleton<DataManager>
{
    public bool IsHidden = false;

    // Menu options for showing and hiding data
    public DataMenuOption showData;
    public DataMenuOption hideData;

    private void Awake()
    {
        Debug.Log("Data manager initialized");
    }

    /**
     * Method showing a list of data available for the current view
     * Note: Not available for this iteration
     * */
    public void ShowAvailableData()
    {
        throw new NotImplementedException();
    }

    /*
     * Method used to hide the billboard containing data for the current scene
     * */
    internal void HideBillboard()
    {
        IsHidden = true;
        GameObject billboard = ViewLoader.Instance.GetCurrentContent().transform.Find("billboard").gameObject;
        billboard.SetActive(false);
        hideData.HideOption();
        showData.ShowOption();
    }

    /*
     * Method used to show the billboard containing data for the current scene
     * */
    internal void ShowBillboard()
    {
        IsHidden = false;
        GameObject billboard = ViewLoader.Instance.GetCurrentContent().transform.Find("billboard").gameObject;
        billboard.SetActive(true);
        showData.HideOption();
        hideData.ShowOption();
    }

    public void ToggleData()
    {
        if (IsHidden)
        {
            ShowBillboard();
        }
        else
        {
            HideBillboard();
        }
    }
}