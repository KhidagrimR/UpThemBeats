using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Loader : MonoBehaviour
{
    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }
    // Attach to a camera or the game manager. Somewhere where you'll find it
    public void Init()
    {
        if (SceneManager.GetSceneByName("UI Scene").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("UI Scene", LoadSceneMode.Additive);
            _isReady = true;
        }
    }
}
