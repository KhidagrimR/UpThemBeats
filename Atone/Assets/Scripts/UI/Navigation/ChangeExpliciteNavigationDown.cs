using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeExpliciteNavigationDown : MonoBehaviour
{
    public List<GameObject> gameObjectsChangeSelectableDownObject;

    public void ChangeSelectableDownObject(GameObject selectableDownObjact) {
        foreach(GameObject go in gameObjectsChangeSelectableDownObject){
            UnityEngine.UI.Navigation navigation = go.GetComponent<Selectable>().navigation;
            navigation.selectOnDown = selectableDownObjact.GetComponent<Selectable>();
            go.GetComponent<Selectable>().navigation = navigation;
        }
    }
}
