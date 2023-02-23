
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_Subtitles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SampleSub;
    [SerializeField] private TextMeshProUGUI subtitleInGameA;
    [SerializeField] private TextMeshProUGUI subtitleInGameB;

    [SerializeField] private TextMeshProUGUI tutoFirstA;
    [SerializeField] private TextMeshProUGUI tutoSecondA;
    [SerializeField] private TextMeshProUGUI tutoFirstB;
    [SerializeField] private TextMeshProUGUI tutoSecondB;

    [Header("Button references")]
    [SerializeField] private Toggle toggleBtn;
    [SerializeField] private TMP_Dropdown themeBtn;
    [SerializeField] private Slider sizeBtn;

    private void Start()
    {
        LoadValues();
    }

    private void LoadValues()
    {
        // Debug.Log($"Loading subtitles settings: on {Atone_SettingsSaveAndLoadHandler.subtitles_areOn}, theme {Atone_SettingsSaveAndLoadHandler.subtittles_theme}, size {Atone_SettingsSaveAndLoadHandler.subtitles_size}");
        
        if(toggleBtn) {toggleBtn.isOn = Atone_SettingsSaveAndLoadHandler.subtitles_areOn == 1;}
        else {ShowOrHideSubs(Atone_SettingsSaveAndLoadHandler.subtitles_areOn == 1);}
        
        if(themeBtn) {themeBtn.value = Atone_SettingsSaveAndLoadHandler.subtittles_theme;}        
        else {ChangeTheme(Atone_SettingsSaveAndLoadHandler.subtittles_theme);}
        
        if(sizeBtn) {sizeBtn.value = Atone_SettingsSaveAndLoadHandler.subtittles_theme;}
        else {ChangeSize(Atone_SettingsSaveAndLoadHandler.subtitles_size);}
    }

    public void ShowOrHideSubs(bool show)
    {
        SampleSub.enabled = show;
        subtitleInGameA.enabled = show;
        subtitleInGameB.enabled = show;
        Atone_SettingsSaveAndLoadHandler.subtitles_areOn = show? 1 : 0;
    }

    public void ChangeTheme(int value)
    {
        switch (value)
        {
            case 0:
                ApplyTheme(Color.white, "#FFFFFF00");
                break;
            case 1:
                ApplyTheme(Color.white, "#00000050");
                break;
            case 2:
                ApplyTheme(Color.yellow, "#00000050");
                break;
            default:
                ApplyTheme(Color.white, "#FFFFFF00");
                break;
        }
        Atone_SettingsSaveAndLoadHandler.subtittles_theme = value;
    }

    void ApplyTheme(Color textColor, string bgColor)
    {
        // Debug.Log("Applying theme");
        string str1 = SampleSub.text;
        string str2 = subtitleInGameA.text;
        string str3 = subtitleInGameB.text;

        string str4 = tutoFirstA.text;
        string str5 = tutoSecondA.text;
        string str6 = tutoFirstB.text;
        string str7 = tutoSecondB.text;

        Regex rich = new Regex(@"<[^>]*>");

        if (rich.IsMatch(str1) && rich.IsMatch(str2))
        {
            str1 = rich.Replace(str1, string.Empty);
            str2 = rich.Replace(str2, string.Empty);
            str3 = rich.Replace(str3, string.Empty);
            str4 = rich.Replace(str4, string.Empty);
            str5 = rich.Replace(str5, string.Empty);
            str6 = rich.Replace(str6, string.Empty);
            str7 = rich.Replace(str7, string.Empty);
        }
        SampleSub.color = textColor;
        SampleSub.text = "<mark="+ bgColor + " padding='50,50,0,0'>" + str1 + "</mark>";

        subtitleInGameA.color = textColor;
        subtitleInGameA.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str2 + "</mark>";

        subtitleInGameB.color = textColor;
        subtitleInGameB.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str3 + "</mark>";

        tutoFirstA.color = textColor;
        tutoFirstA.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str4 + "</mark>";

        tutoSecondA.color = textColor;
        tutoSecondA.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str5 + "</mark>";

        tutoFirstB.color = textColor;
        tutoFirstB.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str6 + "</mark>";

        tutoSecondB.color = textColor;
        tutoSecondB.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str7 + "</mark>";
    }

    public void ChangeSize(float value)
    {
        int[] sizeSub = { 30, 33, 36, 39, 42 };
        int[] sizeTuto = { 37, 40, 43, 47, 50 };

        SampleSub.fontSize = sizeSub[(int)value];
        subtitleInGameA.fontSize = sizeSub[(int)value];
        subtitleInGameB.fontSize = sizeSub[(int)value];

        tutoFirstA.fontSize = sizeTuto[(int)value];
        tutoSecondA.fontSize = sizeTuto[(int)value];
        tutoFirstB.fontSize = sizeTuto[(int)value];
        tutoSecondB.fontSize = sizeTuto[(int)value];

        Atone_SettingsSaveAndLoadHandler.subtitles_size = value;
    }
}
