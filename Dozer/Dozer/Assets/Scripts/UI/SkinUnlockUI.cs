using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkinUnlockUI : MonoBehaviour
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
    }

    public void ScrollTheImage(float percentage, float duration, Action callback = null)
    {
        DOTween.To(() => mask.padding.z, 
            value => mask.padding.Set(0,0,0,value),
            percentage/100*_parentHeight, duration)
            .OnKill((() => callback?.Invoke()));
    }
}
