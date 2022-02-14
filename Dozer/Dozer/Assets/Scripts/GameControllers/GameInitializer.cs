using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameInitializer : MonoBehaviour
{

    public static bool GameControllerReady = false;
    public static bool UISystemReady = false;
    
    [SerializeField] private List<GameObject> maps;
    private void Start()
    {
        
        StartCoroutine(WaitForSpecificSystemReady(UISystem.Instance, (() =>
        {
            UISystemReady = true;
            LoadTheGame();        
        })));
    }
    
    void LoadTheGame()
    {
        ActionSys.GameStatusChanged(GameStatus.Loading);
        var ranInt = Random.Range(0, maps.Count);
        Instantiate(maps[ranInt]);
        StartCoroutine(WaitForSpecificSystemReady(GameController.Instance, () =>
        {
            GameControllerReady = true;
        }));
    }
    
    private static IEnumerator WaitForSpecificSystemReady(ISystem system, Action callback = null)
    {
        while (!system.SystemReady)
        {
            yield return null;
        }
        
        callback?.Invoke();
    }
}
