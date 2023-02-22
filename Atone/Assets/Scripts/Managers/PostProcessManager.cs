using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using System;

public class PostProcessManager : Singleton<PostProcessManager>
{
    [InspectorReadOnly] public bool isSagesActives = false;
    public Volume postProcessVolume;
    VolumeProfile volumeProfile;
    private bool _isReady;
    public bool isReady
    {
        get { return _isReady; }
    }

    #region postprocess param
    private Color bloomStartingColor = new Color();
    UnityEngine.Rendering.Universal.Bloom bloom;

    [Header("Bloom")]
    public ColorParameter bloomBlueColor;
    public ColorParameter bloomRedColor;
    public ColorParameter bloomYellowColor;

    [Header("SagesBloom")]
    public ColorParameter bloomSageBlueColor;
    public ColorParameter bloomSageRedColor;
    public ColorParameter bloomSageYellowColor;
    
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

        public VignetteData(VignetteDataType pVignetteDataType, Color pColor, Vector2 pCenter, float pIntensity, float pSmoothness, float pTransitionDuration  )
        {
            vignetteDataType = pVignetteDataType;
            color = new ColorParameter(pColor, false);

            center = pCenter;
            intensity = pIntensity;
            smoothness = pSmoothness;
            transitionDuration = pTransitionDuration;
        }

        public VignetteData(VignetteData vData)
        {
            vignetteDataType = vData.vignetteDataType;
            ColorParameter color = vData.color;
            Vector2 center = vData.center;
            float intensity = vData.intensity;
            float smoothness = vData.smoothness;
            float transitionDuration = vData.transitionDuration;
        }
    }

    [Header("Vignette")]
    public List<VignetteData> vignetteDataList;
    UnityEngine.Rendering.Universal.Vignette vignette;
    VignetteData vignetteStartingParam;
    VignetteData currentVignetteData;

    public void Init()
    {
        volumeProfile = postProcessVolume.profile;

        if(!volumeProfile.TryGet(out bloom)) 
         throw new System.NullReferenceException(nameof(bloom));

         if(!volumeProfile.TryGet(out vignette)) 
         throw new System.NullReferenceException(nameof(vignette));

        bloomStartingColor = bloom.tint.value;
        ChangeVignette(VignetteData.VignetteDataType.Standing);

        _isReady = true;
    }

    #region Bloom
    public void ChangeColorToRed(float transitionDuration = 1.0f)
    {
        if(isSagesActives) return;
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
        if(isSagesActives) return;
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
        if(isSagesActives) return;
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
        if(isSagesActives) return;
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
    #region Sage bloom
    public void ChangeColorToRedSage(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomSageRedColor.value.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomSageRedColor.value.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomSageRedColor.value.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ChangeColorToYellowSage(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomSageYellowColor.value.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomSageYellowColor.value.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomSageYellowColor.value.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ChangeColorToBlueSage(float transitionDuration = 1.0f)
    {
        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomSageBlueColor.value.r;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomSageBlueColor.value.g;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomSageBlueColor.value.b;
        DOVirtual.Float(from, to, transitionDuration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    public void ResetSagesBloomColor(float transitionDuration = 1.0f)
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

    VignetteData previousVignetteData;

    void ChangeVignette(VignetteData vignetteData)
    {
        if(currentVignetteData == null)
        {
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
        else
        {
            previousVignetteData = new VignetteData(currentVignetteData);
            DOVirtual.Float(0f, 1f, vignetteData.transitionDuration, (float x) => 
            {
                vignette.color.value = new Color (
                    vignetteData.color.value.r * x,
                    vignetteData.color.value.b * x,
                    vignetteData.color.value.g * x
                );
                // valeur_finale = valeur_precedente + (valeur_choisie - valeur_precedente) * x; 
                // si valeur precedente = 0.1 et valeur choisie = 0.5
                // vf = 0.1 + 0;
                // vf = 0.1 + (0.5 - 0.1) * 0.1 = 0.1 + 0.04
                // vf = 0.1

                vignette.intensity.value = previousVignetteData.intensity + (vignetteData.intensity - previousVignetteData.intensity) * x; // 0.5 => 0.1
                vignette.smoothness.value = previousVignetteData.smoothness + (vignetteData.smoothness - previousVignetteData.smoothness)* x;
                vignette.center.value = vignetteData.center;
            });
        }

        //Debug.Log("Change Vignette CALLLED in "+vignetteData.transitionDuration);

        currentVignetteData = vignetteData;
    }   

    #endregion
}
