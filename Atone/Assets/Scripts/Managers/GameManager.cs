using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }

    void Awake()
    {
        // do starting setup stuff here

        // init other manager
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        if (SoundCreator.Instance != null)
        {
            SoundCreator.Instance.Init();
            yield return new WaitUntil(() => SoundCreator.Instance.isReady);
            Debug.Log("soundcreator is ready");
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.Init();
            yield return new WaitUntil(() => PlayerManager.Instance.isReady);
            Debug.Log("Player Manager is ready");
        }

        if (SoundCreator.Instance != null)
            SoundCreator.Instance.PlayMusic();

        _isReady = true;
    }
}
