using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AtoneWorldBend;
using DG.Tweening;

public class FirstPersonVisuals : MonoBehaviour
{
    [SerializeField]private WorldBendController m_wbc;
    public Camera m_Camera;

    private float secsPerBeat;
    
    void Start()
    {        
        secsPerBeat = 60f/132;// valeurs test. Utiliser "SoundCreator.Instance.secPerBeat;" à l'avenir
        DOTween.Init();
        WBCReset_Test();
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
}
