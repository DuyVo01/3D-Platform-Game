using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour
{
    public int playerScore;
    public bool playerEnhanced = false;

    public ParticleSystem[] enhancedParticleEffect;

    public static event Action<PlayerStatus> OnUpdateScore;

    private void OnEnable()
    {
        for (int i = 0; i < enhancedParticleEffect.Length; i++)
        {
            enhancedParticleEffect[i].Stop();
        }
    }

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
        playerEnhanced = true
            ;
        for (int i = 0; i < enhancedParticleEffect.Length; i++)
        { 
            enhancedParticleEffect[i].Play();
        }

        StartCoroutine(EnchancedCoolDown());
    }

    IEnumerator EnchancedCoolDown()
    {
        yield return new WaitForSeconds(5f);

        for (int i = 0; i < enhancedParticleEffect.Length; i++)
        {
            ParticleSystem.MainModule main = enhancedParticleEffect[i].main;
            
            main.loop = false;

            yield return new WaitForSeconds(main.startLifetime.constant);

            enhancedParticleEffect[i].Stop();

            main.loop = true;
        }

        playerEnhanced = false;
    }

}
