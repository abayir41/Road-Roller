using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AISteerSystem : MonoBehaviour, ISteerSystem
{
    public float Angle { get; private set; }

    private Transform _selfTransform;
    
    //Object Selecting
    private PlayerController _playerController;
    private IObjectScanner _objectScanner;
    [SerializeField] private ObjectScanner objectScanner;
    [SerializeField] private GameObject _target;
    
    //Path Setting
    private float cornerDistanceThreshold => MapController.Instance.mapConfig.CornerDistanceThreshold;

    
    //Path Finding
    [SerializeField] private Transform rightTurn;
    [SerializeField] private Transform leftTurn;
    
    //For Path Process
    private NavMeshPath _path;
    private Vector3 _destination;
    private int _cornerIndexer;
    private float _timerForChangeTarget;

    private void Awake()
    {
        _objectScanner = objectScanner;
        _playerController = GetComponent<PlayerController>();
        _selfTransform = transform;
        _path = new NavMeshPath();
    }

    private void Start()
    {
        Angle = 0;
    }
    
    private void Update()
    {
        
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (_target == null)
        {
            _target = ObjectSelector();
            if (_target == null) return;
            
            _timerForChangeTarget = 0;
            
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            var targetInteractableObj = _target.GetComponent<InteractableObj>();
            var targetPosition = targetInteractableObj.ColliderPosition;

            var isThereAnySuitablePointOfTarget = NavMesh.SamplePosition(targetPosition, out var nearestPointOfTarget, 100000000.0f, NavMesh.AllAreas);
            var isThereAnySuitablePointOfAI = NavMesh.SamplePosition(_selfTransform.position, out var nearestPointOfAI, 100000000.0f, NavMesh.AllAreas);
            
            bool checkIfPathExist;
            if (isThereAnySuitablePointOfAI && isThereAnySuitablePointOfTarget)
            {
                checkIfPathExist = NavMesh.CalculatePath(nearestPointOfAI.position, nearestPointOfTarget.position, NavMesh.AllAreas, _path);
            }
            else
            {
                Debug.LogError($"Nearest Point does not exist. TargetPoint Status: {isThereAnySuitablePointOfTarget}, AIPoint Status: {isThereAnySuitablePointOfAI}");
                _target = null;
                return;
            }
            

            if (checkIfPathExist)
            {
                _cornerIndexer = 0;
                _destination = _path.corners[_cornerIndexer];
            }
            else
            {
                Debug.LogError("Path does not exist");
                _target = null;
                return;
            }
        }
        
        if (_path.status != NavMeshPathStatus.PathInvalid)
        {
            _destination = DestinationCalculator();
            
            Angle = CalculateAngle(_destination, _selfTransform.position);
        }
        else
        {
            _target = null;
            return;
        }

        _timerForChangeTarget += Time.deltaTime;
        if (_timerForChangeTarget > 2f)
        {
            _destination = transform.forward * 10;
            _target = null;
        }
    }

    private GameObject ObjectSelector()
    {
        List<GameObject> validObjects = new List<GameObject>();
        foreach (var scannedObject in _objectScanner.ScannedObjects)
        {
            if (scannedObject == null) continue;
            var targetInteractableObj = scannedObject.GetComponent<InteractableObj>();
            if (targetInteractableObj == null) continue;
            var targetThresholdPoint = targetInteractableObj.DestroyThreshold;
            if (targetThresholdPoint < _playerController.Score)
            {
                validObjects.Add(scannedObject);
            }
        }

        return RandomObjectSelector(validObjects);
    }
    private Vector3 DestinationCalculator()
    {
        if (_cornerIndexer == _path.corners.Length - 1)
        {
            return _target.GetComponent<InteractableObj>().ColliderPosition;
        }
         
        if (Vector2.Distance(new Vector2(_destination.x,_destination.z), new Vector2(_selfTransform.position.x,_selfTransform.position.z)) < cornerDistanceThreshold)
        {
            _timerForChangeTarget = 0;
            _cornerIndexer += 1;
            return _path.corners[_cornerIndexer];
        }
        
        return _destination;
        
            
    }
    private float CalculateAngle(Vector3 destination, Vector3 source)
    {
        var vecTowardDestination =  destination - source;
        var dozerRightForwardVec = rightTurn.forward;
        var dozerLeftTurnVec = leftTurn.forward;
            
        var vecTowardDestination2D = new Vector2(vecTowardDestination.x, vecTowardDestination.z);
        var dozerRightForwardVec2D = new Vector2(dozerRightForwardVec.x,dozerRightForwardVec.z);
        var dozerLeftForwardVec2D = new Vector2(dozerLeftTurnVec.x,dozerLeftTurnVec.z);
           
        var leftAngle = Vector2.Angle(dozerLeftForwardVec2D,vecTowardDestination2D);
        var rightAngle = Vector2.Angle(dozerRightForwardVec2D,vecTowardDestination2D);
        
        return leftAngle >= rightAngle ? rightAngle : -leftAngle; 
    }
    private static GameObject RandomObjectSelector(IReadOnlyList<GameObject> objects)
    {
        if (objects.Count == 0) return null;
        var range = Random.Range(0, objects.Count);
        return objects[range];
    }
}
