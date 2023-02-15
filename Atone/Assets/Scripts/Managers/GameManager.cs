using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GameManager : Singleton<GameManager>
{
    public UI_Loader uI_Loader;    
    private FMOD.Studio.Bus gameplayBus;
    [InspectorReadOnly] public bool isPlayerDead;
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
            Debug.Log("<color=red>soundcreator is ready</color>");
        }

        if (uI_Loader != null)
        {
            uI_Loader.Init();
            yield return new WaitUntil(() => uI_Loader.isReady);
            Debug.Log("<color=red>UI is ready</color>");
        }

        /*if (MusicManager.Instance != null)
        {
            MusicManager.Instance.Init();
            yield return new WaitUntil(() => MusicManager.Instance.isReady);
            Debug.Log("Music Manager is ready");
        }*/

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.Init();
            yield return new WaitUntil(() => PlayerManager.Instance.isReady);
            Debug.Log("<color=red>Player Manager is ready</color>");
        }

        if (SequenceManager.Instance != null)
        {
            SequenceManager.Instance.Init();
            yield return new WaitUntil(() => SequenceManager.Instance.isReady);
            Debug.Log("<color=red>Sequence Manager is ready</color>");
        }

        // if (SoundCreator.Instance != null)
        // {   
        //     SoundCreator.Instance.PlayMusic();
        // }

        /*if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StartMusicManager();
        }*/

        if (PostProcessManager.Instance != null)
        {
            PostProcessManager.Instance.Init();
            yield return new WaitUntil(() => PostProcessManager.Instance.isReady);
            Debug.Log("<color=red>PostProcessManager is ready</color>");
        }

        if(UIManager.Instance != null)
        {
            UIManager.Instance.SetupUIGame();
        }

        if (SequenceManager.Instance != null)
        {
            gameplayBus = RuntimeManager.GetBus("bus:/Gameplay_Master");
            SequenceManager.Instance.StartSequence();
        }

        Debug.Log("<color=red>### READY ###</color>");
        _isReady = true;
    }
    public void TogglePauseState()
    {
        isGameCurrentlyPaused = !isGameCurrentlyPaused;
        // SoundCreator.ToggleMusicPause(isGameCurrentlyPaused);
        // RuntimeManager.PauseAllEvents(isGameCurrentlyPaused);
        gameplayBus.setPaused(isGameCurrentlyPaused);
        // MusicManager.ToggleMusicPause(isGameCurrentlyPaused);
        Time.timeScale = isGameCurrentlyPaused ? 0 : 1;
    }
}
