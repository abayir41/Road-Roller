using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float velocityMultiplier;
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
        
        
    }

    void Start()
    {
        _rigidbody.velocity = transform.forward * velocityMultiplier;
    }

    private void Update()
    {
        _rigidbody.velocity = transform.forward * velocityMultiplier;
    }
}
