using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtoneWorldBend;
using DG.Tweening;
using System;

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
    /*void Update()
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
    }*/

    void OnDisable()
    {
        MusicManager.markerUpdated -= ExamineMarker;
    }

    #region LECTEUR DE SCRIPTABLE OBJECT

    void ExamineMarker()
    {        
        int lastHashedMarkerName = Animator.StringToHash(MusicManager.Instance.timelineInfo.markerHit);
        string marker = MusicManager.Instance.timelineInfo.markerHit;
        //Debug.Log(lastHashedMarkerName);
        
        if(!DeformationObjectsCollection.LevelBendMarkers.ContainsKey(lastHashedMarkerName))
        {
            if (string.Compare(marker, "LoopSequence") == 0)
            {
                SequenceManager.Instance.isNextSequenceLooping = true;
            }
            else if (string.Compare(marker, "Start") == 0)
            {
                if (MusicManager.Instance.onMusicStart != null)
                    MusicManager.Instance.onMusicStart();
            }
            else if (marker.Contains("st_"))
            { 
                StartCoroutine(Subtitle.Instance.LaunchSubtitle(marker));
            }
            else if (string.Compare(marker, "AppearSages") == 0)
            {
                // faire apparaitre les sages
                StartCoroutine(PlayerManager.Instance.MakeSagesAppear());

            }
            else if (string.Compare(marker, "DisSages") == 0)
            {
                // faire disparaitre les sages
                StartCoroutine(PlayerManager.Instance.MakeSagesDisappear());
            }
            else if (marker.Contains("Bloom_"))
            {
                // Changer le Bloom
                if (string.Compare(marker.Substring(marker.Length - 1), "r") == 0)
                {
                    //red
                    PostProcessManager.Instance.ChangeColorToRedSage();
                }
                else if (string.Compare(marker.Substring(marker.Length - 1), "y") == 0)
                {
                    //green
                    PostProcessManager.Instance.ChangeColorToYellowSage();
                }
                else
                {
                    //blue
                    PostProcessManager.Instance.ChangeColorToBlueSage();
                }

            }
            else if (string.Compare(marker, "ResetBloom") == 0)
            {
                // Reset le bloom
                PostProcessManager.Instance.ResetSagesBloomColor();
            }
            else if (string.Compare(marker, "FadeIn") == 0)
            {
                SequenceManager.Instance.FadeInCamera(0.5f);
            }
            else if (string.Compare(marker, "FadeOut") == 0)
            {
                SequenceManager.Instance.FadeOutCamera(0.5f);
            }
            else if (marker.Contains("DisplaySageQuips_"))
            {
                // Changer le Bloom
                if (string.Compare(marker.Substring(marker.Length - 1), "r") == 0)
                {
                    //red
                    PlayerManager.Instance.playerController.DisplayRedSageQuips();
                }
                else if (string.Compare(marker.Substring(marker.Length - 1), "y") == 0)
                {
                    //yellow
                    PlayerManager.Instance.playerController.DisplayYellowSageQuips();
                }
                else
                {
                    //blue
                    PlayerManager.Instance.playerController.DisplayBlueSageQuips();
                }

            }
            else if (string.Compare(marker, "HideSageQuips") == 0)
            {
                PlayerManager.Instance.playerController.HideSageQuips();
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
