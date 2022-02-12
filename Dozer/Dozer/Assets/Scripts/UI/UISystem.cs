using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UISystem : MonoBehaviour
{

    public static UISystem Instance;

    [SerializeField] private Slider levelSlider;
    
    //UIs
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private GameObject waitingMenuUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject endUI;
    
    [SerializeField] private float animationDuration = 0.2f;
    
    [Header("Waiting Menu Settings")] 
    [SerializeField] private GameObject clickToPlayButton;
    private TextMeshProUGUI _clickToPlayText;
    [SerializeField] private List<GameObject> waitingMenuUIObjects;
    [SerializeField] private List<Button> waitingMenuButtons;
    [SerializeField] private List<bool> goNegativeMove;
    private List<RectTransform> _waitingMenuUIObjectsRectTransforms;
    private List<float> _waitingMenuOriginalXPositions;

    [Header("Skin Select Menu Settings")] 
    [SerializeField] private GameObject skinSelectMenuParent;
    [SerializeField] private GameObject skinSelectMenu;
    private RectTransform _skinSelectMenuRect;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        _skinSelectMenuRect = skinSelectMenu.GetComponent<RectTransform>();
        _clickToPlayText = clickToPlayButton.GetComponent<TextMeshProUGUI>();
        _waitingMenuUIObjectsRectTransforms = waitingMenuUIObjects.ConvertAll(objects => objects.GetComponent<RectTransform>());
        _waitingMenuOriginalXPositions = _waitingMenuUIObjectsRectTransforms.ConvertAll(objects => objects.position.x);
    }

    private void Start()
    {
        DOTween.Init();
    }

    #region Subscription

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
    
    #endregion


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DisappearsWaitingMenuItems(animationDuration, () => waitingMenuUI.SetActive(false));
            GetSkinSelectMenu(animationDuration);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DisappearsSkinSelectMenu(animationDuration, () => skinSelectMenuParent.SetActive(false));
            GetWaitingMenuItems(animationDuration);
        }
        
    }



    #region GameStatusChange

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

    #endregion


    #region Animations

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

    private void DisappearsWaitingMenuItems(float duration , Action callback = null)
    {
        foreach (var waitingMenuButton in waitingMenuButtons)
        {
            waitingMenuButton.interactable = false;
        }

        _clickToPlayText.DOFade(0, duration).OnKill(() => callback?.Invoke());

        for (var i = 0; i < _waitingMenuUIObjectsRectTransforms.Count; i++)
        {
            var rectTransform = _waitingMenuUIObjectsRectTransforms[i];
            var widthOfRect = rectTransform.sizeDelta.x;
            rectTransform.DOMoveX(goNegativeMove[i] ? (-1 * widthOfRect * 4 / 3) : (Screen.width + widthOfRect * 4 / 3), duration)
                .SetEase(Ease.InCubic);
        }
        
    }
    
    private void GetWaitingMenuItems(float duration, Action callback = null)
    {
        waitingMenuUI.SetActive(true);

        _clickToPlayText.DOFade(1, duration).OnKill((() =>
        {
            foreach (var waitingMenuButton in waitingMenuButtons)
            {
                waitingMenuButton.interactable = true;
            }
            
            callback?.Invoke();
        }));

        for (var i = 0; i < _waitingMenuUIObjectsRectTransforms.Count; i++)
        {
            var rectTransform = _waitingMenuUIObjectsRectTransforms[i];
            rectTransform.DOMoveX(_waitingMenuOriginalXPositions[i], duration)
                .SetEase(Ease.OutBack);
        }
    }

    private void GetSkinSelectMenu(float duration, Action callback = null)
    {
        skinSelectMenuParent.SetActive(true);
        
        _skinSelectMenuRect.DOMoveY(0, duration).OnKill(() => callback?.Invoke());

    }

    private void DisappearsSkinSelectMenu(float duration, Action callback = null)
    {
        _skinSelectMenuRect.DOMoveY(-1 * _skinSelectMenuRect.rect.height, duration).OnKill(() => callback?.Invoke());
    }

    #endregion

}
