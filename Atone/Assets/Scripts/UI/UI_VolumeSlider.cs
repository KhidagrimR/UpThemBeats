using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

namespace Atone_UI
{
    public class UI_VolumeSlider : MonoBehaviour
    {
        // Pour le test j'utilise un Bus appelé "Julien_Test_Master", qui contient les sous-bus "Music", "SFX" et "UI"
        [SerializeField] private string busPath = ""; // Format: "bus:/ + nom"
        private FMOD.Studio.Bus bus;

        private Slider attachedSlider; 

        void Awake(){
            attachedSlider = GetComponent<Slider>();
        }
        private void Start()
        {
            if(busPath != ""){
                bus = RuntimeManager.GetBus(busPath);
            }
            else {
                throw new System.ArgumentException ("busPath field must not be empty");
            }
            
            bus.getVolume(out float volume);
            
            try {
                attachedSlider.onValueChanged.AddListener(delegate{UpdateVolume();});
                attachedSlider.value = volume * attachedSlider.maxValue;
                UpdateVolume();
            }
            catch (System.ArgumentNullException ex){
                Debug.LogError(ex.Message);
            }
            
        }

        public void UpdateVolume()
        {
            bus.setVolume(attachedSlider.value / attachedSlider.maxValue);
        }
    } 
}


