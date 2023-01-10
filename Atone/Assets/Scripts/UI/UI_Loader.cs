using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Loader : MonoBehaviour
{
    // Attach to a camera or the game manager. Somewhere where you'll find it
    void Start()
    {
        if(SceneManager.GetSceneByName("UI Scene").isLoaded == false) {
            SceneManager.LoadSceneAsync("UI Scene", LoadSceneMode.Additive);
        }
    }

    
}
