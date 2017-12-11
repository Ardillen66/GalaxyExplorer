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
                //TODO Load selected scene
                break;
        }
    }
}
