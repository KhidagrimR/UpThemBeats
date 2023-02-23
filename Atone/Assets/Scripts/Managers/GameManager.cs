using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement; 

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

    public bool isGameCurrentlyPaused { get { return currentState == GeneralGameState.PAUSED; }}
    public bool isGameAtMainMenu { get { return currentState == GeneralGameState.MAIN_MENU; }}
    public GeneralGameState currentState { get; private set; }

    public delegate void OnMenu(GeneralGameState oldStateWeWantToLeave);
    public static event OnMenu onMenu; // Needs to be static to not mess things up when communicating to the additive scene 
    public delegate void LoadSettings(bool resetAllSettings);
    public static event LoadSettings loadSettings; 

    void Awake()
    {
        // do starting setup stuff here

        GameAnimatorsParams.BuildDictionary();
        // Debug.Log("Is Main_Menu_A1 scene loaded?"+ (SceneManager.GetActiveScene().name));

        // init other manager
        StartCoroutine(Init());

        //Time.timeScale = 0.25f;
    }   
    private void Start()
    {
        if( SceneManager.GetActiveScene().name == "Main_Menu_A1"
            && (uI_Loader != null))  
        {
            StartCoroutine(StartMainMenuOnMainMenuScene());    
            StartCoroutine(LoadGameSettings(true));  // in main menu, load default settings                            
        }
        else
        {
            StartCoroutine(LoadGameSettings(false));
        }

    }

    IEnumerator Init()
    {
        currentState = GeneralGameState.GAME;
        // isGameCurrentlyPaused = false;
        // isGameAtMainMenu = false; 

        // Main_Menu_A1
        if( SceneManager.GetActiveScene().name == "Main_Menu_A1"
            && (uI_Loader != null))
        {
            // isGameAtMainMenu = true;
            currentState = GeneralGameState.MAIN_MENU;
            uI_Loader.Init();
            yield return new WaitUntil(() => uI_Loader.isReady);
            Debug.Log("<color=red>UI for main menu is ready</color>");                                    
        }
        // Regular game scenes
        else 
        {
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
        }

        Debug.Log("<color=red>### READY ###</color>");
        _isReady = true;
    }
    IEnumerator StartMainMenuOnMainMenuScene()
    {
        while(!Atone_UI.UI_LandingCanvasController.areSubscriptionsDone)
        {
            // Wait for next frame (or 2 frames) to ensure that the event subscription is really done before invoking
            yield return null;
        }
        onMenu?.Invoke(GeneralGameState.MAIN_MENU);
    }  
    IEnumerator LoadGameSettings(bool resetAllSettings)
    {
        while(!Atone_SettingsSaveAndLoadHandler.areSubscriptionsDone)
        {
            yield return null;
        }
        loadSettings?.Invoke(resetAllSettings);
    }
    public void HandleReturnKeyPress()
    {
        // Debug.Log(currentState);

        onMenu?.Invoke(currentState);
        if(currentState == GeneralGameState.GAME)
        {
            PauseGame();
        }
        else if(currentState == GeneralGameState.PAUSED && !Atone_UI.UI_LandingCanvasController.forbidPauseToggle)
        {
            // Debug.Log("Atone_UI.UI_LandingCanvasController.forbidPauseToggle "+ Atone_UI.UI_LandingCanvasController.forbidPauseToggle);
            ResumeGame();
        }
    }
    // public void TogglePauseState()
    // {
    //     isGameCurrentlyPaused = !isGameCurrentlyPaused;
    //     // SoundCreator.ToggleMusicPause(isGameCurrentlyPaused);
    //     // RuntimeManager.PauseAllEvents(isGameCurrentlyPaused);
    //     gameplayBus.setPaused(isGameCurrentlyPaused);
    //     // MusicManager.ToggleMusicPause(isGameCurrentlyPaused);
    //     Time.timeScale = isGameCurrentlyPaused ? 0 : 1;
    // }
    public void PauseGame()
    {
        currentState = GeneralGameState.PAUSED;
        gameplayBus.setPaused(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        // Debug.Log("Resuming game..."); 
        if(currentState == GeneralGameState.MAIN_MENU) {return;}
        currentState = GeneralGameState.GAME;
        gameplayBus.setPaused(false);
        Time.timeScale = isGameCurrentlyPaused ? 0 : 1;
        // Debug.Log("Time.timeScale "+Time.timeScale); 
    }    

    [InspectorReadOnly]
    public int currentSpamCount = 0;
    [InspectorReadOnly]
    public float lastTimeSpamCheck = 0;
    [InspectorReadOnly]
    public float durationBeforeResetSpam = 3f;
    [InspectorReadOnly]
    public int amountOfSpamBeforeCooldown = 3;
    [InspectorReadOnly]
    public bool isOverclocked = false;
    [InspectorReadOnly]
    public float overclockDuration = 0.75f;

    // GameManager.Instance.AddSpamInput();
    public void AddSpamInput()
    {
        if(isOverclocked == true)
        {
            // play here sound if player try to hit obstacle while they are overclocked
            // fmod sound
            MusicManager.Instance.tutoTimer.Play();
            return;
        }

        currentSpamCount++;
        float currentTime = Time.time;
        if(lastTimeSpamCheck + durationBeforeResetSpam < currentTime)
        {
            //reset
            currentSpamCount = 0;
        }
        if(currentSpamCount >= amountOfSpamBeforeCooldown)
        {
            // player spammed
            //Add overclocked Panel
            isOverclocked = true;
            currentSpamCount = 0;
            Invoke("StopOverclock", overclockDuration);
            //play overclocked ANimation
            PlayerManager.Instance.playerController.animationTrigger.PlayAnimation(AnimationEnum.Overclock);
            //set vignette effect
            
            //Add camera shake
            CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.WallrunHit);
        }

        lastTimeSpamCheck = currentTime;
    }

    void StopOverclock()
    {
        isOverclocked = false;
    }
}

public enum GeneralGameState
{
    MAIN_MENU,
    GAME,
    PAUSED
}
