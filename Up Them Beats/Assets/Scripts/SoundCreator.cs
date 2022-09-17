using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;


public class SoundCreator : Singleton<SoundCreator> 
{
    #region Sound variables
        [Header("Sounds Settings")]
        //Song beats per minute
        //This is determined by the song you're trying to sync up to
        public float songBpm;

        //The number of seconds for each song beat
        public float secPerBeat;

        //Current song position, in seconds
        public float songPosition;

        //Current song position, in beats
        public float songPositionInBeats;

        //How many seconds have passed since the song started
        public float dspSongTime;

        //an AudioSource attached to this GameObject that will play the music.
        public AudioSource musicSource;

        //the music itself
        public AudioClip musicClip;

        //The offset to the first beat of the song in seconds
        public float firstBeatOffset;

        //the number of beats in the track
        public int numberOfBeatsInTrack;
    #endregion

    #region 
        [Header("Component Settings")]
        public GameObject notePrefab;
        public Transform noteParent;

        // La distance physique séparant 2 beats
        public float distanceBetweenNotes;

        // direction on x, y and z axis to spawn notes;
        public Vector3 noteSpawnDirection;
    #endregion

    #region Game Variables
        public MusicNote[] musicNoteArray;


    #endregion

    void Start()
    {
        // set the clip to the source
        musicSource.clip = musicClip;

        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        numberOfBeatsInTrack = (int)(musicClip.length * (songBpm / 60));

        musicNoteArray = new MusicNote[numberOfBeatsInTrack];

        for(int i = 0; i < numberOfBeatsInTrack; i++) {
            float notePosition = i * distanceBetweenNotes;
            MusicNote note = Instantiate(notePrefab, noteSpawnDirection * notePosition, Quaternion.identity).GetComponent<MusicNote>();
            note.transform.SetParent(noteParent);

            musicNoteArray[i] = note;
        }


        //Start the music
        musicSource.Play();

    }

    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        // Show note color 
        musicNoteArray[(int)songPositionInBeats].GetComponent<SpriteRenderer>().color = Color.red;

        if((int)songPositionInBeats - 1 >= 0)
            musicNoteArray[(int)songPositionInBeats - 1].GetComponent<SpriteRenderer>().color = Color.grey;
    }

}