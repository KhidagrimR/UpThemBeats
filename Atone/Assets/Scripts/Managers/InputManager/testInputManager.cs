using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInputManager : MonoBehaviour
{
    
    void Start()
    {
        InputManager.Instance.onJump += jump;
        InputManager.Instance.onJumpPressed += jumpPressed;
    }

    public void jump(){
        print("jump");
    }

    public void jumpPressed (){
        print("jump pressed");
    }

   
}
