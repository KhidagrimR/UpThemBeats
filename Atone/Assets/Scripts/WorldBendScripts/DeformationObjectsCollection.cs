using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformationObjectsCollection : MonoBehaviour
{
    [Tooltip("Drag all the MarkerBendData scriptable objects containing world deformation info for the current level")]
    [SerializeField] private MarkerBendData[] bendMarkerObjects = new MarkerBendData[0];

    // On utilisera Animator.StringToHash() au lieu de faire notre propre fonction de Hash;
    // Dictionnaire: clef = le nom du marker converti en int | valeur = les valeurs du BendData 
    private static Dictionary<int, BendData> levelBendMarkers;
    public static Dictionary<int, BendData> LevelBendMarkers {get => levelBendMarkers;}

    private void Awake()
    {
        BuildDictionary();
    }

    private void BuildDictionary()
    {
        int objectCount = bendMarkerObjects.Length;
        if(objectCount == 0)
        {
            Debug.LogWarning("No MarkerBendData objects found to build the dictionary");
            return;
        }

        levelBendMarkers = new Dictionary<int, BendData>(objectCount);
        
        int hashedName;

        for(int i = 0; i < objectCount; i++)
        {
            hashedName = Animator.StringToHash(bendMarkerObjects[i].FMODMarkerName);
            //Debug.Log("hashedName " + hashedName);
            levelBendMarkers.Add(hashedName, bendMarkerObjects[i].DeformationData);           

        }
    }
}
