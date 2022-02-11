using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UISystem : MonoBehaviour
{
    [SerializeField] private Slider levelSlider;
    
    //UIs
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject waitingMenuUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject endUI;
    
    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interaction;
        ActionSys.LevelUpped += LevelUpped;
        ActionSys.GameStatusChanged += GameStatusChange;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
        ActionSys.GameStatusChanged -= GameStatusChange;
    }

    private void Interaction(IInteractable obj)
    {
        var diffBetweenGoalAndSlide = PlayerController.Player.RatioOfBetweenLevels - levelSlider.value;
        StartCoroutine(SlideAnim(diffBetweenGoalAndSlide));
    }
    
    private void LevelUpped(int point)
    {
        StartCoroutine(SlideAnim(-levelSlider.value));
    }

    private void GameStatusChange(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Loading:
                Loading();
                break;
            case GameStatus.WaitingOnMenu:
                WaitingMenu();
                break;
            case GameStatus.Playing:
                PlayingUI();
                break;
            case GameStatus.Paused:
                Paused();
                break;
            case GameStatus.Ended:
                EndUI();
                break;
            default:
                break;
        }
    }
    
    private void Loading()
    {
        DisableAllUI(loadingUI);
    }

    private void WaitingMenu()
    {
        DisableAllUI(waitingMenuUI);
    }

    private void PlayingUI()
    {
        DisableAllUI(gameUI);
    }

    private void Paused()
    {
        DisableAllUI(pauseUI);
    }

    private void EndUI()
    {
        DisableAllUI(endUI);
    }

    private void DisableAllUI(Object except)
    {
        loadingUI.SetActive(false);
        waitingMenuUI.SetActive(false);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);
        endUI.SetActive(false);
        
        if (except == loadingUI)
        {
            loadingUI.SetActive(true);
        }
        else if (except == waitingMenuUI)
        {
            waitingMenuUI.SetActive(true);
        }
        else if (except == gameUI)
        {
            gameUI.SetActive(true);
        }
        else if (except == pauseUI)
        {
            pauseUI.SetActive(true);
        }
        else if (except == endUI)
        {
            endUI.SetActive(true);
        }
    }

    private IEnumerator SlideAnim(float increase)
    {
        float timeElapsed = 0;

        var cachedGrow = 0f;
        while (timeElapsed < 0.2f)
        {

            
            var lerpRatio = timeElapsed / 0.2f;

            var newGrow = Mathf.Lerp(0, increase, lerpRatio);
            
            levelSlider.value += newGrow - cachedGrow;
            cachedGrow = newGrow;
            
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
