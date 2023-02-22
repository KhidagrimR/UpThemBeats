using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSelectableObjectTxtSecondPart : MonoBehaviour
{
    public Selectable themList;
    public Selectable next;
    public Selectable back;
    public void OnEnable() {

        themList.navigation = NavigationInitialization.SetSelectObjectNavigation(themList, next, next, null, null);
        next.navigation = NavigationInitialization.SetSelectObjectNavigation(next, themList, themList, back, back);
        back.navigation = NavigationInitialization.SetSelectObjectNavigation(back, themList, themList, next, next);

    }
}
