using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SequenceManager : Singleton<SequenceManager>
{
    public List<GameObject> sequencesPrefab;
    public List<GameObject> sequences;
    public int currentSequenceIndex = 0;
    public GameObject roadPrefab;
    public int transitionBetweenSequences;

    [HideInInspector] public SequenceHandler currentSequence
    {
        get{return sequences[sequences.Count - 1].GetComponent<SequenceHandler>();}
    }   


    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }
    private bool _isNextSequenceLooping;
    public bool isNextSequenceLooping
    {
        get { return _isNextSequenceLooping; }
        set { _isNextSequenceLooping = value; }
    }


    public void Init()
    {
        LoadTargetSequenceByIndex(currentSequenceIndex);
        MusicManager.Instance.onMusicEnd += LoadNextSequence;
        _isReady = true;
    }

    private void OnDestroy()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.onMusicEnd -= LoadNextSequence;
    }

    public void StartSequence()
    {
        Debug.Log("Start Sequence NOW");
        // on l'active
        currentSequence.gameObject.SetActive(true);
        currentSequence.Init();

        // On set le FMOD event pour qu il corresponde à la musique
        MusicManager.Instance.SetFMODEvent(currentSequence.musicFMODEvent);

        // On setup le music manager
        MusicManager.Instance.Init();
        MusicManager.Instance.StartMusicManager();
        MusicManager.Instance.PlayMusic();
    }

    public void LoadTargetSequenceByIndex(int targetSequenceIndex)
    {
        Vector3 targetSpawnPosition;
        if (sequences.Count == 0)
            targetSpawnPosition = Vector3.zero;
        else
        {
            // on prends le transform de la sequence précédente
            Transform centerRoad = sequences[sequences.Count - 1].GetComponent<SequenceHandler>().lanes[1];
            targetSpawnPosition = new Vector3(0f, 0f, PlayerManager.Instance.playerController.transform.position.z) ; //centerRoad.GetChild(centerRoad.childCount - 1).position + new Vector3(0, 0, 10); // 10 is the length of a road tile
        }

        GameObject sequence = Instantiate(sequencesPrefab[targetSequenceIndex], targetSpawnPosition, Quaternion.identity);
        PlayerManager.Instance.lanes = sequence.GetComponent<SequenceHandler>().lanes;
        sequences.Add(sequence);
    }

    void LoadNextSequence()
    {
        //GameManager.Instance.TogglePauseState();
        sequences[currentSequenceIndex].SetActive(false);

        // si on ne loop PAS la sequence
        if (!isNextSequenceLooping)
        {
            // on passe a la suivante 
            currentSequenceIndex++;
            
        }
        else // sinon
        {
            // on reset le loop
            isNextSequenceLooping = false;

            // on check les conditions de looping de la sequence (ex. score < 500)
            if (SequenceManager.Instance.currentSequence.CheckLoopConditions())
            {
                Debug.Log("Sequence will loop");

            }
            else
            {
                // si le joueur a reussi correctement le niveau loopable, on passe a la sequence suivante
                currentSequenceIndex++;
            }
        }
        LoadTargetSequenceByIndex(currentSequenceIndex);
        StartSequence();
    }

    float sequenceFadeDuration = 5.0f;
    public bool isDeathRestartingMusic;

    public IEnumerator RestartCurrentSequence()
    {
        isDeathRestartingMusic = true;
        SpriteRenderer cameraBlackFade = Camera.main.transform.GetChild(0).GetComponent<SpriteRenderer>();
        DOVirtual.Float(0f, 1f, sequenceFadeDuration, (float x) => 
        {
            cameraBlackFade.color = new Color(
            cameraBlackFade.color.r, 
            cameraBlackFade.color.g, 
            cameraBlackFade.color.b, 
            x);
        });
        
        PlayerManager.Instance.playerController.canPlayerMove = false;
        MusicManager.Instance.StopMusic();
        //sequences[currentSequenceIndex].SetActive(false);
        CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Recoil);

        yield return new WaitForSeconds(sequenceFadeDuration);
        PlayerManager.Instance.playerController.transform.position = new Vector3(0, 1.07f, PlayerManager.Instance.playerController.currentCheckpoint.z);
        PlayerManager.Instance.playerController.canPlayerMove = true;


        //LoadTargetSequenceByIndex(currentSequenceIndex);
        StartSequence();
        //sequences[sequences.Count - 1].gameObject.SetActive(false);

        cameraBlackFade.color = new Color(
            cameraBlackFade.color.r, 
            cameraBlackFade.color.g, 
            cameraBlackFade.color.b, 
            0);
        isDeathRestartingMusic = false;
    }
}
