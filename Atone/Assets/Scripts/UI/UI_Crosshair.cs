using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Crosshair : MonoBehaviour
{
    [SerializeField] private GameObject CrosshairGroup;
    [SerializeField] private GameObject RealCrosshair;

    [SerializeField] private GameObject dotStyle;
    [SerializeField] private GameObject plusStyle;
    [SerializeField] private GameObject basicStyle;
    [SerializeField] private GameObject shorterStyle;

    public void ChangeCrosshair(int value)
    {
        GameObject clone;
        switch (value)
        {
            case 0:
                clone = ApplyCrosshair(CrosshairGroup, dotStyle);
                clone.transform.position += new Vector3(0, 90, 0);
                ApplyCrosshair(RealCrosshair, dotStyle);
                break;
            case 1:
                clone = ApplyCrosshair(CrosshairGroup, plusStyle);
                clone.transform.position += new Vector3(0, 90, 0);
                ApplyCrosshair(RealCrosshair, plusStyle);
                break;
            case 2:
                clone = ApplyCrosshair(CrosshairGroup, basicStyle);
                clone.transform.position += new Vector3(0, 90, 0);
                ApplyCrosshair(RealCrosshair, basicStyle);
                break;
            case 3:
                clone = ApplyCrosshair(CrosshairGroup, shorterStyle);
                clone.transform.position += new Vector3(0, 90, 0);
                ApplyCrosshair(RealCrosshair, shorterStyle);
                break;
            default:
                clone = ApplyCrosshair(CrosshairGroup, dotStyle);
                clone.transform.position += new Vector3(0, 90, 0);
                ApplyCrosshair(RealCrosshair, dotStyle);
                break;
        }
    }

    GameObject ApplyCrosshair(GameObject crosshair, GameObject newStyle)
    {
        foreach (Transform child in crosshair.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GameObject clone = Instantiate(newStyle, new Vector3(0, 0, 1), Quaternion.identity);
        clone.SetActive(true);
        clone.transform.SetParent(crosshair.transform);
        return clone;
    }

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
