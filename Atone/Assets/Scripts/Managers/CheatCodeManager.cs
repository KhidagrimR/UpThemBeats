using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
 
public class CheatCodeManager : MonoBehaviour
{
    [Serializable]
    public class CheatCode
    {
        public string codeInString = "";

        //public KeyCode[] code = new KeyCode[4]; 
        public UnityEvent unityEvent;
    }

    public CheatCode[] cheatCodeArray;
    public float allowedDelay = 1f;
 
    private float _delayTimer;

    [InspectorReadOnly]
    public int _index = 0;

    [InspectorReadOnly]
    public string inputCombinaison = "";

    void Update()
    {
        _delayTimer += Time.deltaTime;
        if (_delayTimer > allowedDelay)
        {
            ResetCheatInput();
        }
 
        if (Input.anyKeyDown)
        {
            inputCombinaison += Input.inputString;

            if (GetCheatCode())
            {
                _index++;
                _delayTimer = 0f;
            }
            else
            {
                ResetCheatInput();
            }
        }

        /*if(cheatCodeChosen != null)
        {
            if (_index == cheatCodeChosen.Length)
            {
                ResetCheatInput();
                CheatEvent.Invoke();
                cheatCodeChosen = null;
            }
        }*/
    }
 
    void ResetCheatInput()
    {
        _index = 0;
        _delayTimer = 0f;
        inputCombinaison = "";
    }
 

    CheatCode cheatCodeChosen;
    public bool GetCheatCode()
    {
        foreach(CheatCode cCode in cheatCodeArray)
        {
            if(cCode.codeInString.Contains(inputCombinaison))
            {
                if(String.Compare(inputCombinaison, cCode.codeInString) == 0)
                {
                    // si c est identique
                    // le code est reussi
                    cCode.unityEvent.Invoke();

                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
}
