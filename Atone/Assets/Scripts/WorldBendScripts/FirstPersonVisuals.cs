using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtoneWorldBend;
using DG.Tweening;

[RequireComponent(typeof(DeformationObjectsCollection))]
public class FirstPersonVisuals : MonoBehaviour
{
    [SerializeField]private WorldBendController m_wbc;
    public Camera m_Camera;

    
    private float secsPerBeat;
    
    void Awake()
    {
        MusicManager.markerUpdated += ExamineMarker;
    }

    void Start()
    {
        secsPerBeat = 60f/132;// valeurs test. Utiliser "SoundCreator.Instance.secPerBeat;" à l'avenir
        DOTween.Init();
        //WBCReset_Test();
    }

    // G H J K L M
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)){
            DOTween.KillAll();
            WBCReset_Test();
        }
        else if(Input.GetKeyDown(KeyCode.H)){
            DOTween.KillAll();
            HardSet();
        }
        else if(Input.GetKeyDown(KeyCode.J)){
            FirstTweenTest();
        }
        else if(Input.GetKeyDown(KeyCode.K)){
            DOTween.KillAll();
            SynchronizedTweenTest_00(4);
        }
    }

    void OnDisable()
    {
        MusicManager.markerUpdated -= ExamineMarker;
    }

    #region LECTEUR DE SCRIPTABLE OBJECT

    void ExamineMarker()
    {        
        int lastHashedMarkerName = Animator.StringToHash(MusicManager.Instance.timelineInfo.markerHit);
        //Debug.Log(lastHashedMarkerName);
        
        if(!DeformationObjectsCollection.LevelBendMarkers.ContainsKey(lastHashedMarkerName))
        {
            if(string.Compare(MusicManager.Instance.timelineInfo.markerHit, "LoopSequence") == 0)
            {
                SequenceManager.Instance.isNextSequenceLooping = true;
            }
            if(string.Compare(MusicManager.Instance.timelineInfo.markerHit, "Start") == 0)
            {
                if(MusicManager.Instance.onMusicStart != null)
                    MusicManager.Instance.onMusicStart();
            }
            return;
        }

        SynchronizedTween(DeformationObjectsCollection.LevelBendMarkers[lastHashedMarkerName]);
    }
    void SynchronizedTween(BendData bendData)
    {
        float duration = bendData.beatDurationOfTween * MusicManager.Instance.SecPerBeat;

        if(bendData.affectCurvatureAxis) 
        { 
            DOVirtual.Vector3(m_wbc.BendCurvatureAxis, bendData.curvatureBendAxis, duration, v =>{m_wbc.BendCurvatureAxis = v;}).SetEase(bendData.tweenEase);
        }
        if(bendData.affectCurvatureSize) 
        { 
            DOVirtual.Float(m_wbc.BendCurvatureSize, bendData.curvatureBendSize, duration,v =>{m_wbc.BendCurvatureSize = v;}).SetEase(bendData.tweenEase);
        }
        if(bendData.affectCurvatureOffset) 
        { 
            DOVirtual.Float(m_wbc.BendCurvatureOffset, bendData.curvatureBendOffset, duration, v =>{m_wbc.BendCurvatureOffset = v;}).SetEase(bendData.tweenEase);
        }
        if(bendData.affectHorSize) 
        { 
            DOVirtual.Float(m_wbc.BendHorizontalSize, bendData.horizontalBendSize, duration, v =>{m_wbc.BendHorizontalSize = v;}).SetEase(bendData.tweenEase);
        }
        if(bendData.affectHorOffset) 
        { 
            DOVirtual.Float(m_wbc.BendHorizontalOffset, bendData.horizontalBendOffset, duration, v =>{m_wbc.BendHorizontalOffset = v;}).SetEase(bendData.tweenEase);
        }
        if(bendData.affectVertSize) 
        { 
            DOVirtual.Float(m_wbc.BendVerticalSize, bendData.verticalBendSize, duration, v =>{m_wbc.BendVerticalSize = v;}).SetEase(bendData.tweenEase);
        }
        if(bendData.affectVertOffset) 
        { 
            DOVirtual.Float(m_wbc.BendVerticalOffset, bendData.verticalBendOffset, duration, v =>{m_wbc.BendVerticalOffset = v;}).SetEase(bendData.tweenEase);
        }
    }

    #endregion

    #region TWEEN TEST METHODS
    void HardSet(){
        m_wbc.BendVerticalSize = 0;
        m_wbc.BendVerticalOffset = 0;
        m_wbc.BendHorizontalSize = -0.6f;
        m_wbc.BendHorizontalOffset = 0;
        m_wbc.BendCurvatureAxis = new Vector3(0,0.08f,1);
        m_wbc.BendCurvatureSize = 10;
        m_wbc.BendCurvatureOffset = 5;
    }
    void FirstTweenTest() {
       DOVirtual.Float(m_wbc.BendCurvatureSize, -10, 2,v =>{m_wbc.BendCurvatureSize = v;}); // 
    }

    void FirstTweenTestBeats(float beatAmount) {
        float duration = beatAmount * secsPerBeat;
       DOVirtual.Float(m_wbc.BendCurvatureSize, -10, duration,v =>{m_wbc.BendCurvatureSize = v;}); // 
    }

    void SynchronizedTweenTest_00(float beatAmount){
        Vector3 temp = new Vector3(0,0.1f,0);
        float duration = beatAmount * secsPerBeat;
        DOVirtual.Vector3(m_wbc.BendCurvatureAxis, temp, duration, v =>{m_wbc.BendCurvatureAxis = v;});
        DOVirtual.Float(m_wbc.BendCurvatureSize, -10f, duration,v =>{m_wbc.BendCurvatureSize = v;}).SetEase(Ease.OutBounce); // 
        DOVirtual.Float(m_wbc.BendHorizontalSize, -0.6f, duration,v =>{m_wbc.BendHorizontalSize = v;}).SetEase(Ease.OutBounce);
        DOVirtual.Float(m_wbc.BendVerticalSize, 4.26f, duration,v =>{m_wbc.BendVerticalSize = v;}).SetEase(Ease.OutBounce);
    }


    void WBCReset_Test(){
        m_wbc.BendVerticalSize = 0;
        m_wbc.BendVerticalOffset = 0;
        m_wbc.BendHorizontalSize = 0;
        m_wbc.BendHorizontalOffset = 0;
        m_wbc.BendCurvatureAxis = new Vector3(0,0,1);
        m_wbc.BendCurvatureSize = 0;
        m_wbc.BendCurvatureOffset = 0;
    }
    #endregion
}
