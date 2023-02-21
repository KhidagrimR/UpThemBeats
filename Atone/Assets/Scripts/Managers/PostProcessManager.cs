using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using System;

public class PostProcessManager : Singleton<PostProcessManager>
{
    public Volume postProcessVolume;
    VolumeProfile volumeProfile;
    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }


    #region postprocess param
    [Header("Bloom")]
    public ColorParameter bloomBlueColor;
    public ColorParameter bloomRedColor;
    public ColorParameter bloomYellowColor;
    private Color bloomStartingColor = new Color();
    UnityEngine.Rendering.Universal.Bloom bloom;

    #endregion
    [Serializable]
    public class VignetteData
    {
        public enum VignetteDataType {Slide,Standing,WallrunLeft,WallrunRight,Snap,Break,Hit,Death};
        public VignetteDataType vignetteDataType;

        public ColorParameter color;
        public Vector2 center;
        [Range(0,1)]
        public float intensity;
        [Range(0.01f,1)]
        public float smoothness;

        public float transitionDuration = 0.25f;
    }

    [Header("Vignette")]
    public List<VignetteData> vignetteDataList;
    UnityEngine.Rendering.Universal.Vignette vignette;
    VignetteData vignetteStartingParam;

    public void Init()
    {
        volumeProfile = postProcessVolume.profile;

        if(!volumeProfile.TryGet(out bloom)) 
         throw new System.NullReferenceException(nameof(bloom));

         if(!volumeProfile.TryGet(out vignette)) 
         throw new System.NullReferenceException(nameof(vignette));

        bloomStartingColor = bloom.tint.value;
        Debug.Log("Vignette = "+vignette.color);

        _isReady = true;
    }

    #region Bloom
    public void ChangeColorToRed(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomRedColor.value.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomRedColor.value.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomRedColor.value.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ChangeColorToYellow(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomYellowColor.value.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomYellowColor.value.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomYellowColor.value.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ChangeColorToBlue(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomBlueColor.value.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomBlueColor.value.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomBlueColor.value.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ResetBloomColor(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomStartingColor.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomStartingColor.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomStartingColor.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ChangeBloomIntensity(float val = 2.0f)
    {
        bloom.intensity = new MinFloatParameter(val, 0f);
    }

    #endregion
    #region Vignette

    //PostProcessManager.Instance.ChangeVignette(PostProcessManager.VignetteData.VignetteDataType.typeA);
    public void ChangeVignette(VignetteData.VignetteDataType _vignetteDataType)
    {
        VignetteData vignetteData = vignetteDataList.Find(x => x.vignetteDataType == _vignetteDataType);

        if(vignetteData != null)
            ChangeVignette(vignetteData);
        else
            throw new Exception("That vignette data :"+_vignetteDataType.ToString()+" can not be found");
    }

    void ChangeVignette(VignetteData vignetteData)
    {
        //Debug.Log("Change Vignette CALLLED in "+vignetteData.transitionDuration);
        DOVirtual.Float(0f, 1f, vignetteData.transitionDuration, (float x) => 
        {
           vignette.color.value = new Color (
                vignetteData.color.value.r * x,
                vignetteData.color.value.b * x,
                vignetteData.color.value.g * x
            );
            vignette.intensity.value = vignetteData.intensity * x;
            vignette.smoothness.value = vignetteData.smoothness * x;
            vignette.center.value = vignetteData.center;
        });
    }   

    #endregion
    /*void Start()
    {
       var vignette = ScriptableObject.CreateInstance<Vignette>();
       vignette.enabled.Override(true);
       vignette.intensity.Override(1f);
       var volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, vignette);
       volume.weight = 0f;
       DOTween.Sequence()
          .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 1f, 1f))
          .AppendInterval(1f)
          .Append(DOTween.To(() => volume.weight, x => volume.weight = x, 0f, 1f))
          .OnComplete(() =>
          {
               RuntimeUtilities.DestroyVolume(volume, true, true);
                Destroy(this);
          });
    }*/

    /*
    UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
 
// You can leave this variable out of your function, so you can reuse it throughout your class.
UnityEngine.Rendering.Universal.Vignette vignette;
 
if(!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
 
vignette.intensity.Override(0.5f);
    
    
    
    
    
    
    */
    
}
