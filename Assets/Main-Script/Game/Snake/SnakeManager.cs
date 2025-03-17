using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking;

[System.Serializable]
public class SnakeBoard
{
    public int fisrtNo;
    public int lastNo;
    public DOTweenPath path;
}
public class SnakeManager : MonoBehaviour
{

    public static SnakeManager Instance;
    public GameObject[] allObj;
    public Sprite[] profileAvatar;

    public SnakePasa yellowPasa, redPasa;
    public int pasaCurrentNo;

    public Text playerNameTxt1;
    public Text playerNameTxt2;

    public Image playerProfile1;
    public Image playerProfile2;

    public Image fillProfile1;
    public Image fillProfile2;

    public Color playerColor1;
    public Color playerColor2;


    public Image pasaImg;
    public Sprite[] pasaSprite;
    public Text timerTxt;

    public List<SnakeBoard> upSnkaeBoard = new List<SnakeBoard>();
    public List<SnakeBoard> downSnkaeBoard = new List<SnakeBoard>();

    public float secondsCount;
    public Image timerFillImg;
    public int isClickAvaliableDice;
    public bool isPathClickAvaliable;
    public bool isPathClick;


    public DOTweenPath path26;
    public DOTweenPath path39;
    public DOTweenPath path51;

    int flag = 0;

    public float timerSpeed;

    public GameObject turnObj;
    public GameObject turnGenObj;


    bool isBotTurn;

    [Header("---Life Manage---")]
    public Image[] box1Lifes;
    public Image[] box2Lifes;
    public Color lifeOnColor, lifeOffColor;

    [Header("---Match Win Manage---")]

    public GameObject winScreenObj;
    public bool isOpenWin;
    public bool isOtherPlayLeft;
    public int playerScoreCnt1;
    public int playerScoreCnt2;


    bool isUpdateOne = false;

    public bool isClickDice = false;

    int winBotCnt6 = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //secondsCount = (10 * 60);
        secondsCount = (TestSocketIO.Instace.playTime * 60);
    }

    // Start is called before the first frame update
    void Start()
    {
        AddBetAmount();
        pasaCurrentNo = Random.Range(1, 7);
        pasaImg.sprite = pasaSprite[pasaCurrentNo - 1];
        SoundManager.Instance.StopBackgroundMusic();
        PlayerNameManage();

        if (DataManager.Instance.playerNo == 1)
        {
            DataManager.Instance.isDiceClick = true;
        }
        else
        {
            DataManager.Instance.isDiceClick = false;
        }
        playerScoreCnt1 = 0;
        playerScoreCnt2 = 0;
        timerFillImg.color = playerColor1;
        winBotCnt6 = Random.Range(1, 4);
    }

    private void AddBetAmount()
    {
        DataManager.Instance.DebitAmount((DataManager.Instance.tourEntryMoney).ToString(), TestSocketIO.Instace.roomid, "Snake-Bet-" + TestSocketIO.Instace.roomid, "game", 0);
    }
    

    public void PlayerNameManage()
    {
        if (DataManager.Instance.isTwoPlayer == true)
        {
            //int index = DataManager.Instance.playerNo;
            int index1 = 0;
            int index2 = 1;
            
            if (DataManager.Instance.playerNo == 2)
            {
                index1 = 1;
                index2 = 0;
            }
            playerNameTxt1.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
            playerNameTxt2.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);


            //playerProfile1.sprite = profileAvatar[DataManager.Instance.joinPlayerDatas[index1].avtar];
            //playerProfile2.sprite = profileAvatar[DataManager.Instance.joinPlayerDatas[index2].avtar];

            StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, playerProfile1));
            StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, playerProfile2));

            if (DataManager.Instance.playerNo == 1)
            {
                fillProfile1.color = playerColor1;
                fillProfile2.color = playerColor2;
            }
            else if (DataManager.Instance.playerNo == 2)
            {
                fillProfile1.color = playerColor2;
                fillProfile2.color = playerColor1;

                if (BotManager.Instance.isConnectBot)
                {
                    SnakeBotSend(true);
                }
            }

        }
    }

    IEnumerator GetImages(string URl, Image image)
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


    bool isLifeEnter = false;
    // Update is called once per frame
    void Update()
    {
        Timer();
        if (true)
        {
            timerFillImg.fillAmount -= 1.0f / timerSpeed * Time.deltaTime;
            if (timerFillImg.fillAmount == 0 && DataManager.Instance.isTimeAuto == false)
            {
                if (isTimeEnter == true)
                {
                    LifeDecrease();
                    //if (isUpdateOne == false)
                    //{
                    //    isUpdateOne = true;
                    //    isClickAvaliableDice = 0;
                    //    PlayerChangeTurn();
                    //}
                }
            }
            else if (timerFillImg.fillAmount < 0.5f && isTimeEnter == false && DataManager.Instance.isTimeAuto == false)
            {
                isLifeEnter = false;
                isTimeEnter = true;
                if (DataManager.Instance.isDiceClick)
                {
                    TickSound();
                }
            }
        }
    }







    bool isTimeFinish = false;

    int cntPlayer1 = 0;
    int cntPlayer2 = 0;
    void LifeDecrease()
    {
        SoundManager.Instance.TickTimerStop();
        if (DataManager.Instance.isTwoPlayer && isLifeEnter == false)
        {
            isLifeEnter = true;
            //isClickAvaliableDice = 0;
            //PlayerChangeTurn();
            if (DataManager.Instance.isDiceClick == true)
            {
                SoundManager.Instance.TimeOutSound();
                if (cntPlayer1 == 3)
                {
                    isTimeFinish = true;
                    isOtherPlayLeft = false;
                    WinUserShow();
                }
                else
                {
                    box1Lifes[cntPlayer1].color = lifeOffColor;
                    cntPlayer1++;
                }
                if (BotManager.Instance.isConnectBot)
                {
                    Change_Turn_Bot(false, true);
                }
                else
                {
                    PlayerChangeTurn();
                }
                isLifeEnter = false;
            }
            else if (DataManager.Instance.isDiceClick == false && DataManager.Instance.isTimeAuto == false)
            {
                if (cntPlayer2 == 3)
                {
                    isOtherPlayLeft = true;
                    WinUserShow();
                }
                else
                {
                    box2Lifes[cntPlayer2].color = lifeOffColor;
                    cntPlayer2++;
                    ReconnectPasaChange();
                }
                isLifeEnter = false;

            }
            //print("Enter The Life is Decrese");
        }
    }

    void ReconnectPasaChange()
    {
        DataManager.Instance.isDiceClick = true;
        isClickAvaliableDice = 0;
        //OurShadowMaintain();
        DataManager.Instance.isTimeAuto = false;
        RestartTimer();
    }


    public void TimerStop()
    {
        DataManager.Instance.isTimeAuto = true;
        timerFillImg.fillAmount = 0;
    }




    void TickSound()
    {
        SoundManager.Instance.TickTimerSound();
        Invoke(nameof(CheckTickSound), 1f);
    }

    void CheckTickSound()
    {
        if (timerFillImg.fillAmount != 0 && DataManager.Instance.isTimeAuto == false && DataManager.Instance.isDiceClick == true)
        {
            TickSound();
        }
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
        if (timeValue.Equals("00:00"))
        {
            //print("Time Over");
            timerTxt.text = "00:00";
            WinUserShow();
            flag = 1;
        }
        if (flag != 1)
        {
            timerTxt.text = timeValue;
        }
    }

    public void TimerSliderColorChange()
    {

        if (DataManager.Instance.playerNo == 1)
        {
            timerFillImg.color = playerColor1;
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            timerFillImg.color = playerColor2;
        }
    }

    public void WinUserShow()
    {
        winScreenObj.SetActive(true);
    }

    bool isTimeEnter = false;
    public void RestartTimer()
    {
        isTimeEnter = false;
        DataManager.Instance.isRestartManage = true;
        timerFillImg.fillAmount = 1;
        isUpdateOne = false;
        //print("Restart Timer : ");
    }

    public void PlayerChangeTurn()
    {
        print("Enter The Player Snake Change");


        SoundManager.Instance.UserTurnSound();
        DataManager.Instance.isDiceClick = false;
        //print("Enter The Player Change : "+DataManager.Instance.isDiceClick);
        DataManager.Instance.isTimeAuto = false;
        if (DataManager.Instance.playerNo == 1)
        {
            timerFillImg.color = playerColor2;
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            timerFillImg.color = playerColor1;
        }
        PlayerDiceChange();
        //OtherShadowMainTain();
        RestartTimer();
    }
    int isSix1 = 0;
    int isSix2 = 0;
    public void PasaButtonClick()
    {
        if (DataManager.Instance.isDiceClick == true && isClickAvaliableDice == 0)
        {
            isClickDice = true;
            SoundManager.Instance.RollDice_Start_Sound();
            pasaImg.gameObject.GetComponent<Animator>().enabled = true;

            if (DataManager.Instance.isAvaliable)
            {
                pasaCurrentNo = DataManager.Instance.currentPNo;
            }
            else
            {
                pasaCurrentNo = Random.Range(1, 7);
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
                    pasaCurrentNo = Random.Range(1, 5);
                }
            }

            isPathClick = true;
            isClickAvaliableDice = 1;
            if (BotManager.Instance.isConnectBot)
            {

            }
            else
            {
                PlayerDice(pasaCurrentNo);
            }
            Invoke(nameof(GenerateDiceNumber), 1.25f);

        }
        else
        {
            GenerateTurnObj();
        }
    }

    public void GenerateTurnObj()
    {
        Instantiate(turnObj, turnGenObj.transform);
    }

    void GenerateDiceNumber()
    {
        pasaImg.gameObject.GetComponent<Animator>().enabled = false;

        SoundManager.Instance.RollDice_Stop_Sound();
        //        PasaImageManage(pasaCurrentNo, 1, false);
        pasaImg.sprite = pasaSprite[pasaCurrentNo - 1];
        //isPathClickAvaliable = true;

        if (DataManager.Instance.playerNo == 1)
        {
            bool isCheckEnter = false;

            if (yellowPasa.pasaCurrentNo == 100 && pasaCurrentNo != 6)
            {
                //Turn Change
                isCheckEnter = true;
                isClickAvaliableDice = 0;
                PlayerChangeTurn();
            }
            else if ((yellowPasa.pasaCurrentNo + pasaCurrentNo) > 100 && yellowPasa.pasaCurrentNo != 100)
            {
                //Turn Change
                isCheckEnter = true;
                isClickAvaliableDice = 0;
                PlayerChangeTurn();
            }
            if (isCheckEnter == false)
            {
                yellowPasa.isStopZoom = false;
                yellowPasa.PlayerPasaZoom();
            }
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            bool isCheckEnter = false;

            if (redPasa.pasaCurrentNo == 100 && pasaCurrentNo != 6)
            {
                //Turn Change
                isCheckEnter = true;
                isClickAvaliableDice = 0;
                PlayerChangeTurn();
            }
            else if ((redPasa.pasaCurrentNo + pasaCurrentNo) > 100 && redPasa.pasaCurrentNo != 100)
            {
                //Turn Change
                isCheckEnter = true;
                isClickAvaliableDice = 0;
                PlayerChangeTurn();
            }
            if (isCheckEnter == false)
            {
                redPasa.isStopZoom = false;
                redPasa.PlayerPasaZoom();
            }
        }
        PlayerAutoMove();
        
    }


    private void PlayerAutoMove()
    {
        switch (DataManager.Instance.playerNo)
        {
            case 1:
                PathButtonClick(yellowPasa.pasaCurrentNo);
                break;
            case 2:
                PathButtonClick(redPasa.pasaCurrentNo);
                break;
        }
    }
    
    



    public void PathButtonClick(int no)
    {
        if (DataManager.Instance.isDiceClick == true && isPathClick == true && isPathClickAvaliable == false && isClickDice == true)
        {
            bool isEnterAv = false;
            if (DataManager.Instance.playerNo == 1)
            {
                if (yellowPasa.pasaCurrentNo == no)
                {
                    isEnterAv = true;
                }
            }
            else if (DataManager.Instance.playerNo == 2)
            {
                if (redPasa.pasaCurrentNo == no)
                {
                    isEnterAv = true;
                }
            }
            if (isEnterAv)
            {
                if (BotManager.Instance.isConnectBot)
                {
                    isPathClickAvaliable = true;
                }

                isClickDice = false;
                DataManager.Instance.isDiceClick = false;
                if (DataManager.Instance.playerNo == 1)
                {
                    yellowPasa.isStopZoom = true;
                    yellowPasa.IncrementPasa(pasaCurrentNo, false, false);
                    MovePlayer(1, pasaCurrentNo);
                }
                else if (DataManager.Instance.playerNo == 2)
                {
                    redPasa.isStopZoom = true;
                    redPasa.IncrementPasa(pasaCurrentNo, false, false);
                    MovePlayer(2, pasaCurrentNo);
                }
            }

        }
    }


    #region Bot Manager

    void SnakeBotSend(bool isStart)
    {
        if (BotManager.Instance.isConnectBot == true)
        {
            if (isStart)
            {
                int rno_Pasa = Random.Range(1, 7);
                StartCoroutine(Move_Generate_Pasa_Bot(rno_Pasa));
            }
            else
            {
                if (BotManager.Instance.botType == BotType.Easy)
                {
                    int rno_Pasa = Random.Range(1, 7);
                    StartCoroutine(Move_Generate_Pasa_Bot(rno_Pasa));
                }
                else if (BotManager.Instance.botType == BotType.Medium)
                {
                    int rnoSimple = Random.Range(0, 3);
                    if (rnoSimple == 1 || rnoSimple == 2)
                    {
                        int no = GenerateCustomeSetNo();
                        print("Pass No : " + no);
                        StartCoroutine(Move_Generate_Pasa_Bot(no));
                    }
                    else
                    {
                        int rno_Pasa = Random.Range(1, 7);
                        StartCoroutine(Move_Generate_Pasa_Bot(rno_Pasa));
                    }
                }
                else if (BotManager.Instance.botType == BotType.Hard)
                {
                    int no = GenerateCustomeSetNo();
                    //print("Pass No : " + no);
                    StartCoroutine(Move_Generate_Pasa_Bot(no));

                }
            }
        }
    }
    int winTempCnt = 0;
    int GenerateCustomeSetNo()
    {
        int cNo = 0;

        int botCurrentNo = 0;
        if (DataManager.Instance.playerNo == 2)
        {
            botCurrentNo = yellowPasa.pasaCurrentNo;
        }
        else if (DataManager.Instance.playerNo == 1)
        {
            botCurrentNo = redPasa.pasaCurrentNo;
        }


        if (botCurrentNo == 100)
        {
            winTempCnt++;
            if (winTempCnt == winBotCnt6)
            {
                cNo = 6;
            }
            else
            {
                cNo = Random.Range(1, 6);
            }
        }
        else
        {
            int rno = Random.Range(0, 3);

            if (rno == 1 || rno == 2)
            {
                if (botCurrentNo < 4)
                {
                    int tempNo = 4 - botCurrentNo;
                    if (tempNo >= 1 && tempNo <= 6)
                    {
                        cNo = tempNo;
                    }
                    else
                    {
                        cNo = Random.Range(3, 7);
                    }
                }
                else
                {
                    if (botCurrentNo < 29)
                    {
                        int tempNo = 29 - botCurrentNo;
                        if (tempNo >= 1 && tempNo <= 6)
                        {
                            cNo = tempNo;
                        }
                        else
                        {
                            cNo = Random.Range(3, 7);
                        }
                    }
                    else
                    {
                        if (botCurrentNo < 36)
                        {
                            int tempNo = 36 - botCurrentNo;
                            if (tempNo >= 1 && tempNo <= 6)
                            {
                                cNo = tempNo;
                            }
                            else
                            {
                                cNo = Random.Range(3, 7);
                            }
                        }
                        else
                        {
                            if (botCurrentNo < 59)
                            {
                                int tempNo = 59 - botCurrentNo;
                                if (tempNo >= 1 && tempNo <= 6)
                                {
                                    cNo = tempNo;
                                }
                                else
                                {
                                    cNo = Random.Range(3, 7);
                                }
                            }
                            else
                            {
                                if (botCurrentNo < 70)
                                {
                                    int tempNo = 70 - botCurrentNo;
                                    if (tempNo >= 1 && tempNo <= 6)
                                    {
                                        cNo = tempNo;
                                    }
                                    else
                                    {
                                        cNo = Random.Range(3, 7);
                                    }
                                }
                                else
                                {
                                    cNo = Random.Range(3, 7);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                print("Enter The Set Cno : " + cNo);
                cNo = Random.Range(3, 7);
            }

            if ((botCurrentNo + cNo) == 26 || (botCurrentNo + cNo) == 41 || (botCurrentNo + cNo) == 69 || (botCurrentNo + cNo) == 87 || (botCurrentNo + cNo) == 97)
            {
                if (cNo == 6 || cNo == 5 || cNo == 4)
                {
                    print("Before Cno : " + cNo);
                    cNo -= Random.Range(1, 4);
                    print("Final CNO : " + cNo);
                }
                else
                {
                    print("Before Cno : " + cNo);
                    cNo += Random.Range(1, 4);
                    print("Final CNO : " + cNo);
                }

            }
            if (botCurrentNo + cNo > 99)
            {
                cNo = 99 - botCurrentNo;
            }

        }
        return cNo;
    }

    IEnumerator Move_Generate_Pasa_Bot(int sendNo)
    {
        yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
        DataManager.Instance.isDiceClick = true;
        SoundManager.Instance.RollDice_Start_Sound();
        pasaImg.gameObject.GetComponent<Animator>().enabled = true;
        StartCoroutine(GenerateDiceNumber_Bot(sendNo));
    }

    IEnumerator GenerateDiceNumber_Bot(int sendNo)
    {
        yield return new WaitForSeconds(1.25f);
        pasaImg.gameObject.GetComponent<Animator>().enabled = false;

        SoundManager.Instance.RollDice_Stop_Sound();

        pasaImg.sprite = pasaSprite[sendNo - 1];

        yield return new WaitForSeconds(1.25f);

        if (DataManager.Instance.playerNo == 2)
        {
            yellowPasa.IncrementPasa(sendNo, false, true);
        }
        else
        {
            redPasa.IncrementPasa(sendNo, false, true);
        }

    }

    public void Change_Turn_Bot(bool isSendPlayer, bool isSendBot)
    {

        if (isSendPlayer)
        {
            isClickAvaliableDice = 0;

            isPathClickAvaliable = false;
            SoundManager.Instance.UserTurnSound();
            DataManager.Instance.isDiceClick = true;
            if (DataManager.Instance.playerNo == 1)
            {
                timerFillImg.color = playerColor1;
            }
            else if (DataManager.Instance.playerNo == 2)
            {
                timerFillImg.color = playerColor2;
            }
            RestartTimer();
        }
        else if (isSendBot)
        {
            print("Send Bot Condition");
            if (DataManager.Instance.playerNo == 2)
            {
                timerFillImg.color = playerColor1;
            }
            else if (DataManager.Instance.playerNo == 1)
            {
                timerFillImg.color = playerColor2;
            }
            RestartTimer();
            SnakeBotSend(false);
        }
    }

    #endregion

    #region Socket Send
    void PlayerDice(int diceNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("DiceNo", diceNo);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("DiceManageCnt", DataManager.Instance.diceManageCnt);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        TestSocketIO.Instace.Senddata("SnakeDiceData", obj);
    }



    public void PlayerDiceChange()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        int noSend = 0;
        //noSend = DataManager.Instance.playerNo;

        if (DataManager.Instance.playerNo == 1)
        {
            noSend = 2;
        }
        else if (DataManager.Instance.playerNo == 2)
        {
            noSend = 1;
        }
        print("Enter The Send No : " + noSend);

        obj.AddField("PlayerNo", noSend);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("SnakeDiceChangeData", obj);

    }
    public void MovePlayer(int pasaNo, int diceNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("PlayerNo", DataManager.Instance.playerNo);
        obj.AddField("TokenNo", pasaNo);
        obj.AddField("TokenMove", diceNo);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("SnakeData", obj);
    }

    #endregion

    #region Socket Receive

    public void AutoMove(int playerNo, int tokenNo, int move)
    {
        //print("Enter The Auto Move");
        //print("Player No : " + playerNo);
        //print("Token No : " + tokenNo);
        //print("Token move : " + move);
        if (tokenNo == 1)
        {
            yellowPasa.IncrementPasa(move, true, false);
        }
        else if (tokenNo == 2)
        {
            redPasa.IncrementPasa(move, true, false);
        }
    }

    public void AutoDice(int no, int pNo, int pNo1)
    {

        SoundManager.Instance.RollDice_Start_Sound();
        pasaImg.gameObject.GetComponent<Animator>().enabled = true;
        StartCoroutine(GenerateDiceNumber_Socket(no, pNo, pNo1));


    }
    IEnumerator GenerateDiceNumber_Socket(int no, int pNo, int pNo1)
    {
        yield return new WaitForSeconds(1.25f);
        pasaImg.gameObject.GetComponent<Animator>().enabled = false;
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
        pasaImg.sprite = pasaSprite[no - 1];


    }

    #endregion
}
