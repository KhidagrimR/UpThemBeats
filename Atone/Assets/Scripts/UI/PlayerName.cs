using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerName : Singleton<PlayerName>
{
    public GameObject container;
    public GameObject textError;
    
    [InspectorReadOnly]
    public string name;

    public void DisplayName(string name) {
        this.name = name;
        print(name);
    }


}
