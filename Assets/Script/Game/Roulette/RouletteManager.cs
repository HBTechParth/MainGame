using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using Random = System.Random;
using Random = UnityEngine.Random;


[System.Serializable]
public class ChipPlaceClass
{
    public int chipNo;
    public List<int> avaliableListNo = new List<int>();
}

[System.Serializable]
public class RouleteeBetClass
{
    public int placeNo;
    public int betImgNo;
    public int betTotalAmount;
    public List<int> betPlacesNo;
    public GameObject chipObj;
    public RouletteButtonData rouletteButtonData;
    public List<GameObject> blackPanelList = new List<GameObject>();
}

public class RouletteManager : MonoBehaviour
{

    public static RouletteManager Instance;

    public Image avatarImg;

    [Header("---Chips Manage---")]
    public Image chipImg;
    public Sprite[] chipSprite;
    public Button plusBtn;
    public Button minusBtn;
    public int betChipNo;
    public GameObject[] chipBtn;
    public float[] betChipPrices;
    public float downValue;
    public float upValue;

    public List<int> noList = new List<int>();

    public int noGen;
    public List<GameObject> triggerObj = new List<GameObject>();
    public GameObject findTriggerObj;

    public bool isRoundOn;

    public GameObject startBettingObj;
    public Animator startBetAnim;

    public GameObject stopBettingObj;
    public Animator stopBetAnim;
    //FirstClipComplete

    [Header("--- Info Screen ---")]
    public GameObject infoScreenObj;
    public GameObject[] innerInfoObj;
    public Button leftButton;
    public Button rightButton;
    public Button clearButton;
    public Button rebetButton;
    public Button undoButton;

    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;

    [Header("--- Wait New Round Screen ---")]
    public GameObject waitNextRoundScreenObj;

    [Header("--- Error Screen ---")]
    public GameObject errorScreenObj;

    [Header("--- Shop Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    [Header("--- Game UI ---")]
    public Text balanceTxt;
    public Text userNameTxt;
    public float totalBetPrice;
    public Text userBetTxt;
    
    [Header("--- Recorder ---")]
    public RectTransform content;
    public GameObject PounCarrier;
    public List<GameObject> Pouns;
    public List<int> winList;
    private float _transformPosition = 175.3074f;

    [Header("---Fake Betting Coins---")]
    public RouletBotPlayers player1;
    public RouletBotPlayers player2;
    public RouletBotPlayers player3;
    public RouletBotPlayers player4;
    public List<RouletBotPlayers> botPlayersList;
    public List<BotPlayersData> BotPlayersDatas;
    public List<GameObject> Objects;
    public List<GameObject> betCoins;
    public List<Button> spawnLocations;


    [Header("---Game Use---")]

    public List<int> a12_1;
    public List<int> a12_2;
    public List<int> a12_3;

    public List<int> b12_1;
    public List<int> b12_2;
    public List<int> b12_3;

    public List<int> start18;
    public List<int> last18;

    public List<int> evenNo;
    public List<int> oddNo;

    public List<int> redNo;
    public List<int> blackNo;

    [Header("--- Bet Manage  & Game Play ---")]
    public Sprite socketChip;
    public GameObject rouletteBoardObj;
    public Image[] greenLineBoard;
    public List<GameObject> blackPanelObj;
    public List<GameObject> btnParentPanelObj;
    public List<RouletteButtonData> btnPanelObj;
    public GameObject chipPrefab;
    public GameObject chipAnimParent;
    public GameObject cancelPrefab;
    public int[] chipsValue;
    public List<RouleteeBetClass> rouleteeBets = new List<RouleteeBetClass>();
    public List<RouleteeBetClass> rouleteeBetsBefore = new List<RouleteeBetClass>();
    public List<int> undoBlockList = new List<int>();
    public Color colorOnGreen;
    public Color colorOffGreen;
    public WheelRoulette wheelRoulette;
    public BallRoulette ballRoulette;
    public GameObject ballStartPos;
    public Text timerTxt;
    public bool isGameRouletteStart;
    public bool isAveUndo;
    public int findNo;
    public float fixTimeSet;
    float secondCount = 0;
    public int timerValue = 0;

    public bool isEnterTheRoulette = false;
    public bool isGameRunning;
    public bool isAdmin;
    public bool isStopBet;

    public float totalCurrentInvest = 0;
    float interval = 0.5f; 
    float nextTime = 0;
    public bool isActive;
    private bool called = false;
    private bool _isTimeSet;
    
    [Header("--- Sounds ---")]
    public Image soundImg;
    public Image musicImg;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateNameBalance();
        isActive = false;
    }


    public void ReGenerateBoard()
    {
        for (int i = 0; i < triggerObj.Count; i++)
        {
            triggerObj[i].transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            triggerObj[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        findNo = 0;
        for (int i = 0; i < noList.Count; i++)
        {
            if (noList[i] == noGen)
            {
                findNo = i;
                break;
            }
        }
        findTriggerObj = triggerObj[findNo];
        wheelRoulette.transform.rotation = Quaternion.Euler(Vector3.zero);
        //ballRoulette.transform.position = ballStartPos.transform.position;
        wheelRoulette.UpdateWheel();
        wheelRoulette.speed = 13;
        wheelRoulette.collider.radius = 308.7f;

        for (int i = 0; i < greenLineBoard.Length; i++)
        {
            greenLineBoard[i].color = colorOffGreen;
        }
        ballRoulette.UpdateBallSecond();
        isRoundOn = false;
        isGameRouletteStart = true;
        
    }

    public void PounRecorder()
    {
        //Instantiate(Pouns[noGen], PounCarrier.transform);
        UpdateHistoryRecord(noGen);
    }

    public void UpdateBetPrice()
    {
        float currentBet = GetPrice(betChipNo);
        totalBetPrice += currentBet;
        userBetTxt.text = totalBetPrice.ToString(CultureInfo.InvariantCulture);
    }



    public void UpdateNameBalance()
    {
        userNameTxt.text = DataManager.Instance.playerData.firstName.ToString();
        balanceTxt.text ="₹ " + DataManager.Instance.playerData.balance.ToString();
    }

    public void ObjectAvaliable()
    {
        //print(findTriggerObj.gameObject.name);
        findTriggerObj.transform.GetChild(0).gameObject.SetActive(true);
    }

    bool isStart = false;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopBackgroundMusic();
        betChipNo = 0;
        //CreateAdmin();
        //ChipUpdateUI();
        clearButton.interactable = false;
        undoButton.interactable = false;

        if (Instance == null)
        {
            //isStart = true;
        }
        rebetButton.interactable = false;
        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL") , avatarImg);
        GetBotPlayers();
        SetBotPlayer();
        NewPlayerEnter();
        HistoryLoader(DataManager.Instance.winRecord);
        ChipAnimMaintain(1);
        CheckSound();

    }

    #region New Player Entry Time Maintain

    public void NewPlayerEnter()
    {
        print("______________________New player entered is called_________________________________");
        if (DataManager.Instance.joinPlayerDatas.Count < TestSocketIO.Instace.rouletteRequirePlayer) return;
        CreateAdmin();
        if (DataManager.Instance.joinPlayerDatas.Count == 1 && isAdmin)
        {
            isEnterTheRoulette = true;
            //RestartTimer();
            StartCoroutine(StartBettingOff());
        }
        else
        {
            if (isAdmin) return;

            if (isGameRunning) return;
            isEnterTheRoulette = true;
            print("Enter The New Player Time");
            waitNextRoundScreenObj.SetActive(true);

            //FindDataAdminRouletee();
            //Get Details Admin
        }
    }
    #endregion


    public void GiveUserData()
    {
        print("______________________New game is called_________________________________");
        //RestartTimer();
        StartCoroutine(StartBettingOff());
    }

    IEnumerator StartBettingOff()
    {
        print("______________________start betting is called_________________________________");
        isGameRunning = true;
        if (isAdmin)
        {
            DataManager.Instance.rouletteGameStatus = true;
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        SoundManager.Instance.CasinoTurnSound();
        isStopBet = true;
        for (int i = 0; i < blackPanelObj.Count; i++)
        {
            blackPanelObj[i].SetActive(false);
        }
        for (int i = 0; i < btnParentPanelObj.Count; i++)
        {
            for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
            {
                Destroy(btnParentPanelObj[i].transform.GetChild(j).transform.gameObject);
            }
        }
        startBettingObj.SetActive(true);
        TurnOnButtons();
        yield return new WaitForSeconds(2.5f);

        rouleteeBetsBefore.Clear();

        
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            RouleteeBetClass rouleteeBet = rouleteeBets[i];
            rouleteeBetsBefore.Add(rouleteeBet);
        }
        
        if (rouleteeBetsBefore.Count > 0)
        {
            rebetButton.interactable = true;
        }
        rouleteeBets.Clear();
        startBetAnim.SetInteger("FirstClipComplete", 1);
        yield return new WaitForSeconds(1f);
        startBettingObj.SetActive(false);
        isActive = true;
        RestartTimer();
    }

    IEnumerator StopBettingOff()
    {
        print("______________________Stop betting is called_________________________________");
        isActive = false;
        if (isAdmin)
        {
            DataManager.Instance.rouletteGameStatus = false;
            TestSocketIO.Instace.GetBetData();
        }
        stopBettingObj.SetActive(true);
        ClearAllChips();
        TurnOffButtons();
        yield return new WaitForSeconds(3f);
        stopBetAnim.SetInteger("FirstClipComplete", 0);
        yield return new WaitForSeconds(1f);
        stopBettingObj.SetActive(false);
        ReGenerateBoard();
        rouletteBoardObj.SetActive(true);
    }

    public void ClearAllChips()
    {
        // for (int i = 0; i < botPlayersList.Count; i++)
        // {
        //     for (int j = botPlayersList[i].transform.childCount - 1; i >= 2; i--)
        //     {
        //         Destroy(botPlayersList[i].transform.GetChild(j).gameObject);
        //     }
        // }
        foreach (var chips in Objects)
        {
            Destroy(chips);
        }
        Objects.Clear();
        totalBetPrice = 0;
        userBetTxt.text = totalBetPrice.ToString(CultureInfo.InvariantCulture);
        clearButton.interactable = false;
        rebetButton.interactable = false;
        undoButton.interactable = false;
    }

    public void RestartTimer()
    {
        //for (int i = 0; i < blackPanelObj.Count; i++)
        //{
        //    blackPanelObj[i].SetActive(false);
        //}
        //for (int i = 0; i < btnParentPanelObj.Count; i++)
        //{
        //    for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
        //    {
        //        Destroy(btnParentPanelObj[i].transform.GetChild(j));
        //    }
        //}
        
        isStopBet = false;
        isEnterTheRoulette = false;
        timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        secondCount = fixTimeSet;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(timerValue == 0 && isEnterTheRoulette == false && waitNextRoundScreenObj.activeSelf == false)
        {
            isEnterTheRoulette = true;
            _isTimeSet = false;
            StartCoroutine(StopBettingOff());
        }
        else if (!timerTxt.text.Equals("0"))
        {
            secondCount -= Time.deltaTime;
            timerValue = ((int)secondCount);
            timerTxt.text = timerValue.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ReGenerateBoard();
        //    rouletteBoardObj.SetActive(true);
        //}

        if (isAdmin && !_isTimeSet)
        {
            if (timerValue == 5)
            {
                AdjustTime();
                _isTimeSet = true;
                print("---------------------------Time is sent-------------------------");
            }
        }
        if (!isActive) return;
        if (Time.time >= nextTime)
        {
            CallBetCoins();
            nextTime += interval;
        }
    }
    
    #region Menu Screen

    public void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    public void HomeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        TestSocketIO.Instace.LeaveRoom();
        SoundManager.Instance.StartBackgroundMusic();
        SceneManager.LoadScene("Main");
    }

    public void InfoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenInfoScreenObj();
    }

    public void StoreButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Instantiate(shopPrefab, shopPrefabParent.transform);
    }
    public void CloseMenuScreen()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
    }


    #endregion

    #region Error Screen
    public void OpenErrorScreen()
    {

        errorScreenObj.SetActive(true);
    }

    public void Error_Ok_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        errorScreenObj.SetActive(false);
    }

    public void Error_Shop_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Instantiate(shopPrefab, shopPrefabParent.transform);
        errorScreenObj.SetActive(false);
    }


    #endregion

    #region Info Screen

    int infoScreenNo = 0;
    public void OpenInfoScreenObj()
    {
        infoScreenObj.SetActive(true);
        infoScreenNo = 0;
        UpdateInfo();
    }

    public void LeftButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenNo--;
        UpdateInfo();
    }

    public void RightButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenNo++;
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (infoScreenNo == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
        else if (infoScreenNo == innerInfoObj.Length - 1)
        {
            leftButton.interactable = true;
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }

        for (int i = 0; i < innerInfoObj.Length; i++)
        {
            if (i == infoScreenNo)
            {
                innerInfoObj[i].SetActive(true);
            }
            else
            {
                innerInfoObj[i].SetActive(false);
            }
        }


    }


    public void BackToInfoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenObj.SetActive(false);
    }

    #endregion

    #region Button Click

    public void SpinButtonClick()
    {

    }

    public void UndoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (rouleteeBets.Count != 0)
        {
            //isAveUndo = true;


            UndoManage();
        }
    }
    public void RebeatButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (rouleteeBetsBefore.Count > 0)
        {
            OnRebet();
        }
    }

    public void ClearButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        ClearFunction();
    }

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    public void MinButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        betChipNo--;
        ChipUpdateUI();
    }

    public void MaxButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        betChipNo++;
        ChipUpdateUI();
    }


    #endregion

    #region Rebet

    void OnRebet()
    {
        if (rouleteeBets.Count != 0)
        {
            ClearFunction();
        }

        foreach (var t in rouleteeBetsBefore)
        {
            t.rouletteButtonData.SimpleRebetGenerate(t);
        }
        rouleteeBetsBefore.Clear();
        if (rouleteeBetsBefore.Count == 0)
        {
            rebetButton.interactable = false;
        }
    }

    #endregion

    #region Other
    
    public void ChipButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        ChipAnimMaintain(no);
    }

    void ChipAnimMaintain(int no)
    {
        betChipNo = no;
        for (int i = 0; i < chipBtn.Length; i++)
        {
            chipBtn[i].transform.DOMoveY(i == no ? upValue : downValue, 0.05f);
        }
        //chipImg.sprite = chipSprite[betChipNo];
    }


    void ChipUpdateUI()
    {
        if (betChipNo == 0)
        {
            minusBtn.interactable = false;
            plusBtn.interactable = true;
        }
        else if (betChipNo == chipSprite.Length - 1)
        {
            minusBtn.interactable = true;
            plusBtn.interactable = false;
        }
        else
        {
            minusBtn.interactable = true;
            plusBtn.interactable = true;
        }

        chipImg.sprite = chipSprite[betChipNo];
    }

    public void CallBetCoins()
    {
        SoundManager.Instance.ThreeBetSound();
        int spawnCoin = Random.Range(0, betCoins.Count);
        int spawnPlayer = Random.Range(0, botPlayersList.Count);
        GameObject chipGen = Instantiate(betCoins[spawnCoin], botPlayersList[spawnPlayer].transform);
        int spawnLocation = Random.Range(0, spawnLocations.Count);
        Vector3 endChip = spawnLocations[spawnLocation].transform.position;
        //chipGen.transform.position = spawnLocations[spawnLocation].transform.position;
        Objects.Add(chipGen);
        ChipGenerate(chipGen, endChip);
        Destroy(chipGen, 1f);
    }
    
    public void ChipGenerate(GameObject chip, Vector3 endPos)
    {
        chip.transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), 0.2f);
        chip.transform.DOMove(endPos, 0.2f).OnComplete(() =>
        {
           // chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
           // {
           //     chip.transform.DOScale(Vector3.one, 0.07f);
            //});
        });
    }

    private void GetBotPlayers()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int[] randomAvatars = avatars.Take(botPlayersList.Count).ToArray();
        
        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int[] randomNames = names.Take(botPlayersList.Count).ToArray();
        
        for (int i = 0; i < botPlayersList.Count; i++)
        {
            BotPlayersData t = new BotPlayersData();
            t.avatar = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
            t.name =  BotManager.Instance.botUserName[randomNames[i]];
            BotPlayersDatas.Add(t);
        }
    }

    private void SetBotPlayer()
    {
        for (int i = 0; i < botPlayersList.Count; i++)
        {
            botPlayersList[i].playerNameTxt.text = BotPlayersDatas[i].name;
            botPlayersList[i].avatar = BotPlayersDatas[i].avatar;
            botPlayersList[i].GetPlayerImage();
        }
    }

    private void TurnOffButtons()
    {
        print("_______________________Buttons are Off__________________________");
        int buttonCount = spawnLocations.Count;
        for (int i = 0; i < buttonCount; i++)
        {
            spawnLocations[i].interactable = false;
        }
    }
    
    private void TurnOnButtons()
    {
        print("_______________________Buttons are On__________________________");
        int buttonCount = spawnLocations.Count;
        for (int i = 0; i < buttonCount; i++)
        {
            spawnLocations[i].interactable = true;
        }

        //clearButton.interactable = true;
        //rebetButton.interactable = true;
        //undoButton.interactable = true;
    }

    public void ActivateButtons()
    {
        clearButton.interactable = rouleteeBets.Count > 0;
        undoButton.interactable = rouleteeBets.Count > 0;
    }

    #endregion



    #region Undo Manage
    void ClearFunction()
    {
        int betAmount = 0;
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            for (int j = 0; j < rouleteeBets[i].blackPanelList.Count; j++)
            {
                rouleteeBets[i].blackPanelList[j].SetActive(false);
            }
            GameObject chipObj = rouleteeBets[i].chipObj;
            betAmount += rouleteeBets[i].betTotalAmount;
            chipObj.transform.DOMove(chipAnimParent.transform.position, 0.2f).OnComplete(() =>
            {
                
            });
        }
        print("This is the bet amount -> " + betAmount);
        if (betAmount != 0 && betAmount > 0) // add bit amount and which number in note
        {
            DataManager.Instance.ReverseAmount(betAmount, DataManager.Instance.gameId, "Roulette-return-" + DataManager.Instance.gameId, "reverse", betChipNo);
            totalBetPrice -= betAmount;
            userBetTxt.text = totalBetPrice.ToString(CultureInfo.InvariantCulture);
        }
        

        rouleteeBets.Clear();
        ActivateButtons();
    }

    public void ClearSpecificNo(int no)
    {
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            if (rouleteeBets[i].placeNo == no)
            {
                for (int j = 0; j < rouleteeBets[i].blackPanelList.Count; j++)
                {
                    rouleteeBets[i].blackPanelList[j].SetActive(false);
                }
                GameObject chipObj = rouleteeBets[i].chipObj;
                chipObj.transform.DOMove(chipAnimParent.transform.position, 0.2f).OnComplete(() =>
                {


                });

                rouleteeBets.Remove(rouleteeBets[i]);
            }

        }

        //rouleteeBets.Remove(no);

    }

    void UndoManage()
    {
        /*if (isAveUndo == false)
        {
            isAveUndo = true;
            for (int i = 0; i < rouleteeBets.Count; i++)
            {
                GameObject chipObj = rouleteeBets[i].chipObj;
                GameObject closeGenObj = Instantiate(cancelPrefab, chipObj.transform);
                closeGenObj.name = "CloseChip";
                undoBlockList.Add(rouleteeBets[i].placeNo);
            }
        }
        else
        {
            for (int i = 0; i < rouleteeBets.Count; i++)
            {
                GameObject chipObj = rouleteeBets[i].chipObj;
                for (int j = 0; j < chipObj.transform.childCount; j++)
                {
                    GameObject closeObj = chipObj.transform.GetChild(j).gameObject;
                    if (closeObj.name == "CloseChip")
                    {
                        Destroy(closeObj);
                        isAveUndo = false;
                    }
                }
                undoBlockList.Clear();
            }
        }*/
        
        int betAmount = 0;
        int chipNo = 0;
        List<int> betPlace = new List<int>();

        if (rouleteeBets.Count > 0)
        {
            int indexNum = rouleteeBets.Count - 1;
            for (int j = 0; j < rouleteeBets[indexNum].blackPanelList.Count; j++)
            {
                rouleteeBets[indexNum].blackPanelList[j].SetActive(false);
            }

            GameObject chipObj = rouleteeBets[indexNum].chipObj;
            betAmount = rouleteeBets[indexNum].betTotalAmount;
            chipNo = rouleteeBets[indexNum].placeNo;
            betPlace = rouleteeBets[indexNum].betPlacesNo;

        
            print("This is the bet amount -> " + betAmount);
            print("This is the Chip no --> " + chipNo);

            SendReverseNo(chipNo, betAmount.ToString(), betPlace);

            if (betAmount != 0 && betAmount > 0)
            {
                DataManager.Instance.ReverseAmount(betAmount, DataManager.Instance.gameId, "Roulette-return-" + DataManager.Instance.gameId, "reverse", betChipNo);
                totalBetPrice -= betAmount;
                userBetTxt.text = totalBetPrice.ToString(CultureInfo.InvariantCulture);
            }

            rouleteeBets.RemoveAt(indexNum);
            chipObj.transform.DOMove(chipAnimParent.transform.position, 0.2f).OnComplete(() =>
            {
                // Any additional completion logic here
                Destroy(chipObj);
            });
        
            ActivateButtons();
        }
    }
    
    public void SendReverseNo(int chipNo, string betAmount, List<int> AvaliableNo)
    {
        string manyBetString = "[" + string.Join(",", AvaliableNo) + "]";
        TestSocketIO.Instace.SetBetData(chipNo, betAmount, manyBetString, "unbet");
    }

    #endregion

    #region Win Rules

    public float WinRuleNo(int sNo)
    {
        int winRuleNo = 0;
        float mulValue = 0;
        if (sNo >= 0 && sNo <= 36)
        {
            winRuleNo = 1;
            mulValue = 36;
        }
        if (sNo >= 71 && sNo <= 127)
        {
            winRuleNo = 2;
            mulValue = 17;
        }
        if (sNo >= 130 && sNo <= 141)
        {
            winRuleNo = 3;
            mulValue = 11;
        }
        if (sNo >= 49 && sNo <= 70)
        {
            winRuleNo = 4;
            mulValue = 8;
        }
        if (sNo == 128 || sNo == 129)
        {
            winRuleNo = 5;
            mulValue = 5;
        }
        if (sNo == 45 || sNo == 46)
        {
            winRuleNo = 6;
            mulValue = 2;
        }
        if (sNo == 44 || sNo == 47)
        {
            winRuleNo = 7;
            mulValue = 2;
        }
        if (sNo == 43 || sNo == 48)
        {
            winRuleNo = 8;
            mulValue = 2;
        }

        if (sNo == 40 || sNo == 41 || sNo == 42)
        {
            winRuleNo = 9;
            mulValue = 4;
        }
        if (sNo == 37 || sNo == 38 || sNo == 39)
        {
            winRuleNo = 9;
            mulValue = 3;
        }
        
        return mulValue;
    }

    public float WinManager()
    {
        float winvalue = 0;
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            if (rouleteeBets[i].rouletteButtonData.btnAvaliableNo.Contains(noGen))
            {
                winvalue += (rouleteeBets[i].betTotalAmount * WinRuleNo(rouleteeBets[i].rouletteButtonData.chipNo));
            }
        }
        return winvalue;
    }

    #endregion


    #region Check Money

    public bool CheckMoney(float money)
    {

        float currentBalance = float.Parse(DataManager.Instance.playerData.balance);
        if ((currentBalance - money) < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        return false;
    }

    public float GetPrice(int no)
    {
        return betChipPrices[no];
    }

    #endregion

    #region Socket Maintain

    public void GetBetSocket(int no)
    {
        for (int i = 0; i < btnPanelObj.Count; i++)
        {
            if (btnPanelObj[i].chipNo == no)
            {
                btnPanelObj[i].Add_Socket_Chip(socketChip);
            }
        }

    }

    #endregion

    #region Admin Maintain

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
            /*SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);*/
        }
    }

    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        //obj.AddField("DeckNo", UnityEngine.Random.Range(0, 37));
       // obj.AddField("DeckNo", 2);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 3);
        obj.AddField("WinList", DataManager.Instance.winRecord);
        obj.AddField("gameStatus", DataManager.Instance.rouletteGameStatus);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int no, string data)
    {
        //noGen = no;
        if (isAdmin) return;
        if (waitNextRoundScreenObj.activeSelf)
        {
            isEnterTheRoulette = true;
            waitNextRoundScreenObj.SetActive(false);
            timerValue = (int)fixTimeSet;
            secondCount = timerValue;
            SoundManager.Instance.CasinoTurnSound();
            DataManager.Instance.UserTurnVibrate();
            StartCoroutine(StartBettingOff());
            //CenterToAddUser();
        }
        
        HistoryLoader(data);
    }

    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {
        //DataManager.Instance.joinPlayerDatas.Clear();
        /*for (int i = 0; i < DataManager.Instance.leaveUpdatePlayerDatas.Count; i++)
        {
            DataManager.Instance.joinPlayerDatas.Add(DataManager.Instance.leaveUpdatePlayerDatas[i]);
        }*/
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObj.activeSelf)
            {
                //RoundGenerate();
                StartCoroutine(StartBettingOff());
                if (waitNextRoundScreenObj.activeSelf)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
            }
        }
        else
        {
            isAdmin = false;
        }


    }
    
    public void HistoryLoader(string data)
    {
        if(data != "")
        {
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        }
        
        int childCount = PounCarrier.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(PounCarrier.transform.GetChild(i).gameObject);
        }
        
        foreach (var t in winList)
        {
            Instantiate(Pouns[t], PounCarrier.transform);
        }

        UpdateScrollRect();
    }

    private void UpdateScrollRect()
    {
        // int lastIndex = PounCarrier.transform.childCount - 1;
        // Vector2 sizeDelta = content.sizeDelta;
        // sizeDelta.y += newObject.GetComponent<RectTransform>().sizeDelta.y;
        // content.sizeDelta = sizeDelta;
        // content.anchoredPosition = new Vector2(0, -sizeDelta.y);
        // setting the scroll view to last
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, _transformPosition);

    }
    
    private void UpdateHistoryRecord(int num)
    {
        if (!isAdmin) return;
        winList.RemoveAt(0);
        winList.Add(num);
        
        int childCount = PounCarrier.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(PounCarrier.transform.GetChild(i).gameObject);
        }
        
        foreach (var t in winList)
        {
            Instantiate(Pouns[t], PounCarrier.transform);
        }
        UpdateScrollRect();
        
        DataManager.Instance.winRecord = string.Join(",", winList.Select(x => x.ToString()).ToArray());
        //SetWinData(DataManager.Instance.winList);
    }


    public void FindDataAdminRouletee()
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("FindDataRouletteAdmin", obj);
    }

    public void SendAdminDataPlayer(string playerID)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("AdminPlayerID", DataManager.Instance.playerData._id);
        obj.AddField("ReceivePlayerID", playerID);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("Time", timerValue);
        obj.AddField("RouletteNumber", noGen);
        TestSocketIO.Instace.Senddata("SendAdminRouleteeData", obj);
    }

    public void AdjustTime()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("Time", timerValue);
        TestSocketIO.Instace.Senddata("SendAdminRouleteeData", obj);
    }

    public void GetAdminDataPlayer(int time, int no)
    {
        //isEnterTheRoulette = true;
       // waitNextRoundScreenObj.SetActive(false);
        timerValue = time;
        secondCount = timerValue;
        //noGen = no;
       // CenterToAddUser();
        //aaa
    }


    void CenterToAddUser()
    {
        isStopBet = true;
        totalCurrentInvest = 0;
        for (int i = 0; i < blackPanelObj.Count; i++)
        {
            blackPanelObj[i].SetActive(false);
        }
        for (int i = 0; i < btnParentPanelObj.Count; i++)
        {
            for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
            {
                Destroy(btnParentPanelObj[i].transform.GetChild(j).transform.gameObject);
            }
        }

        rouleteeBetsBefore.Clear();
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            RouleteeBetClass rouleteeBet = rouleteeBets[i];
            rouleteeBetsBefore.Add(rouleteeBet);
        }
        rouleteeBets.Clear();



        isStopBet = false;
        isEnterTheRoulette = false;
        //timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        //secondCount = fixTimeSet;
    }

    #endregion
    
    #region Sounds

    private void CheckSound()
    {
        soundImg.sprite = DataManager.Instance.GetSound() == 0 ? soundonSprite : soundoffSprite;
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
[System.Serializable]
public class BotPlayersData
{
    public string name;
    public string avatar;
}
//Complete