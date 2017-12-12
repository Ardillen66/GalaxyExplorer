using System;
using UnityEngine;

public enum NavOptionType
{
    GoToSelected,
    SelectDestination,
    GoBack
    //TODO: include possible actions for this menu here

}

public class NavMenuOption : MenuOption
{
    public NavOptionType type;

    public override void OptionAction()
    {
        switch (type)
        {
            case NavOptionType.GoBack:
                ViewLoader.Instance.GoBack();
                break;
            case NavOptionType.GoToSelected:
                GoToSelectedPOI();
                break;
            case NavOptionType.SelectDestination:
                ShowDestinationList();
                break;
        }
    }

    /*
     * Show a list of possible destinations that can be reached from the current scene
     * Allow navigation of the possible destinations in a list structure that shows hierarchy of scenes
     * Note: could not be implemented in time
     * */
    private void ShowDestinationList()
    {
        throw new NotImplementedException();
    }

    /*
     * Identifies the selected POI's in the scene and goes to the first encountered one.
     * Note: Should ask to choose a POI if more than one is selected in the future
     * */
    private void GoToSelectedPOI()
    {
        PointOfInterest[] pois = ViewLoader.Instance.GetHeroView().GetComponentsInChildren<PointOfInterest>(true);
        foreach(PointOfInterest point in pois)
        {
            // For now we just go to the first selected POI we encounter
            if (point.isSelected)
            {
                point.GoToScene();
                return;
            }
        }
    }
}
