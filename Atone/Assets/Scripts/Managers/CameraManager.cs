using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : Singleton<CameraManager>
{
    [Serializable]
    public class CameraEffect
    {
        public CinemachineImpulseSource impulseEffectRef;
        public enum EffectType {
            RedwallDestroy, RedwallDrop, Slide, SlideStop, WallrunLoop, WallrunHit, BopDestroy, Damage, Death
        }
        public EffectType effectType;
        public float impulseStr = 5f;
    }

    public List<CameraEffect> cameraEffects = new List<CameraEffect>();

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
        if(cameraEffect != null)
        {
            //Debug.Log("<color=red>Impulse</color>");
            cameraEffect.impulseEffectRef.GenerateImpulse(cameraEffect.impulseStr);
        }
    }

    public IEnumerator ShakeCameraWhileSliding()
    {
        while(PlayerManager.Instance.playerController.isSliding)
        {
            yield return new WaitForSeconds(.2f);
            //ShakeCamera(CameraEffect.EffectType.Slide);
        }
    }
}
