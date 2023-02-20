using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawsHandler : MonoBehaviour
{
    public WallAnimationTrigger[] wallAnimationTrigger;
    public float delayBetweenWallTrigger = 0.25f;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(PlayerManager.PLAYER_TAG))
        {
            StartCoroutine(StartClawAnim());
        }
    }

    IEnumerator StartClawAnim()
    {
        for(int i = 0, n = wallAnimationTrigger.Length; i < n; i++ )
        {
            wallAnimationTrigger[i].TriggerWallAnimation();
            yield return new WaitForSeconds(delayBetweenWallTrigger);
        }
    }
}
