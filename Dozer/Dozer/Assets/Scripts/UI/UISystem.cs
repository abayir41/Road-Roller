using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UISystem : MonoBehaviour, ISystem
{

    public static UISystem Instance { get; private set; }

    public bool SystemReady { get; private set; }
    public Behaviour System { get; private set; }

    [SerializeField] private float animationDuration = 0.2f;

    private Action<float, Action> _disappearsCurrentUI;

    [Header("Loading Items")] 
    [SerializeField] private GameObject loadingScreenParent;
    [SerializeField] private GameObject loadingBackground;
    [SerializeField] private GameObject loadingText;
    private Image _loadingBackgroundImage;
    private TextMeshProUGUI _loadingTextMeshProUGUI;
    
    [Header("Waiting Menu Settings")] 
    [SerializeField] private GameObject waitingMenuUI;
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
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject timerPlayerObj;
    [SerializeField] private GameObject killsObj;
    [SerializeField] private GameObject levelSliderObj;
    [SerializeField] private GameObject destroyableObjectObj;
    [SerializeField] private GameObject leaderBoardObj;
    private RectTransform _timerPlayerRect;
    private RectTransform _killsRect;
    private RectTransform _levelSliderRect;
    private float _levelSliderOriginalYPosition;
    private RectTransform _destroyableObjectRect;
    private float _destroyableObjectOriginalYPosition;
    private RectTransform _leaderBoardRect;
    private Slider _levelSlider;
    
    [Header("Leaderboard Elements")]
    [SerializeField] private GameObject fourthPartOfLeaderObj;
    [SerializeField] private GameObject firstPartOfLeaderObj;
    [SerializeField] private GameObject secondPartOfLeaderObj;
    [SerializeField] private GameObject thirdPartOfLeaderObj;
    private TextMeshProUGUI _firstText;
    private TextMeshProUGUI _secondText;
    private TextMeshProUGUI _thirdText;
    private TextMeshProUGUI _fourthText;

    [Header("In Game Left Panel")] 
    [SerializeField] private TextMeshProUGUI timePlayerText;
    [SerializeField] private TextMeshProUGUI killText;

    [Header("Next Level Images")]
    [SerializeField] private List<GameObject> destroyableObjectObjItems;
    [SerializeField] private Image endOfSliderImage;
    [SerializeField] private List<Sprite> newDestroyableObjectImages;
    [SerializeField] private Sprite normalImage;

    [Header("End Text")] 
    [SerializeField] private GameObject endText;
    private RectTransform _endTextRectTransform;
    private TextMeshProUGUI _endTextMeshProUGUI;
    
    [Header("Pause Items")]
    [SerializeField] private GameObject pauseParent;
    [SerializeField] private List<GameObject> pauseItems;
    [SerializeField] private GameObject pauseBackGround;
    private float _pauseAlphaAmountBackground;
    private Image _pauseBackground;
    private List<RectTransform> _pauseRectTransforms;
    private List<float> _pauseRectOriginalYPositions;

    [Header("Need to Continue")] 
    [SerializeField] private GameObject needToContinueParent;
    [SerializeField] private List<GameObject> needToContinueItems;
    [SerializeField] private GameObject needToContinueBackGround;
    private float _needToContinueAlphaAmountBackground;
    private Image _needToContinueBackground;
    private List<RectTransform> _needToContinueRectTransforms;
    private List<float> _needToContinueRectOriginalYPositions;

    [Header("End Leaderboard Items")] 
    [SerializeField] private GameObject endLeaderboardParent;
    [SerializeField] private GameObject timeOutOrPlayerCount;
    [SerializeField] private List<GameObject> leaderBoardObjects;
    [SerializeField] private GameObject rewardButtonObject;
    [SerializeField] private GameObject clickToContinueObject;
    private RectTransform _timeOutOrPlayerCountRect;
    private TextMeshProUGUI _timeOutOrPlayerCountText;
    private float _timeOrPlayerCountOriginalXPosition;
    private RectTransform _rewardButtonRect;
    private TextMeshProUGUI _clickToContinueText;
    private float _rewardOriginalXPosition;
    private List<Image> _imagesOfLeaderboardObject;
    private List<TextMeshProUGUI> _textMeshProsOfLeaderboard;
    private List<float> _alphasOfLeaderboardObjects;

    [Header("Dead Screen")] 
    [SerializeField] private GameObject deadScreenParent;
    [SerializeField] private GameObject deadScreenBackGround;
    [SerializeField] private GameObject deadScreenYouLostText;
    [SerializeField] private GameObject deadScreenScoreText;
    [SerializeField] private GameObject deadScreenClickToContinue;
    private Image _deadScreenBackgroundImg;
    private float _alphaOfDeadScreenBackGround;
    private RectTransform _deadScreenYouLostRect;
    private RectTransform _deadScreenScoreTextRect;
    private TextMeshProUGUI _deadScreenClickToContinue;

    [Header("Skin Unlock Screen")] 
    [SerializeField] private GameObject skinUnlockParent;
    [SerializeField] private GameObject skinTotalScoreTextObj;
    [SerializeField] private GameObject skinUnlockObj;
    [SerializeField] private GameObject newSkinUnlockedText;
    [SerializeField] private GameObject skinUnlockClickToContinue;
    [SerializeField] private SkinUnlockUI skinUnlockScript;
    private RectTransform _newSkinUnlockedTextRect;
    private TextMeshProUGUI _skinUnlockClickToContinueText;
    private RectTransform _skinTotalScoreTextRect;
    private RectTransform _skinUnlockRect;
    private float _skinTotalScoreTextOriginalYPosition;
    

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        System = this;


        _newSkinUnlockedTextRect = newSkinUnlockedText.GetComponent<RectTransform>();
        _skinUnlockClickToContinueText = skinUnlockClickToContinue.GetComponent<TextMeshProUGUI>();
        _skinTotalScoreTextRect = skinTotalScoreTextObj.GetComponent<RectTransform>();
        _skinUnlockRect = skinUnlockObj.GetComponent<RectTransform>();
        _skinTotalScoreTextOriginalYPosition = _skinTotalScoreTextRect.position.y;
        
        _deadScreenBackgroundImg = deadScreenBackGround.GetComponent<Image>();
        _alphaOfDeadScreenBackGround = _deadScreenBackgroundImg.color.a;
        _deadScreenYouLostRect = deadScreenYouLostText.GetComponent<RectTransform>();
        _deadScreenScoreTextRect = deadScreenScoreText.GetComponent<RectTransform>();
        _deadScreenClickToContinue = deadScreenClickToContinue.GetComponentInChildren<TextMeshProUGUI>();

        _timeOutOrPlayerCountRect = timeOutOrPlayerCount.GetComponent<RectTransform>();
        _timeOutOrPlayerCountText = timeOutOrPlayerCount.GetComponent<TextMeshProUGUI>();
        _timeOrPlayerCountOriginalXPosition = _timeOutOrPlayerCountRect.position.x;
        _rewardButtonRect = rewardButtonObject.GetComponent<RectTransform>();
        _rewardOriginalXPosition = _rewardButtonRect.position.x;
        _clickToContinueText = clickToContinueObject.GetComponentInChildren<TextMeshProUGUI>();
        _imagesOfLeaderboardObject = leaderBoardObjects.ConvertAll(input => input.GetComponent<Image>());
        _textMeshProsOfLeaderboard =
            leaderBoardObjects.ConvertAll(input => input.GetComponentInChildren<TextMeshProUGUI>());
        _alphasOfLeaderboardObjects = _imagesOfLeaderboardObject.ConvertAll(input => input.color.a);
        
            _loadingBackgroundImage = loadingBackground.GetComponent<Image>();
        _loadingTextMeshProUGUI = loadingText.GetComponent<TextMeshProUGUI>();
        
        _pauseBackground = pauseBackGround.GetComponent<Image>();
        _pauseAlphaAmountBackground = _pauseBackground.color.a;
        _pauseRectTransforms = pauseItems.Select(objects => objects.GetComponent<RectTransform>()).ToList();
        _pauseRectOriginalYPositions = _pauseRectTransforms.Select(rectTransform => rectTransform.position.y).ToList();
        
        _needToContinueBackground = needToContinueBackGround.GetComponent<Image>();
        _needToContinueAlphaAmountBackground = _needToContinueBackground.color.a;
        _needToContinueRectTransforms =
            needToContinueItems.Select(objects => objects.GetComponent<RectTransform>()).ToList();
        _needToContinueRectOriginalYPositions =
            _needToContinueRectTransforms.Select(rectTransform => rectTransform.position.y).ToList();
        
        _endTextRectTransform = endText.transform.GetChild(0).GetComponent<RectTransform>();
        _endTextMeshProUGUI = endText.GetComponentInChildren<TextMeshProUGUI>();
        
        _firstText = firstPartOfLeaderObj.GetComponentInChildren<TextMeshProUGUI>();
        _secondText = secondPartOfLeaderObj.GetComponentInChildren<TextMeshProUGUI>();
        _thirdText = thirdPartOfLeaderObj.GetComponentInChildren<TextMeshProUGUI>();
        _fourthText = fourthPartOfLeaderObj.GetComponentInChildren<TextMeshProUGUI>();

        _timerPlayerRect = timerPlayerObj.GetComponent<RectTransform>();
        _killsRect = killsObj.GetComponent<RectTransform>();
        _levelSliderRect = levelSliderObj.GetComponent<RectTransform>();
        _destroyableObjectRect = destroyableObjectObj.GetComponent<RectTransform>();
        _destroyableObjectOriginalYPosition = _destroyableObjectRect.position.y;
        _leaderBoardRect = leaderBoardObj.GetComponent<RectTransform>();
        _levelSlider = levelSliderObj.GetComponent<Slider>();
        _levelSliderOriginalYPosition = _levelSliderRect.position.y;
        
        _settingsPanel = settingsPanel.GetComponent<RectTransform>();
        
        _modeSelectMenuRect = modeSelectMenu.GetComponent<RectTransform>();
        
        _skinSelectMenuRect = skinSelectMenu.GetComponent<RectTransform>();
        
        _clickToPlayText = clickToPlayButton.GetComponentInChildren<TextMeshProUGUI>();
        _waitingMenuUIObjectsRectTransforms =
            waitingMenuUIObjects.ConvertAll(objects => objects.GetComponent<RectTransform>());
        _waitingMenuOriginalXPositions = _waitingMenuUIObjectsRectTransforms.ConvertAll(objects => objects.position.x);
    }

    private void Start()
    {
        DOTween.Init();
        
        DisappearsSkinUnlockScreen(0);
        DisappearsDeadScreen(0);
        DisappearsInGameUI(0);
        DisappearsSettingPanel(0);
        DisappearsGameModesMenu(0);
        DisappearsSkinSelectMenu(0);
        DisappearsNeedToContinueItems(0);
        DisappearsPauseItems(0);
        DisappearsEndLeaderboard(0);
        DisappearsWaitingMenuItems(0, () =>
        {
            if (GameController.Status == GameStatus.Loading)
            {
                GetLoadingScreen(0, () =>  SystemReady = true);
            }
        });
        
        ActionSys.GameModeChanged?.Invoke(GameMode.TimeCounting);
    }

    #region Subscription

    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interaction;
        ActionSys.LevelUpped += LevelUpped;
        ActionSys.GameStatusChanged += GameStatusChange;
        ActionSys.MaxLevelReached += MaxLevelReached;
    }

    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
        ActionSys.GameStatusChanged -= GameStatusChange;
        ActionSys.MaxLevelReached -= MaxLevelReached;
    }
    

    private void Interaction(IInteractable obj)
    {
        var diffBetweenGoalAndSlide = PlayerController.Player.RatioOfBetweenLevels - _levelSlider.value;
        StartCoroutine(SlideAnim(diffBetweenGoalAndSlide));
    }
    
    private void LevelUpped(int point)
    {
        UpdateNextLevelImages();
        StartCoroutine(SlideAnim(-_levelSlider.value));
    }
    
    #endregion


    private void Update()
    {

        #region Playing
        if (GameController.Status == GameStatus.Playing)
        {
            UpdateTimeOrPlayerCount();
            UpdateKillCount();
            UpdateLeaderboard();
            UpdateNextLevelImages();
        }
        #endregion
    }

    
    #region GameStatusChange

    private void GameStatusChange(GameStatus status)
    {
        
        switch (status)
        {
            case GameStatus.Loading:
                OpenLoadingScreen();
                break;
            case GameStatus.WaitingOnMenu:
                OpenWaitingMenu();
                break;
            case GameStatus.Playing:
                OpenPlayingUI();
                break;
            case GameStatus.Paused:
                OpenPausedScreenUI();
                break;
            case GameStatus.Lost:
                OpenLostGameUI();
                break;
            case GameStatus.Ended:
                OpenEndUI();
                break;
        }
        
    }
    
    
    private void OpenLoadingScreen(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetLoadingScreen(0, () => callback?.Invoke());
        });
    }

    // ReSharper disable once MemberCanBePrivate.Global Because Of Button can access them
    public void OpenWaitingMenu(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetWaitingMenuItems(animationDuration, () => callback?.Invoke());
        });
    }

    // ReSharper disable once MemberCanBePrivate.Global Because Of Button can access them
    public void OpenPlayingUI(Action callback = null)
    {
        UpdateLeaderboard();
        UpdateKillCount();
        UpdateTimeOrPlayerCount();
        UpdateNextLevelImages();
        
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetInGameUI(animationDuration, () => callback?.Invoke());
        });
    }

    private void OpenPausedScreenUI(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetPauseItems(animationDuration, () => callback?.Invoke());
        });
    }


    private void OpenEndUI(Action callback = null)
    {
        if (GameController.Mode == GameMode.TimeCounting)
        {
            _disappearsCurrentUI?.Invoke(animationDuration, () =>
            {
                GetNeedToContinueItems(animationDuration, () => callback?.Invoke());
            });
        }
        else if (GameController.Mode == GameMode.BeTheLast)
        {
            _disappearsCurrentUI?.Invoke(animationDuration, () =>
            {
                ShowEndText(animationDuration * 5, () =>
                {
                    GetEndLeaderboard(animationDuration, () => callback?.Invoke());
                });
            });
        }
    }

    private void OpenEndLeaderboardUI(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetEndLeaderboard(animationDuration, () => callback?.Invoke());
        });
    }

    private void OpenLostGameUI(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration,null);
        ShowEndText(animationDuration * 5, () =>
        {
            GetDeadScreen(animationDuration, () => callback?.Invoke());
        });    
    }

    private void OpenSkinUnlockUI(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetSkinUnlockScreen(animationDuration, () =>
            {
                skinUnlockScript.ScrollTheImage(GameController.Instance.PercentageCalculator(), animationDuration * 10, () =>
                {
                    if (GameController.Instance.NewSkinUnlocked())
                    {
                        GetNewSkinUnlockedText(animationDuration, () => callback?.Invoke());
                    }
                    else
                    {
                        GetSkinUnlockClickToContinue(animationDuration, () => callback?.Invoke());
                    }
                });
            });
        });
    }
    
    private void OpenGameModes(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetGameModesMenu(animationDuration, () => callback?.Invoke());
        });
    }
    
    private void OpenSettings(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetSettingsPanel(animationDuration,() => callback?.Invoke());
        });
    }

    private void OpenSkinMenu(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetSkinSelectMenu(animationDuration,() => callback?.Invoke());
        });
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
    
    private void GetLoadingScreen(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsLoadingScreen;
        loadingScreenParent.SetActive(true);
        _loadingBackgroundImage.DOFade(1, duration);
        _loadingTextMeshProUGUI.DOFade(1, duration).OnKill(() => callback?.Invoke());
    }
    private void DisappearsLoadingScreen(float duration, Action callback = null)
    {
        callback += () => loadingScreenParent.SetActive(false);
        _loadingBackgroundImage.DOFade(0, duration);
        _loadingTextMeshProUGUI.DOFade(0, duration).OnKill(() => callback?.Invoke());
    }
    
    private void GetWaitingMenuItems(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsWaitingMenuItems;
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
        callback += () => waitingMenuUI.SetActive(false);
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
        _disappearsCurrentUI = DisappearsSkinSelectMenu;
        skinSelectMenuParent.SetActive(true);
        
        _skinSelectMenuRect.DOMoveY(0, duration).OnKill(() => callback?.Invoke());

    }
    private void DisappearsSkinSelectMenu(float duration, Action callback = null)
    {
        callback += () => skinSelectMenuParent.SetActive(false);
        _skinSelectMenuRect.DOMoveY(-1 * _skinSelectMenuRect.rect.height, duration).OnKill(() => callback?.Invoke());
    }
    
    private void GetGameModesMenu(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsGameModesMenu;
        modeSelectMenuParent.SetActive(true);
        
        _modeSelectMenuRect.DOScaleY(1, duration).OnKill(() => callback?.Invoke());
    }
    private void DisappearsGameModesMenu(float duration, Action callback = null)
    {
        callback += () => modeSelectMenuParent.SetActive(false);
        _modeSelectMenuRect.DOScaleY(0, duration).OnKill(() => callback?.Invoke());
    }
    
    private void GetSettingsPanel(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsSettingPanel;
        settingsPanelParent.SetActive(true);

        _settingsPanel.DOMoveY((float) Screen.height / 2, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }
    private void DisappearsSettingPanel(float duration, Action callback = null)
    {
        callback += () => settingsPanelParent.SetActive(false);
        _settingsPanel.DOMoveY(-1 * _settingsPanel.rect.height / 2 * 5 / 4, duration).OnKill(() => callback?.Invoke());
    }
    
    private void GetInGameUI(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsInGameUI;
        gameUI.SetActive(true);
        
        _timerPlayerRect.DOMoveX(0, duration).SetEase(Ease.OutBack);
        _killsRect.DOMoveX(0, duration).SetEase(Ease.OutBack);
        _levelSliderRect.DOMoveY(_levelSliderOriginalYPosition, duration).SetEase(Ease.OutBack);
        _destroyableObjectRect.DOMoveY(_destroyableObjectOriginalYPosition, duration).SetEase(Ease.OutBack);
        _leaderBoardRect.DOMoveX(Screen.width, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }
    private void DisappearsInGameUI(float duration, Action callback = null)
    {
        callback += () => gameUI.SetActive(false);
        
        _timerPlayerRect.DOMoveX(-1 * _timerPlayerRect.rect.width, duration);
        _killsRect.DOMoveX(-1 * _killsRect.rect.width, duration);
        _levelSliderRect.DOMoveY(Screen.height + _levelSliderRect.rect.height * 5/3, duration);
        _destroyableObjectRect.DOMoveY(Screen.height + _destroyableObjectRect.rect.height * 4 / 3, duration);
        _leaderBoardRect.DOMoveX(Screen.width + _leaderBoardRect.rect.width, duration).OnKill(() => callback?.Invoke());
    }
    
    private void ShowEndText(float duration, Action callback = null)
    {
        endText.SetActive(true);
        
        callback += () => endText.SetActive(false);
        
        _endTextRectTransform.DOScaleX( 1.1f, duration);
        _endTextRectTransform.DOScaleY(1.1f, duration).OnKill(() =>
        {
            callback?.Invoke();
            _endTextRectTransform.DOScaleX( 1f, 0);
            _endTextRectTransform.DOScaleY(1f, 0);
        });
        
        _endTextMeshProUGUI.DOFade(1, duration / 4).OnKill(() =>
        {
            _endTextMeshProUGUI.DOFade(1, duration / 2).OnKill(() =>
            {
                _endTextMeshProUGUI.DOFade(0, duration / 4);
                
            });
        });
        
    }
    
    private void GetNeedToContinueItems(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsNeedToContinueItems;
        needToContinueParent.SetActive(true);
        _needToContinueBackground.DOFade(_needToContinueAlphaAmountBackground, duration).OnKill((() => callback?.Invoke()));
        
        for (var i = 0; i < _needToContinueRectTransforms.Count; i++)
        {
            _needToContinueRectTransforms[i].DOMoveY(_needToContinueRectOriginalYPositions[i], duration)
                    .SetEase(Ease.OutBack);
        }
    }
    private void DisappearsNeedToContinueItems(float duration, Action callback = null)
    {
        callback += () => needToContinueParent.SetActive(false);
        
        _needToContinueBackground.DOFade(0, duration).OnKill((() => callback?.Invoke()));
        _needToContinueRectTransforms.ForEach(rectTransform =>
        {
            rectTransform.DOMoveY(rectTransform.position.y - Screen.height, duration);
        });
    }
    
    private void GetPauseItems(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsPauseItems;
        pauseParent.SetActive(true);
        _pauseBackground.DOFade(_pauseAlphaAmountBackground, duration).OnKill((() => callback?.Invoke()));
        
        for (var i = 0; i < _pauseRectTransforms.Count; i++)
        {
            _pauseRectTransforms[i].DOMoveY(_pauseRectOriginalYPositions[i], duration)
                .SetEase(Ease.OutBack);
        }
    }
    private void DisappearsPauseItems(float duration, Action callback = null)
    {
        callback += () => pauseParent.SetActive(false);
        _pauseBackground.DOFade(0, duration).OnKill((() => callback?.Invoke()));
        _pauseRectTransforms.ForEach(rectTransform =>
        {
            rectTransform.DOMoveY(rectTransform.position.y - Screen.height, duration);
        });
    }
    
    private void GetEndLeaderboard(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsEndLeaderboard;
        
        endLeaderboardParent.SetActive(true);
        
        _timeOutOrPlayerCountRect.DOMoveX(_timeOrPlayerCountOriginalXPosition, duration).SetEase(Ease.OutBack).OnKill(
            () =>
            {
                StartCoroutine(DelayedStart());
                IEnumerator DelayedStart()
                {
                    for (var i = 0; i < _imagesOfLeaderboardObject.Count; i++)
                    {
                        _imagesOfLeaderboardObject[i].DOFade(_alphasOfLeaderboardObjects[i], duration);
                        _textMeshProsOfLeaderboard[i].DOFade(1, duration);
                        yield return new WaitForSeconds(duration / 2);
                    }

                    _rewardButtonRect.DOMoveX(_rewardOriginalXPosition, duration).OnKill(() =>
                    {
                        _clickToContinueText.DOFade(1, duration).OnKill(() => callback?.Invoke());
                    });
                }            
            });
    }
    private void DisappearsEndLeaderboard(float duration, Action callback = null)
    {

        callback += () => endLeaderboardParent.SetActive(false);
        _timeOutOrPlayerCountRect.DOMoveX(-1 * _timeOutOrPlayerCountRect.rect.width, duration).SetEase(Ease.OutBack).OnKill(
            () =>
            {
                StartCoroutine(DelayedStart());
                IEnumerator DelayedStart()
                {
                    for (var i = 0; i < _imagesOfLeaderboardObject.Count; i++)
                    {
                        _imagesOfLeaderboardObject[i].DOFade(0, duration);
                        _textMeshProsOfLeaderboard[i].DOFade(0, duration);
                        yield return new WaitForSeconds(duration / 2);
                    }

                    _rewardButtonRect.DOMoveX(-1 * _rewardButtonRect.rect.width, duration).OnKill(() =>
                    {
                        _clickToContinueText.DOFade(0, duration).OnKill(() => callback?.Invoke());
                    });
                }            
            });
    }
    
    private void GetDeadScreen(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsDeadScreen;
        deadScreenParent.SetActive(true);
        
        _deadScreenBackgroundImg.DOFade(_alphaOfDeadScreenBackGround, duration).OnKill(() =>
        {
            _deadScreenYouLostRect.DOMoveX((float) Screen.width / 2, duration).SetEase(Ease.OutBack).OnKill(() =>
            {
                _deadScreenScoreTextRect.DOMoveX((float) Screen.width / 2, duration).SetEase(Ease.OutBack).OnKill(() =>
                {
                    _deadScreenClickToContinue.DOFade(1, duration).OnKill(() => callback?.Invoke());
                });
            });
        });
    }
    private void DisappearsDeadScreen(float duration, Action callback = null)
    {
        callback += () => deadScreenParent.SetActive(false);
        _deadScreenBackgroundImg.DOFade(0, duration).OnKill(() =>
        {
            _deadScreenYouLostRect.DOMoveX(-1 * _deadScreenYouLostRect.rect.width, duration).SetEase(Ease.OutBack).OnKill(() =>
            {
                _deadScreenScoreTextRect.DOMoveX(-1 * _deadScreenScoreTextRect.rect.width, duration).SetEase(Ease.OutBack).OnKill(() =>
                {
                    _deadScreenClickToContinue.DOFade(0, duration).OnKill(() => callback?.Invoke());
                });
            });
        });
    }

    #region SkinUnlockScreenEnvo

    private void GetSkinUnlockScreen(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsSkinUnlockScreen;
        skinUnlockParent.SetActive(true);
        _skinTotalScoreTextRect.DOMoveY(_skinTotalScoreTextOriginalYPosition, duration).SetEase(Ease.OutBack);
        _skinUnlockRect.DOScaleX(1f, duration).SetEase(Ease.OutBack);
        _skinUnlockRect.DOScaleY(1f, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }
    private void DisappearsSkinUnlockScreen(float duration, Action callback = null)
    {
        callback += () => skinUnlockParent.SetActive(false);
        DisappearNewSkinUnlockedText(duration);
        _skinTotalScoreTextRect.DOMoveY(Screen.height + _skinTotalScoreTextRect.rect.height, duration);
        _skinUnlockRect.DOScaleX(0, duration);
        _skinUnlockRect.DOScaleY(0, duration).OnKill(() => callback?.Invoke());
    }

    private void GetNewSkinUnlockedText(float duration, Action callback = null)
    {
        newSkinUnlockedText.SetActive(true);
        _newSkinUnlockedTextRect.DOScaleX(1f, duration).SetEase(Ease.OutBack);
        _newSkinUnlockedTextRect.DOScaleY(1f, duration).SetEase(Ease.OutBack).OnKill(() =>
        {
            GetSkinUnlockClickToContinue(duration, () => callback?.Invoke());
        });
    }
    private void DisappearNewSkinUnlockedText(float duration, Action callback = null)
    {
        callback += () => newSkinUnlockedText.SetActive(false);
        DisappearSkinUnlockClickToContinue(duration);
        _newSkinUnlockedTextRect.DOScaleX(0f, duration);
        _newSkinUnlockedTextRect.DOScaleY(0f, duration).OnKill(() => callback?.Invoke());
    }

    private void GetSkinUnlockClickToContinue(float duration, Action callback = null)
    {
        skinUnlockClickToContinue.SetActive(true);
        _skinUnlockClickToContinueText.DOFade(1, duration).OnKill(() => callback?.Invoke());
    }
    
    private void DisappearSkinUnlockClickToContinue(float duration, Action callback = null)
    {
        callback += () => skinUnlockClickToContinue.SetActive(false);
        _skinUnlockClickToContinueText.DOFade(0, duration).OnKill(() => callback?.Invoke());
    }

    #endregion




    #endregion

    #region ButtonMethods


    public void OpenGameModesButton()
    {
       OpenGameModes();
    }
    
    public void OpenSettingsButton()
    {
        OpenSettings();
    }

    public void OpenSkinMenuButton()
    {
       OpenSkinMenu();
    }

    
    public void OpenWaitingMenuButton()
    {
        OpenWaitingMenu();
    }
    
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



    public void StartGameButton()
    {
        ActionSys.GameStatusChanged?.Invoke(GameStatus.Playing);
    }

    public void ClickToContinueHalfTime()
    {
        ActionSys.GameStatusChanged?.Invoke(GameStatus.Playing);
    }

    public void ClickToContinueDeadScreen()
    {
        OpenLoadingScreen(() =>
        {
            Debug.LogError("Not Implemented");
        });
    }

    public void ClickToContinueEndLeaderboard()
    {
        if (GameController.Instance.IsThereAnyNewSkin())
        {
            OpenSkinUnlockUI();
        }
        else
        {
            OpenLoadingScreen(() =>
            {
                Debug.LogError("Not Implemented");
            });
        }
    }

    public void ClickToContinueSkinUnlock()
    {
        OpenLoadingScreen(() =>
        {
            Debug.LogError("Not Implemented");
        });
    }
    public void ExtraTimeButton()
    {
        Debug.LogError("Not Implemented");
    }

    public void ClickToContinueNeedToContinue()
    {
        OpenEndLeaderboardUI();
    }

    #endregion

    #region GameDynamics

    private void UpdateLeaderboard()
    {
        var stat = new List<Player>();
        switch (GameController.Mode)
        {
            case GameMode.TimeCounting:
                stat = LeaderboardsAbstract.Instance.GetLeaderBoard(3, true);
                break;
            case GameMode.BeTheLast:
                stat = LeaderboardsAbstract.Instance.GetLeaderBoard(3);
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    if (stat.Count < 1)
                    {
                        firstPartOfLeaderObj.SetActive(false);
                    }
                    else
                    {
                        _firstText.text = "1-" + stat[0].Name;
                        _firstText.color = stat[0].PlayerColor;
                    }
                    break;
                case 1:
                    if (stat.Count < 2)
                    {
                        secondPartOfLeaderObj.SetActive(false);
                    }
                    else
                    {
                        _secondText.text = "2-" + stat[1].Name;
                        _secondText.color = stat[1].PlayerColor;
                    }
                    break;
                case 2:
                    if (stat.Count < 3)
                    {
                        thirdPartOfLeaderObj.SetActive(false);
                    }
                    else
                    {
                        _thirdText.text = "3-" + stat[2].Name;
                        _thirdText.color = stat[2].PlayerColor;
                    }
                    break;
                case 3:
                    if(stat.All(player => player.Name != "You"))
                    {
                        fourthPartOfLeaderObj.SetActive(true);
                        var player = LeaderboardsAbstract.Instance.GetPlayerByName("You");
            
                        switch (GameController.Mode)
                        {
                            case GameMode.TimeCounting:
                                _fourthText.text = LeaderboardsAbstract.Instance.GetPlayerRank(player, true) + "-" + player.Name;
                                break;
                            case GameMode.BeTheLast:
                                _fourthText.text = LeaderboardsAbstract.Instance.GetPlayerRank(player) + "-" + player.Name;
                                break;
                        }
            
                        _fourthText.text = LeaderboardsAbstract.Instance.GetPlayerRank(player, true) + "-" + player.Name;
                        _fourthText.color = player.PlayerColor;
                    }
                    else
                    {
                        fourthPartOfLeaderObj.SetActive(false);   
                    }
                    break;
            }
        }
    }

    private void UpdateTimeOrPlayerCount()
    {
        switch (GameController.Mode)
        {
            case GameMode.TimeCounting:
                timePlayerText.text = MinuteAndSecondConverter(GameController.Instance.TimeLeft);
                break;
            case GameMode.BeTheLast:
                timePlayerText.text = "Player: " + LeaderboardsAbstract.Instance.AlivePlayerCount;
                break;
        }
    }

    private void UpdateKillCount()
    {
        killText.text = "Kills: " + PlayerController.Player.PlayerProperty.KillCount;
    }

    private void UpdateNextLevelImages()
    {
        var currentLevel = PlayerController.Player.Level;
        var nextLevel = currentLevel + 1;

        if (GameController.GameConfig.DestroyThresholdsFromLevels.Contains(currentLevel))
        {
            var index = GameController.GameConfig.DestroyThresholdsFromLevels.IndexOf(currentLevel);
            destroyableObjectObjItems[index].SetActive(true);
        }
        
        if (GameController.GameConfig.DestroyThresholdsFromLevels.Contains(nextLevel))
        {
            var index = GameController.GameConfig.DestroyThresholdsFromLevels.IndexOf(nextLevel);
            endOfSliderImage.sprite = newDestroyableObjectImages[index];
        }
        else
        {
            endOfSliderImage.sprite = normalImage;
        }
    }

    private void MaxLevelReached()
    {
        _destroyableObjectRect.DOMoveY(_levelSliderOriginalYPosition, animationDuration);
        _levelSliderRect.DOMoveY(Screen.height + _levelSliderRect.rect.height * 5/3, animationDuration);
    }

    #endregion

    #region Utilities

    private static string MinuteAndSecondConverter(int seconds)
    {
        return seconds % 60 >= 10 ? seconds / 60 + ":" + seconds % 60 : seconds / 60 + ":0" + seconds % 60;
    }

    #endregion

}
