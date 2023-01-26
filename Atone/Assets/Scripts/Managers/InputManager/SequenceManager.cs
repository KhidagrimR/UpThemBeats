using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : Singleton<SequenceManager>
{
    public List<GameObject> sequencesPrefab;
    public List<GameObject> sequences;
    public int currentSequenceIndex = 0;
    public GameObject roadPrefab;
    public int transitionBetweenSequences;

    [HideInInspector] public SequenceHandler currentSequence;


    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }


    public void Init()
    {
        LoadTargetSequenceByIndex(currentSequenceIndex);
        MusicManager.Instance.onMusicEnd += LoadNextSequence;
        _isReady = true;
    }

    private void OnDestroy()
    {
        MusicManager.Instance.onMusicEnd -= LoadNextSequence;
    }

    public void StartSequence()
    {
        currentSequence = sequences[currentSequenceIndex].GetComponent<SequenceHandler>();
        currentSequence.gameObject.SetActive(true);
        currentSequence.Init();

        MusicManager.Instance.SetFMODEvent(currentSequence.musicFMODEvent);
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

            Transform centerRoad = sequences[targetSequenceIndex - 1].GetComponent<SequenceHandler>().centerRoad;
            targetSpawnPosition = centerRoad.GetChild(centerRoad.childCount - 1).position + new Vector3(0,0,10); // 10 is the length of a road tile
        }

        GameObject sequence = Instantiate(sequencesPrefab[targetSequenceIndex], targetSpawnPosition, Quaternion.identity);
        sequences.Add(sequence);
    }

    void LoadNextSequence()
    {
        //GameManager.Instance.TogglePauseState();
        sequences[currentSequenceIndex].SetActive(false);
        currentSequenceIndex++;
        LoadTargetSequenceByIndex(currentSequenceIndex);
        StartSequence();
    }
}
