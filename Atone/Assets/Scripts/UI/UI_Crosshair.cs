using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject CrosshairGroup;
    //apply to the real one

    public void ChangeColor(float value)
    {
        if(value <= 255)
        {
            ApplyColor(value, 0, 0);
        } else if(value <= 510)
        {
            ApplyColor(255, 255 - value, 0);
        } else
        {
            ApplyColor(255, 255, 510 - value);
        }
    }

    void ApplyColor(float red, float green, float blue)
    {

        foreach (Renderer child in GetComponentsInChildren<Renderer>())
        {
            print("hello");
            print(child.name);
            child.material.color = new Color(red, green, blue);
        }
    }

    public void ChangeOpacity(float value)
    {
        foreach (Renderer child in CrosshairGroup.GetComponentsInChildren<Renderer>())
        {
            //TODO
        }
    }
}
