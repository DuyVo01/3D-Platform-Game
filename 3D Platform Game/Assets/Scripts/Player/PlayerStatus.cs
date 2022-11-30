using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour
{
    public int playerScore;
    public bool playerEnhanced = false;

    public static event Action<PlayerStatus> OnUpdateScore;
    
    public void UpdateScore()
    {
        OnUpdateScore?.Invoke(this);
    }

    public void ScoreCalculating(int scoreToAdd)
    {
        if (playerEnhanced)
        {
            playerScore = playerScore + (scoreToAdd * 2);
        }
        else
        {
            playerScore += scoreToAdd;
        }
    }

    public void ActivateEnhanced() 
    {
        playerEnhanced = true;
        StartCoroutine(EnchancedCoolDown());
    }

    IEnumerator EnchancedCoolDown()
    {
        yield return new WaitForSeconds(5f);
        playerEnhanced = false;
    }

}
