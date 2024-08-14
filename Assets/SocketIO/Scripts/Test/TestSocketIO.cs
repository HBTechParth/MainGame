using System.Collections;
using UnityEngine;
using SocketIO;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using WebSocketSharp;
using System;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;
//using MoreMountains.NiceVibrations;

public class TestSocketIO : MonoBehaviour
{
    private SocketIOComponent socket;
    public static TestSocketIO Instace;
    public GameObject connectServerObj;
    public GameObject sessionRestoreObj;
    public string roomid;
    public string userdata;
    public float playTime;
    public bool isSocketError;
    public string lobbyId;
    public bool rummyJoinRoomTimeOver = false;

    [Header("--- Game Play Maintain ---")]
    public int teenPattiRequirePlayer;
    public int pointRummyRequirePlayer;
    public int pokerRequirePlayer;
    public int andarBaharRequirePlayer;
    public int dragonTigerRequirePlayer;
    public int sevenUpDownRequirePlayer;
    public int rouletteRequirePlayer;
    public int ludoRequirePlayer;
    

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Instace = this;
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        DontDestroyOnLoad(go);
        socket.On("open", TestOpen);
        socket.On("error", TestError);
        socket.On("close", TestClose);
        socket.On("res", HandelEvents);
    }

    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received===::: " + e.name + " " + e.data);
        // if(DataManager.Instance.isSocketError)
        // {
        //     DataManager.Instance.OpenSeesionRestore();
        // }
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received===::: " + e.name + " " + e.data);
        // if(isSocketError == false && DailyReward.Instance!=null) 
        // {
        //     connectServerObj.SetActive(true);
        //     isSocketError = true;
        //     // DataManager.Instance.OpenConnectServer();
        // }
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received===::: " + e.name + " " + e.data);
    }

    public void HandelEvents(SocketIOEvent data)
    {
        string ev = "";
        print(data);
        JSONNode values = JSON.Parse(data.data.ToString());
        ev = values["ev"].Value.ToString().Trim('"');
        //Debug.Log("Events===:::" + ev.ToString());

        switch (ev)
        {
            case "join":
                setroomid(data.data.ToString());
                break;
                //case "gameStart":
                //setroomid(data.data.ToString());
                //break;

            case "lobbyStat":
                LobbyStatusUpdate(data.data.ToString());
                break;

            //Ludo
            case "LudoData":
                SetLudoData(values.ToString());
                break;
            case "LudoDiceData":
                SetLudoDiceData(values.ToString());
                break;
            case "DecreaseLife":
                SetDecreaseLife(values.ToString());
                break;

            case "LudoDiceStopData":
                SetLudoStopDiceData(values.ToString());
                break;
            case "LudoDiceChangeData":
                SetLudoChangeDiceData(values.ToString());
                break;
            case "LudoDiceChangeDataBot":
                SetLudoDiceChangeDataBot(values.ToString());
                break;
            case "PauseSend":
                ReceivePauseRequest(values.ToString());
                break;
            case "ResumeSend":
                ReceiveResumeRequest(values.ToString());
                break;
            case "PauseRequest":
                PausePlayerRequest(values.ToString());
                break;
            case "GetUserPauseData":
                PauseGetPlayerData(values.ToString());
                break;
            case "leave":
                ResetRole(data.data.ToString());
                break;
            case "disconnect":
                //if (DataManager.Instance.gameMode != GameType.Point_Rummy)
                    ResetRole(data.data.ToString());
                break;
            case "moveuser":
                ResetRole(data.data.ToString());
                break;

            // Other
            case "setGameData":
                GetRoomData(data.data.ToString());
                break;

            case "SendChatMessage":
                SendChatMessageManage(values.ToString());
                break;

            case "SendGiftMessage":
                SendGiftMessageManage(values.ToString());
                break;

            case "setGameId":
                HandelSetGameId(values.ToString());
                break;

            // Teen Patti & Joker & AK47 & Point Rummy

            case "TeenPattiChangeTurnData":
                SetChangeTeenPatti(values.ToString());
                break;

            case "TeenPattiBotBetNo":
                SetBotBetTeenPatti(values.ToString());
                break;

            case "TeenPattiChangeCardStatus":
                SetChangeStatusTeenPatti(values.ToString());
                break;

            case "TeenPattiSendBetData":
                SetBetTeenPatti(values.ToString());
                break;

            case "TeenPattiSlideShowData":
                SetSlideShowTeenPatti(values.ToString());
                break;

            case "TeenPattiWinnerData":
                HandelWinTeenPatti(values.ToString());
                break;

            //Roulette

            case "RouletteSendBetData":
                SetBetRoulette(values.ToString());
                break;
            case "FindDataRouletteAdmin":
                FindRouletteData(values.ToString());
                break;
            case "SendAdminRouleteeData":
                SendAdminRouleteeData(values.ToString());
                break;
            case "getBetData":
                HandleGetData(values.ToString());
                break;

            //CarRoulette

            case "CarRouletteSendBetData":
                Debug.Log("<color=red> IN CarRouletteSendBetData </color>");
                SetBetCarRoulette(values.ToString());
                break;
            case "FindDataCarRouletteAdmin":
                Debug.Log("<color=red> IN FindDataCarRouletteAdmin </color>");
                FindCarRouletteData(values.ToString());
                break;
            case "SendAdminCarRouleteeData":
                Debug.Log("<color=red> IN SendAdminCarRouleteeData </color>");
                SendAdminCarRouletteData(values.ToString());
                break;
            case "getCarRouletteBetData":
                Debug.Log("<color=red> IN getCarRouletteBetData </color>");
                HandleGetCarData(values.ToString());
                break;

            //AndarBahar

            case "AndarBaharBetData":
                SetBetAndarBahar(values.ToString());
                break;
            case "AndarBaharTempNo":
                SetTempNoAndarBahar(values.ToString());
                break;

            // Dragon Tiger

            case "SendDragonTigerBet":
                SetBetDragonTiger(values.ToString());
                break;
            case "SendDeckBet":
                SetGetDeckData(values.ToString());
                break;
            case "setWinListData":
                SetWinData(values.ToString());
                break;


            // Poker

            case "PokerChangeTurnData":
                SetChangePoker(values.ToString());
                break;
            case "PokerSendBetData":
                SetBetPoker(values.ToString());
                break;
            case "PokerBotBetNo":
                SetBotBetPoker(values.ToString());
                break;
            case "PokerSendFlodData":
                SetFoldPoker(values.ToString());
                break;
            case "PokerWinnerData":
                SetWinPoker(values.ToString());
                break;
            case "PokerFinalWinnerData":
                HandelWinPoker(values.ToString());
                break;

            //7 Up Down

            case "SendDiceData":
                SetDiceData(values.ToString());
                break;
            case "SendSevenUpDownBet":
                SetSevenUpDownBet(values.ToString());
                break;
            case "SetWinDataForSevenUpDown":
                SetWinDataForSevenUpDown(values.ToString());
                break;

            //Jhandi Munda

            case "SendDiceDataJhandiMunda":
                SetDiceDataJhandiMunda(values.ToString());
                break;
            case "JhandiMundaBet":
                SetJhandiMundaBet(values.ToString());
                break;
            case "SetWinDataForJhandiMunda":
                SetWinDataJhandiMunda(values.ToString());
                break;

            // Rummy


            case "CardDrawnFromClosedDeck":
                SetDrawCardFromClosedDeck(values.ToString());
                break;
            case "CardDrawnFromDiscardPile":
                SetDrawCardFromDiscardPile(values.ToString());
                break;
            case "CardDiscarded":
                SetDiscardCard(values.ToString());
                break;
                case "RequestShow":
                CallArrangeShowPopUp(values.ToString());
                break;
                case "SubmitShow":
                HandleWinScreen(values.ToString());
                break;
                case "ReturnCardsToDeck":
                ReturnCardsToDeck(values.ToString());
                break;

                /*// snake & ladder

                case "gameStart":
                    GameStart(values.ToString());
                    break;
                case "SnakeDiceData":
                    SetSnakeDiceData(values.ToString());
                    break;
                case "SnakeData":
                    SetSnakeData(values.ToString());
                    break;
                case "SnakeDiceChangeData":
                    SetSnakeChangeDiceData(values.ToString());
                    break;*/
        }
    }
    public void LudoJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", DataManager.Instance.isTwoPlayer ? 2 : 4);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));
        userdata.AddField("isBot", "0");
        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
        
        print("This is the data -> " + userdata);
    }

    public void BotJoinLudoRoom(string userId, string name, string lobbyID, string balance, string avtar)
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", userId);


        userdata.AddField("name", name);

        userdata.AddField("balance", balance);
        userdata.AddField("lobbyId", lobbyID);
        userdata.AddField("maxp", DataManager.Instance.isTwoPlayer ? 2 : 4);
        userdata.AddField("avtar", avtar);
        userdata.AddField("isBot", "1");
        Debug.Log("join event fired");
        //if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        //{
        socket.Emit("joinbot", userdata);
        //}
    }

    public void TeenPattiJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 5);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }

    public void RummyJoinRoom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 6);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }
    
    public void SnakeJoinRoom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 2);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }

    public void RouletteJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("maxp", 99999);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }
    public void AndarBaharJoinroom()
    {
        JSONObject userdata = new JSONObject();
        userdata.AddField("userId", DataManager.Instance.playerData._id.ToString().Trim('"'));

        if (DataManager.Instance.playerData.firstName == null)
        {
            userdata.AddField("name", DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        }
        else
        {
            userdata.AddField("name", DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        }
        userdata.AddField("balance", DataManager.Instance.playerData.balance);
        userdata.AddField("lobbyId", DataManager.Instance.tournamentID);
        userdata.AddField("activeRoom", DataManager.Instance);
        userdata.AddField("maxp", 8);
        userdata.AddField("avtar", PlayerPrefs.GetString("ProfileURL"));

        if (DataManager.Instance.CheckRoomUser(DataManager.Instance.playerData._id.ToString().Trim('"')) == true)
        {
            socket.Emit("join", userdata);
        }
    }

    public void Senddata_Btn()
    {
        JSONObject data = new JSONObject();
        data.AddField("msg", "Hello");
        Senddata("msg", data);
    }

    public void Senddata(string ev, JSONObject data)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        keys.AddField("ev", ev);
        keys.AddField("data", data);
        print("<color=yellow>Data sent = \n" + keys.ToString() + "</color>");
        //Debug.Log("SendData===:::" + keys.ToString());
        socket.Emit("sendToRoom", keys);
    }
    public void SetBetData(int chipNo, string betAmount, string manyBets, string type)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("action", type);
        keys.AddField("room", roomid);
        keys.AddField("amount", int.Parse(betAmount));
        keys.AddField("betNo", chipNo);
        if (chipNo > 36)
        {
            keys.AddField("manyBet", manyBets);
        }
        print("This is sending data -> " + keys);
        socket.Emit("setBetData", keys);
    }
    
    public void GetBetData()
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        print("SendData Form GETBETDATA===:::" + keys);
        socket.Emit("getBetData", keys);
    }
    
    public void GetCarBetData()
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        print("SendData Form GETBETDATA===:::" + keys);
        socket.Emit("getCarRouletteBetData", keys);
    }


    public void RummyLoadScene()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;

        if (activeSceneName != "Main" || !rummyJoinRoomTimeOver)
            return;

        rummyJoinRoomTimeOver = false;

        switch (DataManager.Instance.gameMode)
        {
            case GameType.Point_Rummy:
            case GameType.Pool_Rummy:
            case GameType.Deal_Rummy:
                if (DataManager.Instance.joinPlayerDatas.Count == 1)
                {
                    LeaveRoom();
                    MainMenuManager.Instance.GenerateNoPlayersFound();
                    return;
                }
                break;
            case GameType.Teen_Patti:
            case GameType.Teen_Patti_AK47:
            case GameType.Joker:
            case GameType.Poker:
                MainMenuManager.Instance.CheckPlayers();
                if (DataManager.Instance.joinPlayerDatas.Count >= teenPattiRequirePlayer)
                {
                    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                    return;
                }
                break;
            case GameType.Ludo:
                //MainMenuManager.Instance.LoadLudoBotPlayers();

                if ((DataManager.Instance.isTwoPlayer && DataManager.Instance.joinPlayerDatas.Count >= 2) ||
                    (DataManager.Instance.isFourPlayer && DataManager.Instance.joinPlayerDatas.Count >= 4))
                {
                    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
                    return;
                }
                break;
        }

        MainMenuManager.Instance.GenerateNoPlayersFound();
    }

    //bool isGenerate = false;


    public void LobbyStatusUpdate(string data)
    {
        
        if (SceneManager.GetActiveScene().name == "Main")
        {
            print("Lobby stat called " + data.ToString());
            JSONNode values = JSON.Parse(data);
            JSONNode obj = JSON.Parse(values.ToString());
            string lobbystat = obj["lobbyId"];
            if (DataManager.Instance.joinPlayerDatas.Find(x => x.userId == DataManager.Instance.playerData._id).lobbyId == lobbystat)
                MainMenuManager.Instance.secondsCount = 10f;


        }
    }
    public void StartGameData()
    {
        //JSONObject obj = new JSONObject();
        //obj.AddField("userId", DataManager.Instance.playerData._id);
        //obj.AddField("roomId", TestSocketIO.Instace.roomid);
        //obj.AddField("tournamentId", DataManager.Instance.tournamentID);
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        keys.AddField("lobbyId", DataManager.Instance.tournamentID);
        keys.AddField("userId", DataManager.Instance.playerData._id);
        Debug.Log("SendData===:::" + keys);
        if (DataManager.Instance.joinPlayerDatas.First().userId == DataManager.Instance.playerData._id)
        {
            socket.Emit("gameStart", keys);
            //TestSocketIO.Instace.Senddata("gameStart", obj);
        }
    }

    public void setroomid(string data)
    {
        print("Room Data recevied : " + data);
        //print("Join Player");
        JSONNode values = JSON.Parse(data);
        JSONNode obj = JSON.Parse(values["data"].ToString());
        roomid = obj["roomName"].Value.ToString();

        //print("Room Name : " + roomid);
        if (SceneManager.GetActiveScene().name == "Main")
        {
            userdata = obj["users"].ToString();
        }
        else
        {
            userdata = obj["users"].ToString();
        }

        if (obj["users"][0]["maxp"] == 5)
        {
            //Teen Patti
            /*if (DataManager.Instance.joinPlayerDatas.Count >= 5 && !TeenPattiManager.Instance.isGameStarted)
            {
                DataManager.Instance.joinPlayerDatas.Clear();
                TeenPattiManager.Instance.ResetBot();
                PokerGameManager.Instance.ResetBot();
            }*/
            

            //print("teen Pattoi : " + );
            if (SceneManager.GetActiveScene().name == "Main")
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null && obj["users"].Count < 6)
                    {
                        DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                    }
                }
            }
            if (SceneManager.GetActiveScene().name == "TeenPatti")//obj["users"].Count >= teenPattiRequirePlayer && 
            {
                if (TeenPattiManager.Instance != null)
                {
                    TeenPattiManager.Instance.PlayerFound();
                }
            }
            //if (DataManager.Instance.joinPlayerDatas.Count > 1)
            //{
            //    var playerId = DataManager.Instance.playerData._id; // player ID to match
            //    var joinPlayerDatas = DataManager.Instance.joinPlayerDatas; // list of join player data
            //    joinPlayerDatas.RemoveAll(joinPlayerData => joinPlayerData.userId != playerId);
            //}
            if (SceneManager.GetActiveScene().name == "Poker")//obj["users"].Count >= pokerRequirePlayer && 
            {
                if (PokerGameManager.Instance != null)
                {
                    PokerGameManager.Instance.PlayerFound();
                }
            }





            //if (obj["users"].Count >= teenPattiRequirePlayer && SceneManager.GetActiveScene().name == "Main")
            //{
            //    //Open Scene
            //    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            //}

            //if (SceneManager.GetActiveScene().name == "Main")
            //{
            //    SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));

            //}

            /*if (SceneManager.GetActiveScene().name == "TeenPatti")//obj["users"].Count >= teenPattiRequirePlayer && 
            {
                if (TeenPattiManager.Instance != null)
                {
                    TeenPattiManager.Instance.PlayerFound();
                }
            }*/
        }
        if (obj["users"][0]["maxp"] == 6)
        {
            //Point Rummy
            obj = JSON.Parse(values["data"]["users"].ToString());
            if (SceneManager.GetActiveScene().name == "Main")
            {



                for (int i = 0; i < obj.Count; i++)
                {
                    //int pNo = 0;


                    //if (i == 0)
                    //{
                    //    pNo = 1;
                    //}
                    //else if (i == 1)
                    //{
                    //    pNo = 2;
                    //}
                    //else if (i == 2)
                    //{
                    //    pNo = 3;
                    //}
                    //else if (i == 3)
                    //{
                    //    pNo = 4;
                    //}


                    print("trying to add player : " + values["data"]["users"][i]["name"]);
                    if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null && obj.Count < 7)
                    {
                        DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                    }
                }
            }

            //if (DataManager.Instance.joinPlayerDatas.Count > 1)
            //{
            //    var playerId = DataManager.Instance.playerData._id; // player ID to match
            //    var joinPlayerDatas = DataManager.Instance.joinPlayerDatas; // list of join player data
            //    joinPlayerDatas.RemoveAll(joinPlayerData => joinPlayerData.userId != playerId);
            //}
            // if there is 3 players then bot is disabled 
            //if (DataManager.Instance.joinPlayerDatas.Count <= 4)
            //{
            //    MainMenuManager.Instance.CheckPlayers();
            //}
            

            
        }

        if (obj["users"][0]["maxp"] == 99999)
        {
            //Dragon Tiger

            //print("teen Pattoi : " + );
            for (int i = 0; i < obj.Count; i++)
            {
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                    
                }
            }
            //if (obj["users"].Count )
            //{
            //Open Scene

            if (SceneManager.GetActiveScene().name == "Main")
            {
                //StartGameData();
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }
            if (obj["users"].Count >= dragonTigerRequirePlayer && SceneManager.GetActiveScene().name == "DragonTiger")
            {
                if (DragonTigerManager.Instance != null)
                {
                    DragonTigerManager.Instance.OpenScene();
                }
            }
            if (obj["users"].Count >= rouletteRequirePlayer && SceneManager.GetActiveScene().name == "Rouletee")
            {
                if (RouletteManager.Instance != null)
                {
                    RouletteManager.Instance.NewPlayerEnter();
                }
            }
            if (obj["users"].Count >= rouletteRequirePlayer && SceneManager.GetActiveScene().name == "CarRoulette")
            {
                if (RouletteManager.Instance != null)
                {
                    CarRouletteScript.Instance.NewPlayerEnter();
                }
            }
            if(obj["users"].Count >= sevenUpDownRequirePlayer && SceneManager.GetActiveScene().name == "7UpDown")
            {
                if (SevenUpDownManager.Instance != null)
                {
                    SevenUpDownManager.Instance.SetUpPlayers();
                }
            }
            if(obj["users"].Count >= sevenUpDownRequirePlayer && SceneManager.GetActiveScene().name == "JhandiMunda")
            {
                if (JhandiMundaManager.Instance != null)
                {
                    JhandiMundaManager.Instance.SetUpPlayers();
                }
            }
            
            // for dragon tiger
            string winData = obj["WinList"]["WinList"];
            DataManager.Instance.listString = winData;
            
            // for Aviator
            string winPoints = obj["WinList"]["PointList"];
            DataManager.Instance.historyPoints = winPoints;
            
            // for roulette
            string winRecord = obj["gameData"]["WinList"];
            DataManager.Instance.winRecord = winRecord;
            
            // for Car roulette
            string historyRecord = obj["gameData"]["WinList"];
            DataManager.Instance.historyRecord = historyRecord;

            // if (roomid.Equals(obj["room"]))
            // {
            //     string winData = obj["gameData"]["WinList"];
            //     DragonTigerManager.Instance.HistoryLoader(winData);
            //     print("This is Windata -> " + winData);
            // }
            //}
        }
        if (obj["users"][0]["maxp"] == 8)
        {
            //Andar Bahar

            //print("teen Pattoi : " + );
            for (int i = 0; i < obj.Count; i++)
            {
                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null && values["data"]["users"][i]["lobbyId"] != null)
                {
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"], values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"], values["data"]["users"][i]["balance"], i, values["data"]["users"][i]["avtar"]);
                }
            }

            if (SceneManager.GetActiveScene().name == "Main")
            {
                StartGameData();
                SceneManager.LoadScene(DataManager.Instance.GetModeToSceneName(DataManager.Instance.gameMode));
            }
            if (obj["users"].Count >= andarBaharRequirePlayer && SceneManager.GetActiveScene().name == "AndarBahar")
            {
                if (AndarBaharManager.Instance != null)
                {
                    AndarBaharManager.Instance.PlayerFound();
                }
            }
            
            string winData = obj["gameData"]["WinList"];
            print("This is receving Win data - > " + winData);
            DataManager.Instance.winList = winData;

        }

        else if (obj["users"][0]["maxp"] == 2 || obj["users"][0]["maxp"] == 4)
        {
            if (DataManager.Instance.playerNo == 0)
            {
                if (obj["users"][0]["maxp"] == 2)
                {
                    DataManager.Instance.isTwoPlayer = true;
                    DataManager.Instance.playerNo = obj["users"].Count;

                    if (DataManager.Instance.playerNo == 2) DataManager.Instance.playerNo = 3;

                    if (DataManager.Instance.modeType == 4 && DataManager.Instance.playerNo == 3)
                        DataManager.Instance.playerNo = 2;
                }
                else if (obj["users"][0]["maxp"] == 4)
                {
                    print("Enter The First Player  Con");
                    if (obj["users"][0]["maxp"] == 4)
                    {
                        DataManager.Instance.isFourPlayer = true;
                        DataManager.Instance.playerNo = obj["users"].Count;
                    }
                }
            }

            var count = 0;
            if (DataManager.Instance.isTwoPlayer)
                count = obj.Count;
            else if (DataManager.Instance.isFourPlayer)
                count = 5;
            for (var i = 0; i < count; i++)
            {
                var pNo = 0;
                if (DataManager.Instance.isTwoPlayer)
                {
                    if (i == 0)
                        pNo = 1;
                    else if (i == 1) pNo = 3;
                }
                else
                {
                    pNo = i + 1;
                }

                if (DataManager.Instance.modeType == 4) // For Carrom
                {
                    if (i == 0)
                        pNo = 1;
                    else
                        pNo = 2;
                }

                if (values["data"]["users"][i]["name"] != null && values["data"]["users"][i]["userId"] != null &&
                    values["data"]["users"][i]["lobbyId"] /*lobbyId*/ != null &&
                    values["data"]["users"][i]["lobbyId"] == DataManager.Instance.tournamentID)
                    DataManager.Instance.AddRoomUser(values["data"]["users"][i]["userId"],
                        values["data"]["users"][i]["name"], values["data"]["users"][i]["lobbyId"],
                        values["data"]["users"][i]["balance"], pNo, values["data"]["users"][i]["avtar"]);
            }
            

            if (SceneManager.GetActiveScene().name != "Ludo") return;
            if (LudoManager.Instance == null) return;
            LudoManager.Instance.PlayerJoined();
        }
        
    }

    #region Snake&Ladder
    
    public void StartGameBot(string roomId, string tourId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("room", roomId);
        obj.AddField("lobbyId", tourId);
        obj.AddField("userId", DataManager.Instance.playerData._id.Trim('"'));
        socket.Emit("gameStart", obj);
    }
    
    public void SetSnakeDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Snake")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            //int DiceManageCnt = data["DiceManageCnt"];
            int diceNo = data["DiceNo"];
            string sRoomId = data["RoomId"];
            if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
            {
                SnakeManager.Instance.AutoDice(diceNo, DataManager.Instance.playerNo, playerNo);
            }
        }
    }
    
    public void SetSnakeChangeDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Snake")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            print("Snake : " + values);
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];


            if (!DataManager.Instance.playerData._id.Equals(playerId))
            {
                if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo == DataManager.Instance.playerNo)
                {
                    SoundManager.Instance.UserTurnSound();
                    if (DataManager.Instance.GetVibration() == 0)
                    {
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            //MMNVAndroid.AndroidVibrate(100);
                        }
                    }
                    SnakeManager.Instance.TimerSliderColorChange();

                    DataManager.Instance.isDiceClick = true;

                    SnakeManager.Instance.isClickAvaliableDice = 0;
                    DataManager.Instance.isTimeAuto = false;
                    SnakeManager.Instance.RestartTimer();


                }

            }
        }
    }
    
    public void SetSnakeData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Snake")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int tokenNo = data["TokenNo"];
            int tokenMove = data["TokenMove"];
            int playerNo = data["PlayerNo"];

            string sRoomId = data["RoomId"];


            if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
            {
                SnakeManager.Instance.AutoMove(playerNo, tokenNo, tokenMove);
            }
        }

    }

    
    public string RemoveQuotes(string s)
    {
        string str = s;
        string newstr = str.Replace("\"", "");
        return newstr;
    }
    
    public void GameStart(string values)
    {
        JSONNode keys = JSON.Parse(values.ToString());
        JSONNode users = JSON.Parse(keys["data"]["users"].ToString());
        string room = RemoveQuotes(keys["data"]["room"].ToString());
        string lobbyId = RemoveQuotes(keys["data"]["lobbyId"].ToString());
        string userID = keys["data"]["userId"].ToString();


        //print("Org Id 1: " + lobbyId);
        //print("Org Id 2: " + DataManager.Instance.tournamentID);

        //print("roomid 1: " + roomid);
        //print("roomid 2: " + room);
        if (DataManager.Instance.tournamentID.Equals(lobbyId) && roomid.Equals(room))
        {
            //print("Users Count : " + users.Count);
            if (SnakeGameLoading.Instance != null)
            {
                SnakeGameLoading.Instance.isTwoPlayerReady = users.Count == 2;
            }
        }
    }

    
    

    #endregion
    
    

    

    #region Ludo
    public void SetLudoData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int tokenNo = data["TokenNo"];
            int tokenMove = data["TokenMove"];
            int playerNo = data["PlayerNo"];

            string sRoomId = data["RoomId"];


            //if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
            //{
            //    LudoManager.Instance.AutoMove(playerNo, tokenNo, tokenMove);
            //}
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (DataManager.Instance.isTwoPlayer && !playerId.Contains("Ludo"))
                {
                    if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                    {
                        //print("Move Player No : " + playerNo);
                        //print("Move Token No : " + tokenNo);
                        //print("Move Token Move : " + tokenMove);
                        LudoManager.Instance.AutoMove(playerNo, tokenNo, tokenMove);
                    }
                }
                else if (playerId.Contains("Ludo") ? LudoManager.Instance.isAdmin == false && DataManager.Instance.isTwoPlayer == false : DataManager.Instance.isTwoPlayer == false)
                {
                    if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                    {
                        //print("Move Player No : " + playerNo);
                        //print("Move Token No : " + tokenNo);
                        //print("Move Token Move : " + tokenMove);
                        Debug.Log("Calling AutoMove for bot");
                        LudoManager.Instance.AutoMove(playerNo, tokenNo, tokenMove);
                    }
                }
            }
        }

    }
    public void SetLudoDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            print("diceData = \n" + value.ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            //int DiceManageCnt = data["DiceManageCnt"];
            int diceNo = data["DiceNo"];
            string sRoomId = data["RoomId"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                //{
                //    LudoManager.Instance.AutoDice(diceNo, DataManager.Instance.playerNo, playerNo);
                //}
                if (DataManager.Instance.isTwoPlayer && !playerId.Contains("Ludo"))
                {
                    if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                    {
                        Debug.Log("Calling AutoDice");
                        LudoManager.Instance.AutoDice(diceNo, DataManager.Instance.playerNo, playerNo);
                    }
                }
                else if (playerId.Contains("Ludo") ? LudoManager.Instance.isAdmin == false && DataManager.Instance.isTwoPlayer == false : DataManager.Instance.isTwoPlayer == false && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
                {
                    if (DataManager.Instance.playerData._id != playerId && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                    {
                        Debug.Log("Calling AutoDice for bot no. = " + playerNo);
                        LudoManager.Instance.AutoDice(diceNo, DataManager.Instance.playerNo, playerNo);
                    }
                }
            }

        }
    }

    public void SetDecreaseLife(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            string sRoomId = data["RoomId"];
            string tourId = data["TournamentID"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (LudoManager.Instance.isAdmin == false && sRoomId == roomid && tourId == DataManager.Instance.tournamentID)
                {
                    DataManager.Instance.isDiceClick = true;//exception for isClickAvailableDice
                    LudoManager.Instance.LifeDecrease();
                }
            }
        }
    }

    [HideInInspector]
    public int changeCnt = 0;
    public void SetLudoChangeDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            print("changing dice : \n" + value.ToString());
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            int diceNo = data["DiceNo"];
            string sRoomId = data["RoomId"];

            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (DataManager.Instance.modeType == 3 && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
                {
                    if (DataManager.Instance.joinPlayerDatas.Last().userId == playerId && DataManager.Instance.playerData._id != DataManager.Instance.joinPlayerDatas.Last().userId)
                    {
                        print("1 complete round over ? moveCnt = " + LudoUIManager.Instance.moveCnt);
                        LudoUIManager.Instance.FirstNumberRemove();
                        LudoUIManager.Instance.UpdateBottomDropDown();
                    }
                    else if (DataManager.Instance.playerData._id == playerId && DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas.Last().userId)
                    {
                        print("1 complete round over for last player? moveCnt = " + LudoUIManager.Instance.moveCnt);
                        if (!BotManager.Instance.isConnectBot)
                        {
                            LudoUIManager.Instance.FirstNumberRemove();
                            LudoUIManager.Instance.UpdateBottomDropDown();
                        }
                    }
                    if (LudoUIManager.Instance.moveCnt == 24)
                    {
                        LudoManager.Instance.WinUserShow();
                    }
                }

                if (DataManager.Instance.playerData._id != playerId)//playerId is other player's id not our id
                {

                    //print("Data Player Id : " + DataManager.Instance.playerData._id);
                    //print("Get Player Id : " + playerId);
                    //print("Player No : " + playerNo);
                    if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo == DataManager.Instance.playerNo && DataManager.Instance.modeType == 3 && DataManager.Instance.isTwoPlayer)
                    {
                        SoundManager.Instance.UserTurnSound();
                        if (DataManager.Instance.GetVibration() == 0)
                        {
                            if (Application.platform == RuntimePlatform.Android)
                            {
                                
                                MMNVAndroid.AndroidVibrate(100);
                            }
                        }

                        DataManager.Instance.isDiceClick = true;

                        LudoManager.Instance.isClickAvaliableDice = 0;
                        LudoManager.Instance.OurShadowMaintain();
                        DataManager.Instance.isTimeAuto = false;
                        LudoManager.Instance.RestartTimer();
                        if (DataManager.Instance.modeType == 3)
                        {
                            LudoManager.Instance.DiceLessPasaButton();
                        }
                        return;
                    }
                    if (DataManager.Instance.playerData._id != playerId)
                    {
                        if (DataManager.Instance.isFourPlayer && DataManager.Instance.playerData._id != playerId && !playerId.Contains("Ludo") && LudoManager.Instance.isAdmin == false/*&& LudoManager.Instance.currentPlayerNo != LudoManager.Instance.playerRoundChecker*/)
                            LudoManager.Instance.ChangeTurn();
                        Debug.Log("roundChecker = " + LudoManager.Instance.playerRoundChecker);
                        if (playerId.Contains("Ludo") && LudoManager.Instance.isAdmin && DataManager.Instance.isFourPlayer)
                            LudoManager.Instance.BotChangeTurn(false, true);//uncomment if there is more than one player
                        else if (!playerId.Contains("Ludo") && LudoManager.Instance.isAdmin && DataManager.Instance.isFourPlayer)
                        {
                            LudoManager.Instance.ChangeTurn();
                            LudoManager.Instance.OurShadowMaintain();
                            DataManager.Instance.isTimeAuto = false;
                            LudoManager.Instance.RestartTimer();
                            if (DataManager.Instance.modeType == 3)
                            {
                                DataManager.Instance.isDiceClick = true;
                                LudoManager.Instance.isClickAvaliableDice = 0;
                                LudoManager.Instance.isCheckEnter = false;
                                LudoManager.Instance.isPathClick = true;
                                LudoManager.Instance.isPathClickAvaliable = false;
                                //LudoManager.Instance.DiceLessPasaButton();
                            }
                        }
                    }
                    if (DataManager.Instance.isTwoPlayer && !playerId.Contains("Ludo"))
                    {
                        if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo == DataManager.Instance.playerNo)
                        {
                            SoundManager.Instance.UserTurnSound();
                            if (DataManager.Instance.GetVibration() == 0)
                            {
                                if (Application.platform == RuntimePlatform.Android)
                                {
                                    //MMNVAndroid.AndroidVibrate(100);
                                }
                            }
                            print("This is clicked in Testsocket");
                            DataManager.Instance.isDiceClick = true;
                            LudoManager.Instance.isClickAvaliableDice = 0;
                            LudoManager.Instance.OurShadowMaintain();
                            DataManager.Instance.isTimeAuto = false;
                            LudoManager.Instance.RestartTimer();
                            //if (DataManager.Instance.modeType == 3)
                            //{
                            //    LudoManager.Instance.DiceLessPasaButton();
                            //}
                        }
                    }
                    else if (playerId.Contains("Ludo") && LudoManager.Instance.isAdmin == false && DataManager.Instance.isTwoPlayer == false /*: DataManager.Instance.isTwoPlayer == false*/)
                    {
                        Debug.Log("Changed taking place in socket");
                        if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo == DataManager.Instance.playerNo)
                        {
                            SoundManager.Instance.UserTurnSound();
                            if (DataManager.Instance.GetVibration() == 0)
                            {
                                if (Application.platform == RuntimePlatform.Android)
                                {
                                    //MMNVAndroid.AndroidVibrate(100);
                                }
                            }
                            if (!playerId.Contains("Ludo"))
                            {

                                DataManager.Instance.isDiceClick = true;
                                LudoManager.Instance.isClickAvaliableDice = 0;

                            }
                            else
                            {
                                DataManager.Instance.isDiceClick = false;
                                LudoManager.Instance.isClickAvaliableDice = 1;
                            }
                            print("This is clicked in Testsocket");
                            LudoManager.Instance.OurShadowMaintain();
                            DataManager.Instance.isTimeAuto = false;
                            LudoManager.Instance.RestartTimer();
                            if (DataManager.Instance.modeType == 3)
                            {
                                //LudoManager.Instance.DiceLessPasaButton();
                            }
                        }
                    }
                    else if (!playerId.Contains("Ludo") && LudoManager.Instance.isAdmin == false)
                    {
                        Debug.Log("Changed taking place in socket");


                        SoundManager.Instance.UserTurnSound();
                        if (DataManager.Instance.GetVibration() == 0)
                        {
                            if (Application.platform == RuntimePlatform.Android)
                            {
                                //MMNVAndroid.AndroidVibrate(100);
                            }
                        }
                        if (!playerId.Contains("Ludo") && DataManager.Instance.playerNo == LudoManager.Instance.playerRoundChecker)
                        {
                            DataManager.Instance.isDiceClick = true;
                            LudoManager.Instance.isClickAvaliableDice = 0;
                        }
                        else
                        {
                            DataManager.Instance.isDiceClick = false;
                            LudoManager.Instance.isClickAvaliableDice = 1;
                        }
                        print("This is clicked in Testsocket");
                        LudoManager.Instance.OurShadowMaintain();
                        DataManager.Instance.isTimeAuto = false;
                        LudoManager.Instance.RestartTimer();
                        if (DataManager.Instance.modeType == 3)
                        {
                            LudoManager.Instance.DiceLessPasaButton();
                        }

                    }
                    else if (DataManager.Instance.isFourPlayer == true ? LudoManager.Instance.isAdmin && !playerId.Contains("Ludo") /*&& LudoManager.Instance.playerRoundChecker != 1*/ : false)
                    {
                        SoundManager.Instance.UserTurnSound();

                        if (!playerId.Contains("Ludo") && DataManager.Instance.playerNo == LudoManager.Instance.playerRoundChecker)
                        {
                            DataManager.Instance.isDiceClick = true;
                            LudoManager.Instance.isClickAvaliableDice = 0;
                        }
                        else
                        {
                            DataManager.Instance.isDiceClick = false;
                            LudoManager.Instance.isClickAvaliableDice = 1;
                        }
                        print("This is clicked in Testsocket");
                        LudoManager.Instance.OurShadowMaintain();
                        DataManager.Instance.isTimeAuto = false;
                        LudoManager.Instance.RestartTimer();
                        if (DataManager.Instance.modeType == 3)
                        {
                            LudoManager.Instance.DiceLessPasaButton();
                        }

                    }




                }

            }
        }
    }

    public void SetLudoDiceChangeDataBot(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            int diceNo = data["DiceNo"];
            string sRoomId = data["RoomId"];
            string playerRoundChecker = data["PlayerRoundChecker"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (DataManager.Instance.playerData._id != playerId)
                {
                    print("Player No : " + playerNo);
                    if (LudoManager.Instance.isAdmin == false)
                    {
                        if (DataManager.Instance.modeType == 3 && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
                        {
                            if (DataManager.Instance.joinPlayerDatas.Last().userId == playerId && DataManager.Instance.playerData._id != DataManager.Instance.joinPlayerDatas.Last().userId)
                            {
                                print("1 complete round over ? moveCnt = " + LudoUIManager.Instance.moveCnt);
                                LudoUIManager.Instance.FirstNumberRemove();
                                LudoUIManager.Instance.UpdateBottomDropDown();
                            }
                            else if (DataManager.Instance.playerData._id == playerId && DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas.Last().userId)
                            {
                                print("1 complete round over for last player? moveCnt = " + LudoUIManager.Instance.moveCnt);
                                LudoUIManager.Instance.FirstNumberRemove();
                                LudoUIManager.Instance.UpdateBottomDropDown();
                            }
                            if (LudoUIManager.Instance.moveCnt == 24)
                            {
                                LudoManager.Instance.WinUserShow();
                            }
                        }
                        LudoManager.Instance.playerRoundChecker = int.Parse(playerRoundChecker);
                        //LudoManager.Instance.BotChangeTurn(false, true);//uncomment if there is more than one player
                        LudoManager.Instance.OurShadowMaintain();
                        LudoManager.Instance.RestartTimer();
                        Debug.Log("roundChecker = " + LudoManager.Instance.playerRoundChecker);
                    }
                }
            }
        }
    }

    public void PausePlayerRequest(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (DataManager.Instance.playerData._id != playerId)
                {
                    print("Data Player Id : " + DataManager.Instance.playerData._id);
                    print("Get Player Id : " + playerId);
                    print("Player No : " + playerNo);
                    if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                    {
                        LudoManager.Instance.PauseUserDataSend();
                    }
                }
            }
        }
    }

    public void ReceivePauseRequest(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            int playerNo = data["PlayerNo"];
            //tourId == DataManager.Instance.tournamentID && sRoomId == roomid
            string sRoomId = data["RoomId"];
            string tourId = data["TournamentID"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (playerNo != -1)
                {
                    print("Admin is in pause mode. Temporarily switching admin to player 2");
                    if (DataManager.Instance.playerNo == playerNo + 1)
                    {
                        LudoManager.Instance.isAdmin = true;
                        LudoManager.Instance.isAdminPause = true;
                        if (DataManager.Instance.joinPlayerDatas.Any(x => x.userId.Contains("Ludo")))
                        {
                            BotManager.Instance.isConnectBot = true;
                            if (DataManager.Instance.joinPlayerDatas[LudoManager.Instance.playerRoundChecker - 1].userId.Contains("Ludo"))
                                LudoManager.Instance.GenerateDiceNumberStart_Bot(false);
                        }
                    }
                }
                else
                {
                    LudoManager.Instance.isOtherPlayerPause = true;
                }
            }

        }
    }

    public void ReceiveResumeRequest(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            string tourId = data["TournamentID"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (playerNo == 1)
                {
                    print("Admin has returned to game. Giving admin privileges to player 1");
                    if (DataManager.Instance.playerNo == playerNo + 1)
                    {
                        LudoManager.Instance.isAdmin = false;
                        LudoManager.Instance.isAdminPause = false;
                        BotManager.Instance.isConnectBot = false;
                        
                    }
                }
                else
                    LudoManager.Instance.isOtherPlayerPause = false;
            }
        }
    }

    Image[] SelectPasa(int updatedPlayerNo)
    {
        for (int i = 0; i < LudoManager.Instance.box1Token.Length; i++)
        {
            var pasa = LudoManager.Instance.box1Token[i].GetComponent<PasaManage>();
            if (updatedPlayerNo == pasa.updatedPlayerNo)
                return LudoManager.Instance.box1Token;
        }
        for (int i = 0; i < LudoManager.Instance.box2Token.Length; i++)
        {
            var pasa = LudoManager.Instance.box2Token[i].GetComponent<PasaManage>();
            if (updatedPlayerNo == pasa.updatedPlayerNo)
                return LudoManager.Instance.box2Token;
        }
        for (int i = 0; i < LudoManager.Instance.box3Token.Length; i++)
        {
            var pasa = LudoManager.Instance.box3Token[i].GetComponent<PasaManage>();
            if (updatedPlayerNo == pasa.updatedPlayerNo)
                return LudoManager.Instance.box3Token;
        }
        for (int i = 0; i < LudoManager.Instance.box4Token.Length; i++)
        {
            var pasa = LudoManager.Instance.box4Token[i].GetComponent<PasaManage>();
            if (updatedPlayerNo == pasa.updatedPlayerNo)
                return LudoManager.Instance.box4Token;
        }
        return LudoManager.Instance.box1Token;
    }

    private void LifeCheck(int parent, int lifeLost, int score)
    {
        switch (parent)
        {
            case 1:
            {
                LudoManager.Instance.cntPlayer1 = lifeLost;
                for (int i = 0; i < LudoManager.Instance.box1Lifes.Length; i++)
                {
                    if (i < LudoManager.Instance.cntPlayer1)
                        LudoManager.Instance.box1Lifes[i].color = LudoManager.Instance.lifeOffColor;
                }
                break;
            }
            case 2:
            {
                LudoManager.Instance.cntPlayer2 = lifeLost;
                for (int i = 0; i < LudoManager.Instance.box2Lifes.Length; i++)
                {
                    if (i < LudoManager.Instance.cntPlayer2)
                        LudoManager.Instance.box2Lifes[i].color = LudoManager.Instance.lifeOffColor;
                }
                break;
            }
            case 3:
            {
                LudoManager.Instance.cntPlayer3 = lifeLost;
                for (int i = 0; i < LudoManager.Instance.box3Lifes.Length; i++)
                {
                    if (i < LudoManager.Instance.cntPlayer3)
                        LudoManager.Instance.box3Lifes[i].color = LudoManager.Instance.lifeOffColor;
                }
                break;
            }
            case 4:
            {
                LudoManager.Instance.cntPlayer4 = lifeLost;
                for (int i = 0; i < LudoManager.Instance.box3Lifes.Length; i++)
                {
                    if (i < LudoManager.Instance.cntPlayer4)
                        LudoManager.Instance.box4Lifes[i].color = LudoManager.Instance.lifeOffColor;
                }
                break;
            }
        }

    }
    
    public void SetLudoStopDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];

            string sRoomId = data["RoomId"];


            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //LudoManager.Instance.StopDiceLine();
            }
        }
    }

    public void PauseGetPlayerData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Ludo")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float sliderValue = data["GreenSliderValue"];
            int dot = data["OurDot"];
            bool isTurn = data["Turn"];
            float gameTime = data["GameTime"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                if (!DataManager.Instance.isTwoPlayer)
                    LudoManager.Instance.playerRoundChecker = data["playerRoundChecker"];

                //if (DataManager.Instance.playerData._id != playerId)
                //{
                //    print("Data Player Id : " + DataManager.Instance.playerData._id);
                //    print("Get Player Id : " + playerId);
                //    print("Player No : " + playerNo);
                //    if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                //    {
                //        LudoManager.Instance.PauseDataRetriveSocket(sliderValue, dot, isTurn);
                //    }

                //}

                if (DataManager.Instance.playerData._id != playerId)
                {
                    print("Data Player Id : " + DataManager.Instance.playerData._id);
                    print("Get Player Id : " + playerId);
                    print("Player No : " + playerNo);
                    if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && playerNo != DataManager.Instance.playerNo)
                    {
                        for (int i = 0; i < data["users"].Count; i++)
                        {
                            List<GameObject> pathList = new List<GameObject>();
                            if (data["users"][i]["updatedPlayerNo"] == "1")
                            {

                                for (int j = 0; j < data["users"][i]["tokens"].Count; j++)
                                {
                                    var pasa = SelectPasa(1)[j].GetComponent<PasaManage>();
                                    pathList = pasa.orgParentNo == 1 ? LudoManager.Instance.numberObj : pasa.orgParentNo == 2 ? LudoManager.Instance.numberObj3 :
                                        pasa.orgParentNo == 3 ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj4;

                                    pasa.playerSubNo = data["users"][i]["tokens"][j]["playerSubNo"];
                                    pasa.pasaCurrentNo = data["users"][i]["tokens"][j]["pasaCurrentNo"];
                                    if (data["users"][i]["tokens"][j]["isSafe"] == "1")
                                        pasa.isSafe = true;
                                    else if (data["users"][i]["tokens"][j]["isSafe"] == "0")
                                        pasa.isSafe = false;

                                    if (data["users"][i]["tokens"][j]["isStarted"] == "1")
                                        pasa.isStarted = true;
                                    else if (data["users"][i]["tokens"][j]["isStarted"] == "0")
                                        pasa.isStarted = false;

                                    if (pasa.pasaCurrentNo > 0)
                                        pasa.transform.position = pathList[pasa.pasaCurrentNo].transform.position;

                                    if (j == data["users"][i]["tokens"].Count - 1)
                                        LifeCheck(pasa.orgParentNo, data["users"][i]["lifeLost"], data["users"][i]["score"]);
                                }
                            }
                            if (data["users"][i]["updatedPlayerNo"] == "2")
                            {
                                for (int j = 0; j < data["users"][i]["tokens"].Count; j++)
                                {
                                    var pasa = SelectPasa(2)[j].GetComponent<PasaManage>();
                                    pathList = pasa.orgParentNo == 1 ? LudoManager.Instance.numberObj : pasa.orgParentNo == 2 ? LudoManager.Instance.numberObj3 :
                                        pasa.orgParentNo == 3 ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj4;
                                    pasa.playerSubNo = data["users"][i]["tokens"][j]["playerSubNo"];
                                    pasa.pasaCurrentNo = data["users"][i]["tokens"][j]["pasaCurrentNo"];
                                    if (pasa.pasaCurrentNo > 0)
                                        pasa.transform.position = pathList[pasa.pasaCurrentNo].transform.position;
                                    if (data["users"][i]["tokens"][j]["isSafe"] == "1")
                                        pasa.isSafe = true;
                                    else if (data["users"][i]["tokens"][j]["isSafe"] == "0")
                                        pasa.isSafe = false;

                                    if (data["users"][i]["tokens"][j]["isStarted"] == "1")
                                        pasa.isStarted = true;
                                    else if (data["users"][i]["tokens"][j]["isStarted"] == "0")
                                        pasa.isStarted = false;

                                    if (j == data["users"][i]["tokens"].Count - 1)
                                        LifeCheck(pasa.orgParentNo, data["users"][i]["lifeLost"], data["users"][i]["score"]);
                                }
                            }
                            if (data["users"][i]["updatedPlayerNo"] == "3")
                            {
                                for (int j = 0; j < data["users"][i]["tokens"].Count; j++)
                                {
                                    var pasa = SelectPasa(3)[j].GetComponent<PasaManage>();
                                    pathList = pasa.orgParentNo == 1 ? LudoManager.Instance.numberObj : pasa.orgParentNo == 2 ? LudoManager.Instance.numberObj3 :
                                        pasa.orgParentNo == 3 ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj4;
                                    pasa.playerSubNo = data["users"][i]["tokens"][j]["playerSubNo"];
                                    pasa.pasaCurrentNo = data["users"][i]["tokens"][j]["pasaCurrentNo"];
                                    if (pasa.pasaCurrentNo > 0)
                                        pasa.transform.position = pathList[pasa.pasaCurrentNo].transform.position;
                                    if (data["users"][i]["tokens"][j]["isSafe"] == "1")
                                        pasa.isSafe = true;
                                    else if (data["users"][i]["tokens"][j]["isSafe"] == "0")
                                        pasa.isSafe = false;

                                    if (data["users"][i]["tokens"][j]["isStarted"] == "1")
                                        pasa.isStarted = true;
                                    else if (data["users"][i]["tokens"][j]["isStarted"] == "0")
                                        pasa.isStarted = false;

                                    if (j == data["users"][i]["tokens"].Count - 1)
                                        LifeCheck(pasa.orgParentNo, data["users"][i]["lifeLost"], data["users"][i]["score"]);
                                }
                            }
                            if (data["users"][i]["updatedPlayerNo"] == "4")
                            {
                                for (int j = 0; j < data["users"][i]["tokens"].Count; j++)
                                {
                                    var pasa = SelectPasa(4)[j].GetComponent<PasaManage>();
                                    pathList = pasa.orgParentNo == 1 ? LudoManager.Instance.numberObj : pasa.orgParentNo == 2 ? LudoManager.Instance.numberObj3 :
                                        pasa.orgParentNo == 3 ? LudoManager.Instance.numberObj2 : LudoManager.Instance.numberObj4;
                                    pasa.playerSubNo = data["users"][i]["tokens"][j]["playerSubNo"];
                                    pasa.pasaCurrentNo = data["users"][i]["tokens"][j]["pasaCurrentNo"];
                                    if (pasa.pasaCurrentNo > 0)
                                        pasa.transform.position = pathList[pasa.pasaCurrentNo].transform.position;
                                    if (data["users"][i]["tokens"][j]["isSafe"] == "1")
                                        pasa.isSafe = true;
                                    else if (data["users"][i]["tokens"][j]["isSafe"] == "0")
                                        pasa.isSafe = false;

                                    if (data["users"][i]["tokens"][j]["isStarted"] == "1")
                                        pasa.isStarted = true;
                                    else if (data["users"][i]["tokens"][j]["isStarted"] == "0")
                                        pasa.isStarted = false;

                                    if (j == data["users"][i]["tokens"].Count - 1)
                                        LifeCheck(pasa.orgParentNo, data["users"][i]["lifeLost"], data["users"][i]["score"]);
                                }
                            }
                        }
                        LudoManager.Instance.PauseDataRetriveSocket(sliderValue, dot, isTurn, gameTime);
                    }

                }

            }
        }
    }
    
    

    
    
    public void MatchEnded()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("room", roomid);
        obj.AddField("lobbyId", DataManager.Instance.tournamentID);
        obj.AddField("userId", DataManager.Instance.playerData._id);
        obj.AddField("playerNo", DataManager.Instance.playerNo);
        socket.Emit("leave", obj);
    }

    #endregion


    #region Teen patti

    public void SetChangeTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id turnchange: " + playerId);
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);

                TeenPattiManager.Instance.GetPlayerTurn(playerNo);
            }
        }
        else if(SceneManager.GetActiveScene().name == "Joker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);

                JokerManager.Instance.GetPlayerTurn(playerNo);
            }
        }
        else if(SceneManager.GetActiveScene().name == "AK47")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);

                AK47Manager.Instance.GetPlayerTurn(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id turnchange: " + playerId);
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id &&*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PointRummyManager.Instance.GetPlayerTurn(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id turnchange: " + playerId);
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id &&*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PoolRummyManager.Instance.GetPlayerTurn(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id turnchange: " + playerId);
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id &&*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);

                DealRummyManager.Instance.GetPlayerTurn(playerNo);
            }
        }

    }
    
    public void SetBotBetTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int botBetNo = data["BotNo"];
            int botPlayerNo = data["BotPlayerNo"];
            float amount = data["CurrentAmount"];
            int index = data["CurrentIndex"];
            
            if (tourId == DataManager.Instance.tournamentID && playerId != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);

                TeenPattiManager.Instance.GetBotBetNo(botBetNo, botPlayerNo, amount, index);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Joker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int botBetNo = data["BotNo"];
            int botPlayerNo = data["BotPlayerNo"];
            float amount = data["CurrentAmount"];
            int index = data["CurrentIndex"];

            if (tourId == DataManager.Instance.tournamentID && playerId != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);

                JokerManager.Instance.GetBotBetNo(botBetNo, botPlayerNo, amount, index);
            }
        }
        else if (SceneManager.GetActiveScene().name == "AK47")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            
            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int botBetNo = data["BotNo"];
            int botPlayerNo = data["BotPlayerNo"];
            float amount = data["CurrentAmount"];
            int index = data["CurrentIndex"];

            if (tourId == DataManager.Instance.tournamentID && playerId != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);

                AK47Manager.Instance.GetBotBetNo(botBetNo, botPlayerNo, amount, index);
            }
        }

    }

    public void SetBetTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float betAmount = data["BetAmount"];
            string betType = data["BetType"];
            int currentIndex = data["currentIndex"];
            int currentPrice = data["currentPrice"];
            string playerSlideShowSendId = data["playerSlideShowSendId"];
            string playerIdSlideShowId = data["playerIdSlideShowId"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID /*&& playerID == DataManager.Instance.playerData._id*/)
            {
                //print("Teen Patti playerNo : " + playerNo);

                TeenPattiManager.Instance.GetBet(playerNo, betAmount, betType, playerSlideShowSendId, playerIdSlideShowId, currentIndex, currentPrice);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Joker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float betAmount = data["BetAmount"];
            string betType = data["BetType"];
            int currentIndex = data["currentIndex"];
            int currentPrice = data["currentPrice"];
            string playerSlideShowSendId = data["playerSlideShowSendId"];
            string playerIdSlideShowId = data["playerIdSlideShowId"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID /*&& playerID == DataManager.Instance.playerData._id*/)
            {
                //print("Teen Patti playerNo : " + playerNo);

                JokerManager.Instance.GetBet(playerNo, betAmount, betType, playerSlideShowSendId, playerIdSlideShowId, currentIndex, currentPrice);
            }
        }
        else if (SceneManager.GetActiveScene().name == "AK47")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float betAmount = data["BetAmount"];
            string betType = data["BetType"];
            int currentIndex = data["currentIndex"];
            int currentPrice = data["currentPrice"];
            string playerSlideShowSendId = data["playerSlideShowSendId"];
            string playerIdSlideShowId = data["playerIdSlideShowId"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID /*&& playerID == DataManager.Instance.playerData._id*/)
            {
                //print("Teen Patti playerNo : " + playerNo);

                AK47Manager.Instance.GetBet(playerNo, betAmount, betType, playerSlideShowSendId, playerIdSlideShowId, currentIndex, currentPrice);
            }
        }


    }
    public void SetSlideShowTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            string SlideShowCancelPlayerId = data["SlideShowCancelPlayerId"];
            string SlideShowPlayerId = data["SlideShowPlayerId"];
            string SlideShowType = data["SlideShowType"];




            if (DataManager.Instance.playerData._id.Equals(SlideShowPlayerId) && tourId == DataManager.Instance.tournamentID && playerID != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);
                if (SlideShowType == "Accept")
                {
                    TeenPattiManager.Instance.SlideShow_Accpet_Socket(SlideShowPlayerId, SlideShowCancelPlayerId);
                }
                else if (SlideShowType == "Cancel")
                {
                    TeenPattiManager.Instance.SlideShow_Cancel_Socket();
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Joker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            string SlideShowCancelPlayerId = data["SlideShowCancelPlayerId"];
            string SlideShowPlayerId = data["SlideShowPlayerId"];
            string SlideShowType = data["SlideShowType"];

            if (DataManager.Instance.playerData._id.Equals(SlideShowPlayerId) && tourId == DataManager.Instance.tournamentID && playerID != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);
                if (SlideShowType == "Accept")
                {
                    JokerManager.Instance.SlideShow_Accpet_Socket(SlideShowPlayerId, SlideShowCancelPlayerId);
                }
                else if (SlideShowType == "Cancel")
                {
                    JokerManager.Instance.SlideShow_Cancel_Socket();
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "AK47")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            string SlideShowCancelPlayerId = data["SlideShowCancelPlayerId"];
            string SlideShowPlayerId = data["SlideShowPlayerId"];
            string SlideShowType = data["SlideShowType"];

            if (DataManager.Instance.playerData._id.Equals(SlideShowPlayerId) && tourId == DataManager.Instance.tournamentID && playerID != DataManager.Instance.playerData._id)
            {
                //print("Teen Patti playerNo : " + playerNo);
                if (SlideShowType == "Accept")
                {
                    AK47Manager.Instance.SlideShow_Accpet_Socket(SlideShowPlayerId, SlideShowCancelPlayerId);
                }
                else if (SlideShowType == "Cancel")
                {
                    AK47Manager.Instance.SlideShow_Cancel_Socket();
                }
            }
        }


    }
    public void HandelWinTeenPatti(string values)
    {
        
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            string WinnerPlayerId = data["WinnerPlayerId"];




            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);
                //WinnerList

                //string[] a1 = WinnerList.Split(",");
                /*List<int> winnerNumber = new List<int>();
                for (int i = 0; i < a1.Length; i++)
                {
                    if (a1[i] != null && a1[i].Length != 0)
                    {
                        try
                        {
                            int winnerNo = int.Parse(a1[i]);
                            winnerNumber.Add(winnerNo);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }*/
                //TeenPattiManager.Instance.CreditWinnerAmount(playerID);

                TeenPattiManager.Instance.HandelTeenPattiWinData(WinnerPlayerId);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Joker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            string WinnerPlayerId = data["WinnerPlayerId"];
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                JokerManager.Instance.HandelTeenPattiWinData(WinnerPlayerId);
            }
        }
        else if (SceneManager.GetActiveScene().name == "AK47")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            string WinnerPlayerId = data["WinnerPlayerId"];
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                AK47Manager.Instance.HandelTeenPattiWinData(WinnerPlayerId);
            }
        }

    }

    public void SetChangeStatusTeenPatti(string values)
    {
        if (SceneManager.GetActiveScene().name == "TeenPatti")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                print("Teen Patti playerNo : " + playerNo);
                TeenPattiManager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Joker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                print("joker playerNo : " + playerNo);
                JokerManager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "AK47")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID && /*playerId == DataManager.Instance.playerData._id*/ sRoomId == DataManager.Instance.gameId)
            {
                print("AK47 playerNo : " + playerNo);
                AK47Manager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID /*&& playerId == DataManager.Instance.playerData._id*/ && sRoomId == DataManager.Instance.gameId)
            {
                
                PointRummyManager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID /*&& playerId == DataManager.Instance.playerData._id*/ && sRoomId == DataManager.Instance.gameId)
            {
                
                PoolRummyManager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            string cardStatus = data["CardStatus"];
            if (tourId == DataManager.Instance.tournamentID /*&& playerId == DataManager.Instance.playerData._id*/ && sRoomId == DataManager.Instance.gameId)
            {
                
                DealRummyManager.Instance.GetCardStatus(cardStatus, playerNo);
            }
        }
        


    }
    #endregion

    #region PointRummy

    public void SetDrawCardFromClosedDeck(string values)
    {
        if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int index = data["CardIndex"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                PointRummyManager.Instance.PlayerDrawClosedCard(index, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int index = data["CardIndex"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                PoolRummyManager.Instance.PlayerDrawClosedCard(index, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int index = data["CardIndex"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                DealRummyManager.Instance.PlayerDrawClosedCard(index, playerNo);
            }
        }


    }
    public void SetDrawCardFromDiscardPile(string values)
    {
        if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId + "playerNo = " + playerNo);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                PointRummyManager.Instance.PlayerDrawCardFromDiscardPile(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId + "playerNo = " + playerNo);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                PoolRummyManager.Instance.PlayerDrawCardFromDiscardPile(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId + "playerNo = " + playerNo);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                DealRummyManager.Instance.PlayerDrawCardFromDiscardPile(playerNo);
            }
        }

    }
    public void SetDiscardCard(string values)
    {
        if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int index = data["CardIndex"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                PointRummyManager.Instance.PlayerDiscardCard(index, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int index = data["CardIndex"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                PoolRummyManager.Instance.PlayerDiscardCard(index, playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int index = data["CardIndex"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id))
            {
                //print("Teen Patti playerNo : " + playerNo);

                DealRummyManager.Instance.PlayerDiscardCard(index, playerNo);
            }
        }

    }

    public void CallArrangeShowPopUp(string values)
    {
        if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            if(tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id) && sRoomId == DataManager.Instance.gameId)
            {
                PointRummyManager.Instance.PlayerRequestFinishGame(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            if(tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id) && sRoomId == DataManager.Instance.gameId)
            {
                PoolRummyManager.Instance.PlayerRequestFinishGame(playerNo);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            if(tourId == DataManager.Instance.tournamentID && !playerId.Equals(DataManager.Instance.playerData._id) && sRoomId == DataManager.Instance.gameId)
            {
                DealRummyManager.Instance.PlayerRequestFinishGame(playerNo);
            }
        }

    }

    public void ReturnCardsToDeck(string values)
    {
        if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            print("Cards sequence =   " + data.ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)
            {
                //data["ReturnCards"][i]["cardIndex"];
                List<int> cardIndexes = new List<int>();
                for (int i = 0; i < data["ReturnCards"].Count; i++)
                {
                    cardIndexes.Add(data["ReturnCards"][i]["cardIndex"]);
                }

                PointRummyManager.Instance.PlayerGetCardsReturnedToDeck(cardIndexes);
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            print("Cards sequence =   " + data.ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)
            {
                //data["ReturnCards"][i]["cardIndex"];
                List<int> cardIndexes = new List<int>();
                for (int i = 0; i < data["ReturnCards"].Count; i++)
                {
                    cardIndexes.Add(data["ReturnCards"][i]["cardIndex"]);
                }

                PoolRummyManager.Instance.PlayerGetCardsReturnedToDeck(cardIndexes);
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            print("Cards sequence =   " + data.ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)
            {
                //data["ReturnCards"][i]["cardIndex"];
                List<int> cardIndexes = new List<int>();
                for (int i = 0; i < data["ReturnCards"].Count; i++)
                {
                    cardIndexes.Add(data["ReturnCards"][i]["cardIndex"]);
                }

                DealRummyManager.Instance.PlayerGetCardsReturnedToDeck(cardIndexes);
            }
        }

    }

    public void HandleWinScreen(string values)
    {
        if (SceneManager.GetActiveScene().name == "PointRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float score = data["Points"];
            string status = data["Status"];
            int validDeclaration = data["ValidDeclaration"];// 0 = true/ 1 = false
            int dropped = data["Dropped"];// 0 = true/ 1 = false
            
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);
                PointRummyManager.Instance.resultReceivedCount++;
                float initialPosition = 450f;
                int _counter = 0;
                for (int i = 0; i < PointRummyManager.Instance.teenPattiPlayers.Count; i++)
                {
                    if (PointRummyManager.Instance.teenPattiPlayers[i].playerNo == playerNo)
                    {
                        if (i != 0)
                            PointRummyManager.Instance.resultNamesText[i].text = PointRummyManager.Instance.teenPattiPlayers[i].playerNameTxt.text;
                        else if (i == 0)
                            PointRummyManager.Instance.resultNamesText[i].text = DataManager.Instance.playerData.firstName;
                        PointRummyManager.Instance.resultTransform[i].gameObject.SetActive(true);
                        PointRummyManager.Instance.playerPoints[i] = score;
                        PointRummyManager.Instance.teenPattiPlayers[i].points = score;
                        if (validDeclaration == 0)
                        {
                            PointRummyManager.Instance.resultPointsText[i].text = score.ToString();
                            if (dropped == 0)
                            {
                                PointRummyManager.Instance.resultStatusText[i].text = "Dropped";
                                PointRummyManager.Instance.resultAmountText[i].text = "-₹" + (score * DataManager.Instance.pointValue);
                            }
                            else if (score == 0)
                            {
                                float totalscore = 0;
                                foreach (var item in PointRummyManager.Instance.teenPattiPlayers)
                                    totalscore += item.points;
                                
                                PointRummyManager.Instance.resultStatusText[i].text = "Won";
                                PointRummyManager.Instance.resultAmountText[i].text = "₹" + (totalscore * DataManager.Instance.pointValue);
                            }
                            else if (score > 0)
                            {
                                PointRummyManager.Instance.resultStatusText[i].text = "Lost";
                                PointRummyManager.Instance.resultAmountText[i].text = "-₹" + (score * DataManager.Instance.pointValue);
                            }
                            if (status == "Left")
                                PointRummyManager.Instance.resultStatusText[i].text = status;
                        }
                        else if (validDeclaration == 1)
                        {
                            PointRummyManager.Instance.teenPattiPlayers[i].points = 80f;
                            PointRummyManager.Instance.resultPointsText[i].text = "80";
                            PointRummyManager.Instance.resultAmountText[i].text = "-₹8";
                            if (status == "Left")
                                PointRummyManager.Instance.resultStatusText[i].text = status;
                            if (dropped == 0)
                                PointRummyManager.Instance.resultStatusText[i].text = "Wrong Show";
                        }
                        for (int j = 0; j < data["Cards"].Count; j++)//group count
                        {
                            //if (validDeclaration == 1 || dropped == 0)
                            //    break;
                            GameObject newGroup = Instantiate(PointRummyManager.Instance.newGroup.gameObject, PointRummyManager.Instance.resultCardsHolder[i]);
                            newGroup.transform.SetAsLastSibling();
                            newGroup.transform.localPosition = new Vector3(initialPosition - (_counter * ((data["Cards"][j - 1]["Group"].Count * 60f) + 130f)), -180f, 0f);
                            initialPosition = newGroup.transform.localPosition.x;
                            _counter = 1;
                            for (int k = 0; k < data["Cards"][j]["Group"].Count; k++)//cards count
                            {
                                GameObject newCard = Instantiate(PointRummyManager.Instance.cardPrefab, newGroup.transform);
                                newCard.transform.localPosition = new Vector3(k * -60f, 0f, 0f);
                                print("cardIndex = " + data["Cards"][j]["Group"][k]["cardIndex"]);
                                newCard.GetComponent<Image>().sprite = PointRummyManager.Instance.cardShuffles[data["Cards"][j]["Group"][k]["cardIndex"]].cardSprite;
                                newCard.transform.SetAsFirstSibling();
                            }
                        }
                    }
                }
                    //PointRummyManager.Instance.resultTransform[index].gameObject.SetActive(true)

                if (PointRummyManager.Instance.isGameComplete)
                    PointRummyManager.Instance.FinalResult();
                if (!PointRummyManager.Instance.isGameComplete && DataManager.Instance.joinPlayerDatas.Count == 1)
                    PointRummyManager.Instance.FinalResult();
            }
        }
        else if (SceneManager.GetActiveScene().name == "PoolRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float score = data["Points"];
            string status = data["Status"];
            int validDeclaration = data["ValidDeclaration"];// 0 = true/ 1 = false
            int dropped = data["Dropped"];// 0 = true/ 1 = false
            float gameScore = data["GamePoints"];
            
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);
                PoolRummyManager.Instance.resultReceivedCount++;
                float initialPosition = 450f;
                int _counter = 0;
                foreach (var item in PoolRummyManager.Instance.playerSquList)
                {
                    if(item.playerNo == playerNo)
                    {
                        item.gameScoreText.text = gameScore.ToString();
                        item.playerGamePoints = gameScore;
                        break;
                    }
                }
                for (int i = 0; i < PoolRummyManager.Instance.teenPattiPlayers.Count; i++)
                {
                    if(PoolRummyManager.Instance.teenPattiPlayers[i].playerNo == playerNo)
                    {
                        if (i != 0)
                            PoolRummyManager.Instance.resultNamesText[i].text = PoolRummyManager.Instance.teenPattiPlayers[i].playerNameTxt.text;
                        else if (i == 0)
                            PoolRummyManager.Instance.resultNamesText[i].text = DataManager.Instance.playerData.firstName;

                        PoolRummyManager.Instance.resultTransform[i].gameObject.SetActive(true);
                        if (validDeclaration == 0)
                        {
                            PoolRummyManager.Instance.teenPattiPlayers[i].playerGamePoints = score;
                            PoolRummyManager.Instance.resultPointsText[i].text = score.ToString();
                            if (dropped == 0)
                            {
                                PoolRummyManager.Instance.resultStatusText[i].text = "Dropped";
                                PoolRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            }
                            else if (score == 0)
                            {
                                PoolRummyManager.Instance.resultStatusText[i].text = "Won";
                                PoolRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            }
                            else if (score > 0)
                            {
                                PoolRummyManager.Instance.resultStatusText[i].text = "Lost";
                                PoolRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            }
                            if (status == "Left")
                                PoolRummyManager.Instance.resultStatusText[i].text = status;

                            if (gameScore > DataManager.Instance.pointLimit)
                            {
                                PoolRummyManager.Instance.resultStatusText[i].text = "Lost(DQ)";
                                PoolRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            }
                        }
                        else if (validDeclaration == 1)
                        {
                            PoolRummyManager.Instance.teenPattiPlayers[i].playerGamePoints = score;
                            PoolRummyManager.Instance.resultPointsText[i].text = score.ToString();
                            //PoolRummyManager.Instance.resultAmountText[i].text = "-₹8";
                            if (status == "Left")
                                PoolRummyManager.Instance.resultStatusText[i].text = status;
                            if (dropped == 0)
                                PoolRummyManager.Instance.resultStatusText[i].text = "Wrong Show";
                        }
                        for (int j = 0; j < data["Cards"].Count; j++)//group count
                        {
                            //if (validDeclaration == 1 || dropped == 0)
                            //    break;
                            GameObject newGroup = Instantiate(PoolRummyManager.Instance.newGroup.gameObject, PoolRummyManager.Instance.resultCardsHolder[i]);
                            newGroup.transform.SetAsLastSibling();
                            newGroup.transform.localPosition = new Vector3(initialPosition - (_counter * ((data["Cards"][j - 1]["Group"].Count * 60f) + 130f)), -180f, 0f);
                            initialPosition = newGroup.transform.localPosition.x;
                            _counter = 1;
                            for (int k = 0; k < data["Cards"][j]["Group"].Count; k++)//cards count
                            {
                                GameObject newCard = Instantiate(PoolRummyManager.Instance.cardPrefab, newGroup.transform);
                                newCard.transform.localPosition = new Vector3(k * -60f, 0f, 0f);
                                print("cardIndex = " + data["Cards"][j]["Group"][k]["cardIndex"]);
                                newCard.GetComponent<Image>().sprite = PoolRummyManager.Instance.cardShuffles[data["Cards"][j]["Group"][k]["cardIndex"]].cardSprite;
                                newCard.transform.SetAsFirstSibling();
                            }
                        }

                        break;
                    }
                }
                
                if (PoolRummyManager.Instance.isGameComplete)
                    PoolRummyManager.Instance.FinalResult();
                if (!PoolRummyManager.Instance.isGameComplete && DataManager.Instance.joinPlayerDatas.Count == 1)
                    PoolRummyManager.Instance.FinalResult();
            }
        }
        else if (SceneManager.GetActiveScene().name == "DealRummy")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float score = data["Points"];
            string status = data["Status"];
            int validDeclaration = data["ValidDeclaration"];// 0 = true/ 1 = false
            //int dropped = data["Dropped"];// 0 = true/ 1 = false
            float gameScore = data["GamePoints"];
            
            if (tourId == DataManager.Instance.tournamentID && sRoomId == DataManager.Instance.gameId)
            {
                //print("Teen Patti playerNo : " + playerNo);
                DealRummyManager.Instance.resultReceivedCount++;
                float initialPosition = 450f;
                int _counter = 0;
                foreach (var item in DealRummyManager.Instance.playerSquList)
                {
                    if(item.playerNo == playerNo)
                    {
                        item.gameScoreText.text = gameScore.ToString();
                        item.playerGamePoints = gameScore;
                        break;
                    }
                }
                for (int i = 0; i < DealRummyManager.Instance.teenPattiPlayers.Count; i++)
                {
                    if(DealRummyManager.Instance.teenPattiPlayers[i].playerNo == playerNo)
                    {
                        if (i != 0)
                            DealRummyManager.Instance.resultNamesText[i].text = DealRummyManager.Instance.teenPattiPlayers[i].playerNameTxt.text;
                        else if (i == 0)
                            DealRummyManager.Instance.resultNamesText[i].text = DataManager.Instance.playerData.firstName;

                        DealRummyManager.Instance.resultTransform[i].gameObject.SetActive(true);
                        if (validDeclaration == 0)
                        {
                            DealRummyManager.Instance.teenPattiPlayers[i].playerGamePoints = score;
                            DealRummyManager.Instance.resultPointsText[i].text = score.ToString();
                            //if (dropped == 0)
                            //{
                            //    DealRummyManager.Instance.resultStatusText[i].text = "Dropped";
                            //    DealRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            //}
                            if (score == 0)
                            {
                                DealRummyManager.Instance.resultStatusText[i].text = "Won";
                                DealRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            }
                            else if (score > 0)
                            {
                                DealRummyManager.Instance.resultStatusText[i].text = "Lost";
                                DealRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            }
                            if (status == "Left")
                                DealRummyManager.Instance.resultStatusText[i].text = status;

                            //if (gameScore > DataManager.Instance.pointLimit)
                            //{
                            //    DealRummyManager.Instance.resultStatusText[i].text = "Lost(DQ)";
                            //    DealRummyManager.Instance.resultAmountText[i].text = gameScore.ToString();
                            //}
                        }
                        else if (validDeclaration == 1)
                        {
                            DealRummyManager.Instance.teenPattiPlayers[i].playerGamePoints = score;
                            DealRummyManager.Instance.resultPointsText[i].text = score.ToString();
                            //DealRummyManager.Instance.resultAmountText[i].text = "-₹8";
                                DealRummyManager.Instance.resultStatusText[i].text = "Wrong Show";
                            //if (dropped == 0)
                            //    DealRummyManager.Instance.resultStatusText[i].text = "Dropped";
                        }
                        for (int j = 0; j < data["Cards"].Count; j++)//group count
                        {
                            //if (validDeclaration == 1)
                            //    break;
                            GameObject newGroup = Instantiate(DealRummyManager.Instance.newGroup.gameObject, DealRummyManager.Instance.resultCardsHolder[i]);
                            newGroup.transform.SetAsLastSibling();
                            newGroup.transform.localPosition = new Vector3(initialPosition - (_counter * ((data["Cards"][j - 1]["Group"].Count * 60f) + 130f)), -180f, 0f);
                            initialPosition = newGroup.transform.localPosition.x;
                            _counter = 1;
                            for (int k = 0; k < data["Cards"][j]["Group"].Count; k++)//cards count
                            {
                                GameObject newCard = Instantiate(DealRummyManager.Instance.cardPrefab, newGroup.transform);
                                newCard.transform.localPosition = new Vector3(k * -60f, 0f, 0f);
                                print("cardIndex = " + data["Cards"][j]["Group"][k]["cardIndex"]);
                                newCard.GetComponent<Image>().sprite = DealRummyManager.Instance.cardShuffles[data["Cards"][j]["Group"][k]["cardIndex"]].cardSprite;
                                newCard.transform.SetAsFirstSibling();
                            }
                        }

                        break;
                    }
                }
                
                if (DealRummyManager.Instance.isGameComplete)
                    DealRummyManager.Instance.FinalResult();
                if (!DealRummyManager.Instance.isGameComplete && DataManager.Instance.joinPlayerDatas.Count == 1)
                    DealRummyManager.Instance.FinalResult();
            }
        }

    }


        #endregion

        #region Roulette

        public void SetBetRoulette(string values)
    {
        if (SceneManager.GetActiveScene().name == "Rouletee")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            int chipNo = data["chipNo"];
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);
                RouletteManager.Instance.GetBetSocket(chipNo);
            }
        }

    }

    public void FindRouletteData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Rouletee")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"]; ;
            string sRoomId = data["RoomId"];
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && RouletteManager.Instance.isAdmin)
            {
                RouletteManager.Instance.SendAdminDataPlayer(playerID);
            }
        }

    }

    public void SendAdminRouleteeData(string values)
    {
        if (SceneManager.GetActiveScene().name == "Rouletee")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string adminPlayerID = data["AdminPlayerID"];
            string receivePlayerID = data["ReceivePlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int time = data["Time"];
            int rouletteNumber = data["RouletteNumber"];

            if (receivePlayerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && !RouletteManager.Instance.isAdmin)
            {
                RouletteManager.Instance.GetAdminDataPlayer(time, rouletteNumber);
            }
            /* if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && !RouletteManager.Instance.isAdmin)
             {
                 RouletteManager.Instance.GetAdminDataPlayer(180, rouletteNumber);
             }*/
        }

    }

    #endregion

    #region CarRoulette
    public void SetBetCarRoulette(string values)
    {
        if (SceneManager.GetActiveScene().name == "CarRoulette")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int boxNo = data["boxNo"];
            int chipNo = data["chipNo"];
          
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                CarRouletteScript.Instance.GetCarRouletteBet(boxNo, chipNo);
            }
            CarRouletteScript.Instance.TotalBetSet(chipNo);
        }

    }

    public void FindCarRouletteData(string values)
    {
        if (SceneManager.GetActiveScene().name == "CarRoulette")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"]; ;
            string sRoomId = data["RoomId"];
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && RouletteManager.Instance.isAdmin)
            {
                CarRouletteScript.Instance.SendAdminDataPlayer(playerID);
            }
        }

    }

    public void SendAdminCarRouletteData(string values)
    {
        if (SceneManager.GetActiveScene().name == "CarRoulette")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string adminPlayerID = data["AdminPlayerID"];
            string receivePlayerID = data["ReceivePlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int time = data["Time"];
            int rouletteNumber = data["RouletteNumber"];

            /*if (receivePlayerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid && !RouletteManager.Instance.isAdmin)
            {
                RouletteManager.Instance.GetAdminDataPlayer(time, rouletteNumber);
            }*/
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid && !RouletteManager.Instance.isAdmin)
            {
                CarRouletteScript.Instance.GetAdminDataPlayer(time, rouletteNumber);
            }
        }

    }

    #endregion


    #region AndarBahar

    public void SetBetAndarBahar(string values)
    {
        if (SceneManager.GetActiveScene().name == "AndarBahar")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            bool isAndar = data["isAndar"];
            bool isBahar = data["isBahar"];
            float value1 = data["value"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                AndarBaharManager.Instance.GetBet(isAndar, isBahar, playerID, value1);
            }
        }

    }

    public void SetTempNoAndarBahar(string values)
    {
        if (SceneManager.GetActiveScene().name == "AndarBahar")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int tempNo = data["TempNo"];
            bool isRight = data["Right"];
            bool isLeft = data["Left"];
            
            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                AndarBaharManager.Instance.GetTempNo(tempNo, isRight, isLeft);
            }
        }
    }
    #endregion

    #region DragonTiger

    public void SetBetDragonTiger(string values)
    {
        if (SceneManager.GetActiveScene().name == "DragonTiger")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int boxNo = data["boxNo"];
            int chipNo = data["chipNo"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                DragonTigerManager.Instance.GetDragonTigerBet(boxNo, chipNo);
            }
        }

    }
    
    public void SetGetDeckData(string values)
    {
        if (SceneManager.GetActiveScene().name == "DragonTiger")
        {
            JSONNode value = JSON.Parse(values);
            print(value.ToString());
            JSONNode valueData = JSON.Parse(value["data"].ToString());

            if (roomid.Equals(valueData["room"]))
            {
                int deckNo1 = valueData["DeckNo1"];
                int deckNo2 = valueData["DeckNo2"];
                DragonTigerManager.Instance.GetDeckData(deckNo1, deckNo2);
            }
        }
    }
    
    public void SetWinData(string values)
    {
        if (SceneManager.GetActiveScene().name == "DragonTiger")
        {
            JSONNode value = JSON.Parse(values);
            print("This is recevied data -> set windata " + value.ToString());
            JSONNode valueData = JSON.Parse(value["data"].ToString());

            if (roomid.Equals(valueData["room"]))
            {
                string winData = valueData["WinList"]["WinList"]["WinList"];
                DragonTigerManager.Instance.GetUpdatedHistory(winData);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Aviator")
        {
            JSONNode value = JSON.Parse(values);
            print("This is recevied data -> set windata " + value.ToString());
            JSONNode valueData = JSON.Parse(value["data"].ToString());

            if (roomid.Equals(valueData["room"]))
            {
                string winData = valueData["WinList"]["WinList"]["PointList"];
                AviatorGameManager.Instance.GetUpdatedHistory(winData);
            }
        }

    }

    #endregion

    #region Seven Up Down

    public void SetSevenUpDownBet(string values)
    {
        if(SceneManager.GetActiveScene().name == "7UpDown")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tournamentID = data["TournamentID"];
            string sentRoomID = data["roomId"];
            int area = data["area"];
            int chipNo = data["chipNo"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tournamentID == DataManager.Instance.tournamentID && sentRoomID == roomid)
                SevenUpDownManager.Instance.GetSevenUpDownBet(area, chipNo, playerID);//betting as non admin player to show in other players' screen
        }

    }

    public void SetDiceData(string values)
    {
        if (SceneManager.GetActiveScene().name == "7UpDown")
        {
            JSONNode value = JSON.Parse(values);
            print("Data received  = \n" + value.ToString());
            JSONNode data = JSON.Parse(value["data"].ToString());
            if (roomid.Equals(data["room"]))
            {
                int dice1 = data["Dice1"];
                int dice2 = data["Dice2"];
                SevenUpDownManager.Instance.GetDiceData(dice1, dice2);
            }
        }
    }

    public void SetWinDataForSevenUpDown(string values)
    {
        if(SceneManager.GetActiveScene().name == "7UpDown")
        {
            JSONNode value = JSON.Parse(values);
            print("This is recevied data for history -> set windata " + value.ToString());
            JSONNode valueData = JSON.Parse(value["data"].ToString());

            if (roomid.Equals(valueData["room"]) && SevenUpDownManager.Instance.isAdmin == false)
            {
                string winData = valueData["WinList"];
                string diceWinData = valueData["DiceList"];
                SevenUpDownManager.Instance.GetUpdatedHistory(winData, diceWinData);
            }
        }
    }
    #endregion

    #region Jhandi Munda
    public void SetJhandiMundaBet(string values)
    {
        if (SceneManager.GetActiveScene().name == "JhandiMunda")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tournamentID = data["TournamentID"];
            string sentRoomID = data["roomId"];
            int area = data["area"];
            int chipNo = data["chipNo"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tournamentID == DataManager.Instance.tournamentID && sentRoomID == roomid)
                JhandiMundaManager.Instance.GetSevenUpDownBet(area, chipNo, playerID);//betting as non admin player to show in other players' screen
        }

    }

    public void SetDiceDataJhandiMunda(string values)
    {
        if (SceneManager.GetActiveScene().name == "JhandiMunda")
        {
            JSONNode value = JSON.Parse(values);
            print("Data received  = \n" + value.ToString());
            JSONNode data = JSON.Parse(value["data"].ToString());
            if (roomid.Equals(data["room"]))
            {
                int dice1 = data["Dice1"];
                int dice2 = data["Dice2"];
                List<int> diceData = new List<int>();
                for (int i = 0; i < data["WinData"].Count; i++)
                {
                    diceData.Add(data["WinData"][i]["diceData"]);
                }
                JhandiMundaManager.Instance.GetResultData(diceData);
            }
        }
    }

    public void SetWinDataJhandiMunda(string values)
    {
        if (SceneManager.GetActiveScene().name == "JhandiMunda")
        {
            JSONNode value = JSON.Parse(values);
            print("This is recevied data for history -> set windata " + value.ToString());
            JSONNode valueData = JSON.Parse(value["data"].ToString());

            if (roomid.Equals(valueData["room"]) && SevenUpDownManager.Instance.isAdmin == false)
            {
                //string winData = valueData["WinList"];
                //string diceWinData = valueData["DiceList"];
                List<int> winData = new List<int>();

                for (int i = 0; i < valueData["WinData"].Count; i++)
                {
                    winData.Add(valueData["WinData"][i]["symbolIndex"]);
                }

                JhandiMundaManager.Instance.GetUpdatedHistory(winData);
            }
        }
    }

    #endregion

    #region Poker

    public void SetChangePoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerId = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];

            print("Player Id : " + playerId);
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetPlayerTurn(playerNo);
            }
        }

    }
    
    public void SetBotBetPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());
            
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            int botBetNo = data["BotNo"];
            int botPlayerNo = data["BotPlayerNo"];
            float botBetAmount = data["BetAmount"];
            
            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetBotBetNo(botBetNo, botPlayerNo, botBetAmount);
            }
        }
    }


    public void SetBetPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            int playerNo = data["PlayerNo"];
            string sRoomId = data["RoomId"];
            float betAmount = data["BetAmount"];
            string betType = data["BetType"];

            if (!playerID.Equals(DataManager.Instance.playerData._id) && tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetPokerBet(playerNo, betAmount, betType);
            }
        }

    }
    

    public void SetFoldPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string playerNo = data["FoldPlayerId"];
            string sRoomId = data["RoomId"];

            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);

                PokerGameManager.Instance.GetPokerFold(playerNo);
            }
        }

    }
    
    public void HandelWinPoker(string values)
    {
        if (SceneManager.GetActiveScene().name != "Poker") return;
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());

        string playerID = data["PlayerID"];
        string tourId = data["TournamentID"];
        string sRoomId = data["RoomId"];
        string winnerPlayerId = data["WinnerPlayerId"];
            
        if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
        {
            PokerGameManager.Instance.CallFinalWinner(winnerPlayerId);
        }
    }

    
    public void SetWinPoker(string values)
    {
        if (SceneManager.GetActiveScene().name == "Poker")
        {
            JSONNode value = JSON.Parse(values);
            JSONNode data = JSON.Parse(value["data"].ToString());

            string playerID = data["PlayerID"];
            string tourId = data["TournamentID"];
            string sRoomId = data["RoomId"];
            string WinnerList = data["WinnerList"];




            if (tourId == DataManager.Instance.tournamentID && sRoomId == roomid)
            {
                //print("Teen Patti playerNo : " + playerNo);
                //WinnerList

                string[] a1 = WinnerList.Split(",");
                List<int> winnerNumber = new List<int>();
                for (int i = 0; i < a1.Length; i++)
                {
                    if (a1[i] != null && a1[i].Length != 0)
                    {
                        try
                        {
                            int winnerNo = int.Parse(a1[i]);
                            winnerNumber.Add(winnerNo);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                PokerGameManager.Instance.GetWinners(winnerNumber);
            }
        }

    }

    #endregion

    #region Set Room Data

    public void SetRoomdata(string roomId, JSONObject data)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomId);
        keys.AddField("gameData", data);
        Debug.Log("SendData===:::" + keys);
        socket.Emit("setGameData", keys);
    }
    public void SetGameId(string lobbyId)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomid);
        keys.AddField("lobbyId", lobbyId);
        Debug.Log("SendData===:::" + keys);
        socket.Emit("setGameId", keys);
    }
    
    public void HandelSetGameId(string values)
    {
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());
        DataManager.Instance.gameId = data["gameId"];
        Debug.Log("SendData===:::" + data);
    }
    
    public void HandleGetData(string values)
    {
        
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());
        RouletteManager.Instance.noGen = data["betWin"];
        print("This is recevied data ->____________________> " + values);
        //Show betwin key as a winner 

    }
    
    public void HandleGetCarData(string values)
    {
        
        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());
        CarRouletteScript.Instance.noGen = data["betWin"];
        print("This is recevied data ->____________________> " + value);
        //Show betwin key as a winner 

    }
    
    public void SetWinData(string roomId, JSONObject data)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("room", roomId);

        keys.AddField("WinList", data);
        Debug.Log("SendData===:::" + keys);
        socket.Emit("setWinListData", keys);
    }

    public void SetLobbyCount(string lobbyId)
    {
        JSONObject keys = new JSONObject();
        keys.AddField("lobbyId", lobbyId);

        //Debug.Log("SendData===:::" + keys.ToString());
        socket.Emit("lobbyStat", keys);
    }

    public void SendChatMessageManage(string values)
    {

        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());

        string sRoomId = data["RoomId"];
        string msg = data["Message"];
        string gameName = data["gameName"];
        string playerId = data["PlayerID"];
        if (sRoomId == roomid)
        {
            if (gameName == "TeenPatti")
            {
                TeenPattiManager.Instance.GetChat(playerId, msg);
            }
            else if (gameName == "AndarBahar")
            {
                AndarBaharManager.Instance.GetChat(playerId, msg);
            }
            else if (gameName == "Poker")
            {
                PokerGameManager.Instance.GetChat(playerId, msg);
            }
            else if (gameName == "Ludo")
            {
                LudoUIManager.Instance.GetChat(playerId, msg);
            }
            
        }

    }

    public void SendGiftMessageManage(string values)
    {

        JSONNode value = JSON.Parse(values);
        JSONNode data = JSON.Parse(value["data"].ToString());

        string sRoomId = data["RoomId"];
        string gameName = data["gameName"];
        string sendPlayerID = data["SendPlayerID"];
        string receivePlayerID = data["ReceivePlayerID"];
        int giftNo = data["GiftNo"];
        int type = data["Type"];
        string message = data["Message"];
        if (sRoomId == roomid)
        {
            if (gameName == "TeenPatti")
            {
                TeenPattiManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo);
            }
            else if (gameName == "AndarBahar")
            {
                AndarBaharManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo);
            }
            else if (gameName == "Poker")
            {
                PokerGameManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo);
            }
            else if (gameName == "Ludo")
            {
                LudoUIManager.Instance.GetGift(sendPlayerID, receivePlayerID, giftNo, type, message);
            }
        }


    }


    public void GetRoomData(string data)
    {
        JSONNode value = JSON.Parse(data);
        print("Room Data : \n" + value.ToString());
        JSONNode valueData = JSON.Parse(value["data"].ToString());

        if (roomid.Equals(valueData["room"]))
        {
            int gameMode = valueData["gameData"]["gameMode"];
            int deckNo = valueData["gameData"]["DeckNo"];
            int deckNo1 = valueData["gameData"]["DeckNo1"];
            int deckNo2 = valueData["gameData"]["DeckNo2"];//dealer
            int dice1 = valueData["gameData"]["Dice1"];//SevenUpDown
            int dice2 = valueData["gameData"]["Dice2"];//SevenUpDown
            string winData = valueData["gameData"]["WinList"];
            string playerId = valueData["gameData"]["PlayerID"];
            int jokerIndex = valueData["gameData"]["Joker"];
            int discardIndex = valueData["gameData"]["DiscardedCard"];
            int[] diceData = new int[6];

            for (int i = 0; i < valueData["gameData"]["WinData"].Count; i++)
                diceData[i] = valueData["gameData"]["WinData"][i]["diceData"];
            

            List<int> newDeck = new List<int>();
            for (int i = 0; i < valueData["gameData"]["UpdatedDeck"].Count; i++)
                newDeck.Add(valueData["gameData"]["UpdatedDeck"][i]["Index"]);
            
            //data["users"][i]["tokens"][j]["playerSubNo"];
            

            if (gameMode == 1)
            {
                DragonTigerManager.Instance.GetRoomData(deckNo1, deckNo2, winData);
            }
            else if (gameMode == 2)
            {
                AndarBaharManager.Instance.GetRoomData(deckNo, winData);
            }
            else if (gameMode == 3)
            {
                RouletteManager.Instance.GetRoomData(deckNo, winData);
            }
            else if (gameMode == 4)
            {
                CarRouletteScript.Instance.GetRoomData(deckNo, winData);
            }
            else if (gameMode == 5)
            {
                PokerGameManager.Instance.GetRoomData(deckNo1, deckNo2);
            }
            else if (gameMode == 8)
            {
                TeenPattiManager.Instance.GetRoomData(deckNo, deckNo2, playerId);
            }
            else if (gameMode == 9)
            {
                AndarBaharManager.Instance.GetUpdatedHistory(winData);
            }
            else if (gameMode == 13)
            {
                SevenUpDownManager.Instance.GetRoomData(dice1, dice2);
            }
            else if (gameMode == 14)
            {
                JokerManager.Instance.GetRoomData(deckNo, deckNo2, playerId);
            }
            else if (gameMode == 10)
            {
                AK47Manager.Instance.GetRoomData(deckNo, deckNo2, playerId);
            }
            else if (gameMode == 11)
            {
                print("Data received =\n " + valueData.ToString());
                PointRummyManager.Instance.GetRoomData(playerId, newDeck, jokerIndex,discardIndex, deckNo2);
            }
            else if (gameMode == 15)
            {
                print("Data received =\n " + valueData.ToString());
                PoolRummyManager.Instance.GetRoomData(playerId, newDeck, jokerIndex,discardIndex, deckNo2);
            }
            else if (gameMode == 16)
            {
                print("Data received =\n " + valueData.ToString());
                DealRummyManager.Instance.GetRoomData(playerId, newDeck, jokerIndex,discardIndex, deckNo2);
            }
            else if (gameMode == 18)
            {
                print("Data received =\n " + valueData.ToString());
                JhandiMundaManager.Instance.GetRoomData(diceData);
            }

        }

        //JSONNode data = JSON.Parse(value["data"].ToString());
    }

    #endregion

    public void LeaveRoom()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("room", roomid);
        obj.AddField("lobbyId", DataManager.Instance.tournamentID);
        obj.AddField("userId", DataManager.Instance.playerData._id);
        obj.AddField("playerNo", DataManager.Instance.playerNo);

        //if(PointRummyManager.Instance != null)
        //{
        //    if(DataManager.Instance.gameMode == GameType.Point_Rummy)
        //        PointRummyManager.Instance.ReturnCardsToDeck();
        //}
        print("Leaving");

        DataManager.Instance.tournamentID = "";
        DataManager.Instance.tourEntryMoney = 0;
        DataManager.Instance.tourCommision = 0;
        DataManager.Instance.commisionAmount = 0;
        DataManager.Instance.orgIndexPlayer = 0;
        DataManager.Instance.joinPlayerDatas.Clear();
        roomid = "";
        userdata = "";
        playTime = 0;
        //obj.AddField("userId", Datamanger.Intance.userID.Trim('"'));
        socket.Emit("leave", obj);
    }

    public void ResetRole(string values)
    {

        JSONNode keys = JSON.Parse(values.ToString());
        JSONNode data = JSON.Parse(keys["data"]["users"].ToString());
        userdata = data.ToString();
        bool isAvaliable = false;


        print("Key Data : " + keys.ToString());
        print("data Data : " + data.ToString());

        string room = keys["data"]["room"];
        string leaveUserId = keys["data"]["userId"];
        string playerId = data[0]["userId"];
        string lobbyID = data[0]["lobbyId"];
        int playerNo = data[0]["playerNo"];



        //if (DataManager.Instance.isTwoPlayer)
        //{
        //print("Room Id : " + roomid);
        //print("leaver Id : " + leaveUserId);
        //print("player Id : " + playerId);
        //print("Lobby Id : " + lobbyID);
        if (DataManager.Instance.gameMode == GameType.Point_Rummy)
        {
            if (PointRummyManager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    print("Left player no = " + index);
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    PointRummyManager.Instance.ChangeAAdmin(leaveUserId, playerId, index + 1);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Pool_Rummy)
        {
            if (PoolRummyManager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    print("Left player no = " + index);
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    PoolRummyManager.Instance.ChangeAAdmin(leaveUserId, playerId, index + 1);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Deal_Rummy)
        {
            if (DealRummyManager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    print("Left player no = " + index);
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    DealRummyManager.Instance.ChangeAAdmin(leaveUserId, playerId, index + 1);
                }
            }
        }


        if (DataManager.Instance.gameMode == GameType.Teen_Patti)
        {
            if (TeenPattiManager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }
                
                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    TeenPattiManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }

        if (DataManager.Instance.gameMode == GameType.Joker)
        {
            if (JokerManager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    JokerManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Teen_Patti_AK47)
        {
            if (AK47Manager.Instance != null)
            {
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    AK47Manager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        /*if (DataManager.Instance.gameMode == GameType.Poker)
        {
            if (PokerGameManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();

                /*DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }#1#
                
                int index = DataManager.Instance.joinPlayerDatas.FindIndex(player => player.userId == leaveUserId);
                if (index != -1)
                {
                    DataManager.Instance.joinPlayerDatas.RemoveAt(index);
                }
                
                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    PokerGameManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }*/

        if (DataManager.Instance.gameMode == GameType.Roulette)
        {
            if (RouletteManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    RouletteManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Car_Roulette)
        {
            if (RouletteManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    CarRouletteScript.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Andar_Bahar)
        {
            if (AndarBaharManager.Instance != null)
            {
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                DataManager.Instance.joinPlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }
                //print("Room 1: " + room);
                //print("Room 1: " + roomid);
                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    AndarBaharManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if (DataManager.Instance.gameMode == GameType.Dragon_Tiger)
        {
            if (DragonTigerManager.Instance != null)
            {
                DataManager.Instance.joinPlayerDatas.Clear();
                //DataManager.Instance.leaveUpdatePlayerDatas.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    JoinPlayerData jData = new JoinPlayerData();
                    jData.userId = data[i]["userId"];
                    jData.userName = data[i]["name"];
                    jData.balance = data[i]["balance"];
                    jData.lobbyId = data[i]["lobbyId"];
                    jData.playerNo = 0;
                    jData.avtar = data[i]["avtar"];
                    //DataManager.Instance.leaveUpdatePlayerDatas.Add(jData);
                    DataManager.Instance.joinPlayerDatas.Add(jData);

                }

                if (room == roomid && lobbyID == DataManager.Instance.tournamentID)
                {
                    DragonTigerManager.Instance.ChangeAAdmin(leaveUserId, playerId);
                }
            }
        }
        if(DataManager.Instance.gameMode == GameType.Ludo)
        {
            if(LudoManager.Instance != null)
            {
                if (DataManager.Instance.joinPlayerDatas.First().userId == leaveUserId)
                {
                    playerNo = DataManager.Instance.joinPlayerDatas.First().playerNo;
                    DataManager.Instance.joinPlayerDatas.RemoveAt(0);
                    //DataManager.Instance.joinPlayerDatas.Clear();
                    //for (int i = 0; i < data.Count; i++)
                    //{
                    //    JoinPlayerData jData = new JoinPlayerData();
                    //    jData.userId = data[i]["userId"];
                    //    jData.userName = data[i]["name"];
                    //    jData.balance = data[i]["balance"];
                    //    jData.lobbyId = data[i]["lobbyId"];
                    //    jData.playerNo = 0;
                    //    jData.avtar = data[i]["avtar"];
                    //    DataManager.Instance.joinPlayerDatas.Add(jData);
                    //}
                }
                else
                {
                    playerNo = DataManager.Instance.joinPlayerDatas.Find(x => x.userId == leaveUserId).playerNo;
                    DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas.Find(x => x.userId == leaveUserId));
                }
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                    DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;
                
            }
            if (room == roomid /*&& lobbyId == DataManager.Instance.tournamentID*/)
                LudoManager.Instance.ChangeAdmin(/*leaveUserId, playerId,*/ playerNo);
        }


        if (data[0]["userId"].Equals(DataManager.Instance.playerData._id) && DataManager.Instance.isTwoPlayer)
        {
            isAvaliable = true;
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 1)
            isAvaliable = true;
        //}
        if (isAvaliable == true)
        {
            if (DataManager.Instance.gameMode == GameType.Ludo)
            {
                if (LudoManager.Instance != null)
                {
                    LudoManager.Instance.isOtherPlayLeft = true;
                    LudoManager.Instance.WinUserShow();
                }
            }
        }

        if(DataManager.Instance.gameMode == GameType.SevenUpDown)
        {
            if(SevenUpDownManager.Instance != null)
            {
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(leaveUserId))
                    {
                        DataManager.Instance.joinPlayerDatas.RemoveAt(i);
                        break;
                    }
                }
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                    DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;

                if (room == roomid)
                    SevenUpDownManager.Instance.ChangeAdmin(leaveUserId);
            }
        }

        if(DataManager.Instance.gameMode == GameType.Jhandi_Munda)
        {
            if(JhandiMundaManager.Instance != null)
            {
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(leaveUserId))
                    {
                        DataManager.Instance.joinPlayerDatas.RemoveAt(i);
                        break;
                    }
                }
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                    DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;

                if (room == roomid)
                    JhandiMundaManager.Instance.ChangeAdmin(leaveUserId);
            }
        }


        /*if (DataManager.Instance.gameMode == GameType.Snake)
        {
            if (SnakeManager.Instance != null)
            {
                SnakeManager.Instance.isOtherPlayLeft = true;
                SnakeManager.Instance.WinUserShow();
            }
        }*/
        //SocketGameplay.Instace.ResetGameData();
    }









}
