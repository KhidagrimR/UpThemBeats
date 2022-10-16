using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInputManager : MonoBehaviour
{
    
    void Start()
    {
        InputManager.onJump += jump;
        InputManager.onJumpPressed += jumpPressed;
    }

    public void jump(){
        print("jump");
    }

    public void jumpPressed (){
        print("jump pressed");
    }

   
}
