﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UISystem : MonoBehaviour, ISystem
{

    public static UISystem Instance;

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

    [Header("Skin Unlock")] 
    [SerializeField] private SkinUnlockUI skinUnlockUI;
    private int _totalScore;
    
    [Header("End Leaderboard Items")] 
    [SerializeField] private GameObject timeOutOrPlayerCount;
    [SerializeField] private List<GameObject> leaderBoardObjects;
    [SerializeField] private GameObject rewardButtonObject;
    [SerializeField] private GameObject clickToContinueObject;
    private List<Image> _colorsOfLeaderboardObject;
    private List<Color> _colorsOfTextMeshPros;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        System = this;

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
        
        DisappearsInGameUI(0);
        DisappearsSettingPanel(0);
        DisappearsGameModesMenu(0);
        DisappearsSkinSelectMenu(0);
        DisappearsNeedToContinueItems(0);
        DisappearsPauseItems(0);
        DisappearsWaitingMenuItems(0, () =>
        {
            if (GameController.Status == GameStatus.Loading)
            {
                GetLoadingScreen(0);
            }
            SystemReady = true;
        });
        
        ActionSys.GameModeChanged?.Invoke(GameMode.BeTheLast);
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
        if(!GameInitializer.MapControllerReady) return;

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
                Loading();
                break;
            case GameStatus.WaitingOnMenu:
                OpenWaitingMenu();
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
        }
        
    }
    
    private void Loading(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetLoadingScreen(0, () => callback?.Invoke());
        });
    }

    public void OpenWaitingMenu(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetWaitingMenuItems(animationDuration, () => callback?.Invoke());
        });
    }

    private void PlayingUI(Action callback = null)
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

    private void Paused(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetPauseItems(animationDuration, () => callback?.Invoke());
        });
    }


    private void EndUI(Action callback = null)
    {
        _disappearsCurrentUI?.Invoke(animationDuration, (() =>
        {
            
        }));
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

    

    #endregion

    #region ButtonMethods

    public void OpenGameModes()
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetGameModesMenu(animationDuration);
        });
    }

    public void OpenSettings()
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetSettingsPanel(animationDuration);
        });
    }

    public void OpenSkinMenu()
    {
        _disappearsCurrentUI?.Invoke(animationDuration, () =>
        {
            GetSkinSelectMenu(animationDuration);
        });
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

    public void StartGame()
    {
       _disappearsCurrentUI?.Invoke(animationDuration, () =>
       {
           ActionSys.GameStatusChanged?.Invoke(GameStatus.Playing);
       });
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
                timePlayerText.text = MinuteAndSecondConverter(MapController.Instance.TimeLeft);
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
