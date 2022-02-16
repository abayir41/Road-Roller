using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameInitializer : MonoBehaviour, ISystem
{

    [SerializeField] private List<GameObject> maps;
    public static GameObject CurrentMap { get; private set; }
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        StartCoroutine(InitializedGame());
    }

    IEnumerator InitializedGame()
    {
        ActionSys.GameStatusChanged?.Invoke(GameStatus.Loading);
        
        yield return 0;
        
        UISystem.Instance.ResetTheSystem();

        yield return 0;
        
        LoadTheGame();
        
        yield return 0;
        
        ActionSys.GameModeChanged?.Invoke(GameMode.TimeCounting);
        
        yield return 0;
        
        ActionSys.GameStatusChanged?.Invoke(GameStatus.WaitingOnMenu);
    }

    private void LoadTheGame(Action callback = null)
    {
        var ranInt = Random.Range(0, maps.Count);
        CurrentMap = Instantiate(maps[ranInt]);
    }
    
    private void OnEnable()
    {
        ActionSys.ResetGame += ResetTheSystem;
    }

    private void OnDisable()
    {
        ActionSys.ResetGame -= ResetTheSystem;
    }

    public void ResetTheSystem()
    {
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        Destroy(CurrentMap);

        yield return 0;
        
        GameController.Instance.ResetTheSystem();

        yield return 0;

        UISystem.Instance.ResetTheSystem();

        yield return 0;
        
        LoadTheGame();

        yield return 0;
        
        ActionSys.GameStatusChanged?.Invoke(GameStatus.WaitingOnMenu);
    }
}
