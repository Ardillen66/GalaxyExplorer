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

    public void ShowAvailableData()
    {
        throw new NotImplementedException();
    }

    internal void HideBillboard()
    {
        IsHidden = true;
        throw new NotImplementedException();
    }

    internal void ShowBillboard()
    {
        IsHidden = false;
        throw new NotImplementedException();
    }
}