using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

interface IObjectScanner
{
    List<string> Filter { get;}
    List<GameObject> ScannedObjects { get; }
}
public class ObjectScanner : MonoBehaviour, IObjectScanner
{
    public List<string> Filter => GameController.Instance.CollisionObjectFilter;

    private void OnEnable()
    {
        ActionSys.ObjectDestroyed += RemoveObjectFromObjects;
    }

    private void OnDisable()
    {
        ActionSys.ObjectDestroyed -= RemoveObjectFromObjects;
    }
    
    public List<GameObject> ScannedObjects => scannedObjects;


    [SerializeField]
    private List<GameObject> scannedObjects;

    private SphereCollider _collider;
    private float _initialRadius; 
    private void Awake()
    {
        
        _collider = GetComponent<SphereCollider>();
        _initialRadius = _collider.radius;
        scannedObjects = new List<GameObject>();
    }
    
    private void Update()
    {
        if (ScannedObjects.Count == 0)
        {
            _collider.radius *= 2;
        }
        else
        {
            _collider.radius = _initialRadius;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Filter.Contains(other.gameObject.tag))
            scannedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(ScannedObjects.Contains(other.gameObject))
            scannedObjects.Remove(other.gameObject);
    }
    
    private void RemoveObjectFromObjects(GameObject colObject)
    {
        if(ScannedObjects.Contains(colObject))
            scannedObjects.Remove(colObject);
    }
}
