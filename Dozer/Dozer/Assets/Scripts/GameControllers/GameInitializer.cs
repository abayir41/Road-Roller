using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameInitializer : MonoBehaviour
{

    public static bool MapControllerReady = false;

    [SerializeField] private List<GameObject> maps;
    private void Start()
    {
        ActionSys.GameStatusChanged(GameStatus.Loading);
        StartCoroutine(WaitForSpecificSystemReady(UISystem.Instance,LoadTheGame));
    }

    private void LoadTheGame()
    {
        
        var ranInt = Random.Range(0, maps.Count);
        Instantiate(maps[ranInt]);
        StartCoroutine(WaitForSpecificSystemReady(MapController.Instance, () =>
        {
            MapControllerReady = true;
        }));
    }
    
    
    private static IEnumerator WaitForSpecificSystemReady(ISystem system, Action callback = null)
    {
        system.System.enabled = true;
        
        while (!system.SystemReady)
        {
            yield return null;
        }
        callback?.Invoke();
    }
}
