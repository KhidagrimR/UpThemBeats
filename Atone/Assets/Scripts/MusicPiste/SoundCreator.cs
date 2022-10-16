using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;


public class SoundCreator : Singleton<SoundCreator>
{
    [Header("Setup Generation vars")]
    //Song beats per minute
    public float songBpm; //This is determined by the song you're trying to sync up to
    public float firstBeatOffset; //The offset to the first beat of the song in seconds
    public float distanceBetweenNotes; // La distance physique séparant 2 beats

    [Header("LevelDesign Settings")]
    public Vector3 noteSpawnDirection; // direction on x, y and z axis to spawn notes;
    public Vector3 noteSpawnAngle; // rotation on x, y and z axis to spawn notes;

    [Header("Component Settings")]
    public GameObject notePrefab;
    public GameObject halfNotePrefab;
    public Transform noteParent;

    [Header("Sounds Vars")]
    public AudioSource musicSource; //an AudioSource attached to this GameObject that will play the music.
    public AudioClip musicClip; //the music itself


    [Header("Sounds Info")]
    public float secPerBeat; //The number of seconds for each song beat
    public float songPosition; //Current song position, in seconds
    public float songPositionInBeats; //Current song position, in beats
    public float dspSongTime; //How many seconds have passed since the song started
    public int numberOfBeatsInTrack; //the number of beats in the track

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
        //SetupVars();

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
        // set the clip to the source
        musicSource.clip = musicClip;

        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds between each beat
        secPerBeat = 60f / songBpm;

        numberOfBeatsInTrack = (int)(musicClip.length * (songBpm / 60));

        musicNoteArray = new MusicNote[numberOfBeatsInTrack];
        musicHalfNoteArray = new MusicNote[numberOfBeatsInTrack - 1];
    }

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
        Debug.Log("StartMusic");
        musicSource.Play();
    }
}