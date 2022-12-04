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
    [SerializeField] private EventReference musicFMODEvent; //FMOD Event reference.    

    public TimelineInfo timelineInfo = null;
    
    
    private GCHandle timelineHandle; // needed to access a managed object (the timeline info) from unmanaged memory.
    // https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.gchandle?view=net-7.0
    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private static FMOD.Studio.EventInstance musicFMODInstance; //FMOD event instance that allows us to interact with it.
    public static FMOD.Studio.EventInstance MusicFMODInstance{get{return musicFMODInstance;} set{musicFMODInstance = value;}}
    
    #endregion
    
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    #region INTERNAL CLASS TO WRITE THE TRACK DATA TO
    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }
    #endregion

    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    #region MONOBEHAVIOUR CALLBACKS
    private void Awake(){
        
        if(!musicFMODEvent.IsNull){
            musicFMODInstance = RuntimeManager.CreateInstance(musicFMODEvent);
            //musicFMODInstance.start(); // Only if we want to start playing on Awake
        }
        else {Debug.LogError("No music Instance could be created because musicFMODEvent is null.");}
    }
    private void Start(){
        if(!musicFMODEvent.IsNull){
            timelineInfo = new TimelineInfo();

            beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);  // This is a callback from UNMANAGED memory.
            
            timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned); // Pin it so as to not collect it until we manually release it
            musicFMODInstance.setUserData(GCHandle.ToIntPtr(timelineHandle)); // Write value to the address
            musicFMODInstance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER); // Bitwise OR opertation (0x00001000 OR 0x00000800) 

        }
    }
    private void Update(){
       
    }
    

    private void OnDestroy(){
        musicFMODInstance.setUserData(IntPtr.Zero);
        musicFMODInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicFMODInstance.release();
        timelineHandle.Free();
    }
    #endregion

    public void PlayMusic()
    {
        //Start the music
        Debug.Log("StartMusic from MUSIC MANAGER");
        musicFMODInstance.start();      // FMOD Test Julien
        //musicFMODInstance.release();    // FMOD Test Julien
    }

    public static void ToggleMusicPause(bool isPausing){
        MusicFMODInstance.setPaused(isPausing);
    }




    //-----------------------------------------------------------------------------------------------------------------------------------------------------

    // Il faut s'assurer que les info ne soient pas supprimées par le GC
    // FMOD recommande dans sa doc de descendre en bas-niveau pour effectuer les réglages
    // FMOD.RESULT is just an error code, but what is managed inside the function is unmanaged memory.
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr instancePtr, System.IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        IntPtr timelineInfoPtr; // Used to pull data from the event. Address of the the timeline info that we want to extract
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        if(result != FMOD.RESULT.OK)
        {
            Debug.LogError("Callback error on timeline info pointer "+ result);
        }
        else if(timelineInfoPtr != IntPtr.Zero) // If we point to something in memory
        {
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr); // Initialize a handle
            TimelineInfo timelineInfo =(TimelineInfo)timelineHandle.Target; // dereference our pointer and try to cast the value to our managed TimelineInfo type

            switch(type)
            {
                // Valid values: 0x00001000, 0x00000800 or 0x00001800
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT: // 0x00001000
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBeat = parameter.beat;
                        }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER: // 0x00000800
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                        Debug.Log("Marker name = " + parameter.name);
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;


    }
}
