using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AISteerSystem : MonoBehaviour, ISteerSystem
{
    public float Angle { get; private set; }

    private Transform _selfTransform;
    
    //Object Selecting
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

    //Testing
    public Transform target;

    private void Awake()
    {
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
        
        var targetPosition = Vector3.zero;

        if (_target == null)
        {
            _target = RandomObjectSelector(_objectScanner.ScannedObjects);

            if (_target == null) return;
            
            targetPosition = _target.GetComponent<InteractableObj>().ColliderPosition;

            var isThereAnyNearestPoint = NavMesh.SamplePosition(target.position, out var nearestPoint, 100000000.0f, NavMesh.AllAreas);

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
            var dozerRightForwardVec = rightTurn.forward;
            var dozerLeftTurnVec = leftTurn.forward;
            
            var vecTowardDestination2D = new Vector2(vecTowardDestination.x, vecTowardDestination.z);
            var dozerRightForwardVec2D = new Vector2(dozerRightForwardVec.x,dozerRightForwardVec.z);
            var dozerLeftForwardVec2D = new Vector2(dozerLeftTurnVec.x,dozerLeftTurnVec.z);
           
            var leftAngle = Vector2.Angle(dozerLeftForwardVec2D,vecTowardDestination2D);
            var rightAngle = Vector2.Angle(dozerRightForwardVec2D,vecTowardDestination2D);
            Angle = leftAngle >= rightAngle ? rightAngle : -leftAngle; 
            
        }
        
    }

    private static GameObject RandomObjectSelector(IReadOnlyList<GameObject> objects)
    {
        if (objects.Count != 0)
        {
            var range = Random.Range(0, objects.Count);
            return objects[range];
        }
        return null;
    }
}
