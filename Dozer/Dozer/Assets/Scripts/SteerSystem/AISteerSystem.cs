﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AISteerSystem : MonoBehaviour, ISteerSystem
{
    public float Angle
    {
        get { return _angle; }
    }
    
    private float _angle;

    
    private Transform _selfTransform;
    
    
    //Object
    private IObjectScanner _objectScanner;
    private GameObject _target;
    
    //Path 
    [SerializeField]
    private float cornerDistanceThreshold;
    private NavMeshPath _path;
    private Vector3 _destination;
    private int _cornerIndexer;

    private void Awake()
    {
        _objectScanner = GetComponentInChildren<IObjectScanner>();
        Debug.Log(_objectScanner);
        _selfTransform = transform;
        _path = new NavMeshPath();
    }

    private void Start()
    {
        _angle = 0;
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = RandomObjectSelecter(_objectScanner.ScannedObjects);
            Debug.Log(_target);
            if (_target == null) return;

            var checkIfPathExist = NavMesh.CalculatePath(_selfTransform.position, _target.transform.position, NavMesh.AllAreas, _path);

            if (checkIfPathExist)
            {
                _cornerIndexer = 0;
                _destination = _path.corners[_cornerIndexer];
            }
            else
            {
                Debug.LogError("Path is not exist");
            }
        }

        if (_path.status != NavMeshPathStatus.PathInvalid)
        {
            if (Vector3.Distance(_destination, _selfTransform.position) < cornerDistanceThreshold)
            {
                _cornerIndexer += 1;
            }

            if (_cornerIndexer == _path.corners.Length - 1)
            {
                _destination = _target.transform.position;
            }

            _angle = Vector3.Angle(_selfTransform.position, _destination);
        }
        
    }

    private GameObject RandomObjectSelecter(IReadOnlyList<GameObject> objects)
    {
        if (objects.Count != 0)
        {
            var range = Random.Range(0, objects.Count);
            return objects[range];
        }
        return null;
    }
}
