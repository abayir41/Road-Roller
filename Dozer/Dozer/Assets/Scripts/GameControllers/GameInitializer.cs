using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameInitializer : MonoBehaviour
{
    //[SerializeField] private List<GameObject> otherObjects;
    [SerializeField] private List<GameObject> maps;
    private void Start()
    {
        ActionSys.GameStatusChanged(GameStatus.Loading);
        
        /*
        foreach (var otherObject in otherObjects)
        {
            Instantiate(otherObject);
        }
        */
        
        var ranInt = Random.Range(0, maps.Count);
        Instantiate(maps[ranInt]);
    }
}
