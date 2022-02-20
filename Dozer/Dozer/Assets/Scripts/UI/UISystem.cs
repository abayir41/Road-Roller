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
    [SerializeField] private List<GameObject> skinsContentItems;
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
    private float _alphaBackgroundAmount;
    private Image _firstLeaderBackground;
    private Image _secondLeaderBackground;
    private Image _thirdLeaderBackground;
    private Image _fourthLeaderBackground;
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
    [SerializeField] private GameObject needToContinueTimeOut;
    [SerializeField] private GameObject needToContinueRewardImage;
    [SerializeField] private GameObject needToContinueRewardButton;
    [SerializeField] private GameObject needToContinueBackGround;
    [SerializeField] private GameObject needToContinueClickToContinue;
    private TextMeshProUGUI _needToContinueClickToContinueText;
    private float _needToContinueAlphaAmountBackground;
    private Image _needToContinueBackground;
    private RectTransform _needToContinueTimeOutRect;
    private RectTransform _needToContinueRewardImageRect;
    private RectTransform _needToContinueRewardButtonRect;
    private float _needToContinueTimeOutOriginalYPosition;

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
    private TextMeshProUGUI _deadScreenYourScoreText;
    private TextMeshProUGUI _deadScreenClickToContinue;

    [Header("Skin Unlock Screen")] 
    [SerializeField] private GameObject skinUnlockParent;
    [SerializeField] private GameObject skinTotalScoreTextObj;
    [SerializeField] private GameObject skinUnlockObj;
    [SerializeField] private GameObject newSkinUnlockedText;
    [SerializeField] private GameObject skinUnlockClickToContinue;
    [SerializeField] private SkinUnlockUI skinUnlockScript;
    [SerializeField] private List<GameObject> skinUnlockImageObjs;
    private RectTransform _newSkinUnlockedTextRect;
    private TextMeshProUGUI _skinUnlockTotalScoreText;
    private List<Image> _skinUnlockNewSkinImages;
    private TextMeshProUGUI _skinUnlockClickToContinueText;
    private RectTransform _skinTotalScoreTextRect;
    private RectTransform _skinUnlockRect;
    private float _skinTotalScoreTextOriginalYPosition;
    

    private void Awake()
    {
        if(Instance == null)
            Instance = this;


        _skinUnlockNewSkinImages = skinUnlockImageObjs.ConvertAll(input => input.GetComponent<Image>());
        _skinUnlockTotalScoreText = skinTotalScoreTextObj.GetComponentInChildren<TextMeshProUGUI>();
        _newSkinUnlockedTextRect = newSkinUnlockedText.GetComponent<RectTransform>();
        _skinUnlockClickToContinueText = skinUnlockClickToContinue.GetComponent<TextMeshProUGUI>();
        _skinTotalScoreTextRect = skinTotalScoreTextObj.GetComponent<RectTransform>();
        _skinUnlockRect = skinUnlockObj.GetComponent<RectTransform>();
        _skinTotalScoreTextOriginalYPosition = _skinTotalScoreTextRect.position.y;

        _deadScreenYourScoreText = deadScreenScoreText.GetComponent<TextMeshProUGUI>();
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

        _needToContinueClickToContinueText = needToContinueClickToContinue.GetComponentInChildren<TextMeshProUGUI>();
        _needToContinueBackground = needToContinueBackGround.GetComponent<Image>();
        _needToContinueAlphaAmountBackground = _needToContinueBackground.color.a;
        _needToContinueRewardButtonRect = needToContinueRewardButton.GetComponent<RectTransform>();
        _needToContinueRewardImageRect = needToContinueRewardImage.GetComponent<RectTransform>();
        _needToContinueTimeOutRect = needToContinueTimeOut.GetComponent<RectTransform>();
        _needToContinueTimeOutOriginalYPosition = _needToContinueTimeOutRect.position.y;
        
        _endTextRectTransform = endText.transform.GetChild(0).GetComponent<RectTransform>();
        _endTextMeshProUGUI = endText.GetComponentInChildren<TextMeshProUGUI>();
        
        _firstLeaderBackground = firstPartOfLeaderObj.GetComponent<Image>();
        _secondLeaderBackground = secondPartOfLeaderObj.GetComponent<Image>();
        _thirdLeaderBackground = thirdPartOfLeaderObj.GetComponent<Image>();
        _fourthLeaderBackground = fourthPartOfLeaderObj.GetComponent<Image>();
        _alphaBackgroundAmount = _firstLeaderBackground.color.a;
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
        DOTween.Init(logBehaviour:LogBehaviour.Verbose);
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
                UpdateEndLeaderBoard();
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
                if (GameController.Instance.IsTimeOutRewardVideoReady())
                {
                    GetNeedToContinueItems(animationDuration, () =>
                    {
                        GetNeedToContinueVideoItems(animationDuration, () =>
                        {
                            GetNeedToContinueClickToContinue(animationDuration, () => callback?.Invoke());
                        });
                    });
                }
                else
                {
                    GetNeedToContinueItems(animationDuration, () =>
                    {
                        GetNeedToContinueClickToContinue(animationDuration, () => callback?.Invoke());
                    });
                }
            });
        }
        else if (GameController.Mode == GameMode.BeTheLast)
        {
            UpdateEndText("You WON!!");
            
            _disappearsCurrentUI?.Invoke(animationDuration, () =>
            {
                ShowEndText(animationDuration * 5, () =>
                {
                    if (GameController.Instance.IsEndLeaderboardVideoReady())
                    {
                        GetEndLeaderboard(animationDuration, () =>
                        {
                            GetEndLeaderboardRewardButton(animationDuration, () =>
                            {
                                GetEndLeaderboardClickToContinue(animationDuration, () => callback?.Invoke());
                            });       
                        });
                    }
                    else
                    {
                        GetEndLeaderboard(animationDuration, () =>
                        {
                            GetEndLeaderboardClickToContinue(animationDuration, () => callback?.Invoke());
                        });
                    }
                });
            });
        }
    }

    private void OpenEndLeaderboardUI(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            if (GameController.Instance.IsEndLeaderboardVideoReady())
            {
                GetEndLeaderboard(animationDuration, () =>
                {
                    GetEndLeaderboardRewardButton(animationDuration, () =>
                    {
                        GetEndLeaderboardClickToContinue(animationDuration, () => callback?.Invoke());
                    });       
                });
            }
            else
            {
                GetEndLeaderboard(animationDuration, () =>
                {
                    GetEndLeaderboardClickToContinue(animationDuration, () => callback?.Invoke());
                });
            }
        });
    }

    private void OpenLostGameUI(Action callback = null)
    {
        UpdateEndText("You CRASHED!!");
        UpdateDeadScreen();
        
        _disappearsCurrentUI?.Invoke(animationDuration,null);
        ShowEndText(animationDuration * 5, () =>
        {
            GetDeadScreen(animationDuration, () => callback?.Invoke());
        });    
    }

    private void OpenSkinUnlockUI(Action callback = null)
    {
        UpdateSkinUnlockScreen();
        
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetSkinUnlockScreen(animationDuration, () =>
            {
                ScoreIncreaseAnim(animationDuration * 10);
                skinUnlockScript.ScrollTheImage(GameController.PercentageCalculator(), animationDuration * 10, () =>
                {
                    if (GameController.NewSkinUnlocked())
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

    #region NeedToContinue

        private void GetNeedToContinueItems(float duration, Action callback = null)
    {
        _disappearsCurrentUI = DisappearsNeedToContinueItems;
        needToContinueParent.SetActive(true);
        _needToContinueBackground.DOFade(_needToContinueAlphaAmountBackground, duration).OnKill(() => callback?.Invoke());
        _needToContinueTimeOutRect.DOMoveY(_needToContinueTimeOutOriginalYPosition, duration).SetEase(Ease.OutBack);

    }
    private void DisappearsNeedToContinueItems(float duration, Action callback = null)
    {
        callback += () => needToContinueParent.SetActive(false);
        DisappearsNeedToContinueVideoItems(duration);
        _needToContinueBackground.DOFade(0, duration).OnKill(() => callback?.Invoke());
        _needToContinueTimeOutRect.DOMoveY(Screen.height + _needToContinueTimeOutRect.rect.height, duration);
    }

    private void GetNeedToContinueVideoItems(float duration, Action callback = null)
    {
        _needToContinueRewardImageRect.DOScaleY(1f, duration).SetEase(Ease.OutBack);
        _needToContinueRewardImageRect.DOScaleX(1f, duration).SetEase(Ease.OutBack);

        _needToContinueRewardButtonRect.DOScaleX(1f, duration).SetEase(Ease.OutBack);
        _needToContinueRewardButtonRect.DOScaleY(1f, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }
    
    private void DisappearsNeedToContinueVideoItems(float duration, Action callback = null)
    {
        DisappearNeedToContinueClickToContinue(duration);
        _needToContinueRewardImageRect.DOScaleY(0f, duration).SetEase(Ease.OutBack);
        _needToContinueRewardImageRect.DOScaleX(0f, duration).SetEase(Ease.OutBack);

        _needToContinueRewardButtonRect.DOScaleX(0f, duration).SetEase(Ease.OutBack);
        _needToContinueRewardButtonRect.DOScaleY(0f, duration).SetEase(Ease.OutBack).OnKill(() => callback?.Invoke());
    }

    private void GetNeedToContinueClickToContinue(float duration, Action callback = null)
    {
        _needToContinueClickToContinueText.DOFade(1f, duration).OnKill(() => callback?.Invoke());
    }
    
    private void DisappearNeedToContinueClickToContinue(float duration, Action callback = null)
    {
        _needToContinueClickToContinueText.DOFade(0f, duration).OnKill(() => callback?.Invoke());
    }

    #endregion
    
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

    #region EndLeaderboard
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
                    var players = LeaderboardsAbstract.Instance.GetLeaderBoard(true);
                    var capacity = players.Count >= _textMeshProsOfLeaderboard.Count
                        ? _textMeshProsOfLeaderboard.Count
                        : players.Count;
                    
                    for (var i = 0; i < capacity; i++)
                    {
                        if (i == capacity - 1)
                        {
                            _imagesOfLeaderboardObject[i].DOFade(_alphasOfLeaderboardObjects[i], duration);
                            _textMeshProsOfLeaderboard[i].DOFade(1, duration).OnKill(() => callback?.Invoke());
                        }
                        else
                        {
                            _imagesOfLeaderboardObject[i].DOFade(_alphasOfLeaderboardObjects[i], duration);
                            _textMeshProsOfLeaderboard[i].DOFade(1, duration);
                            yield return new WaitForSeconds(duration / 2);
                        }
                    }
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
                    var players = LeaderboardsAbstract.Instance.GetLeaderBoard(true);
                    var capacity = players.Count >= _textMeshProsOfLeaderboard.Count
                        ? _textMeshProsOfLeaderboard.Count
                        : players.Count;
                    
                    for (var i = 0; i < _imagesOfLeaderboardObject.Count; i++)
                    {
                        if (i == capacity - 1)
                        {
                            _imagesOfLeaderboardObject[i].DOFade(0, duration);
                            _textMeshProsOfLeaderboard[i].DOFade(0, duration);
                        }
                        else
                        {
                            _imagesOfLeaderboardObject[i].DOFade(0, duration);
                            _textMeshProsOfLeaderboard[i].DOFade(0, duration);
                            yield return new WaitForSeconds(duration / 4);
                        }
                    }
                    
                    DisappearsEndLeaderboardRewardButton(duration, () => callback?.Invoke());
                }            
            });
    }

    private void GetEndLeaderboardRewardButton(float duration, Action callback = null)
    {
        _rewardButtonRect.DOMoveX(_rewardOriginalXPosition, duration).OnKill(() => callback?.Invoke());
    }

    private void DisappearsEndLeaderboardRewardButton(float duration, Action callback = null)
    {
        DisappearEndLeaderboardClickToContinue(duration);
        _rewardButtonRect.DOMoveX(-1 * _rewardButtonRect.rect.width, duration).OnKill(() => callback?.Invoke());
    }

    private void GetEndLeaderboardClickToContinue(float duration, Action callback = null)
    {
        _clickToContinueText.DOFade(1, duration).OnKill(() => callback?.Invoke());
    }
    
    private void DisappearEndLeaderboardClickToContinue(float duration, Action callback = null)
    {
        _clickToContinueText.DOFade(0, duration).OnKill(() => callback?.Invoke());
    }
    

    #endregion

    
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
            _deadScreenYouLostRect.DOMoveX(-3 * _deadScreenYouLostRect.rect.width, duration).SetEase(Ease.OutBack).OnKill(() =>
            {
                _deadScreenScoreTextRect.DOMoveX(-3 * _deadScreenScoreTextRect.rect.width, duration).SetEase(Ease.OutBack).OnKill(() =>
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

    private void ScoreIncreaseAnim(float duration, Action callback = null)
    {
        var x = (float)(GameController.TotalScore -
                 LeaderboardsAbstract.Instance.GetPlayerByName("You").Score);
        DOTween.To(() => x, value =>
        {
            x = value;
            _skinUnlockTotalScoreText.text = "Total Score: " + (int)x;
        }, GameController.TotalScore, duration).OnKill(() => callback?.Invoke());
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
            ActionSys.ResetGame?.Invoke();
        });
    }

    public void ClickToContinueEndLeaderboard()
    {
        if (GameController.IsThereAnyNewSkin())
        {
            OpenSkinUnlockUI();
        }
        else
        {
            OpenLoadingScreen(() =>
            {
                 ActionSys.ResetGame?.Invoke();
            });
        }
    }

    public void ClickToContinueSkinUnlock()
    {
        OpenLoadingScreen(() =>
        {
            ActionSys.ResetGame?.Invoke();
        });
    }
    public void ExtraTimeButton()
    {
        Debug.LogError("Not Implemented");
    }
    
    public void DoubleScoreButton()
    {
        Debug.LogError("Not Implemented");
    }

    public void ClickToContinueNeedToContinue()
    {
        OpenEndLeaderboardUI();
    }

    private void RefreshSkinSelectItems()
    {
        skinsContentItems.ForEach(o => o.SetActive(false));
        
        for (int i = 0; i < GameController.GameConfig.DozerSkins.Count; i++)
        {
            skinsContentItems[i].transform.GetChild(0).GetComponent<Image>().sprite =
                GameController.GameConfig.DozerSkins[i].Preview;
        }
        for (int i = 0; i <= GameController.UnlockedSkinIndex; i++)
        {
            skinsContentItems[i].SetActive(true);
        }
    }

    public void SkinSelected(int id)
    {
        for (int i = 0; i < skinsContentItems.Count; i++)
        {
            if(i == id) 
                skinsContentItems[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            else
                skinsContentItems[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
        
        ActionSys.SkinSelected?.Invoke(id);
    }
    
    #endregion

    #region GameDynamics

    private void UpdateLeaderboard()
    {
        var stat = new List<Player>();
        switch (GameController.Mode)
        {
            case GameMode.TimeCounting:
                stat = LeaderboardsAbstract.Instance.GetLeaderBoard(3);
                break;
            case GameMode.BeTheLast:
                stat = LeaderboardsAbstract.Instance.GetLeaderBoard(3);
                break;
        }

        
        if (stat.Count < 1)
        {
            var c = _firstLeaderBackground.color;
            _firstLeaderBackground.color = new Color(c.r, c.g, c.b, 0);
            
            c = _firstText.color;
            _firstText.color = new Color(c.r, c.g, c.b, 0);
        }
        else
        {
            var c = _firstLeaderBackground.color;
            _firstLeaderBackground.color = new Color(c.r, c.g, c.b, _alphaBackgroundAmount);
            
            c = _firstText.color;
            _firstText.color = new Color(c.r, c.g, c.b, 1);
            
            _firstText.text = "1-" + stat[0].Name;
            _firstText.color = stat[0].PlayerColor;
        }
        
        if (stat.Count < 2)
        {
            var c = _secondLeaderBackground.color;
            _secondLeaderBackground.color = new Color(c.r, c.g, c.b, 0);
            
            c = _secondText.color;
            _secondText.color = new Color(c.r, c.g, c.b, 0);
        }
        else
        {
            var c = _secondLeaderBackground.color;
            _secondLeaderBackground.color = new Color(c.r, c.g, c.b, _alphaBackgroundAmount);
            
            c = _secondText.color;
            _secondText.color = new Color(c.r, c.g, c.b, 1);
            
            _secondText.text = "2-" + stat[1].Name;
            _secondText.color = stat[1].PlayerColor;
        }
        
        
        if (stat.Count < 3)
        {
            var c = _thirdLeaderBackground.color;
            _thirdLeaderBackground.color = new Color(c.r, c.g, c.b, 0);
            
            c = _thirdText.color;
            _thirdText.color = new Color(c.r, c.g, c.b, 0);
        }
        else
        {
            var c = _thirdLeaderBackground.color;
            _thirdLeaderBackground.color = new Color(c.r, c.g, c.b, _alphaBackgroundAmount);
            
            c = _thirdText.color;
            _thirdText.color = new Color(c.r, c.g, c.b, 1);
            
            _thirdText.text = "3-" + stat[2].Name;
            _thirdText.color = stat[2].PlayerColor;
        }
        
        if(stat.All(player => player.Name != "You"))
        {
            var c = _fourthLeaderBackground.color;
            _fourthLeaderBackground.color = new Color(c.r, c.g, c.b, _alphaBackgroundAmount);
            
            c = _fourthText.color;
            _fourthText.color = new Color(c.r, c.g, c.b, 1);
            
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
            var c = _fourthLeaderBackground.color;
            _fourthLeaderBackground.color = new Color(c.r, c.g, c.b, 0);
            
            c = _fourthText.color;
            _fourthText.color = new Color(c.r, c.g, c.b, 0);
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

    private void UpdateEndLeaderBoard()
    {

        var players = LeaderboardsAbstract.Instance.GetLeaderBoard(true);
        var capacity = players.Count >= _textMeshProsOfLeaderboard.Count
            ? _textMeshProsOfLeaderboard.Count
            : players.Count;
        
        for (int i = 0; i < capacity; i++)
        {
            _textMeshProsOfLeaderboard[i].text = (i+1) + "- " + players[i].Name;
            _textMeshProsOfLeaderboard[i].color = players[i].PlayerColor;
        }
    }

    private void UpdateEndText(string text)
    {
        _endTextMeshProUGUI.text = text;
    }

    private void UpdateSkinUnlockScreen()
    {
        _skinUnlockTotalScoreText.text = "Total Score: " +
                                    (GameController.TotalScore -
                                     LeaderboardsAbstract.Instance.GetPlayerByName("You").Score);
        _skinUnlockNewSkinImages.ForEach(image => image.sprite = GameController.GameConfig.DozerSkins[GameController.UnlockedSkinIndex + 1].Preview);
    }

    private void UpdateDeadScreen()
    {
        _deadScreenYourScoreText.text = "Your Score: " + LeaderboardsAbstract.Instance.GetPlayerByName("You").Score;
    }

    #endregion

    #region Utilities

    private static string MinuteAndSecondConverter(int seconds)
    {
        return seconds % 60 >= 10 ? seconds / 60 + ":" + seconds % 60 : seconds / 60 + ":0" + seconds % 60;
    }

    #endregion

    public void ResetTheSystem()
    {
        _disappearsCurrentUI = DisappearsLoadingScreen;
        skinUnlockScript.ResetTheSystem();
        RefreshSkinSelectItems();
        SkinSelected(GameController.SelectedSkinIndex);
        
        
        DisappearsSkinUnlockScreen(0);
        DisappearsDeadScreen(0);
        DisappearsInGameUI(0);
        DisappearsSettingPanel(0);
        DisappearsGameModesMenu(0);
        DisappearsSkinSelectMenu(0);
        DisappearsNeedToContinueItems(0);
        DisappearsPauseItems(0);
        DisappearsEndLeaderboard(0);
        DisappearsWaitingMenuItems(0);
        
    }

}
