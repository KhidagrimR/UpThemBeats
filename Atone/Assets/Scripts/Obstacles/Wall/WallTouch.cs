using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTouch : MonoBehaviour
{
    public bool canPlayerDodgeWithSlide = false;

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(PlayerManager.PLAYER_TAG))
        {
            if (col.TryGetComponent<PlayerController>(out PlayerController player))
            {
                if (canPlayerDodgeWithSlide && player.isSliding)
                    return;

                player.TakeDamage();
            }
        }
    }
}
