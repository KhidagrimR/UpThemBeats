using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

namespace Atone_UI
{
    public class UI_VolumeSlider : MonoBehaviour
    {
        [Header("Bus or VCA path")] // VCA : Voltage Controlled Amplifier
        // Pour le test j'utilise un Bus appelé "Julien_Test_Master", qui contient les sous-bus "Music", "SFX" et "UI"
        [SerializeField] private string busPath = ""; // Format: "bus:/ + nom" ou "vca:/ + nom"
        private FMOD.Studio.Bus bus;
        private FMOD.Studio.VCA vca;
        private bool useVCA = false;  
        private bool hasStartRun = false;

        private Slider attachedSlider; 

        public float volumeValue {get; set;}

        void Awake(){
            attachedSlider = GetComponent<Slider>();
        }
        private void Start()
        {
            //float volume;
            if(busPath == ""){   
                throw new System.ArgumentException ("busPath field must not be empty");
            }
            else if(busPath[0] == 'b') {
                bus = RuntimeManager.GetBus(busPath);
                // volumeValue now retreived from PlayerPrefs
                //bus.getVolume(out volume);
                //volumeValue = volume;
            } 
            else if (busPath[0] == 'v') {
                useVCA = true;
                vca = RuntimeManager.GetVCA(busPath);
                
                //vca.getVolume(out volume);
                //volumeValue = volume;
            }
            else {
                throw new System.ArgumentException ("invalid string. busPath field must be either for a bus or vca.");
            }
            
            
            try {
                attachedSlider.onValueChanged.AddListener(delegate{UpdateVolume();});
                attachedSlider.value = volumeValue; // volume * attachedSlider.maxValue;
                UpdateVolume();
            }
            catch (System.ArgumentNullException ex){
                Debug.LogError(ex.Message);
            }
            hasStartRun = true;
        }

        void OnDestroy()
        {
            hasStartRun = false;
        }

        private void UpdateVolume()
        {
            // FMOD uses values 0 to 1
            if(!useVCA){
                bus.setVolume(attachedSlider.value / attachedSlider.maxValue);
            }
            else
            {
                vca.setVolume(attachedSlider.value / attachedSlider.maxValue);
            }
            volumeValue = attachedSlider.value;
        }

        public void UpdateVolume(float newVol)
        {
            newVol = Mathf.Clamp(newVol, 0f, 100f);
            volumeValue = newVol;
            if(hasStartRun) 
            { 
                UpdateVolume(); 
            }
            
        }
    } 
}


