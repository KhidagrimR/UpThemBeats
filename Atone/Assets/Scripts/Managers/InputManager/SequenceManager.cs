using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SequenceManager : Singleton<SequenceManager>
{
    

    [InspectorReadOnly]
    // current instanciated Sequences
    public SequenceHandler currentSequence;

    public int startingSequenceIndex = 0; // 0 means the game start with the first sequence of the list, 5 means the game starts with the 6th sequence of the list

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
    
    public float sequenceFadeDuration = 3.0f;
    [InspectorReadOnly]
    public bool isDeathRestartingMusic;
// All sequences in prefab
    public List<GameObject> sequencesPrefab;
    public void Init()
    {
        LoadTargetSequenceByIndex(startingSequenceIndex);
        MusicManager.Instance.onMusicEnd += LoadNextSequence;
        _isReady = true;
    }

    private void OnDestroy()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.onMusicEnd -= LoadNextSequence;
    }

    // Sequence Manager work flow
    // Load a sequence, remove the previous(if there is one) then star the sequence
    



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
        // get spawn position
        Vector3 targetSpawnPosition = Vector3.zero;
        if (currentSequence != null)
        {
            // on prends le transform de la sequence précédente
            Transform centerRoad = currentSequence.lanes[1];
            targetSpawnPosition = new Vector3(0f, 0f, PlayerManager.Instance.playerController.transform.position.z) ; //centerRoad.GetChild(centerRoad.childCount - 1).position + new Vector3(0, 0, 10); // 10 is the length of a road tile
        
            // on détruit l'ancienne sequence
            Destroy(currentSequence.gameObject);
        }

        GameObject targetSequence = Instantiate(sequencesPrefab[targetSequenceIndex], targetSpawnPosition, Quaternion.identity);
        currentSequence = targetSequence.GetComponent<SequenceHandler>();
        PlayerManager.Instance.lanes = currentSequence.lanes;
    }

    void LoadNextSequence()
    {
        // si on ne loop PAS la sequence
        if (!isNextSequenceLooping)
        {
            // on passe a la suivante 
            startingSequenceIndex++;
            
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
                startingSequenceIndex++;
            }
        }
        LoadTargetSequenceByIndex(startingSequenceIndex);
        StartSequence();
    }

    public IEnumerator RestartCurrentSequence()
    {
        GameManager.Instance.isPlayerDead = true;
        isDeathRestartingMusic = true;
        PlayerManager.Instance.playerController.canPlayerMove = false;

        FadeInCamera();
        MusicManager.Instance.StopMusic();
        CameraManager.Instance.ShakeCamera(CameraManager.CameraEffect.EffectType.Death);

        yield return new WaitForSeconds(sequenceFadeDuration);

        // reset player position, set him temporary indestructible to avoid multiple life loss
        PlayerController player = PlayerManager.Instance.playerController;
        StartCoroutine(player.SetIndestructible());
        player.transform.position = new Vector3(0, 1.07f, player.currentCheckpoint.z);

        // load current Sequence once more
        LoadTargetSequenceByIndex(startingSequenceIndex);
        StartSequence();

        //Make player move again
        player.canPlayerMove = true;

        FadeOutCamera();

        isDeathRestartingMusic = false;
        GameManager.Instance.isPlayerDead = false;
    }

    void FadeInCamera()
    {
        SpriteRenderer cameraBlackFade = Camera.main.transform.GetChild(0).GetComponent<SpriteRenderer>();
        DOVirtual.Float(0f, 1f, sequenceFadeDuration, (float x) => 
        {
            cameraBlackFade.color = new Color(
            cameraBlackFade.color.r, 
            cameraBlackFade.color.g, 
            cameraBlackFade.color.b, 
            x);
        });
    }

    void FadeOutCamera()
    {
        SpriteRenderer cameraBlackFade = Camera.main.transform.GetChild(0).GetComponent<SpriteRenderer>();
        cameraBlackFade.color = new Color(
            cameraBlackFade.color.r, 
            cameraBlackFade.color.g, 
            cameraBlackFade.color.b, 
            0);
    }

    public void GoToTargetSequence(int sequenceIndexChosen)
    {
        isDeathRestartingMusic = true;
        MusicManager.Instance.StopMusic();
        startingSequenceIndex = sequenceIndexChosen;
        LoadTargetSequenceByIndex(startingSequenceIndex);
        StartSequence();
        isDeathRestartingMusic = false;
    }
}
