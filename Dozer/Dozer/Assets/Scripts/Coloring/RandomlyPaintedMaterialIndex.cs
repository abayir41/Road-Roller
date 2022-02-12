using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomlyPaintedMaterialIndex : MonoBehaviour,IRandomlyPaintedMaterialIndex
{
    public List<int> MaterialIndexes => materialIndexes;
    [SerializeField] private List<int> materialIndexes;
}
