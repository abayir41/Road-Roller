using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapInitializer : MonoBehaviour
{
    [SerializeField] private List<GameObject> maps;
    private void Start()
    {
        var ranInt = Random.Range(0, maps.Count);
        Instantiate(maps[ranInt]);
        Destroy(this);
    }
}
