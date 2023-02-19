using System;
using System.Runtime.InteropServices;
using UnityEngine;
using FMODUnity;


// Nouvelle méthode pour traquer les battements depuis FMOD
// Ceci recquiert un travail avec gestion manuelle de l'allocation pour éviter toute perte de référence en cours de route.
public class MusicManager : Singleton<MusicManager>
{
    #region MEMBER VARIABLES
    // Note to self: remember that the Singleton status allows us to use the public static MusicManager.Instance
    //[SerializeField][BankRef] private string currentSoundBank;    
    /*[SerializeField]*/ private EventReference musicFMODEvent; //FMOD Event reference. Set by Sequence manager via SetFMODEvent function

    public TimelineInfo timelineInfo = null;

    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }
    public delegate void OnMusicEnd(); // delegate for when the music ends
    public OnMusicEnd onMusicEnd;
    public delegate void OnMusicStart(); // delegate for when the music ends
    public OnMusicStart onMusicStart;

    private GCHandle timelineHandle; // needed to access a managed object (the timeline info) from unmanaged memory.
    // https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.gchandle?view=net-7.0
    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private FMOD.Studio.EventDescription musicDescriptionCallback;
    private static FMOD.Studio.EventInstance musicFMODInstance; //FMOD event instance that allows us to interact with it.
    public static FMOD.Studio.EventInstance MusicFMODInstance { get { return musicFMODInstance; } set { musicFMODInstance = value; } }

    // Spawners and actions will listen for those. Example: shooting bullets on beat, bending world on marker
    public delegate void BeatIsHit();
    public static BeatIsHit beatUpdated;
    public delegate void HalfBeatIsHit();
    public static HalfBeatIsHit halfBeatUpdated;
    public delegate void MarkerIsHit();
    public static MarkerIsHit markerUpdated;

    public static int lastBeat = 0;
    public static string lastMarker = null;

    private float secPerBeat;
    public float SecPerBeat { get => secPerBeat; }

    private bool isSecPerBeatSet = false;

    #endregion

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    #region INTERNAL CLASS TO WRITE THE TRACK DATA TO
    /// <summary>
    /// This makes use of C# Interop services to communicate with unmanaged code: 
    /// * https://learn.microsoft.com/en-us/dotnet/framework/interop/
    /// * https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.structlayoutattribute?view=net-7.0
    /// * https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.layoutkind?view=net-7.0
    /// </summary>

    [StructLayout(LayoutKind.Sequential)][Serializable]
    public class TimelineInfo
    {
        public int currentBeat = 0; // ex: in 4/4, values are 1,2,3 or 4
        public int currentBar = 0;
        public float currentTempo = 0;
        public int currentPositionInMS = 0; // position in milliseconds
        public int songLength = 0;
        public FMOD.StringWrapper markerHit = new FMOD.StringWrapper(); // Fetches name of last marker hit
    }
    #endregion

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    #region MONOBEHAVIOUR CALLBACKS
    private void Awake()
    {

        /*if(!musicFMODEvent.IsNull){
            musicFMODInstance = RuntimeManager.CreateInstance(musicFMODEvent);
            //musicFMODInstance.start(); // Only if we want to start playing on Awake
        }
        else {Debug.LogError("No music Instance could be created because musicFMODEvent is null.");}*/
    }
    private void Start()
    {
        /*if (!musicFMODEvent.IsNull)
        {
            timelineInfo = new TimelineInfo();

            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);  // This is a callback from UNMANAGED memory.

            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned); // Pin it so as to not collect it until we manually release it
            musicFMODInstance.setUserData(GCHandle.ToIntPtr(timelineHandle)); // Write value to the address
            musicFMODInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER); // Bitwise OR opertation (0x00001000 OR 0x00000800) 

            // Fetch the song length in MS
            musicFMODInstance.getDescription(out musicDescriptionCallback);
            musicDescriptionCallback.getLength(out int sLength);
            Debug.Log("Current music length in milliseconds :" + sLength);
            timelineInfo.songLength = sLength;

        }*/
    }

    public void Init()
    {
        if (!musicFMODEvent.IsNull)
        {
            musicFMODInstance = RuntimeManager.CreateInstance(musicFMODEvent);
            //musicFMODInstance.start(); // Only if we want to start playing on Awake
        }
        else { Debug.LogError("No music Instance could be created because musicFMODEvent is null."); }

        _isReady = true;
    }

    public void StartMusicManager()
    {
        if (!musicFMODEvent.IsNull)
        {
            timelineInfo = new TimelineInfo();

            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);  // This is a callback from UNMANAGED memory.

            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned); // Pin it so as to not collect it until we manually release it
            musicFMODInstance.setUserData(GCHandle.ToIntPtr(timelineHandle)); // Write value to the address
            musicFMODInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER); // Bitwise OR opertation (0x00001000 OR 0x00000800) 

            // Fetch the song length in MS
            musicFMODInstance.getDescription(out musicDescriptionCallback);
            musicDescriptionCallback.getLength(out int sLength);
            Debug.Log("Current music length in milliseconds :" + sLength);
            timelineInfo.songLength = sLength;
        }
    }

    private bool triggerOnceMusicEnd = false;

    private void Update()
    {
        if (!GameManager.Instance.isReady) return;
        // Note pour faire spawner en avance. 2 étapes:
        //      * Rajouter un délai initial en millisecondes à currentPositionInMS pour désigner le battement à marquer
        //      * Dans FMOD, déplacer les marqueurs d'autant de temps pour correspondre au délai
        musicFMODInstance.getTimelinePosition(out timelineInfo.currentPositionInMS);

        // First draft
        if (lastMarker != timelineInfo.markerHit)
        {
            lastMarker = timelineInfo.markerHit;

            if (markerUpdated != null)
            {
                markerUpdated();
            }
        }

        if (lastBeat != timelineInfo.currentBeat)
        {
            lastBeat = timelineInfo.currentBeat;

            if (beatUpdated != null)
            {
                beatUpdated();
            }
        }
        
        if (timelineInfo.currentPositionInMS >= timelineInfo.songLength && triggerOnceMusicEnd == true)
        {
            //Debug.Log("Musics end A");
            if (onMusicEnd != null && SequenceManager.Instance.isDeathRestartingMusic == false)
            {
                //Debug.Log("Musics end B");
                triggerOnceMusicEnd = false;
                onMusicEnd();
            }
        }

        if (!isSecPerBeatSet && timelineInfo.currentTempo != 0)
        {
            secPerBeat = 60f / timelineInfo.currentTempo;
            Debug.Log("| Current Tempo :" + timelineInfo.currentTempo);
            isSecPerBeatSet = true;
        }
    }

    private void OnDestroy()
    {
        isSecPerBeatSet = false;
        musicFMODInstance.setUserData(IntPtr.Zero);
        musicFMODInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicFMODInstance.release();
        if(timelineHandle.IsAllocated) {timelineHandle.Free();}  // Or suffer the memory leak curse
    }

    public GameObject test;
    private int lastBeatTest = 0;

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (!GameManager.Instance.isReady)
            return;

        //GUILayout.Box($"Current Beat = {timelineInfo.currentBeat} | Last marker = {(string)timelineInfo.markerHit}");
        GUI.Box(new Rect(Screen.width - 300, 0, 300, 50), $"Current Beat = {timelineInfo.currentBeat} | Last marker = {(string)timelineInfo.markerHit}");

        // Place gameobject behind the player, watch scene while playing to see if beats are in time
        /*if (timelineInfo.currentBeat != lastBeatTest)
        {
            lastBeatTest = timelineInfo.currentBeat;
            Instantiate(test, PlayerManager.Instance.playerController.transform.position, Quaternion.identity);
        }*/

    }

#endif
    #endregion


    #region MISC METHODS
    public void PlayMusic()
    {
        //Start the music
        Debug.Log("StartMusic from MUSIC MANAGER");
        triggerOnceMusicEnd = true;
        musicFMODInstance.start();      // FMOD Test Julien

        //musicFMODInstance.release();    // FMOD Test Julien
    }

    public void StopMusic()
    {
        musicFMODInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);      // FMOD Test Julien
    }

    public void SetFMODEvent(EventReference pmusicFMODEvent)
    {
        musicFMODEvent = pmusicFMODEvent;
    }

    // Pause now handled by GameManager directly
    // public static void ToggleMusicPause(bool isPausing){
    //     MusicFMODInstance.setPaused(isPausing);
    // }

    #endregion

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    #region  FMOD CALLBACK
    // Il faut s'assurer que les info ne soient pas supprimées par le GC
    // FMOD recommande dans sa doc de descendre en bas-niveau pour effectuer les réglages
    // FMOD.RESULT is just an error code, but what is managed inside the function is unmanaged memory.
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr instancePtr, System.IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        IntPtr timelineInfoPtr; // Used to pull data from the event. Address of the the timeline info that we want to extract
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Callback error on timeline info pointer " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero) // If we point to something in memory
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr); // Initialize a handle
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target; // dereference our pointer and try to cast the value to our managed TimelineInfo type

            switch (type)
            {
                // Valid values: 0x00001000, 0x00000800 or 0x00001800
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT: // 0x00001000
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBeat = parameter.beat;
                        // Can also modify timelinInfo to include the Bar (parameter.bar) and the current tempo (parameter.tempo)
                        timelineInfo.currentBar = parameter.bar;
                        timelineInfo.currentTempo = parameter.tempo;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER: // 0x00000800
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.markerHit = parameter.name;
                        Debug.Log("Marker name = " + parameter.name);
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;


    }
    #endregion
}
