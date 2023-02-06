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
                ApplyColor(CrosshairGroup, Color.white);
                ApplyColor(RealCrosshair, Color.white);
                break;
            case 1:
                ApplyColor(CrosshairGroup, Color.red);
                ApplyColor(RealCrosshair, Color.red);
                break;
            case 2:
                ApplyColor(CrosshairGroup, Color.yellow);
                ApplyColor(RealCrosshair, Color.yellow);
                break;
            case 3:
                ApplyColor(CrosshairGroup, Color.cyan);
                ApplyColor(RealCrosshair, Color.cyan);
                break;
            case 4:
                ApplyColor(CrosshairGroup, Color.magenta);
                ApplyColor(RealCrosshair, Color.magenta);
                break;
            default:
                ApplyColor(CrosshairGroup, Color.white);
                ApplyColor(RealCrosshair, Color.white);
                break;
        }
    }

    void ApplyColor(GameObject crosshair, Color color)
    {
        Image[] elements = crosshair.GetComponentsInChildren<Image>();
        foreach (Image child in elements)
        {
            if (!child.name.Contains("black")) {
                child.color = color;
            }
        }
    }

    public void ChangeOpacity(float value)
    {
        ApplyOpacity(CrosshairGroup, value);
        ApplyOpacity(RealCrosshair, value);
    }

    void ApplyOpacity(GameObject crosshair, float value)
    {
        Image[] elements = crosshair.GetComponentsInChildren<Image>();
        foreach (Image child in elements)
        {
            Color newColor = new Color(child.color.r, child.color.g, child.color.b, value);

            child.color = newColor;
            print(child.color.a);
        }
    }
}
