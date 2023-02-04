using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScore : MonoBehaviour
{
    public int pointObstacle;
    public void OnTriggerEnter(Collider col) {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
            PlayerManager.Instance.IncreaseScore(gameObject.GetComponent<BoxCollider>().bounds.extents.z, gameObject.transform.position.z, pointObstacle);
    }
}
