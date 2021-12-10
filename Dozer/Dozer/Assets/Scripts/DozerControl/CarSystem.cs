using System;
using System.Collections;
using UnityEngine;

public class CarSystem : MonoBehaviour
{
    [SerializeField] private Transform bodyGrowingPoint;
    [SerializeField] private Transform rollerGrowingPoint;
    [SerializeField] private Transform rotationPoint;
    private Transform _transform;

    [SerializeField] private float animationDuration;
    [SerializeField] private AnimationCurve growingAnimShapeCurve;
    [SerializeField] private int maxGrowPoint;
    private CarController _carController;
    private PlayerController _playerController;

    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interact;
        ActionSys.LevelUpped += Interact;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interact;
        ActionSys.LevelUpped -= Interact;
    }

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>(); 
        _carController = GetComponent<CarController>();
        _transform = GetComponent<Transform>();
        
    }

    private void Start()
    {
        if (!(Camera.main is null)) Camera.main.transform.LookAt(bodyGrowingPoint);
        _carController.SetVelocity(maxGrowPoint);
    }

    private void Interact(int reward)
    {
        if (_playerController.TotalCrashPoint >= maxGrowPoint) return;
        _carController.SetVelocity(maxGrowPoint);
        StartCoroutine(GrowAnim(bodyGrowingPoint,reward));
    }
    
    private void Interact(IInteractable interactable)
    {
        if (_playerController.TotalCrashPoint >= maxGrowPoint) return;
        _carController.SetVelocity(maxGrowPoint);
        StartCoroutine(GrowAnim(bodyGrowingPoint,interactable.ObjectHitPoint));
    }

    private IEnumerator GrowAnim(Transform growPart,float growAmount)
    {
        float timeElapsed = 0;
        var increase = growAmount / 1000;

        var goalScale = Vector3.one * increase;

        var cachedGrow = Vector3.zero;
        
        while (timeElapsed < animationDuration)
        {
            var lerpRatio = timeElapsed / animationDuration;
            var animFloat = growingAnimShapeCurve.Evaluate(lerpRatio);
            
            var newGrow = Vector3.Lerp(Vector3.zero, goalScale, animFloat);
            
            growPart.localScale += newGrow - cachedGrow;
            cachedGrow = newGrow;
            
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
    }
    
}
