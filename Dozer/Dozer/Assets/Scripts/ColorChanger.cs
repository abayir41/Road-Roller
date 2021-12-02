using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public interface IColorChanger
{
    void ChangeColor(Color color, int materialIndex);
}

public interface IColorChangerRandomly : IColorChanger
{
    void SelectColorRandomly(int materialIndex);
}
public class ColorChanger : MonoBehaviour, IColorChangerRandomly
{
    [SerializeField]
    private Color[] presetColors;
    private Renderer _renderer;
    private MaterialPropertyBlock _materialProperty;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _materialProperty = new MaterialPropertyBlock();
    }

    public void ChangeColor(Color color, int materialIndex)
    {
        _materialProperty.SetColor("_Color",color);
        _renderer.SetPropertyBlock(_materialProperty,materialIndex);
    }

    public void SelectColorRandomly(int materialIndex)
    {
        ChangeColor(presetColors[Random.Range(0, presetColors.Length)],materialIndex);
    }
}
