using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource player;
    [SerializeField] private AudioClip soundSystemOpenedSound;
    private bool _toggleSoundSystem = true;

    private void OnEnable()
    {
        ActionSys.PlaySound += PlaySound;
        ActionSys.ToggleSoundSystem += ToggleSoundSystem;
    }

    
    private void OnDisable()
    {
        ActionSys.PlaySound -= PlaySound;
        ActionSys.ToggleSoundSystem -= ToggleSoundSystem;
    }
    
    private void PlaySound(AudioClip obj)
    {
        if (!_toggleSoundSystem || obj == null || player == null) return;
        player.clip = obj;
        player.Play();
    }
    
    private void ToggleSoundSystem()
    {
        _toggleSoundSystem = !_toggleSoundSystem;

        if (_toggleSoundSystem)
        {
            PlaySound(soundSystemOpenedSound);
        }
    }
}
