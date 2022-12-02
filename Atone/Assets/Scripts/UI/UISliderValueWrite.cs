using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Atone_UI
{
    public class UISliderValueWrite : MonoBehaviour
    {
        private Slider attachedSlider;
        [SerializeField]private TMP_Text sliderTextValue;

        void Awake(){
            attachedSlider = GetComponent<Slider>();
        }
        void Start(){
            attachedSlider.onValueChanged.AddListener(delegate{UpdateTextValue();});
            sliderTextValue.text = attachedSlider.value.ToString();
        }
        void UpdateTextValue()
        {
            sliderTextValue.text = attachedSlider.value.ToString();
        }
    }
}