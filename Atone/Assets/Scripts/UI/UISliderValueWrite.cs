using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Atone_UI
{
    public class UISliderValueWrite : MonoBehaviour
    {
        private Slider attachedSlider;
        [SerializeField]private TMP_Text sliderText;

        void Awake(){
            attachedSlider = GetComponent<Slider>();
        }
        void Start(){
            try {
                attachedSlider.onValueChanged.AddListener(delegate{UpdateTextValue();});
                UpdateTextValue();
                // sliderText.text = attachedSlider.value.ToString();
            }
            catch(System.ArgumentNullException ex){
                Debug.LogError(ex.Message);
            }
        }
        void UpdateTextValue()
        {            
            sliderText.text = attachedSlider.value.ToString("F0");
        }
    }
}