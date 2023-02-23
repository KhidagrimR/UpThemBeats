using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSelectableObjectCrosshairPart : MonoBehaviour
{
    public Selectable changeColor;
    public Selectable changeOpacity;
    public Selectable next;
    public Selectable back;
    public void OnEnable() {

        changeColor.navigation = NavigationInitialization.SetSelectObjectNavigation(changeColor, back, back, null, null);
        changeOpacity.navigation = NavigationInitialization.SetSelectObjectNavigation(changeOpacity, next, next, null, null);
        next.navigation = NavigationInitialization.SetSelectObjectNavigation(next, changeOpacity, changeOpacity, back, back);
        back.navigation = NavigationInitialization.SetSelectObjectNavigation(back, changeColor, changeColor, next, next);

    }
}
