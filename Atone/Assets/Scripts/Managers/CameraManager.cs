using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineVirtualCamera cmVcam;
    [Serializable]
    public class CameraEffect
    {
        public CinemachineImpulseSource impulseEffectRef;
        public enum EffectType
        {
            RedwallDestroy, RedwallDrop, Slide, SlideStop, WallrunLoop, WallrunHit, BopDestroy, Damage, Death
        }
        public EffectType effectType;
        public float impulseStr = 5f;
    }

    [Serializable]
    public class HeadbobNoiseSettings
    {
        public HeadbobType headbobType;
        public enum HeadbobType
        {
            UpstandBob, SlideBob, WallrunBob
        }
        public NoiseSettings noiseSettings;
    }

    public List<CameraEffect> cameraEffects = new List<CameraEffect>();
    public List<HeadbobNoiseSettings> headbobNoiseSettings = new List<HeadbobNoiseSettings>();

    // Start is called before the first frame update
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShakeCamera(CameraEffect.EffectType.Explosion);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShakeCamera(CameraEffect.EffectType.Bump);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShakeCamera(CameraEffect.EffectType.Recoil);
        }
    }*/

    public void ShakeCamera(CameraEffect.EffectType effect)
    {
        CameraEffect cameraEffect = cameraEffects.Find(x => x.effectType == effect);
        if (cameraEffect != null)
        {
            //Debug.Log("<color=red>Impulse</color>");
            cameraEffect.impulseEffectRef.GenerateImpulse(cameraEffect.impulseStr);
        }
    }

     public void ChangeHeadbobType(HeadbobNoiseSettings.HeadbobType _headbobType)
    {
        HeadbobNoiseSettings headbobNoiseSetting = headbobNoiseSettings.Find(x => x.headbobType == _headbobType);
        if (headbobNoiseSetting != null)
        {
            //Debug.Log("<color=red>Impulse</color>");
            cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = headbobNoiseSetting.noiseSettings;
        }
    }

    public IEnumerator ShakeCameraWhileSliding()
    {
        while (PlayerManager.Instance.playerController.isSliding)
        {
            yield return new WaitForSeconds(.2f);
            //ShakeCamera(CameraEffect.EffectType.Slide);
        }
    }
}
