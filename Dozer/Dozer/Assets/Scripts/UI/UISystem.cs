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

    [Header("Game Mode Select Menu Settings")] 
    [SerializeField] private GameObject modeSelectMenuParent;
    [SerializeField] private GameObject modeSelectMenu;
    private RectTransform _modeSelectMenuRect;

    [Header("Setting Panel Settings")] 
    [SerializeField] private GameObject settingsPanelParent;
    [SerializeField] private GameObject settingsPanel;
    private RectTransform _settingsPanel;

    [Header("In Game Settings")] 
    [SerializeField] private GameObject timerPlayerObj;
    [SerializeField] private GameObject killsObj;
    [SerializeField] private GameObject levelSliderObj;
    [SerializeField] private GameObject leaderBoardObj;
    [SerializeField] private GameObject fourthPartOfLeaderObj;
    private RectTransform _timerPlayerRect;
    private RectTransform _killsRect;
    private RectTransform _levelSliderRect;
    private float _levelSliderOriginalYPosition;
    private RectTransform _leaderBoardRect;
    private Slider _levelSlider;
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;


        _timerPlayerRect = timerPlayerObj.GetComponent<RectTransform>();
        _killsRect = killsObj.GetComponent<RectTransform>();
        _levelSliderRect = levelSliderObj.GetComponent<RectTransform>();
        _leaderBoardRect = leaderBoardObj.GetComponent<RectTransform>();
        _levelSlider = levelSliderObj.GetComponent<Slider>();
        _levelSliderOriginalYPosition = _levelSliderRect.position.y;
        
        _settingsPanel = settingsPanel.GetComponent<RectTransform>();
        
        _modeSelectMenuRect = modeSelectMenu.GetComponent<RectTransform>();
        
        _skinSelectMenuRect = skinSelectMenu.GetComponent<RectTransform>();
        
        _clickToPlayText = clickToPlayButton.GetComponent<TextMeshProUGUI>();
        _waitingMenuUIObjectsRectTransforms = waitingMenuUIObjects.ConvertAll(objects => objects.GetComponent<RectTransform>());
        _waitingMenuOriginalXPositions = _waitingMenuUIObjectsRectTransforms.ConvertAll(objects => objects.position.x);
    }

    private void Start()
    {
        DOTween.Init();
        
        DisappearsInGameUI(0);
        DisappearsSettingPanel(0);
        DisappearsGameModesMenu(0);
        DisappearsSkinSelectMenu(0);
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
        var diffBetweenGoalAndSlide = PlayerController.Player.RatioOfBetweenLevels - _levelSlider.value;
        StartCoroutine(SlideAnim(diffBetweenGoalAndSlide));
    }
    
    private void LevelUpped(int point)
    {
        StartCoroutine(SlideAnim(-_levelSlider.value));
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
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DisappearsWaitingMenuItems(animationDuration, () =>
            {
                waitingMenuUI.SetActive(false);
                GetGameModesMenu(animationDuration / 2);
            });
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DisappearsGameModesMenu(animationDuration / 2, () =>
            {
                modeSelectMenuParent.SetActive(false);
                GetWaitingMenuItems(animationDuration);
            });
            
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DisappearsWaitingMenuItems(animationDuration, () =>
            {
                waitingMenuUI.SetActive(false);
                GetSettingsPanel(animationDuration);
            });
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            DisappearsSettingPanel(animationDuration, () =>
            {
                settingsPanelParent.SetActive(false);
                GetWaitingMenuItems(animationDuration);
            });
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            DisappearsWaitingMenuItems(animationDuration, () =>
            {
                waitingMenuUI.SetActive(false);
                GetInGameUI(animationDuration);
            });
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            DisappearsInGameUI(animationDuration, () =>
            {
                gameUI.SetActive(false);
                GetWaitingMenuItems(animationDuration);
            });
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
            
            _levelSlider.value += newGrow - cachedGrow;
            cachedGrow = newGrow;
            
            timeElapsed += Time.deltaTime;

            yield return null;
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

    private void GetSkinSelectMenu(float duration, Action callback = null)
    {
        skinSelectMenuParent.SetActive(true);
        
        _skinSelectMenuRect.DOMoveY(0, duration).OnKill(() => callback?.Invoke());

    }

    private void DisappearsSkinSelectMenu(float duration, Action callback = null)
    {
        _skinSelectMenuRect.DOMoveY(-1 * _skinSelectMenuRect.rect.height, duration).OnKill(() => callback?.Invoke());
    }

    private void GetGameModesMenu(float duration, Action callback = null)
    {
        modeSelectMenuParent.SetActive(true);
        
        _modeSelectMenuRect.DOScaleY(1, duration).OnKill(() => callback?.Invoke());
    }
    
    private void DisappearsGameModesMenu(float duration, Action callback = null)
    {
        _modeSelectMenuRect.DOScaleY(0, duration).OnKill(() => callback?.Invoke());
    }

    private void GetSettingsPanel(float duration, Action callback = null)
    {
        settingsPanelParent.SetActive(true);

        _settingsPanel.DOMoveY((float) Screen.height / 2, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }

    private void DisappearsSettingPanel(float duration, Action callback = null)
    {
        _settingsPanel.DOMoveY(-1 * _settingsPanel.rect.height / 2 * 5 / 4, duration).OnKill(() => callback?.Invoke());
    }
    
    private void GetInGameUI(float duration, Action callback = null)
    {
        gameUI.SetActive(true);
        
        _timerPlayerRect.DOMoveX(0, duration).SetEase(Ease.OutBack);
        _killsRect.DOMoveX(0, duration).SetEase(Ease.OutBack);
        _levelSliderRect.DOMoveY(_levelSliderOriginalYPosition, duration).SetEase(Ease.OutBack);
        _leaderBoardRect.DOMoveX(Screen.width, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }
    
    private void DisappearsInGameUI(float duration, Action callback = null)
    {
        _timerPlayerRect.DOMoveX(-1 * _timerPlayerRect.rect.width, duration);
        _killsRect.DOMoveX(-1 * _killsRect.rect.width, duration);
        _levelSliderRect.DOMoveY(Screen.height + _levelSliderRect.rect.height * 5/3, duration);
        _leaderBoardRect.DOMoveX(Screen.width + _leaderBoardRect.rect.width, duration).OnKill(() => callback?.Invoke());
    }

    #endregion

    #region ButtonMethods

    public void GameModeChange(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                ActionSys.GameModeChanged?.Invoke(GameMode.TimeCounting);
                break;
            case 1:
                ActionSys.GameModeChanged?.Invoke(GameMode.BeTheLast);
                break;
        }
    }

    public void StartGame()
    {
        DisappearsWaitingMenuItems(animationDuration, () =>
        {
            waitingMenuUI.SetActive(false);
            GetInGameUI(animationDuration, () =>
            {
                ActionSys.GameStatusChanged?.Invoke(GameStatus.Playing);
            });
        });
    }

    #endregion
}
