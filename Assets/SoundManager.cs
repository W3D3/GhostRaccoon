using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource background;
    public AudioClip backgroundClip;
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
    
    public void playRacMove()
    {
        if (!racMove.isPlaying)
        {
            racMove.clip = racMoveClip;
            racMove.Play();
        }
    }

    public void playRacNoise()
    {
        racNoise.clip = racNoiseClip;
        racNoise.Play();
    }
    
    public void playRacSwitch()
    {
        racSwitch.clip = racSwitchClip;
        racSwitch.Play();
    }
    
    public void playDeath()
    {
        death.clip = deathClip;
        death.Play();
    }
    
    public void playWin()
    {
        win.clip = winClip;
        win.Play();
    }
    
    public void playDetect()
    {
        detect.clip = detectClip;
        detect.Play();
    }
}
