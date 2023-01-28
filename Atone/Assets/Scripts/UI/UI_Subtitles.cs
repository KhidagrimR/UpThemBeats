using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;


public class UI_Subtitles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SampleSub;


    public void ShowOrHideSubs(bool show)
    {
        SampleSub.enabled = show;
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
    }

    void ApplyTheme(Color textColor, string bgColor)
    {
        string str = SampleSub.text;
        Regex rich = new Regex(@"<[^>]*>");

        if (rich.IsMatch(str))
        {
            str = rich.Replace(str, string.Empty);
        }
        SampleSub.color = textColor;
        SampleSub.text = "<mark="+ bgColor + " padding='50,50,0,0'>" + str + "</mark>";
    }

    public void ChangeSize(float value)
    {
        int[] size = { 18, 23, 28, 33, 38 };
        SampleSub.fontSize = size[(int)value];
    }
}
