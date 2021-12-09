using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRandomlyPaintedMaterialIndex
{
    List<int> MaterialIndexes { get;}
}
public class RandomlyPaintedMaterialIndex : MonoBehaviour,IRandomlyPaintedMaterialIndex
{
    public List<int> MaterialIndexes
    {
        get { return materialIndexes; }
    }
    [SerializeField] private List<int> materialIndexes;
}
