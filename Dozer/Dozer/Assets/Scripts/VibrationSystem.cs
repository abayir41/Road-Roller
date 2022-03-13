using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class VibrationSystem : MonoBehaviour
{
    private bool _toggleVibrationSystem = true;
    private void OnEnable()
    {
        ActionSys.Vibrate += Vibrate;
        ActionSys.ToggleVibrationSystem += ToggleVibrationSystem;
    }
    
    private void OnDisable()
    {
        ActionSys.Vibrate -= Vibrate;
        ActionSys.ToggleVibrationSystem -= ToggleVibrationSystem;
    }
    
    private void ToggleVibrationSystem()
    {
        _toggleVibrationSystem = !_toggleVibrationSystem;
        if(_toggleVibrationSystem)
            Vibrate(HapticTypes.Selection);
    }

    
    private void Vibrate(HapticTypes type)
    {
        if(_toggleVibrationSystem)
            MMVibrationManager.Haptic(type);
    }
}
