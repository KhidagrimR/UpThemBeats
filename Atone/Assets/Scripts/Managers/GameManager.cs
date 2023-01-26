using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GameManager : Singleton<GameManager>
{
    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }

    public bool isGameCurrentlyPaused { get; set; }

    void Awake()
    {
        // do starting setup stuff here

        GameAnimatorsParams.BuildDictionary();

        // init other manager
        StartCoroutine(Init());

        //Time.timeScale = 0.25f;
    }

    IEnumerator Init()
    {
        isGameCurrentlyPaused = false;

        if (SoundCreator.Instance != null)
        {
            SoundCreator.Instance.Init();
            yield return new WaitUntil(() => SoundCreator.Instance.isReady);
            Debug.Log("soundcreator is ready");
        }

        /*if (MusicManager.Instance != null)
        {
            MusicManager.Instance.Init();
            yield return new WaitUntil(() => MusicManager.Instance.isReady);
            Debug.Log("Music Manager is ready");
        }*/

        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.Init();
            yield return new WaitUntil(() => SequenceManager.Instance.isReady);
            Debug.Log("Sequence Manager is ready");
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.Init();
            yield return new WaitUntil(() => PlayerManager.Instance.isReady);
            Debug.Log("Player Manager is ready");
        }

        // if (SoundCreator.Instance != null)
        // {   
        //     SoundCreator.Instance.PlayMusic();
        // }

        /*if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StartMusicManager();
        }*/

        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.StartSequence();
        }

        _isReady = true;
    }

    public void TogglePauseState()
    {
        isGameCurrentlyPaused = !isGameCurrentlyPaused;
        // SoundCreator.ToggleMusicPause(isGameCurrentlyPaused);
        RuntimeManager.PauseAllEvents(isGameCurrentlyPaused);
        //MusicManager.ToggleMusicPause(isGameCurrentlyPaused);
        Time.timeScale = isGameCurrentlyPaused ? 0 : 1;
    }
}
