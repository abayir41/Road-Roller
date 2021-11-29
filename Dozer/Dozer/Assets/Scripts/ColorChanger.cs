using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public interface IColorChanger
{
    void ChangeColor(Color color);
}

public interface IColorChangerRandomly : IColorChanger
{
    void SelectColorRandomly();
}
public class ColorChanger : MonoBehaviour, IColorChangerRandomly
{
    [SerializeField] 
    private int materialIndex;
    [SerializeField]
    private Color[] presetColors;
    private Renderer _renderer;
    private MaterialPropertyBlock _materialProperty;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materialProperty = new MaterialPropertyBlock();
    }

    public void ChangeColor(Color color)
    {
        _materialProperty.SetColor("_Color",color);
        _renderer.SetPropertyBlock(_materialProperty,materialIndex);
    }

    public void SelectColorRandomly()
    {
        ChangeColor(presetColors[Random.Range(0, presetColors.Length)]);
    }
}
