using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;


public class UI_Subtitles : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SampleSub;
    [SerializeField] private TextMeshProUGUI subtitleInGame;


    public void ShowOrHideSubs(bool show)
    {
        SampleSub.enabled = show;
        subtitleInGame.enabled = show;
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
        string str1 = SampleSub.text;
        string str2 = subtitleInGame.text;

        Regex rich = new Regex(@"<[^>]*>");

        if (rich.IsMatch(str1) && rich.IsMatch(str2))
        {
            str1 = rich.Replace(str1, string.Empty);
            str2 = rich.Replace(str2, string.Empty);
        }
        SampleSub.color = textColor;
        SampleSub.text = "<mark="+ bgColor + " padding='50,50,0,0'>" + str1 + "</mark>";

        subtitleInGame.color = textColor;
        subtitleInGame.text = "<mark=" + bgColor + " padding='50,50,0,0'>" + str2 + "</mark>";
    }

    public void ChangeSize(float value)
    {
        int[] size = { 18, 23, 28, 33, 38 };
        SampleSub.fontSize = size[(int)value];
        subtitleInGame.fontSize = size[(int)value];
    }
}
