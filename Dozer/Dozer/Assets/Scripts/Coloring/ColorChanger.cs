using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// Changing the color of material without any creating instances.
/// </summary>
public class ColorChanger : MonoBehaviour, IColorChangerRandomly
{

    public Color[] Colors => presetColors;
    [SerializeField] private Color[] presetColors;

    public List<int> PaintMaterialIndexes => paintMaterialIndexes;
    [SerializeField] private List<int> paintMaterialIndexes;
    
    private Renderer _renderer;
    
    private MaterialPropertyBlock _materialProperty;
    
    private Dictionary<int, Color> _materialInColors;
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

    public void SelectColorRandomly(Color[] colors,int materialIndex)
    {
        ChangeColor(presetColors[Random.Range(0, presetColors.Length)],materialIndex);
    }

    
}
