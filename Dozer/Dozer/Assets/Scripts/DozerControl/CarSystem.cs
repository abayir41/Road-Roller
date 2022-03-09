﻿using System.Collections;
using UnityEngine;

public class CarSystem : MonoBehaviour
{
    [Header("Dozer Body Point For Using In Animations")]
    [SerializeField] private Transform bodyGrowingPoint;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration;
    [SerializeField] private AnimationCurve growingAnimShapeCurve;

    [Header("Skin Point")] 
    [SerializeField] private Transform visualPoint;

    public Transform VisualPoint => visualPoint;

    private PlayerController _playerController;
    private CarActionSys _carActionSystem;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        
    }
    
    private void OnEnable()
    {
        _carActionSystem = _playerController.ActionSysCar;
        _carActionSystem.ObjectGotHit += Interact;
        _carActionSystem.LevelUpped += Interact;
    }
    
    private void OnDisable()
    {
        _carActionSystem.ObjectGotHit -= Interact;
        _carActionSystem.LevelUpped -= Interact;
    }

    
    //Stands For Level Up
    private void Interact(int reward)
    {
        StartCoroutine(GrowAnim(bodyGrowingPoint,reward));
    }
    
    //Stands For normal gains
    private void Interact(IInteractable interactable)
    {
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
