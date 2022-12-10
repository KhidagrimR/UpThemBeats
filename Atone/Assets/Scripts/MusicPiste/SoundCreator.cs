using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using FMODUnity;


public class SoundCreator : Singleton<SoundCreator>
{
    [Header("Setup Generation vars")]
    //Song beats per minute
    [SerializeField] private float songBpm; //This is determined by the song you're trying to sync up to // test value 80
    [SerializeField] private float firstBeatOffset; //The offset to the first beat of the song in seconds
    [SerializeField] private float distanceBetweenNotes; // La distance physique séparant 2 beats // test value 10
    public float DistanceBetweenNotes {get {return distanceBetweenNotes;}}

    [Header("LevelDesign Settings")]
    [SerializeField] private Vector3 noteSpawnDirection; // direction on x, y and z axis to spawn notes;
    [SerializeField] private Vector3 noteSpawnAngle; // rotation on x, y and z axis to spawn notes;

    [Header("Component Settings")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject halfNotePrefab;
    [SerializeField] private Transform noteParent;

    [Header("Sounds Vars")]
    [SerializeField] private AudioSource musicSource; //an AudioSource attached to this GameObject that will play the music.
    [SerializeField] private AudioClip musicClip; //the music itself

    [Header("Sounds Vars (FMOD) Test for now")]
    [SerializeField] private EventReference musicFMODEvent; //FMOD Event reference.
    private static FMOD.Studio.EventInstance musicFMODInstance; //FMOD event instance that allows us to interact with it.
    public static FMOD.Studio.EventInstance MusicFMODInstance{get{return musicFMODInstance;} set{musicFMODInstance = value;}}


    [Header("Sounds Info")]
    [SerializeField][InspectorReadOnly] private float secPerBeat; //The number of seconds for each song beat.
    [SerializeField][InspectorReadOnly] private float songPosition; //Current song position, in seconds
    [SerializeField][InspectorReadOnly] private float songPositionInBeats; //Current song position, in beats
    [SerializeField][InspectorReadOnly] private float dspSongTime; //How many seconds have passed since the song started
    [SerializeField][InspectorReadOnly] private int numberOfBeatsInTrack; //the number of beats in the track
    // Properties for read-only access
    public float SecPerBeat {get{return secPerBeat;}}
    public float SongPosition {get{return songPosition;}}
    public float SongPositionInBeats {get{return songPositionInBeats;}}
    public float DspSongTime {get{return dspSongTime;}}
    public float NumberOfBeatsInTrack {get{return numberOfBeatsInTrack;}}

    #region GO Variables
    public MusicNote[] musicNoteArray;
    public MusicNote[] musicHalfNoteArray;
    #endregion

    private bool _isReady;
    public bool isReady
    {
        get{return _isReady;}
    }

    public void Init()
    {
        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;
        SetupVars();
        GenerateMusicSheet();

        _isReady = true;
    }

    #region UnityMethods

    void Update()
    {
        if(!GameManager.Instance.isReady) return;

        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        // Show note color 
        musicNoteArray[(int)songPositionInBeats].GetComponent<SpriteRenderer>().color = Color.red;
        
        if ((int)songPositionInBeats - 1 >= 0)
            musicNoteArray[(int)songPositionInBeats - 1].GetComponent<SpriteRenderer>().color = Color.grey;
    }
    #endregion

    #region Generate Level Note

    public void GenerateMusicSheet()
    {
        for (int i = 0; i < numberOfBeatsInTrack; i++)
        {
            float notePosition = i * distanceBetweenNotes;
            float halfNotePosition = i * distanceBetweenNotes + distanceBetweenNotes / 2;

            GenerateCompleteBeat(notePosition, i);

            if (i < numberOfBeatsInTrack - 1)
                GenerateHalfBeat(halfNotePosition, i);
        }
    }

    void GenerateCompleteBeat(float notePosition, int index)
    {
        MusicNote note = Instantiate(notePrefab, noteSpawnDirection * notePosition, Quaternion.identity).GetComponent<MusicNote>();
        note.transform.SetParent(noteParent);
        note.transform.eulerAngles = noteSpawnAngle;

        musicNoteArray[index] = note;
    }

    void GenerateHalfBeat(float halfNotePosition, int index)
    {
        MusicNote note = Instantiate(halfNotePrefab, noteSpawnDirection * halfNotePosition, Quaternion.identity).GetComponent<MusicNote>();
        note.transform.SetParent(noteParent);
        note.transform.eulerAngles = noteSpawnAngle;

        musicHalfNoteArray[index] = note;
    }
    #endregion

    public void SetupVars() 
    {
        musicFMODInstance = RuntimeManager.CreateInstance(musicFMODEvent); // FMOD Test Julien

        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        // set the clip to the source
        musicSource.clip = musicClip;        

        //Calculate the number of seconds between each beat
        secPerBeat = 60f / songBpm;

        numberOfBeatsInTrack = (int)(musicClip.length * (songBpm / 60));    // Length in seconds

        musicNoteArray = new MusicNote[numberOfBeatsInTrack];
        musicHalfNoteArray = new MusicNote[numberOfBeatsInTrack - 1];

        // Moved to PlayMusic for now
        // musicFMODInstance.start();      // FMOD Test Julien
        // musicFMODInstance.release();    // FMOD Test Julien
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    #region FMOD METHODS
    public void SetupVarsFMOD()
    {
        musicFMODInstance = MusicManager.MusicFMODInstance;// RuntimeManager.CreateInstance(musicFMODEvent);

        //Calculate the number of seconds between each beat
        secPerBeat = 60f / songBpm;

        FMOD.Studio.EventDescription evt;
        musicFMODInstance.getDescription(out evt);

        // numberOfBeatsInTrack = (int)(musicClip.length * (songBpm / 60));

        musicNoteArray = new MusicNote[numberOfBeatsInTrack];
        musicHalfNoteArray = new MusicNote[numberOfBeatsInTrack - 1];
    }

    [AOT.MonoPInvokeCallback(typeof (FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT SoundCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr instancePtr, System.IntPtr paramPtr)
    {
        FMOD.RESULT result;
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        switch(type)
        {
            case FMOD.Studio.EVENT_CALLBACK_TYPE.SOUND_PLAYED:
                {
                    FMOD.Sound sound = new FMOD.Sound(paramPtr);
                    result = sound.getLength(out uint length, FMOD.TIMEUNIT.MS);
                    if(result == FMOD.RESULT.OK)
                    {
                        return result;
                    }
                    Debug.Log("Sound Played. Length in ms: " + length);
                    break;
                }
        }
        return FMOD.RESULT.OK;
    }
    #endregion
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    public void ResetMusicSheet()
    {
        foreach (Transform child in noteParent)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void PlayMusic()
    {
        //Start the music
        // Debug.Log("StartMusic from SOUND CREATOR");
        // musicFMODInstance.start();      // FMOD Test Julien
        //musicFMODInstance.release();    // FMOD Test Julien
        //musicSource.Play();
    }
}