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
    private GameObject _target;
    
    //Path Setting
    [SerializeField]
    private float cornerDistanceThreshold;
    
    //Path Finding
    [SerializeField] private Transform rightTurn;
    [SerializeField] private Transform leftTurn;
    
    //For Path Process
    private NavMeshPath _path;
    private Vector3 _destination;
    private int _cornerIndexer;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _objectScanner = GetComponentInChildren<IObjectScanner>();
        _selfTransform = transform;
        _path = new NavMeshPath();
    }

    private void Start()
    {
        Angle = 0;
    }

    private void Update()
    {
        

        if (_target == null)
        {
            _target = ObjectSelector();

            var targetInteractableObj = _target.GetComponent<InteractableObj>();
            var targetPosition = targetInteractableObj.ColliderPosition;

            var isThereAnySuitablePointOfTarget = NavMesh.SamplePosition(targetPosition, out var nearestPointOfTarget, 100000000.0f, NavMesh.AllAreas);
            var isThereAnySuitablePointOfAI = NavMesh.SamplePosition(_selfTransform.position, out var nearestPointOfAI, 100000000.0f, NavMesh.AllAreas);
            
            var checkIfPathExist = false;
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
    }

    private GameObject ObjectSelector()
    {
        List<GameObject> validObjects = new List<GameObject>();
        foreach (var scannedObject in _objectScanner.ScannedObjects)
        {
            var targetInteractableObj = scannedObject.GetComponent<InteractableObj>();
            var targetThresholdPoint = targetInteractableObj.DestroyThreshold;
            if (targetThresholdPoint < _playerController.TotalCrashPoint)
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
         
        if (Vector3.Distance(_destination, _selfTransform.position) < cornerDistanceThreshold)
        {
            _cornerIndexer += 1;
            return _path.corners[_cornerIndexer];
        }
        
        return _destination;
        
            
    }
    private float CalculateAngle(Vector3 destination, Vector3 source)
    {
        var vecTowardDestination =  _destination - _selfTransform.position;
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
