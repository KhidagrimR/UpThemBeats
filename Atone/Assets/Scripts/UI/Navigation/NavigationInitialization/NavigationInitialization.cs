using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationInitialization : MonoBehaviour
{
    public static UnityEngine.UI.Navigation SetSelectObjectNavigation(Selectable gameObjectToChangeSelectedObject,
                                                                      Selectable objectUp, Selectable objectDown, 
                                                                      Selectable objectLeft, Selectable objectRight) {

        UnityEngine.UI.Navigation navigationGameObject = gameObjectToChangeSelectedObject.navigation;

        navigationGameObject.selectOnUp = objectUp;
        navigationGameObject.selectOnDown = objectDown;
        navigationGameObject.selectOnLeft = objectLeft;
        navigationGameObject.selectOnRight = objectRight;

        gameObjectToChangeSelectedObject.navigation = navigationGameObject;

        return gameObjectToChangeSelectedObject.navigation;
    }
}
