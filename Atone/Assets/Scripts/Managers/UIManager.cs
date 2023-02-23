using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public GameObject mainMenuLanding;
    public GameObject menuCamera;

    public void SetupUIGame()
    {
        // mainMenuLanding.SetActive(false);
        //menuCamera.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
