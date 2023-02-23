using UnityEngine;
using Atone_UI;

public class UI_Audio : MonoBehaviour
{
    [SerializeField] private UI_VolumeSlider masterSlider;
    [SerializeField] private UI_VolumeSlider musicSlider;
    [SerializeField] private UI_VolumeSlider sfxSlider;
    [SerializeField] private UI_VolumeSlider uiSlider;
    [SerializeField] private UI_VolumeSlider dialoguesSlider;

    private void Start()
    {
        LoadValues();
    }
    private void LoadValues()
    {
        masterSlider.UpdateVolume(Atone_SettingsSaveAndLoadHandler.audio_Master);
        musicSlider.UpdateVolume(Atone_SettingsSaveAndLoadHandler.audio_Music);
        sfxSlider.UpdateVolume(Atone_SettingsSaveAndLoadHandler.audio_SFX);
        uiSlider.UpdateVolume(Atone_SettingsSaveAndLoadHandler.audio_UI);
        dialoguesSlider.UpdateVolume(Atone_SettingsSaveAndLoadHandler.audio_Dialogues);
    }

}
