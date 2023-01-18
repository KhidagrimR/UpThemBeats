using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName ="Bend on FMOD Marker", menuName ="ScriptableObjects/MarkerBendData", order = 1)]
public class MarkerBendData : ScriptableObject
{   
    // Ces données seront normalement utilisées par le script FirstPersonVisuals.cs

    // Principe: 
    // * Donner le nom du marqueur FMOD qui déclenchera la déformation
    // * Remplir les informations nécessaires pour donner l'aspect désiré
    // * Choisir les paramètres de l'animation DOTTWEEN (à venir)
    [Header("Nom du marqueur FMOD")]
    [SerializeField]private string fMODMarkerName;

    // [Header("Paramètres à affecter")]
    // [SerializeField]private bool affectVertSize;
    // [SerializeField]private bool affectVertOffset;
    // [SerializeField]private bool affectHorSize, affectHorOffset;
    // [SerializeField]private bool affectCurvatureAxis, affectCurvatureSize, affectCurvatureOffset;

    // [Header("Paramètres de déformation")]
    // [SerializeField]private float verticalBendSize;
    // [SerializeField]private float verticalBendOffset;
    // [SerializeField]private float horizontalBendSize, horizontalBendOffset;
    // [SerializeField]private Vector3 curvatureBendAxis;
    // [SerializeField]private float curvatureBendSize, curvatureBendOffset;

    // [Header("Paramètres de tweening (en cours)")]
    // [SerializeField]private float beatDurationOfTween;
    // [SerializeField]private Ease tweenEase;  // If none, set to Unset
    // //
    // //ease-in: slow at the beginning, fast/abrupt at the end
    // //ease-out: fast/abrupt at the beginning, slow at the end
    
    // FORMAT:
    // public static Tweener Vector3(Vector3 from, Vector3 to, float duration, TweenCallback<Vector3> onVirtualUpdate);
    // public static Tweener Float(float from, float to, float duration, TweenCallback<float> onVirtualUpdate);
    // public static Tweener Int(int from, int to, float duration, TweenCallback<int> onVirtualUpdate);

    // Example: DOVirtual.Float(m_wbc.BendVerticalSize, 4.26f, duration,v =>{m_wbc.BendVerticalSize = v;}).SetEase(Ease.OutBounce);
    // (See FirstPersonVisuals for more examples)

    [Header("Données de déformation")]
    [SerializeField] private BendData deformationData;
    public BendData DeformationData {get => deformationData;}

    // Public accessors to prevent write operations during runtime
    public string FMODMarkerName {get => fMODMarkerName;}
    //
    public bool AffectVertSize {get => deformationData.affectVertSize;}
    public bool AffectVertOffset {get => deformationData.affectVertOffset;}
    public bool AffectHorSize {get => deformationData.affectHorSize;}
    public bool AffectHorOffset {get => deformationData.affectHorOffset;}
    public bool AffectCurvatureAxis {get => deformationData.affectCurvatureAxis;}
    public bool AffectCurvatureSize {get => deformationData.affectCurvatureSize;}
    public bool AffectCurvatureOffset {get => deformationData.affectCurvatureOffset;}
    //
    public float VerticalBendSize {get => deformationData.verticalBendSize;}
    public float VerticalBendOffset {get => deformationData.verticalBendOffset;}
    public float HorizontalBendSize {get => deformationData.horizontalBendSize;}
    public float HorizontalBendOffset {get => deformationData.horizontalBendOffset;}
    public Vector3 CurvatureBendAxis { get => deformationData.curvatureBendAxis;}
    public float CurvatureBendSize {get => deformationData.curvatureBendSize;}
    public float CurvatureBendOffset {get => deformationData.curvatureBendOffset;}
    //
    public float BeatDurationOfTween {get => deformationData.beatDurationOfTween;}
    public Ease TweenEase {get => deformationData.tweenEase;}

}

[System.Serializable]
public struct BendData 
{    
    [Header("Paramètres à affecter")]
    public bool affectVertSize;
    public bool affectVertOffset;
    public bool affectHorSize, affectHorOffset;
    public bool affectCurvatureAxis, affectCurvatureSize, affectCurvatureOffset;

    [Header("Paramètres de déformation")]
    public float verticalBendSize;
    public float verticalBendOffset;
    public float horizontalBendSize, horizontalBendOffset;
    public Vector3 curvatureBendAxis;
    public float curvatureBendSize, curvatureBendOffset;

    [Header("Paramètres de tweening (en cours)")]
    public float beatDurationOfTween;
    public Ease tweenEase;  // If none, set to Unset
    //
    //ease-in: slow at the beginning, fast/abrupt at the end
    //ease-out: fast/abrupt at the beginning, slow at the end
}
