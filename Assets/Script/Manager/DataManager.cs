using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameType
{
    None,
    Dragon_Tiger,
    Andar_Bahar,
    Roulette,
    Car_Roulette,
    Ludo,
    Fun_Target,
    Rummy,
    Teen_Patti,
    Crash,
    Teen_Patti_AK47,
    Point_Rummy,
    Red_vs_Black,
    SevenUpDown,
    Joker,
    Pool_Rummy,
    Deal_Rummy,
    Poker,
    Jhandi_Munda,
    Aviator,
    SpinAndWin
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;



    public string url;
    public PlayerData playerData;

    public float adminPercentage;
    public float minimumDepositAmount;

    public string cashFreeId;
    public float tourBonusCut;

    public GameType gameMode;
    [Space]
    [Header("--- Tap Particle ---")] public GameObject tapParticles;
    [Space]
    public int playerNo;
    public bool isTwoPlayer;
    public bool isFourPlayer;
    public GameObject winParticles;

    [Header("--- Basic ---")]
    public string gameType;
    public string appVersion;
    public string appUrl;
    public int currentPNo;

    [Header("---Tournament---")]
    public float tourEntryMoney;
    public float winAmount;
    public string tournamentID;
    public string gameId;
    //public string currentRoomName;
    public List<JoinPlayerData> joinPlayerDatas = new List<JoinPlayerData>();
    public List<JoinPlayerData> leaveUpdatePlayerDatas = new List<JoinPlayerData>();
    public int tourCommision;
    public float commisionAmount;
    public int orgIndexPlayer;
    public List<TournamentData> tournamentData = new List<TournamentData>();
    public bool isTournamentLoaded;
    public int gameComplexity;

    [Header("--Ludo--")]
    public int modeType;
    public bool isDiceClick;
    public bool isTimeAuto;
    public int diceManageCnt;
    public bool hasCalledOpenTournamentLoadScreen = false;
    [HideInInspector]
    public bool isBotSix;
    public int botPasaNo;
    public bool isRestartManage;
    public bool isAvaliable;
    public float betPrice;
    public bool IsOneTimeOpen = false;

    [Header("---Daily Spin---")]
    public int thisMonthDays;



    [Header("---Teen Patti---")]
    public float chaalLimit;
    public float potLimit;

    [Header("---DragonTiger---")]
    public string listString;

    [Header("---Aviator---")]
    public string historyPoints;

    [Header("---Andar Bahar---")]
    public string winList;

    [Header("---Roulette---")]
    public bool rouletteGameStatus;
    public string winRecord;

    [Header("---Car Roulette---")]
    public string historyRecord;

    [Header("---SevenUpDown---")]
    public string sevenUpDownWinHistory;
    public string sevenUpDownDiceHistory;

    [Header("---Rummy---")]
    public float pointValue = 0.1f;

    public float pointLimit = 101;//for pool rummy must be 101/201

    public bool isShopRequest;


    public string PanSavedKey = "PanSaved";
    public string OtpVerifiedKey = "OtpVerified";

    private Sprite selectedAvatarSprite;
    private int selectedAvatarIndex;
    private Image profileTopImg;
    private Sprite[] avatars;
    private string avatarFolderPath = "Avatar";






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        isTournamentLoaded = false;
    }

    [Obsolete("Obsolete")]
    private void Start()
    {
        GetVersionUpdate();
        GetTournament();
        //MainMenuManager.Instance.GetTran();
        LoadProfile();
    }

    #region TapParticals

    private void Update()
    {
        /*if (!Input.GetMouseButtonDown(0)) return;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject particleObj = Instantiate(tapParticles, new Vector2(pos.x, pos.y), Quaternion.identity);
        Destroy(particleObj, 3f);*/
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
            float scaleFactor = CalculateScaleFactor(worldPosition);
            InstantiateTapEffect(worldPosition, scaleFactor);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBackButton();
        }
    }
    private void HandleBackButton()
    {
        // Get the currently active scene
        Scene activeScene = SceneManager.GetActiveScene();

        // Find the Menu Button in the active scene (by name or tag)
        GameObject menuButtonObject = GameObject.Find("-- Menu Button --");

        if (menuButtonObject == null)
        {
            Debug.LogError("Menu Button not found in the active scene!");
            return;
        }

        // Get the Button component
        Button menuButton = menuButtonObject.GetComponent<Button>();

        if (menuButton == null)
        {
            Debug.LogError("Menu Button does not have a Button component!");
            return;
        }

        // Invoke the OnClick event of the Menu Button
        menuButton.onClick.Invoke();
    }
    private void InstantiateTapEffect(Vector3 position, float scaleFactor)
    {
        GameObject particleObj = Instantiate(tapParticles, position, Quaternion.identity);
        particleObj.transform.localScale = Vector3.one * scaleFactor;
        Destroy(particleObj, 2f);
    }
    private float CalculateScaleFactor(Vector3 touchPositionInWorldSpace)
    {
        float scaleFactor = 0.3f; // Set the base scale factor to 0.3

        if (!Camera.main.orthographic)
        {
            // For perspective cameras, adjust the scale factor based on the camera's field of view
            // and distance from the touch position
            float fov = Camera.main.fieldOfView;
            float distance = Vector3.Distance(Camera.main.transform.position, touchPositionInWorldSpace);
            float perspectiveScaleFactor = Mathf.Tan(Mathf.Deg2Rad * (fov / 2)) * distance;

            // Adjust the scale factor to maintain consistency with the orthographic camera scale
            scaleFactor = 0.3f / perspectiveScaleFactor;
        }

        return scaleFactor;
    }


    #endregion

    #region Storage

    public void SetLoginValue(string value)
    {
        PlayerPrefs.SetString("LoginValue", value);
    }
    public string GetLoginValue()
    {
        return PlayerPrefs.GetString("LoginValue", "N");
    }

    public void SetSound(int no)
    {
        PlayerPrefs.SetInt("SoundValue", no);
    }
    public int GetSound()
    {
        return PlayerPrefs.GetInt("SoundValue", 0);
    }

    public void SetMusic(int no)
    {
        PlayerPrefs.SetInt("MusicValue", no);
    }
    public int GetMusic()
    {
        return PlayerPrefs.GetInt("MusicValue", 0);
    }
    public void SetVibration(int no)
    {
        PlayerPrefs.SetInt("VibrationValue", no);
    }
    public int GetVibration()
    {
        return PlayerPrefs.GetInt("VibrationValue", 0);
    }

    public void SetNotification(int no)
    {
        PlayerPrefs.SetInt("NotificationValue", no);
    }
    public int GetNotification()
    {
        return PlayerPrefs.GetInt("NotificationValue", 0);
    }

    public void SetFriendRequest(int no)
    {
        PlayerPrefs.SetInt("FriendRequestValue", no);
    }
    public int GetFriendRequest()
    {
        return PlayerPrefs.GetInt("FriendRequestValue", 0);
    }

    public void SetBankValue(int no)
    {
        PlayerPrefs.SetInt("AddedBankValue", no);
    }

    public int GetBankValue()
    {
        return PlayerPrefs.GetInt("AddedBankValue", 0);
    }
    public void SetAvatarValue(int no)
    {
        PlayerPrefs.SetInt("AvatarValue", no);
    }

    public int GetAvatarValue()
    {
        return PlayerPrefs.GetInt("AvatarValue", 0);
    }

    public void SetDefaultPlayerName(string s)
    {
        string setData = "user" + UnityEngine.Random.Range(99, 999) + s;
        //print("Set Data : " + setData);
        PlayerPrefs.SetString("Default_User_Name", setData);
    }
    public string GetDefaultPlayerName()
    {
        return PlayerPrefs.GetString("Default_User_Name");
    }


    public void SetPlayedGame(int no)
    {
        PlayerPrefs.SetInt("PlayedGame", no);
    }

    public int GetPlayedGame()
    {
        return PlayerPrefs.GetInt("PlayedGame", 0);
    }

    public void SetWonMoneyGame(float no)
    {
        PlayerPrefs.SetFloat("WonPlayedGame", no);
    }

    public float GetWonMoneyGame()
    {
        return PlayerPrefs.GetFloat("WonPlayedGame", 0);
    }

    public bool IsPanSaved()
    {
        return PlayerPrefs.GetInt(PanSavedKey, 0) == 1;
    }

    public bool IsOtpVerified()
    {
        return PlayerPrefs.GetInt(OtpVerifiedKey, 0) == 1;
    }

    public void SetSelectedAvatarSprite(Sprite sprite)
    {
        selectedAvatarSprite = sprite;
    }

    public Sprite GetSelectedAvatarSprite()
    {
        return selectedAvatarSprite;
    }

    #endregion

    public void OnApplicationClose()
    {
        Application.Quit();
    }

    public void LoadProfile()
    {
        LoadAvatars();
        int selectedAvatarIndex = PlayerPrefs.GetInt("SelectedAvatarIndex", -1);
        if (selectedAvatarIndex != -1)
        {
            LoadSelectedAvatarSprite(selectedAvatarIndex);
        }
    }

    private void LoadSelectedAvatarSprite(int index)
    {
        if (index == -1)
        {
            StartCoroutine(GetImages(PlayerPrefs.GetString("ProfileURL"), profileTopImg));
            selectedAvatarSprite = profileTopImg.sprite;
        }
        else if (index >= 0 && index < avatars.Length)
        {
            selectedAvatarSprite = avatars[index];
        }
        else
        {
            selectedAvatarSprite = null; // Use a default sprite if desired
        }
    }

    private void LoadAvatars()
    {
        avatars = Resources.LoadAll<Sprite>(avatarFolderPath);
    }

    public void LoadProfileImage(string url, Image avatar)
    {
        string prefix = "http://139.59.77.167/assets/img/";

        if (url.StartsWith(prefix))
        {
            StartCoroutine(GetImages(url, avatar));
        }
        else
        {
            Sprite selectedAvatarSprite = GetSelectedAvatarSprite();

            if (selectedAvatarSprite != null)
            {
                avatar.sprite = selectedAvatarSprite;
            }
            else
            {
                StartCoroutine(GetImages(url, avatar));
            }
        }
    }


    public void UserTurnVibrate()
    {
        // Check if the device supports vibration
        if (!SystemInfo.supportsVibration) return;
        // Vibrate the device for 500 milliseconds
        Handheld.Vibrate();
    }


    public string GetModeToSceneName(GameType type)
    {
        string gameName = type switch
        {
            GameType.Teen_Patti => "TeenPatti",
            GameType.Andar_Bahar => "AndarBahar",
            GameType.Roulette => "Rouletee",
            GameType.Dragon_Tiger => "DragonTiger",
            GameType.Ludo => "Ludo",
            GameType.Car_Roulette => "CarRoulette",
            GameType.Fun_Target => "FunTarget",
            GameType.Rummy => "Rummy",
            GameType.Crash => "Crash",
            GameType.Teen_Patti_AK47 => "AK47",
            GameType.Point_Rummy => "PointRummy",
            GameType.Red_vs_Black => "RedVsBlack",
            GameType.SevenUpDown => "7UpDown",
            GameType.Joker => "Joker",
            GameType.Pool_Rummy => "PoolRummy",
            GameType.Deal_Rummy => "DealRummy",
            GameType.Poker => "Poker",
            GameType.Jhandi_Munda => "JhandiMunda",
            GameType.Aviator => "Aviator",
            GameType.SpinAndWin => "SpinAndWin",
            _ => ""
        };
        /*else if (type == GameType.Poker)
        {
            gameName = "Poker";
        }*/
        return gameName;
    }

    // #endregion

    #region Daily Spin

    public void SetDayValue(int no, int value)
    {
        PlayerPrefs.SetInt("Date" + no, value);
    }

    public int GetDayValue(int no)
    {
        return PlayerPrefs.GetInt("Date" + no, 0);
    }

    public void SetDayRewardValue(int no, int money)
    {
        PlayerPrefs.SetInt("Date" + no, money);
    }


    #endregion

    #region Version Update

    public void GetVersionUpdate()
    {
        StartCoroutine(GetVersionUpdates());
    }

    IEnumerator GetVersionUpdates()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/v1/players/versionlist");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Get Request Message : " + request.downloadHandler.text);

            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(keys["data"].ToString());

            appVersion = data[0]["versionControle"];
            appUrl = data[0]["appLink"];

            //InternetManager.Instance.CheckUpdate();
        }

    }

    #endregion


    #region Tournaments

    [Obsolete("Obsolete")]
    public void GetTournament()
    {
        if (tournamentData.Count == 0)
        {
            StartCoroutine(GetTournaments());
        }

    }

    [Obsolete("Obsolete")]
    IEnumerator GetTournaments()
    {
        tournamentData.Clear();
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/tournaments");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("Tour Data : " + request.downloadHandler.text);
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(values["data"].ToString());

            string login = GetLoginValue();
            if (values["success"] == false && login == "Y")
            {
                // if token expires then login vale is set to NO then show login screen.
                SetLoginValue("N");
                yield break;
            }

            for (int i = 0; i < data.Count; i++)
            {
                TournamentData t = new TournamentData
                {
                    bot = data[i]["bot"],
                    bonusAmountDeduction = data[i]["bonusAmountDeduction"],
                    active = data[i]["active"],
                    _id = data[i]["_id"],
                    name = data[i]["name"],
                };

                t.modeType = int.Parse(data[i]["mode"]) switch
                {
                    1 => GameType.Teen_Patti,
                    2 => GameType.Dragon_Tiger,
                    3 => GameType.Roulette,
                    4 => GameType.Poker,
                    5 => GameType.Andar_Bahar,
                    6 => GameType.Ludo,
                    7 => GameType.Teen_Patti_AK47,
                    8 => GameType.Joker,
                    9 => GameType.SevenUpDown,
                    10 => GameType.Car_Roulette,
                    11 => GameType.Jhandi_Munda,
                    12 => GameType.Point_Rummy,
                    //12 => GameType.Red_vs_Black,
                    //13 => GameType.SevenUpDown,
                    13 => GameType.Pool_Rummy,//15
                    14 => GameType.Deal_Rummy,
                    15 => GameType.Aviator,
                    16 => GameType.SpinAndWin,
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
                    tournamentData.Add(t);
                }
            }
        }
        else
        {
            if (request.error != null) Logger.log.Log(request.error.ToString());
        }
    }

    #endregion


    #region Null Maintain
    public bool IsNullOrEmpty(string value)
    {
        return value == null || value.Length == 0;
    }

    #endregion

    #region User Maintain


    public void AddRoomUser(string userId, string userName, string lobbyId, string balance, int playerNo, string avtar)
    {
        JoinPlayerData joinPlayer = new JoinPlayerData();
        joinPlayer.userId = userId;
        joinPlayer.userName = userName;
        joinPlayer.balance = balance;
        joinPlayer.playerNo = playerNo;
        joinPlayer.lobbyId = lobbyId;
        joinPlayer.avtar = avtar;

        int check = 0;
        for (int i = 0; i < joinPlayerDatas.Count; i++)
        {
            if (joinPlayerDatas[i].userId == joinPlayer.userId)
            {
                Debug.Log("Player check : " + joinPlayer.userName);
                check++;
            }

        }
        if (check == 0)
        {
            Debug.Log("Player Added : " + joinPlayer.userName);
            Debug.Log("Player Balence : " + joinPlayer.balance);
            joinPlayerDatas.Add(joinPlayer);
        }
        else
        {
            return;
        }
    }

    public bool CheckRoomUser(string userId)
    {


        Debug.Log("User ID =>  " + userId);
        JoinPlayerData joinPlayer = new JoinPlayerData();
        joinPlayer.userId = userId;


        int check = 0;
        for (int i = 0; i < joinPlayerDatas.Count; i++)
        {
            if (joinPlayerDatas[i].userId == joinPlayer.userId)
            {
                check++;
                Debug.Log("Check  =  " + check);
            }

        }
        Debug.Log("Check   $=  " + check);
        if (check == 0)
        {
            return true;
        }
        return false;
    }

    #endregion


    #region Debit and Credit Player Manage



    #region Credit

    //float com = (float)(bid.amount * Datamanger.Intance.data.commission) / 100;
    //LocalPlayer.Instace.addamount(bid.amount, TestSocketIO.Instace.roomid, "Internal Bid Won", "won", com);
    public void AddAmount(float amount, string roomid, string note, string log, float adminc, int winNo)
    {
        StartCoroutine(SendWonamount(amount, roomid, note, log, adminc, winNo));
    }

    IEnumerator SendWonamount(float amount, string roomid, string note, string log, float adminc, int winNo)
    {
        print("Win Amount : " + amount);
        WWWForm form = new WWWForm();
        form.AddField("amount", amount.ToString());
        form.AddField("gameId", roomid);
        form.AddField("note", note);
        form.AddField("logType", log);
        form.AddField("tournamentId", tournamentID);
        form.AddField("adminCommision", adminc.ToString());
        form.AddField("betNo", winNo);
        UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/game/won", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            print("Data Credit : " + request.downloadHandler.text.ToString());
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                JSONNode data = JSON.Parse(values["data"].ToString());
                Setplayerdata(data);
                //playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
                //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
                //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
                //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
            }
        }
        else
        {
            print("Data Credit : " + request.error);

        }
    }

    public void ReverseAmount(float amount, string roomid, string note, string log, int betNo)
    {
        StartCoroutine(ReverseBetAmount(amount, roomid, note, log, betNo));
    }

    private IEnumerator ReverseBetAmount(float amount, string roomid, string note, string log, int betNo)
    {
        print("Win Amount : " + amount);
        WWWForm form = new WWWForm();
        form.AddField("amount", amount.ToString());
        form.AddField("gameId", roomid);
        form.AddField("note", note);
        form.AddField("logType", log);
        form.AddField("betNo", betNo);
        form.AddField("tournamentId", tournamentID);
        //form.AddField("adminCommision", adminc.ToString());
        UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/reverse", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            print("Data Credit : " + request.downloadHandler.text.ToString());
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                JSONNode data = JSON.Parse(values["data"].ToString());
                Setplayerdata(data);
                //playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
                //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
                //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
                //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
            }
        }
        else
        {
            print("Data Credit : " + request.error);

        }
    }
    #endregion

    #region Debit

    public void DebitAmount(string amount, string roomId, string note, string logType, int betNo)
    {
        print("amount :  " + amount);
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("gameId", roomId);
        form.AddField("note", note);
        form.AddField("logType", logType);
        form.AddField("betNo", betNo);
        form.AddField("tournamentId", tournamentID);
        print("This is the debet data -> " + form);
        DebitAmount_Send(form);
    }

    void DebitAmount_Send(WWWForm form)
    {
        StartCoroutine(Debit_Amount_Ienum(form));
    }

    IEnumerator Debit_Amount_Ienum(WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/debit", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        JSONNode data = JSON.Parse(values["data"].ToString());

        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            if (request.error != "")
            {
                // Debug.Log("DebitError = " + request.error);
                Invoke(nameof(LudoManager.Instance.AddBetAmount), 1f);
            }
        }
        print("<color=blue> Debit Value : </color>" + request.downloadHandler.text);
        Setplayerdata(data);
        //Balance_Txt.text = data["balance"].ToString().Trim('"');
        //        playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
        //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
        //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
        //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
    }



    #endregion

    #region Bonus

    public void BonusDebitAmount(string amount, string roomId, string note, string logType)
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("gameId", roomId);
        form.AddField("note", note);
        form.AddField("logType", logType);
        BonusDebitAmount_Send(form);
    }

    void BonusDebitAmount_Send(WWWForm form)
    {
        StartCoroutine(Bonus_Debit_Amount_Ienum(form));
    }

    IEnumerator Bonus_Debit_Amount_Ienum(WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/debitBonus", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        JSONNode data = JSON.Parse(values["data"].ToString());
        //Balance_Txt.text = data["balance"].ToString().Trim('"');
        playerData.balance = data["balance"].ToString().Trim('"');
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.Getdata();
        }
    }


    public void BonusDebitAmount_Credit(string amount, string note, string logType)
    {
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("note", note);
        form.AddField("logType", logType);
        BonusDebitAmount_Send_Credit(form);
    }

    void BonusDebitAmount_Send_Credit(WWWForm form)
    {
        StartCoroutine(Bonus_Debit_Amount_Ienum_Credit(form));
    }

    IEnumerator Bonus_Debit_Amount_Ienum_Credit(WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/creditBonus", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        //JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
        //JSONNode data = JSON.Parse(values["data"].ToString());
        //WaitPanelManager.Instance.ClosePanel();

        //Balance_Txt.text = data["balance"].ToString().Trim('"');
        //playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
        //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
        //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
        //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
        if (request.error == null)
        {
            print("<color=blue>Data Credit : </color>" + request.downloadHandler.text);
            JSONNode values = JSON.Parse(request.downloadHandler.text);
            if (values["success"] == true)
            {
                JSONNode data = JSON.Parse(values["data"].ToString());
                Setplayerdata(data);
            }
        }
        else
        {
            print("Data Credit : " + request.error);

        }
    }


    #endregion

    public void SendLeaderBoardData(string winPlayerId, float amountWon, string tourId, int winner, string gameId, string note, string players)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerId", winPlayerId);
        form.AddField("amountWon", amountWon.ToString());
        form.AddField("tournamentId", tourId);
        form.AddField("winner", winner);
        form.AddField("gameId", gameId);
        form.AddField("gameStatus", "won");
        form.AddField("note", note);
        form.AddField("players", players);
        StartCoroutine(SendLeaderBoardDatas(form));
    }


    IEnumerator SendLeaderBoardDatas(WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(url + "/api/v1/players/saveleaderboard", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                //print("Bonus String :" + values["data"].ToString());
                //JSONNode data = JSON.Parse(values["data"].ToString());
                //playerData.balance = data[nameof(DataManager.Instance.playerData.balance)].ToString().Trim('"');
                //playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
                //playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
                //playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
            }
        }
    }




    public void Setplayerdata(JSONNode data)
    {
        Debug.Log("User Data===:::" + data.ToString());

        if (data[nameof(playerData.balance)] == "")
        {
            data[nameof(playerData.balance)] = "";
        }
        playerData.balance = ((float)data[nameof(playerData.balance)]).ToString("F2");
        playerData.kycStatus = data[nameof(playerData.kycStatus)];
        if (data[nameof(playerData.wonCount)] == "")
        {
            data[nameof(playerData.wonCount)] = "";
        }
        playerData.wonCount = data[nameof(playerData.wonCount)];
        if (data[nameof(playerData.joinCount)] == "")
        {
            data[nameof(playerData.joinCount)] = "";
        }
        playerData.joinCount = data[nameof(playerData.joinCount)];
        playerData.deposit = (data[nameof(playerData.deposit)]).ToString();
        playerData.winings = (data[nameof(playerData.winings)]).ToString();
        playerData.bonus = (data[nameof(playerData.bonus)]).ToString();
        playerData._id = data[nameof(playerData._id)];
        playerData.phone = data[nameof(playerData.phone)];
        playerData.aadharNumber = data[nameof(playerData.aadharNumber)];
        playerData.refer_code = data[nameof(playerData.refer_code)];
        playerData.email = data[nameof(playerData.email)];
        playerData.firstName = data[nameof(playerData.firstName)];
        playerData.lastName = data[nameof(playerData.lastName)];
        playerData.gender = data[nameof(playerData.gender)];
        playerData.state = data[nameof(playerData.state)];
        playerData.createdAt = RemoveQuotes(data[nameof(playerData.createdAt)].ToString());
        playerData.countryCode = data[nameof(playerData.countryCode)];

        string getName = data[nameof(playerData.dob)];
        if (getName == "" || getName == null)
        {
            playerData.dob = "none";
        }
        else
        {
            playerData.dob = RemoveQuotes(data[nameof(playerData.dob)]);
        }
        playerData.panNumber = data[nameof(playerData.panNumber)];
        playerData.membership = "free";
        //DataManager.Instance.playerData.membership = data[nameof(DataManager.Instance.playerData.membership)];
        playerData.avatar = GetAvatarValue();
        playerData.refer_count = data[nameof(playerData.refer_count)];
        playerData.refrer_level = data[nameof(playerData.refrer_level)];
        playerData.refrer_amount_total = data[nameof(playerData.refrer_amount_total)];

        playerData.refer_lvl1_count = data[nameof(playerData.refer_lvl1_count)];
        playerData.refer_vip_count = data[nameof(playerData.refer_vip_count)];
        playerData.refer_deposit_count = data[nameof(playerData.refer_deposit_count)];


        if (CheckNullOrEmpty(GetDefaultPlayerName()) && CheckNullOrEmpty(playerData.firstName))
        {
            print("Sub String : ");
            DataManager.Instance.SetDefaultPlayerName(DataManager.Instance.playerData.phone.Substring(0, 5));
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }
        else if (CheckNullOrEmpty(playerData.firstName))
        {
            DataManager.Instance.playerData.firstName = DataManager.Instance.GetDefaultPlayerName();
        }


        if (TeenPattiManager.Instance != null)
        {
            TeenPattiManager.Instance.DisplayCurrentBalance();
        }
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.coinTxt.text = playerData.balance.ToString();
        } 
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.bonusTxt.text = playerData.bonus.ToString();
        }
        if (RouletteManager.Instance != null)
        {
            RouletteManager.Instance.UpdateNameBalance();
        }
        if (CarRouletteScript.Instance != null)
        {
            CarRouletteScript.Instance.UpdateNameBalance();
        }
        if (DragonTigerManager.Instance != null)
        {
            DragonTigerManager.Instance.UpdateNameBalance();
        }
        if (AndarBaharManager.Instance != null)
        {
            AndarBaharManager.Instance.UpdateNameBalance();
        }
        if (PokerGameManager.Instance != null)
        {
            PokerGameManager.Instance.DisplayCurrentBalance();
        }
        if (SevenUpDownManager.Instance != null)
        {
            SevenUpDownManager.Instance.UpdateBalance();
        }
        if (AK47Manager.Instance != null)
        {
            AK47Manager.Instance.DisplayCurrentBalance();
        }
        if (JokerManager.Instance != null)
        {
            JokerManager.Instance.DisplayCurrentBalance();
        }
        if (AviatorGameManager.Instance != null)
        {
            AviatorGameManager.Instance.UpdateBalance();
        } 
        if (SpinAndWinManager.Instance != null)
        {
            SpinAndWinManager.Instance.UpdateNameBalance();
        }
    }

    public bool CheckNullOrEmpty(string value)
    {
        return value == null || value.Length == 0;
    }
    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }

    public void SetLoginEmail(int no)
    {
        PlayerPrefs.SetInt("LoginEmailCheck", no);
    }


    public int GetLoginEmail()
    {
        return PlayerPrefs.GetInt("LoginEmailCheck", 0);
    }

    #region Recently Manage

    public void SetRecent1(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent1", gameName);
    }


    public string GetRecent1()
    {
        return PlayerPrefs.GetString("GameNameRecent1", "none");
    }

    public void SetRecent2(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent2", gameName);
    }


    public string GetRecent2()
    {
        return PlayerPrefs.GetString("GameNameRecent2", "none");
    }

    public void SetRecent3(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent3", gameName);
    }


    public string GetRecent3()
    {
        return PlayerPrefs.GetString("GameNameRecent3", "none");
    }

    public void SetRecent4(string gameName)
    {
        PlayerPrefs.SetString("GameNameRecent4", gameName);
    }


    public string GetRecent4()
    {
        return PlayerPrefs.GetString("GameNameRecent4", "none");
    }
    #endregion

    #endregion

    public IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            if (image != null)
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
                image.color = new Color(255, 255, 255, 255);
            }
        }
    }
}
[System.Serializable]
public class PlayerData
{
    public string role;
    public string balance;
    public string kycStatus;
    public string wonCount;
    public string joinCount;
    public string deposit;
    public string winings;
    public string bonus;
    public string _id;
    public string phone;
    public string status;
    public string countryCode;
    public string createdAt;
    public string deviceType;
    public string aadharNumber;
    public string country;
    public string dob;
    public string email;
    public string firstName;
    public string lastName;
    public string gender;
    public string panNumber;
    public int avatar;
    public string refer_code;
    public string membership;
    public string state;
    public string refer_count;
    public string refrer_level;
    public string refrer_amount_total;
    public string refer_lvl1_count;
    public string refer_vip_count;
    public string refer_deposit_count;
}
[System.Serializable]
public class JoinPlayerData
{
    public string userId;
    public string userName;
    public string balance;
    public string lobbyId;
    public int playerNo;
    public string avtar;
    public string pPicture;
}
