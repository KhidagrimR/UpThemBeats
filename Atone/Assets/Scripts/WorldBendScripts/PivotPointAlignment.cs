using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPointAlignment : MonoBehaviour
{
    // Ebauche pour si le point pivot est dissocié de l'objet Joueur
    [SerializeField] private Transform playerTransform;
    public float pSpeed {get; set;}    // Assigné par PlayerManager

    // Update is called once per frame
    void Update()
    {
        //transform.forward = playerTransform.forward;
        transform.position += transform.forward * pSpeed * Time.deltaTime;
    }
}
