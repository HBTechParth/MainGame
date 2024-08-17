using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SevenUpDownManager : MonoBehaviour
{
    public static SevenUpDownManager Instance;

    public GameObject dealer;

    public Image playerAvatar;
    public Text playerName;

    public Text playerBalanceText;
    public Transform winParticleParent;
    public Text winAnimationTxt;
    public int DownRatio = 2;
    public int UpRatio = 2;
    public int OnRatio = 5;


    public List<GameObject> Chips;

    [Header("--- Error Screen ---")]
    public GameObject errorScreenObject;


    public int downBetValue;
    public int upBetValue;
    public int onBetValue;
    public int totalBet;


    public List<GameObject> downChips;
    public List<GameObject> upChips;
    public List<GameObject> onChips;


    [Header("--- Players Controller ---")]
    public List<SevenUpDownPlayerManager> playerList = new List<SevenUpDownPlayerManager>();


    public Text totalBetText;
    [Header("--- Dice Controller ---")]
    public Sprite[] diceNumbers;
    public GameObject diceRollObject;
    public GameObject dice1;
    public GameObject dice2;
    public int dice1Result;
    public int dice2Result;

    [Header("--- Start Stop Bet ---")]
    public GameObject startBetObj;
    public GameObject stopBetObj;


    public GameObject playerProfile;
    public Transform otherProfile;
    public GameObject chipPrefab;
    public Sprite[] chipSprite;//0 = 10//1 = 50//2 = 100//3 = 500// 4 = 1000
    public float[] chipValue;
    public Transform downArea;
    public Transform upArea;
    public Transform onArea;

    public int selectedChipNo;//0 = 10//1 = 50//2 = 100//3 = 500// 4 = 1000
    public float fixTimerValue;
    public float timerValue;
    public float secondsCount;
    public Text timerText;
    bool isEnterBetStop;
    private bool _isClickAvailable;



    [Header("--- Chip Generate Position ---")]
    public float min7Downx;//50 for down // -550 for up
    public float max7Downx;//600 for down // -100 for up
    public float min7Downy;
    public float max7Downy;

    public float min7Onx;//50 for down // -550 for up
    public float max7Onx;//600 for down // -100 for up
    public float min7Ony;
    public float max7Ony;

    public float min7Upx;//50 for down // -550 for up
    public float max7Upx;//600 for down // -100 for up
    public float min7Upy;
    public float max7Upy;

    public float upValue;//value by which the chip button would go up when selected
    public float downValue;//value by which the chip button would go down when deselected

    public GameObject waitNextRoundScreenObject;

    [Header("--- Win Object ---")]
    public GameObject down7Win;
    public GameObject up7Win;
    public GameObject on7Win;

    [Header("--- HistoryTracking---")]
    public Transform historyHolder;
    public GameObject resultPrefab;
    public List<int> winList;
    public List<int> diceResultList;

    public int maxWinListCount = 16;
    public List<GameObject> chipsHistory = new List<GameObject>();
    
    [Header("--- Sounds ---")]
    public Image soundImg;
    public Image vibrationImg;
    public Image musicImg;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite vibrationonSprite;
    public Sprite vibrationoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;

    private string playerPrefsKey = "SevenUpDownHistory";

    // Start is called before the first frame update

    public bool isAdmin;

    public enum BetValue
    {
        Bet10, Bet50, Bet100, Bet500, Bet1000, Bet5000, Bet0
    }
    public List<BetValue> TestBV = new List<BetValue> { BetValue.Bet10, BetValue.Bet50, BetValue.Bet100, BetValue.Bet500, BetValue.Bet1000, BetValue.Bet5000 };

    GameObject Manager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void UpdateBalance()
    {
        playerBalanceText.text = DataManager.Instance.playerData.balance.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in playerList)
        {
            item.gameObject.SetActive(false);
        }
        SoundManager.Instance.StopBackgroundMusic();
        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), playerAvatar));
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL") , playerAvatar);
        playerName.text = DataManager.Instance.playerData.firstName;
        ChipButtonClick(0);
        UpdateBalance();
        GetSetBotPlayer();
        HistoryLoader(DataManager.Instance.sevenUpDownWinHistory, DataManager.Instance.sevenUpDownDiceHistory);
        InitialiseScene();
        CheckSound();
    }
    

    public void InitialiseScene()
    {
        if(DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.sevenUpDownRequirePlayer)
        {
            CreateAdmin(); //Creating Admin
            if(DataManager.Instance.joinPlayerDatas.Count == 1 && isAdmin)
            {
                SetUpPlayers();
                StartCoroutine(StartBet());
            }
            else
            {
                if (isAdmin)
                    return;
                if (_isClickAvailable == false && isEnterBetStop == false)
                    waitNextRoundScreenObject.SetActive(true);
                print("Non admin player waiting for next round");
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerValue == 0 && isEnterBetStop == false && waitNextRoundScreenObject.activeSelf == false)
        {
            isEnterBetStop = true;
            StartCoroutine(StopBet());
            //Call Stop Betting function

        }
        else if (!timerText.text.Equals("0"))
        {
            secondsCount -= Time.deltaTime;
            timerValue = Mathf.CeilToInt(secondsCount);
            timerText.text = timerValue.ToString();
        }
    }



    #region GamePlay

    public bool CheckMoney(float money)
    {
        float currentBalance = float.Parse(DataManager.Instance.playerData.balance);
        if (currentBalance - money < 0)
            return false;
        else
            return true;
    }

    public void PlaceBetButton(int number)
    {
        if (_isClickAvailable == false)
            return;

        bool hasMoney = CheckMoney(chipValue[selectedChipNo]);
        if(hasMoney == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        switch (number)//1 = 7 Down //2 = 7 Up //3 = 7
        {
            case 1:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Seven_Up_Down_Bet-" + DataManager.Instance.gameId, "game", 13);
                    downBetValue += (int)chipValue[selectedChipNo];
                    Vector3 randomPosition = new Vector3(Random.Range(min7Downx, max7Downx), Random.Range(min7Downy, max7Downy));
                    GameObject chip = Instantiate(chipPrefab, downArea);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    downChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 2:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Seven_Up_Down_Bet-" + DataManager.Instance.gameId, "game", 13);
                    upBetValue += (int)chipValue[selectedChipNo];
                    Vector3 randomPosition = new Vector3(Random.Range(min7Upx, max7Upx), Random.Range(min7Upy, max7Upy));
                    GameObject chip = Instantiate(chipPrefab, upArea);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    upChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 3:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Seven_Up_Down_Bet-" + DataManager.Instance.gameId, "game", 13);
                    onBetValue += (int)chipValue[selectedChipNo];
                    Vector3 randomPosition = new Vector3(Random.Range(min7Onx, max7Onx), Random.Range(min7Ony, max7Ony));
                    GameObject chip = Instantiate(chipPrefab, onArea);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    onChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
        }
        totalBetText.text = totalBet.ToString();
        SendSevenUpDownBet(number, selectedChipNo);//SocketEvent to send to admin/other players
        UpdateBalance();
    }

    public void ChipButtonClick(int no)
    {
        selectedChipNo = no;
        for (int i = 0; i < Chips.Count; i++)
        {
            if (i == no)
                Chips[i].transform.DOLocalMoveY(upValue, 0.05f);
            else
                Chips[i].transform.DOLocalMoveY(downValue, 0.05f);
        }
    }

    public void RollDice()
    {
        diceRollObject.SetActive(true);
        SoundManager.Instance.RollDice_Start_Sound();



        dice1.GetComponent<Animator>().enabled = true;
        dice2.GetComponent<Animator>().enabled = true;
        Invoke(nameof(GenerateDiceNumbers), 2f);
    }

    public void GenerateDiceNumbers()
    {
        dice1.GetComponent<Animator>().enabled = false;
        dice2.GetComponent<Animator>().enabled = false;
        //SoundManager.Instance.RollDice_Stop_Sound();
        dice1.GetComponent<Image>().sprite = diceNumbers[dice1Result - 1];
        dice2.GetComponent<Image>().sprite = diceNumbers[dice2Result - 1];

        GetDice();
    }

    public void GetDice()
    {
        int diceTotal = 0;
        print("Dice1 = " + dice1Result + " Dice2 = " + dice2Result);

        diceTotal = dice1Result + dice2Result;

        if (isAdmin)
        {
            int winNo = 0;
            if (diceTotal < 7)
            {
                winNo = 1;
            }
            else if (diceTotal > 7)
            {
                winNo = 2;

            }
            else if (diceTotal == 7)
            {
                winNo = 3;

            }
            UpdateHistoryChips(winNo, diceTotal);
        }


        StartCoroutine(RoundOverAnimation(diceTotal));
    }

    public IEnumerator RoundOverAnimation(int diceTotal)
    {
        yield return new WaitForSeconds(3f);
        diceRollObject.SetActive(false);
        float animationSpeed = 0.2f;
        if (diceTotal < 7)
        {
            MoveChips(upChips, downArea, min7Downx, max7Downx, min7Downy, max7Downy, animationSpeed, downChips);
            MoveChips(onChips, downArea, min7Downx, max7Downx, min7Downy, max7Downy, animationSpeed, downChips);
            yield return new WaitForSeconds(animationSpeed);
            for (int i = 0; i < upArea.childCount; i++)
            {
                upArea.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < onArea.childCount; i++)
            {
                onArea.GetChild(i).gameObject.SetActive(false);
            }
            //for (int i = 0; i < upChips.Count; i++)
            //{
            //    upChips[i].transform.DOScale(Vector3.zero, animationSpeed);
            //    upChips[i].transform.DOMove(dealer.transform.position, animationSpeed).OnComplete(() =>
            //    {
            //        Vector3 randomPosition = new Vector3(Random.Range(min7Downx, max7Downx), Random.Range(min7Downy, max7Downy));
            //        upChips[i].transform.SetParent(downArea);
            //        ChipGenerate(upChips[i].gameObject, randomPosition);
            //    });

            //}

            //for (int i = 0; i < onChips.Count; i++)
            //{
            //    onChips[i].transform.DOScale(Vector3.zero, animationSpeed);
            //    onChips[i].transform.DOMove(dealer.transform.position, animationSpeed).OnComplete(() =>
            //    {
            //        Vector3 randomPosition = new Vector3(Random.Range(min7Downx, max7Downx), Random.Range(min7Downy, max7Downy));
            //        onChips[i].transform.SetParent(downArea);
            //        ChipGenerate(upChips[i].gameObject, randomPosition);
            //    });

            //}

        }
        else if (diceTotal > 7)
        {
            MoveChips(downChips, upArea, min7Upx, max7Upx, min7Upy, max7Upy, animationSpeed, upChips);
            MoveChips(onChips, upArea, min7Upx, max7Upx, min7Upy, max7Upy, animationSpeed, upChips);

            yield return new WaitForSeconds(animationSpeed);
            for (int i = 0; i < onArea.childCount; i++)
            {
                onArea.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < downArea.childCount; i++)
            {
                downArea.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (diceTotal == 7)
        {
            MoveChips(upChips, onArea, min7Onx, max7Onx, min7Ony, max7Ony, animationSpeed, onChips);
            MoveChips(downChips, onArea, min7Onx, max7Onx, min7Ony, max7Ony, animationSpeed, onChips);
            yield return new WaitForSeconds(animationSpeed);
            for (int i = 0; i < upArea.childCount; i++)
            {
                upArea.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < downArea.childCount; i++)
            {
                downArea.GetChild(i).gameObject.SetActive(false);
            }

        }

        float investPrice = 0;
        float betPrice = 0;
        int winNo = 0;

        if (diceTotal < 7)
        {
            winNo = 1;
            down7Win.SetActive(true);
            betPrice = downBetValue;
            investPrice = betPrice * 2;
        }
        else if (diceTotal > 7)
        {
            winNo = 2;
            up7Win.SetActive(true);
            betPrice = upBetValue;
            investPrice = betPrice * 2;
        }
        else if (diceTotal == 7)
        {
            winNo = 3;
            on7Win.SetActive(true);
            betPrice = onBetValue;
            investPrice = betPrice * 5;
        }

        float adminPercentage = DataManager.Instance.adminPercentage;
        if (betPrice != 0)
        {
            float winReward = investPrice - betPrice;
            float adminCommission = adminPercentage / 100;
            float winAmount = winReward - (winReward * adminCommission);
            float playerWinAmount = betPrice + winAmount;
            print("Player wins, betPrice = " + betPrice);
            winAnimationTxt.gameObject.SetActive(true);
            winAnimationTxt.text = "+" + playerWinAmount;
            Invoke(nameof(WinAmountTextOff), 1.25f);
            //winning animation and credit amount
            GameObject particleEffect = Instantiate(DataManager.Instance.winParticles, winParticleParent);
            Destroy(particleEffect, 2.5f);
            SoundManager.Instance.CasinoWinSound();
            DataManager.Instance.AddAmount(playerWinAmount, DataManager.Instance.gameId, "Seven_Up_Down_Win" + DataManager.Instance.gameId, "won", adminCommission, winNo);
            UpdateBalance();
        }
        yield return new WaitForSeconds(3f);
        dice1Result = 0;
        dice2Result = 0;
        totalBet = 0;
        totalBetText.text = totalBet.ToString();
        ClearChips();
        down7Win.SetActive(false);
        on7Win.SetActive(false);
        up7Win.SetActive(false);
        WinAfterRoundChange();
    }
    
    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);
    }
    

    public void MoveChips(List<GameObject> chips, Transform winArea, float minX, float maxX, float minY, float maxY, float animationSpeed, List<GameObject> addChips)
    {
        for (int i = 0; i < chips.Count; i++)
        {
            int no = i;
            Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
            chips[no].transform.DOScale(Vector3.zero, animationSpeed);
            chips[no].transform.DOMove(dealer.transform.position, animationSpeed).OnComplete(() =>
            {
                chips[no].transform.SetParent(winArea);
                chips[no].transform.DOLocalMove(randomPosition, animationSpeed).OnComplete(() =>
                {

                    //chips.RemoveAt(no);

                });
                chips[no].transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), animationSpeed);
                chips[no].transform.DOScale(Vector3.one, animationSpeed);
                addChips.Add(chips[no]);
            });
        }

    }

    #endregion

    #region Admin Maintain


    public void SetRoomData(int dice1, int dice2)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("Dice1", dice1);
        obj.AddField("Dice2", dice2);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 13);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);   
    }

    public void SetDiceData(int dice1, int dice2)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("Dice1", dice1);
        obj.AddField("Dice2", dice2);
        obj.AddField("room", TestSocketIO.Instace.roomid);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        print("Sending new dice data");
        TestSocketIO.Instace.Senddata("SendDiceData", obj);
    }

    public void GetRoomData(int dice1, int dice2)
    {
        dice1Result = dice1;
        dice2Result = dice2;

        if (isAdmin)
            return;
        StartCoroutine(StartBet());
        SetUpPlayers();
        if (waitNextRoundScreenObject.activeSelf)
            waitNextRoundScreenObject.SetActive(false);
    }
    private void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
            isAdmin = true;
    }

    public void ChangeAdmin(string leftPlayerID)
    {
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObject.activeSelf == true)
            {
                StartCoroutine(StartBet());
                SetUpPlayers();
                waitNextRoundScreenObject.SetActive(false);
            }
        }
        else
            isAdmin = false;
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        System.Random rand = new System.Random();
        var shuffledAvatars = avatars.OrderBy(_ => rand.Next()).ToList();
        int[] randomAvatars = shuffledAvatars.Take(playerList.Count).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        var shuffledName = names.OrderBy(_ => rand.Next()).ToList();
        int[] randomNames = shuffledName.Take(playerList.Count).ToArray();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerId.Equals(leftPlayerID))
            {
                playerList[i].isPlayer = false;
                playerList[i].avatarURL = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                playerList[i].userName.text = BotManager.Instance.botUserName[randomNames[i]];
                StartCoroutine(DataManager.Instance.GetImages(playerList[i].avatarURL, playerList[i].avatarImage));
                break;
            }
        }

        SetUpPlayers();
    }

    #endregion

    #region Socket Events

    public void GetDiceData(int dice1, int dice2)
    {
        if (isAdmin)
            return;
        dice1Result = dice1;
        dice2Result = dice2;
        print("new dice results received from admin");
        print("dice1 = " + dice1Result + " dice2 = " + dice2Result);
    }

    public void SendSevenUpDownBet(int area, int chipNo)// area = 7Up || 7Down || 7
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("roomId", TestSocketIO.Instace.roomid);
        obj.AddField("area", area);
        obj.AddField("chipNo", chipNo);
        TestSocketIO.Instace.Senddata("SendSevenUpDownBet", obj);
    }

    public void AddWinData(string winListData, string diceListData)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("room", TestSocketIO.Instace.roomid);
        obj.AddField("WinList", winListData);
        obj.AddField("DiceList", diceListData);

        TestSocketIO.Instace.Senddata("SetWinDataForSevenUpDown", obj);
    }

    public void GetSevenUpDownBet(int area, int chipNo, string playerID)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerId.Equals(playerID))
            {
                otherProfile= playerList[i].transform;
                break;
            }
        }
        SevenUpDownBet(area, chipNo, otherProfile);

        

    }

    private void SevenUpDownBet(int area, int chipNo, Transform profile)
    {
        if (area == 1)
        {
            Vector3 randomPosition = new Vector3(Random.Range(min7Downx, max7Downx),
                Random.Range(min7Downy, max7Downy));
            GameObject chip = Instantiate(chipPrefab, downArea);
            chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
            chip.transform.position = profile.transform.position;//pending
            downChips.Add(chip);
            ChipGenerate(chip, randomPosition);
        }
        else if (area == 2)
        {
            Vector3 randomPosition = new Vector3(Random.Range(min7Upx, max7Upx), Random.Range(min7Upy, max7Upy));
            GameObject chip = Instantiate(chipPrefab, upArea);
            chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
            chip.transform.position = profile.transform.position;//pending
            upChips.Add(chip);
            ChipGenerate(chip, randomPosition);
        }
        else if (area == 3)
        {
            Vector3 randomPosition = new Vector3(Random.Range(min7Onx, max7Onx), Random.Range(min7Ony, max7Ony));
            GameObject chip = Instantiate(chipPrefab, onArea);
            chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
            chip.transform.position = profile.transform.position;//pending
            onChips.Add(chip);
            ChipGenerate(chip, randomPosition);
        }
    }

    

    #endregion

    #region Round Maintain

    public IEnumerator StartBet()
    {
        isEnterBetStop = true;
        _isClickAvailable = true;
        if(isAdmin)
        {
            dice1Result = Random.Range(1, 7);
            dice2Result = Random.Range(1, 7);
            SetRoomData(dice1Result, dice2Result);
            //SetDiceData(dice1Result, dice2Result);//testing to see if it works without this function call
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        startBetObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Vector3 customZoomScale = new Vector3(4.0f, 4.0f, 4.0f);
        StartAnimationPlay(objects, customZoomScale, 0.1f, 0.009f);
        yield return new WaitForSeconds(1.35f);
        startBetObj.SetActive(false);
        betAnimationONOff(true);
        RestartTimer();
    }

    public IEnumerator StopBet()
    {
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        _isClickAvailable = false;
        isEnterBetStop = true;
        stopBetObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Vector3 customZoomScale = new Vector3(3.0f, 3.0f, 3.0f);
        StartAnimationPlay(stopObjects, customZoomScale, 0.1f, 0.1f);
        SevenUpDownAIManager.Instance.isActive = false;
        if (isAdmin)
        {
            //if (upBetValue + downBetValue + onBetValue > 0)
            //{
                dice1Result = Random.Range(1, 7);
                dice2Result = Random.Range(1, 7);
                SetDiceData(dice1Result, dice2Result);
            //}
        }
        yield return new WaitForSeconds(1.5f);
        stopBetObj.SetActive(false);
        betAnimationONOff(false);
        RollDice();
    }
    public List<GameObject> objects;  // List of objects to animate
    public List<GameObject> stopObjects;

    public void StartAnimationPlay(List<GameObject> objects, Vector3 zoomScale, float zoomDuration, float delayBetweenAnimations)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (GameObject obj in objects)
        {
            sequence.AppendCallback(() => obj.SetActive(true));
            sequence.Append(obj.transform.DOScale(zoomScale, zoomDuration).SetEase(Ease.OutQuad));
            sequence.Append(obj.transform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.OutQuad));
            sequence.AppendInterval(delayBetweenAnimations);
        }

        // Play the sequence
        sequence.Play();
    }
    public void betAnimationONOff(bool isStart)
    {
        if (isStart)
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }

        }
        else
        {
            for (int i = 0; i < stopObjects.Count; i++)
            {
                stopObjects[i].SetActive(false);
            }
        }
    }
    public void RestartTimer()
    {
        
        downBetValue = 0;
        upBetValue = 0;
        onBetValue = 0;

        timerValue = fixTimerValue;
        secondsCount = timerValue;
        timerText.text = timerValue.ToString();
        isEnterBetStop = false;
        SevenUpDownAIManager.Instance.isActive = true;
    }

    void WinAfterRoundChange()//called after round is over
    {

        if (isAdmin)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 1 || isAdmin)
            {
                StartCoroutine(StartBet());
                SetUpPlayers();
            }
            else
                SetUpPlayers();
            
        }
    }

    public void SetUpPlayers()
    {
        int counter = 0;
        playerName.text = DataManager.Instance.playerData.firstName;
        UpdateBalance();
        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), playerAvatar));
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL") , playerAvatar);
        for (int i = 0; i < playerList.Count; i++)
        {
            if (counter < DataManager.Instance.joinPlayerDatas.Count)
            {
                if (DataManager.Instance.joinPlayerDatas[counter].userId.Equals(DataManager.Instance.playerData._id))
                    counter++;
            }
            if (counter < DataManager.Instance.joinPlayerDatas.Count && DataManager.Instance.joinPlayerDatas.Count > 1)
            {
                playerList[i].gameObject.SetActive(true);
                playerList[i].isPlayer = true;
                playerList[i].playerId = DataManager.Instance.joinPlayerDatas[counter].userId;
                playerList[i].userName.text = DataManager.Instance.joinPlayerDatas[counter].userName;
                playerList[i].avatarURL = DataManager.Instance.joinPlayerDatas[counter].avtar;
                StartCoroutine(DataManager.Instance.GetImages(playerList[i].avatarURL, playerList[i].avatarImage));
            }
            else
            {
                playerList[i].playerId = "Bot" + i.ToString();
                playerList[i].isPlayer = false;
                //print("Player not present/left, adding bot");
            }
            counter++;
        }
        foreach (var item in playerList)
        {
            if (item.gameObject.activeInHierarchy == true && item.isPlayer == false)
                return;
        }
        GetSetBotPlayer();
    }

    #endregion

    #region ChipsControl

    public void ChipGenerate(GameObject chip, Vector3 endPosition)
    {
        chip.transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), 0.2f);
        chip.transform.DOLocalMove(endPosition, 0.2f).OnComplete(() =>
        {
            chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
            {
                chip.transform.DOScale(Vector3.one, 0.07f);
            });
        });
    }

    public void ClearChips()
    {
        for (int i = 0; i < upChips.Count; i++)
        {
            Destroy(upChips[i]);
        }
        for (int i = 0; i < downChips.Count; i++)
        {
            Destroy(downChips[i]);
        }
        for (int i = 0; i < onChips.Count; i++)
        {
            Destroy(onChips[i]);
        }
        upChips.Clear();
        downChips.Clear();
        onChips.Clear();

    }

    #endregion

    #region Player/Bot History

    public void GetSetBotPlayer()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        System.Random rand = new System.Random();
        var shuffledAvatars = avatars.OrderBy(_ => rand.Next()).ToList();
        int[] randomAvatars = shuffledAvatars.Take(playerList.Count).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        var shuffledName = names.OrderBy(_ => rand.Next()).ToList();
        int[] randomNames = shuffledName.Take(playerList.Count).ToArray();

        for (int i = 0;i < playerList.Count; i++)
        {
            //item.
            //history.avatar = BotManager.Instance.botUser_Profile_URL[randomAvatars]
            if (playerList[i].isPlayer == false)
            {
                playerList[i].gameObject.SetActive(true);
                //playerList[i].playerId = "Bot" + i.ToString();
                playerList[i].avatarURL = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                playerList[i].userName.text = BotManager.Instance.botUserName[randomNames[i]];
                StartCoroutine(DataManager.Instance.GetImages(playerList[i].avatarURL, playerList[i].avatarImage));
            }
            else
                continue;
        }
    }

    #endregion

    #region Error Screen

    public void OpenErrorScreen()
    {
        errorScreenObject.SetActive(true);
    }

    public void Error_OKButtonClick()
    {
        SoundManager.Instance.ButtonClick();
    }

    #endregion

    #region Menu Screen

    public void LobbyButtonClick()
    {
        TestSocketIO.Instace.LeaveRoom();
        SoundManager.Instance.StartBackgroundMusic();
        SceneManager.LoadScene("Main");
    }

    private void OnApplicationQuit()
    {
        TestSocketIO.Instace.LeaveRoom();
    }

    #endregion

    public void HistoryTracker(int win, int diceResult)//1 = 7Down //2 = 7Up //3 = 7
    {
        if (historyHolder.childCount == maxWinListCount)
            Destroy(historyHolder.GetChild(0).gameObject);
        var chip = Instantiate(resultPrefab, historyHolder);
        chip.GetComponent<ResultPrefab7>().ChipDetails(win, diceResult);
    }

    private void UpdateHistoryChips(int winNo, int diceResult)
    {
        if (winList.Count >= maxWinListCount)
            winList.RemoveAt(0);
        if (diceResultList.Count >= maxWinListCount)
            diceResultList.RemoveAt(0);
        winList.Add(winNo);
        diceResultList.Add(diceResult);



        HistoryTracker(winNo, diceResult);


        if (!isAdmin)
            return;
        DataManager.Instance.sevenUpDownWinHistory = string.Join(",", winList.Select(x => x.ToString()).ToArray());
        DataManager.Instance.sevenUpDownDiceHistory = string.Join(",", diceResultList.Select(x => x.ToString()).ToArray());

        AddWinData(DataManager.Instance.sevenUpDownWinHistory, DataManager.Instance.sevenUpDownDiceHistory);
    }

    private void LoadHistoryChips()
    {
        string listString = PlayerPrefs.GetString(playerPrefsKey, "");
        if (listString != "")
            winList = new List<int>(listString.Split(',').Select(x => int.Parse(x)));
        else
            return;
    }

    public void GetUpdatedHistory(string winData, string diceWinData)
    {
        if (isAdmin)
            return;
        if (winData != "")
            winList = new List<int>(winData.Split(',').Select(x => int.Parse(x)));
        if (diceWinData != "")
            diceResultList = new List<int>(diceWinData.Split(',').Select(x => int.Parse(x)));
        else
            return;

        //HistoryTracker(item);
        

        for (int i = 0; i < historyHolder.childCount; i++)
        {
            Destroy(historyHolder.GetChild(i).gameObject);
        }
        if (historyHolder.childCount == maxWinListCount)
            Destroy(historyHolder.GetChild(0).gameObject);
        for (int i = 0; i < winList.Count; i++)
        {
            HistoryTracker(winList[i], diceResultList[i]);
        }
        
    }

    public void HistoryLoader(string data, string diceData)
    {
        if(diceData != "")
            diceResultList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        if (data != "")
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        else
            return;

        for (int i = 0; i < winList.Count; i++)
        {
            HistoryTracker(winList[i], diceResultList[i]);
        }
    }

    #region Sounds

    private void CheckSound()
    {
        soundImg.sprite = DataManager.Instance.GetSound() == 0 ? soundonSprite : soundoffSprite;
        vibrationImg.sprite = DataManager.Instance.GetVibration() == 0 ? vibrationonSprite : vibrationoffSprite;
        musicImg.sprite = DataManager.Instance.GetMusic() == 0 ? musiconSprite : musicoffSprite;
    }

    public void SoundButtonClick()
    {
        if (soundImg.sprite == soundonSprite)
        {
            DataManager.Instance.SetSound(1);
            soundImg.sprite = soundoffSprite;
        }
        else if (soundImg.sprite == soundoffSprite)
        {
            DataManager.Instance.SetSound(0);
            soundImg.sprite = soundonSprite;
        }
    }
    

    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibrationImg.sprite == vibrationonSprite)
        {
            DataManager.Instance.SetVibration(1);
            vibrationImg.sprite = vibrationoffSprite;
        }
        else if (vibrationImg.sprite == vibrationoffSprite)
        {
            DataManager.Instance.SetVibration(0);
            vibrationImg.sprite = vibrationonSprite;
        }
    }
    
    public void MusicButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (musicImg.sprite == musiconSprite)
        {
            DataManager.Instance.SetMusic(1);
            musicImg.sprite = musicoffSprite;
            SoundManager.Instance.StartBackgroundMusic();
        }
        else if (musicImg.sprite == musicoffSprite)
        {
            DataManager.Instance.SetMusic(0);
            musicImg.sprite = musiconSprite;
            SoundManager.Instance.StartBackgroundMusic();
            SoundManager.Instance.ButtonClick();
        }
    }


    #endregion
}


