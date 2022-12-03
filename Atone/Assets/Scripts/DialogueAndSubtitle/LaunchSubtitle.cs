using System.Collections;
using UnityEngine;

namespace Assets.Scripts.DialogueAndSubtitle
{
    public class LaunchSubtitle : MonoBehaviour
    {
        public string dialogue;
        public GameObject subtitle;
        void Start()
        {
            StartCoroutine(subtitle.GetComponent<Subtitle>().LaunchSubtitle(dialogue));
        }


    }
}