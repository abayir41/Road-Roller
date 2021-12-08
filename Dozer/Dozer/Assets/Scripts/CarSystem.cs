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

    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interact;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interact;
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Interact(IInteractable interactable)
    {
        if (GameController.Instance.TotalCrashPoint >= maxGrowPoint) return;
        StartCoroutine(GrowAnim(bodyGrowingPoint,interactable.ObjectHitPoint));
    }

    private IEnumerator GrowAnim(Transform growPart,float growAmount)
    {
        float timeElapsed = 0;
        var increase = growAmount / GameController.Instance.MultipleStepPointConstant;

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
