using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

        if(playerStatus != null)
        {
            playerStatus.ActivateEnhanced();
            playerStatus.UpdateScore();
            gameObject.SetActive(false);
        }
    }
}
