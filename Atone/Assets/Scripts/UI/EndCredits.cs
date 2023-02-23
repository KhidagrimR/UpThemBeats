using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    public GameObject credits;

    public void DisableCredit() {
        credits.SetActive(false);
    }
}
