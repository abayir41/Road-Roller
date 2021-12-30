using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

interface IObjectScanner
{
    List<GameObject> ScannedObjects { get; }
}
public class ObjectScanner : MonoBehaviour, IObjectScanner
{
    public List<GameObject> ScannedObjects
    {
        get { return scannedObjects; }
    }

    [SerializeField]
    private List<GameObject> scannedObjects;

    private void Awake()
    {
        scannedObjects = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        scannedObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    { 
        scannedObjects.Remove(other.gameObject);
    }
}
