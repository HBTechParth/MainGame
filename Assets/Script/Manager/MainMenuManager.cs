using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;
using WebSocketSharp;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;
using WebSocketSharp;
using System;
using System.Linq;
using DG.Tweening;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{

    public static MainMenuManager Instance;

    [Header("--- Home ---")]
    public Image avatarImg;
    public Text userNameTxt;
    public Text userIdTxt;
    public Text coinTxt;
    public Text bonusTxt;
    public Text diamondTxt;
    public TournamentData tourData;
    public Slider tableSet;
    //public Text[] entryValues;
    public List<Text> entryValues;
    public Text referenceText;
    public TextMeshProUGUI gameName;
    public TextMeshProUGUI tableLimitOrValue;
    public GameObject tableSelectionScreen;
    public Button joinButton;
    public GameObject lowBalancePopup;

    [Header("--- Prefab ---")]
    public GameObject prefabParent;
    public GameObject editProfilePrefab;
    public GameObject settingPrefab;
    public GameObject withdrawPrefab;
    public GameObject shopScreenPrefab;
    public GameObject contactUsPrefab;
    public GameObject tourErrorPrefab;
    public GameObject ludoLoadingPrefab;
    public GameObject tranHistoryPrefab;
    public GameObject refferalPrefab;
    public GameObject withdrawErrorPrefab;
    public GameObject snakeLoadingPrefab;
    public GameObject scratchCardPrefab;
    public GameObject dailyRewardPrefab;

    public GameObject noplayersOnlinePrefab;
    private GameObject shopScreenInstance;
    public GameObject leaderboardPanel;

    [Header("--- Tournament ---")]
    public GameObject snakeTournamentLoadPanel;
    public bool isPressJoin = false;
    public GameObject ludoModeSelect;
    public Button twoPlayerButton;
    public Button fourPlayerButton;
    private float selectedValue;

    //public List<TournamentData> tournamentData = new List<TournamentData>();
    public List<CouponData> couponDatas = new List<CouponData>();

    //public GameObject settingUpdateObj;
    public int minPlayerRequired = 5;


    public float secondsCount = 10f;
    public Text timeTxt;
    public bool rummyjoinButtonClicked = false;
    public GameObject timerObject;

    [Header("---Screen---")]
    public List<GameObject> screenObj = new List<GameObject>();

    [Header("--- Daily Reward ---")]
    public GameObject dailyRewardPanel;
    public GameObject spinWheelScreenOpen;
    public GameObject spinDialogObj;
    public GameObject popup;
    public Text timeRemainingText;
    private DateTime lastSpinTime;
    private TimeSpan timeUntilNextSpin;
    public int numberOfTurns;

    [Header("--- Banner ---")]
    public GameObject fullScreenAd;
    private List<FullScreenAd> fullScreenAds = new List<FullScreenAd>();
    private int closedBannerCount = 0;

    [Header("---Notification List Panel---")]
    public GameObject notificationListPanel;
    public GameObject notificationRedDot;

    [Header("---Withdraw---")]
    public bool isWithdraw;

    public List<NotiBarManage> notiBarManages = new List<NotiBarManage>();

    public int botPlayers;

    public bool ludoBotPlayersLoaded = false;

    //bool isPressJoin;



    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Time.timeScale = 1;
        if (Instance == null)
        {
            Instance = this;
        }


    }
    // Start is called before the first frame update
    [Obsolete("Obsolete")]
    void Start()
    {
        //GetVersionUpdate();
        UpdateAllData();
        //GetTournament();
        //GetTran();
        GetCoupon();

        DataManager.Instance.GetTournament();
        GetFullScreenBanner();
        StartCoroutine(InstantiateDailyRewardWithDelay(5f));

    }


    public void UpdateAllData()
    {
        Getdata();
        //Getnotification();
    }
    // Update is called once per frame
    private void Update()
    {
        if (rummyjoinButtonClicked)
        {
            if (secondsCount > 0)
            {
                secondsCount -= Time.deltaTime;
                float seconds = secondsCount % 60;
                if (seconds.ToString("0").Length == 1)
                {
                    timeTxt.text = "Starting....." + "0" + seconds.ToString("0");
                }
                else
                {
                    timeTxt.text = "Starting....." + seconds.ToString("0");
                }
            }
            else
            {
                TestSocketIO.Instace.rummyJoinRoomTimeOver = true;
                rummyjoinButtonClicked = false;
                TestSocketIO.Instace.StartGameData();

                TestSocketIO.Instace.RummyLoadScene();
            }
        }

        if (DataManager.Instance.gameMode == GameType.Ludo && secondsCount <= 3 && !ludoBotPlayersLoaded)
        {
            LoadLudoBotPlayers();
            ludoBotPlayersLoaded = true;
        }


        if (DataManager.Instance.tournamentData != null || DataManager.Instance.tournamentData.Count != 0) return;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Splash");

    }

    #region Home

    public void ProfileButtonClick()
    {
        //SoundManager.Instance.ButtonClick();
        GenerateEditProfile();
    }

    public void SettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateSetting();
    }

    public void CustomerButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //Application.OpenURL("mailto: " + "support@teenpattiblack.com" + " ? subject = " + "subject" + " & body = " + "body");
        string url = "https://wa.link/hyrndh";
        Application.OpenURL(url);
        //GenerateContactUs();
    }

    public void MailButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateNotification();
    }

    public void OpenLeaderBoard()
    {
        SoundManager.Instance.ButtonClick();
        leaderboardPanel.gameObject.SetActive(true);
    }

    public void VIPButtonClick()
    {

    }

    public void GameButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        switch (no)
        {
            case 1:
                {
                    // Roulette
                    DataManager.Instance.gameMode = GameType.Roulette;
                    string getTour = IsAvaliableSingleTournament(GameType.Roulette);
                    if (!string.IsNullOrEmpty(getTour))
                    {
                        DataManager.Instance.tournamentID = getTour;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                    {
                        GenerateTournamentError();
                    }

                    break;
                }
            case 2:
                {
                    DataManager.Instance.gameMode = GameType.Andar_Bahar;
                    // Andar Bahar
                    string getTour = IsAvaliableSingleTournament(GameType.Andar_Bahar);
                    if (!string.IsNullOrEmpty(getTour))
                    {
                        DataManager.Instance.tournamentID = getTour;
                        TestSocketIO.Instace.AndarBaharJoinroom();
                    }

                    //SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                    //string getTour = IsAvaliableSingleTournament(GameType.Roulette);
                    //if (getTour != null && getTour.Length != 0)
                    //{
                    //    DataManager.Instance.tournamentID = getTour;
                    //    TestSocketIO.Instace.RouletteJoinroom();
                    //}
                    //else
                    //{
                    //    GenerateTournamentError();
                    //}
                    break;
                }
            case 3:
                {
                    // Dragon Tiger
                    DataManager.Instance.gameMode = GameType.Dragon_Tiger;
                    string getTour = IsAvaliableSingleTournament(GameType.Dragon_Tiger);
                    if (!string.IsNullOrEmpty(getTour))
                    {
                        DataManager.Instance.tournamentID = getTour;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                    {
                        GenerateTournamentError();
                    }

                    break;
                }
            case 4:
                //DataManager.Instance.gameMode = GameType.Ludo;

                //TournamentPanel.Instance.gameType = GameType.Ludo;
                //TournamentPanel.Instance.GenerateTournament();
                TurnOnLudoSelector();


                //string getTour = IsAvaliableSingleTournament(GameType.Ludo);
                //if (getTour != null && getTour.Length != 0)
                //{
                //    DataManager.Instance.tournamentID = getTour;//AA

                //    Screen.orientation = ScreenOrientation.Portrait;
                //    TestSocketIO.Instace.playTime = 2f;
                //    DataManager.Instance.playerNo = 0;
                //    DataManager.Instance.diceManageCnt = 0;
                //    DataManager.Instance.tourEntryMoney = 0;
                //    DataManager.Instance.winAmount = 0;
                //    //DataManager.Instance.tourBon
                //    TestSocketIO.Instace.LudoJoinroom();
                //}
                break;
            case 5:
                {
                    // Teen Patti
                    DataManager.Instance.gameMode = GameType.Teen_Patti;
                    string getTour = IsAvaliableSingleTournament(GameType.Teen_Patti);
                    gameName.text = "";


                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                        Debug.Log("Amount   =? " + item.betAmount);
                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Teen_Patti, minBetValues);
                    else
                        GenerateTournamentError();

                    //if (!string.IsNullOrEmpty(getTour))
                    //{
                    //    //DataManager.Instance.tournamentID = getTour;
                    //    //timerObject.SetActive(true);
                    //    //rummyjoinButtonClicked = true;
                    //    //TestSocketIO.Instace.TeenPattiJoinroom();
                    //}
                    //else
                    //{
                    //    GenerateTournamentError();
                    //}

                    //Instantiate(tournamentTeenPattiPrefab, prefabParent.transform);
                    //TournamentPanel.Instance.gameType = GameType.Teen_Patti;
                    //TournamentPanel.Instance.GenerateTournament();
                    break;
                }
            case 6:
                {
                    // Poker
                    DataManager.Instance.gameMode = GameType.Poker;
                    //SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                    string getTour = IsAvaliableSingleTournament(GameType.Poker);
                    gameName.text = "";

                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Poker, minBetValues);
                    else
                        GenerateTournamentError();

                    break;
                }
            /*case 7:
            {
                // Snake & Ladder
                DataManager.Instance.gameMode = GameType.Snake;
                //SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                string getTour = IsAvaliableSnakeTournament(GameType.Snake);
                if (!string.IsNullOrEmpty(getTour))
                {
                    Instantiate(tournamentLudoPrefab, prefabParent.transform);
                    TournamentPanel.Instance.gameType = GameType.Snake;
                    TournamentPanel.Instance.GenerateTournament();
                    /*DataManager.Instance.tournamentID = getTour;
                    TestSocketIO.Instace.SnakeJoinRoom();
                    DataManager.Instance.tournamentID = tourData._id;
                    DataManager.Instance.tourEntryMoney = tourData.betAmount * 10;
                    TestSocketIO.Instace.playTime = tourData.time;

                    BotManager.Instance.isBotAvalible = tourData.bot;
                    print(tourData.bot);
                    int complex = tourData.complexity;
                    //print("Cnt Move : " + index + " Complex : " + complex);

                    BotManager.Instance.botType = complex switch
                    {
                        1 => BotType.Easy,
                        2 => BotType.Medium,
                        3 => BotType.Hard,
                        _ => BotManager.Instance.botType
                    };
                    Instantiate(snakeLoadingPrefab, prefabParent.transform);#1#
                }
                else
                {
                    GenerateTournamentError();
                }

                break;
            }*/
            case 8:
                {
                    // Car Roulette
                    DataManager.Instance.gameMode = GameType.Car_Roulette;
                    string getTour = IsAvaliableSingleTournament(GameType.Car_Roulette);
                    if (!string.IsNullOrEmpty(getTour))
                    {
                        DataManager.Instance.tournamentID = getTour;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                    {
                        GenerateTournamentError();
                    }

                    break;
                }
            case 9:
                {
                    //7 Up Down
                    DataManager.Instance.gameMode = GameType.SevenUpDown;
                    string getTournamentID = IsAvaliableSingleTournament(GameType.SevenUpDown);
                    if (!string.IsNullOrEmpty(getTournamentID))
                    {
                        DataManager.Instance.tournamentID = getTournamentID;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                        GenerateTournamentError();
                }
                break;
            case 10:
                {
                    //Joker
                    DataManager.Instance.gameMode = GameType.Joker;
                    string getTournamentID = IsAvaliableSingleTournament(GameType.Joker);

                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Joker, minBetValues);
                    else
                        GenerateTournamentError();
                    //if (!string.IsNullOrEmpty(getTournamentID))
                    //{
                    //    DataManager.Instance.tournamentID = getTournamentID;
                    //    timerObject.SetActive(true);
                    //    rummyjoinButtonClicked = true;
                    //    TestSocketIO.Instace.TeenPattiJoinroom();
                    //}
                    //else
                    //    GenerateTournamentError();
                }
                break;
            case 11:
                {
                    //AK47
                    DataManager.Instance.gameMode = GameType.Teen_Patti_AK47;
                    string getTournamentID = IsAvaliableSingleTournament(GameType.Teen_Patti_AK47);
                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Teen_Patti_AK47, minBetValues);
                    else
                        GenerateTournamentError();

                    //if (!string.IsNullOrEmpty(getTournamentID))
                    //{
                    //    DataManager.Instance.tournamentID = getTournamentID;
                    //    timerObject.SetActive(true);
                    //    rummyjoinButtonClicked = true;
                    //    TestSocketIO.Instace.TeenPattiJoinroom();
                    //}
                    //else
                    //    GenerateTournamentError();
                }
                break;
            case 12:
                {
                    //Point Rummy
                    DataManager.Instance.gameMode = GameType.Point_Rummy;
                    string getTournamentID = IsAvaliableSingleTournament(GameType.Point_Rummy);
                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                        Debug.Log("Point Amount   =? " + item.betAmount);

                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Point_Rummy, minBetValues);
                    else
                        GenerateTournamentError();

                    //if (!string.IsNullOrEmpty(getTournamentID))
                    //{
                    //    DataManager.Instance.pointValue = SetBetAmount(GameType.Point_Rummy);
                    //    DataManager.Instance.tournamentID = getTournamentID;
                    //    timerObject.SetActive(true);
                    //    rummyjoinButtonClicked = true;
                    //    TestSocketIO.Instace.RummyJoinRoom();
                    //}
                    //else
                    //    GenerateTournamentError();
                }
                break;
            case 13:
                {
                    //Pool Rummy
                    DataManager.Instance.gameMode = GameType.Pool_Rummy;
                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Pool_Rummy, minBetValues);
                    else
                        GenerateTournamentError();
                    //string getTournamentID = IsAvaliableSingleTournament(GameType.Pool_Rummy);
                    //if (!string.IsNullOrEmpty(getTournamentID))
                    //{
                    //    DataManager.Instance.tourEntryMoney = SetBetAmount(GameType.Pool_Rummy);
                    //    DataManager.Instance.tournamentID = getTournamentID;
                    //    timerObject.SetActive(true);
                    //    rummyjoinButtonClicked = true;
                    //    TestSocketIO.Instace.RummyJoinRoom();
                    //}
                    //else
                    //    GenerateTournamentError();
                }
                break;
            case 14:
                {
                    //Deal Rummy
                    DataManager.Instance.gameMode = GameType.Deal_Rummy;
                    List<float> minBetValues = new List<float>();
                    foreach (var item in DataManager.Instance.tournamentData)
                    {
                        if (item.modeType == DataManager.Instance.gameMode)
                            minBetValues.Add(item.betAmount);
                    }
                    if (minBetValues.Count > 0)
                        SelectValueOfTournament(GameType.Deal_Rummy, minBetValues);
                    else
                        GenerateTournamentError();
                    //string getTournamentID = IsAvaliableSingleTournament(GameType.Deal_Rummy);
                    //if (!string.IsNullOrEmpty(getTournamentID))
                    //{
                    //    DataManager.Instance.tourEntryMoney = SetBetAmount(GameType.Deal_Rummy);
                    //    DataManager.Instance.tournamentID = getTournamentID;
                    //    timerObject.SetActive(true);
                    //    rummyjoinButtonClicked = true;
                    //    TestSocketIO.Instace.RummyJoinRoom();
                    //}
                    //else
                    //    GenerateTournamentError();
                }
                break;
            case 15:
                {
                    //Jhandi Munda
                    DataManager.Instance.gameMode = GameType.Jhandi_Munda;
                    string getTournamentID = IsAvaliableSingleTournament(GameType.Jhandi_Munda);
                    if (!string.IsNullOrEmpty(getTournamentID))
                    {
                        DataManager.Instance.tournamentID = getTournamentID;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                        GenerateTournamentError();
                }
                break;
            case 16:
                {
                    //Aviator
                    DataManager.Instance.gameMode = GameType.Aviator;
                    string getTournamentID = IsAvaliableSingleTournament(GameType.Aviator);
                    if (!string.IsNullOrEmpty(getTournamentID))
                    {
                        DataManager.Instance.tournamentID = getTournamentID;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                        GenerateTournamentError();
                }
                break;
            case 17:
                {
                    // New Spin And Win
                    Debug.Log("SpinAndWin");
                    DataManager.Instance.gameMode = GameType.SpinAndWin;
                    string getTour = IsAvaliableSingleTournament(GameType.SpinAndWin);
                    if (!string.IsNullOrEmpty(getTour))
                    {
                        DataManager.Instance.tournamentID = getTour;
                        TestSocketIO.Instace.RouletteJoinroom();
                    }
                    else
                    {
                        GenerateTournamentError();
                    }

                    break;
                }

        }
    }

    public void BackButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void PopupShop()
    {
        CloseLowBalancePopup();
        CoinButtonClick();
    }

    public void CloseLowBalancePopup()
    {
        SoundManager.Instance.ButtonClick();
        lowBalancePopup.gameObject.SetActive(false);
    }

    private void GenerateLowBalanceError()
    {
        lowBalancePopup.gameObject.SetActive(true);
    }

    public void JoinButtonClick()
    {
        /*if (!string.IsNullOrEmpty(DataManager.Instance.tournamentID))
        {
            //DataManager.Instance.tournamentID = getTour;
            timerObject.SetActive(true);
            rummyjoinButtonClicked = true;

            //TestSocketIO.Instace.TeenPattiJoinroom();
            switch (DataManager.Instance.gameMode)
            {
                case GameType.Teen_Patti or GameType.Teen_Patti_AK47 or GameType.Joker or GameType.Poker:
                    TestSocketIO.Instace.TeenPattiJoinroom();
                    break;
                case GameType.Point_Rummy or GameType.Pool_Rummy or GameType.Deal_Rummy:
                    TestSocketIO.Instace.RummyJoinRoom();
                    break;
                case GameType.Ludo:
                    TestSocketIO.Instace.LudoJoinroom();
                    break;
                case GameType.None:
                    GenerateTournamentError();
                    break;
            }
        }
        else
            GenerateTournamentError();*/

        Debug.Log(" selectedValue  => " + selectedValue);
        SetTableLimitAndData(selectedValue, DataManager.Instance.gameMode);

        if (string.IsNullOrEmpty(DataManager.Instance.tournamentID))
        {
            GenerateTournamentError();
            ClearTableLimitAndData();
            return;
        }

        if (!float.TryParse(DataManager.Instance.playerData.balance, out float playerBalance))
        {
            // Handle invalid balance format
            print("Invalid Formate !");
            GenerateTournamentError();
            ClearTableLimitAndData();
            return;
        }

        if (playerBalance < selectedValue)
        {
            GenerateLowBalanceError();
            ClearTableLimitAndData();
            return;
        }

        timerObject.SetActive(true);
        rummyjoinButtonClicked = true;

        switch (DataManager.Instance.gameMode)
        {
            case GameType.Teen_Patti:
            case GameType.Teen_Patti_AK47:
            case GameType.Joker:
            case GameType.Poker:
                TestSocketIO.Instace.TeenPattiJoinroom();
                break;
            case GameType.Point_Rummy:
            case GameType.Pool_Rummy:
            case GameType.Deal_Rummy:
                TestSocketIO.Instace.RummyJoinRoom();
                break;
            case GameType.Ludo:
                TestSocketIO.Instace.LudoJoinroom();
                break;
            case GameType.None:
                GenerateTournamentError();
                break;
        }
    }
    public int divider;
    /*private void SelectValueOfTournament(GameType modeType, List<float> minimumBetOrEntryFeesOrPointValue)
    {
        tableSelectionScreen.SetActive(true);
        foreach (var item in entryValues)//clearing all the values for current gamemode
            item.text = "";

        switch (modeType)
        {
            case GameType.Rummy:
                gameName.text = "POINT RUMMY";
                break;
            case GameType.Teen_Patti:
                gameName.text = "TEENPATTI - CLASSIC";
                break;
            
            case GameType.Teen_Patti_AK47:
                gameName.text = "TEENPATTI - AK-47";
                break;
            case GameType.Point_Rummy:
                gameName.text = "POINT RUMMY";
                break;
            case GameType.Joker:
                gameName.text = "TEENPATTI - JOKER";
                break;
            case GameType.Pool_Rummy:
                gameName.text = "POOL RUMMY";
                break;
            case GameType.Deal_Rummy:
                gameName.text = "DEAL RUMMY";
                break;
            case GameType.Poker:
                gameName.text = "POKER";
                break;
            case GameType.Ludo:
                gameName.text = "LUDO";
                break;
        }
        tableSet.maxValue = minimumBetOrEntryFeesOrPointValue.Count - 1;
        float _initialValue = -740f;//-30f;
        int _divisionParts = 1400 / (minimumBetOrEntryFeesOrPointValue.Count - 1);

        print("position = " + referenceText.transform.position + " localPosition = " + referenceText.transform.localPosition);

        for (int i = 0; i < minimumBetOrEntryFeesOrPointValue.Count; i++)
        {
            Text text = Instantiate(referenceText, tableSet.transform);
            //entryValues.Add(text);
            //entryValues[i].text = minimumBetOrEntryFeesOrPointValue[i].ToString();
            //entryValues[i].transform.localPosition = new Vector2(_initialValue + (i * _divisionParts), -75f);
            
            text.text = minimumBetOrEntryFeesOrPointValue[i].ToString();
            text.transform.localPosition = new Vector2(_initialValue + (i * _divisionParts), -75f);
        }

        //for (int i = 0; i < minimumBetOrEntryFeesOrPointValue.Count; i++)
        //    entryValues[i].text = minimumBetOrEntryFeesOrPointValue[i].ToString();
        tableLimitOrValue.text = "SELECT TABLE : ₹" + minimumBetOrEntryFeesOrPointValue[0].ToString();
        DataManager.Instance.tournamentID = DataManager.Instance.tournamentData.Find(x => x.modeType == modeType && x.betAmount == minimumBetOrEntryFeesOrPointValue[0])._id;
        DataManager.Instance.betPrice = minimumBetOrEntryFeesOrPointValue[0];
        tableSet.onValueChanged.AddListener(v =>
        {
            for (int i = 0; i < minimumBetOrEntryFeesOrPointValue.Count; i++)
            {
                if (v == i)
                {
                    tableLimitOrValue.text = "SELECT TABLE : ₹" + minimumBetOrEntryFeesOrPointValue[i].ToString();
                    joinButton.interactable = true;
                    DataManager.Instance.tournamentID = DataManager.Instance.tournamentData.Find(x => x.modeType == modeType && x.betAmount == minimumBetOrEntryFeesOrPointValue[i])._id;
                    DataManager.Instance.betPrice = minimumBetOrEntryFeesOrPointValue[i];
                    break;
                }
            }
            if(v >= minimumBetOrEntryFeesOrPointValue.Count)
            {
                tableLimitOrValue.text = "SELECT TABLE : NO TABLE SELECTED";
                joinButton.interactable = false;
                DataManager.Instance.betPrice = 0;
                DataManager.Instance.tournamentID = "";
            }
        });
    }*/

    private void SelectValueOfTournament(GameType modeType, List<float> minimumBetOrEntryFeesOrPointValue)
    {
        for (int i = 0; i < minimumBetOrEntryFeesOrPointValue.Count; i++)
        {

            Debug.Log("VAL = "+minimumBetOrEntryFeesOrPointValue[i]);
        }
        tableSelectionScreen.SetActive(true);
        ClearEntryValues();

        SetGameName(modeType);

        var tableCount = minimumBetOrEntryFeesOrPointValue.Count;
        tableSet.maxValue = tableCount - 1;
        var initialValueX = -740f;
        var divisionParts = 1400 / (tableCount - 1);


        for (var i = 0; i < tableCount; i++)
        {
            var text = Instantiate(referenceText, tableSet.transform);
            text.text = minimumBetOrEntryFeesOrPointValue[i].ToString();
            text.transform.localPosition = new Vector2(initialValueX + i * divisionParts, -75f);
        }

        // Set default value to the 0th index
        selectedValue = minimumBetOrEntryFeesOrPointValue[0];
        tableLimitOrValue.text = $"SELECT TABLE : ₹{selectedValue}";
        joinButton.interactable = true;

        tableSet.onValueChanged.AddListener(delegate (float v)
        {
            int index = (int)v;
            selectedValue = minimumBetOrEntryFeesOrPointValue[index];
            tableLimitOrValue.text = $"SELECT TABLE : ₹{selectedValue}";
            joinButton.interactable = true;
        });
    }

    private void SetGameName(GameType modeType)
    {
        gameName.text = modeType switch
        {
            GameType.Rummy => "POINT RUMMY",
            GameType.Teen_Patti => "TEENPATTI - CLASSIC",
            GameType.Teen_Patti_AK47 => "TEENPATTI - AK-47",
            GameType.Point_Rummy => "POINT RUMMY",
            GameType.Joker => "TEENPATTI - JOKER",
            GameType.Pool_Rummy => "POOL RUMMY",
            GameType.Deal_Rummy => "DEAL RUMMY",
            GameType.Poker => "POKER",
            GameType.Ludo => "LUDO",
            _ => ""
        };
    }

    private void ClearEntryValues()
    {
        foreach (var item in entryValues)
            item.text = "";
    }

    private void SetTableLimitAndData(float selectedValue, GameType modeType)
    {
        var tournamentData = DataManager.Instance.tournamentData.Find(x => x.modeType == modeType && x.betAmount == selectedValue);
        DataManager.Instance.tournamentID = tournamentData._id;
        DataManager.Instance.betPrice = selectedValue;
        TestSocketIO.Instace.playTime = tournamentData.time;
        DataManager.Instance.tourEntryMoney = tournamentData.betAmount;
        DataManager.Instance.tourBonusCut = tournamentData.bonusAmountDeduction;
        BotManager.Instance.isBotAvalible = tournamentData.bot;

        int complexity = tournamentData.complexity;
        BotManager.Instance.botType = complexity switch
        {
            1 => BotType.Easy,
            2 => BotType.Medium,
            3 => BotType.Hard,
            _ => BotManager.Instance.botType
        };
    }

    private void ClearTableLimitAndData()
    {
        DataManager.Instance.tournamentID = string.Empty;
        DataManager.Instance.betPrice = 0f;
        TestSocketIO.Instace.playTime = 0;
        DataManager.Instance.tourEntryMoney = 0f;
        DataManager.Instance.tourBonusCut = 0f;
        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.botType = BotType.Hard;
    }

    float SetBetAmount(GameType modeType)
    {
        foreach (var item in DataManager.Instance.tournamentData)
        {
            if (item.modeType == modeType)
                return item.betAmount;
        }
        return 0;
    }

    string IsAvaliableSingleTournament(GameType modeType)
    {
        for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
        {
            if (DataManager.Instance.tournamentData[i].modeType == modeType)
            {
                DataManager.Instance.gameComplexity = DataManager.Instance.tournamentData[i].complexity;
                return DataManager.Instance.tournamentData[i]._id;
            }
        }
        return null;
    }


    string IsAvaliableSnakeTournament(GameType modeType)
    {
        List<TournamentData> tournaments = new List<TournamentData>();

        for (int i = 0; i < DataManager.Instance.tournamentData.Count; i++)
        {
            if (DataManager.Instance.tournamentData[i].modeType != modeType) continue;
            tournaments.Add(DataManager.Instance.tournamentData[i]);
            tourData = DataManager.Instance.tournamentData[i];
            return DataManager.Instance.tournamentData[i]._id;

        }

        return null;
    }

    private void TurnOnLudoSelector()
    {
        ludoModeSelect.gameObject.SetActive(true);
        ludoBotPlayersLoaded = false;
        DataManager.Instance.isTwoPlayer = false;
        DataManager.Instance.isFourPlayer = false;
        DataManager.Instance.modeType = 1;
        LudoGameMode(true);
    }

    public void TurnOffLudoSelector()
    {
        ludoModeSelect.gameObject.SetActive(false);
    }

    public void LudoGameMode(bool isTwoPlayer)
    {
        if (isTwoPlayer)
        {
            DataManager.Instance.isTwoPlayer = true;
            DataManager.Instance.isFourPlayer = false;
            twoPlayerButton.image.enabled = true;
            fourPlayerButton.image.enabled = false;
        }
        else
        {
            DataManager.Instance.isTwoPlayer = false;
            DataManager.Instance.isFourPlayer = true;
            twoPlayerButton.image.enabled = false;
            fourPlayerButton.image.enabled = true;
        }
    }


    public void LudoContinueButtonClick()
    {
        TurnOffLudoSelector();
        DataManager.Instance.gameMode = GameType.Ludo;
        string getTour = IsAvaliableSingleTournament(GameType.Ludo);
        gameName.text = "";


        List<float> minBetValues = new List<float>();
        foreach (var item in DataManager.Instance.tournamentData)
        {
            if (item.modeType == DataManager.Instance.gameMode)
                minBetValues.Add(item.betAmount);
        }
        if (minBetValues.Count > 0)
            SelectValueOfTournament(GameType.Ludo, minBetValues);
        else
            GenerateTournamentError();
    }




    public void ShopButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateShop();
    }

    public void SafeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        print("Balance : " + float.Parse(DataManager.Instance.playerData.balance));
        if (float.Parse(DataManager.Instance.playerData.balance) < 100)
        {
            GenerateWithdrawErrorPanel();
        }
        else
        {
            GenerateWithdraw();
        }
    }

    public void RankButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateHistroy();
    }

    public void ShareButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //string shareTxt = "Download Latest StarX apk from Link Here : \n\n" + DataManager.Instance.appUrl + " \n\nUse this referral code :" + DataManager.Instance.playerData.refer_code;

        //new NativeShare().Share(shareTxt);
        GenerateRefferalPanel();
    }

    public void NoticeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateNotification();
    }

    public void CoinButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        GenerateShop();
    }




    #endregion

    #region Prefab

    public void GenerateSetting()
    {
        Instantiate(settingPrefab, prefabParent.transform);
    }

    public void GenerateWithdraw()
    {
        Instantiate(withdrawPrefab, prefabParent.transform);
    }

    public void GenerateEditProfile()
    {
        Instantiate(editProfilePrefab, prefabParent.transform);
    }

    public void GenerateShop()
    {
        shopScreenInstance = Instantiate(shopScreenPrefab, prefabParent.transform);
    }

    public void DestroyShop()
    {
        if (shopScreenInstance != null)
        {
            Destroy(shopScreenInstance);
        }
    }

    public void GenerateNotification()
    {
        Instantiate(notificationListPanel, prefabParent.transform);
    }

    public void GenerateContactUs()
    {
        Instantiate(contactUsPrefab, prefabParent.transform);
    }

    public void GenerateTournamentError()
    {
        Instantiate(tourErrorPrefab, prefabParent.transform);
    }

    public void GenerateNoPlayersFound()
    {
        timerObject.SetActive(false);
        Instantiate(noplayersOnlinePrefab, prefabParent.transform);
    }
    public void GenerateHistroy()
    {
        Instantiate(tranHistoryPrefab, prefabParent.transform);
    }

    public void GenerateLoadingPanel()
    {
        Instantiate(ludoLoadingPrefab, prefabParent.transform);

    }

    public void GenerateRefferalPanel()
    {
        Instantiate(refferalPrefab, prefabParent.transform);

    }

    public void GenerateWithdrawErrorPanel()
    {
        Instantiate(withdrawErrorPrefab, prefabParent.transform);

    }
    #endregion

    //#region API Calling

    #region DailySpin

    public void GenerateSpinDialogPrefab(int wonAmount)
    {
        GameObject dialog = Instantiate(spinDialogObj, prefabParent.transform);
        SpinDialogPanel spin = dialog.GetComponent<SpinDialogPanel>();
        spin.earnAmount = wonAmount;
        spin.DisplayText();
    }

    public void OpenSpinWheelScreen()
    {
        string lastSpinTimeStr = PlayerPrefs.GetString("LastSpinTime");
        int remainingTurns = PlayerPrefs.GetInt("RemainingTurns", numberOfTurns);

        if (string.IsNullOrEmpty(lastSpinTimeStr) || DateTime.TryParse(lastSpinTimeStr, out lastSpinTime))
        {
            if (string.IsNullOrEmpty(lastSpinTimeStr) || DateTime.Now >= lastSpinTime.AddHours(24))
            {
                lastSpinTime = DateTime.Now;
                PlayerPrefs.SetString("LastSpinTime", lastSpinTime.ToString());
                remainingTurns = numberOfTurns; // Reset the number of turns
                PlayerPrefs.SetInt("RemainingTurns", remainingTurns);
            }

            if (remainingTurns > 0)
            {
                CalculateTimeUntilNextSpin();
                ShowSpinWheelScreen();
            }
            else
            {
                ShowPopupAndTimeRemaining();
            }
        }
        else
        {
            lastSpinTime = DateTime.Now;
            PlayerPrefs.SetString("LastSpinTime", lastSpinTime.ToString());
            remainingTurns = numberOfTurns; // Set the initial number of turns
            PlayerPrefs.SetInt("RemainingTurns", remainingTurns);
            CalculateTimeUntilNextSpin();
            ShowSpinWheelScreen();
        }
    }


    private void ShowSpinWheelScreen()
    {
        dailyRewardPanel.SetActive(true);
        SoundManager.Instance.StopBackgroundMusic();
        spinWheelScreenOpen.SetActive(true);
    }

    private void ShowPopupAndTimeRemaining()
    {
        popup.SetActive(true);
        CalculateTimeUntilNextSpin();
        timeRemainingText.text = string.Format("Next spin unlocks in {0:D2}:{1:D2}",
            timeUntilNextSpin.Hours, timeUntilNextSpin.Minutes);
    }

    public void ClearLastSpinTime()
    {
        PlayerPrefs.DeleteKey("LastSpinTime");
        PlayerPrefs.DeleteKey("RemainingTurns");
        PlayerPrefs.DeleteKey("LastRewardClaimDate");
    }

    private void CalculateTimeUntilNextSpin()
    {
        // Calculate the time remaining until the next spin becomes available
        DateTime nextSpinTime = lastSpinTime.AddHours(24);
        timeUntilNextSpin = nextSpinTime - DateTime.Now;
    }


    public void CloseSpinnerWheel()
    {
        dailyRewardPanel.SetActive(false);
        SoundManager.Instance.StartBackgroundMusic();
        spinWheelScreenOpen.SetActive(false);
    }


    #endregion

    #region Banner

    private void GetFullScreenBanner()
    {
        if (DataManager.Instance.IsOneTimeOpen) return;
        StartCoroutine(GetBanners());
        DataManager.Instance.IsOneTimeOpen = true;
    }
    IEnumerator GetBanners()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/banners");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Banner : " + request.downloadHandler.text);

            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(values["data"].ToString());

            if (data.Count == 0)
            {
                print("There are no ads");
                // No banners available, check for scratch card availability immediately
                CheckForTheAvailability();
                yield break;
            }

            int no = 0;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i]["status"] != "active" || data[i]["location"] != "HOME" || data[i]["bannerType"] != "fullscreen") continue;
                {
                    GameObject fullScreenObj = Instantiate(fullScreenAd, prefabParent.transform);
                    string url = data[i]["url"];
                    no++;
                    FullScreenAd fullscreen = fullScreenObj.GetComponent<FullScreenAd>();

                    fullScreenAds.Add(fullscreen);
                    fullscreen.OnBannerClosed += OnBannerClosed;

                    Image bannerImage = fullScreenObj.GetComponent<Image>();
                    bannerImage.color = new Color(bannerImage.color.r, bannerImage.color.g, bannerImage.color.b, 0f);

                    fullScreenObj.AddComponent<Button>().onClick.AddListener(() => BannerClick(url));

                    StartCoroutine(GetImages(data[i]["imageUrl"].ToString().Trim('"'), fullscreen.bannerImage));

                    fullScreenObj.transform.localScale = Vector3.zero;
                    fullScreenObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                        .OnComplete(() => bannerImage.DOFade(0.6f, 0.3f));
                }
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }


    void BannerClick(string bannerUrl)
    {
        //print("Banner URL : " + bannerUrl);
        if (bannerUrl != "" && bannerUrl != null)
        {
            Application.OpenURL(bannerUrl);
        }
    }

    IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            if (image != null)
            {
                image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
            }
        }
    }


    #endregion

    #region DailyScratchCard

    private void OnBannerClosed()
    {
        closedBannerCount++;

        // Check if all banners are closed
        if (closedBannerCount == fullScreenAds.Count)
        {
            // Unsubscribe from the OnBannerClosed event for all banners
            foreach (FullScreenAd ad in fullScreenAds)
            {
                ad.OnBannerClosed -= OnBannerClosed;
            }

            CheckForTheAvailability();
        }
    }

    private void CheckForTheAvailability()
    {
        if (IsDailyRewardAvailable())
            InstantiateScratchCardPrefab();
        else
            Debug.Log("Daily reward is not available today.");
    }

    private bool IsDailyRewardAvailable()
    {
        string lastRewardClaimDateString = PlayerPrefs.GetString("LastRewardClaimDate", string.Empty);

        if (string.IsNullOrEmpty(lastRewardClaimDateString))
        {
            // No previous reward claim date found, allow the reward
            return true;
        }

        DateTime lastRewardClaimDate;
        if (DateTime.TryParse(lastRewardClaimDateString, out lastRewardClaimDate))
        {
            DateTime currentDate = DateTime.Now.Date;

            if (lastRewardClaimDate < currentDate)
            {
                // Last reward claim was on a previous day, allow the reward
                return true;
            }
        }

        // Reward has already been claimed today or an invalid date format
        return false;
    }

    private void InstantiateScratchCardPrefab()
    {
        // Instantiate the scratch card prefab
        Instantiate(scratchCardPrefab, prefabParent.transform);

        SaveLastRewardClaimDate();
    }

    private void SaveLastRewardClaimDate()
    {
        // Update the last reward claim date to today
        PlayerPrefs.SetString("LastRewardClaimDate", DateTime.Now.Date.ToString());
        PlayerPrefs.Save();
    }

    #endregion

    #region Daily Reward

    private IEnumerator InstantiateDailyRewardWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        InstantiateDailyReward();
    }
    private void InstantiateDailyReward()
    {
        string lastClaimDateStr = PlayerPrefs.GetString("LastClaimDate", "");
        System.DateTime lastClaimDate = string.IsNullOrEmpty(lastClaimDateStr) ? System.DateTime.MinValue : System.DateTime.Parse(lastClaimDateStr);

        if (lastClaimDate.Date < System.DateTime.Now.Date)
        {
            Instantiate(dailyRewardPrefab, prefabParent.transform);
        }
    }

    public void ClearAllDailyRewardStatus()
    {
        PlayerPrefs.DeleteKey("CurrentDay");
        PlayerPrefs.DeleteKey("TotalEarnings");
        PlayerPrefs.DeleteKey("LastClaimDate");
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.DeleteKey("DailyRewardStatus" + i);
        }
        PlayerPrefs.Save();
        StartCoroutine(InstantiateDailyRewardWithDelay(2f));
    }

    #endregion


    #region Profile Data
    public void Getdata()
    {
        StartCoroutine(GetPlayerdata());
    }


    IEnumerator GetPlayerdata()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/profile");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Data:" + request.downloadHandler.text);
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(values["data"].ToString());
            if (values["success"] == false)
            {
                print("MY");
                DataManager.Instance.SetLoginValue("N");
                SceneManager.LoadScene("Splash");
                yield break;
            }
            Setplayerdata(data, true);
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }

    public void SavePlayerProfile()
    {
        StartCoroutine(Profiledatasave());
    }

    IEnumerator Profiledatasave()
    {
        WWWForm form = new WWWForm();
        form.AddField("firstName", DataManager.Instance.playerData.firstName);
        form.AddField("lastName", DataManager.Instance.playerData.lastName);
        form.AddField("gender", DataManager.Instance.playerData.gender);
        form.AddField("email", DataManager.Instance.playerData.email);
        form.AddField("state", DataManager.Instance.playerData.state);
        form.AddField("panNumber", DataManager.Instance.playerData.panNumber);
        form.AddField("aadharNumber", DataManager.Instance.playerData.aadharNumber);
        form.AddField("dob", DataManager.Instance.playerData.dob);
        form.AddField("country", DataManager.Instance.playerData.country);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());

            JSONNode datas = JSON.Parse(values["data"].ToString());
            //Debug.Log("User Data===:::" + datas.ToString());
            Setplayerdata(datas, false);
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }

    public void Setplayerdata(JSONNode data, bool isGet)
    {
        Debug.Log("User Data===:::" + data.ToString());

        if (isGet)
        {
            if (data[nameof(DataManager.Instance.playerData.balance)] == "")
            {
                data[nameof(DataManager.Instance.playerData.balance)] = "";
            }
            DataManager.Instance.playerData.balance = ((float)data[nameof(DataManager.Instance.playerData.balance)]).ToString("F2");
            DataManager.Instance.playerData.kycStatus = data[nameof(DataManager.Instance.playerData.kycStatus)];
            if (data[nameof(DataManager.Instance.playerData.wonCount)] == "")
            {
                data[nameof(DataManager.Instance.playerData.wonCount)] = "";
            }
            DataManager.Instance.playerData.wonCount = data[nameof(DataManager.Instance.playerData.wonCount)];
            if (data[nameof(DataManager.Instance.playerData.joinCount)] == "")
            {
                data[nameof(DataManager.Instance.playerData.joinCount)] = "";
            }
            DataManager.Instance.playerData.joinCount = data[nameof(DataManager.Instance.playerData.joinCount)];
            DataManager.Instance.playerData.deposit = (data[nameof(DataManager.Instance.playerData.deposit)]).ToString();
            DataManager.Instance.playerData.winings = (data[nameof(DataManager.Instance.playerData.winings)]).ToString();
            DataManager.Instance.playerData.bonus = (data[nameof(DataManager.Instance.playerData.bonus)]).ToString();
            DataManager.Instance.playerData._id = data[nameof(DataManager.Instance.playerData._id)];
            DataManager.Instance.playerData.phone = data[nameof(DataManager.Instance.playerData.phone)];
            DataManager.Instance.playerData.aadharNumber = data[nameof(DataManager.Instance.playerData.aadharNumber)];
            DataManager.Instance.playerData.refer_code = data[nameof(DataManager.Instance.playerData.refer_code)];
            DataManager.Instance.playerData.email = data[nameof(DataManager.Instance.playerData.email)];
            DataManager.Instance.playerData.firstName = data[nameof(DataManager.Instance.playerData.firstName)];
            DataManager.Instance.playerData.lastName = data[nameof(DataManager.Instance.playerData.lastName)];
            DataManager.Instance.playerData.gender = data[nameof(DataManager.Instance.playerData.gender)];
            DataManager.Instance.playerData.state = data[nameof(DataManager.Instance.playerData.state)];
            DataManager.Instance.playerData.createdAt = RemoveQuotes(data[nameof(DataManager.Instance.playerData.createdAt)].ToString());
            DataManager.Instance.playerData.countryCode = data[nameof(DataManager.Instance.playerData.countryCode)];

            string getName = data[nameof(DataManager.Instance.playerData.dob)];
            if (getName == "" || getName == null)
            {
                DataManager.Instance.playerData.dob = "none";
            }
            else
            {
                DataManager.Instance.playerData.dob = RemoveQuotes(data[nameof(DataManager.Instance.playerData.dob)]);
            }
            DataManager.Instance.playerData.panNumber = data[nameof(DataManager.Instance.playerData.panNumber)];
            DataManager.Instance.playerData.membership = "free";
            //DataManager.Instance.playerData.membership = data[nameof(DataManager.Instance.playerData.membership)];
            DataManager.Instance.playerData.avatar = DataManager.Instance.GetAvatarValue();
            DataManager.Instance.playerData.refer_count = data[nameof(DataManager.Instance.playerData.refer_count)];
            DataManager.Instance.playerData.refrer_level = data[nameof(DataManager.Instance.playerData.refrer_level)];
            DataManager.Instance.playerData.refrer_amount_total = data[nameof(DataManager.Instance.playerData.refrer_amount_total)];

            DataManager.Instance.playerData.refer_lvl1_count = data[nameof(DataManager.Instance.playerData.refer_lvl1_count)];
            DataManager.Instance.playerData.refer_vip_count = data[nameof(DataManager.Instance.playerData.refer_vip_count)];
            DataManager.Instance.playerData.refer_deposit_count = data[nameof(DataManager.Instance.playerData.refer_deposit_count)];
        }
        else
        {

            DataManager.Instance.playerData.email = data[nameof(DataManager.Instance.playerData.email)];
            DataManager.Instance.playerData.firstName = data[nameof(DataManager.Instance.playerData.firstName)];
            DataManager.Instance.playerData.lastName = data[nameof(DataManager.Instance.playerData.lastName)];
            DataManager.Instance.playerData.gender = data[nameof(DataManager.Instance.playerData.gender)];
            DataManager.Instance.playerData.panNumber = data[nameof(DataManager.Instance.playerData.panNumber)];
            DataManager.Instance.playerData.state = data[nameof(DataManager.Instance.playerData.state)];

            DataManager.Instance.playerData.aadharNumber = data[nameof(DataManager.Instance.playerData.aadharNumber)];
            DataManager.Instance.playerData.country = data[nameof(DataManager.Instance.playerData.country)];
            DataManager.Instance.playerData.dob = data[nameof(DataManager.Instance.playerData.dob)];
            DataManager.Instance.playerData.avatar = DataManager.Instance.GetAvatarValue();

            Getdata();

        }
        //print("Default Name : " + DataManager.Instance.GetDefaultPlayerName().Length);
        //print("Player Name : " + DataManager.Instance.playerData.firstName);
        coinTxt.text = DataManager.Instance.playerData.balance.ToString();
        bonusTxt.text = DataManager.Instance.playerData.bonus.ToString();
        if (DataManager.Instance.GetDefaultPlayerName().IsNullOrEmpty() && DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            print("Sub String : ");
            DataManager.Instance.SetDefaultPlayerName(DataManager.Instance.playerData.phone.Substring(0, 5));
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }
        else if (DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }
        UserUpdateDisplayData();
        //TopBarDataSet();

    }

    #endregion

    #region Notification
    // public void Getnotification()
    // {
    //     StartCoroutine(GetNotifications());
    // }
    //
    // IEnumerator GetNotifications()
    // {
    //     UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/notifications/player");
    //     request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
    //     yield return request.SendWebRequest();
    //
    //     if (request.error == null && !request.isNetworkError)
    //     {
    //         JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
    //         //print("Update Data : " + value.ToString())
    //         if (value.Count > 0)
    //         {
    //             notificationRedDot.SetActive(true);
    //         }
    //         else
    //         {
    //             notificationRedDot.SetActive(false);
    //         }
    //     }
    //
    // }


    /*
    #region Tournaments

    [Obsolete("Obsolete")]
    public void GetTournament()
    {
        StartCoroutine(GetTournaments());
    }
    [Obsolete("Obsolete")]
    IEnumerator GetTournaments()
    {
        //DataManager.Instance.tournamentData.Clear();
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/tournaments");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Tour Data : " + request.downloadHandler.text);

            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(values["data"].ToString());

            for (int i = 0; i < data.Count; i++)
            {
                TournamentData t = new TournamentData
                {
                    bot = data[i]["bot"],
                    bonusAmountDeduction = data[i]["bonusAmountDeduction"],
                    active = data[i]["active"],
                    _id = data[i]["_id"],
                    name = data[i]["name"]
                };

                t.modeType = int.Parse(data[i]["mode"]) switch
                {
                    1 => GameType.Teen_Patti,
                    2 => GameType.Dragon_Tiger,
                    3 => GameType.Roulette,
                    4 => GameType.Poker,
                    5 => GameType.Andar_Bahar,
                    6 => GameType.Ludo,
                    _ => t.modeType
                };
                t.betAmount = data[i]["betAmount"];
                t.minBet = data[i]["minBet"];
                t.maxBet = data[i]["maxBet"];
                t.maxPayout = data[i]["maxPayout"];
                t.challLimit = data[i]["challLimit"];
                t.potLimit = data[i]["potLimit"];
                t.players = int.Parse(data[i]["players"]);
                t.winner = int.Parse(data[i]["winner"]);

                float winAmount = 0;
                for (int j = 0; j < data[i]["winnerRow"].Count; j++)
                {
                    t.winnerRow.Add(data[i]["winnerRow"][j]);
                    winAmount += data[i]["winnerRow"][j];
                }

                t.totalWinAmount = winAmount;


                t.time = float.Parse(data[i]["time"]);
                t.complexity = int.Parse(data[i]["complexity"]);
                t.interval = int.Parse(data[i]["interval"]);
                t._v = data[i]["__v"];
                t.createdAt = data[i]["createdAt"];
                t.updatedAt = data[i]["updatedAt"];
                if (t.active)
                {
                    DataManager.Instance.tournamentData.Add(t);
                }
            }

        }
        else
        {
            if (request.error != null) Logger.log.Log(request.error.ToString());
        }

    }

    #endregion
    */


    #region Transaction
    public void GetTran()
    {
        StartCoroutine(GetTrans());
    }

    IEnumerator GetTrans()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/transactions/player");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        if (request.error == null && !request.isNetworkError)
        {
            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(keys["data"].ToString());

            JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
            if (value.Count > 0)
            {
                //isWithdraw = true;
            }
            else
            {
                isWithdraw = false;
            }

            for (int i = 0; i < value["data"].Count; i++)
            {

                //byte[] st = Encoding.ASCII.GetBytes(value["data"][i]["title"].ToString().Trim('"'));
                //string data = Encoding.UTF8.GetString(st);



                string paymentStatus = value["data"][i]["paymentStatus"];
                string logType = value["data"][i]["logType"];
                //string _id = value["data"][i]["_id"];
                //string amount = value["data"][i]["amount"];
                //string transactionType = value["data"][i]["transactionType"];
                //string note = value["data"][i]["note"];
                //string createdAt = value["data"][i]["createdAt"];



                if (logType == "withdraw" && paymentStatus == "PROCESSING")
                {
                    isWithdraw = true;
                }

            }
        }

        #endregion
    }

    #endregion

    #region Common

    public void UserUpdateDisplayData()
    {

        if (DataManager.Instance.playerData.firstName.IsNullOrEmpty())
        {
            userNameTxt.text = DataManager.Instance.GetDefaultPlayerName();
        }
        else
        {
            userNameTxt.text = DataManager.Instance.playerData.firstName;
        }
        if (DataManager.Instance.playerData.email.Length > 14)
        {
            userIdTxt.text = DataManager.Instance.playerData.email.Substring(0, 14) + "...";
        }
        else
        {
            userIdTxt.text = DataManager.Instance.playerData.email;
        }

        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
        LoadProfileImage();
    }

    public void LoadProfileImage()
    {
        Sprite selectedAvatarSprite = DataManager.Instance.GetSelectedAvatarSprite();

        if (selectedAvatarSprite != null)
        {
            avatarImg.sprite = selectedAvatarSprite;
        }
        else
        {
            // Handle the case when no avatar sprite is selected
            StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
        }
    }

    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;

    }
    #endregion

    #region Load Bot

    public void CheckPlayers()
    {
        botPlayers = minPlayerRequired - DataManager.Instance.joinPlayerDatas.Count;
        if (DataManager.Instance.joinPlayerDatas.Count <= minPlayerRequired)
        {
            int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
            avatars.Shuffle();
            int[] randomAvatars = avatars.Take(botPlayers).ToArray();

            int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
            names.Shuffle();
            int[] randomNames = names.Take(botPlayers).ToArray();

            for (int i = 0; i < botPlayers; i++)
            {
                string avatar =
                    BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                string botUserName =
                    BotManager.Instance.botUserName[randomNames[i]];
                string userId = DataManager.Instance.joinPlayerDatas[i].userId
                    .Substring(0, DataManager.Instance.joinPlayerDatas[i].userId.Length - 1) + "TeenPatti";
                DataManager.Instance.AddRoomUser(userId, botUserName,
                    DataManager.Instance.joinPlayerDatas[i].lobbyId,
                    UnityEngine.Random.Range(459, 10000).ToString(), i, avatar);
            }
        }
    }


    public void LoadLudoBotPlayers()
    {
        print("---------------Load Bot For Ludo-----------------");
        if (DataManager.Instance.isTwoPlayer)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 2)
            {
                return;
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1 && BotManager.Instance.isBotAvalible)
            {
                StartCoroutine(DelayedCheckForBot());
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                Debug.Log("3");

                GenerateNoPlayersFound();
                return;
            }
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            int maxPlayer = 4;
            int playerRequired = maxPlayer - DataManager.Instance.joinPlayerDatas.Count;
            print("Data Manager Join Player Count : " + DataManager.Instance.joinPlayerDatas.Count);

            if (DataManager.Instance.joinPlayerDatas.Count == 4)
            {
                return;
            }
            else if (DataManager.Instance.joinPlayerDatas.Count is 1 or 2 or 3 && BotManager.Instance.isBotAvalible)
            {
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas[i].userId)
                    {
                        DataManager.Instance.playerNo = i + 1;
                        break;
                    }
                }
                for (int i = 0; i < playerRequired; i++)
                {
                    Debug.Log("Adding bot for 4 player game");
                    int playerNo = i + 2;
                    string avatar = BotManager.Instance.botUser_Profile_URL[UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
                    string botUserName = BotManager.Instance.botUserName[UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
                    string userId = DataManager.Instance.joinPlayerDatas[i].userId.Substring(0, DataManager.Instance.joinPlayerDatas[i].userId.Length - 1) + "Ludo";
                    DataManager.Instance.AddRoomUser(userId, botUserName, DataManager.Instance.joinPlayerDatas[i].lobbyId, 10.ToString(), playerNo, avatar);
                    TestSocketIO.Instace.BotJoinLudoRoom(userId, botUserName, DataManager.Instance.joinPlayerDatas[i].lobbyId, 10.ToString(), avatar);
                }

                BotManager.Instance.isConnectBot = true;
                int rnoInd = 0;
                if (rnoInd == 0)
                {
                    print("This is the assigned player number -> " + DataManager.Instance.playerNo);

                    JoinPlayerData joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                    JoinPlayerData joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];
                    JoinPlayerData joinplayerData3 = DataManager.Instance.joinPlayerDatas[2];
                    JoinPlayerData joinplayerData4 = DataManager.Instance.joinPlayerDatas[3];

                    string userId1 = joinplayerData1.userId;
                    string userName1 = joinplayerData1.userName;
                    string avtar1 = joinplayerData1.avtar;
                    DataManager.Instance.joinPlayerDatas[0].userId = userId1;
                    DataManager.Instance.joinPlayerDatas[0].userName = userName1;
                    DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                    DataManager.Instance.joinPlayerDatas[0].avtar = avtar1;

                    DataManager.Instance.joinPlayerDatas[1].userId = joinplayerData2.userId;
                    DataManager.Instance.joinPlayerDatas[1].userName = joinplayerData2.userName;
                    DataManager.Instance.joinPlayerDatas[1].balance = joinplayerData2.balance;
                    DataManager.Instance.joinPlayerDatas[1].playerNo = 2;
                    DataManager.Instance.joinPlayerDatas[1].avtar = joinplayerData2.avtar;

                    DataManager.Instance.joinPlayerDatas[2].userId = joinplayerData3.userId;
                    DataManager.Instance.joinPlayerDatas[2].userName = joinplayerData3.userName;
                    DataManager.Instance.joinPlayerDatas[2].balance = joinplayerData3.balance;
                    DataManager.Instance.joinPlayerDatas[2].playerNo = 3;
                    DataManager.Instance.joinPlayerDatas[2].avtar = joinplayerData3.avtar;

                    DataManager.Instance.joinPlayerDatas[3].userId = joinplayerData4.userId;
                    DataManager.Instance.joinPlayerDatas[3].userName = joinplayerData4.userName;
                    DataManager.Instance.joinPlayerDatas[3].balance = joinplayerData4.balance;
                    DataManager.Instance.joinPlayerDatas[3].playerNo = 4;
                    DataManager.Instance.joinPlayerDatas[3].avtar = joinplayerData4.avtar;
                    BotManager.Instance.isConnectBot = true;
                }
                else
                {
                    BotManager.Instance.isConnectBot = true;
                }
            }
            else if (DataManager.Instance.joinPlayerDatas.Count < 4)
            {
                Debug.Log("4");

                GenerateNoPlayersFound();
            }
        }
    }


    private IEnumerator DelayedCheckForBot()
    {
        yield return new WaitForSeconds(3f);

        if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            var playerNo = 3;
            var avatar = BotManager.Instance.botUser_Profile_URL[UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
            var botUserName = BotManager.Instance.botUserName[UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
            var userId = DataManager.Instance.joinPlayerDatas[0].userId
                .Substring(0, DataManager.Instance.joinPlayerDatas[0].userId.Length - 1) + "Ludo";
            DataManager.Instance.AddRoomUser(userId, botUserName, DataManager.Instance.joinPlayerDatas[0].lobbyId,
                10.ToString(), playerNo, avatar);

            BotManager.Instance.isConnectBot = true;

            int rnoInd = UnityEngine.Random.Range(0, 2);
            print("rnoInd : " + rnoInd);

            if (rnoInd == 0)
            {
                DataManager.Instance.playerNo = 3;

                var joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                var joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];

                var userId1 = joinplayerData1.userId;
                var userName1 = joinplayerData1.userName;
                var balance1 = joinplayerData1.balance;
                var avtar1 = joinplayerData1.avtar;

                DataManager.Instance.joinPlayerDatas[0].userId = joinplayerData2.userId;
                DataManager.Instance.joinPlayerDatas[0].userName = joinplayerData2.userName;
                DataManager.Instance.joinPlayerDatas[0].balance = joinplayerData2.balance;
                DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                DataManager.Instance.joinPlayerDatas[0].avtar = joinplayerData2.avtar;

                DataManager.Instance.joinPlayerDatas[1].userId = userId1;
                DataManager.Instance.joinPlayerDatas[1].userName = userName1;
                DataManager.Instance.joinPlayerDatas[1].balance = balance1;
                DataManager.Instance.joinPlayerDatas[1].playerNo = 3;
                DataManager.Instance.joinPlayerDatas[1].avtar = avtar1;

                BotManager.Instance.isConnectBot = true;
            }
            else
            {
                BotManager.Instance.isConnectBot = true;
            }
        }
    }


    #endregion

    #region Snake and ladder maintain

    public void SetSnakeGame(Text t)
    {
        DataManager.Instance.SetPlayedGame(DataManager.Instance.GetPlayedGame() + 1);
        if (DataManager.Instance.joinPlayerDatas.Count == 2)
        {
            TestSocketIO.Instace.StartGameBot(TestSocketIO.Instace.roomid, DataManager.Instance.joinPlayerDatas[0].lobbyId);
            //StartCoroutine(LoadSnakeScene());
            GenerateSnakeLoadingPanel();
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 1 &&
                     BotManager.Instance.isBotAvalible) // && DataManager.Instance.gameType!="Game")
        {
            TestSocketIO.Instace.StartGameBot(TestSocketIO.Instace.roomid, DataManager.Instance.joinPlayerDatas[0].lobbyId);
            //print("Enter The Bot Connect");
            //print("Enter The Condition");
            int playerNo = 2;


            string avatar =
                BotManager.Instance.botUser_Profile_URL[
                    UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
            string botUserName =
                BotManager.Instance.botUserName[UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
            string userId = DataManager.Instance.joinPlayerDatas[0].userId
                .Substring(0, DataManager.Instance.joinPlayerDatas[0].userId.Length - 1) + "Snake";
            DataManager.Instance.AddRoomUser(userId, botUserName, DataManager.Instance.joinPlayerDatas[0].lobbyId,
                10.ToString(), playerNo, avatar);


            BotManager.Instance.isConnectBot = true;
            int rnoInd = UnityEngine.Random.Range(0, 2);
            //int rnoInd = 0;
            print("rnoInd : " + rnoInd);
            if (rnoInd == 0)
            {
                DataManager.Instance.playerNo = 2;

                JoinPlayerData joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                JoinPlayerData joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];


                string userId1 = joinplayerData1.userId;
                string userName1 = joinplayerData1.userName;
                string balance1 = joinplayerData1.balance;
                string avtar1 = joinplayerData1.avtar;
                string pPic = joinplayerData1.pPicture;
                //print("Join Player Data 1 : " + joinplayerData1.userName);
                //print("Join Player Data 2 : " + joinplayerData2.userName);
                DataManager.Instance.joinPlayerDatas[0].userId = joinplayerData2.userId;
                DataManager.Instance.joinPlayerDatas[0].userName = joinplayerData2.userName;
                DataManager.Instance.joinPlayerDatas[0].balance = joinplayerData2.balance;
                DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                DataManager.Instance.joinPlayerDatas[0].avtar = joinplayerData2.avtar;
                DataManager.Instance.joinPlayerDatas[0].pPicture = joinplayerData2.pPicture;

                DataManager.Instance.joinPlayerDatas[1].userId = userId1;
                DataManager.Instance.joinPlayerDatas[1].userName = userName1;
                DataManager.Instance.joinPlayerDatas[1].balance = balance1;
                DataManager.Instance.joinPlayerDatas[1].playerNo = 2;
                DataManager.Instance.joinPlayerDatas[1].avtar = avtar1;
                DataManager.Instance.joinPlayerDatas[1].pPicture = pPic;
                BotManager.Instance.isConnectBot = true;
                //StartCoroutine(LoadSnakeScene());
                GenerateSnakeLoadingPanel();
            }
            else
            {
                BotManager.Instance.isConnectBot = true;
                GenerateSnakeLoadingPanel();
                //StartCoroutine(LoadSnakeScene());
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            DataManager.Instance.tournamentID = "";
            DataManager.Instance.tourEntryMoney = 0;
            DataManager.Instance.tourCommision = 0;
            DataManager.Instance.commisionAmount = 0;
            DataManager.Instance.orgIndexPlayer = 0;
            DataManager.Instance.joinPlayerDatas.Clear();
            isPressJoin = false;
            t.text = "JOIN";
            TestSocketIO.Instace.roomid = "";
            TestSocketIO.Instace.userdata = "";
            TestSocketIO.Instace.playTime = 0;
            TestSocketIO.Instace.LeaveRoom();
            GenerateTournamentError();
        }
    }

    private void GenerateSnakeLoadingPanel()
    {
        Instantiate(snakeTournamentLoadPanel, prefabParent.transform);
    }

    #endregion

    #region Ludo Other Maintain


    public void OpenTournamentLoadScreen(Text t)
    {
        DataManager.Instance.SetPlayedGame(DataManager.Instance.GetPlayedGame() + 1);
        if (DataManager.Instance.isTwoPlayer)
        {
            print("Data Manager Join Player Count : " + DataManager.Instance.joinPlayerDatas.Count);
            if (DataManager.Instance.joinPlayerDatas.Count == 2)
            {
                StartCoroutine(LoadScene());
                //GenerateLoadingPanel();
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1 &&
                     BotManager.Instance.isBotAvalible) // && DataManager.Instance.gameType!="Game")
            {
                //print("Enter The Bot Connect");
                //print("Enter The Condition");
                int playerNo = 3;


                string avatar =
                    BotManager.Instance.botUser_Profile_URL[
                        UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
                string botUserName =
                    BotManager.Instance.botUserName[UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
                string userId = DataManager.Instance.joinPlayerDatas[0].userId
                    .Substring(0, DataManager.Instance.joinPlayerDatas[0].userId.Length - 1) + "Ludo";
                DataManager.Instance.AddRoomUser(userId, botUserName, DataManager.Instance.joinPlayerDatas[0].lobbyId,
                    10.ToString(), playerNo, avatar);


                BotManager.Instance.isConnectBot = true;
                int rnoInd = UnityEngine.Random.Range(0, 2);
                //int rnoInd = 0;
                print("rnoInd : " + rnoInd);
                if (rnoInd == 0)
                {
                    DataManager.Instance.playerNo = 3;

                    JoinPlayerData joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                    JoinPlayerData joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];


                    string userId1 = joinplayerData1.userId;
                    string userName1 = joinplayerData1.userName;
                    string balance1 = joinplayerData1.balance;
                    string avtar1 = joinplayerData1.avtar;
                    string pPic = joinplayerData1.pPicture;
                    //print("Join Player Data 1 : " + joinplayerData1.userName);
                    //print("Join Player Data 2 : " + joinplayerData2.userName);
                    DataManager.Instance.joinPlayerDatas[0].userId = joinplayerData2.userId;
                    DataManager.Instance.joinPlayerDatas[0].userName = joinplayerData2.userName;
                    DataManager.Instance.joinPlayerDatas[0].balance = joinplayerData2.balance;
                    DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                    DataManager.Instance.joinPlayerDatas[0].avtar = joinplayerData2.avtar;
                    DataManager.Instance.joinPlayerDatas[0].pPicture = joinplayerData2.pPicture;

                    DataManager.Instance.joinPlayerDatas[1].userId = userId1;
                    DataManager.Instance.joinPlayerDatas[1].userName = userName1;
                    DataManager.Instance.joinPlayerDatas[1].balance = balance1;
                    DataManager.Instance.joinPlayerDatas[1].playerNo = 3;
                    DataManager.Instance.joinPlayerDatas[1].avtar = avtar1;
                    DataManager.Instance.joinPlayerDatas[1].pPicture = pPic;
                    BotManager.Instance.isConnectBot = true;
                    StartCoroutine(LoadScene());
                    //GenerateLoadingPanel();
                }
                else
                {
                    BotManager.Instance.isConnectBot = true;
                    //GenerateLoadingPanel();
                    StartCoroutine(LoadScene());
                }
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                DataManager.Instance.tournamentID = "";
                DataManager.Instance.tourEntryMoney = 0;
                DataManager.Instance.tourCommision = 0;
                DataManager.Instance.commisionAmount = 0;
                DataManager.Instance.orgIndexPlayer = 0;
                DataManager.Instance.joinPlayerDatas.Clear();
                isPressJoin = false;

                t.text = "JOIN";
                TestSocketIO.Instace.roomid = "";
                TestSocketIO.Instace.userdata = "";
                TestSocketIO.Instace.playTime = 0;
                TestSocketIO.Instace.LeaveRoom();
                GenerateTournamentError();
            }

            //if (DataManager.Instance.joinPlayerDatas.Count == 2)
            //{
            //    GenerateLoadingPanel();
            //}
            //else if(DataManager.Instance.joinPlayerDatas.Count==1)
            //{
            //    DataManager.Instance.tournamentID = "";
            //    DataManager.Instance.tourEntryMoney = 0;
            //    DataManager.Instance.tourCommision = 0;
            //    DataManager.Instance.commisionAmount = 0;
            //    DataManager.Instance.orgIndexPlayer = 0;
            //    DataManager.Instance.joinPlayerDatas.Clear();
            //    t.text = "JOIN";
            //    isPressJoin = false;
            //    TestSocketIO.Instace.roomid = "";
            //    TestSocketIO.Instace.userdata = "";
            //    TestSocketIO.Instace.playTime = 0;
            //    Instantiate(tournamentErrorObj, parentObj.transform);

            //}
            //else if(DataManager.Instance.joinPlayerDatas.Count == 1)
            //{

            //}
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            int maxPlayer = 4;
            int playerRequired = maxPlayer - DataManager.Instance.joinPlayerDatas.Count;
            print("Data Manager Join Player Count : " + DataManager.Instance.joinPlayerDatas.Count);

            if (DataManager.Instance.joinPlayerDatas.Count == 4)
            {
                StartCoroutine(LoadScene());
                //GenerateLoadingPanel();
            }
            else if (DataManager.Instance.joinPlayerDatas.Count is 1 or 2 or 3 &&
                     BotManager.Instance.isBotAvalible) // && DataManager.Instance.gameType!="Game")
            {
                //print("Enter The Bot Connect");
                //print("Enter The Condition");
                // for assigning player number
                Debug.Log("DataManager.Instance.playerNo before switch = " + DataManager.Instance.playerNo);

                //DataManager.Instance.playerNo = DataManager.Instance.joinPlayerDatas.Count switch
                //{
                //    2 => 2,
                //    3 => 3,
                //    1 => 1,
                //    _ => DataManager.Instance.playerNo
                //};

                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas[i].userId)
                    {
                        DataManager.Instance.playerNo = i + 1;
                        break;
                    }
                }
                Debug.Log("DataManager.Instance.playerNo after switch = " + DataManager.Instance.playerNo);
                //if (DataManager.Instance.playerNo == 1)
                //{
                for (int i = 0; i < playerRequired; i++)
                {
                    Debug.Log("Adding bot for 4 player game");
                    int playerNo = i + 2;
                    string avatar =
                        BotManager.Instance.botUser_Profile_URL[
                            UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count)];
                    string botUserName =
                        BotManager.Instance.botUserName[
                            UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count)];
                    string userId = DataManager.Instance.joinPlayerDatas[i].userId
                        .Substring(0, DataManager.Instance.joinPlayerDatas[i].userId.Length - 1) + "Ludo";
                    DataManager.Instance.AddRoomUser(userId, botUserName,
                        DataManager.Instance.joinPlayerDatas[i].lobbyId,
                        10.ToString(), playerNo, avatar);
                    TestSocketIO.Instace.BotJoinLudoRoom(userId, botUserName, DataManager.Instance.joinPlayerDatas[i].lobbyId, 10.ToString(), avatar);
                }
                //}


                BotManager.Instance.isConnectBot = true;
                //int rnoInd = UnityEngine.Random.Range(0, 2);
                int rnoInd = 0;
                print("rnoInd : " + rnoInd);
                if (rnoInd == 0)
                {
                    print("This is the assigned player number -> " + DataManager.Instance.playerNo);

                    JoinPlayerData joinplayerData1 = DataManager.Instance.joinPlayerDatas[0];
                    JoinPlayerData joinplayerData2 = DataManager.Instance.joinPlayerDatas[1];
                    JoinPlayerData joinplayerData3 = DataManager.Instance.joinPlayerDatas[2];
                    JoinPlayerData joinplayerData4 = DataManager.Instance.joinPlayerDatas[3];

                    string userId1 = joinplayerData1.userId;
                    string userName1 = joinplayerData1.userName;
                    string balance1 = joinplayerData1.balance;
                    string avtar1 = joinplayerData1.avtar;
                    //print("Join Player Data 1 : " + joinplayerData1.userName);
                    //print("Join Player Data 2 : " + joinplayerData2.userName);
                    DataManager.Instance.joinPlayerDatas[0].userId = userId1;
                    DataManager.Instance.joinPlayerDatas[0].userName = userName1;
                    DataManager.Instance.joinPlayerDatas[0].balance = balance1;
                    DataManager.Instance.joinPlayerDatas[0].playerNo = 1;
                    DataManager.Instance.joinPlayerDatas[0].avtar = avtar1;

                    DataManager.Instance.joinPlayerDatas[1].userId = joinplayerData2.userId;
                    DataManager.Instance.joinPlayerDatas[1].userName = joinplayerData2.userName;
                    DataManager.Instance.joinPlayerDatas[1].balance = joinplayerData2.balance;
                    DataManager.Instance.joinPlayerDatas[1].playerNo = 2;
                    DataManager.Instance.joinPlayerDatas[1].avtar = joinplayerData2.avtar;

                    DataManager.Instance.joinPlayerDatas[2].userId = joinplayerData3.userId;
                    DataManager.Instance.joinPlayerDatas[2].userName = joinplayerData3.userName;
                    DataManager.Instance.joinPlayerDatas[2].balance = joinplayerData3.balance;
                    DataManager.Instance.joinPlayerDatas[2].playerNo = 3;
                    DataManager.Instance.joinPlayerDatas[2].avtar = joinplayerData3.avtar;

                    DataManager.Instance.joinPlayerDatas[3].userId = joinplayerData4.userId;
                    DataManager.Instance.joinPlayerDatas[3].userName = joinplayerData4.userName;
                    DataManager.Instance.joinPlayerDatas[3].balance = joinplayerData4.balance;
                    DataManager.Instance.joinPlayerDatas[3].playerNo = 4;
                    DataManager.Instance.joinPlayerDatas[3].avtar = joinplayerData4.avtar;
                    BotManager.Instance.isConnectBot = true;
                    StartCoroutine(LoadScene());
                    //GenerateLoadingPanel();
                }
                else
                {
                    BotManager.Instance.isConnectBot = true;
                    StartCoroutine(LoadScene());
                    //GenerateLoadingPanel();
                }
            }
            else if (DataManager.Instance.joinPlayerDatas.Count == 1)
            {
                DataManager.Instance.tournamentID = "";
                DataManager.Instance.tourEntryMoney = 0;
                DataManager.Instance.tourCommision = 0;
                DataManager.Instance.commisionAmount = 0;
                DataManager.Instance.orgIndexPlayer = 0;
                DataManager.Instance.joinPlayerDatas.Clear();
                isPressJoin = false;

                t.text = "JOIN";
                TestSocketIO.Instace.roomid = "";
                TestSocketIO.Instace.userdata = "";
                TestSocketIO.Instace.playTime = 0;
                TestSocketIO.Instace.LeaveRoom();
                GenerateTournamentError();
            }
        }
    }

    public IEnumerator LoadScene()
    {

        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Ludo");

        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                //Destroy(obj);
                Screen.orientation = ScreenOrientation.Portrait;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public IEnumerator LoadSnakeScene()
    {

        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Snake");

        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                //Destroy(obj);
                Screen.orientation = ScreenOrientation.Portrait;
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }



    #endregion

    #region Coupon List
    public void GetCoupon()
    {
        StartCoroutine(GetCoupons());
    }

    IEnumerator GetCoupons()
    {
        print("Enter the Coupon");
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/couponlist/bonus");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Coupn Text : " + request.downloadHandler.text);
            JSONNode value = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(value["data"].ToString());


            print(request.downloadHandler.text);
            for (int i = 0; i < data.Count; i++)
            {
                CouponData tempData = new CouponData();
                tempData.id = RemoveQuotes(data[i]["_id"].ToString());
                tempData.minAmount = data[i]["minAmount"];
                tempData.couponAmount = data[i]["couponAmount"];
                tempData.isActive = data[i]["active"];
                if (tempData.isActive)
                {
                    couponDatas.Add(tempData);
                }
            }
        }

    }

    #endregion


}
[System.Serializable]
public class TournamentData
{
    public bool bot;
    public float bonusAmountDeduction;
    public bool active;
    public string _id;
    public string name;
    public GameType modeType;
    public float betAmount;
    public float minBet;
    public float maxBet;
    public float maxPayout;
    public float challLimit;
    public float potLimit;
    public int players;
    public int winner;
    public List<string> winnerRow = new List<string>();
    public float totalWinAmount;
    public float time;
    public int complexity;
    public int interval;
    public string _v;
    public string createdAt;
    public string updatedAt;
}

[System.Serializable]
public class CouponData
{
    public string id;
    public float minAmount;
    public float couponAmount;
    public bool isActive;
}
