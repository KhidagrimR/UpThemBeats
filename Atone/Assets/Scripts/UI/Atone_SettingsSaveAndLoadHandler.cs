using UnityEngine;

public class Atone_SettingsSaveAndLoadHandler : Singleton<Atone_SettingsSaveAndLoadHandler>
{
    // This will be used to save and load our player settings from PlayerPrefs. Useful between scenes.
    #region VARIABLES
    private static int _subtitles_areOn = 1, _subtitles_theme = 0, _crosshair_isOn = 1, _crosshair_style = 0, _crosshair_colour = 0;
    private static float _subtitles_size = 2f, _crosshair_opacity = 1f, _audio_Master = 100f, _audio_Music = 100f, _audio_SFX = 100f, _audio_UI = 100f, _audio_Dialogues = 100f;

    public static bool areSubscriptionsDone {get; private set;}

    #endregion    

    #region PROPERTIES
    // Subtitles
    public static int subtitles_areOn {get{return _subtitles_areOn;} set{_subtitles_areOn =  Mathf.Clamp(value, 0, 1);}} // bool. Default true (1)
    public static int subtittles_theme {get{return _subtitles_theme;} set{_subtitles_theme = Mathf.Clamp(value, 0, 2);}} // 0 to 2 on dropdown. Default 0
    public static float subtitles_size {get{return _subtitles_size;} set{_subtitles_size =  Mathf.Clamp(value, 0, 4);}} // 0 to 4 on slider. Default is 2

    // // Crosshair
    public static int crosshair_isOn {get{return _crosshair_isOn;} set{_crosshair_isOn =  Mathf.Clamp(value, 0, 1);}} // bool. Default true (1)
    public static int crosshair_style {get{return _crosshair_style;} set{_crosshair_style =  Mathf.Clamp(value, 0, 3);}} // 0 to 3 dropdown. Default 0
    public static int crosshair_colour {get{return _crosshair_colour;} set{_crosshair_colour =  Mathf.Clamp(value, 0, 4);}} // 0 to 4 dropdown. Default 0
    public static float crosshair_opacity {get{return _crosshair_opacity;} set{_crosshair_opacity =  Mathf.Clamp01(value);}} // 0 to 1 slider. Default 1

    // // Audio
    public static float audio_Master {get{return _audio_Master;} set{_audio_Master =  Mathf.Clamp(value, 0f, 100f);}} // 0 to 100 slider. Default 100
    public static float audio_Music {get{return _audio_Music;} set{_audio_Music =  Mathf.Clamp(value, 0f, 100f);}} // 0 to 100 slider. Default 100
    public static float audio_SFX {get{return _audio_SFX;} set{_audio_SFX =  Mathf.Clamp(value, 0f, 100f);}} // 0 to 100 slider. Default 100
    public static float audio_UI {get{return _audio_UI;} set{_audio_UI =  Mathf.Clamp(value, 0f, 100f);}} // 0 to 100 slider. Default 100
    public static float audio_Dialogues {get{return _audio_Dialogues;} set{_audio_Dialogues =  Mathf.Clamp(value, 0f, 100f);}} // 0 to 100 slider. Default 100

    #endregion

    private static readonly string[] settingsKeys = {
        // Subtitles : indices 0 to 2
        "subtitles_areOn", // 1
        "subtittles_theme", // 0
        "subtitles_size", // 2f
        // Crosshair : indices 3 to 6
        "crosshair_isOn", // 1
        "crosshair_style", // 0
        "crosshair_colour", // 0
        "crosshair_opacity", // 1f
        // Audio : indices 7 to 11
        "audio_Master", // 100f
        "audio_Music",
        "audio_SFX",
        "audio_UI",
        "audio_Dialogues"
    };


    private void Start()
    {
        // Subscribe to your events here
        GameManager.loadSettings += LoadAllSettingsFromEvent;
        areSubscriptionsDone = true;

    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
             GameManager.loadSettings -= LoadAllSettingsFromEvent;
        }
        areSubscriptionsDone = false;
    }

    private void ResetDefaultSettings()
    {   
        for(int i = 0; i < 12; i++)
        {
            PlayerPrefs.DeleteKey(settingsKeys[i]);
        }

        PlayerPrefs.SetInt(settingsKeys[0], 1);
        PlayerPrefs.SetInt(settingsKeys[1], 0);

        PlayerPrefs.SetFloat(settingsKeys[2], 2f);

        PlayerPrefs.SetInt(settingsKeys[3], 1);
        PlayerPrefs.SetInt(settingsKeys[4], 0);
        PlayerPrefs.SetInt(settingsKeys[5], 0);

        PlayerPrefs.SetFloat(settingsKeys[6], 1f);
        PlayerPrefs.SetFloat(settingsKeys[7], 100f);
        PlayerPrefs.SetFloat(settingsKeys[8], 100f);
        PlayerPrefs.SetFloat(settingsKeys[9], 100f);
        PlayerPrefs.SetFloat(settingsKeys[10], 100f);
        PlayerPrefs.SetFloat(settingsKeys[11], 100f);
        PlayerPrefs.Save();
        // Debug.Log("PlayerPrefs reset");
        LoadAllSettings();
    }

    // SAVE --------------------------------------------------------------------------------------------------------------------------------
    public void SaveAllSettings()
    {
        PlayerPrefs.Save();
    }
    public void SetAllSettings()
    {
        SetSubtitleSettings();
        SetCrosshairSettings();
        SetAudioSettings();
        PlayerPrefs.Save();
    }
    public void SetSubtitleSettings()
    {
        PlayerPrefs.SetInt(settingsKeys[0], subtitles_areOn);
        PlayerPrefs.SetInt(settingsKeys[1], subtittles_theme);
        PlayerPrefs.SetFloat(settingsKeys[2], subtitles_size);
    }
    public void SetCrosshairSettings()
    {
        PlayerPrefs.SetInt(settingsKeys[3], crosshair_isOn);
        PlayerPrefs.SetInt(settingsKeys[4], crosshair_style);
        PlayerPrefs.SetInt(settingsKeys[5], crosshair_colour);
        PlayerPrefs.SetFloat(settingsKeys[6], crosshair_opacity);
    }
    public void SetAudioSettings()
    {
        PlayerPrefs.SetFloat(settingsKeys[7], audio_Master);
        PlayerPrefs.SetFloat(settingsKeys[8], audio_Music);
        PlayerPrefs.SetFloat(settingsKeys[9], audio_SFX);
        PlayerPrefs.SetFloat(settingsKeys[10], audio_UI);
        PlayerPrefs.SetFloat(settingsKeys[11], audio_Dialogues);
    }

    // LOAD --------------------------------------------------------------------------------------------------------------------------------
    public void LoadAllSettingsFromEvent(bool resetAllSettings)
    {
        if(resetAllSettings){ 
            // Debug.Log("Resetting PlayerPrefs...");
            ResetDefaultSettings(); }
        else{ 
            // Debug.Log("Loading already saved PlayerPrefs..."); 
            LoadAllSettings(); }
    }

    private void LoadAllSettings()
    {
        LoadSubtitleSettings();
        LoadCrosshairSettings();
        LoadAudioSettings();
    }
    public void LoadSubtitleSettings()
    {        
        subtitles_areOn = PlayerPrefs.GetInt(settingsKeys[0], 1);
        subtittles_theme = PlayerPrefs.GetInt(settingsKeys[1], 0);
        subtitles_size = PlayerPrefs.GetFloat(settingsKeys[2], 2f);        
    }
    public void LoadCrosshairSettings()
    {        
        crosshair_isOn = PlayerPrefs.GetInt(settingsKeys[3], 1);
        crosshair_style = PlayerPrefs.GetInt(settingsKeys[4], 0);
        crosshair_colour = PlayerPrefs.GetInt(settingsKeys[5], 0);
        crosshair_opacity = PlayerPrefs.GetFloat(settingsKeys[6], 1f);        
    }
    public void LoadAudioSettings()
    {        
        audio_Master = PlayerPrefs.GetFloat(settingsKeys[7], 100f);
        audio_Music = PlayerPrefs.GetFloat(settingsKeys[8], 100f);
        audio_SFX = PlayerPrefs.GetFloat(settingsKeys[9], 100f);
        audio_UI = PlayerPrefs.GetFloat(settingsKeys[10], 100f);
        audio_Dialogues = PlayerPrefs.GetFloat(settingsKeys[11], 100f);        
    }
}
