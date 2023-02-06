using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLaneTrigger : MonoBehaviour
{
    [InspectorReadOnly]
    public bool isTrigger;

    public int pointObstacle;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
        {
            if (col.TryGetComponent<PlayerController>(out PlayerController player))
            {
                isTrigger = true;
                player.isAbleToChangeLane = true;
                PlayerManager.Instance.gameObjectTriggerChangeLane = this;
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
        {
            if (col.TryGetComponent<PlayerController>(out PlayerController player))
            {
                isTrigger = false;
                player.isAbleToChangeLane = false;
                PlayerManager.Instance.gameObjectTriggerChangeLane = null;
            }
        }
    }
}
