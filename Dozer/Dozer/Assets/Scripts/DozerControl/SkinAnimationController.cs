using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinAnimationController : MonoBehaviour
{
    
    private Animator _animator;
    
    [Header("Config")] 
    [SerializeField] private float angularVelocityMultiplier;

    private static readonly int TireSpeedMultiplier = Animator.StringToHash("TireSpeedMultiplier");


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameController.Status != GameStatus.Playing)
        {
            UpdateTheTireAnimationSpeedMultiplier(0.0f);
            return;
        }
        
        UpdateTheTireAnimationSpeedMultiplier(angularVelocityMultiplier);
    }

    private void UpdateTheTireAnimationSpeedMultiplier(float multiplier)
    {
        _animator.SetFloat(TireSpeedMultiplier, multiplier);
    }
}
