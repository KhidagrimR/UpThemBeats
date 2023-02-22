using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSelectableObjectTxtFirstPart : MonoBehaviour
{
    public Selectable yes;
    public Selectable no;
    public Selectable next;
    public Selectable back;
    public void OnEnable() {
        yes.navigation = NavigationInitialization.SetSelectObjectNavigation(yes, next, no, null, null);
        no.navigation = NavigationInitialization.SetSelectObjectNavigation(no, yes, next, null, null);
        next.navigation = NavigationInitialization.SetSelectObjectNavigation(next, no, yes, back, back);
        back.navigation = NavigationInitialization.SetSelectObjectNavigation(back, no, yes, next, next);

    }
}
