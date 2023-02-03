using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject CrosshairGroup;
    [SerializeField] private GameObject RealCrosshair;

    public void ChangeColor(int value)
    {
        switch (value)
        {
            case 0:
                ApplyColor(Color.white);
                break;
            case 1:
                ApplyColor(Color.red);
                break;
            case 2:
                ApplyColor(Color.yellow);
                break;
            case 3:
                ApplyColor(Color.cyan);
                break;
            case 4:
                ApplyColor(Color.magenta);
                break;
            default:
                ApplyColor(Color.white);
                break;
        }
    }

    void ApplyColor(Color color)
    {
        Image[] elements = CrosshairGroup.GetComponentsInChildren<Image>();
        foreach (Image child in elements)
        {
            if (!child.name.Contains("black")) {
                child.color = color;
            }
        }
    }

    public void ChangeOpacity(float value)
    {
        Image[] elements = CrosshairGroup.GetComponentsInChildren<Image>();
        foreach (Image child in elements)
        {
            Color newColor = new Color(child.color.r, child.color.g, child.color.b, value);
            
            child.color = newColor;
            print(child.color.a);
        }
    }
}
