using MoreMountains.NiceVibrations;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LudoManager : MonoBehaviour
{
    // Testing 
    public List<int> safeNoBotList = new List<int>();
    public static LudoManager Instance;
    public GameObject startGamePopup;

    public GameObject notTurnPre;
    public GameObject notTurnParent;
    public List<GameObject> numberObj;
    public List<GameObject> numberObj2;
    public List<GameObject> numberObj3;
    public List<GameObject> numberObj4;



    public List<int> orgListNo2 = new List<int>();
    public List<int> orgListNo3 = new List<int>();
    public List<int> orgListNo4 = new List<int>();

    public Sprite[] diceSprite;

    public List<PasaManage> pasaSocketList = new List<PasaManage>();
    public List<PasaManage> currentPlayerPasaList = new List<PasaManage>();

    public List<GameObject> bluePasaWinList = new List<GameObject>();
    public List<GameObject> redPasaWinList = new List<GameObject>();
    public List<GameObject> greenPasaWinList = new List<GameObject>();
    public List<GameObject> yellowPasaWinList = new List<GameObject>();

    public List<int> safeNo;
    public List<GameObject> pasaObjects = new List<GameObject>();

    public int playerRoundChecker;
    public Text winAmount;
    
    public Image pasaImage1;
    public RectTransform pasa1;
    public Image pasaImage2;
    public RectTransform pasa2;
    public Image pasaImage3;
    public RectTransform pasa3;
    public Image pasaImage4;
    public RectTransform pasa4;

    public Sprite[] pasaSprite;

    public int currentPlayerNo;
    private int dicelessPlayerNo;//int to add exception for diceless mode 2 player with bot when player is green
    public int ActivePlayer;
    public int rollingPlayer;

    public int pasaCurrentNo = 1;

    public GameObject generatePasaFireParticles;

    public List<PasaManage> pasaCollectList = new List<PasaManage>();
    public List<PasaManage> pasaBotPlayer = new List<PasaManage>();
    public Image[] subPasaParentImg;

    public Image box1Img;
    public Image box1LineImg;
    public Image box2Img;
    public Image box2LineImg;
    public Image box3Img;
    public Image box3LineImg;
    public Image box4Img;
    public Image box4LineImg;
    public Image[] box1Token;
    public Image[] box2Token;
    public Image[] box3Token;
    public Image[] box4Token;
    public Image box1CircleImg;
    public Image box2CircleImg;
    public Image box3CircleImg;
    public Image box4CircleImg;

    public Image[] box1Lifes;
    public Image[] box2Lifes;
    public Image[] box3Lifes;
    public Image[] box4Lifes;

    public Color lifeOnColor;
    public Color lifeOffColor;


    public Sprite blueBoxSprite;
    public Sprite blueBoxLineSprite;
    public Sprite redBoxSprite;
    public Sprite redBoxLineSprite;
    public Sprite greenBoxSprite;
    public Sprite greenBoxLineSprite;
    public Sprite yellowBoxSprite;
    public Sprite yellowBoxLineSprite;

    public Sprite blueToken;
    public Sprite redToken;
    public Sprite greenToken;
    public Sprite yellowToken;
    public Sprite blueCircleSprite;
    public Sprite redCircleSprite;
    public Sprite greenCircleSprite;
    public Sprite yellowCircleSprite;

    public Text player1Txt;
    public Text player1Id;
    public Text player2Txt;
    public Text player2Id;
    public Text player3Txt;
    public Text player3Id;
    public Text player4Txt;
    public Text player4Id;

    public Text[] playerScores;

    public int playerScoreCnt1;
    public int playerScoreCnt2;
    public int playerScoreCnt3;
    public int playerScoreCnt4;

    public GameObject score56Anim1;
    public GameObject score56Anim2;
    public GameObject score56Anim3;
    public GameObject score56Anim4;

    public Animator shadow1;
    public Animator shadow2;
    public Animator shadow3;
    public Animator shadow4;

    public GameObject idelPasa1;
    public GameObject idelPasa2;
    public GameObject idelPasa3;
    public GameObject idelPasa4;
    
    
    public GameObject ludo1;
    public GameObject ludo2;
    public GameObject ludo3;
    public GameObject ludo4;


    public Image timerFillImg1;
    public Image timerFillImg2;
    public Image timerFillImg3;
    public Image timerFillImg4;
    public float timerSpeed;

    public int isClickAvaliableDice;//if == 1, then player can click on dice and pasa // if == 0 then player cannot click on dice/pasa
    public Color shadowOff;
    float secondsCount = 0;
    int flag = 0;
    public GameObject timerObject;
    public Text timerTxt;
    public GameObject winScreen;
    public bool isPathClick;
    public bool isOtherPlayLeft;
    public bool isTimeFinish;

    public bool isPauseGetData;
    public bool isPathClickAvaliable;

    public List<DiceLessDataStore> diceLessDataStore = new List<DiceLessDataStore>();

    public Color blueColor;
    public Color redColor;
    public Color greenColor;
    public Color yellowColor;

    public Sprite[] profileSprite;

    public bool isLudoOpenWin;
    public Text pasaNoTxt;
    public Text pasaNoTxt2;
    public Text pasaNoTxt3;
    public Text pasaNoTxt4;
    public Color colorOff;
    public Color colorOn;

    public bool isCheckEnter = false;

    public bool isDicelessKillTimeFree;

    public List<int> mainDicelist = new List<int>();
    private bool isApplicationPaused;
    private int clickCounter;
    private bool isSixObtained;
    private Coroutine blinkCoroutine; // Reference to the blinking coroutine
    private Coroutine blinkCoroutine2; // Reference to the blinking coroutine
    float blinkDuration = 0.25f;
    
    public GameObject turnGenObj;
    public GameObject turnObj;
    
    bool isEntered = false;
    public int botPlayerNo = 0;
    public bool isAdmin = false;
    public bool isAdminPause = false;
    public bool isOtherPlayerPause = false;//if non admin player is in pause mode
    public bool isPause = false;//if current player is in pause mode
    public bool isPlayer1Left = false;//this is for bot positioning
    public bool isPlayer2Left = false;//this is for bot positioning
    public bool isPlayer3Left = false;//this is for bot positioning
    public bool hasEmptyPlayerSpace = false;//for shadow maintain

    public bool isSix = false;//when bot has 6 on dice
    public int counter;//for ending the movement
    public List<string> leftPlayerNames = new List<string>();

    //bool botSixManage = false;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        foreach (var item in diceLessDataStore)
        {
            print("List Number = " + item.no);
            int _total = 0;
            foreach (var _item in item.numberList)
                _total += _item;
            print("List Total = " + _total);
        }
        if (DataManager.Instance.isFourPlayer)
        {
            isAdmin = DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id);
            if (isAdmin == false)
                BotManager.Instance.isConnectBot = false;
        }
        GameMoneyCut();
        //if (DataManager.Instance.modeType == 3)
        //{
        //    if (DataManager.Instance.joinPlayerDatas.Count(x => !x.userId.Contains("Ludo")) == 1 && isAdmin)
        //        StartGamePlay();
        //    else if (isAdmin == false)
        //        StartGamePlay();
        //    else if(isAdmin)
        //    {
        //        Time.timeScale = 0;
        //    }
        //}
        //else
            StartGamePlay();
        
        //for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        //{
        //    if(DataManager.Instance.joinPlayerDatas[i].userId.Contains())
        //}
        DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
    }

    private void GameMoneyCut()
    {
        if (DataManager.Instance.tourBonusCut == 0)
        {
            // No tour bonus cut, directly debit from deposit
            DataManager.Instance.DebitAmount(DataManager.Instance.tourEntryMoney.ToString(),TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game", 0);
        }
        else if (DataManager.Instance.tourEntryMoney == 0)
        {
            // Tour entry money is 0, handle separately
            var cutMoney = DataManager.Instance.tourBonusCut;

            if (float.Parse(DataManager.Instance.playerData.bonus) >= cutMoney)
            {
                // Sufficient balance in bonus, cut from bonus
                DataManager.Instance.BonusDebitAmount(cutMoney.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game");
            }
            else
            {
                // Insufficient balance in bonus, cut the available bonus balance
                float bonusBalance = float.Parse(DataManager.Instance.playerData.bonus);
                DataManager.Instance.BonusDebitAmount(bonusBalance.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game");
    
                float remainingMoney = cutMoney - bonusBalance;
                DataManager.Instance.DebitAmount(remainingMoney.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game", 0);
            }
        }
        else
        {
            var remainingBonus = float.Parse(DataManager.Instance.playerData.bonus) - DataManager.Instance.tourBonusCut;
            if (remainingBonus >= 0)
            {
                // Sufficient balance in bonus after applying tour bonus cut
                var cutMoney = DataManager.Instance.tourBonusCut;
                var remainingMoney = DataManager.Instance.tourEntryMoney - cutMoney;
                DataManager.Instance.BonusDebitAmount(cutMoney.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game");
                DataManager.Instance.DebitAmount(remainingMoney.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game", 0);
            }
            else
            {
                // Insufficient balance in bonus after applying tour bonus cut
                if (float.Parse(DataManager.Instance.playerData.bonus) <= 0)
                {
                    DataManager.Instance.DebitAmount(DataManager.Instance.tourEntryMoney.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game", 0);
                }
                else
                {
                    float bonusBalance = float.Parse(DataManager.Instance.playerData.bonus);
                    DataManager.Instance.BonusDebitAmount(bonusBalance.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game");
    
                    float remainingMoney = DataManager.Instance.tourEntryMoney - bonusBalance;
                    DataManager.Instance.DebitAmount(remainingMoney.ToString(), TestSocketIO.Instace.roomid, "Game Play " + TestSocketIO.Instace.roomid, "game", 0);
                }
                
            }
        }
        
    }


    public void GenerateNotATurn()
    {
        Instantiate(notTurnPre, notTurnParent.transform);
    }
    public void PlayerNameManage()
    {
        if (DataManager.Instance.isTwoPlayer == true)
        {

            int index1 = 0;
            int index2 = 1;


            if (DataManager.Instance.playerNo == 3)
            {
                index1 = 1;
                index2 = 0;

            }

            //print("Index 1 : " + index1);
            //print("Index 2 : " + index2);
            player1Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
            player1Id.text = DataManager.Instance.joinPlayerDatas[index1].userId;
            player3Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);
            player3Id.text = DataManager.Instance.joinPlayerDatas[index2].userId;


            //subPasaParentImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index1].avtar];
            //subPasaParentImg[2].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index2].avtar];
            
            DataManager.Instance.LoadProfileImage(DataManager.Instance.joinPlayerDatas[index1].avtar, subPasaParentImg[0]);
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, subPasaParentImg[2]));
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            int index1 = 0;
            int index2 = 1;
            int index3 = 2;
            int index4 = 3;
            if (DataManager.Instance.playerNo == 1)
            {
                index1 = 0;
                index2 = 1;
                index3 = 2;
                index4 = 3;
            }
            else if (DataManager.Instance.playerNo == 2)
            {
                index1 = 1;
                index2 = 2;
                index3 = 3;
                index4 = 0;
            }
            else if(DataManager.Instance.playerNo == 3)
            {
                index1 = 2;
                index2 = 3;
                index3 = 0;
                index4 = 1;
            }
            else if (DataManager.Instance.playerNo == 4)
            {
                index1 = 3;
                index2 = 0;
                index3 = 1;
                index4 = 2;
            }
            player1Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
            player1Id.text = DataManager.Instance.joinPlayerDatas[index1].userId;
            player2Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);
            player2Id.text = DataManager.Instance.joinPlayerDatas[index2].userId;
            player3Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index3].userName);
            player3Id.text = DataManager.Instance.joinPlayerDatas[index3].userId;
            player4Txt.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index4].userName);
            player4Id.text = DataManager.Instance.joinPlayerDatas[index4].userId;
            
            //subPasaParentImg[0].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index1].avtar];
            //subPasaParentImg[1].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index2].avtar];
            //subPasaParentImg[2].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index1].avtar];
            //subPasaParentImg[3].sprite = profileSprite[DataManager.Instance.joinPlayerDatas[index2].avtar];
            
            DataManager.Instance.LoadProfileImage(DataManager.Instance.joinPlayerDatas[index1].avtar, subPasaParentImg[0]);
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, subPasaParentImg[1]));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index3].avtar, subPasaParentImg[2]));
            StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index4].avtar, subPasaParentImg[3]));
            
        }
    }
    
    public string UserNameStringManage(string name)
    {
        if (name != null && name != "")
        {
            if (name.Length > 13)
            {
                name = name.Substring(0, 10) + "...";
            }
            else
            {
                name = name;
            }
        }
        return name;
    }


    void Timer()
    {
        secondsCount -= Time.deltaTime;
        float minutes = Mathf.Floor(secondsCount / 60);
        float seconds = secondsCount % 60;


        string Min = minutes.ToString();
        string Sec = Mathf.RoundToInt(seconds).ToString();
        if (Min.Length == 1)
        {
            Min = "0" + Min;
        }
        if (Sec.Length == 1)
        {
            Sec = "0" + Sec;
        }
        if (Min.Length != 1 && Sec.Length != 1)
        {
            Min = Min;
            Sec = Sec;
        }

        string timeValue = Min + ":" + Sec;
        if (timeValue.Equals("00:00") || secondsCount <= 0f)
        {
            print("Time Over");
            timerTxt.text = "00:00";
            WinUserShow();
            flag = 1;
        }
        if (flag != 1)
        {
            timerTxt.text = timeValue;
        }
    }


    public void WinUserShow()
    {
        winScreen.SetActive(true);
    }
    
    public void ClearAllData()
    {
        pasaSocketList.Clear();
        currentPlayerPasaList.Clear();
        pasaObjects.Clear();
    }
    
    public void PlayerJoined()
    {
        Time.timeScale = 1;
        ClearAllData();
        RestartTimer();
        StartGamePlay();
        pasaCurrentNo = UnityEngine.Random.Range(1, 7);
        pasaImage1.sprite = pasaSprite[pasaCurrentNo - 1];
        PlayerNameManage();
        //SoundManager.Instance.//////////////////////////////////Start sound
    }

    public void StartGamePlay()
    {
        //print("Play Time : " + TestSocketIO.Instace.playTime);
        secondsCount = (TestSocketIO.Instace.playTime * 60);
        if (DataManager.Instance.isFourPlayer)
            isAdmin = DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id);
        else
        {
            if (DataManager.Instance.joinPlayerDatas.Any(x => x.userId.Contains("Ludo")))
            {
                isAdmin = true;
                BotManager.Instance.isConnectBot = true;
            }
            if (DataManager.Instance.playerNo == 1)
                isAdmin = true;
        }
        if (Instance == null)
        {
            Instance = this;
        }
        GameInIt();
        currentPlayerNo = DataManager.Instance.playerNo;
        dicelessPlayerNo = currentPlayerNo;
        if (DataManager.Instance.isTwoPlayer)
        {
            SetInitialTurn();
        }
        else if(DataManager.Instance.isFourPlayer)
        {
            if(DataManager.Instance.playerNo == 1)
            {
                SetInitialTurn();
            }
            else
            {
                playerRoundChecker = 1;
                ActivePlayer = 1;
                rollingPlayer = 1;
                DataManager.Instance.isDiceClick = false;
                isClickAvaliableDice = 1;
                DataManager.Instance.isTimeAuto = false;
            }
        }
        
        if (DataManager.Instance.playerNo == 1)
        {
            DataManager.Instance.isDiceClick = true;
            isClickAvaliableDice = 0;
        }
        else if(isAdmin == true && BotManager.Instance.isConnectBot)
        {
            //print("Enter First Enter Bot");
            GenerateDiceNumberStart_Bot(true);
        }
        else if(DataManager.Instance.isTwoPlayer && DataManager.Instance.playerNo != 1)
        {
            DataManager.Instance.isDiceClick = false;
        }
        

        if (DataManager.Instance.playerNo == 1)
        {
            mainDicelist = diceLessDataStore[UnityEngine.Random.Range(0, 10)].numberList;
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            mainDicelist = diceLessDataStore[UnityEngine.Random.Range(10, 20)].numberList;

        }
        else if (DataManager.Instance.playerNo == 3)
        {
            mainDicelist = diceLessDataStore[UnityEngine.Random.Range(20, 30)].numberList;

        }
        else if (DataManager.Instance.playerNo == 4)
        {
            mainDicelist = diceLessDataStore[UnityEngine.Random.Range(30, 40)].numberList;

        }

        playerScores[0].text = playerScoreCnt1.ToString();
        playerScores[1].text = playerScoreCnt2.ToString();
        playerScores[2].text = playerScoreCnt3.ToString();
        playerScores[3].text = playerScoreCnt4.ToString();

        score56Anim1.SetActive(false);
        score56Anim2.SetActive(false);
        score56Anim3.SetActive(false);
        score56Anim4.SetActive(false);

        timerFillImg1.color = blueColor;

        if (DataManager.Instance.isTwoPlayer)
        {
            if (DataManager.Instance.modeType == 1 || DataManager.Instance.modeType == 2)
            {
                pasaImage1.color = colorOn;
                pasaImage3.color = colorOn;
                pasaNoTxt.color = colorOff;
                pasaNoTxt3.color = colorOff;
            }
            else
            {
                pasaImage1.color = colorOff;
                pasaImage3.color = colorOff;
                pasaNoTxt.color = colorOn;
                pasaNoTxt3.color = colorOn;
            }
        }
        else if(DataManager.Instance.isFourPlayer)
        {
            if (DataManager.Instance.modeType == 1 || DataManager.Instance.modeType == 2)
            {
                pasaImage1.color = colorOn;
                pasaImage2.color = colorOn;
                pasaImage3.color = colorOn;
                pasaImage4.color = colorOn;
                pasaNoTxt.color = colorOff;
                pasaNoTxt2.color = colorOff;
                pasaNoTxt3.color = colorOff;
                pasaNoTxt4.color = colorOff;
            }
            else
            {
                pasaImage1.color = colorOff;
                pasaImage2.color = colorOff;
                pasaImage3.color = colorOff;
                pasaImage4.color = colorOff;
                pasaNoTxt.color = colorOn;
                pasaNoTxt2.color = colorOn;
                pasaNoTxt3.color = colorOn;
                pasaNoTxt4.color = colorOn;
            }
        }
        //if(isAdmin == false && DataManager.Instance.modeType == 3)
        //{
        //    JSONObject obj = new JSONObject();
        //    obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        //    obj.AddField("TournamentID", DataManager.Instance.tournamentID);

        //    TestSocketIO.Instace.Senddata("Startgame", obj);
        //}

        //if(isAdmin && DataManager.Instance.modeType == 3)
        //{
        //    DiceLessPasaButton();
        //}
    }

    public void ShowStartGamePopup()
    {
        startGamePopup.SetActive(true);
        
        startGamePopup.transform.localScale = Vector3.zero;
        startGamePopup.transform.DOScale(Vector3.one, 0.25f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                startGamePopup.transform.DOScale(Vector3.zero, 0.25f)
                    .SetEase(Ease.InBack)
                    .SetDelay(0.75f)
                    .OnComplete(() => { startGamePopup.SetActive(false); });
            });
    }
    
    private void SetInitialTurn()
    {
        
        if (DataManager.Instance.playerNo == 1)
        {
            playerRoundChecker = 1;
            ActivePlayer = 1;
            rollingPlayer = 1;
            DataManager.Instance.isDiceClick = true;
            isClickAvaliableDice = 0;
        }
        else
        {
            switch (DataManager.Instance.playerNo)
            {
                case 2:
                {
                    playerRoundChecker = 2;
                    ActivePlayer = 2;
                    rollingPlayer = 2;
                    //DataManager.Instance.isDiceClick = true;
                    break;
                }
                case 3:
                {
                    playerRoundChecker = 3;
                    ActivePlayer = 3;
                    rollingPlayer = 3;
                    //DataManager.Instance.isDiceClick = true;
                    break;
                }
                case 4:
                {
                    playerRoundChecker = 4;
                    ActivePlayer = 4;
                    rollingPlayer = 4;
                    //DataManager.Instance.isDiceClick = true;
                    break;
                }
            }
            //ActivePlayer = 3;
            //print("Enter First Enter Bot");
            // GenerateDiceNumberStart_Bot(true);
        }
        playerRoundChecker = 1;
        ActivePlayer = 1;
        rollingPlayer = 1;
        //DataManager.Instance.isDiceClick = true;
    }



    #region Start Player Manage

    void GameInIt()
    {

        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < box2Img.gameObject.transform.childCount; i++)
            {
                box2Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < box4Img.gameObject.transform.childCount; i++)
            {
                box4Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (DataManager.Instance.playerNo == 1)
        {
            box1Img.sprite = blueBoxSprite;
            box1LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box1Token, blueToken);
            box1CircleImg.sprite = blueCircleSprite;

            box2Img.sprite = redBoxSprite;
            box2LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box2Token, redToken);
            box2CircleImg.sprite = redCircleSprite;

            box3Img.sprite = greenBoxSprite;
            box3LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box3Token, greenToken);
            box3CircleImg.sprite = greenCircleSprite;

            box4Img.sprite = yellowBoxSprite;
            box4LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box4Token, yellowToken);
            box4CircleImg.sprite = yellowCircleSprite;

            shadow1.enabled = true;
            shadow2.enabled = false;
            shadow3.enabled = false;
            shadow4.enabled = false;
            if (DataManager.Instance.isTwoPlayer)
            {
                ludo1.gameObject.SetActive(true);
                ludo2.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(false);
                ludo4.gameObject.SetActive(false);
                idelPasa1.gameObject.SetActive(true);
                idelPasa2.gameObject.SetActive(false);
                idelPasa3.gameObject.SetActive(true);
                idelPasa4.gameObject.SetActive(false);
            }
            else if(DataManager.Instance.isFourPlayer)
            {
                ludo1.gameObject.SetActive(true);
                ludo2.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(false);
                ludo4.gameObject.SetActive(false);
                idelPasa1.gameObject.SetActive(true);
                idelPasa2.gameObject.SetActive(true);
                idelPasa3.gameObject.SetActive(true);
                idelPasa4.gameObject.SetActive(true);
            }

            //subPasaParentImg[0].sprite = 
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            box1Img.sprite = redBoxSprite;
            box1LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box1Token, redToken);
            box1CircleImg.sprite = redCircleSprite;

            box2Img.sprite = greenBoxSprite;
            box2LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box2Token, greenToken);
            box2CircleImg.sprite = greenCircleSprite;

            box3Img.sprite = yellowBoxSprite;
            box3LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box3Token, yellowToken);
            box3CircleImg.sprite = yellowCircleSprite;

            box4Img.sprite = blueBoxSprite;
            box4LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box4Token, blueToken);
            box4CircleImg.sprite = blueCircleSprite;

            /*shadow1.enabled = false;
            shadow2.enabled = true;
            shadow3.enabled = false;
            shadow4.enabled = false;*/
            
            shadow1.enabled = false;
            shadow2.enabled = false;
            shadow3.enabled = false;
            shadow4.enabled = true;

            //if (DataManager.Instance.isTwoPlayer)
            //{
            //    ludo1.gameObject.SetActive(true);
            //    ludo2.gameObject.SetActive(false);
            //    ludo3.gameObject.SetActive(false);
            //    ludo4.gameObject.SetActive(false);
            //}
            if (DataManager.Instance.isFourPlayer)
            {
                ludo1.gameObject.SetActive(false);
                ludo2.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(false);
                ludo4.gameObject.SetActive(true);
            }
        }
        else if (DataManager.Instance.playerNo == 3)
        {
            box1Img.sprite = greenBoxSprite;
            box1LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box1Token, greenToken);
            box1CircleImg.sprite = greenCircleSprite;

            box2Img.sprite = yellowBoxSprite;
            box2LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box2Token, yellowToken);
            box2CircleImg.sprite = yellowCircleSprite;

            box3Img.sprite = blueBoxSprite;
            box3LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box3Token, blueToken);
            box3CircleImg.sprite = blueCircleSprite;

            box4Img.sprite = redBoxSprite;
            box4LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box4Token, redToken);
            box4CircleImg.sprite = redCircleSprite;

            /*shadow1.enabled = false;
            shadow2.enabled = false;
            shadow3.enabled = true;
            shadow4.enabled = false;*/
            shadow1.enabled = false;
            shadow2.enabled = false;
            shadow3.enabled = true;
            shadow4.enabled = false;

            if (DataManager.Instance.isTwoPlayer)
            {
                ludo1.gameObject.SetActive(false);
                ludo2.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(true);
                ludo4.gameObject.SetActive(false);
                idelPasa1.gameObject.SetActive(true);
                idelPasa2.gameObject.SetActive(false);
                idelPasa3.gameObject.SetActive(true);
                idelPasa4.gameObject.SetActive(false);
            }
            else if(DataManager.Instance.isFourPlayer)
            {
                ludo1.gameObject.SetActive(false);
                ludo2.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(true);
                ludo4.gameObject.SetActive(false);
                idelPasa1.gameObject.SetActive(true);
                idelPasa2.gameObject.SetActive(true);
                idelPasa3.gameObject.SetActive(true);
                idelPasa4.gameObject.SetActive(true);

            }
        }
        else if (DataManager.Instance.playerNo == 4)
        {
            box1Img.sprite = yellowBoxSprite;
            box1LineImg.sprite = yellowBoxLineSprite;
            SetTokenImages(box1Token, yellowToken);
            box1CircleImg.sprite = yellowCircleSprite;

            box2Img.sprite = blueBoxSprite;
            box2LineImg.sprite = blueBoxLineSprite;
            SetTokenImages(box2Token, blueToken);
            box2CircleImg.sprite = blueCircleSprite;

            box3Img.sprite = redBoxSprite;
            box3LineImg.sprite = redBoxLineSprite;
            SetTokenImages(box3Token, redToken);
            box3CircleImg.sprite = redCircleSprite;

            box4Img.sprite = greenBoxSprite;
            box4LineImg.sprite = greenBoxLineSprite;
            SetTokenImages(box4Token, greenToken);
            box4CircleImg.sprite = greenCircleSprite;

            /*shadow1.enabled = false;
            shadow2.enabled = false;
            shadow3.enabled = false;
            shadow4.enabled = true;*/
            
            shadow1.enabled = false;
            shadow2.enabled = true;
            shadow3.enabled = false;
            shadow4.enabled = false;

            //if (DataManager.Instance.isTwoPlayer)
            //{
            //    ludo1.gameObject.SetActive(true);
            //    ludo3.gameObject.SetActive(false);
            //}
            if (DataManager.Instance.isFourPlayer)
            {
                ludo1.gameObject.SetActive(false);
                ludo2.gameObject.SetActive(true);
                ludo3.gameObject.SetActive(false);
                ludo4.gameObject.SetActive(false);
            }
        }

        if (DataManager.Instance.modeType != 3) return;
        idelPasa1.gameObject.SetActive(false);
        idelPasa2.gameObject.SetActive(false);
        idelPasa3.gameObject.SetActive(false);
        idelPasa4.gameObject.SetActive(false);
    }

    void SetTokenImages(Image[] img, Sprite token)
    {
        for (int i = 0; i < img.Length; i++)
        {
            img[i].sprite = token;
        }
    }

    #endregion

    #region Pasa Image Manage

    //public void PasaImageManage(int no, int pNo, bool isSocket)
    //{

    //    subPasaParentImg[pNo - 1].sprite = diceSprite[no - 1];

    //    for (int i = 0; i < subPasaParentImg.Length; i++)
    //    {
    //        if ((pNo - 1) == i)
    //        {
    //            subPasaParentImg[i].GetComponent<Button>().interactable = true;
    //        }
    //        else
    //        {
    //            subPasaParentImg[i].GetComponent<Button>().interactable = false;
    //        }
    //    }
    //}



    #endregion


    #region Score Manage

    public void ScoreManage(int pNo, int plusNumber)
    {
        if (pNo == 1)
        {
            playerScoreCnt1 = playerScoreCnt1 + plusNumber;
            playerScores[0].text = playerScoreCnt1.ToString();
            if (plusNumber > 50)
            {
                score56Anim1.SetActive(true);
                StartCoroutine(OffScore56(score56Anim1));
            }
        }
        else if (pNo == 2)
        {
            playerScoreCnt2 = playerScoreCnt2 + plusNumber;
            playerScores[1].text = playerScoreCnt2.ToString();
            if (plusNumber > 50)
            {
                score56Anim2.SetActive(true);
                StartCoroutine(OffScore56(score56Anim2));
            }
        }
        else if (pNo == 3)
        {
            playerScoreCnt3 = playerScoreCnt3 + plusNumber;
            playerScores[2].text = playerScoreCnt3.ToString();
            if (plusNumber > 50)
            {
                score56Anim3.SetActive(true);
                StartCoroutine(OffScore56(score56Anim3));
            }
        }
        else if (pNo == 4)
        {
            playerScoreCnt4 = playerScoreCnt4 + plusNumber;
            playerScores[3].text = playerScoreCnt4.ToString();
            if (plusNumber > 50)
            {
                score56Anim4.SetActive(true);
                StartCoroutine(OffScore56(score56Anim4));
            }
        }
    }

    public void ScoreManageDecrese(int pNo, int decreseNumber)
    {
        if (pNo == 1)
        {
            playerScoreCnt1 = playerScoreCnt1 - decreseNumber;
            if (playerScoreCnt1 <= 0)
            {
                playerScoreCnt1 = 0;
            }
            playerScores[0].text = playerScoreCnt1.ToString();
        }
        else if (pNo == 2)
        {
            playerScoreCnt2 = playerScoreCnt2 - decreseNumber;
            if (playerScoreCnt2 <= 0)
            {
                playerScoreCnt2 = 0;
            }
            playerScores[1].text = playerScoreCnt2.ToString();
        }
        else if (pNo == 3)
        {
            playerScoreCnt3 = playerScoreCnt3 - decreseNumber;
            if (playerScoreCnt3 <= 0)
            {
                playerScoreCnt3 = 0;
            }
            playerScores[2].text = playerScoreCnt3.ToString();
        }
        else if (pNo == 4)
        {
            playerScoreCnt4 = playerScoreCnt4 - decreseNumber;
            if (playerScoreCnt4 <= 0)
            {
                playerScoreCnt4 = 0;
            }
            playerScores[3].text = playerScoreCnt4.ToString();
        }
    }

    IEnumerator OffScore56(GameObject obj56)
    {
        yield return new WaitForSeconds(2f);
        obj56.SetActive(false);
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Invoke(nameof(AddBetAmount), 1f);
        clickCounter = 0;
        pasaCurrentNo = UnityEngine.Random.Range(1, 5);
        pasaImage1.sprite = pasaSprite[pasaCurrentNo - 1];
        pasaImage2.sprite = pasaSprite[pasaCurrentNo - 1];
        pasaImage3.sprite = pasaSprite[pasaCurrentNo - 1];
        pasaImage4.sprite = pasaSprite[pasaCurrentNo - 1];
        SoundManager.Instance.StopBackgroundMusic();
        PlayerNameManage();
        timerObject.gameObject.SetActive(true);// setting timer only for timer mode
        ShowStartGamePopup();
    }
    
    public void AddBetAmount()
    {
        
        DataManager.Instance.DebitAmount((DataManager.Instance.betPrice).ToString(), TestSocketIO.Instace.roomid/*DataManager.Instance.gameId*/, "Ludo-Bet-" + TestSocketIO.Instace.roomid/*DataManager.Instance.gameId*/, "game", 0);
    }

    bool isTimeEnter = false;
    public void RestartTimer()
    {
        for (int j = 0; j < currentPlayerPasaList.Count; j++)
        {
            currentPlayerPasaList[j].isReadyForClick = false;
        }
        isTimeEnter = false;
        DataManager.Instance.isRestartManage = true;
        if (DataManager.Instance.isTwoPlayer)
        {
            timerFillImg1.fillAmount = 1;
            timerFillImg3.fillAmount = 1;
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            timerFillImg1.fillAmount = 1;
            timerFillImg2.fillAmount = 1;
            timerFillImg3.fillAmount = 1;
            timerFillImg4.fillAmount = 1;
        }
        //print("Restart Timer : ");
    }
    
    public void ChangeTurn()
    {
        switch (DataManager.Instance.joinPlayerDatas.Count)
        {
            case 4:
                playerRoundChecker = playerRoundChecker switch
                {
                    1 => 2,
                    2 => 3,
                    3 => 4,
                    4 => 1,
                    _ => playerRoundChecker
                };
                break;
            case 3:
                playerRoundChecker = playerRoundChecker switch
                {
                    1 => 2,
                    2 => 3,
                    3 => 1,
                    _ => playerRoundChecker
                };
                break;
            case 2:
                playerRoundChecker = playerRoundChecker switch
                {
                    1 => 2,
                    2 => 1,
                    _ => playerRoundChecker
                };
                break;
            case 1:
                //match over, declare winner
                break;
        }
        if (playerRoundChecker == 0)
            playerRoundChecker = 1;
    }


    public void PlayerChangeTurn()
    {
        isBotTurn = false;
        SoundManager.Instance.TickTimerStop();
        print("Playerchangeturn called " + playerRoundChecker);
        if ( LudoUIManager.Instance.bottomOneLineParent.transform.childCount >= 0)
        {
            if (DataManager.Instance.modeType == 3 && playerRoundChecker == DataManager.Instance.playerNo && DataManager.Instance.isFourPlayer)
                Destroy(LudoUIManager.Instance.bottomOneLineParent.transform.GetChild(0).gameObject);
            else if (DataManager.Instance.modeType == 3 && DataManager.Instance.isTwoPlayer)
                Destroy(LudoUIManager.Instance.bottomOneLineParent.transform.GetChild(0).gameObject);
            if (DataManager.Instance.isFourPlayer ? DataManager.Instance.modeType == 3 && playerRoundChecker == DataManager.Instance.joinPlayerDatas.Last().playerNo && DataManager.Instance.joinPlayerDatas.Last().userId.Contains("Ludo") : false)
            {
                LudoUIManager.Instance.FirstNumberRemove();
                LudoUIManager.Instance.UpdateBottomDropDown();
            }
            //else if (DataManager.Instance.isTwoPlayer && BotManager.Instance.isConnectBot)
            //    GameUIManager.Instance.FirstNumberRemove();

        }
        
        if(DataManager.Instance.isTwoPlayer)
        {
            isClickAvaliableDice = 1;
            DataManager.Instance.isDiceClick = false;
        }

        if (DataManager.Instance.isFourPlayer)
        {
            ChangeTurn();
        }
        //bool deactivateSocket = false;//if mode is diceless, then playerdicechange will be prevented to call twice by this variable
        SoundManager.Instance.UserTurnSound();
        //DataManager.Instance.isDiceClick = false;
        if (playerRoundChecker == DataManager.Instance.playerNo && DataManager.Instance.isFourPlayer && DataManager.Instance.modeType != 3)
        {
            isClickAvaliableDice = 0;
            DataManager.Instance.isDiceClick = true;
        }
        else if(DataManager.Instance.modeType != 3 && DataManager.Instance.isFourPlayer)
        {
            isClickAvaliableDice = 1;
            DataManager.Instance.isDiceClick = false;
        }
        PlayerDiceChange();
        if (playerRoundChecker == DataManager.Instance.playerNo && DataManager.Instance.isFourPlayer && DataManager.Instance.modeType == 3)
        {
            isClickAvaliableDice = 0;
            DataManager.Instance.isDiceClick = true;
            if (DataManager.Instance.modeType == 3)
            {
                DiceLessPasaButton();
                //deactivateSocket = true;
                //return;
            }
        }
        else if(DataManager.Instance.modeType == 3 && DataManager.Instance.isFourPlayer)
        {
            DataManager.Instance.isDiceClick = false;
            isClickAvaliableDice = 1;
        }
        print("Changing turn before moving? for player");
        //if (deactivateSocket == false)
        if (DataManager.Instance.isTwoPlayer)
            OtherShadowMainTain();
        else
            OurShadowMaintain();
        RestartTimer();
        DataManager.Instance.isTimeAuto = false;
    }

    public void ReconnectPasaChange()
    {
        //print("Enter The Player Change");
        //    DataManager.Instance.isDiceClick = true;
        //    DataManager.Instance.isTimeAuto = false;
        ////    PlayerDiceChange();
        //    OurShadowMainTain();
        //    RestartTimer();

        DataManager.Instance.isDiceClick = true;
        isClickAvaliableDice = 0;
        OurShadowMaintain();
        DataManager.Instance.isTimeAuto = false;
        RestartTimer();
    }

    bool isLifeEnter = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLudoOpenWin != false) return;
        if (true)
        {
            Timer();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //LudoManager.Instance.GeneratePasaFire();
            //if (pasaCollectList.Count == 1)
            //{
            //    SoundManager.Instance.TokenKillSound();
            //    pasaCollectList[0].Move_Decrement_Steps();
            //}

            //PlayerChangeTurn();
            //RestartTimer();
        }

        if (true)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                DecreaseFillImageTime(timerFillImg1);
                DecreaseFillImageTime(timerFillImg3);
            }
            else if(DataManager.Instance.isFourPlayer)
            {
                DecreaseFillImageTime(timerFillImg1);
                DecreaseFillImageTime(timerFillImg2);
                DecreaseFillImageTime(timerFillImg3);
                DecreaseFillImageTime(timerFillImg4);
            }
        }

        //if(DataManager.Instance.isTwoPlayer && DataManager.Instance.joinPlayerDatas.Count != 2)
        //{
        //    isOtherPlayLeft = true;
        //    WinUserShow();
        //}
    }
    
    private void DecreaseFillImageTime(Image fillImage)
    {
        fillImage.fillAmount -= 1.0f / timerSpeed * Time.deltaTime;
        if (fillImage.fillAmount == 0 && DataManager.Instance.isTimeAuto == false)
        {
            if (isTimeEnter == true)
            {
                if (DataManager.Instance.isFourPlayer)
                {
                    DataManager.Instance.isDiceClick = true;
                    isClickAvaliableDice = 0;
                }
                if(!DataManager.Instance.isFourPlayer || isAdmin)
                    LifeDecrease();
            }
        }
        else if (fillImage.fillAmount < 0.5f && isTimeEnter == false && DataManager.Instance.isTimeAuto == false)
        {
            isLifeEnter = false;
            isTimeEnter = true;
            if (DataManager.Instance.isDiceClick)
            {
                TickSound();
            }
        }
    }
    public int cntPlayer1 = 0;
    public int cntPlayer2 = 0;
    public int cntPlayer3 = 0;
    public int cntPlayer4 = 0;
    public void LifeDecrease()
    {
        if(isAdmin && DataManager.Instance.isFourPlayer)
            DecreaseLife();
        
        isPathClickAvaliable = false;
        isClickAvaliableDice = 1;
        
        SoundManager.Instance.TickTimerStop();
        if (DataManager.Instance.isTwoPlayer && isLifeEnter == false)
        {
            isLifeEnter = true;
            if (DataManager.Instance.isDiceClick == true)
            {
                SoundManager.Instance.TimeOutSound();
                if (cntPlayer1 == 2)
                {
                    isTimeFinish = true;
                    isOtherPlayLeft = false;
                    WinUserShow();//game ended for losing player
                    Invoke(nameof(KickPlayerForInactivity), 1f);
                    //TestSocketIO.Instace.LeaveRoom();
                    //BotManager.Instance.isBotAvalible = false;
                    //BotManager.Instance.isConnectBot = false;
                    ////SoundManager.Instance.StartBackgroundMusic();
                    //DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
                }
                else
                {
                    box1Lifes[cntPlayer1].color = lifeOffColor;
                    cntPlayer1++;
                }
                //PlayerChangeTurn();
                if (BotManager.Instance.isConnectBot)
                {
                    BotChangeTurn(false, true);
                }
                else
                {
                    PlayerChangeTurn();
                }
            }
            else if (DataManager.Instance.isDiceClick == false && DataManager.Instance.isTimeAuto == false)
            {
                if (cntPlayer2 == 2)
                {
                    leftPlayerNames.Add(DataManager.Instance.joinPlayerDatas.Find(x => x.userId != DataManager.Instance.playerData._id).userName);
                    isOtherPlayLeft = true;
                    WinUserShow();
                }
                else
                {
                    box3Lifes[cntPlayer2].color = lifeOffColor;
                    cntPlayer2++;
                    ReconnectPasaChange();
                }
            }
            // print("Enter The Life is Decrese");
        }
        else if (DataManager.Instance.isFourPlayer && isLifeEnter == false)
        {
            int colour = -1;
            foreach (var item in pasaBotPlayer)
            {
                if (playerRoundChecker == item.updatedPlayerNo)
                {
                    colour = item.orgParentNo;
                    break;
                }
            }
            if(colour == -1)
            {
                foreach (var item in box1Token)
                {
                    var pasa = item.GetComponent<PasaManage>();
                    if(playerRoundChecker == pasa.updatedPlayerNo)
                    {
                        colour = pasa.orgParentNo;
                        break;
                    }
                }
            }

            isLifeEnter = true;
            switch (DataManager.Instance.isDiceClick)
            {
                case true:
                    {
                        if (playerRoundChecker == DataManager.Instance.playerNo && cntPlayer1 == 2)
                        {
                            isTimeFinish = true;
                            isOtherPlayLeft = false;
                            //WinUserShow();
                            //if (isPlayerNextTurn() == false && isAdmin)
                            //{
                            //    BotChangeTurn(false, true);
                            //    //isBotTurn = false;
                            //}
                            //else if (isPlayerNextTurn() == false && isAdmin == false)
                            //{
                            //    ChangeTurn();
                            //    DataManager.Instance.isDiceClick = false;
                            //    isClickAvaliableDice = 1;
                            //    DataManager.Instance.isTimeAuto = false;
                            //    BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, true);
                            //}
                            //else
                            //    PlayerChangeTurn();

                            //GameUIManager.Instance.Setting_LeaveMatch_ButtonClick();
                            WinUserShow();//game ended for losing player
                            Invoke(nameof(KickPlayerForInactivity), 1f);
                            //TestSocketIO.Instace.LeaveRoom();
                            //BotManager.Instance.isBotAvalible = false;
                            //BotManager.Instance.isConnectBot = false;
                            ////SoundManager.Instance.StartBackgroundMusic();
                            //DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
                            return;
                        }
                        else if (isAdminPause && isAdmin && colour == 2 ? cntPlayer2 == 2 : colour == 3 ? cntPlayer3 == 2 : colour == 4 ? cntPlayer4 == 2 : false)
                        {
                            isAdminPause = false;
                            isAdmin = false;
                            DataManager.Instance.joinPlayerDatas.RemoveAt(playerRoundChecker - 1);
                            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                                DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;
                            ChangeAdmin(playerRoundChecker);
                            return;
                        }
                        else if(isAdmin == false /*&& isOtherPlayerPause && (cntPlayer1 == 2 || cntPlayer2 == 2 || cntPlayer3 == 2)*/ )
                        {
                            //DataManager.Instance.joinPlayerDatas.RemoveAt(playerRoundChecker - 1);
                            //for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                            //    DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;
                            //ChangeAdmin(playerRoundChecker);
                            //return;
                        }
                        else if(isAdmin && colour == 2 ? cntPlayer2 == 2 : colour == 3 ? cntPlayer3 == 2 : colour == 4 ? cntPlayer4 == 2 : false)
                        {
                            //DataManager.Instance.joinPlayerDatas.RemoveAt(playerRoundChecker - 1);
                            //for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                            //    DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;
                            //ChangeAdmin(playerRoundChecker);
                            isOtherPlayerPause = false;
                            return;
                        }
                            DataManager.Instance.isDiceClick = false;
                        SoundManager.Instance.TimeOutSound();
                        if ((isAdmin || !hasEmptyPlayerSpace) && !isAdminPause)
                        {
                            
                            switch (isAdmin ? colour : playerRoundChecker)
                            {
                                //case 1 when cntPlayer1 == 2:
                                //    isTimeFinish = true;
                                //    isOtherPlayLeft = false;
                                //    WinUserShow();
                                //    break;
                                    
                                case 1:
                                    switch (DataManager.Instance.playerNo)
                                    {
                                        case 1:
                                            box1Lifes[cntPlayer1].color = lifeOffColor;
                                            cntPlayer1++;
                                            break;
                                        case 2:
                                            box4Lifes[cntPlayer4].color = lifeOffColor;
                                            cntPlayer4++;
                                            break;
                                        case 3:
                                            box3Lifes[cntPlayer3].color = lifeOffColor;
                                            cntPlayer3++;
                                            break;
                                        case 4:
                                            box2Lifes[cntPlayer2].color = lifeOffColor;
                                            cntPlayer2++;
                                            break;

                                    }
                                    //box1Lifes[cntPlayer1].color = lifeOffColor;
                                    //cntPlayer1++;

                                    break;
                                //case 2 when cntPlayer2 == 2:
                                //    isTimeFinish = true;
                                //    isOtherPlayLeft = false;
                                //    WinUserShow();
                                case 2:
                                    switch (DataManager.Instance.playerNo)
                                    {
                                        case 1:
                                            box2Lifes[cntPlayer2].color = lifeOffColor;
                                            cntPlayer2++;
                                            break;
                                        case 2:
                                            box1Lifes[cntPlayer1].color = lifeOffColor;
                                            cntPlayer1++;
                                            break;
                                        case 3:
                                            box4Lifes[cntPlayer4].color = lifeOffColor;
                                            cntPlayer4++;
                                            break;
                                        case 4:
                                            box3Lifes[cntPlayer3].color = lifeOffColor;
                                            cntPlayer3++;
                                            break;

                                    }
                                    break;
                                //case 3 when cntPlayer3 == 2:
                                //    isTimeFinish = true;
                                //    isOtherPlayLeft = false;
                                //    WinUserShow();
                                //    break;
                                case 3:
                                    switch (DataManager.Instance.playerNo)
                                    {
                                        case 1:
                                            box3Lifes[cntPlayer3].color = lifeOffColor;
                                            cntPlayer3++;
                                            break;
                                        case 2:
                                            box2Lifes[cntPlayer2].color = lifeOffColor;
                                            cntPlayer2++;
                                            break;
                                        case 3:
                                            box1Lifes[cntPlayer1].color = lifeOffColor;
                                            cntPlayer1++;
                                            break;
                                        case 4:
                                            box4Lifes[cntPlayer4].color = lifeOffColor;
                                            cntPlayer4++;
                                            break;

                                    }
                                    //box3Lifes[cntPlayer3].color = lifeOffColor;
                                    //cntPlayer3++;
                                    break;
                                //case 4 when cntPlayer4 == 2:
                                //    isTimeFinish = true;
                                //    isOtherPlayLeft = false;
                                //    WinUserShow();
                                //    break;
                                case 4:
                                    switch (DataManager.Instance.playerNo)
                                    {
                                        case 1:
                                            box4Lifes[cntPlayer4].color = lifeOffColor;
                                            cntPlayer4++;
                                            break;
                                        case 2:
                                            box3Lifes[cntPlayer3].color = lifeOffColor;
                                            cntPlayer3++;
                                            break;
                                        case 3:
                                            box2Lifes[cntPlayer2].color = lifeOffColor;
                                            cntPlayer2++;
                                            break;
                                        case 4:
                                            box1Lifes[cntPlayer1].color = lifeOffColor;
                                            cntPlayer1++;
                                            break;
                                    }
                                    //box4Lifes[cntPlayer4].color = lifeOffColor;
                                    //cntPlayer4++;
                                    break;
                            }
                            //if(isAdmin)
                            //{
                            //    if(cntPlayer2 == 2)
                            //    {
                            //        DataManager.Instance.joinPlayerDatas.RemoveAt(1);
                            //        ChangeAdmin(colour);
                            //    }
                            //}
                        }
                        else
                        {
                            print("Colour = " + colour);
                            switch (colour)
                            {
                                case 1:
                                    box1Lifes[cntPlayer1].color = lifeOffColor;
                                    cntPlayer1++;
                                    break;
                                case 2:
                                    box2Lifes[cntPlayer2].color = lifeOffColor;
                                    cntPlayer2++;
                                    break;
                                case 3:
                                    box3Lifes[cntPlayer3].color = lifeOffColor;
                                    cntPlayer3++;
                                    break;
                                case 4:
                                    box4Lifes[cntPlayer4].color = lifeOffColor;
                                    cntPlayer4++;
                                    break;

                            }
                        }
                        //if (isPlayerNextTurn() == false)
                        //{
                        //    BotChangeTurn(false, true);
                        //}
                        //else
                        //{
                        //    PlayerChangeTurn();//uncomment if facing problems in changing turn after life is decreased
                        //}
                        //if (playerRoundChecker == DataManager.Instance.playerNo && cntPlayer1 == 3)
                        //{
                        //    isTimeFinish = true;
                        //    isOtherPlayLeft = false;
                        //    WinUserShow();
                        //    if (isPlayerNextTurn() == false && isAdmin)
                        //        BotChangeTurn(false, true);
                        //    else if (isPlayerNextTurn() == false && isAdmin == false)
                        //    {
                        //        ChangeTurn();
                        //        DataManager.Instance.isDiceClick = false;
                        //        isClickAvaliableDice = 1;
                        //        DataManager.Instance.isTimeAuto = false;
                        //        BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, true);
                        //    }
                        //    else
                        //        PlayerChangeTurn();

                        //    TestSocketIO.Instace.LeaveRoom();
                        //    break;
                        //}
                        //else if (isAdminPause && isAdmin && colour == 2 ? cntPlayer2 == 3 : colour == 3 ? cntPlayer3 == 3 : colour == 4 ? cntPlayer4 == 3 : false)
                        //{
                        //    isAdminPause = false;
                        //    isAdmin = false;
                        //    DataManager.Instance.joinPlayerDatas.RemoveAt(playerRoundChecker - 1);
                        //    for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                        //        DataManager.Instance.joinPlayerDatas[i].playerNo = i + 1;
                        //    ChangeAdmin(playerRoundChecker);
                        //    return;
                        //}
                        if (isAdmin)
                        {
                            if (isPlayerNextTurn() == false && isAdmin)
                            {
                                print("Enter The Direct Condition");
                                BotChangeTurn(false, true);
                            }
                            else if (isPlayerNextTurn() == false && isAdmin == false)
                            {

                                //playerRoundChecker = playerRoundChecker switch
                                //{
                                //    1 => 2,
                                //    2 => 3,
                                //    3 => 4,
                                //    4 => 1,
                                //    _ => playerRoundChecker
                                //};
                                ChangeTurn();
                                DataManager.Instance.isDiceClick = false;
                                isClickAvaliableDice = 1;
                                DataManager.Instance.isTimeAuto = false;
                                BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, true);
                            }
                            else
                                PlayerChangeTurn();
                        }
                        break;
                    }
                case false when DataManager.Instance.isTimeAuto == false:
                    {
                        switch (playerRoundChecker)
                        {
                            // if (cntPlayer2 == 3)
                            // {
                            //     isOtherPlayLeft = true;
                            //     WinUserShow();
                            // }
                            // else
                            // {
                            //     box3Lifes[cntPlayer2].color = lifeOffColor;
                            //     cntPlayer2++;
                            //     ReconnectPasaChange();
                            // }
                            case 1 when cntPlayer1 == 2:
                                isOtherPlayLeft = true;
                                isTimeFinish = true;
                                //GameUIManager.Instance.Setting_LeaveMatch_ButtonClick();//WinUserShow();
                                WinUserShow();//game ended for losing player
                                Invoke(nameof(KickPlayerForInactivity), 1f);
                                
                                break;
                            case 1:
                                box1Lifes[cntPlayer1].color = lifeOffColor;
                                cntPlayer1++;
                                ReconnectPasaChange();
                                break;
                            case 2 when cntPlayer2 == 2:
                                isOtherPlayLeft = true;
                                WinUserShow();
                                break;
                            case 2:
                                box2Lifes[cntPlayer2].color = lifeOffColor;
                                cntPlayer2++;
                                ReconnectPasaChange();
                                break;
                            case 3 when cntPlayer3 == 2:
                                isOtherPlayLeft = true;
                                WinUserShow();
                                break;
                            case 3:
                                box3Lifes[cntPlayer3].color = lifeOffColor;
                                cntPlayer3++;
                                ReconnectPasaChange();
                                break;
                            case 4 when cntPlayer4 == 2:
                                isOtherPlayLeft = true;
                                WinUserShow();
                                break;
                            case 4:
                                box4Lifes[cntPlayer4].color = lifeOffColor;
                                cntPlayer4++;
                                ReconnectPasaChange();
                                break;
                        }

                        break;
                    }
            }
            // print("Enter The Life is Decrese");
        }
    }

    private void KickPlayerForInactivity()
    {
        TestSocketIO.Instace.LeaveRoom();
        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.isConnectBot = false;
        //SoundManager.Instance.StartBackgroundMusic();
        DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
    }
    
    #region Admin Maintain

    public void ChangeAdmin(int playerNo)
    {
        int playerTobeRemoved = -1;
        hasEmptyPlayerSpace = true;
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId) && isAdmin == false)
        {
            switch (playerNo)
            {
                case 2:
                    isPlayer2Left = true;
                    break;
                case 3:
                    isPlayer3Left = true;
                    break;
            }
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Any(x => x.userId.Contains("Ludo")))
                BotManager.Instance.isConnectBot = true;
        }
        else if(!DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
            isAdmin = false;
        foreach (var item in pasaBotPlayer)
        {
            if (item.updatedPlayerNo == playerNo)
            {
                playerTobeRemoved = item.orgParentNo;
                break;
            }
        }
        foreach (var item in pasaObjects)
        {

        }
        pasaObjects.RemoveAll(x => x.GetComponent<PasaManage>().updatedPlayerNo == playerNo);
        pasaBotPlayer.RemoveAll(x => x.updatedPlayerNo == playerNo);
        pasaSocketList.RemoveAll(x => x.updatedPlayerNo == playerNo);
        switch (playerTobeRemoved)
        {
            case 1:
                playerScoreCnt1 = -1;
                for (int i = 0; i < box1Img.gameObject.transform.childCount; i++)
                    box1Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
                shadow1.enabled = false;
                foreach (var item in box1Token)
                    Destroy(item.gameObject);
                
                break;
            case 2:
                playerScoreCnt2 = -1;
                for (int i = 0; i < box2Img.gameObject.transform.childCount; i++)
                    box2Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
                shadow2.enabled = false;
                foreach (var item in box2Token)
                    Destroy(item.gameObject);
                break;
            case 3:
                playerScoreCnt3 = -1;
                for (int i = 0; i < box3Img.gameObject.transform.childCount; i++)
                    box3Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
                shadow3.enabled = false;
                foreach (var item in box3Token)
                    Destroy(item.gameObject);
                break;
            case 4:
                playerScoreCnt4 = -1;
                for (int i = 0; i < box4Img.gameObject.transform.childCount; i++)
                    box4Img.gameObject.transform.GetChild(i).gameObject.SetActive(false);
                shadow4.enabled = false;
                foreach (var item in box4Token)
                    Destroy(item.gameObject);
                break;
        }
        foreach (var item in pasaBotPlayer)
        {
            if (item.updatedPlayerNo > playerNo)
                item.updatedPlayerNo--;
        }
        if (box1Token.Length > 0)
        {
            foreach (var item in box1Token)
            {
                var pasa = item.GetComponent<PasaManage>();
                if (pasa.updatedPlayerNo > playerNo)
                    pasa.updatedPlayerNo--;
                else
                    break;
            }
        }
        DataManager.Instance.playerNo = DataManager.Instance.joinPlayerDatas.Find(x => x.userId == DataManager.Instance.playerData._id).playerNo;
        if (playerNo == playerRoundChecker && isAdmin)
        {
            playerRoundChecker--;
            if (isPlayerNextTurn() == false && isAdmin)
            {
                print("Enter The Direct Condition");
                BotChangeTurn(false, true);
            }
            else if (isPlayerNextTurn() == false && isAdmin == false)
            {
                ChangeTurn();
                DataManager.Instance.isDiceClick = false;
                isClickAvaliableDice = 1;
                DataManager.Instance.isTimeAuto = false;
                BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, true);
            }
            else
                PlayerChangeTurn();

        }
        else if (isAdmin && playerRoundChecker != 1)
            playerRoundChecker--;
        else if (playerNo == playerRoundChecker)
            playerRoundChecker--;
        else if (playerNo <= DataManager.Instance.playerNo)
            playerRoundChecker--;
        //else if(playerRoundChecker == currentPlayerNo)
        if (playerRoundChecker == 0 && DataManager.Instance.playerNo != 1 && playerNo != 1)
            playerRoundChecker = 1;


        if (playerRoundChecker <= DataManager.Instance.joinPlayerDatas.Count && DataManager.Instance.isFourPlayer && playerRoundChecker - 1 >= 0)
        {
            if (DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId.Contains("Ludo") && isAdmin)
            {
                Invoke(nameof(WaitTurnChangeAfter), UnityEngine.Random.Range(0.95f, 1.5f));
            }
        }

        //if (DataManager.Instance.joinPlayerDatas.Count == 1)
        //{
        //    isOtherPlayLeft = true;
        //    WinUserShow();
        //}
    }

    #endregion



    public void TimerStop()
    {
        DataManager.Instance.isTimeAuto = true;
        if (DataManager.Instance.isTwoPlayer)
        {
            timerFillImg1.fillAmount = 0;
            timerFillImg3.fillAmount = 0;
        }
        else if(DataManager.Instance.isFourPlayer)
        {
            timerFillImg1.fillAmount = 0;
            timerFillImg2.fillAmount = 0;
            timerFillImg3.fillAmount = 0;
            timerFillImg4.fillAmount = 0;
        }
    }




    void TickSound()
    {
        SoundManager.Instance.TickTimerSound();
        Invoke(nameof(CheckTickSound), 1f);
    }

    void CheckTickSound()
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            if (timerFillImg1.fillAmount != 0 && timerFillImg3.fillAmount != 0 && DataManager.Instance.isTimeAuto == false && DataManager.Instance.isDiceClick == true)
                TickSound();
        }
        else if(DataManager.Instance.isFourPlayer)
        {
            if (timerFillImg1.fillAmount != 0 && timerFillImg2.fillAmount != 0 &&
                timerFillImg3.fillAmount != 0 && timerFillImg4.fillAmount != 0 &&
                DataManager.Instance.isTimeAuto == false && DataManager.Instance.isDiceClick == true)
                TickSound();
        }
    }

    #region All Player On First

    public void TokenAllNumberFirst()
    {
        for (int i = 0; i < currentPlayerPasaList.Count; i++)
        {
            currentPlayerPasaList[i].GetComponent<PasaManage>().PasaOnFirst();
        }
        Invoke(nameof(AutoFourPlayerBackFirst), 0.2f);
    }




    #endregion

    #region Generate Random Number pasa
    int isSix1 = 0;
    int isSix2 = 0;
    public void PasaButtonClick()
    {
        if (DataManager.Instance.isDiceClick == true && isClickAvaliableDice == 0 && DataManager.Instance.modeType != 3)
        {

            isCheckEnter = false;

            isClickAvaliableDice = 1;
            if (DataManager.Instance.modeType != 3)
                SoundManager.Instance.RollDice_Start_Sound();
            if (DataManager.Instance.isTwoPlayer)
            {
                pasaImage1.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage2.gameObject.GetComponent<Animator>().enabled = true;
            }
            else if(DataManager.Instance.isFourPlayer)
            {
                pasaImage1.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage2.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage3.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage4.gameObject.GetComponent<Animator>().enabled = true;
            }
            isPathClickAvaliable = false;
            //if (DataManager.Instance.isAvaliable)
            //{
            //    pasaCurrentNo = DataManager.Instance.currentPNo;
            //}
            //else
            //{
            //    //pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            //    if (clickCounter < 3 && UnityEngine.Random.Range(0, 10) < 8 && !isSixObtained) // 90% chance for a 6 within the first three clicks
            //    {
            //        pasaCurrentNo = 6;
            //        isSixObtained = true;
            //    }
            //    else
            //    {
            //        pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            //    }
            //    clickCounter++;

            //}
            if(currentPlayerPasaList.Count <= 2 && currentPlayerPasaList.Any(x => x.pasaCurrentNo >= 52 ) && DataManager.Instance.joinPlayerDatas.Any(x => x.userId.Contains("Ludo")))
            {
                foreach (var item in currentPlayerPasaList)
                {
                    if(item.pasaCurrentNo >= 52)
                    {
                        List<int> numberList = new List<int>() { 1, 2, 3, 4, 5, 6};
                        numberList.Remove(57 - item.pasaCurrentNo);
                        int index = UnityEngine.Random.Range(0, numberList.Count);
                        pasaCurrentNo = numberList[index];
                        break;
                    }
                }
            }
            else
                pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            if (pasaCurrentNo == 6)
            {
                if (isSix1 == 0)
                {
                    isSix1 = 1;
                }
                else if (isSix2 == 0)
                {
                    isSix2 = 1;
                }
                else if (isSix1 == 1 && isSix2 == 1)
                {
                    isSix1 = 0;
                    isSix2 = 0;
                    pasaCurrentNo = UnityEngine.Random.Range(1, 6);
                }
            }
            isPathClick = true;
            isClickAvaliableDice = 1;
            print("Button clicked");
            PlayerDice(pasaCurrentNo);

            if (DataManager.Instance.modeType == 1)
            {
                isCheckEnter = true;
            }
            //Invoke(nameof(GenerateDiceNumber), 1.25f);
            StartCoroutine(GenerateDiceNumber());

        }
        else
        {
            GenerateNotATurn();
        }

    }

    public void StopPasaZoom()
    {
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].playerNo == currentPlayerNo)
            {
                pasaCollectList[i].isStopZoom = true;
            }
        }
    }

    public void DiceLessPasaButton()
     {
        if (DataManager.Instance.isDiceClick == true && isClickAvaliableDice == 0 && DataManager.Instance.modeType == 3)
        {

            isCheckEnter = false;

            //SoundManager.Instance.RollDice_Start_Sound();
            //pasaImage.gameObject.GetComponent<Animator>().enabled = true;
            isPathClickAvaliable = false;

            //if (DataManager.Instance.isAvaliable)
            //{
            //    pasaCurrentNo = DataManager.Instance.currentPNo;
            //}
            //else
            //{
            //    pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            //}
            //if (pasaCurrentNo == 6)
            //{
            //    if (isSix1 == 0)
            //    {
            //        isSix1 = 1;
            //    }
            //    else if (isSix2 == 0)
            //    {
            //        isSix2 = 1;
            //    }
            //    else if (isSix1 == 1 && isSix2 == 1)
            //    {
            //        isSix1 = 0;
            //        isSix2 = 0;
            //        pasaCurrentNo = UnityEngine.Random.Range(1, 5);
            //    }
            //}

            pasaCurrentNo = mainDicelist[LudoUIManager.Instance.moveCnt];
            LudoUIManager.Instance.FirstNumberYellow();
            //Invoke(nameof(CallFirstYellow), 0.1f);
            isPathClick = true;
            isClickAvaliableDice = 1;
            print("My turn");
            print("dice number = " + pasaCurrentNo + " list indicator = " + mainDicelist[LudoUIManager.Instance.moveCnt]);
            PlayerDice(pasaCurrentNo);

            //Invoke(nameof(GenerateDiceNumber), 0f);
            StartCoroutine(GenerateDiceNumber());

        }
    }

    private void CallFirstYellow()
    {
        LudoUIManager.Instance.FirstNumberYellow();
    }

    public void DiceLessPasaButton_Kill()
    {
        if (DataManager.Instance.isDiceClick == true && isClickAvaliableDice == 0)
        {

            isCheckEnter = false;
            isClickAvaliableDice = 1;////////////
            //SoundManager.Instance.RollDice_Start_Sound();
            //pasaImage.gameObject.GetComponent<Animator>().enabled = true;
            isPathClickAvaliable = false;
            if (DataManager.Instance.isAvaliable)
            {
                pasaCurrentNo = DataManager.Instance.currentPNo;
            }
            else
            {
                pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            }
            if (pasaCurrentNo == 6)
            {
                if (isSix1 == 0)
                {
                    isSix1 = 1;
                }
                else if (isSix2 == 0)
                {
                    isSix2 = 1;
                }
                else if (isSix1 == 1 && isSix2 == 1)
                {
                    isSix1 = 0;
                    isSix2 = 0;
                    pasaCurrentNo = UnityEngine.Random.Range(1, 5);
                }
            }

            pasaCurrentNo = UnityEngine.Random.Range(1, 7);
            print("-------------DiceLesspasakill----------------------");
            pasaNoTxt.text = "";
            pasaNoTxt2.text = "";
            //GameUIManager.Instance.FirstNumberYellow();
            isPathClick = true;
            isClickAvaliableDice = 1;
            print("Kill?");
            PlayerDice(pasaCurrentNo);

            //Invoke(nameof(GenerateDiceNumber), 0f);
            
            StartCoroutine(GenerateDiceNumber());

        }
    }

    IEnumerator GenerateDiceNumber()
    {
        yield return new WaitForSeconds(0.5f);
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = false;
            pasa1.localScale = Vector3.one;
            pasa3.localScale = Vector3.one;

            SoundManager.Instance.RollDice_Stop_Sound();
            //        PasaImageManage(pasaCurrentNo, 1, false);
            pasaImage1.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage3.sprite = pasaSprite[pasaCurrentNo - 1];

            pasaNoTxt.text = pasaCurrentNo.ToString();
            pasaNoTxt3.text = pasaCurrentNo.ToString();
            print("-----------------Generate dice number is called for 2 player------------------------- dice =  " + pasaCurrentNo);
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage2.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage4.gameObject.GetComponent<Animator>().enabled = false;
            pasa1.localScale = Vector3.one;
            pasa2.localScale = Vector3.one;
            pasa3.localScale = Vector3.one;
            pasa4.localScale = Vector3.one;

            SoundManager.Instance.RollDice_Stop_Sound();
            //        PasaImageManage(pasaCurrentNo, 1, false);
            pasaImage1.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage2.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage3.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage4.sprite = pasaSprite[pasaCurrentNo - 1];

            pasaNoTxt.text = pasaCurrentNo.ToString();
            pasaNoTxt2.text = pasaCurrentNo.ToString();
            pasaNoTxt3.text = pasaCurrentNo.ToString();
            pasaNoTxt4.text = pasaCurrentNo.ToString();
            print("-----------------Generate dice number is called for 4 player------------------------- ");


        }

            yield return new WaitForSeconds(0.75f);
        
        CheckPasaThePlayer();

        bool isBotChangeTurn = false;
        if (pasaCurrentNo == 6)
        {
            for (int i = 0; i < currentPlayerPasaList.Count; i++)
            {
                currentPlayerPasaList[i].isReadyForClick = true;
            }
        }
        else
        {
            if (pasaCollectList.Count == 0)
            {
                if (pasaCurrentNo != 6)
                {
                    isBotChangeTurn = true;
                    CheckThePasaBool();
                }

            }
        }

        if (pasaCollectList.Count != 4 && pasaCurrentNo == 6 && currentPlayerPasaList.Any(x => x.pasaCurrentNo + pasaCurrentNo <= 57))
        {

            isPathClickAvaliable = false;
            isCheckEnter = false;
            // isPathClick = true;//GG
            //return;
            yield break;
        }
        //else if(pasaCurrentNo == 6)
        //{
        //    isClickAvaliableDice = 0;
        //    RestartTimer();
        //}
        else
        {
            if (pasaCollectList.Count == 1)
            {
                if ((pasaCurrentNo + pasaCollectList[0].pasaCurrentNo) > 57)
                {
                    isClickAvaliableDice = 0;
                    if (DataManager.Instance.isTwoPlayer)
                    {
                        if (BotManager.Instance.isConnectBot)
                        {
                            BotChangeTurn(false, true);

                        }
                        else
                        {
                            if (BotManager.Instance.isConnectBot)
                            {
                                BotChangeTurn(false, true);

                            }
                            else
                            {
                                PlayerChangeTurn();
                            }
                        }
                        isCheckEnter = false;
                        
                    }
                    else if (DataManager.Instance.isFourPlayer)
                    {
                        //if (isPlayerNextTurn() == false)
                        //    BotChangeTurn(false, true);
                        //else
                        //    PlayerChangeTurn();

                        if (isPlayerNextTurn() == false && isAdmin)
                        {
                            print("Enter The Direct Condition");
                            BotChangeTurn(false, true);
                        }
                        else if (isPlayerNextTurn() == false && isAdmin == false)
                        {

                            //playerRoundChecker = playerRoundChecker switch
                            //{
                            //    1 => 2,
                            //    2 => 3,
                            //    3 => 4,
                            //    4 => 1,
                            //    _ => playerRoundChecker
                            //};
                            ChangeTurn();
                            DataManager.Instance.isDiceClick = false;
                            isClickAvaliableDice = 1;
                            DataManager.Instance.isTimeAuto = false;
                            BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, true);
                        }
                        else
                            PlayerChangeTurn();

                        isCheckEnter = false;
                        //return;
                    }
                }
                else
                {
                    isPathClickAvaliable = true;
                    MovePlayer(pasaCollectList[0].playerSubNo, pasaCurrentNo);
                    PlayerStopDice();
                    isCheckEnter = true;
                    pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);

                }
            }
            else
            {

                if (pasaCollectList.Count > 0)//auto moving condition
                {
                    int cnt = 0;
                    int pNo = pasaCollectList[0].pasaCurrentNo;
                    for (int i = 0; i < pasaCollectList.Count; i++)
                    {
                        if (pasaCollectList[i].pasaCurrentNo == pNo)
                            cnt++;
                    }
                    if (cnt == pasaCollectList.Count && (pasaCollectList[0].pasaCurrentNo + pasaCurrentNo) <= 57)
                    {
                        //isPathClickAvaliable = true;

                        //pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);
                        //PlayerStopDice();
                        //MovePlayer(pasaCollectList[0].playerSubNo, pasaCurrentNo);



                        if (isBotChangeTurn == false)
                        {
                            isCheckEnter = true;
                            isPathClickAvaliable = true;
                            pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);
                        }
                        else
                        {
                            isCheckEnter = false;
                            // isPathClick = true;//GG
                        }


                        //if (BotManager.Instance.isConnectBot == false)///////////////////////////////////////////
                        //{
                            PlayerStopDice();

                            MovePlayer(pasaCollectList[0].playerSubNo, pasaCurrentNo);
                        //}
                    }
                    else if(pasaCollectList.Count > 1)
                    {
                        int moveAble = 0;
                        int index = -1;
                        int pasaOnSamePosition = 0;
                        int currentPosition = -1;
                        for (int i = 0; i < pasaCollectList.Count; i++)
                        {
                            
                            if (pasaCollectList[i].pasaCurrentNo + pasaCurrentNo <= 57)
                            {
                                moveAble++;
                                index = i;
                            }
                        }
                        if (index != -1)
                        {
                            currentPosition = pasaCollectList[index].pasaCurrentNo;
                            for (int i = 0; i < pasaCollectList.Count; i++)
                            {
                                if (pasaCollectList[i].pasaCurrentNo + pasaCurrentNo <= 57)
                                {
                                    if (currentPosition == pasaCollectList[i].pasaCurrentNo)
                                        pasaOnSamePosition++;
                                }
                            }
                        }

                        if (moveAble == pasaOnSamePosition && index != -1)
                        {
                            if (isBotChangeTurn == false)
                            {
                                isCheckEnter = true;
                                isPathClickAvaliable = true;
                                pasaCollectList[index].Move_Increment_Steps(pasaCurrentNo);
                            }
                            else
                                isCheckEnter = false;
                            PlayerStopDice();
                            MovePlayer(pasaCollectList[index].playerSubNo, pasaCurrentNo);
                        }
                        else if (moveAble == 1 && index != -1)//if only 1 pasa can move
                        {
                            if (isBotChangeTurn == false)
                            {
                                isCheckEnter = true;
                                isPathClickAvaliable = true;
                                pasaCollectList[index].Move_Increment_Steps(pasaCurrentNo);
                            }
                            else
                                isCheckEnter = false;
                            PlayerStopDice();
                            MovePlayer(pasaCollectList[index].playerSubNo, pasaCurrentNo);
                        }
                        else if (currentPlayerPasaList.All(x => x.pasaCurrentNo + pasaCurrentNo > 56))//if no pasa can move or complete under the steps indicated on dice
                        {
                            bool isPasaZoomEnter = false;
                            isPathClickAvaliable = false;
                            isCheckEnter = false;
                            for (int i = 0; i < pasaCollectList.Count; i++)
                            {
                                if (pasaCollectList[i].playerNo == currentPlayerNo)
                                {
                                    // pasaCollectList[i].isFirstZoom = false;
                                    pasaCollectList[i].isStopZoom = false;
                                    isPasaZoomEnter = true;
                                    //pasaCollectList[i].PlayerPasaZoom();
                                }
                            }
                            //isPathClick = true;//GG
                            //if (pasaCurrentNo == 6)
                            //{
                            //    isBotChangeTurn = false;
                            //    isClickAvaliableDice = 0;
                            //    RestartTimer();
                            //}
                            //else
                            //{
                                isBotChangeTurn = true;
                                CheckThePasaBool();
                                PlayerStopDice();
                            //}


                        }
                        else
                        {
                            bool isPasaZoomEnter = false;
                            isPathClickAvaliable = false;
                            isCheckEnter = false;
                            for (int i = 0; i < pasaCollectList.Count; i++)
                            {
                                if (pasaCollectList[i].playerNo == currentPlayerNo)
                                {
                                    // pasaCollectList[i].isFirstZoom = false;
                                    pasaCollectList[i].isStopZoom = false;
                                    isPasaZoomEnter = true;
                                    //pasaCollectList[i].PlayerPasaZoom();
                                }
                            }
                            //isBotChangeTurn = true;
                            //CheckThePasaBool();
                            //PlayerStopDice();
                        }

                    }
                    else
                    {
                        bool isPasaZoomEnter = false;
                        isPathClickAvaliable = false;
                        isCheckEnter = false;
                        for (int i = 0; i < pasaCollectList.Count; i++)
                        {
                            if (pasaCollectList[i].playerNo == currentPlayerNo)
                            {
                                // pasaCollectList[i].isFirstZoom = false;
                                pasaCollectList[i].isStopZoom = false;
                                isPasaZoomEnter = true;
                                //pasaCollectList[i].PlayerPasaZoom();
                            }
                        }

                        //isPathClick = true;//GG
                        isBotChangeTurn = true;
                        CheckThePasaBool();
                        PlayerStopDice();

                    }
                }
                else
                {
                    PlayerStopDice();
                }
            }
        }
    }
    #endregion

    #region Board Box Button

    public void PathButtonClick(int no)
    {
        //print("DataManager.Instance.isDiceClick : " + DataManager.Instance.isDiceClick);
        //print("isPathClick : " + isPathClick);
        //print("isPathClickAvaliable : " + isPathClickAvaliable);
        //print("isCheckEnter : " + isCheckEnter);
        if (DataManager.Instance.isDiceClick == true && isPathClick == true && isPathClickAvaliable == false && isCheckEnter == false)
        {
            //if (currentPlayerNo != DataManager.Instance.playerNo)///////////////////////////////////////
            //{
            //    print("Not a Enter");
            //}
            //else
            //{
            //}

            //isCheckEnter = true;
            CheckPasaThePlayer();
            if (checkPlayerPasa(no))
            {
                SoundManager.Instance.TickTimerStop();
                //isPathClick = false;//uncomment if causing problems

                //isCheckEnter = true;
                StopPasaZoom();
                //StopPasaZoomSix();

                //pasaCollectList[0].Move_Increment_Steps(pasaCurrentNo);
                for (int i = 0; i < pasaObjects.Count; i++)
                {
                    PasaManage pasa = pasaObjects[i].GetComponent<PasaManage>();

                    if (pasa.pasaCurrentNo == no && pasa.updatedPlayerNo == DataManager.Instance.playerNo && (pasa.pasaCurrentNo + pasaCurrentNo) <= 57)
                    {

                        isPathClick = false;
                        isCheckEnter = false;
                        pasaNoTxt.text = "";
                        pasaNoTxt2.text = "";
                        pasa.Move_Increment_Steps(pasaCurrentNo);
                        PlayerStopDice();
                        MovePlayer(pasa.playerSubNo, pasaCurrentNo);
                        break;
                    }
                    //else if(!(pasa.pasaCurrentNo + pasaCurrentNo <= 57))
                    //{

                    //}

                }
            }

        }
    }
    #endregion

    bool checkPlayerPasa(int no)
    {
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo == no && pasaCollectList[i].isPlayer == true)
            {
                return true;
            }
        }
        return false;
    }

    void CheckPasaThePlayer()
    {
        pasaCollectList.Clear();

        for (int i = 0; i < pasaObjects.Count; i++)
        {
            PasaManage pasa = pasaObjects[i].GetComponent<PasaManage>();
            if (pasa.playerNo == currentPlayerNo && pasa.isPlayer == true)
            {
                if (pasa.pasaCurrentNo < 57)
                {
                    pasaCollectList.Add(pasa);
                }
            }
        }

    }

    public void GeneratePasaFire()
    {
        Destroy(Instantiate(generatePasaFireParticles, new Vector3(0, 0, 0f), Quaternion.identity), 6f);
    }

    /*#region Blinking

    public void StartPlayerBlinking(float blinkDuration)
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine); // Stop any existing blinking coroutine

        blinkCoroutine = StartCoroutine(BlinkCoroutine(currentPlayerPasaList, blinkDuration));
    }
    public void StartBotBlinking(float blinkDuration)
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine); // Stop any existing blinking coroutine
        
        blinkCoroutine2 = StartCoroutine(BlinkCoroutine(pasaBotPlayer, blinkDuration));
    }

    public void StopPlayerBlinking()
    {
        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        ResetPasaScales(); // Reset the scale of all pasa objects
    }
    public void StopBotBlinking()
    {
        if (blinkCoroutine2 != null)
            StopCoroutine(blinkCoroutine2);

        ResetPasaScales(); // Reset the scale of all pasa objects
    }

    private IEnumerator BlinkCoroutine(List<PasaManage> pasaList, float blinkDuration)
    {
        float originalScale = 0.55f;
        float highlightScale = 0.7f;
        float elapsed = 0f;

        while (true)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed / blinkDuration, 1f);

            float targetScale = Mathf.Lerp(originalScale, highlightScale, EaseInOutQuad(t));

            SetPasaScales(pasaList, targetScale);

            yield return null;
        }
    }

    private void SetPasaScales(List<PasaManage> pasaList, float scale)
    {
        foreach (PasaManage pasa in pasaList)
        {
            pasa.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void ResetPasaScales()
    {
        foreach (PasaManage pasa in currentPlayerPasaList)
        {
            pasa.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        }

        foreach (PasaManage pasa in pasaBotPlayer)
        {
            pasa.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        }
    }

    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }

    #endregion*/
    
    #region Shadown Maintain
    public void OurShadowMaintain()
    {
        /*StartPlayerBlinking(blinkDuration);///////////////////////////////////////////////////////
        StopBotBlinking();*/
        
        int colour = -1;
        foreach (var item in pasaBotPlayer)
        {
            if (playerRoundChecker == item.updatedPlayerNo)
            {
                colour = item.orgParentNo;
                break;
            }
        }
        
        if (DataManager.Instance.isTwoPlayer)
        {
            shadow1.enabled = true;
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;
            
            ludo1.gameObject.SetActive(true);
            ludo3.gameObject.SetActive(false);

            if (DataManager.Instance.playerNo == 3)
            {
                timerFillImg1.color = greenColor;
                timerFillImg3.color = greenColor;
            }
            else
            {
                timerFillImg1.color = blueColor;
                timerFillImg3.color = blueColor;
            }
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            ludo1.gameObject.SetActive(false);
            ludo2.gameObject.SetActive(false);
            ludo3.gameObject.SetActive(false);
            ludo4.gameObject.SetActive(false);
            if ((isAdmin || !hasEmptyPlayerSpace) && !isAdminPause)
            {
                switch (isAdmin ? colour : playerRoundChecker)
                {
                    case 2:
                        switch (DataManager.Instance.playerNo)
                        {
                            case 1:
                                HighlightSecondPosition();
                                break;
                            case 2:
                                HighlightFirstPosition();
                                break;
                            case 3:
                                HighlightFourthPosition();
                                break;
                            case 4:
                                HighlightThirdPosition();
                                break;
                        }
                        break;
                    case 3:
                        switch (DataManager.Instance.playerNo)
                        {
                            case 1:
                                HighlightThirdPosition();
                                break;
                            case 2:
                                HighlightSecondPosition();
                                break;
                            case 3:
                                HighlightFirstPosition();
                                break;
                            case 4:
                                HighlightFourthPosition();
                                break;
                        }
                        break;
                    case 4:
                        switch (DataManager.Instance.playerNo)
                        {
                            case 1:
                                HighlightFourthPosition();
                                break;
                            case 2:
                                HighlightThirdPosition();
                                break;
                            case 3:
                                HighlightSecondPosition();
                                break;
                            case 4:
                                HighlightFirstPosition();
                                break;
                        }
                        break;
                    default:
                        switch (DataManager.Instance.playerNo)
                        {
                            case 1:
                                HighlightFirstPosition();
                                break;
                            case 2:
                                HighlightFourthPosition();
                                break;
                            case 3:
                                HighlightThirdPosition();
                                break;
                            case 4:
                                HighlightSecondPosition();
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (colour)
                {
                    case 1:
                        HighlightFirstPosition();
                        break;
                    case 2:
                        HighlightSecondPosition();
                        break;
                    case 3:
                        HighlightThirdPosition();
                        break;
                    case 4:
                        HighlightFourthPosition();
                        break;
                    default:
                        HighlightFirstPosition();
                        break;
                }
            }
        }
        /*else
        {
            shadow1.enabled = true;
            shadow2.GetComponent<Image>().color = shadowOff;
            shadow2.enabled = false;
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;
            shadow4.GetComponent<Image>().color = shadowOff;
            shadow4.enabled = false;
            if (DataManager.Instance.playerNo == 3)
            {
                timerFillImg1.color = greenColor;
            }
            else if (DataManager.Instance.playerNo == 2)
            {
                timerFillImg1.color = redColor;
            }
            else if (DataManager.Instance.playerNo == 4)
            {
                timerFillImg1.color = yellowColor;
            }
            else
            {
                timerFillImg1.color = blueColor;
            }
        }*/
    }

    private void HighlightFirstPosition()
    {
        if (shadow1 != null)
            shadow1.enabled = true;
        if (shadow2 != null)
        {
            shadow2.GetComponent<Image>().color = shadowOff;
            shadow2.enabled = false;
        }
        if (shadow3 != null)
        {
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;
        }
        if (shadow4 != null)
        {
            shadow4.GetComponent<Image>().color = shadowOff;
            shadow4.enabled = false;
        }
        if (!shadow1.gameObject.activeInHierarchy)
        {
            if (shadow2.gameObject.activeInHierarchy)
            {
                shadow2.enabled = true;
            }
            else
                shadow3.enabled = true;
        }
        else
            shadow1.enabled = true;

        if (shadow1.enabled == true)
        {
            timerFillImg1.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo1.gameObject.SetActive(true);
        }
        else if (shadow2.enabled == true)
        {
            timerFillImg1.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo2.gameObject.SetActive(true);
        }
        else if (shadow3.enabled == true)
        {
            timerFillImg1.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo3.gameObject.SetActive(true);
        }
        else if (shadow4.enabled == true)
        {
            timerFillImg1.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo4.gameObject.SetActive(true);
        }
    }

    private void HighlightSecondPosition()
    {
        
        if (shadow2 != null)
            shadow2.enabled = true;

        if (shadow1 != null)
        {
            shadow1.GetComponent<Image>().color = shadowOff;
            shadow1.enabled = false;
        }
        if (shadow3 != null)
        {
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;
        }
        if (shadow4 != null)
        {
            shadow4.GetComponent<Image>().color = shadowOff;
            shadow4.enabled = false;
        }
        if (!shadow2.gameObject.activeInHierarchy)
        {
            if (shadow3.gameObject.activeInHierarchy)
            {
                shadow3.enabled = true;
            }
            else
                shadow4.enabled = true;
        }
        else
            shadow2.enabled = true;

        //timerFillImg.color = redColor;
        //if (box2Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box3Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box4Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box1Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;

        if (shadow1.enabled == true)
        {
            timerFillImg1.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo1.gameObject.SetActive(true);
        }
        else if (shadow2.enabled == true)
        {
            timerFillImg1.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo2.gameObject.SetActive(true);
        }
        else if (shadow3.enabled == true)
        {
            timerFillImg1.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo3.gameObject.SetActive(true);
        }
        else if (shadow4.enabled == true)
        {
            timerFillImg1.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo4.gameObject.SetActive(true);
        }
    }

    private void HighlightThirdPosition()
    {
        if (shadow3 != null)
            shadow3.enabled = true;
        if (shadow2 != null)
        {
            shadow2.GetComponent<Image>().color = shadowOff;
            shadow2.enabled = false;
        }
        if (shadow1 != null)
        {
            shadow1.GetComponent<Image>().color = shadowOff;
            shadow1.enabled = false;
        }
        if (shadow4 != null)
        {
            shadow4.GetComponent<Image>().color = shadowOff;
            shadow4.enabled = false;
        }
        if (!shadow3.gameObject.activeInHierarchy)
        {
            if (shadow4.gameObject.activeInHierarchy)
            {
                shadow4.enabled = true;
            }
            else
                shadow1.enabled = true;
        }
        else
            shadow3.enabled = true;
        //timerFillImg.color = greenColor;
        //if (box3Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box4Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box1Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box2Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;

        if (shadow1.enabled == true)
        {
            timerFillImg1.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo1.gameObject.SetActive(true);
        }
        else if (shadow2.enabled == true)
        {
            timerFillImg1.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo2.gameObject.SetActive(true);
        }
        else if (shadow3.enabled == true)
        {
            timerFillImg1.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo3.gameObject.SetActive(true);
        }
        else if (shadow4.enabled == true)
        {
            timerFillImg1.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo4.gameObject.SetActive(true);
        }
    }

    private void HighlightFourthPosition()
    {
        if (shadow4 != null)
            shadow4.enabled = true;
        if (shadow2 != null)
        {
            shadow2.GetComponent<Image>().color = shadowOff;
            shadow2.enabled = false;
        }
        if (shadow3 != null)
        {
            shadow3.GetComponent<Image>().color = shadowOff;
            shadow3.enabled = false;
        }
        if (shadow1 != null)
        {
            shadow1.GetComponent<Image>().color = shadowOff;
            shadow1.enabled = false;
        }
        if (!shadow4.gameObject.activeInHierarchy)
        {
            if (shadow1.gameObject.activeInHierarchy)
            {
                shadow1.enabled = true;
            }
            else
                shadow2.enabled = true;
        }
        else
            shadow4.enabled = true;
        //timerFillImg.color = yellowColor;
        //if (box3Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box1Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box2Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
        //else if (box3Img.transform.GetChild(0).gameObject.activeInHierarchy)
        //    timerFillImg.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;

        if (shadow1.enabled == true)
        {
            timerFillImg1.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box1Img.sprite == blueBoxSprite) ? blueColor : (box1Img.sprite == redBoxSprite) ? redColor : (box1Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo1.gameObject.SetActive(true);
        }
        else if (shadow2.enabled == true)
        {
            timerFillImg1.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box2Img.sprite == blueBoxSprite) ? blueColor : (box2Img.sprite == redBoxSprite) ? redColor : (box2Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo2.gameObject.SetActive(true);
        }
        else if (shadow3.enabled == true)
        {
            timerFillImg1.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box3Img.sprite == blueBoxSprite) ? blueColor : (box3Img.sprite == redBoxSprite) ? redColor : (box3Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo3.gameObject.SetActive(true);
        }
        else if (shadow4.enabled == true)
        {
            timerFillImg1.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg2.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg3.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            timerFillImg4.color = (box4Img.sprite == blueBoxSprite) ? blueColor : (box4Img.sprite == redBoxSprite) ? redColor : (box4Img.sprite == greenBoxSprite) ? greenColor : yellowColor;
            ludo4.gameObject.SetActive(true);
        }
    }


    public void OtherShadowMainTain()
    {
        /*StartBotBlinking(blinkDuration);
        StopPlayerBlinking();*//////////////////////////////////////////////////
        if (DataManager.Instance.isTwoPlayer)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                shadow1.enabled = false;
                shadow1.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = true;
                
                ludo1.gameObject.SetActive(false);
                ludo3.gameObject.SetActive(true);
                if (DataManager.Instance.playerNo == 1)
                {
                    timerFillImg1.color = greenColor;
                    timerFillImg3.color = greenColor;
                }
                else
                {
                    timerFillImg1.color = blueColor;
                    timerFillImg3.color = blueColor;
                }
            }
            else if ((DataManager.Instance.playerNo + 2) == 5)
            {
                shadow1.enabled = true;
                shadow3.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = false;
                if (DataManager.Instance.playerNo == 1)
                {
                    timerFillImg1.color = greenColor;
                }
                else
                {
                    timerFillImg3.color = blueColor;
                }
            }
        }
        /*else
        {
            if ((DataManager.Instance.playerNo + 1) == 2)
            {
                shadow2.enabled = true;
                shadow1.GetComponent<Image>().color = shadowOff;
                shadow1.enabled = false;
                shadow3.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = false;
                shadow4.GetComponent<Image>().color = shadowOff;
                shadow4.enabled = false;
            }
            else if ((DataManager.Instance.playerNo + 1) == 3)
            {
                shadow3.enabled = true;
                shadow2.GetComponent<Image>().color = shadowOff;
                shadow2.enabled = false;
                shadow1.GetComponent<Image>().color = shadowOff;
                shadow1.enabled = false;
                shadow4.GetComponent<Image>().color = shadowOff;
                shadow4.enabled = false;
            }
            else if ((DataManager.Instance.playerNo + 1) == 4)
            {
                shadow4.enabled = true;
                shadow2.GetComponent<Image>().color = shadowOff;
                shadow2.enabled = false;
                shadow3.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = false;
                shadow1.GetComponent<Image>().color = shadowOff;
                shadow1.enabled = false;
            }
            else if ((DataManager.Instance.playerNo + 1) == 5)
            {
                shadow1.enabled = true;
                shadow2.GetComponent<Image>().color = shadowOff;
                shadow2.enabled = false;
                shadow3.GetComponent<Image>().color = shadowOff;
                shadow3.enabled = false;
                shadow4.GetComponent<Image>().color = shadowOff;
                shadow4.enabled = false;
            }
        }*/
        else if(DataManager.Instance.isFourPlayer)
        {
            switch (playerRoundChecker)
            {
                case 2:
                    shadow2.enabled = true;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg1.color = redColor;
                    break;
                case 3:
                    shadow3.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg1.color = greenColor;
                    break;
                case 4:
                    shadow4.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow1.GetComponent<Image>().color = shadowOff;
                    shadow1.enabled = false;
                    timerFillImg1.color = yellowColor;
                    break;
                default:
                    shadow1.enabled = true;
                    shadow2.GetComponent<Image>().color = shadowOff;
                    shadow2.enabled = false;
                    shadow3.GetComponent<Image>().color = shadowOff;
                    shadow3.enabled = false;
                    shadow4.GetComponent<Image>().color = shadowOff;
                    shadow4.enabled = false;
                    timerFillImg1.color = blueColor;
                    break;
            }
        }
    }





    //public void ShadowManage()
    //{
    //    if (DataManager.Instance.playerNo == 1)
    //    {

    //    }
    //    else if (DataManager.Instance.playerNo == 2)
    //    {
    //        shadow2.enabled = true;
    //        shadow1.GetComponent<Image>().color = shadowOff;
    //        shadow1.enabled = false;
    //        shadow3.GetComponent<Image>().color = shadowOff;
    //        shadow3.enabled = false;
    //        shadow4.GetComponent<Image>().color = shadowOff;
    //        shadow4.enabled = false;

    //    }
    //    else if (DataManager.Instance.playerNo == 3)
    //    {

    //    }
    //    else if (DataManager.Instance.playerNo == 4)
    //    {
    //        shadow4.enabled = true;
    //        shadow2.GetComponent<Image>().color = shadowOff;
    //        shadow2.enabled = false;
    //        shadow3.GetComponent<Image>().color = shadowOff;
    //        shadow3.enabled = false;
    //        shadow1.GetComponent<Image>().color = shadowOff;
    //        shadow1.enabled = false;
    //    }


    //}

    #endregion

    #region SocketIO Methods


    #region SEND
    public void MovePlayer(int pasaNo, int diceNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("TokenNo", pasaNo);
        obj.AddField("TokenMove", diceNo);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        if (DataManager.Instance.modeType == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                pasaNoTxt.text = "";
                pasaNoTxt3.text = "";
            }
            else if (DataManager.Instance.isFourPlayer)
            {
                pasaNoTxt.text = "";
                pasaNoTxt2.text = "";
                pasaNoTxt3.text = "";
                pasaNoTxt4.text = "";
            }
        }
        TestSocketIO.Instace.Senddata("LudoData", obj);
    }
    
    public void MoveBot(int pasaNo, int diceNo, string playerId, int playerNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", playerId);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", playerNo);
        obj.AddField("TokenNo", pasaNo);
        obj.AddField("TokenMove", diceNo);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        if (DataManager.Instance.modeType == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                pasaNoTxt.text = "";
                pasaNoTxt3.text = "";
            }
            else if (DataManager.Instance.isFourPlayer)
            {
                pasaNoTxt.text = "";
                pasaNoTxt2.text = "";
                pasaNoTxt3.text = "";
                pasaNoTxt4.text = "";
            }
        }

        TestSocketIO.Instace.Senddata("LudoData", obj);
    }

    void PlayerDice(int diceNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("DiceNo", diceNo);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("DiceManageCnt", DataManager.Instance.diceManageCnt);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        print("Calling diceData when playerRoundChecker = " + playerRoundChecker);

        TestSocketIO.Instace.Senddata("LudoDiceData", obj);
    }
    
    public void BotDice(int diceNo, string playerId, int playerNo, int diceManageCnt)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", playerId);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("DiceNo", diceNo);
        obj.AddField("PlayerNo", playerNo);
        obj.AddField("DiceManageCnt", DataManager.Instance.diceManageCnt);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        //obj.AddField("eventname", "BotDice");
        print("Calling bot dice");
        TestSocketIO.Instace.Senddata("LudoDiceData", obj);
    }
    
    public void DecreaseLife()
    {
        JSONObject obj = new JSONObject();
        
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("DecreaseLife", obj);
    }

    public void PlayerStopDice()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("LudoDiceStopData", obj);
    }
    
    public void BotStopDice(string playerID)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", playerID);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("eventname", "LudoDiceStopData");
        TestSocketIO.Instace.Senddata("LudoDiceStopData", obj);
    }

    public void PauseDataGetRequest()// Called after returning to game after minimizing
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        TestSocketIO.Instace.Senddata("PauseRequest", obj);
    }
    
    public void PauseDataSendRequest()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);

        if (isAdmin)
            obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        else
            obj.AddField("PlayerNo", -1);

        //obj.AddField("users", new JSONObject(users));
        TestSocketIO.Instace.Senddata("PauseSend", obj);
    }
    
    public void ResumeDataSendRequest()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        TestSocketIO.Instace.Senddata("ResumeSend", obj);
    }


    public void PlayerDiceChange()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        int noSend = 0;
        noSend = DataManager.Instance.playerNo;

        if (DataManager.Instance.playerNo == 1)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                noSend = 3;
            }
            else
            {
                noSend = 2;
            }
            
            if (DataManager.Instance.isFourPlayer)
            {
                noSend = playerRoundChecker;
            }
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            noSend = playerRoundChecker;//3 was default value
        }
        else if (DataManager.Instance.playerNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
            {
                noSend = 1;
            }
            else
            {
                noSend = 4;
            }
            if (DataManager.Instance.isFourPlayer)
            {
                noSend = playerRoundChecker;
            }
        }
        else if (DataManager.Instance.playerNo == 4)
        {
            noSend = playerRoundChecker;//1 was default value
        }
        print("changing turn when playerRoundChecker = " + playerRoundChecker);
        

        obj.AddField("PlayerNo", noSend);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);


        TestSocketIO.Instace.Senddata("LudoDiceChangeData", obj);



    }
    
    public void BotDiceChange(string playerId, int playerNo, bool calledByPlayer)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", playerId);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        int noSend = 0;
        noSend = DataManager.Instance.playerNo;

        if (playerNo == 1)
        {
            if (DataManager.Instance.isTwoPlayer)
                noSend = 3;
            else
                noSend = 2;

            if (DataManager.Instance.isFourPlayer)
                noSend = playerRoundChecker;
        }
        else if (playerNo == 2)
            noSend = playerRoundChecker;//from 3 

        else if (playerNo == 3)
        {
            if (DataManager.Instance.isTwoPlayer)
                noSend = 1;
            else
                noSend = 4;
            if (DataManager.Instance.isFourPlayer)
                noSend = playerRoundChecker;
        }
        else if (playerNo == 4)
            noSend = playerRoundChecker;

        obj.AddField("PlayerNo", noSend);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerRoundChecker", playerRoundChecker);
        Debug.Log("roundChecker = " + playerRoundChecker);
        print("turn change by bot ");
        if (calledByPlayer == true)
            TestSocketIO.Instace.Senddata("LudoDiceChangeData", obj);
        else
            TestSocketIO.Instace.Senddata("LudoDiceChangeDataBot", obj);

    }

    
    #endregion

    #region Receive

    public void CheckThePasaBool()
    {

        if (DataManager.Instance.isTwoPlayer)
        {
            if (BotManager.Instance.isConnectBot)
            {
                print("Enter The Direct Condition");
                BotChangeTurn(false, true);
            }
            else
            {
                PlayerChangeTurn();
            }
            RestartTimer();
            
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            if (isPlayerNextTurn() == false && isAdmin)
            {
                print("Enter The Direct Condition");
                BotChangeTurn(false, true);
            }
            else if (isPlayerNextTurn() == false && isAdmin == false)
            {

                //playerRoundChecker = playerRoundChecker switch
                //{
                //    1 => 2,
                //    2 => 3,
                //    3 => 4,
                //    4 => 1,
                //    _ => playerRoundChecker
                //};
                ChangeTurn();
                DataManager.Instance.isDiceClick = false;
                isClickAvaliableDice = 1;
                DataManager.Instance.isTimeAuto = false;
                BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, true);
            }
            else
                PlayerChangeTurn();
            RestartTimer();
        }


        //RestartTimer();//Greejesh
        //DataManager.Instance.isChange = true;


        // print("Change Data Pass");
    }
    
    private void AddUserData(JSONObject users, Image[] token, JSONObject[] tokenNo)
    {
        for (int i = 0; i < token.Length; i++)
        {
            var pasa = token[i].GetComponent<PasaManage>();
            tokenNo[i] = new JSONObject();
            users.AddField("updatedPlayerNo", pasa.updatedPlayerNo);
            
            tokenNo[i].AddField("playerSubNo", pasa.playerSubNo);
            tokenNo[i].AddField("pasaCurrentNo", pasa.pasaCurrentNo);
            if (pasa.isSafe)
                tokenNo[i].AddField("isSafe", "1");
            else
                tokenNo[i].AddField("isSafe", "0");

            if (pasa.isStarted)
                tokenNo[i].AddField("isStarted", "1");
            else
                tokenNo[i].AddField("isStarted", "0");
            users.AddField("tokens", new JSONObject(tokenNo));
        }
    }


    public void PauseUserDataSend()
    {
        /*JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("GreenSliderValue", timerFillImg1.fillAmount);
        obj.AddField("OurDot", cntPlayer2);
        bool isTurn = false;
        if (DataManager.Instance.isDiceClick)
        {
            isTurn = false;
        }
        else
        {
            isTurn = true;
        }

        obj.AddField("Turn", isTurn);

        TestSocketIO.Instace.Senddata("GetUserPauseData", obj);*/
        JSONObject obj = new JSONObject();

        JSONObject[] users = new JSONObject[4];
        JSONObject[] token1 = new JSONObject[4];
        JSONObject[] token2 = new JSONObject[4];
        JSONObject[] token3 = new JSONObject[4];
        JSONObject[] token4 = new JSONObject[4];
        
        for (int i = 0; i < users.Length; i++)
            users[i] = new JSONObject();

        AddUserData(users[0], box1Token, token1);
        users[0].AddField("lifeLost", cntPlayer1);
        users[0].AddField("score", playerScoreCnt1);
        
        if (box2Img.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            AddUserData(users[1], box2Token, token2);
            users[1].AddField("lifeLost", cntPlayer2);
            users[1].AddField("score", playerScoreCnt2);
        }
        if (box3Img.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            AddUserData(users[2], box3Token, token3);
            users[2].AddField("lifeLost", cntPlayer3);
            users[2].AddField("score", playerScoreCnt3);
        }
        if (box4Img.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            AddUserData(users[3], box4Token, token4);
            users[3].AddField("lifeLost", cntPlayer4);
            users[3].AddField("score", playerScoreCnt4);
        }

        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("GreenSliderValue", timerFillImg1.fillAmount);
        obj.AddField("OurDot", cntPlayer2);
        obj.AddField("playerRoundChecker", playerRoundChecker);
        obj.AddField("GameTime", secondsCount);
        bool isTurn = false;
        if (DataManager.Instance.isDiceClick)
        {
            isTurn = false;
        }
        else
        {
            isTurn = true;
        }

        obj.AddField("Turn", isTurn);
        obj.AddField("users", new JSONObject(users));
        TestSocketIO.Instace.Senddata("GetUserPauseData", obj);
    }



    public void PauseDataRetriveSocket(float fill, int number, bool isTurn, float gameTime)
    {
        isPauseGetData = true;
        secondsCount = gameTime;
        if (DataManager.Instance.isTwoPlayer)
        {
            timerFillImg1.fillAmount = fill;
            timerFillImg3.fillAmount = fill;
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            timerFillImg1.fillAmount = fill;
            timerFillImg2.fillAmount = fill;
            timerFillImg3.fillAmount = fill;
            timerFillImg4.fillAmount = fill;
        }
        if (isTurn)
        {
            DataManager.Instance.isDiceClick = true;
            isClickAvaliableDice = 0;
            OurShadowMaintain();
            DataManager.Instance.isTimeAuto = false;
            isTimeEnter = false;
            DataManager.Instance.isRestartManage = true;
        }
        else
        {
            DataManager.Instance.isDiceClick = false;
            isClickAvaliableDice = 1;
            //OtherShadowMainTain();
            if (DataManager.Instance.isTwoPlayer)
                OtherShadowMainTain();
            else
                OurShadowMaintain();
            DataManager.Instance.isTimeAuto = false;
            isTimeEnter = false;
        }
        cntPlayer1 = number;
        for (int i = 0; i < box1Lifes.Length; i++)
        {
            if (i < cntPlayer1)
            {
                box1Lifes[i].color = lifeOffColor;
            }
        }

    }


    //public in

    public void AutoMove(int playerNo, int tokenNo, int move)
    {
        //print("Enter The Auto Move");
        //print("Player No : " + playerNo);
        //print("Token No : " + tokenNo);
        //print("Token move : " + move);
        for (int i = 0; i < pasaSocketList.Count; i++)
        {
            if (pasaSocketList[i].updatedPlayerNo == playerNo && pasaSocketList[i].playerSubNo == tokenNo)
            {
                if (pasaSocketList[i].orgParentNo == 1)
                {
                    //print("Enter Auto Move 1");
                    pasaSocketList[i].MoveStart(1, move);
                }
                else if (pasaSocketList[i].orgParentNo == 2)
                {
                    //pasaSocketList[i].MoveStart(numberObj2, move);
                    pasaSocketList[i].MoveStart(2, move);
                }
                else if (pasaSocketList[i].orgParentNo == 3)
                {
                    //                    print("Enter Auto Move 3");
                    pasaSocketList[i].MoveStart(3, move);
                    //pasaSocketList[i].MoveStart(numberObj3, move);
                }
                else if (pasaSocketList[i].orgParentNo == 4)
                {
                    //  pasaSocketList[i].MoveStart(numberObj4,move);
                    pasaSocketList[i].MoveStart(4, move);
                }
            }

        }
    }

    public void AutoFourPlayerBackFirst()//changes remaining in this function
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            for (int i = 0; i < pasaSocketList.Count; i++)
            {
                if (pasaSocketList[i].playerNo != DataManager.Instance.playerNo)// && pasaSocketList[i].playerSubNo == tokenNo)
                {
                    // i  //                    print("Enter Auto Move 3");
                    pasaSocketList[i].MoveStart(3, 1);
                    //pasaSocketList[i].MoveStart(numberObj3, move);

                }
            }
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            for (int i = 0; i < pasaSocketList.Count; i++)
            {
                if (pasaSocketList[i].playerNo != DataManager.Instance.playerNo)// && pasaSocketList[i].playerSubNo == tokenNo)
                {
                    // i  //                    print("Enter Auto Move 3");
                    pasaSocketList[i].MoveStart(pasaSocketList[i].orgParentNo, 1);
                    //pasaSocketList[i].MoveStart(numberObj3, move);

                }
            }
        }


        //for (int i = 0; i < pasaSocketList.Count; i++)
        //{
        //    if(pasaSocketList[i].orgParentNo == DataManager.Instance.playerNo)
        //    {
        //        pasaSocketList[i].MoveStart()
        //    }
        //}
    }


    //    public void StopDiceLine()
    //    {
    ////        print("Enter Fill Amount");
    //        if (DataManager.Instance.isRestartManage == true)
    //        {
    //            DataManager.Instance.isRestartManage = false;
    //        }
    //        else
    //        {
    //            timerFillImg.fillAmount = 0;
    //        }

    //    }
    public void AutoDice(int no, int pNo, int pNo1)
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            if (DataManager.Instance.modeType == 3)
                GenerateDiceNumber_SocketDiceLess(no, pNo, pNo1);
            else
                StartCoroutine(GenerateDiceNumber_Socket(no, pNo, pNo1));
            if(DataManager.Instance.modeType != 3)
                SoundManager.Instance.RollDice_Start_Sound();
                pasaImage1.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage3.gameObject.GetComponent<Animator>().enabled = true;
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            playerRoundChecker = pNo1;
            if (DataManager.Instance.modeType == 3)
                GenerateDiceNumber_SocketDiceLess(no, pNo, pNo1);
            else
                StartCoroutine(GenerateDiceNumber_Socket(no, pNo, pNo1));
            
            if(DataManager.Instance.modeType != 3)
                SoundManager.Instance.RollDice_Start_Sound();
                pasaImage1.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage2.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage3.gameObject.GetComponent<Animator>().enabled = true;
                pasaImage4.gameObject.GetComponent<Animator>().enabled = true;


        }

    }
    IEnumerator GenerateDiceNumber_Socket(int no, int pNo, int pNo1)
    {
        yield return new WaitForSeconds(0.5f);
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = false;
            pasa1.localScale = Vector3.one;
            pasa3.localScale = Vector3.one;
            pasaImage1.sprite = pasaSprite[no - 1];
            pasaImage3.sprite = pasaSprite[no - 1];
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage2.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage4.gameObject.GetComponent<Animator>().enabled = false;
            pasa1.localScale = Vector3.one;
            pasa2.localScale = Vector3.one;
            pasa3.localScale = Vector3.one;
            pasa4.localScale = Vector3.one;
            pasaImage1.sprite = pasaSprite[no - 1];
            pasaImage2.sprite = pasaSprite[no - 1];
            pasaImage3.sprite = pasaSprite[no - 1];
            pasaImage4.sprite = pasaSprite[no - 1];


        }
        //print("Player No : " + pNo);
        //print("Player No 1 : " + pNo1);

        SoundManager.Instance.RollDice_Stop_Sound();

        if (pNo == DataManager.Instance.playerNo)
        {
            //            PasaImageManage(no, 3, true);
        }
        else if (pNo1 == DataManager.Instance.playerNo)
        {
            //          PasaImageManage(no, 3, true);
        }
    }

    void GenerateDiceNumber_SocketDiceLess(int no, int pNo, int pNo1)
    {
        //pasaImage.gameObject.GetComponent<Animator>().enabled = false;
        //print("Player No : " + pNo);
        //print("Player No 1 : " + pNo1);

        if (pNo == DataManager.Instance.playerNo)
        {
            //            PasaImageManage(no, 3, true);
        }
        else if (pNo1 == DataManager.Instance.playerNo)
        {
            //          PasaImageManage(no, 3, true);
        }
        SoundManager.Instance.RollDice_Stop_Sound();
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaImage1.color = colorOff;
            pasaImage3.color = colorOff;
            pasaNoTxt.color = colorOn;
            pasaNoTxt3.color = colorOn;
            pasaNoTxt.text = no.ToString();
            pasaNoTxt3.text = no.ToString();
            print("-------------------SocketDiceless 2 Player-------------------");
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaImage1.color = colorOff;
            pasaImage2.color = colorOff;
            pasaImage3.color = colorOff;
            pasaImage4.color = colorOff;
            pasaNoTxt.color = colorOn;
            pasaNoTxt2.color = colorOn;
            pasaNoTxt3.color = colorOn;
            pasaNoTxt4.color = colorOn;
            pasaNoTxt.text = no.ToString();
            pasaNoTxt2.text = no.ToString();
            pasaNoTxt3.text = no.ToString();
            pasaNoTxt4.text = no.ToString();
            print("-------------------SocketDiceless 4 Player-------------------");


        }
    }



    #endregion

    #endregion


    #region  Application Pause
    
    private void OnEnable()
    {
        Application.runInBackground = true; // Allow the game to run in the background
        Application.focusChanged += OnApplicationFocusChanged;
    }

    //private void OnDisable()
    //{
    //    Application.focusChanged -= OnApplicationFocusChanged;
    //}

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isApplicationPaused = pause;
            // Application paused
            DateTime date = DateTime.Now;
            PauseDataSendRequest();
            PlayerPrefs.SetString("PlayTimeDate", date.ToString());
            if (DataManager.Instance.isTwoPlayer && BotManager.Instance.isConnectBot)
                Time.timeScale = 0f;
        }
        else
        {
            isApplicationPaused = pause;
            if (DataManager.Instance.isTwoPlayer && BotManager.Instance.isConnectBot)
            {
                Time.timeScale = 1f;
                return;
            }
            print("Game Resumed");
            // Application resumed
            //PauseDataGetRequest();
            ResumeDataSendRequest();
            //Invoke(nameof(WaitTimeToCheck), 3f);
            Invoke(nameof(PauseDataGetRequest), 1.5f);
            int realPlayerCounter = 0;
            foreach (var item in DataManager.Instance.joinPlayerDatas)
            {
                if (!item.userId.Contains("Ludo"))
                    realPlayerCounter++;
            }
            print("Real player count = " + realPlayerCounter);
            if (realPlayerCounter > 1)
                Invoke(nameof(WaitTimeToCheck), 3f);
            else
                GetDiffPause();//gettime to diff

        }
    }

    private void OnApplicationFocusChanged(bool hasFocus)
    {
        //print("Pause : " + hasFocus);
        //isApplicationPaused = !hasFocus;

        //if (isApplicationPaused)
        //{
        //    // Application paused
        //    DateTime date = DateTime.Now;
        //    PauseDataSendRequest();
        //    PlayerPrefs.SetString("PlayTimeDate", date.ToString());
        //}
        //else
        //{
        //    // Application resumed
        //    //PauseDataGetRequest();
        //    ResumeDataSendRequest();
        //    //Invoke(nameof(WaitTimeToCheck), 3f);
        //    Invoke(nameof(PauseDataGetRequest), 1.5f);
        //    Invoke(nameof(WaitTimeToCheck), 3f);
        //    //gettime to diff
        //    GetDiffPause();
        //}
    }
    /*private void OnApplicationPause(bool pause)
    {
        print("Pause : " + pause);
        if (pause)
        {
            //Check
            DateTime date = DateTime.Now;

            PlayerPrefs.SetString("PlayTimeDate", date.ToString());
        }
        else
        {

            PauseDataGetRequest();
            Invoke(nameof(WaitTimeToCheck), 3f);
            //gettime to diff
            GetDiffPause();
        }
    }*/


    void WaitTimeToCheck()
    {
        print("Pause Player success");
        if (isPauseGetData == false)
        {
            DataManager.Instance.tournamentID = "";
            DataManager.Instance.tourEntryMoney = 0;
            DataManager.Instance.tourCommision = 0;
            DataManager.Instance.commisionAmount = 0;
            DataManager.Instance.orgIndexPlayer = 0;
            DataManager.Instance.joinPlayerDatas.Clear();
            TestSocketIO.Instace.roomid = "";
            TestSocketIO.Instace.userdata = "";
            TestSocketIO.Instace.playTime = 0;
            SoundManager.Instance.StartBackgroundMusic();
            DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
            SceneManager.LoadScene("Main");
        }
        else if (isPauseGetData == true)
        {
            isPauseGetData = false;
            //GetDiffPause();
        }
    }


    void GetDiffPause()
    {
        print("Pause Difference called");
        string getDate = PlayerPrefs.GetString("PlayTimeDate", "none");
        if (getDate == "none")
        {
            return;
        }

        int createHour = int.Parse(getDate.Split(" ")[1].Split(":")[0]);
        //int currHour = ;
        int createMinute = int.Parse(getDate.Split(" ")[1].Split(":")[1]);
        int createSecond = int.Parse(getDate.Split(" ")[1].Split(":")[2]);

        DateTime date = DateTime.Now;
        string curDate = date.ToString();
        int currHour = int.Parse(curDate.Split(" ")[1].Split(":")[0]);

        int currMinute = int.Parse(curDate.Split(" ")[1].Split(":")[1]);
        int currSecond = int.Parse(curDate.Split(" ")[1].Split(":")[2]);


        DateTime dateTime1 = DateTime.Parse(createHour + ":" + createMinute + ":" + createSecond);
        DateTime dateTime2 = DateTime.Parse(currHour + ":" + currMinute + ":" + currSecond);

        var diff = (dateTime2 - dateTime1).TotalSeconds;

        string changeString = diff.ToString();

        long diffInSeconds = long.Parse(changeString);

        if (diffInSeconds >= 60)
        {
            DataManager.Instance.tournamentID = "";
            DataManager.Instance.tourEntryMoney = 0;
            DataManager.Instance.tourCommision = 0;
            DataManager.Instance.commisionAmount = 0;
            DataManager.Instance.orgIndexPlayer = 0;
            DataManager.Instance.joinPlayerDatas.Clear();
            TestSocketIO.Instace.roomid = "";
            TestSocketIO.Instace.userdata = "";
            TestSocketIO.Instace.playTime = 0;
            DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
            SceneManager.LoadScene("Main");
        }
        else
        {
            /*if (IsBotPlaying())
            {
                print("Bot is playing");
            }
            else
            {
                print("Game is played with real players and have taken current time");
            }*/
            print("Time difference mitigated");
            secondsCount -= diffInSeconds;
        }

    }
    
    private bool IsBotPlaying()
    {
        foreach (var playerData in DataManager.Instance.joinPlayerDatas)
        {
            if (playerData.userId.EndsWith("Ludo"))
            {
                print("Bot is present in the game");
                return true;
            }
        }

        return false;
    }



    #endregion



    #region Bot Manager

    bool isOneBot = false;
    bool isTwoBot = false;
    bool isOneTimeGive = false;
    int easyCount = 0;
    bool easyBoolSet = false;

    public void GenerateDiceNumberStart_Bot(bool isStart)
    {
        if (BotManager.Instance.isConnectBot)
        {
            print("Bot Dice called");
            if (isStart)
            {
                //BotMoveTokeStore botMoveGet = Check_Kill_Bot();
                //PasaButtonClick_Bot(botMoveGet);

                CheckPasaThePlayer_Bot();
                BotMoveTokeStore botMoveGet = new BotMoveTokeStore();
                //if (DataManager.Instance.isAvaliable)
                //{
                //    pasaCurrentNo = DataManager.Instance.currentPNo;
                //}
                //else
                //{
                //pasaCurrentNo = UnityEngine.Random.Range(1, 7);
                if (clickCounter < 3 && UnityEngine.Random.Range(0, 10) < 8 && !isSixObtained) // 90% chance for a 6 within the first three clicks
                {
                    botMoveGet.moveNo = 6;
                    isSixObtained = true;
                }
                else
                {
                    botMoveGet.moveNo = UnityEngine.Random.Range(1, 7);
                    clickCounter = 0;
                }
                clickCounter++;

                //}
                //botMoveGet.moveNo = UnityEngine.Random.Range(1, 5);

                //botMoveGet.moveNo = 6;

                PasaButtonClick_Bot(botMoveGet);
            }
            else
            {
                switch (BotManager.Instance.botType)
                {
                    case BotType.Easy:
                        {
                            /*CheckPasaThePlayer_Bot();
                        BotMoveTokeStore botMoveGet = new BotMoveTokeStore();
                        botMoveGet.moveNo = UnityEngine.Random.Range(1, 7);
                        if (easyBoolSet == false)
                        {
                            if (botMoveGet.moveNo == 6)
                            {
                                easyBoolSet = true;
                            }
                            else
                            {
                                easyCount++;
                                if (easyCount == 9)
                                {
                                    botMoveGet.moveNo = 6;
                                    easyBoolSet = true;
                                }
                            }
                        }
                        //botMoveGet.moveNo = 6;

                        PasaButtonClick_Bot(botMoveGet);*/
                            BotMoveTokeStore botMoveGet = GetBotMoveForEasy();
                            PasaButtonClick_Bot(botMoveGet);
                            break;
                        }
                    case BotType.Medium:
                        {
                            /*int rno = UnityEngine.Random.Range(0, 5);
                        if (rno == 0 || rno == 1 || rno == 2)
                        {
                            CheckPasaThePlayer_Bot();
                            BotMoveTokeStore botMoveGet = new BotMoveTokeStore();
                            botMoveGet.moveNo = UnityEngine.Random.Range(1, 7);
                            if (easyBoolSet == false)
                            {
                                if (botMoveGet.moveNo == 6)
                                {
                                    easyBoolSet = true;
                                }
                                else
                                {
                                    easyCount++;
                                    if (easyCount == 6)
                                    {
                                        botMoveGet.moveNo = 6;
                                        easyBoolSet = true;
                                    }
                                }
                            }


                            PasaButtonClick_Bot(botMoveGet);
                        }
                        else
                        {
                            BotMoveTokeStore botMoveGet = Check_Kill_Bot();
                            PasaButtonClick_Bot(botMoveGet);
                        }*/
                            BotMoveTokeStore botMoveGet = GetBotMoveForMedium();
                            PasaButtonClick_Bot(botMoveGet);
                            break;
                        }
                    case BotType.Hard:
                        {
                            BotMoveTokeStore botMoveGet = Check_Kill_Bot();
                            PasaButtonClick_Bot(botMoveGet);
                            break;
                        }
                }
            }
        }
    }

    public void PasaButtonClick_Bot(BotMoveTokeStore botMove)
    {
        if (DataManager.Instance.modeType != 3)
            SoundManager.Instance.RollDice_Start_Sound();
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = true;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = true;
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = true;
            pasaImage2.gameObject.GetComponent<Animator>().enabled = true;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = true;
            pasaImage4.gameObject.GetComponent<Animator>().enabled = true;
        }
            // print("Pasa Bot Con 1");
            //isPathClick = false;
            isClickAvaliableDice = 1;
        pasaCurrentNo = botMove.moveNo;
        BotDice(botMove.moveNo, DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, playerRoundChecker, 0);
        StartCoroutine(GenerateDiceNumber_Bot(botMove));
    }


    IEnumerator GenerateDiceNumber_Bot(BotMoveTokeStore botMove)
    {
        yield return new WaitForSeconds(0.5f);
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = false;
            pasa1.localScale = Vector3.one;
            pasa3.localScale = Vector3.one;

            pasaImage1.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage3.sprite = pasaSprite[pasaCurrentNo - 1];

            print("----------------DicelessBot 2 player ---------------------");

            pasaNoTxt.text = pasaCurrentNo.ToString();
            pasaNoTxt3.text = pasaCurrentNo.ToString();
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaImage1.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage2.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage3.gameObject.GetComponent<Animator>().enabled = false;
            pasaImage4.gameObject.GetComponent<Animator>().enabled = false;
            pasa1.localScale = Vector3.one;
            pasa2.localScale = Vector3.one;
            pasa3.localScale = Vector3.one;
            pasa4.localScale = Vector3.one;

            pasaImage1.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage2.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage3.sprite = pasaSprite[pasaCurrentNo - 1];
            pasaImage4.sprite = pasaSprite[pasaCurrentNo - 1];

            print("----------------DicelessBot 4 player ---------------------");

            pasaNoTxt.text = pasaCurrentNo.ToString();
            pasaNoTxt2.text = pasaCurrentNo.ToString();
            pasaNoTxt3.text = pasaCurrentNo.ToString();
            pasaNoTxt4.text = pasaCurrentNo.ToString();
        }

        SoundManager.Instance.RollDice_Stop_Sound();
        yield return new WaitForSeconds(0.75f);

        if (botMove.pasaToken != null)
        {
            //if (botMove.isMoveSend)
            //{
            //    if (pasaCurrentNo == 6)
            //    {
            //        List<PasaManage> notAmove = new List<PasaManage>();
            //        for (int i = 0; i < pasaCollectList.Count; i++)
            //        {
            //            if (pasaCollectList[i].pasaCurrentNo == 0)
            //            {
            //                notAmove.Add(pasaCollectList[i]);
            //            }
            //        }
            //        if (notAmove.Count > 0)
            //        {
            //            notAmove[0].MoveStart_Bot(3, pasaCurrentNo);
            //        }
            //        else
            //        {
            //            botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
            //        }

            //    }
            //    else
            //    {
            //        botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
            //    }
            //}
            //else
            //{
            //    botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
            //}
            botMove.pasaToken.counter = 0;//remove this line if causing issues
            print("Sending step count = " + pasaCurrentNo);
            botMove.pasaToken.MoveStart_Bot(DataManager.Instance.isTwoPlayer ? 3 : botMove.pasaToken.orgParentNo, pasaCurrentNo);
            print("-------- BotPlayerMovementName--------------- " + botMove.pasaToken.name);
        }

        else if (botMove.pasaToken == null)
        {
            if (pasaCurrentNo == 6)
            {
                List<PasaManage> notAmove = new List<PasaManage>();
                for (int i = 0; i < pasaCollectList.Count; i++)
                {
                    if (pasaCollectList[i].pasaCurrentNo == 0)
                    {
                        notAmove.Add(pasaCollectList[i]);
                    }
                }
                if (notAmove.Count > 0)
                {
                    notAmove[0].counter = 0;
                    print("Sending step count = " + pasaCurrentNo);
                    notAmove[0].MoveStart_Bot(DataManager.Instance.isTwoPlayer ? 3 : notAmove[0].orgParentNo, pasaCurrentNo);
                }
            }
            else
            {
                //Invoke(nameof(WaitAfterTurnChangeNotOut), 0f);

                if (DataManager.Instance.isTwoPlayer)
                {
                    print("!!!!!!Enter The Else COndition First Six!!!!!!");
                    //GenerateDiceNumberStart_Bot(false);
                    Invoke(nameof(WaitAfterTurnChangeNotOut), 0f);
                }
                else if(DataManager.Instance.isFourPlayer && playerRoundChecker == 4)
                {
                    print("!!!!!!!!!!!!!!!!!!! Condision passed!!!!!!!!!!!!!!!!!!!!!!");
                    Invoke(nameof(WaitAfterTurnChangeNotOut), 0f);//next turn is player
                }
                else
                {
                    BotChangeTurn(false, true);
                }
            }
        }
        //else
        //{
        //    if (botMove.isMoveSend)
        //    {
        //        if (pasaCurrentNo == 6)
        //        {
        //            List<PasaManage> notAmove = new List<PasaManage>();
        //            for (int i = 0; i < pasaCollectList.Count; i++)
        //            {
        //                if (pasaCollectList[i].pasaCurrentNo == 0)
        //                {
        //                    notAmove.Add(pasaCollectList[i]);
        //                }
        //            }
        //            if (notAmove.Count > 0)
        //            {
        //                notAmove[0].MoveStart_Bot(3, pasaCurrentNo);
        //            }
        //            else
        //            {
        //                botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
        //            }

        //        }
        //        else
        //        {
        //            botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
        //        }
        //    }
        //    else
        //    {
        //        botMove.pasaToken.MoveStart_Bot(3, pasaCurrentNo);
        //    }
        //}
    }

    void WaitAfterTurnChangeNotOut()
    {
        BotChangeTurn(true, false);
    }


    void CheckPasaThePlayer_Bot()
    {
        if (DataManager.Instance.isTwoPlayer)
        {
            pasaCollectList.Clear();
            int botPlayerNo = 1;
            if (DataManager.Instance.playerNo == 1)
            {
                botPlayerNo = 3;
            }
            for (int i = 0; i < pasaBotPlayer.Count; i++)
            {
                PasaManage pasa = pasaBotPlayer[i].GetComponent<PasaManage>();
                if (pasa.playerNo == botPlayerNo && pasa.isPlayer == false)
                {
                    pasaCollectList.Add(pasa);
                }
            }
        }
        else if (DataManager.Instance.isFourPlayer)
        {
            pasaCollectList.Clear();
            switch (playerRoundChecker)
            //switch (DataManager.Instance.playerNo)
            {
                case 2:
                    {
                        
                        botPlayerNo = playerRoundChecker;
                        //if (isPlayer2Left && isPlayer3Left)
                        //    botPlayerNo = 4;
                        //else if(isPlayer2Left)
                        //    botPlayerNo = 3;
                        if(hasEmptyPlayerSpace || isAdminPause)
                        {
                            foreach (var item in pasaBotPlayer)
                            {
                                if (playerRoundChecker == item.updatedPlayerNo)
                                {
                                    botPlayerNo = item.orgParentNo;
                                    break;
                                }
                            }
                        }
                        AddingPasaCollectionList(botPlayerNo);
                        break;
                    }
                case 3:
                    {
                        botPlayerNo = playerRoundChecker;
                        //if (isPlayer3Left)
                        //    botPlayerNo = 4;
                        if (hasEmptyPlayerSpace || isAdminPause)
                        {
                            foreach (var item in pasaBotPlayer)
                            {
                                if (playerRoundChecker == item.updatedPlayerNo)
                                {
                                    botPlayerNo = item.orgParentNo;
                                    break;
                                }
                            }
                        }
                        AddingPasaCollectionList(botPlayerNo);
                        break;
                    }
                case 4:
                    {
                        botPlayerNo = playerRoundChecker;
                        if (hasEmptyPlayerSpace || isAdminPause)
                        {
                            foreach (var item in pasaBotPlayer)
                            {
                                if (playerRoundChecker == item.updatedPlayerNo)
                                {
                                    botPlayerNo = item.orgParentNo;
                                    break;
                                }
                            }
                        }
                        AddingPasaCollectionList(botPlayerNo);
                        isEntered = false;
                        break;
                    }
            }

        }
    }
    
    public void AddingPasaCollectionList(int botPlayerNo)
    {
        for (int i = 0; i < pasaBotPlayer.Count; i++)
        {
            PasaManage pasa = pasaBotPlayer[i].GetComponent<PasaManage>();
            if (pasa.orgParentNo == botPlayerNo && pasa.isPlayer == false)
            {
                pasaCollectList.Add(pasa);
            }
        }
    }

    BotMoveTokeStore GetBotMoveForEasy()
    {
        BotMoveTokeStore botMoveTokeStore = new BotMoveTokeStore();
        CheckPasaThePlayer_Bot();

        List<PasaManage> moveBotPlayer = MoveablePlayer();
        List<PasaManage> homeBotPlayer = HomePlayerBot();
        List<PasaManage> safeBotPlayer = SafePlayerBot();
        
        if (moveBotPlayer.Count > 0)
        {
            if (homeBotPlayer.Count > 0)
            {
                PasaManage pSafeManage = homeBotPlayer[0];
                int maxRange = 57 - pSafeManage.orgNo;
                bool isFindEnter = false;
                int moveNo = UnityEngine.Random.Range(1, maxRange + 1);  // Generate a random move number between 1 and 6
                int checkNo = pSafeManage.orgNo + moveNo;      // Calculate the new position by adding the move number
                if (checkNo <= 57)                             // Check if the new position is within the home stretch
                {
                    isFindEnter = true;
                    botMoveTokeStore.moveNo = moveNo;
                    botMoveTokeStore.pasaToken = pSafeManage;
                    botMoveTokeStore.isHomeSend = true;
                }
            }
            else
            {
                if (safeBotPlayer.Count > 0)
                {
                    PasaManage pSafeManage = safeBotPlayer[0];
                    botMoveTokeStore.moveNo = Bot_Random_Genrate();
                    botMoveTokeStore.pasaToken = pSafeManage;
                    botMoveTokeStore.isMoveSend = true;
                }
                else
                {
                    botMoveTokeStore.moveNo = Bot_Random_Genrate();
                    int rno = UnityEngine.Random.Range(0, moveBotPlayer.Count);
                    botMoveTokeStore.pasaToken = moveBotPlayer[rno];
                    botMoveTokeStore.isMoveSend = true;
                }
            }
        }
        else
        {
            botMoveTokeStore.moveNo = Bot_Random_Genrate();
            botMoveTokeStore.pasaToken = null;
        }

        return botMoveTokeStore;
    }
    
    BotMoveTokeStore GetBotMoveForMedium()
    {
        BotMoveTokeStore botMoveTokeStore = new BotMoveTokeStore();
        CheckPasaThePlayer_Bot();

        List<PasaManage> moveBotPlayer = MoveablePlayer();
        List<PasaManage> homeBotPlayer = HomePlayerBot();
        List<PasaManage> safeBotPlayer = SafePlayerBot();

        if (moveBotPlayer.Count > 0)
        {
            if (homeBotPlayer.Count > 0)
            {
                PasaManage pSafeManage = homeBotPlayer[0];
                bool isFindEnter = false;
                for (int i = 1; i < 7; i++)
                {
                    int checkNo = pSafeManage.orgNo + i;
                    if (checkNo == 57 && isFindEnter == false)
                    {
                        isFindEnter = true;
                        botMoveTokeStore.moveNo = i;
                        botMoveTokeStore.pasaToken = pSafeManage;
                        botMoveTokeStore.isHomeSend = true;
                    }
                }
            }
            else
            {
                if (safeBotPlayer.Count > 0)
                {
                    PasaManage pSafeManage = safeBotPlayer[0];
                    botMoveTokeStore.moveNo = Bot_Random_Genrate();
                    botMoveTokeStore.pasaToken = pSafeManage;
                    botMoveTokeStore.isMoveSend = true;
                }
                else
                {
                    botMoveTokeStore.moveNo = Bot_Random_Genrate();
                    int rno = UnityEngine.Random.Range(0, moveBotPlayer.Count);
                    botMoveTokeStore.pasaToken = moveBotPlayer[rno];
                    botMoveTokeStore.isMoveSend = true;
                }
            }
        }
        else
        {
            botMoveTokeStore.moveNo = Bot_Random_Genrate();
            botMoveTokeStore.pasaToken = null;
        }

        return botMoveTokeStore;
    }

    BotMoveTokeStore Check_Kill_Bot()
    {
        BotMoveTokeStore botMoveTokeStore = new BotMoveTokeStore();
        CheckPasaThePlayer_Bot();

        List<PasaManage> moveBotPlayer = MoveablePlayer();
        List<PasaManage> killBotPlayer = KillPlayerBot();
        List<PasaManage> homeBotPlayer = HomePlayerBot();
        List<PasaManage> safeBotPlayer = SafePlayerBot();



        if (moveBotPlayer.Count > 0)
        {
            if (killBotPlayer.Count > 0)
            {
                PasaManage pSafeManage1 = killBotPlayer[0];
                PasaManage pSafeManage2 = killBotPlayer[1];
                bool isFindEnter = false;
                for (int i = 1; i < 7; i++)
                {
                    int checkNo = pSafeManage1.orgNo + i;
                    if (checkNo == pSafeManage2.orgNo && isFindEnter == false)
                    {
                        isFindEnter = true;
                        botMoveTokeStore.moveNo = i;
                        botMoveTokeStore.pasaToken = pSafeManage1;
                        botMoveTokeStore.isKillSend = true;
                    }
                }
            }
            else
            {
                if (homeBotPlayer.Count > 0)
                {
                    PasaManage pSafeManage = homeBotPlayer[0];
                    bool isFindEnter = false;
                    for (int i = 1; i < 7; i++)
                    {
                        int checkNo = pSafeManage.orgNo + i;
                        if (checkNo == 57 && isFindEnter == false)
                        {
                            isFindEnter = true;
                            botMoveTokeStore.moveNo = i;
                            botMoveTokeStore.pasaToken = pSafeManage;
                            botMoveTokeStore.isHomeSend = true;
                        }
                    }
                }
                else
                {
                    if (safeBotPlayer.Count > 0)
                    {

                        List<PasaManage> movePlayerOrgAv = new List<PasaManage>();
                        for (int i = 0; i < pasaObjects.Count; i++)
                        {
                            PasaManage pManageOrgDv = pasaObjects[i].GetComponent<PasaManage>();
                            if (currentPlayerPasaList.Contains(pManageOrgDv))
                            {
                                movePlayerOrgAv.Add(pManageOrgDv);
                            }
                        }

                        List<bool> checkTheMove = new List<bool>();

                        if (movePlayerOrgAv.Count > 0)
                        {
                            for (int i = 0; i < safeBotPlayer.Count; i++)
                            {
                                PasaManage pSafeManage = safeBotPlayer[i];
                                bool isUnsafe = false;
                                for (int j = 0; j < movePlayerOrgAv.Count; j++)
                                {
                                    PasaManage pMoveOrgManage = movePlayerOrgAv[i];

                                    for (int k = 1; k < 7; k++)
                                    {
                                        if (pSafeManage.orgNo == (pMoveOrgManage.orgNo + k) && isUnsafe == false)
                                        {
                                            isUnsafe = true;
                                        }

                                    }

                                    if (isUnsafe)
                                    {
                                        break;
                                    }
                                }
                                checkTheMove.Add(isUnsafe);
                            }

                            if (checkTheMove.Contains(true))
                            {
                                bool isFindEnter = false;
                                for (int i1 = 0; i1 < checkTheMove.Count; i1++)
                                {
                                    bool isGetCheck = checkTheMove[i1];
                                    if (isGetCheck)
                                    {
                                        PasaManage pSafeManage = safeBotPlayer[i1];
                                        for (int i = 1; i < 7; i++)
                                        {
                                            int checkNo = pSafeManage.orgNo + i;
                                            if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isFindEnter == false)
                                            {
                                                isFindEnter = true;
                                                botMoveTokeStore.moveNo = i;
                                                botMoveTokeStore.pasaToken = pSafeManage;
                                                botMoveTokeStore.isSafeSend = true;
                                                //generatePasaNo = i;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {

                                bool isKillAvliable = false;
                                //int orgFirstNo = 1;

                                //for (int i = 0; i < movePlayerOrgAv.Count; i++)
                                //{
                                //    if (movePlayerOrgAv[i].orgNo > 27 && movePlayerOrgAv[i].orgNo < 34)
                                //    {

                                //    }
                                //}
                                for (int i = 0; i < movePlayerOrgAv.Count; i++)
                                {
                                    if (movePlayerOrgAv[i].orgNo > 27 && movePlayerOrgAv[i].orgNo < 34)
                                    {
                                        isKillAvliable = true;
                                        break;
                                    }
                                }

                                if (isKillAvliable == true)
                                {
                                    int cnt = 0;
                                    bool isExist = false;
                                    for (int i = 0; i < pasaBotPlayer.Count; i++)
                                    {
                                        if (pasaBotPlayer[i].orgNo == 27 && pasaBotPlayer[i].pasaCurrentNo == 1 && isExist == false)
                                        {
                                            isExist = true;
                                        }
                                        else
                                        {
                                            if (pasaBotPlayer[i].orgNo == 0 && pasaBotPlayer[i].pasaCurrentNo == 0)
                                            {
                                                cnt++;
                                            }
                                        }
                                    }
                                    if (cnt > 0 && isExist == false)
                                    {
                                        PasaManage pSafeManage = safeBotPlayer[0];
                                        botMoveTokeStore.moveNo = 6;
                                        botMoveTokeStore.pasaToken = null;//Greejesh Create a Null
                                        botMoveTokeStore.isMoveSend = true;
                                    }
                                    else
                                    {
                                        PasaManage pSafeManage = safeBotPlayer[0];
                                        botMoveTokeStore.moveNo = Bot_Random_Genrate();
                                        botMoveTokeStore.pasaToken = pSafeManage;
                                        botMoveTokeStore.isMoveSend = true;
                                    }
                                }
                                else
                                {
                                    PasaManage pSafeManage = safeBotPlayer[0];
                                    botMoveTokeStore.moveNo = Bot_Random_Genrate();
                                    botMoveTokeStore.pasaToken = pSafeManage;
                                    botMoveTokeStore.isMoveSend = true;
                                }
                            }
                        }
                        else
                        {
                            PasaManage pSafeManage = safeBotPlayer[0];
                            botMoveTokeStore.moveNo = Bot_Random_Genrate();
                            botMoveTokeStore.pasaToken = pSafeManage;
                            botMoveTokeStore.isMoveSend = true;
                        }

                        //PasaManage pSafeManage = safeBotPlayer[0];
                        //bool isFindEnter = false;
                        //for (int i = 1; i < 7; i++)
                        //{
                        //    int checkNo = pSafeManage.orgNo + i;
                        //    if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isFindEnter == false)
                        //    {
                        //        isFindEnter = true;
                        //        botMoveTokeStore.moveNo = i;
                        //        botMoveTokeStore.pasaToken = pSafeManage;
                        //        botMoveTokeStore.isSafeSend = true;

                        //        //generatePasaNo = i;
                        //    }
                        //}
                    }
                    else
                    {
                        //generatePasaNo = Bot_Random_Genrate();
                        botMoveTokeStore.moveNo = Bot_Random_Genrate();
                        int rno = UnityEngine.Random.Range(0, moveBotPlayer.Count);
                        botMoveTokeStore.pasaToken = moveBotPlayer[rno];
                        botMoveTokeStore.isMoveSend = true;

                    }
                }
            }
        }
        else
        {
            botMoveTokeStore.moveNo = Bot_Random_Genrate();
            botMoveTokeStore.pasaToken = null;
            // generatePasaNo = Bot_Random_Genrate();
        }

        return botMoveTokeStore;
    }

    bool isFirstEnter = false;
    bool isSecondBotEnter = false;
    int botRandomGenCnt = 0;
    int botSixCounter = 0;
    bool OneTimeEnter = false;
    int Bot_Random_Genrate()
    {
        int rGen = 0;
        //int chanceForMoreThanThree = UnityEngine.Random.Range(1, 5);
        //if (chanceForMoreThanThree > 1)
        //    rGen = UnityEngine.Random.Range(4, 7);
        //else
        if (DataManager.Instance.modeType != 3)
            rGen = UnityEngine.Random.Range(1, 7);

        else if (DataManager.Instance.modeType == 3)
        {
            //int rGen = 0;
            int chanceForMoreThanThree = UnityEngine.Random.Range(1, 5);
            if (chanceForMoreThanThree > 1)
                rGen = UnityEngine.Random.Range(4, 7);
            else
                rGen = UnityEngine.Random.Range(1, 7);
        }



        //First Test
        botRandomGenCnt++;
        int checkCnt = 0;
        if (botRandomGenCnt == 3)
        {
            for (int i = 0; i < pasaBotPlayer.Count; i++)
            {
                if (pasaBotPlayer[i].pasaCurrentNo == 0)
                {
                    checkCnt++;
                }
            }
        }
        if (checkCnt == 4 && OneTimeEnter == false)
        {
            OneTimeEnter = true;
            rGen = 6;
        }
        //if (botRandomGenCnt <= 2)
        //{

        //    rGen = 6;
        //}
        //else
        //{
        //    rGen = UnityEngine.Random.Range(1, 7);
        //}
        //if (botSixManage == false)
        //{
        //    botSixManage = true;

        //}

        if (rGen == 6)
        {
            botSixCounter++;
        }

        if (botSixCounter == 3)
        {
            rGen = UnityEngine.Random.Range(1, 6);
            botSixCounter = 0;
        }
        //Normal Test
        //if (rGen == 6)
        //{
        //    if (isFirstEnter == false)
        //    {
        //        isFirstEnter = true;
        //    }
        //    else if (isSecondBotEnter == false)
        //    {
        //        isSecondBotEnter = true;
        //    }
        //    else
        //    {
        //        rGen = UnityEngine.Random.Range(1, 6);
        //        isFirstEnter = false;
        //        isSecondBotEnter = false;
        //    }
        //}

        //Second Test
        //if (DataManager.Instance.isBotSix)
        //{
        //    DataManager.Instance.isBotSix = false;
        //    rGen = 6;

        //    //rGen = DataManager.Instance.botPasaNo;
        //}
        //else
        //{
        //    rGen = DataManager.Instance.botPasaNo;
        //}
        //rGen = 1;
        return rGen;
    }
    
    public void OnceTimeTurnBot()
    {
        RestartTimer();
        print("OnceTimeTurnBot");
        GenerateDiceNumberStart_Bot(false);
        //BotChangeTurn(false, true);
    }


    List<PasaManage> MoveablePlayer()
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }
        return movePlayerAv;
    }

    List<PasaManage> KillPlayerBot()
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }

        List<PasaManage> killPlayerAv = new List<PasaManage>();
        if (movePlayerAv.Count > 0)
        {
            List<PasaManage> killMainPlayer = new List<PasaManage>();

            for (int i = 0; i < pasaObjects.Count; i++)
            {
                int checkNo = pasaObjects[i].GetComponent<PasaManage>().pasaCurrentNo;
                if (checkNo != 1 && checkNo != 9 && checkNo != 14 && checkNo != 22 && checkNo != 27 && checkNo != 35 && checkNo != 40 && checkNo != 48 && pasaObjects[i].GetComponent<PasaManage>().playerNo == DataManager.Instance.playerNo)
                {
                    killMainPlayer.Add(pasaObjects[i].GetComponent<PasaManage>());
                }
            }

            PasaManage temp = null;
            if (killMainPlayer.Count > 0)
            {
                for (int i = 0; i < killMainPlayer.Count - 1; i++)
                {
                    // traverse i+1 to array length
                    for (int j = i + 1; j < killMainPlayer.Count; j++)
                    {
                        if (killMainPlayer[i].pasaCurrentNo < killMainPlayer[j].pasaCurrentNo)
                        {
                            temp = killMainPlayer[i];
                            killMainPlayer[i] = killMainPlayer[j];
                            killMainPlayer[j] = temp;
                        }
                    }
                }
                bool isKillPlayerStore = false;
                for (int i = 0; i < killMainPlayer.Count; i++)
                {
                    for (int j = 0; j < movePlayerAv.Count; j++)
                    {
                        int killPlayerOrgNo = killMainPlayer[i].orgNo;
                        int mainPlayerOrgNo = movePlayerAv[j].orgNo;
                        for (int k = 1; k < 7; k++)
                        {
                            int checkNo = mainPlayerOrgNo + k;
                            if (checkNo == killPlayerOrgNo && isKillPlayerStore == false)
                            {
                                isKillPlayerStore = true;
                                killPlayerAv.Clear();

                                // Two Type Manage First is Bot Player Kill Second Element is Get Pasa Number to kill
                                killPlayerAv.Add(movePlayerAv[j]);
                                killPlayerAv.Add(killMainPlayer[i]);
                            }
                        }
                    }
                }
            }
            else
            {
                //
                killPlayerAv = killMainPlayer;
            }


        }
        return killPlayerAv;
    }

    List<PasaManage> HomePlayerBot()
    {
        List<PasaManage> homePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 51)
            {
                homePlayerAv.Add(pasaCollectList[i]);
            }
        }


        return homePlayerAv;
    }

    List<PasaManage> SafePlayerBot()
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }

        List<PasaManage> safePlayerAv = new List<PasaManage>();
        bool isBotSafeEnter = false;
        if (movePlayerAv.Count > 0)
        {
            for (int i = 0; i < movePlayerAv.Count; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    int checkNo = movePlayerAv[i].orgNo + j;
                    if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isBotSafeEnter == false)
                    {
                        safePlayerAv.Add(movePlayerAv[i]);
                        isBotSafeEnter = true;
                    }
                }
            }
        }
        else
        {
            safePlayerAv.Clear();
        }
        return safePlayerAv;
    }

    /*List<PasaManage> SafeMovePlayerBot(List<PasaManage> safeBotPlayer)
    {
        List<PasaManage> movePlayerAv = new List<PasaManage>();
        for (int i = 0; i < pasaCollectList.Count; i++)
        {
            if (pasaCollectList[i].pasaCurrentNo > 0)
            {
                movePlayerAv.Add(pasaCollectList[i]);
            }
        }



        List<PasaManage> safeMovePlayerAv = new List<PasaManage>();
        bool isBotSafeEnter = false;
        if (movePlayerAv.Count > 0)
        {
            for (int i = 0; i < movePlayerAv.Count; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    int checkNo = movePlayerAv[i].orgNo - j;
                    if ((checkNo == 1 || checkNo == 9 || checkNo == 14 || checkNo == 22 || checkNo == 27 || checkNo == 35 || checkNo == 40 || checkNo == 48) && isBotSafeEnter == false)
                    {
                        safeMovePlayerAv.Add(movePlayerAv[i]);
                        isBotSafeEnter = true;
                    }
                }
            }
        }
        else
        {
            safeMovePlayerAv.Clear();
        }


        return safeMovePlayerAv;
    }
    */
    //public bool BOTBOOL;
    void WaitTurnChangeAfter()
    {
        print("WaitTurnChangeAfter");
        GenerateDiceNumberStart_Bot(false);
    }
    
    public bool isPlayerNextTurn()
    {
        int index = playerRoundChecker;
        //if (playerRoundChecker == 0)
        //    playerRoundChecker = 1;
        if(DataManager.Instance.joinPlayerDatas.Count == playerRoundChecker)
            index = 0;
        //if (playerRoundChecker == 4)
        //    index = 0;
        if (DataManager.Instance.joinPlayerDatas[index].userId.Contains("Ludo"))
        {
            Debug.Log("Next turn is bot");
            return false;
        }
        else
        {
            Debug.Log("Next turn is player");
            return true;
        }
    }
    public bool isBotTurn = false;
    public void BotChangeTurn(bool isSendBot, bool isSendPlayer)
    {
        //print("Enter The change turn : " + isSendBot + "  Else : " + isSendPlayer);
        SoundManager.Instance.TickTimerStop();
        print("BotChangeTurn called" + playerRoundChecker);
        if (DataManager.Instance.modeType == 3 && playerRoundChecker == DataManager.Instance.playerNo && DataManager.Instance.isFourPlayer)
        {
            Destroy(LudoUIManager.Instance.bottomOneLineParent.transform.GetChild(0).gameObject);
            
        }
        if (DataManager.Instance.isFourPlayer)
        {
            ChangeTurn();
        }
        if (DataManager.Instance.isFourPlayer)
        {
            if(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId.Contains("Ludo"))
            {
                isSendBot = false;
                isSendPlayer = true;
            }
            else
            {
                isSendBot = true;
                isSendPlayer = false;
            }
        }
        if (isSendPlayer)
        {
            
            
            if (DataManager.Instance.isTwoPlayer && DataManager.Instance.modeType == 3 && dicelessPlayerNo == 1)
            {
                Destroy(LudoUIManager.Instance.bottomOneLineParent.transform.GetChild(0).gameObject);
                LudoUIManager.Instance.FirstNumberRemove();
                LudoUIManager.Instance.UpdateBottomDropDown();
            }
            else if (DataManager.Instance.isTwoPlayer && DataManager.Instance.modeType == 3 && dicelessPlayerNo == 3)
            {
                dicelessPlayerNo = 1;
            }
            
                isBotTurn = true;
            //botSixManage = false;
            BotDiceChange(DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].userId, DataManager.Instance.joinPlayerDatas[playerRoundChecker - 1].playerNo, false);//comment if there is conflict in 2 player
            botSixCounter = 0;
            isClickAvaliableDice = 1;
            SoundManager.Instance.UserTurnSound();
            DataManager.Instance.isDiceClick = false;
            DataManager.Instance.isTimeAuto = false;
            // PlayerDiceChange();
            //OtherShadowMainTain();
            if (DataManager.Instance.isTwoPlayer)
                OtherShadowMainTain();
            else
                OurShadowMaintain();
            RestartTimer();
            Invoke(nameof(WaitTurnChangeAfter), UnityEngine.Random.Range(0.95f, 1.5f));
            //GenerateDiceNumberStart_Bot(false);
        }
        else if (isSendBot)
        {
            if (DataManager.Instance.isTwoPlayer && DataManager.Instance.modeType == 3 && dicelessPlayerNo == 1)
                LudoUIManager.Instance.FirstNumberYellow();
            isBotTurn = false;
            SoundManager.Instance.UserTurnSound();
            if (DataManager.Instance.GetVibration() == 0)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    MMNVAndroid.AndroidVibrate(100);
                }
            }
            if (playerRoundChecker == DataManager.Instance.playerNo && DataManager.Instance.isFourPlayer)
            {
                isClickAvaliableDice = 0;
                DataManager.Instance.isDiceClick = true;
                //DataManager.Instance.isTimeAuto = true;
            }
            else if(DataManager.Instance.isFourPlayer)
            {
                DataManager.Instance.isDiceClick = false;
                isClickAvaliableDice = 1;
            }
            else if(DataManager.Instance.isTwoPlayer)
            {
                isClickAvaliableDice = 0;
                DataManager.Instance.isDiceClick = true;
            }
            //if(DataManager.Instance.modeType != 3)
            print("Changing turn before moving");
                PlayerDiceChange();
            isCheckEnter = false;
            isClickAvaliableDice = 0;
            OurShadowMaintain();
            DataManager.Instance.isTimeAuto = false;
            RestartTimer();
            if (DataManager.Instance.modeType == 3)
            {
                DiceLessPasaButton();
            }
        }
    }

    #endregion


}
[System.Serializable]
public class DiceLessDataStore
{
    public int no;
    public List<int> numberList = new List<int>();

}
[System.Serializable]
public class BotMoveTokeStore
{
    public int moveNo;
    public PasaManage pasaToken;
    public bool isMoveSend;
    public bool isKillSend;
    public bool isSafeSend;
    public bool isHomeSend;
}