
using TMPro;
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

    [Header("Button references")]
    [SerializeField] private Toggle toggleBtn;
    [SerializeField] private TMP_Dropdown styleBtn;
    [SerializeField] private TMP_Dropdown colourBtn;
    [SerializeField] private Slider opacityBtn;

    private Color colorPicked = Color.white;
    private float opacityPicked = 1;

    private void Start()
    {
        LoadValues();
    }

    private void LoadValues()
    {
        // Debug.Log($"Loading crosshair settings: on {Atone_SettingsSaveAndLoadHandler.crosshair_isOn}, style {Atone_SettingsSaveAndLoadHandler.crosshair_style}, colour {Atone_SettingsSaveAndLoadHandler.crosshair_colour}, opacity {Atone_SettingsSaveAndLoadHandler.crosshair_opacity}");
        
        if(toggleBtn) {toggleBtn.isOn = Atone_SettingsSaveAndLoadHandler.crosshair_isOn == 1;}
        else {ShowOrHideCrosshair(Atone_SettingsSaveAndLoadHandler.crosshair_isOn == 1);}
        
        if(styleBtn) {styleBtn.value = Atone_SettingsSaveAndLoadHandler.crosshair_style;}
        else {ChangeCrosshair(Atone_SettingsSaveAndLoadHandler.crosshair_style);}
        
        if(colourBtn) {colourBtn.value = Atone_SettingsSaveAndLoadHandler.crosshair_colour;}
        else {ChangeColor(Atone_SettingsSaveAndLoadHandler.crosshair_colour);}
        
        if(opacityBtn) {opacityBtn.value = Atone_SettingsSaveAndLoadHandler.crosshair_opacity;}
        else {ChangeOpacity(Atone_SettingsSaveAndLoadHandler.crosshair_opacity);}
    }

    // Need to affect the object
    public void ShowOrHideCrosshair(bool show)
    {
        Atone_SettingsSaveAndLoadHandler.crosshair_isOn = show? 1 : 0;
    }

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

        Atone_SettingsSaveAndLoadHandler.crosshair_style = value;
    }

    GameObject ApplyCrosshair(GameObject crosshair, GameObject newStyle)
    {
        foreach (Transform child in crosshair.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GameObject clone = Instantiate(newStyle, new Vector3(0, 0, 1), Quaternion.identity);
        clone.SetActive(true);
        ApplyColor(clone, colorPicked);
        ApplyOpacity(clone, opacityPicked);
        clone.transform.SetParent(crosshair.transform);
        return clone;
    }

    public void ChangeColor(int value)
    {
        switch (value)
        {
            case 0:
                colorPicked = Color.white;
                ApplyColor(CrosshairGroup, Color.white);
                ApplyColor(RealCrosshair, Color.white);
                break;
            case 1:
                colorPicked = Color.red;
                ApplyColor(CrosshairGroup, Color.red);
                ApplyColor(RealCrosshair, Color.red);
                break;
            case 2:
                colorPicked = Color.yellow;
                ApplyColor(CrosshairGroup, Color.yellow);
                ApplyColor(RealCrosshair, Color.yellow);
                break;
            case 3:
                colorPicked = Color.cyan;
                ApplyColor(CrosshairGroup, Color.cyan);
                ApplyColor(RealCrosshair, Color.cyan);
                break;
            case 4:
                colorPicked = Color.magenta;
                ApplyColor(CrosshairGroup, Color.magenta);
                ApplyColor(RealCrosshair, Color.magenta);
                break;
            default:
                colorPicked = Color.white;
                ApplyColor(CrosshairGroup, Color.white);
                ApplyColor(RealCrosshair, Color.white);
                break;
        }
        Atone_SettingsSaveAndLoadHandler.crosshair_colour = value;
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
        opacityPicked = value;
        ApplyOpacity(CrosshairGroup, value);
        ApplyOpacity(RealCrosshair, value);

        Atone_SettingsSaveAndLoadHandler.crosshair_opacity = value;
    }

    void ApplyOpacity(GameObject crosshair, float value)
    {
        Image[] elements = crosshair.GetComponentsInChildren<Image>();
        foreach (Image child in elements)
        {
            Color newColor = new Color(child.color.r, child.color.g, child.color.b, value);
            child.color = newColor;
        }
    }
}
