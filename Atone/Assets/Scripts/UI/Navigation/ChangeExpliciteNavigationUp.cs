using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeExpliciteNavigationUp : MonoBehaviour
{

    public void ChangeSelectableDownObject(GameObject selectableDownObjact) {
        
        UnityEngine.UI.Navigation navigation = gameObject.GetComponent<Selectable>().navigation;
        navigation.selectOnUp = selectableDownObjact.GetComponent<Selectable>();
        gameObject.GetComponent<Selectable>().navigation = navigation;
    }

}
