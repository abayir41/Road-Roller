using System;
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

    public Transform target;

    private void Awake()
    {
        _objectScanner = GetComponentInChildren<IObjectScanner>();
        _selfTransform = transform;
        _path = new NavMeshPath();
    }

    private void Start()
    {
        _angle = 0;
    }

    private void Update()
    {
        
        var targetPosition = Vector3.zero;

        if (_target == null)
        {
            _target = RandomObjectSelecter(_objectScanner.ScannedObjects);

            if (_target == null) return;
            
            targetPosition = _target.GetComponent<InteractableObj>().ColliderPosition;

            NavMeshHit nearestPoint;
            var isThereAnyNearestPoint = NavMesh.SamplePosition(target.position, out nearestPoint, 100000000.0f, NavMesh.AllAreas);

            var checkIfPathExist = false;
            if (isThereAnyNearestPoint)
            {
                checkIfPathExist = NavMesh.CalculatePath(_selfTransform.position, nearestPoint.position, NavMesh.AllAreas, _path);
            }
            else
            {
                Debug.LogError("Nearest Point does not exist");
            }
            
            Debug.Log(nearestPoint.position);

            if (checkIfPathExist)
            {
                _cornerIndexer = 1;
                _destination = _path.corners[_cornerIndexer];
                
            }
            else
            {
                Debug.LogError("Path does not exist");
            }
        }
        
        Debug.Log(_path.corners);
        Debug.Log(_destination);
        if (_path.status != NavMeshPathStatus.PathInvalid)
        {
            if (_cornerIndexer == _path.corners.Length - 1)
            {
                _destination = targetPosition;
            }
            
            if (Vector3.Distance(_destination, _selfTransform.position) < cornerDistanceThreshold && _cornerIndexer < _path.corners.Length - 1)
            {
                _cornerIndexer += 1;
                _destination = _path.corners[_cornerIndexer];
            }

            var vecTowardDestination =  _destination - _selfTransform.position;
            var vecTowardDestination2D = new Vector2(vecTowardDestination.x, vecTowardDestination.z);
            var dozerForwardVec = _selfTransform.forward;
            var dozerForwardVec2D = new Vector2(dozerForwardVec.x, dozerForwardVec.z);

            _angle = Utilities.AngleCalculator(vecTowardDestination2D) - Utilities.AngleCalculator(dozerForwardVec2D);
            _angle *= -1;
            Debug.DrawLine(_selfTransform.position,_selfTransform.position+vecTowardDestination*100,Color.red);
            Debug.DrawLine(_selfTransform.position,_selfTransform.position+dozerForwardVec*100,Color.red);
            Debug.Log(_angle);
            Debug.DrawLine(_destination,_destination+Vector3.up*100,Color.red);
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
