using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using FMODUnity;

public class BopTriggerArrival : MonoBehaviour
{
    public GameObject bopVisuel;
    public GameObject arrivalPointBop;
    public GameObject bopTrigger;

    public GameObject SFXManager;

    public TextMeshProUGUI countDownText;
    private int _countDown;
    int countDown
    {
        get { return _countDown; }
        set
        {
            _countDown = value;
            countDownText.text = value.ToString();
        }

    }
    public float travelTimeToCenter;
    public float timeStayCenter;

    // duration while the drone make small pauses while moving
    public float timePausesOnMovement = 0.25f;
    public float speedMultiplier = 1.0f;

    void Start()
    {
        bopVisuel.gameObject.SetActive(false);
    }


    public void OnTriggerEnter(Collider other)
    {
        bopVisuel.gameObject.SetActive(true);
        StartCoroutine(LaunchArrival());
    }

    public void InitDistance()
    {
        float distance = PlayerManager.Instance.playerController.playerSpeed * travelTimeToCenter;
        //gameObject.transform.position = new Vector3(bopTrigger.transform.position.x, bopTrigger.transform.position.y, bopTrigger.transform.position.z - distance);
    }

    public IEnumerator LaunchArrival()
    {
        SFXManager.GetComponent<SFXManagerBop>().arrivalSound.Play();
        countDown = 2;

        Vector3 startPoint = Vector3.zero + bopVisuel.transform.position;
        Vector3 endpoint = arrivalPointBop.transform.position;
        float direction = startPoint.x > endpoint.x ? 1 : -1;

        float halfToCenterDuration = travelTimeToCenter / 2;

        // on déplace le bop par accoups
        // on a besoin de la durée d'un beat
        float beatDuration = SoundCreator.Instance.SecPerBeat;

        // le bop se déplace en 2 temps
        // on doit connaitre la moitié de la distance
        Vector3 halfDistancePoint = new Vector3(Vector3.Distance(startPoint, endpoint) / 2 * direction, startPoint.y, startPoint.z);

        bopVisuel.gameObject.transform.DOMove(halfDistancePoint, halfToCenterDuration * speedMultiplier - timePausesOnMovement).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(halfToCenterDuration);
        countDown = 1;

        //SFXManager.GetComponent<SFXManagerBop>().loadDataSound.Play();
        bopVisuel.gameObject.transform.DOMove(endpoint, halfToCenterDuration * speedMultiplier - timePausesOnMovement).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(halfToCenterDuration);
        countDown = 0;


        yield return new WaitForSeconds(timeStayCenter);
        if(!gameObject.transform.parent.GetComponentInChildren<BopTriggerDestruction>().isDestroy)
            SFXManager.GetComponent<SFXManagerBop>().validationDataSound.Play();
        bopVisuel.gameObject.transform.DOMoveY(-20, halfToCenterDuration * speedMultiplier);
    }
}
