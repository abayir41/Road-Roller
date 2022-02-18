using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinUnlockUI : MonoBehaviour, ISystem
{
 

    [SerializeField] private RectMask2D mask;
    [SerializeField] private RectTransform parentRectTransform;
    private float _paddingAmount;
    private float _parentHeight;

    private void Awake()
    {
        _parentHeight = parentRectTransform.rect.height;
    }

    private void Start()
    {
        DOTween.Init();
        mask.padding = new Vector4(0,GameController.SkinUnlockProgressPercentage/100*_parentHeight,0, 0);
    }

    public void ScrollTheImage(float percentage, float duration, Action callback = null)
    {
        DOTween.To(() => mask.padding.y, 
            value => mask.padding = new Vector4(0,value,0,0),
            percentage/100*_parentHeight, duration)
            .OnKill(() => callback?.Invoke());
    }

    public void ResetTheSystem()
    {
        mask.padding = new Vector4(0,GameController.SkinUnlockProgressPercentage/100*_parentHeight,0, 0);
    }


}
