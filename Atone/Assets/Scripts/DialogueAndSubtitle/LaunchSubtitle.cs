using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DialogueAndSubtitle
{
    public class LaunchSubtitle : MonoBehaviour
    {
        public string dialogue;
        private void OnTriggerEnter(Collider other){
            StartCoroutine(Subtitle.Instance.LaunchSubtitle(dialogue));
        }


    }
}