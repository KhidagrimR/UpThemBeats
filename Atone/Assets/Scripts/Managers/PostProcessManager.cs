using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

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
    UnityEngine.Rendering.Universal.Bloom bloom;

    #endregion

    public void Init()
    {
        volumeProfile = postProcessVolume.profile;

        _isReady = true;
    }

    public void ChangeColorToRed(float duration = 1.0f)
    {
       if(!volumeProfile.TryGet(out bloom)) 
         throw new System.NullReferenceException(nameof(bloom));

        // Change RED color
        float from = bloom.tint.value.r;
        float to = bloomRedColor.value.r;
        DOVirtual.Float(from, to, duration, (float x) => {
            bloom.tint.value = new Color(x,bloom.tint.value.g,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change GREEN color
        from = bloom.tint.value.g;
        to = bloomRedColor.value.g;
        DOVirtual.Float(from, to, duration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,x,bloom.tint.value.b,bloom.tint.value.a);
        });

        // Change BLUE color
        from = bloom.tint.value.b;
        to = bloomRedColor.value.b;
        DOVirtual.Float(from, to, duration, (float x) => {
            bloom.tint.value = new Color(bloom.tint.value.r,bloom.tint.value.g,x,bloom.tint.value.a);
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColorToRed(10.0f);
        }
    }

    
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
