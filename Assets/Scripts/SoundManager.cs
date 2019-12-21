using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Everything Sound related is managed here
 * Class uses Singleton for easier use 
 */
public class SoundManager : MonoBehaviour
{
    public AudioSource racSwitch;
    public AudioClip racSwitchClip;
    public AudioSource racMove;
    public AudioClip racMoveClip;
    public AudioSource racNoise;
    public AudioClip racNoiseClip;
    public AudioSource death;
    public AudioClip deathClip;
    public AudioSource win;
    public AudioClip winClip;
    public AudioSource detect;
    public AudioClip detectClip;
    
    public static SoundManager Instance = null;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    /**
     * Sound played if raccoon is moving
     */
    public void playRacMove()
    {
        if (!racMove.isPlaying)
        {
            racMove.volume = 0.08f;
            racMove.clip = racMoveClip;
            racMove.Play();
        }
    }

    /**
     * Sound played if raccoon makes noise to distract guards
     */
    public void playRacNoise()
    {
        if (!racNoise.isPlaying)
        {
             racNoise.clip = racNoiseClip;
             racNoise.Play();
        }
    }
    
    /**
     * Sound played if soul switches raccoon
     */
    public void playRacSwitch()
    {
        racSwitch.clip = racSwitchClip;
        racSwitch.Play();
    }
    
    /**
     * Sound played if raccoon with soul is caught by Guard
     */
    public void playDeath()
    {
        death.clip = deathClip;
        death.Play();
    }
    
    /**
     * Sound played if Soul reaches goal
     */
    public void playWin()
    {
        win.clip = winClip;
        win.Play();
    }
    
    /**
     * Sound played if Guard detects soulless raccoon
     */
    public void playDetect()
    {
        detect.clip = detectClip;
        detect.Play();
    }
}
