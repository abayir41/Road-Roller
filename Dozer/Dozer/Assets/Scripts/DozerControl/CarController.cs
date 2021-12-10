using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float velocityMultiplier;
    [SerializeField] private float angularVelocityMultiplier;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private ISteerSystem _steerSystem;

    public void SetVelocity(int maxGrowPoint)
    {
        velocityMultiplier = (float)_playerController.TotalCrashPoint / maxGrowPoint * 20f + 3f;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
        _steerSystem = GetComponent<BasicSteerSystem>();
    }

    private void Update()
    {
        _rigidbody.velocity = transform.forward * velocityMultiplier;
        
        var angle = _steerSystem.Angle;
        var registeredTurns = (int)Math.Abs(angle) / 90;
        var lastAngle = ((int) Math.Abs(angle) % 90) * Utilities.PosOrNeg(angle);
        var playerInput = Mathf.Sin(lastAngle * Mathf.Deg2Rad);
        playerInput += registeredTurns * Utilities.PosOrNeg(angle);
        _rigidbody.angularVelocity = Vector3.up * (angularVelocityMultiplier * playerInput);
    }
}
