using System;
using System.Collections;
using UnityEngine;

public class CarSystem : MonoBehaviour
{
    [Header("Dozer Body Point For Using In Animations")]
    [SerializeField] private Transform bodyGrowingPoint;
    [SerializeField] private Transform rollerGrowingPoint;
    [SerializeField] private Transform rotationPoint;
    private Transform _transform;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration;
    [SerializeField] private AnimationCurve growingAnimShapeCurve;
    
    [Header("Car Settings")]
    [SerializeField] private int maxGrowPoint;
    private CarController _carController;
    private PlayerController _playerController;

    private void OnEnable()
    {
        _playerController.ActionSysCar.ObjectGotHit += Interact;
        _playerController.ActionSysCar.LevelUpped += Interact;
    }
    
    private void OnDisable()
    {
        _playerController.ActionSysCar.ObjectGotHit -= Interact;
        _playerController.ActionSysCar.LevelUpped -= Interact;
    }

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>(); 
        _carController = GetComponent<CarController>();
        _transform = GetComponent<Transform>();
        
    }

    private void Start()
    {
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
