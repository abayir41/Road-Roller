﻿using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float _velocityMultiplier;
    [SerializeField] private float angularVelocityDivider;
    private float _velocityMultiplierDivider;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private ISteerSystem _steerSystem;



    private void OnEnable()
    {
        _playerController.ActionSysCar.LevelUpped += SetVelocity;
    }

    private void OnDisable()
    {
        _playerController.ActionSysCar.LevelUpped -= SetVelocity;
    }
    
    private void SetVelocity(int velocity)
    {
        _velocityMultiplier += velocity / _velocityMultiplierDivider;
    }
    
    public void SetVelocity(float startVelocity,float velocityDivider)
    {
        _velocityMultiplierDivider = velocityDivider;
        _velocityMultiplier = startVelocity;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
        _steerSystem = GetComponent<ISteerSystem>();
    }

    private void Update()
    {
        if (GameController.Status != GameStatus.Playing)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            return;
        }

        _rigidbody.velocity = transform.forward * _velocityMultiplier;
        _rigidbody.angularVelocity = Vector3.up * (_steerSystem.Angle / angularVelocityDivider);
        
        if(_rigidbody.angularVelocity.x != 0 || _rigidbody.angularVelocity.z != 0)
            Correction();
    }

    public void Correction()
    {
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
    }
    public void Correction(int y)
    {
        _rigidbody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }

}
