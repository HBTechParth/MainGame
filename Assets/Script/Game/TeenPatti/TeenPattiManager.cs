using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class TeenPattiWinMaintain
{
    public int ruleNo;
    public List<CardSuffle> winList = new List<CardSuffle>();
}
public class TeenPattiManager : MonoBehaviour
{

    public static TeenPattiManager Instance;

    public GameObject waitNextRoundScreenObj;
    public GameObject playerFindScreenObj;

    [Header("---Game Play---")]
    public int gameDealerNo;
    public bool isWin = false;
    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public List<ListStoreData> listStoreDatas = new List<ListStoreData>();
    public List<int> mainList = new List<int>();
    public List<CardSuffle> cardSufflesGen = new List<CardSuffle>();
    public List<CardSuffle> cardSufflesSort = new List<CardSuffle>();

    public List<CardSuffle> newCardSS = new List<CardSuffle>();
    public List<CardSuffle> newCardSS1 = new List<CardSuffle>();


    public List<TeenPattiPlayer> teenPattiPlayers = new List<TeenPattiPlayer>();
    public List<TeenPattiPlayer> playerSquList = new List<TeenPattiPlayer>();

    public TeenPattiPlayer player1;
    public TeenPattiPlayer player2;
    public TeenPattiPlayer player3;
    public TeenPattiPlayer player4;
    public TeenPattiPlayer player5;

    public Sprite packCardSprite;
    public Sprite simpleCardSprite;
    public float[] chipPrice;
    public Sprite[] chipsSprite;
    public GameObject chipObj;
    public Transform[] playerPosition;
    public List<GameObject> spawnedCoins = new List<GameObject>();
    public BoxCollider2D boxCollider;
    public float minBoardX;
    public float maxBoardX;
    public float minBoardY;
    public float maxBoardY;


    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;

    [Header("--- Rule Screen ---")]
    public GameObject ruleScreenObj;

    [Header("--- Open Message Screen ---")]
    public GameObject messageScreeObj;
    public GameObject giftScreenObj;


    [Header("--- Prefab ---")]
    public GameObject targetBetObj;
    public GameObject betPrefab;
    public GameObject cardTmpPrefab;
    public GameObject prefabParent;
    public GameObject cardTmpStart;
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;


    [Header("--- Chat Panel ---")]
    public GameObject chatPanelParent;
    public GameObject chatMePrefab;
    public GameObject chatOtherPrefab;

    [Header("--- Game UI ---")]
    public GameObject errorScreenObj;
    public GameObject errorScreenObjONBET;
    public GameObject slideShowPanel;
    public Text slideShowName;
    public Image slideShowProfilePic;
    public GameObject skippedChanceObject;
    public Text totalPriceTxt;
    public Button showButton;
    public GameObject bottomBox;
    public GameObject rulesTab;
    public Text rulesText;
    public Text betAmountTxt;
    public Text priceBtnTxt;
    public Text priceBtnTxtDouble;
    public Button plusBtn;
    public Button minusBtn;
    //public GameObject blindx2button;
    public bool isAdmin;
    public int playerNo;
    public int currentPlayer;
    public float timerSpeed;
    public bool isGameStop;
    public int[] numbers = { 5, 10, 20, 50 };
    public int currentPriceIndex = 0;
    public int runningPriceIndex;
    public Image sideShowPopupImage;
    public Text sideShowPopupImageText;
    public float delay;
    private bool isPopupOpen = false;
    public GameObject exitPanel;
    public GameObject entryPopup;
    public Text winAnimationTxt;


    [Header("--- Gift Maintain ---")]
    public GameObject giftParentObj;
    public GameObject giftPrefab;
    public List<GiftBox> giftBoxes = new List<GiftBox>();

    [Header("--- Game Data Maintain ---")]
    public float totalBetAmount;
    public float playerBetAmount;
    public float bootValue;
    public float potLimitValue;
    public float minLimitValue;
    public float doubleLimitValue;
    public float maxLimitValue;
    public float incrementNo;
    public float bonusUseValue;
    float minChaalValue;
    float maxChaalValue;
    float minBlindValue;
    float maxBlindValue;
    public float currentPriceValue;
    public float currentBlindValue;
    public float currentSeenValue;

    public TeenPattiPlayer slideShowPlayer;
    public List<TeenPattiWinMaintain> winMaintain = new List<TeenPattiWinMaintain>();
    public bool isGameStarted;

    public bool isBotActivate;
    public int roundCounter;
    public int boxDisplayCount;

    public int winningBotNo = -1;//the bot which is going to win

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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        //ShowPopUp();
        SoundManager.Instance.StopBackgroundMusic();
        roundCounter = 0;
        boxDisplayCount = 0;
        rulesTab.SetActive(false);

        //Invoke(nameof(CheckWin), 15f);
        //StartGamePlay();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            teenPattiPlayers[i].gameObject.SetActive(false);
        }
        playerFindScreenObj.SetActive(true);
        Debug.Log(" playerFindScreenObj");
        DisplayCurrentBalance();
        PlayerFound();
        CheckSound();

    }

    private void ShowPopUp()
    {
        entryPopup.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseEntryPopup()
    {
        entryPopup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayerFound()
    {
        Debug.Log("joinPlayerDatas  > " + DataManager.Instance.joinPlayerDatas.Count + "     TestSocketIO.Instace.teenPattiRequirePlayer   => " + TestSocketIO.Instace.teenPattiRequirePlayer);
        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.teenPattiRequirePlayer)
        {
            Debug.Log("joinPlayerDatas  > " + DataManager.Instance.joinPlayerDatas.Count + "     TestSocketIO.Instace.teenPattiRequirePlayer   => " + TestSocketIO.Instace.teenPattiRequirePlayer);
            CreateAdmin();
            if (DataManager.Instance.joinPlayerDatas.Count > 5)
            {
                Debug.Log("Play");
                StartCoroutine(WaitGameToComplete(CheckNewPlayers));
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 5 && isAdmin)
            {
                Debug.Log("isGameStarted  =" + isGameStarted);
                if (!isGameStarted)
                {
                    Debug.Log("isGameStarted  =" + isGameStarted);
                    StartGamePlay();
                    Debug.LogError("StartGamePlay");

                }
            }
            else
            {
                Debug.Log("isAdmin  =" + isAdmin);
                if (isAdmin) return;
                Debug.Log("isAdmin  =" + isAdmin);
                Debug.Log("isGameStarted  =" + isGameStarted);
                if (!isGameStarted)
                {
                    Debug.Log("isGameStarted  =" + isGameStarted);
                    waitNextRoundScreenObj.SetActive(true);
                }
            }
        }
        else
        {
            playerFindScreenObj.SetActive(true);
            Debug.Log("playerFindScreenObj");
        }
    }

    private IEnumerator WaitGameToComplete(Action callback)
    {
        yield return new WaitUntil(() => !isGameStarted);
        callback();
    }

    private IEnumerator WaitGameToCompleteRemovePlayer(System.Action<int> callback, int parameter)
    {
        Debug.Log("WaitGameToCompleteRemovePlayer");
        yield return new WaitUntil(() => !isGameStarted);
        callback(parameter);
    }

    public void CheckNewPlayers()
    {
        // removing bot players from list with string common bot string name
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://139.84.132.115/assets/img/profile-picture/")).ToList();
        // assiging new remaining bot players
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
        ResetBot();
        //Activating bots
        ActivateBotPlayers();

        print("_______________________This Function is called ---------------------------------");
    }


    public void CheckLeftPlayer(int index)
    {
        //DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[index]);
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://139.84.132.115/assets/img/profile-picture/")).ToList();
        Debug.Log("Count  =>  " + DataManager.Instance.joinPlayerDatas.Count);
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
        ResetBot();
        //Activating bots
        ActivateBotPlayers();
    }

    public void DisplayCurrentBalance()
    {
        totalPriceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        playerNo = player1.playerNo;
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            // MatchResult();
            BetAnim(teenPattiPlayers[UnityEngine.Random.Range(0, 3)], 0.5f);
        }*/
    }

    #region GamePlay Manager

    public TeenPattiWinMaintain MatchResult(CardSuffle card1, CardSuffle card2, CardSuffle card3)
    {
        TeenPattiWinMaintain teenPattiWinMaintain = new TeenPattiWinMaintain();

        List<CardSuffle> newData = new List<CardSuffle>();
        newData.Add(card1);
        newData.Add(card2);
        newData.Add(card3);

        newCardSS = newData;


        newCardSS1 = NewSort(newData);


        bool isColor = IsColorMatch(newCardSS1);
        List<CardSuffle> threeCards = GetThreeCard(newCardSS1);
        List<CardSuffle> twoCards = GetTwoCard(newCardSS1);
        List<CardSuffle> ronCards = RonValue(newCardSS1);
        List<CardSuffle> highCards = HighCard(newCardSS1);



        //ronCard

        if (threeCards.Count == 3)
        {
            //Three List
            teenPattiWinMaintain.ruleNo = 1;
            teenPattiWinMaintain.winList = threeCards;
        }
        else if (isColor && ronCards.Count == 3)
        {
            teenPattiWinMaintain.ruleNo = 2;
            teenPattiWinMaintain.winList = ronCards;
        }
        else if (ronCards.Count == 3)
        {
            //ron List
            teenPattiWinMaintain.ruleNo = 3;
            teenPattiWinMaintain.winList = ronCards;
        }
        else if (isColor)
        {
            //High Card
            teenPattiWinMaintain.ruleNo = 4;
            teenPattiWinMaintain.winList = highCards;
        }
        else if (twoCards.Count == 3)
        {
            //Two Cards
            teenPattiWinMaintain.ruleNo = 5;
            teenPattiWinMaintain.winList = twoCards;
        }
        else if (highCards.Count == 3)
        {
            //High Cards
            teenPattiWinMaintain.ruleNo = 6;
            teenPattiWinMaintain.winList = highCards;
        }


        return teenPattiWinMaintain;
        //GetTwoCard(newCardSS1);
    }

    List<CardSuffle> GetTwoCard(List<CardSuffle> cards)
    {
        List<CardSuffle> twoCardSuffle = new List<CardSuffle>();
        int cnt1 = 0;
        int cnt2 = 0;
        int startNo = cards[0].cardNo;
        int endNo = cards[1].cardNo;
        /*   print("Card Satrt No : " + cards.Count);
           print("Card End No : " + endNo);*/
        for (int i = 0; i < cards.Count; i++)
        {


            if (cards[i].cardNo == startNo)
            {
                cnt1++;
            }
            else if (cards[i].cardNo == endNo)
            {
                cnt2++;
            }
        }
        /*    print("card cnt 1 : " + cnt1);
            print("card cnt 2 : " + cnt2);*/
        if (cnt1 == 2)
        {
            int noEnter = -1;
            for (int i = 0; i < cards.Count; i++)
            {
                // print("Card No : " + cards[i].cardNo);
                if (cards[i].cardNo == startNo)
                {
                    // print("Enter Card");
                    twoCardSuffle.Add(cards[i]);
                }
                else
                {
                    noEnter = i;
                }
            }
            //print("No Enter : " + noEnter);

            twoCardSuffle.Add(cards[noEnter]);

        }
        else if (cnt2 == 2)
        {
            int noEnter = -1;
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardNo == endNo)
                {
                    twoCardSuffle.Add(cards[i]);
                }
                else
                {
                    noEnter = i;
                }
            }
            twoCardSuffle.Add(cards[noEnter]);
        }


        //    print("Enter twoCard Suffle Count : " + twoCardSuffle.Count);
        return twoCardSuffle;

    }

    bool IsColorMatch(List<CardSuffle> cards)
    {
        int cnt1 = 0;
        int cnt2 = 0;
        int cnt3 = 0;
        int cnt4 = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].color == CardColorType.Clubs)
            {
                cnt1++;
            }
            else if (cards[i].color == CardColorType.Diamonds)
            {
                cnt2++;
            }
            else if (cards[i].color == CardColorType.Spades)
            {
                cnt3++;
            }
            else if (cards[i].color == CardColorType.Hearts)
            {
                cnt4++;
            }
        }

        if (cnt1 >= 3 || cnt2 >= 3 || cnt3 >= 3 || cnt4 >= 3)
        {
            return true;
        }

        return false;
    }

    List<CardSuffle> RonValue(List<CardSuffle> cards)
    {


        List<CardSuffle> ronvalue = new List<CardSuffle>();
        bool isRon = false;
        if (cards[1].cardNo == cards[0].cardNo + 1 && cards[2].cardNo == cards[0].cardNo + 2)
        {
            isRon = true;
        }
        else if (cards[0].cardNo == 2 && cards[1].cardNo == 3 && cards[2].cardNo == 14)
        {
            isRon = true;
        }

        if (isRon)
        {
            ronvalue = cards;
        }



        //   print("is Ron : " + isRon);
        return ronvalue;
    }

    List<CardSuffle> GetThreeCard(List<CardSuffle> cards)
    {
        List<CardSuffle> threeCardSuffle = new List<CardSuffle>();
        int cnt = 0;
        int startNo = cards[0].cardNo;

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].cardNo == startNo)
            {
                cnt++;
            }
        }

        if (cnt == 3)
        {
            threeCardSuffle = cards;
        }

        for (int i = 0; i < threeCardSuffle.Count; i++)
        {
            // print(i + "-" + threeCardSuffle[i].cardNo);
        }

        return threeCardSuffle;

    }

    List<CardSuffle> HighCard(List<CardSuffle> cards)
    {
        //    print("high cards count : " + cards.Count);s
        List<CardSuffle> highCards = new List<CardSuffle>();
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            highCards.Add(cards[i]);
        }

        return highCards;
    }

    List<CardSuffle> NewSort(List<CardSuffle> cards)
    {
        List<CardSuffle> newCards = new List<CardSuffle>();
        //newCards = cards;
        for (int i = 0; i < cardSufflesSort.Count; i++)
        {
            for (int j = 0; j < cards.Count; j++)
            {
                if (cardSufflesSort[i].cardNo == cards[j].cardNo && cardSufflesSort[i].color == cards[j].color)
                {
                    CardSuffle c = new CardSuffle();
                    c.cardNo = cardSufflesSort[i].cardNo;
                    c.color = cardSufflesSort[i].color;
                    c.cardSprite = cardSufflesSort[i].cardSprite;
                    //newCards.Add(cardSufflesSort[i]);
                    newCards.Add(c);
                    break;
                }
            }
        }
        // print("new cards Count : " + newCards.Count);
        for (int i = 0; i < newCards.Count; i++)
        {
            if (newCards[i].cardNo == 1)
            {
                newCards[i].cardNo = 14;
            }
            else if (newCards[i].cardNo == 11)
            {
                newCards[i].cardNo = 13;
            }
            else if (newCards[i].cardNo == 13)
            {
                newCards[i].cardNo = 11;

            }
        }

        return newCards;
    }

    #endregion

    public void EnableSeeCards()
    {
        boxDisplayCount++;
        if (boxDisplayCount != 4) return;
        SeeButtonClick();
    }

    #region GamePlay Button And Other Manage

    public void SeeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        for (int i = 0; i < player1.seeObj.Length; i++)
        {
            player1.seeObj[i].SetActive(false);
        }

        player1.CardDisplay();
        DisplayRules();

        //currentPriceValue = minLimitValue * 2;
        /*if (currentPriceValue < 10)
        {
            currentPriceValue = doubleLimitValue;
            currentPriceIndex = 1;
        }*/
        if (currentPlayer == player1.playerNo && !player1.isSeen)//if seen bet is also minimumValue, then the value will not increase
        {
            if (currentSeenValue > currentPriceValue)
            {
                currentPriceValue = currentSeenValue - 3;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (currentPriceValue <= numbers[i])
                    {
                        currentPriceValue = numbers[i];
                        currentPriceIndex = i;
                        runningPriceIndex = i;
                        break;
                    }
                }
            }
            //currentPriceIndex += 1;
            currentPriceValue = numbers[currentPriceIndex];
            priceBtnTxt.text = player1.isSeen ? "Chaal : " + currentPriceValue : "Blind : " + currentPriceValue;
            priceBtnTxtDouble.text = player1.isSeen ? "Chaal : " + currentPriceValue * 2 : "Blind : " + currentPriceValue * 2;
        }
        player1.isSeen = true;
        player1.isBlind = false;
        player1.isPack = false;
        //currentPriceIndex = 1;
        //blindx2button.SetActive(false);
        //plusBtn.gameObject.SetActive(true);
        //minusBtn.gameObject.SetActive(true);
        //priceBtnTxt.gameObject.transform.parent.transform.localPosition = new Vector3(490.00f, 90.81f, 0.00f);
        //priceBtnTxt.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        priceBtnTxt.text = "Chaal : " + currentPriceValue;
        priceBtnTxtDouble.text = "Chaal : " + currentPriceValue * 2;
        Debug.Log("ChangeCardStatus   =>  " + playerNo);

        ChangeCardStatus("SEEN", player1.playerNo);
    }

    public void GiftButtonClick(TeenPattiPlayer giftPlayer)
    {
        SoundManager.Instance.ButtonClick();
        giftScreenObj.SetActive(true);
        GiftSendManager.Instance.gameName = "TeenPatti";
        GiftSendManager.Instance.teenPattiOtherPlayer = giftPlayer;
    }

    public IEnumerator RestartGamePlay()
    {

        Debug.Log("isGameStarted => " + isGameStarted);


        isGameStarted = false;
        DeleteAllCoins();
        for (int i = 0; i < playerSquList.Count; i++)
        {
            playerSquList[i].NotATurn();
        }
        Debug.Log("<color=blue>-------RestartGamePlay BEFORE---------</color>");
        yield return new WaitForSeconds(6f);
        isWinningRun = false;
        Debug.Log("<color=blue>--------RestartGamePlay AFTER--------</color>");
        //  CheckAllPlayerBalanceAndReplace();
        //  yield return new WaitForSeconds(3f);
        //print("Enther The Generate Player");
        if (isAdmin)
        {
            winningBotNo = -1;
            CheckNewPlayers();
            StartGamePlay();
            Debug.LogError("StartGamePlay");

            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
            print("Enther The Generate Player1");
            //isBotActivate = true;

        }
    }

    public void CheckAllPlayerBalanceAndReplace()
    {
        int num = DataManager.Instance.joinPlayerDatas.Count;
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].isBot)
            {
                // Convert the playerBalance.text (string) to a float for comparison
                float playerBalanceValue;
                if (float.TryParse(teenPattiPlayers[i].playerBalence.text, out playerBalanceValue))
                {
                    // Check if the balance of the bot is below the minimum limit value
                    if (playerBalanceValue < minLimitValue)
                    {
                        for (int j = 0; j < num; j++)
                        {
                            // Match the playerId of the bot with the userId in joinPlayerDatas
                            if (DataManager.Instance.joinPlayerDatas[j].userId.Equals(teenPattiPlayers[i].playerId))
                            {
                                // Remove the player from joinPlayerDatas
                                DataManager.Instance.joinPlayerDatas.RemoveAt(j);

                                // Add a new bot at the same index
                                AddNewBotAtIndex(j); // Use 'j' as index to replace at the same position

                                Debug.Log("Removed bot with low balance and added a new bot at index: " + j);
                                break; // Exit inner loop after replacement
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Invalid balance format for bot: " + teenPattiPlayers[i].name);
                }
            }
        }
    }


    private void AddNewBotAtIndex(int index)
    {
        Debug.Log("INDEX =  " + index);
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int avatarIndex = avatars[0]; // Pick the first shuffled avatar

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int nameIndex = names[0]; // Pick the first shuffled name

        string avatar = BotManager.Instance.botUser_Profile_URL[avatarIndex];
        string botUserName = BotManager.Instance.botUserName[nameIndex];
        string userId = Guid.NewGuid().ToString().Substring(0, 8) + "TeenPatti"; // Generate a new unique user ID

        // Add the new bot at the specified index
        DataManager.Instance.AddRoomUserLOWBALENCE(userId, botUserName,
            DataManager.Instance.joinPlayerDatas[index].lobbyId, // Ensure this index refers to valid data
            UnityEngine.Random.Range(50, 55).ToString(), index, avatar, index); // Pass the index

        Debug.Log("New TeenPatti BOT added at index: " + index);
    }




    public void StartGamePlay()
    {
        //StartCoroutine(RestartGamePlay());
        //Debug.Log("StartGamePlay");
        Debug.Log("IS ADMIN =>  " + isAdmin);
        if (isAdmin)
        {
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        minLimitValue = DataManager.Instance.betPrice;
        for (int i = 0; i < numbers.Length; i++)
        {
            if (minLimitValue == numbers[i])
            {
                currentPriceIndex = i;
                runningPriceIndex = i;
            }
        }

        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        isGameStop = true;
        waitNextRoundScreenObj.SetActive(false);
        playerFindScreenObj.SetActive(false);
        Debug.Log("playerFindScreenObj False");

        bootValue = 1f;
        potLimitValue = 30f;
        //minLimitValue = 5f;
        doubleLimitValue = 10f;
        maxLimitValue = 1000f;
        incrementNo = 5f;
        minChaalValue = minLimitValue * 2;
        maxChaalValue = maxLimitValue * 2;
        minBlindValue = minLimitValue;
        maxBlindValue = maxLimitValue;


        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            teenPattiPlayers[i].isSeen = false;
            teenPattiPlayers[i].isPack = false;
            teenPattiPlayers[i].isBlind = true;
            teenPattiPlayers[i].userTurnCount = 0;
            teenPattiPlayers[i].SetActiveTrue();
            teenPattiPlayers[i].inactiveCount = 0;
        }

        currentPriceValue = minLimitValue;
        currentBlindValue = minLimitValue;
        currentSeenValue = minLimitValue;
        priceBtnTxt.text = "Blind : " + currentPriceValue;
        priceBtnTxtDouble.text = "Blind : " + currentPriceValue * 2;

        minusBtn.interactable = false;
        rulesTab.SetActive(false);
        roundCounter = 0;
        totalBetAmount = 0;
        boxDisplayCount = 0;
        //runningPriceIndex = 0;
        //currentPriceIndex = 0;
        //StartBet();//Greejesh
        betAmountTxt.text = totalBetAmount.ToString();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            TeenPattiPlayer currentPlayer = teenPattiPlayers[i];
            for (int j = 0; j < currentPlayer.playerWinObj.Length; j++)
            {
                currentPlayer.playerWinObj[j].SetActive(false);
            }
            currentPlayer.packImg.SetActive(false);
            currentPlayer.seenImg.SetActive(false);
            currentPlayer.blindIMG.SetActive(false);
            currentPlayer.cardImg1.gameObject.SetActive(false);
            currentPlayer.cardImg2.gameObject.SetActive(false);
            currentPlayer.cardImg3.gameObject.SetActive(false);
            currentPlayer.delearObj.SetActive(false);

            for (int j = 0; j < currentPlayer.seeObj.Length; j++)
            {
                currentPlayer.seeObj[j].SetActive(false);
            }
        }

        isGameStarted = true;

        StartCoroutine(DataMaintain());
        Debug.Log("DataMaintain");
    }

    IEnumerator DataMaintain()
    {
        Debug.Log("DataMaintain   DataMaintain");
        playerSquList.Clear();
        if (DataManager.Instance.joinPlayerDatas.Count == 2)
        {
            Debug.Log("NO ________>     2");
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(true);
            player3.gameObject.SetActive(false);
            player4.gameObject.SetActive(false);
            player5.gameObject.SetActive(false);

            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
                else
                {
                    player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player2.playerNo = (i + 1);
                    player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player2.UpdateAvatar();
                }
            }


            if (player1.playerNo == 1)
            {
                playerSquList.Add(player1);
                playerSquList.Add(player2);
            }
            else if (player1.playerNo == 2)
            {
                playerSquList.Add(player2);
                playerSquList.Add(player1);
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 3)
        {
            Debug.Log("NO ________>     3");

            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
            }

            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(false);

                playerSquList.Add(player1);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {

                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;

                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 2)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(false);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {

                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            playerSquList.Add(player3);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player1);

                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);

                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 3)
            {
                Debug.Log("NO ________>     3-");

                player2.gameObject.SetActive(false);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(true);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                    }
                }
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 4)
        {
            Debug.Log("NO ________>     4");

            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                    // player1.playerNo = 1;
                    player1.playerNo = (i + 1);
                }
            }

            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            // player2.playerNo =  2;
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            //  player4.playerNo = 4;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            //  player5.playerNo = 5;
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 2)
            {
                Debug.Log("NO ________>     2-");

                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(false);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 3)
            {
                Debug.Log("NO ________>     3-");

                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 4)
            {
                Debug.Log("NO ________>     4-");

                player2.gameObject.SetActive(false);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            playerSquList.Add(player1);

                            cntPlayer++;
                        }
                    }
                }
            }
        }
        else if (DataManager.Instance.joinPlayerDatas.Count == 5)
        {
            Debug.Log("NO ________>     5");

            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
            }
            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;

                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 2)
            {
                Debug.Log("NO ________>     2-");

                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 3)
            {
                Debug.Log("NO ________>     3-");

                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;

                        }
                        else if (cntPlayer == 1)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 4)
            {
                Debug.Log("NO ________>     4-");

                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;

                        }
                        else if (cntPlayer == 3)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                    }
                }
            }
            else if (player1.playerNo == 5)
            {
                Debug.Log("NO ________>     5-");

                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            // playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalence.text = DataManager.Instance.joinPlayerDatas[i].balance;

                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                    }
                }
            }
        }


        yield return new WaitForSeconds(1f);
        ActivateBotPlayers();



        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].isBot && teenPattiPlayers[i].gameObject.activeInHierarchy)
            {
                Debug.Log("----NAme  = >  " + teenPattiPlayers[i]);
                BetAnim(teenPattiPlayers[i], currentPriceValue, currentPriceIndex);
                Debug.Log(".");
            }
            else if (!teenPattiPlayers[i].isBot && teenPattiPlayers[i].gameObject.activeInHierarchy)
            {
                Debug.Log("----NAme  = >  " + teenPattiPlayers[i]);
                Debug.Log("<color=blue>.</color>");
                StartBetTORealPlayer(teenPattiPlayers[i]);
            }
        }


        yield return new WaitForSeconds(1f);

        int playerSend = DataManager.Instance.joinPlayerDatas.Count;
        float speed = 0.2f;

        Debug.Log($"Starting card distribution for {playerSend} players.");

        // Track the number of cards each active player has received
        int[] cardsGiven = new int[teenPattiPlayers.Count];
        int totalCardsDistributed = 0;
        int totalCardsNeeded = playerSend * 3; // Each active player needs 3 cards

        // Continue distributing cards until all active players have received three cards each
        while (totalCardsDistributed < totalCardsNeeded)
        {
            bool cardDistributedInThisRound = false;

            for (int i = 0; i < teenPattiPlayers.Count; i++)
            {
                Debug.Log(" cardsGiven = > " + cardsGiven[i]);
                if (teenPattiPlayers[i].gameObject.activeSelf && cardsGiven[i] < 3)
                {
                    int currentCardIndex = cardsGiven[i] + 1; // Card index to be distributed (1, 2, or 3)
                    Debug.Log($"Player {i + 1} is active. Sending card {currentCardIndex}...");

                    GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
                    SoundManager.Instance.CasinoCardMoveSound();
                    obj.transform.position = cardTmpStart.transform.position;

                    // Determine which card image to move to (cardImg1, cardImg2, or cardImg3)
                    Transform targetCardPosition = currentCardIndex switch
                    {
                        1 => teenPattiPlayers[i].cardImg1.transform, // First card position
                        2 => teenPattiPlayers[i].cardImg2.transform, // Second card position
                        3 => teenPattiPlayers[i].cardImg3.transform, // Third card position
                        _ => null
                    };

                    Debug.Log($"Card {currentCardIndex} is moving to Player {i + 1}'s position...");

                    // Perform the animation to move the card to the appropriate position
                    obj.transform.DOMove(targetCardPosition.position, speed).OnComplete(() =>
                    {
                        Debug.Log($"Card {currentCardIndex} reached Player {i + 1}. Activating card image...");
                        Destroy(obj);

                        // Activate the appropriate card image once the animation is complete
                        switch (currentCardIndex)
                        {
                            case 1:
                                teenPattiPlayers[i].cardImg1.gameObject.SetActive(true);
                                Debug.Log($"Player {i + 1} - First card activated.");
                                break;
                            case 2:
                                teenPattiPlayers[i].cardImg2.gameObject.SetActive(true);
                                Debug.Log($"Player {i + 1} - Second card activated.");
                                break;
                            case 3:
                                teenPattiPlayers[i].cardImg3.gameObject.SetActive(true);
                                Debug.Log($"Player {i + 1} - Third card activated.");
                                break;
                        }
                    });

                    // Update tracking variables
                    cardsGiven[i]++;
                    totalCardsDistributed++;

                    // Indicate that a card was distributed in this round
                    cardDistributedInThisRound = true;

                    // Wait for the animation to complete before moving to the next player
                    yield return new WaitForSeconds(speed);

                    // Exit loop if all cards have been distributed
                    if (totalCardsDistributed >= totalCardsNeeded)
                    {
                        break;
                    }
                }
            }

            // Check if no card was distributed in this round (safety check to prevent infinite loop)
            if (!cardDistributedInThisRound)
            {
                Debug.LogError("No card was distributed in this round! Exiting to prevent infinite loop.");
                break;
            }
        }

        Debug.Log("All three cards have been successfully distributed to all active players.");


        yield return new WaitForSeconds(speed);
        for (int i = 0; i < player1.seeObj.Length; i++)
        {
            player1.seeObj[i].SetActive(true);
        }


        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].playerNo == gameDealerNo)
            {
                playerSquList[i].RestartFillLine();
                playerSquList[i].delearObj.SetActive(true);
                if (playerSquList[i].playerNo == player1.playerNo)
                {
                    ShowTextChange();
                    bottomBox.SetActive(true);
                    DataManager.Instance.UserTurnVibrate();
                    EnableSeeCards();
                }
                else
                {
                    bottomBox.SetActive(false);
                }
            }
            else
            {
                playerSquList[i].delearObj.SetActive(false);
                playerSquList[i].NotATurn();
            }
        }

        isGameStop = false;

        isBotActivate = true;
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true)
            {
                playerSquList[i].CardGenerate();
            }
        }
        CheckBotWinning();
    }





    private void CheckBotWinning()//this functions determines that which bot has better cards than player and if more than 1 bot does then it determines which has the best one
    {
        if (winningBotNo == -1)
        {
            foreach (var player in playerSquList)
            {
                player.SumOfPlayerCards();
            }
            List<TeenPattiPlayer> teenPattiWinner = new List<TeenPattiPlayer>();
            List<TeenPattiPlayer> sortedNumbersDescending = playerSquList.OrderByDescending(n => n.sumOfCards).ToList();

            for (int i = 0; i < sortedNumbersDescending.Count; i++)
            {
                //calculate sum of 3 card value and store in a varible
                if (sortedNumbersDescending[i].isPack == false)
                {
                    bool isEnter = false;
                    for (int j = 0; j < teenPattiWinner.Count; j++)
                    {
                        if (teenPattiWinner[j].ruleNo > sortedNumbersDescending[i].ruleNo)
                        {
                            isEnter = true;
                        }
                    }
                    // Highest rule number player will be added to teenpattiwinner list
                    if (isEnter == true)
                    {
                        teenPattiWinner.Clear();

                        teenPattiWinner.Add(sortedNumbersDescending[i]);
                        //print("Clear");
                    }
                    else if (teenPattiWinner.Count == 0)
                    {
                        teenPattiWinner.Add(sortedNumbersDescending[i]);

                    }//print("Add");
                }
            }
            if (teenPattiWinner[0].isBot)
                winningBotNo = teenPattiWinner[0].playerNo;
            else
                winningBotNo = 0;
        }
    }

    public void ResetBot()
    {
        player1.isBot = false;
        player2.isBot = false;
        player3.isBot = false;
        player4.isBot = false;
        player5.isBot = false;
    }

    private void ActivateBotPlayers()
    {
        /*switch (isAdmin)
        {
            case true:
                switch (MainMenuManager.Instance.botPlayers)
                {
                    case 4:
                        player2.isBot = true;
                        player3.isBot = true;
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 3:
                        player3.isBot = true;
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 2:
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 1:
                        player5.isBot = true;
                        break;
                }

                break;
            case false:
                switch (MainMenuManager.Instance.botPlayers)
                {
                    case 4:
                        player2.isBot = true;
                        player3.isBot = true;
                        player4.isBot = true;
                        player5.isBot = true;
                        break;
                    case 3:
                        player2.isBot = true;
                        player3.isBot = true;
                        player4.isBot = true;
                        break;
                    case 2:
                        player2.isBot = true;
                        player3.isBot = true;
                        break;
                    case 1:
                        player2.isBot = true;
                        break;
                }

                break;
        }*/

        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.joinPlayerDatas[i].userId.EndsWith("TeenPatti"))
            {
                playerSquList[i].isBot = true;
            }
        }
    }



    #endregion

    #region Panel Button

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    public void MessageButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        messageScreeObj.SetActive(true);
    }


    public void PackButtonClick()
    {
        if (!isGameStop)
        {
            SoundManager.Instance.ButtonClick();
            Debug.Log("ChangeCardStatus   =>  " + playerNo);

            ChangeCardStatus("PACK", player1.playerNo);
            bottomBox.SetActive(false);
        }
    }

    public void LobbyButtonClick()
    {
        menuScreenObj.SetActive(false);
        exitPanel.gameObject.SetActive(true);
        //Time.timeScale = 0;
    }

    public void CloseExitPopup()
    {
        exitPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowButtonClick(Text t)
    {
        if (isGameStop) return;
        SoundManager.Instance.ButtonClick();
        //BetAnim(player1, currentPriceValue);


        switch (t.text)
        {
            case "Show":
                ShowCardToAllUser();
                CheckFinalWinner("Show");
                //SendTeenPattiBet(player1.playerNo, 0, "Show", "", "");
                //ChangePlayerTurn(player1.playerNo);
                break;
            case "Side Show":
                if (CheckMoney(currentPriceValue) == false)
                {
                    SoundManager.Instance.ButtonClick();
                    Debug.Log("OpenErrorScreen");
                    OpenErrorScreenONBET();
                    return;
                }
                //BetAnim(player1, currentPriceValue);
                SendTeenPattiBet(player1.playerNo, currentPriceValue, "SideShow", slideShowPlayer.playerId, player1.playerId);
                Debug.LogError("slideShowPlayer.playerId => " + slideShowPlayer.playerId + "    is bot =>  " + slideShowPlayer.isBot);

                Debug.Log("player1 => " + player1.playerBalence.text);
                DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 1);
                Debug.Log("player1 => " + player1.playerBalence.text);
                Debug.Log("playerBetAmount => " + playerBetAmount);
                playerBetAmount += currentPriceValue;
                Debug.Log("playerBetAmount => " + playerBetAmount);
                Debug.Log("currentPriceValue => " + currentPriceValue);
                //Game Stop and check the card and one card is pack
                showButton.interactable = false;
                if (slideShowPlayer.isBot)
                {
                    Debug.Log("Slid Show OPEN");
                    Invoke(nameof(OnPopupButtonClick), delay);
                }
                break;
        }
    }

    private void OnPopupButtonClick()
    {
        if (isPopupOpen) return;
        Debug.Log("Slid Show OPEN");
        isPopupOpen = true;
        sideShowPopupImage.gameObject.SetActive(true);
        sideShowPopupImageText.text = slideShowPlayer.playerNameTxt.text + " Denied SideShow Request";
        Debug.Log("Player No   =>  " + playerNo);
        Debug.Log("slideShowPlayer Player No   =>  " + slideShowPlayer.playerNo);
        ChangePlayerTurn(playerNo);
        //sideShowPopupImage.transform.localScale = Vector3.zero;
        /*sideShowPopupImage.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
        });*/
        StartCoroutine(WaitForPopupClose());
    }

    private IEnumerator WaitForPopupClose()
    {
        yield return new WaitUntil(() => !isPopupOpen);
        showButton.interactable = true;
    }

    public void ClosePopup()
    {
        if (!isPopupOpen) return;
        isPopupOpen = false;
        sideShowPopupImage.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            sideShowPopupImage.gameObject.SetActive(false);
        });
    }

    public void MinusButtonClick()
    {
        if (isGameStop) return;
        /*SoundManager.Instance.ButtonClick();
            minusBtn.interactable = false;
            plusBtn.interactable = true;

            currentPriceValue /= 2;
            if (!player1.isPack && player1.isBlind)
            {

                priceBtnTxt.text = "Blind\n" + currentPriceValue;
            }
            else if (!player1.isPack && player1.isSeen)
            {

                priceBtnTxt.text = "Chaal\n" + currentPriceValue;
            }*/
        SoundManager.Instance.ButtonClick();
        // minusBtn.interactable = false;
        // plusBtn.interactable = true;
        if (currentPriceIndex > 0 && currentPriceIndex > runningPriceIndex)
        {
            currentPriceIndex--;
            currentPriceValue = numbers[currentPriceIndex];
            //PriceTxt.text = price.ToString();
            if (currentPriceIndex == numbers.Length - 1)
            {
                plusBtn.interactable = false;
                minusBtn.interactable = true;
            }
            else if (currentPriceIndex == 0)
            {
                plusBtn.interactable = true;
                minusBtn.interactable = false;
            }
            else
            {
                plusBtn.interactable = true;
                minusBtn.interactable = true;
            }
        }
        else
        {
            plusBtn.interactable = true;
            minusBtn.interactable = false;
        }

        priceBtnTxt.text = player1.isPack switch
        {
            //currentPriceValue /= 2;
            false when player1.isBlind => "Blind : " + currentPriceValue,
            false when player1.isSeen => "Chaal : " + currentPriceValue,
            _ => priceBtnTxt.text
        };
        priceBtnTxtDouble.text = player1.isPack switch
        {
            //currentPriceValue /= 2;
            false when player1.isBlind => "Blind : " + currentPriceValue * 2,
            false when player1.isSeen => "Chaal : " + currentPriceValue * 2,
            _ => priceBtnTxtDouble.text
        };
    }

    public void PlusButtonClick()
    {
        if (isGameStop) return;
        /*SoundManager.Instance.ButtonClick();
            plusBtn.interactable = false;
            minusBtn.interactable = true;

            currentPriceValue *= 2;
            if (!player1.isPack && player1.isBlind)
            {

                priceBtnTxt.text = "Blind\n" + currentPriceValue;
            }
            else if (!player1.isPack && player1.isSeen)
            {

                priceBtnTxt.text = "Chaal\n" + currentPriceValue;
            }*/
        SoundManager.Instance.ButtonClick();
        // plusBtn.interactable = false;
        // minusBtn.interactable = true;

        if (currentPriceIndex < numbers.Length - 1)
        {
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    if(currentPriceValue == numbers[i])
            //    {
            //        currentPriceIndex = i;
            //    }
            //}
            currentPriceIndex++;
            currentPriceValue = numbers[currentPriceIndex];
            //PriceTxt.text = price.ToString();
            if (currentPriceIndex == numbers.Length - 1)
            {
                plusBtn.interactable = false;
                minusBtn.interactable = true;
            }
            else if (currentPriceIndex == 0)
            {
                plusBtn.interactable = true;
                minusBtn.interactable = false;
            }
            else
            {
                plusBtn.interactable = true;
                minusBtn.interactable = true;
            }
        }

        priceBtnTxt.text = player1.isPack switch
        {
            //currentPriceValue *= 2;
            false when player1.isBlind => "Blind : " + currentPriceValue,
            false when player1.isSeen => "Chaal : " + currentPriceValue,
            _ => priceBtnTxt.text
        };
        priceBtnTxtDouble.text = player1.isPack switch
        {
            //currentPriceValue *= 2;
            false when player1.isBlind => "Blind : " + currentPriceValue * 2,
            false when player1.isSeen => "Chaal : " + currentPriceValue * 2,
            _ => priceBtnTxtDouble.text
        };
    }

    public void StartBet()
    {
        if (CheckMoney(currentPriceValue) == false)
        {
            SoundManager.Instance.ButtonClick();
            Debug.Log("OpenErrorScreen");

            OpenErrorScreen();
            return;
        }
        SoundManager.Instance.ThreeBetSound();
        BetAnim(player1, currentPriceValue, currentPriceIndex);
        DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 2);
        playerBetAmount += currentPriceValue;
    }

    public void StartBetTORealPlayer(TeenPattiPlayer player)
    {
        if (CheckMoney(currentPriceValue) == false)
        {
            SoundManager.Instance.ButtonClick();
            Debug.Log("OpenErrorScreen");

            OpenErrorScreenONBET();
            return;
        }
        SoundManager.Instance.ThreeBetSound();
        string id = player.playerId;
        BetAnim(player, currentPriceValue, currentPriceIndex);
        DataManager.Instance.DebitAmount((currentPriceValue).ToString(), id, "TeenPatti-Bet-" + id, "game", 2);
        playerBetAmount += currentPriceValue;
    }
    public void BetButtonClick()
    {
        if (!isGameStop)
        {
            if (CheckMoney(currentPriceValue) == false)
            {
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("OpenErrorScreen");

                OpenErrorScreen();
                return;
            }
            if (player1.isBlind)
            {
                currentBlindValue = currentPriceValue;
                //currentPriceValue = currentBlindValue;
                //for (int i = 0; i < numbers.Length; i++)
                //{
                //    if(currentPriceValue == numbers[i])
                //    {
                //        currentPriceIndex = i;
                //        runningPriceIndex = i;
                //        break;
                //    }
                //}
            }
            SoundManager.Instance.ThreeBetSound();
            BetAnim(player1, currentPriceValue, currentPriceIndex);
            DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 3);
            playerBetAmount += currentPriceValue;
            //priceBtnTxt.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            // bonusUseValue
            // User Maintain
            runningPriceIndex = currentPriceIndex;
            SendTeenPattiBet(player1.playerNo, currentPriceValue, player1.isBlind ? "Blind" : "Bet", "", "");
            Debug.LogError("mahadeV 4");

            ChangePlayerTurn(player1.playerNo);
        }
    }
    public void BetButtonClickDouble()
    {
        if (!isGameStop)
        {
            if (CheckMoney(currentPriceValue * 2) == false)
            {
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("OpenErrorScreen");

                OpenErrorScreen();
                return;
            }
            if (player1.isBlind)
            {
                currentBlindValue = currentPriceValue * 2;
                //currentPriceValue = currentBlindValue;
                //for (int i = 0; i < numbers.Length; i++)
                //{
                //    if(currentPriceValue == numbers[i])
                //    {
                //        currentPriceIndex = i;
                //        runningPriceIndex = i;
                //        break;
                //    }
                //}
            }
            SoundManager.Instance.ThreeBetSound();
            BetAnim(player1, currentPriceValue * 2, currentPriceIndex);
            DataManager.Instance.DebitAmount((currentPriceValue * 2).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 3);
            playerBetAmount += currentPriceValue * 2;
            //priceBtnTxt.transform.parent.gameObject.GetComponent<Button>().interactable = false;
            // bonusUseValue
            // User Maintain
            runningPriceIndex = currentPriceIndex;
            SendTeenPattiBet(player1.playerNo, currentPriceValue * 2, player1.isBlind ? "Blind" : "Bet", "", "");
            Debug.LogError("mahadeV 4");

            ChangePlayerTurn(player1.playerNo);
        }
    }
    //public void DoubleBetButtonClick()
    //{
    //    if (!isGameStop)
    //    {
    //        currentPriceValue = doubleLimitValue;
    //        currentPriceIndex = 1;

    //        if (CheckMoney(currentPriceValue) == false)
    //        {
    //            SoundManager.Instance.ThreeBetSound();
    //            OpenErrorScreen();
    //            return;
    //        }
    //        SoundManager.Instance.ThreeBetSound();
    //        BetAnim(player1, currentPriceValue, currentPriceIndex);
    //        DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 3);
    //        playerBetAmount += currentPriceValue;
    //        // bonusUseValue
    //        // User Maintain
    //        runningPriceIndex = currentPriceIndex;
    //        SendTeenPattiBet(player1.playerNo, currentPriceValue, "Bet", "", "");
    //        ChangePlayerTurn(player1.playerNo);
    //        //blindx2button.SetActive(false);
    //        //plusBtn.gameObject.SetActive(true);
    //        //minusBtn.gameObject.SetActive(true);
    //        //priceBtnTxt.gameObject.transform.parent.transform.localPosition = new Vector3(490.00f, 90.81f, 0.00f);
    //        priceBtnTxt.text = "Chaal\n" + currentPriceValue;
    //    }
    //}

    #endregion

    #region Menu Panel

    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    public void CloseMenuScreenButton()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
    }

    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            Time.timeScale = 1;
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else if (no == 2)
        {
            OpenRuleScreen();
        }
        else if (no == 3)
        {
            //Shop
            Instantiate(shopPrefab, shopPrefabParent.transform);
        }
    }

    public void LeaveTableForInactivity()
    {
        Time.timeScale = 1;
        SoundManager.Instance.StartBackgroundMusic();
        SceneManager.LoadScene("Main");
    }

    #endregion

    #region Rule Panel

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }

    public void CloseRuleButton()
    {
        SoundManager.Instance.ButtonClick();
        ruleScreenObj.SetActive(false);
    }

    #endregion

    #region Bet Maintain

    public void BetAnim(TeenPattiPlayer player, float amount, int priceIndex)
    {
        /*GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = amount.ToString();
        genBetObj.transform.position = player.sendBetObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            totalBetAmount += amount;
            betAmountTxt.text = totalBetAmount.ToString();
        });*/
        Debug.Log("amount  => " + amount);
        totalBetAmount += amount;
        Debug.Log("totalBetAmount  => " + totalBetAmount);
        float currentBalance = float.Parse(player.playerBalence.text);
        Debug.Log("currentBalance => " + currentBalance);
        Debug.Log("player.playerBalence => " + float.Parse(player.playerBalence.text));
        // Subtract the amount from the balance
        currentBalance -= amount;
        Debug.LogError("CHAL AMount  " + amount);
        // Update the player's balance text with the new balance
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            Debug.Log("joinPlayerDatas => " + DataManager.Instance.joinPlayerDatas[i].userId + "   DataManager.Instance.playerData._id   =>  " + DataManager.Instance.playerData._id);
            if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(player.playerId))
            {
                float balance;
                if (float.TryParse(DataManager.Instance.joinPlayerDatas[i].balance, out balance))
                {
                    balance -= amount;

                    DataManager.Instance.joinPlayerDatas[i].balance = balance.ToString();
                }
            }
        }
        player.playerBalence.text = currentBalance.ToString();
        betAmountTxt.text = totalBetAmount.ToString();
        Debug.Log("PRICE INDEX _______ > " + priceIndex);
        SpawnCoin(priceIndex, player.transform);
    }

    public void GetBotBetNo(int num, int botPlayerNo, float currentAmount, int currentIndex)
    {
        /*if (isAdmin) return;
        switch (roundCounter)
        {
            case <= 1:
                switch (num)
                {
                    case 1:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
            case 2:
                switch (num)
                {
                    case 1:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                }

                break;
            case 3:
                switch (num)
                {
                    case 1:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                }

                break;
            case >= 4:
                switch (num)
                {
                    case 1:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[botPlayerNo].isPack) return;
                        break;
                    }
                    case 4:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                        //ChangeCardStatus("SEEN", botPlayerNo);
                        if(playerSquList[index].isPack) return;
                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
        }*/

        if (isAdmin) return;

        currentSeenValue = currentAmount;
        currentPriceValue = currentAmount;
        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
        print("Bot Betting done " + currentAmount + " index = " + index);
        if (index < 0 || playerSquList[index].isPack) return;

        Debug.Log("roundCounter = > " + roundCounter);
        switch (roundCounter)
        {
            case <= 1:
            case 2:
            case 3:
            case 4:
            case 5:
                BetAnim(playerSquList[index], currentAmount, currentIndex);
                SoundManager.Instance.ThreeBetSound();
                break;

            case >= 7 when num <= 4:
                BetAnim(playerSquList[index], currentAmount, currentIndex);
                SoundManager.Instance.ThreeBetSound();
                break;

            case >= 7 when num == 5:
                break;
        }

    }


    #endregion

    #region SlideShowPanel
    public void Accept_SlideShow(string sendId, string currentId)
    {
        SlideShowSendSocket(sendId, currentId, "Accept");
        StartCoroutine(CheckSlideShowWinner(sendId, currentId, false));
    }

    public void Cancel_SlideShow(string sendId, string currentId)
    {
        SlideShowSendSocket(sendId, currentId, "Cancel");
    }

    private void SpawnCoin(int priceIndex, Transform player)
    {
        if (chipObj == null)
        {
            Debug.LogError("chipObj is null");
            return;
        }

        if (teenPattiPlayers == null || teenPattiPlayers.Count == 0)
        {
            Debug.LogError("teenPattiPlayers list is null or empty");
            return;
        }

        Debug.Log("SpawnCoin called");

        Vector3 dPos = GetRandomPosInBoxCollider2D();
        TeenPattiPlayer chipOrigin = null;

        foreach (var item in teenPattiPlayers)
        {
            if (item.playerNo == currentPlayer)
            {
                chipOrigin = item;
                break;
            }
        }

        if (chipOrigin == null || chipOrigin.transform == null)
        {
            Debug.LogError("chipOrigin or chipOrigin.transform is null");
            return;
        }

        if (priceIndex < 0 || priceIndex >= chipsSprite.Length)
        {
            Debug.LogError("priceIndex is out of bounds");
            return;
        }
        else if (chipsSprite[priceIndex] == null)
        {
            Debug.LogError("chipsSprite[priceIndex] is null");
            return;
        }

        GameObject coin = Instantiate(chipObj, chipOrigin.transform);
        coin.transform.GetComponent<Image>().sprite = chipsSprite[priceIndex];

        if (player == null || player.transform == null)
        {
            Debug.LogError("Player or Player.transform is null");
            return;
        }

        ChipGenerate(coin, player.transform, dPos);
        spawnedCoins.Add(coin);

        Debug.Log("Coin spawned and added to the list");
    }

    private Vector3 GetRandomPosInBoxCollider2D()
    {
        Bounds bounds = boxCollider.bounds;
        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 90f);
    }
    public void ChipGenerate(GameObject chip, Transform playerStartPosition, Vector3 endPos)
    {
        // Set the chip's starting position to the player's position
        chip.transform.position = playerStartPosition.position;

        // Random rotation for the chip
        chip.transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), 0.2f);

        // Move the chip to the end position
        chip.transform.DOMove(endPos, 0.3f).OnComplete(() =>
        {
            // Scale down slightly and then back to normal
            chip.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f).OnComplete(() =>
            {
                chip.transform.DOScale(Vector3.one, 0.07f);

                // Optionally set the parent if needed (you can remove this if it's not required)
                chip.transform.SetParent(playerStartPosition);
            });
        });
    }



    public void DeleteAllCoins()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            Destroy(coin);
        }
        spawnedCoins.Clear();
    }

    #endregion

    #region Error Screen
    public void OpenErrorScreen()
    {
        errorScreenObj.SetActive(true);
    }
    public void OpenErrorScreenONBET()
    {
        errorScreenObjONBET.SetActive(true);
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


    #endregion

    #region Show Maintain

    public void DisplayRules()
    {
        switch (player1.ruleNo)
        {
            case 1:
                rulesTab.SetActive(true);
                rulesText.text = "TRAIL";
                break;
            case 2:
                rulesTab.SetActive(true);
                rulesText.text = "PURE";
                break;
            case 3:
                rulesTab.SetActive(true);
                rulesText.text = "SEQUENCE";
                break;
            case 4:
                rulesTab.SetActive(true);
                rulesText.text = "COLOR";
                break;
            case 5:
                rulesTab.SetActive(true);
                rulesText.text = "PAIR";
                break;
            case 6:
                rulesTab.SetActive(true);
                rulesText.text = "HIGH";
                break;
        }
    }

    public void ShowTextChange()
    {
        /*List<TeenPattiPlayer> avaliablePlayer = new List<TeenPattiPlayer>();
        List<TeenPattiPlayer> withOutPlayerList = new List<TeenPattiPlayer>();
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].isSeen == true && playerSquList[i].isPack == false && playerSquList[i].isBlind == false)
            {
                avaliablePlayer.Add(playerSquList[i]);
                if (playerSquList[i] != player1)
                {
                    withOutPlayerList.Add(playerSquList[i]);
                }
            }
        }
        int tCnt = 0;
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].isPack == false)
            {
                tCnt++;
            }
        }
        if (avaliablePlayer.Count == 1)
        {
            //win
        }
        else if (avaliablePlayer.Count == 2 || tCnt == 2)
        {
            showButton.interactable = true;
            showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
        }
        else if (avaliablePlayer.Count > 2)
        {
            //int playerN = player1.playerNo;
            //for (int i = 0; i < teenPattiPlayers.Count; i++)
            //{
            //    if (teenPattiPlayers[i].gameObject.activeSelf == true && teenPattiPlayers[i].isSeen == true && teenPattiPlayers[i].isBlind == false && teenPattiPlayers[i].isPack == false && teenPattiPlayers[i] != player1)
            //    {
            //        int cPNo = teenPattiPlayers[i].playerNo;
            //        if (playerN == cPNo + 1)
            //        {

            //        }
            //    }
            //}

            slideShowPlayer = null;
            int currentPlayerNo = player1.playerNo;


            if (avaliablePlayer.Count == 3)
            {

                for (int j = 0; j < withOutPlayerList.Count; j++)
                {
                    int targetNo = withOutPlayerList[j].playerNo;

                    int checkNo = targetNo + 1;

                    if (slideShowPlayer == null)
                    {
                        if (currentPlayerNo == 1)
                        {
                            bool isSkip = player3.isBlind == true;

                            if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 2)
                        {
                            bool isSkip = player1.isBlind == true;

                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 3)
                        {
                            bool isSkip = player2.isBlind == true;

                            if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                    }

                }
            }
            else if (avaliablePlayer.Count == 4)
            {
                for (int j = 0; j < withOutPlayerList.Count; j++)
                {
                    int targetNo = withOutPlayerList[j].playerNo;

                    int checkNo = targetNo + 1;

                    if (slideShowPlayer == null)
                    {
                        if (currentPlayerNo == 1)
                        {
                            bool isSkip = player4.isBlind == true;
                            if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 2)
                        {
                            bool isSkip = player1.isBlind == true;
                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 3)
                        {
                            bool isSkip = player2.isBlind == true;
                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 4)
                        {
                            bool isSkip = player3.isBlind == true;
                            if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                    }

                }
            }
            else if (avaliablePlayer.Count == 5)
            {
                for (int j = 0; j < withOutPlayerList.Count; j++)
                {
                    int targetNo = withOutPlayerList[j].playerNo;

                    int checkNo = targetNo + 1;

                    if (slideShowPlayer == null)
                    {
                        if (currentPlayerNo == 1)
                        {
                            bool isSkip = player5.isBlind == true;
                            if (checkNo == 6 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 2)
                        {
                            bool isSkip = player1.isBlind == true;
                            if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 6 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 3)
                        {
                            bool isSkip = player2.isBlind == true;
                            if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 6 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 4)
                        {
                            bool isSkip = player3.isBlind == true;
                            if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 2 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                        else if (currentPlayerNo == 5)
                        {
                            bool isSkip = player4.isBlind == true;
                            if (checkNo == 5 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 4 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                            else if (checkNo == 3 && isSkip == false)
                            {
                                showButton.interactable = true;
                                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                                slideShowPlayer = withOutPlayerList[j - 1];
                            }
                        }
                    }

                }
            }

        }
        else
        {
            showButton.interactable = false;
            showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
        }*/

        switch (roundCounter)
        {
            case <= 2:
                showButton.interactable = false;
                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side Show";
                break;
            case >= 3:
                showButton.interactable = true;
                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side Show";
                break;
        }

        List<TeenPattiPlayer> availablePlayer = playerSquList.Where(t => t.gameObject.activeSelf && !t.isPack).ToList();

        var myIndex = availablePlayer.IndexOf(player1);

        int previousIndex = (myIndex - 1 + availablePlayer.Count) % availablePlayer.Count;

        while (availablePlayer[previousIndex].isPack)
        {
            previousIndex = (previousIndex - 1 + availablePlayer.Count) % availablePlayer.Count;
        }

        var previousPlayer = availablePlayer[previousIndex];
        slideShowPlayer = previousPlayer;



        if (availablePlayer.Count >= 3) return;
        showButton.interactable = true;
        showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
    }


    #endregion

    #region Game Restart Round Maintain

    public void GameRestartRound()
    {

    }

    #endregion

    #region Socket Manager

    public void GetChat(string playerID, string msg)
    {
        if (playerID.Equals(DataManager.Instance.playerData._id))
        {
            TypeMessageBox typeMessageBox = Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
            typeMessageBox.Update_Message_Box(msg);
        }
        else
        {
            TypeMessageBox typeMessageBox = Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
            typeMessageBox.Update_Message_Box(msg);
        }
        Canvas.ForceUpdateCanvases();
    }

    public void GetGift(string sendPlayerID, string receivePlayerId, int giftNo)
    {
        GameObject sendPlayerObj = null;
        GameObject receivePlayerObj = null;

        //print("Send Id : " + sendPlayerID);
        //print("Receive Id : " + receivePlayerId);
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerId == sendPlayerID)
            {
                sendPlayerObj = teenPattiPlayers[i].fillLine.gameObject;
            }
            else if (teenPattiPlayers[i].playerId == receivePlayerId)
            {
                receivePlayerObj = teenPattiPlayers[i].fillLine.gameObject;
            }
        }

        GameObject giftGen = Instantiate(giftPrefab, giftParentObj.transform);

        for (int i = 0; i < giftBoxes.Count; i++)
        {
            if (i == giftNo)
            {
                giftGen.transform.GetComponent<Image>().sprite = giftBoxes[i].giftSprite;
            }
        }
        giftGen.transform.position = sendPlayerObj.transform.position;
        giftGen.transform.DOMove(receivePlayerObj.transform.position, 0.4f).OnComplete(() =>
         {
             giftGen.transform.DOMove(receivePlayerObj.transform.position, 1f).OnComplete(() =>
             {

                 giftGen.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
                  {
                      Destroy(giftGen);
                  });

             });
         });



        //if (playerID.Equals(DataManager.Instance.playerData._id))
        //{
        //    TypeMessageBox typeMessageBox = Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
        //    typeMessageBox.Update_Message_Box(msg);
        //}
        //else
        //{
        //    TypeMessageBox typeMessageBox = Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
        //    typeMessageBox.Update_Message_Box(msg);
        //}
        //Canvas.ForceUpdateCanvases();
    }


    void CreateAdmin()
    {
        Debug.Log("joinPlayerDatas   => " + DataManager.Instance.joinPlayerDatas[0].userId + " DataManager.Instance.playerData._id  " + DataManager.Instance.playerData._id);
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            Debug.Log("joinPlayerDatas   => " + DataManager.Instance.joinPlayerDatas[0].userId + " DataManager.Instance.playerData._id  " + DataManager.Instance.playerData._id);
            Debug.Log("is adi => " + isAdmin);
            isAdmin = true;
            Debug.Log("DataManager.Instance.joinPlayerDatas[0]     =  " + DataManager.Instance.joinPlayerDatas[0].userName);
            Debug.Log("is adi => " + isAdmin);
            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        //Greejesh Set PlayerNo
        //int nameCnt = 1;
        //for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        //{
        //    if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
        //    {
        //        playerNo = (i + 1);
        //        player1.playerNo = (i + 1);
        //    }
        //    else
        //    {
        //        if (nameCnt == 1)
        //        {
        //            player2.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //        else if (nameCnt == 2)
        //        {
        //            player3.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //        else if (nameCnt == 3)
        //        {
        //            player4.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //        else if (nameCnt == 4)
        //        {
        //            player5.playerNo = (i + 1);
        //            nameCnt++;
        //        }
        //    }
        //}
    }
    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        int dealerNo = 0;

        dealerNo = UnityEngine.Random.Range(1, DataManager.Instance.joinPlayerDatas.Count + 1);

        obj.AddField("DeckNo", UnityEngine.Random.Range(0, 300));
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("DeckNo2", dealerNo);
        //obj.AddField("DeckNo", 154);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 8);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int deckNo, int dealerNo, string playerId)
    {
        //if (playerId != DataManager.Instance.playerData._id) return;
        //print("Deck no : " + deckNo);`
        mainList = listStoreDatas[deckNo].noList;
        gameDealerNo = dealerNo;
        currentPlayer = dealerNo;
        // changing card sprite to default
        foreach (var t in teenPattiPlayers)
        {
            t.CardGenerate();
        }

        if (isAdmin) return;
        Debug.Log("waitNextRoundScreenObj   active  = > " + waitNextRoundScreenObj.activeSelf);
        if (waitNextRoundScreenObj.activeSelf)
        {
            waitNextRoundScreenObj.SetActive(false);
        }

        //MainMenuManager.Instance.CheckPlayers();

        Debug.LogError("StartGamePlay");
        StartGamePlay();

    }




    public void ChangePlayerTurn(int pNo)
    {
        Debug.Log("---- Change Turn  ------- " + pNo);
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("Action", "ChangePlayerTurn");
        TestSocketIO.Instace.Senddata("TeenPattiChangeTurnData", obj);
    }


    public void SlideShowSendSocket(string slideShowCancelPlayerID, string slideShowPlayerID, string type)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("SlideShowCancelPlayerId", slideShowCancelPlayerID);
        obj.AddField("SlideShowPlayerId", slideShowPlayerID);
        obj.AddField("SlideShowType", type);
        obj.AddField("Action", "SideShowRequest");
        TestSocketIO.Instace.Senddata("TeenPattiSlideShowData", obj);
    }


    public void SendTeenPattiBet(int pNo, float amount, string betType, string playerSlideShowSend, string playerIdSlideShow)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("BetAmount", amount);
        obj.AddField("BetType", betType);
        obj.AddField("currentIndex", currentPriceIndex);
        obj.AddField("currentPrice", currentPriceValue);
        obj.AddField("playerSlideShowSendId", playerSlideShowSend);
        obj.AddField("playerIdSlideShowId", playerIdSlideShow);
        obj.AddField("Action", "PlaceBet");
        TestSocketIO.Instace.Senddata("TeenPattiSendBetData", obj);
        Debug.LogError("mahadeV");
    }


    public void SetTeenPattiWon(string winnerPlayerId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("WinnerPlayerId", winnerPlayerId);
        //obj.AddField("WinnerList", value);
        obj.AddField("Action", "WinData");
        TestSocketIO.Instace.Senddata("TeenPattiWinnerData", obj);
    }

    public void ChangeCardStatus(string value, int pno)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pno);
        obj.AddField("CardStatus", value);
        obj.AddField("Action", "CardStatus");

        //player1.isSeen = true;
        Debug.Log("<color=yellow> TeenPattiChangeCardStatus SEND  </color> ");
        TestSocketIO.Instace.Senddata("TeenPattiChangeCardStatus", obj);
    }

    bool isCheckTurnPack(int nextPlayerNo)
    {
        for (int i = 0; i < playerSquList.Count; i++)
        {
            Debug.Log("I =>  " + i + "  playerSquList[i].gameObject.activeInHierarchy  => " + playerSquList[i].gameObject.activeInHierarchy + "  playerSquList[i].playerNo = " + playerSquList[i].playerNo + "   nextPlayerNo = > " + nextPlayerNo + "   playerSquList[i].isPack  =  " + playerSquList[i].isPack);
            if (playerSquList[i].gameObject.activeInHierarchy && playerSquList[i].playerNo == nextPlayerNo && playerSquList[i].isPack == true)
            {
                Debug.Log("RETURN TRUE ");
                return true;
            }
        }
        Debug.Log("RETURN FALSE ");
        return false;
    }


    public void GetPlayerTurn(int playerNo)
    {
        bool isPlayerNotEnter = false;
        int nextPlayerNo = 0;
        //if (playerNo == DataManager.Instance.joinPlayerDatas.Count)//5
        if (playerSquList.Count == playerNo)
        {
            nextPlayerNo = 1;
            roundCounter++;
        }
        else
        {
            nextPlayerNo = playerNo + 1;
            //foreach (var item in playerSquList)
            //{
            //    if(item.playerNo == 5 && )
            //}
        }



        if (nextPlayerNo == 1)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 1;

            }
            else
            {
                nextPlayerNo = 2;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 2;
                }
                else
                {
                    nextPlayerNo = 3;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 3;
                    }
                    else
                    {
                        nextPlayerNo = 4;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 4;
                        }
                        else
                        {
                            nextPlayerNo = 5;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 5;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 2)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 2;
            }
            else
            {
                nextPlayerNo = 3;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 3;
                }
                else
                {
                    nextPlayerNo = 4;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 4;
                    }
                    else
                    {
                        nextPlayerNo = 5;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 5;
                        }
                        else
                        {
                            nextPlayerNo = 1;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 1;
                                roundCounter++;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 3)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 3;
            }
            else
            {
                nextPlayerNo = 4;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 4;
                }
                else
                {
                    nextPlayerNo = 5;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 5;
                    }
                    else
                    {
                        nextPlayerNo = 1;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 1;
                            roundCounter++;
                        }
                        else
                        {
                            nextPlayerNo = 2;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 2;
                            }
                        }
                    }
                }
            }
        }
        else if (nextPlayerNo == 4)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 4;
            }
            else
            {
                nextPlayerNo = 5;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 5;
                }
                else
                {
                    nextPlayerNo = 1;
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 1;
                        roundCounter++;
                    }
                    else
                    {
                        nextPlayerNo = 2;
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 2;
                        }
                        else
                        {
                            nextPlayerNo = 3;
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 3;
                            }
                        }
                    }
                }

            }
        }
        else if (nextPlayerNo == 5)
        {
            Debug.Log("NO  = ? ");
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                Debug.Log("NO  = ? ");
                nextPlayerNo = 5;
            }
            else
            {
                nextPlayerNo = 1;
                Debug.Log("NO  = ? " + nextPlayerNo);
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 1;
                    Debug.Log("NO  = ? " + nextPlayerNo);
                    roundCounter++;
                }
                else
                {
                    nextPlayerNo = 2;
                    Debug.Log("NO  = ? " + nextPlayerNo);
                    if (isCheckTurnPack(nextPlayerNo) == false)
                    {
                        nextPlayerNo = 2;
                        Debug.Log("NO  = ? " + nextPlayerNo);
                    }
                    else
                    {
                        nextPlayerNo = 3;
                        Debug.Log("NO  = ? " + nextPlayerNo);
                        if (isCheckTurnPack(nextPlayerNo) == false)
                        {
                            nextPlayerNo = 3;
                            Debug.Log("NO  = ? " + nextPlayerNo);
                        }
                        else
                        {
                            nextPlayerNo = 4;
                            Debug.Log("NO  = ? " + nextPlayerNo);
                            if (isCheckTurnPack(nextPlayerNo) == false)
                            {
                                nextPlayerNo = 4;
                                Debug.Log("NO  = ? " + nextPlayerNo);
                            }
                        }
                    }
                }
            }
        }
        //nextPlayerNo = playerNo;

        print("Next Player No : " + nextPlayerNo + "");
        currentPlayer = nextPlayerNo;
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].playerNo == nextPlayerNo)
            {
                print("next chance given to :" + playerSquList[i].playerNo + " Teenpattiplayer = " + playerSquList[i].gameObject.name);
                playerSquList[i].RestartFillLine();
                if (playerSquList[i].playerNo == nextPlayerNo && playerSquList[i] == player1)
                {

                    ShowTextChange();
                    player1.GetAdjacentPlayersPrice(nextPlayerNo, out float currentPrice, out int priceIndex);
                    foreach (var item in playerSquList)
                    {
                        if (item.playerNo == playerNo && item.isSeen && player1.isBlind && item.isBot)//if bot has seen
                        {
                            currentSeenValue = currentPriceValue;
                            //currentSeenValue = currentPriceValue;
                            currentPriceValue = currentBlindValue;
                            for (int j = 0; j < numbers.Length; j++)
                            {
                                if (currentPriceValue == numbers[j])
                                {
                                    currentPriceIndex = j;
                                    runningPriceIndex = j;
                                }
                            }
                            break;
                        }
                        else if (item.playerNo == playerNo && item.isBot && item.isBlind && player1.isSeen)//if bot is blind
                        {
                            currentPriceValue = currentPriceValue + 3;
                            for (int j = 0; j < numbers.Length; j++)
                            {
                                if (currentPriceValue <= numbers[j])
                                {
                                    currentPriceValue = numbers[j];
                                    currentPriceIndex = j;
                                    runningPriceIndex = j;
                                    currentSeenValue = currentPriceValue;
                                    break;
                                }
                            }
                            break;
                        }
                        else if (item.playerNo == playerNo && player1.isBlind && item.isPack)
                        {
                            currentSeenValue = currentPriceValue;
                            //currentSeenValue = currentPriceValue;
                            currentPriceValue = currentBlindValue;
                        }
                    }
                    //currentPriceValue = currentPrice;
                    //currentPriceIndex = priceIndex;
                    priceBtnTxt.text = player1.isSeen ? "Chaal : " + currentPriceValue : "Blind : " + currentPriceValue;
                    priceBtnTxtDouble.text = player1.isSeen ? "Chaal : " + currentPriceValue * 2 : "Blind : " + currentPriceValue * 2;
                    bottomBox.SetActive(true);
                    DataManager.Instance.UserTurnVibrate();
                    IsBoTShowRound4ENd();

                    EnableSeeCards();
                }
                else
                {
                    bottomBox.SetActive(false);
                }

            }
            else
            {
                playerSquList[i].NotATurn();
            }
        }
    }
    public void IsBoTShowRound4ENd()
    {
        for (int i = 0; i < playerSquList.Count; i++)
        {
            Debug.Log("isBot => " + playerSquList[i].isBot + "  PLAYER => " + playerSquList[i].name + "  roundCounter  => " + roundCounter);
            if (playerSquList[i].isBot && roundCounter == 4)
            {
                Debug.Log("IS BLIND => " + playerSquList[i].isBlind);
                if (playerSquList[i].isBlind)
                {
                    Debug.Log("ChangeCardStatus SEEN  =>  " + (i + 1));
                    ChangeCardStatus("SEEN", (i + 1));
                }
            }
        }

    }
    //public void GetPlayerTurn(int playerNo)
    //{

    //    int nextPlayerNo = 0;
    //    if (playerNo == 1)
    //    {
    //        nextPlayerNo = 2;


    //    }
    //    else if (playerNo == 2)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 2)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //        else
    //        {
    //            bool isTurnClose = false;
    //            for (int i = 0; i < teenPattiPlayers.Count; i++)
    //            {
    //                if (teenPattiPlayers[i].playerNo == 3 && teenPattiPlayers[i].isPack == true)
    //                {
    //                    isTurnClose = true;
    //                }
    //            }
    //            if (isTurnClose == false)
    //            {
    //                nextPlayerNo = 3;
    //            }
    //            else
    //            {
    //                nextPlayerNo = 1;
    //            }
    //        }
    //    }
    //    else if (playerNo == 3)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 3)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //        else
    //        {
    //            bool isTurnClose = false;
    //            for (int i = 0; i < teenPattiPlayers.Count; i++)
    //            {
    //                if (teenPattiPlayers[i].playerNo == 4 && teenPattiPlayers[i].isPack == true)
    //                {
    //                    isTurnClose = true;
    //                }
    //            }
    //            if (isTurnClose == false)
    //            {
    //                nextPlayerNo = 4;
    //            }
    //            else
    //            {
    //                nextPlayerNo = 1;
    //            }
    //        }
    //    }
    //    else if (playerNo == 4)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 4)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //        else
    //        {
    //            bool isTurnClose = false;
    //            for (int i = 0; i < teenPattiPlayers.Count; i++)
    //            {
    //                if (teenPattiPlayers[i].playerNo == 5 && teenPattiPlayers[i].isPack == true)
    //                {
    //                    isTurnClose = true;
    //                }
    //            }
    //            if (isTurnClose == false)
    //            {
    //                nextPlayerNo = 5;
    //            }
    //            else
    //            {
    //                nextPlayerNo = 1;
    //            }
    //        }
    //    }
    //    else if (playerNo == 5)
    //    {
    //        if (DataManager.Instance.joinPlayerDatas.Count == 5)
    //        {
    //            nextPlayerNo = 1;
    //        }
    //    }



    //    //nextPlayerNo = playerNo;
    //    for (int i = 0; i < teenPattiPlayers.Count; i++)
    //    {
    //        if (teenPattiPlayers[i].playerNo == nextPlayerNo)
    //        {
    //            teenPattiPlayers[i].RestartFillLine();
    //            if (teenPattiPlayers[i].playerNo == player1.playerNo)
    //            {
    //                bottomBox.SetActive(true);
    //            }
    //            else
    //            {
    //                bottomBox.SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            teenPattiPlayers[i].NotATurn();
    //        }
    //    }
    //}

    public void CreditWinnerAmount(string playerID)
    {
        float winnerAmount = (float)totalBetAmount;

        //print("Win No : " + winnerNo[i]);
        for (int j = 0; j < teenPattiPlayers.Count; j++)
        {
            if (teenPattiPlayers[j].playerId == playerID && teenPattiPlayers[j].gameObject.activeSelf == true)
            {
                Debug.Log("CreditWinnerAmount  ");
                float adminPercentage = DataManager.Instance.adminPercentage;
                float winAmount = winnerAmount;
                float adminCommssion = (adminPercentage / 100);
                float playerWinAmount = winAmount - (winAmount * adminCommssion);

                // Generate Number
                GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
                genBetObj.transform.GetChild(1).GetComponent<Text>().text = playerWinAmount.ToString("F2"); // Display with 2 decimal places
                genBetObj.transform.position = targetBetObj.transform.position;
                totalBetAmount = 0;

                // Animate the bet object to the target position
                genBetObj.transform.DOMove(teenPattiPlayers[j].sendBetObj.transform.position, 0.3f).OnComplete(() =>
                {
                    // Parse current balance, add the win amount, and update the UI text

                });

                float currentBalance = float.Parse(teenPattiPlayers[j].playerBalence.text);
                float newBalance = currentBalance + playerWinAmount;
                Debug.Log("WIN AMOU   => " + playerWinAmount);
                Debug.Log("Player Name => " + teenPattiPlayers[j].name);
                Debug.Log("Player Bal => " + teenPattiPlayers[j].playerBalence.text);
                teenPattiPlayers[j].playerBalence.text = newBalance.ToString("F2"); // Update balance with 2 decimal formatting
                Debug.Log("Player Bal => " + teenPattiPlayers[j].playerBalence.text);

                // Update the player's balance in the DataManager instance
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (teenPattiPlayers[j].playerId == DataManager.Instance.joinPlayerDatas[i].userId)
                    {
                        Debug.Log("Player Name => " + DataManager.Instance.joinPlayerDatas[i].userName);

                        Debug.Log("Player BALLLL => " + DataManager.Instance.joinPlayerDatas[i].balance);
                        DataManager.Instance.joinPlayerDatas[i].balance = newBalance.ToString("F2");
                        Debug.Log("Player BALLLL => " + DataManager.Instance.joinPlayerDatas[i].balance);
                    }
                }
                // Happening outside Dotween animation
                if (teenPattiPlayers[j].playerNo == player1.playerNo)
                {
                    //Add to  winnner Amount



                    print(playerWinAmount + "<-------- Crediting amount Outside animation");

                    if (playerWinAmount != 0)
                    {
                        SoundManager.Instance.CasinoWinSound();
                        winAnimationTxt.gameObject.SetActive(true);
                        winAnimationTxt.text = "+" + playerWinAmount;
                        Invoke(nameof(WinAmountTextOff), 1.5f);
                        DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "TeenPatti-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
                    }
                }


                Destroy(genBetObj, 0.4f);
            }
        }


        Invoke(nameof(GameRestartRound), 0.4f);
    }

    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);

    }
    const float epsilon = 0.0001f;

    public void GetBet(int playerNo, float amount, string type, string playerSlideShowSendId, string playerIdSlideShowId, int curIndex, int curPrice)
    {
        print("Got Bet for player =" + playerNo);
        if (type == "Show")
        {
            //ShowCardToAllUser("Show", true);
        }
        else if (type == "SideShow")
        {
            //print("Enter The First Slide Show");
            if (playerSlideShowSendId.Equals(player1.playerId) && !playerIdSlideShowId.Equals(player1.playerId))
            {
                print("Enter The Second Side Show");

                slideShowPanel.SetActive(true);
                TeenPattiSlideShow.Instance.sendId = playerSlideShowSendId;
                TeenPattiSlideShow.Instance.currentId = playerIdSlideShowId;
                foreach (var item in DataManager.Instance.joinPlayerDatas)
                {
                    if (playerIdSlideShowId == item.userId)
                    {
                        slideShowName.text = item.userName;
                        StartCoroutine(DataManager.Instance.GetImages(item.avtar, slideShowProfilePic));
                        break;
                    }
                }
            }
        }
        int playerIndex = 0;
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerNo == playerNo)
            {
                playerIndex = i;
            }
        }

        bool isB = false;
        bool isS = false;
        if (type != "SideShow")
        {
            BetAnim(teenPattiPlayers[playerIndex], amount, curIndex);
            currentPriceIndex = curIndex;
            /*if (Mathf.Abs(amount - 10f) < epsilon)
            {
                //blindx2button.SetActive(false);
                //plusBtn.gameObject.SetActive(true);
                //minusBtn.gameObject.SetActive(true);
                //priceBtnTxt.gameObject.transform.parent.transform.localPosition = new Vector3(490.00f, 90.81f, 0.00f);
            }*/
        }
        if (teenPattiPlayers[playerIndex].isBlind)
        {
            teenPattiPlayers[playerIndex].blindIMG.SetActive(true);
            isB = true;
        }
        else if (teenPattiPlayers[playerIndex].isSeen)
        {
            teenPattiPlayers[playerIndex].seenImg.SetActive(true);
            isS = true;
        }
        //currentPriceValue = amount;
        if (!player1.isPack && player1.isBlind)
        {
            if (isS)
            {
                currentSeenValue = curPrice;
                if (player1.isTurn)
                    currentPriceValue = currentBlindValue;
                //currentPriceValue /= 2;
                //currentPriceValue = minLimitValue;
                //currentPriceValue = curPrice / 2;
                //for (int i = numbers.Length - 1; i >= 0; i--)
                //{
                //    if (currentPriceValue >= numbers[i])
                //    {
                //        currentPriceIndex = i;
                //        runningPriceIndex = i;
                //        break;
                //    }
                //}
            }
            else if (isB)
            {
                currentPriceValue = curPrice;
                currentBlindValue = curPrice;
                currentSeenValue = curPrice + 3;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (currentSeenValue <= numbers[i])
                    {
                        currentSeenValue = numbers[i];
                        break;
                    }
                }
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (currentPriceValue == numbers[i])
                    {
                        currentPriceIndex = i;
                        runningPriceIndex = i;
                        break;
                    }
                }
                /*currentPriceValue = amount;
                minLimitValue = amount;
                if(minLimitValue > 5)
                    priceBtnTxt.transform.parent.gameObject.GetComponent<Button>().interactable = false;*/
            }
            priceBtnTxt.text = "Blind : " + curPrice;
            priceBtnTxtDouble.text = "Blind : " + curPrice * 2;
        }
        else if (!player1.isPack && player1.isSeen)
        {
            if (isS)
            {
                currentSeenValue = curPrice;
                /*currentPriceValue = amount;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if(currentPriceValue == numbers[i])
                    {
                        currentPriceIndex = i;
                        runningPriceIndex = i;
                        break;
                    }
                }*/
                currentPriceValue = curPrice;
            }
            else if (isB)
            {
                currentBlindValue = curPrice;
                currentPriceValue = curPrice + 3;
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (currentPriceValue <= numbers[i])
                    {
                        currentPriceIndex = i;
                        runningPriceIndex = i;
                        currentPriceValue = numbers[i];
                        break;
                    }
                }
                currentSeenValue = currentPriceValue;
                //currentPriceValue = curPrice;
            }
            priceBtnTxt.text = "Chaal : " + curPrice;
            priceBtnTxtDouble.text = "Chaal : " + curPrice * 2;
        }
    }


    public void SlideShow_Accpet_Socket(string playerId1, string playerId2)
    {
        if (DataManager.Instance.playerData._id.Equals(playerId1) || DataManager.Instance.playerData._id.Equals(playerId2))
        {
            isPopupOpen = true;
            StartCoroutine(CheckSlideShowWinner(playerId1, playerId2, true));
        }

    }

    public void SlideShow_Cancel_Socket()
    {
        ChangePlayerTurn(player1.playerNo);
    }

    public void GetCardStatus(string value, int playerNo)
    {
        //print("Card Status : " + value + "    Player No  :" + playerNo);
        for (int i = 0; i < playerSquList.Count; i++)
        {

            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].playerNo == playerNo)
            {
                Debug.Log("VALUE =>  " + value);
                if (value.Equals("SEEN"))
                {
                    playerSquList[i].isSeen = true;
                    playerSquList[i].isBlind = false;
                    playerSquList[i].isPack = false;
                    ShowTextChange();
                    //ShowTextChange(teenPattiPlayers[i]);

                    playerSquList[i].seenImg.SetActive(true);
                    playerSquList[i].blindIMG.SetActive(false);
                    playerSquList[i].packImg.SetActive(false);
                }
                else if (value.Equals("Blind"))
                {
                    playerSquList[i].isSeen = false;
                    playerSquList[i].isBlind = true;
                    playerSquList[i].isPack = false;
                    ShowTextChange();
                    //ShowTextChange(teenPattiPlayers[i]);

                    playerSquList[i].blindIMG.SetActive(true);
                    playerSquList[i].seenImg.SetActive(false);
                    playerSquList[i].packImg.SetActive(false);
                }
                else if (value.Equals("PACK"))
                {
                    playerSquList[i].isPack = true;
                    playerSquList[i].isBlind = false;
                    playerSquList[i].isSeen = false;
                    for (int j = 0; j < playerSquList[i].seeObj.Length; j++)
                    {
                        playerSquList[i].seeObj[j].SetActive(false);
                    }
                    playerSquList[i].packImg.SetActive(true);
                    playerSquList[i].seenImg.SetActive(false);
                    playerSquList[i].blindIMG.SetActive(false);
                    //CheckWin();
                    CheckPackTime(playerSquList[i]);
                    // Greejesh Pack Check



                }
            }
        }


    }

    public bool isWinningRun = false;
    void CheckPackTime(TeenPattiPlayer packPlayer)
    {
        print("Enter The Check Player");
        List<TeenPattiPlayer> livePlayers = new List<TeenPattiPlayer>();
        print(teenPattiPlayers.Count);
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].isPack == false && teenPattiPlayers[i].gameObject.activeInHierarchy)
            {
                livePlayers.Add(teenPattiPlayers[i]);
            }
        }
        print("livePlayers.Count : " + livePlayers.Count);
        //  packPlayer.isTurn = false;
        print("NAME : " + packPlayer.name);

        Debug.Log("isWinningRun  =>  " + isWinningRun);
        if (livePlayers.Count == 1 && !isWinningRun)
        {
            livePlayers[0].isTurn = false;
            string winValue = ",";
            winValue += livePlayers[0].playerNo + ",";
            Debug.Log("livePlayers[0].playerNo  =>  " + livePlayers[0].playerNo + "  playerNo  => " + playerNo);
            Debug.Log("WIN AMOUNT =>  " + winValue);
            if (livePlayers[0].playerNo == playerNo || livePlayers[0].isBot)
            {
                SetTeenPattiWon(livePlayers[0].playerId);
                isWinningRun = true;
                Debug.LogWarning("------------------won is called-------------------------------------");
                CreditWinnerAmount(livePlayers[0].playerId);
            }
            foreach (var t in livePlayers[0].playerWinObj)
            {
                t.SetActive(true);
            }

            /*  Debug.Log("------ RestartGamePlay ---- ");
              StartCoroutine(RestartGamePlay());*/
        }
        else
        {
            Debug.Log("IS ADMIN ===   " + isAdmin);
            if (isAdmin)
            {
                ChangePlayerTurn(packPlayer.playerNo);
            }
        }
    }


    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {

        /*bool isAdminLeave = false;
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerId.Equals(leavePlayerId) && teenPattiPlayers[i].playerNo == 1)
            {
                isAdminLeave = true;
            }
        }*/

        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].playerId.Equals(leavePlayerId))
            {
                // Log when a player is found with matching playerId
                Debug.Log($"Player with ID: {leavePlayerId} found at index {i}");

                teenPattiPlayers[i].isPack = true;
                teenPattiPlayers[i].isBlind = false;
                teenPattiPlayers[i].isSeen = false;

                // Log state changes of the player
                Debug.Log($"Player {teenPattiPlayers[i].playerNameTxt.text} is now packed. Blind: {teenPattiPlayers[i].isBlind}, Seen: {teenPattiPlayers[i].isSeen}");

                for (int j = 0; j < teenPattiPlayers[i].seeObj.Length; j++)
                {
                    teenPattiPlayers[i].seeObj[j].SetActive(false);
                    // Log the deactivation of see objects
                    Debug.Log($"Deactivated seeObj[{j}] for player {teenPattiPlayers[i].playerNameTxt.text}");
                }

                teenPattiPlayers[i].packImg.SetActive(true);
                // Log the activation of the pack image
                Debug.Log($"Activated pack image for player {teenPattiPlayers[i].playerNameTxt.text}");

                // Start coroutine to remove player with log
                Debug.Log($"Starting coroutine to remove player {teenPattiPlayers[i].playerNameTxt.text}");
                StartCoroutine(WaitGameToCompleteRemovePlayer(CheckLeftPlayer, i));

                // Log the pack time check
                Debug.Log($"Checking pack time for player {teenPattiPlayers[i].playerNameTxt.text}");
                CheckPackTime(teenPattiPlayers[i]);
                Debug.Log("IS WINNING =>  " + isWinningRun);
                if (teenPattiPlayers[i].isTurn && !isWinningRun)
                {
                    // Log the change of turn if it's the current player's turn
                    Debug.Log($"It was {teenPattiPlayers[i].playerNameTxt.text}'s turn. Changing to the next player.");
                    ChangePlayerTurn(teenPattiPlayers[i].playerNo);
                }

                Debug.Log($"teenPattiPlayers[i].isTurn  {teenPattiPlayers[i].isTurn}");
            }
        }

        /*if (isAdminLeave)
        {
            if (player1.playerId.Equals(adminId))
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
        }*/

        /*for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].gameObject.activeSelf)
            {
                string playerIdGet = teenPattiPlayers[i].playerId;

                bool isEnter = false;
                for (int j = 0; j < DataManager.Instance.joinPlayerDatas.Count; j++)
                {
                    if (playerIdGet.Equals(DataManager.Instance.joinPlayerDatas[j].userId))
                    {

                        isEnter = true;
                    }
                }
                
                if (isEnter == false)
                {
                    teenPattiPlayers[i].gameObject.SetActive(false);
                    teenPattiPlayers.RemoveAt(i);
                    teenPattiPlayers[i].isPack = true;
                    teenPattiPlayers[i].isBlind = false;
                    teenPattiPlayers[i].isSeen = false;
                    teenPattiPlayers[i].packImg.SetActive(true);
                    CheckPackTime(teenPattiPlayers[i]);
                }
            }
        }*/
        Debug.Log("playerData  = " + DataManager.Instance.playerData._id + "   joinPlayerDatas  = " + DataManager.Instance.joinPlayerDatas[0].userId);
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            Debug.Log("ISADMIN   =" + isAdmin);
            isAdmin = true;
            Debug.Log("joinPlayerDatas => " + DataManager.Instance.joinPlayerDatas.Count + "   waitNextRoundScreenObj  > " + waitNextRoundScreenObj.activeSelf);
            if (DataManager.Instance.joinPlayerDatas.Count == 5 && waitNextRoundScreenObj.activeSelf)
            {
                DataManager.Instance.joinPlayerDatas.RemoveAt(0);
                //RoundGenerate();
                CheckJoinedPlayers();
                StartGamePlay();
                Debug.LogError("StartGamePlay");

                if (waitNextRoundScreenObj.activeSelf)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
                ResetBot();
                ActivateBotPlayers();
            }
        }
        else
        {
            Debug.Log("isGameStarted   =>  " + isGameStarted);
            if (!isGameStarted)
            {
                isAdmin = false;
            }

            Debug.Log("Count   =>  " + DataManager.Instance.joinPlayerDatas.Count);
            if (DataManager.Instance.joinPlayerDatas.Count < 6) return;
            int index = DataManager.Instance.joinPlayerDatas.FindIndex(leftPlayer => leftPlayer.userId == leavePlayerId);
            DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[index]);
        }
    }

    public void CheckJoinedPlayers()
    {
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://139.84.132.115/assets/img/profile-picture/")).ToList();
        // assiging new remaining bot players
        Debug.Log("Count   =>  " + DataManager.Instance.joinPlayerDatas.Count);
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
    }

    #endregion

    #region CheckWin

    public IEnumerator CheckSlideShowWinner(string playerId1, string playerId2, bool isSocket)
    {
        int cnt = 0;

        //List<TeenPattiPlayer> teenSlideShowPlayers = new List<TeenPattiPlayer>();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (/*isSocket &&*/ teenPattiPlayers[i].gameObject.activeSelf == true && teenPattiPlayers[i].isPack == false && (teenPattiPlayers[i].playerId.Equals(playerId1) || teenPattiPlayers[i].playerId.Equals(playerId2)))
            {
                cnt++;
                teenPattiPlayers[i].CardDisplay();
                //teenSlideShowPlayers.Add(teenPattiPlayers[i]);
            }
        }
        if (CheckMoney(currentPriceValue) == false)
        {
            SoundManager.Instance.ButtonClick();
            Debug.Log("OpenErrorScreen");

            OpenErrorScreen();
            yield break;
        }
        if (isSocket)
        {
            BetAnim(player1, currentPriceValue, currentPriceIndex);
            DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 1);
            playerBetAmount += currentPriceValue;
        }
        yield return new WaitForSeconds(0.75f);

        /*if (teenSlideShowPlayers.Count == 2)
        {
            TeenPattiPlayer slidePlayer1 = teenPattiPlayers[0];
            TeenPattiPlayer slidePlayer2 = teenPattiPlayers[1];

            if (slidePlayer1.ruleNo < slidePlayer2.ruleNo)
            {
                if (isSocket)
                {
                    ChangeCardStatus("PACK", slidePlayer2.playerNo);
                    ChangePlayerTurn(slidePlayer2.playerNo);
                }
                //pack slideplayer2
            }
            else if (slidePlayer2.ruleNo < slidePlayer1.ruleNo)
            {
                //pack slideplayer1
                if (isSocket)
                {
                    ChangeCardStatus("PACK", slidePlayer1.playerNo);
                    ChangePlayerTurn(slidePlayer1.playerNo);
                }
            }
            else if (slidePlayer2.ruleNo == slidePlayer1.ruleNo)
            {
                if (slidePlayer1.ruleNo == 1)
                {
                    if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 2
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                    else if (slidePlayer2.card1.cardNo > slidePlayer1.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer1.playerNo);
                        }
                    }
                    else if (slidePlayer1.card1.cardNo == slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                }
                else if (slidePlayer1.ruleNo == 5)
                {
                    if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 2
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                    else if (slidePlayer2.card1.cardNo > slidePlayer1.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer1.playerNo);
                        }
                    }
                    else if (slidePlayer1.card1.cardNo == slidePlayer2.card1.cardNo)
                    {

                        if (slidePlayer1.card3.cardNo > slidePlayer2.card3.cardNo)
                        {
                            //pack slide player 2
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer2.playerNo);
                                ChangePlayerTurn(slidePlayer2.playerNo);
                            }
                        }
                        else if (slidePlayer2.card3.cardNo > slidePlayer1.card3.cardNo)
                        {
                            //pack slide player 1
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                ChangePlayerTurn(slidePlayer1.playerNo);
                            }
                        }
                        else if (slidePlayer1.card3.cardNo == slidePlayer2.card3.cardNo)
                        {
                            //pack slide player 1
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                ChangePlayerTurn(slidePlayer1.playerNo);
                            }
                        }
                    }
                }
                else
                {
                    int highestNo1 = 0;
                    if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 2
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
                            ChangePlayerTurn(slidePlayer2.playerNo);
                        }
                    }
                    else if (slidePlayer1.card1.cardNo < slidePlayer2.card1.cardNo)
                    {
                        //pack slide player 1
                        if (isSocket)
                        {
                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
                            ChangePlayerTurn(slidePlayer1.playerNo);
                        }
                    }
                    else
                    {
                        if (slidePlayer1.card2.cardNo > slidePlayer2.card2.cardNo)
                        {
                            //pack slide player 2
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer2.playerNo);
                                ChangePlayerTurn(slidePlayer2.playerNo);
                            }
                        }
                        else if (slidePlayer1.card2.cardNo < slidePlayer2.card2.cardNo)
                        {
                            //pack slide player 1
                            if (isSocket)
                            {
                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                ChangePlayerTurn(slidePlayer1.playerNo);
                            }
                        }
                        else
                        {
                            if (slidePlayer1.card3.cardNo > slidePlayer2.card3.cardNo)
                            {
                                //pack slide player 2
                                if (isSocket)
                                {
                                    ChangeCardStatus("PACK", slidePlayer2.playerNo);
                                    ChangePlayerTurn(slidePlayer2.playerNo);
                                }
                            }
                            else if (slidePlayer1.card3.cardNo < slidePlayer2.card3.cardNo)
                            {
                                //pack slide player 1
                                if (isSocket)
                                {
                                    ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                    ChangePlayerTurn(slidePlayer1.playerNo);
                                }
                            }
                            else if (slidePlayer1.card3.cardNo == slidePlayer2.card3.cardNo)
                            {
                                //pack slide player 1
                                if (isSocket)
                                {
                                    ChangeCardStatus("PACK", slidePlayer1.playerNo);
                                    ChangePlayerTurn(slidePlayer1.playerNo);
                                }
                            }
                        }
                    }

                }

            }
        }*/

    }


    public void ShowCardToAllUser()
    {
        winMaintain.Clear();
        foreach (var t in teenPattiPlayers.Where(t => t.gameObject.activeSelf == true && (t.isSeen || t.isBlind) && t.isPack == false))
        {
            t.CardDisplay();
        }
        //CheckFinalWinner(type);
        bottomBox.SetActive(false);
    }

    public string CheckFinalWinner(string type)
    {
        List<TeenPattiPlayer> teenPattiWinner = new List<TeenPattiPlayer>();

        foreach (var t in teenPattiPlayers)
        {
            t.seenImg.SetActive(false);
        }

        /*List<TeenPattiPlayer> teenPattiWinner = teenPattiPlayers.Where(p => p.gameObject.activeSelf && (p.isSeen || p.isBlind) && !p.isPack).OrderByDescending(p => p.ruleNo).ThenByDescending(p => p.isBot && p.ruleNo == 6).ToList();

        if (teenPattiWinner.Count > 0)
        {
            TeenPattiPlayer winner = teenPattiWinner[0];
            teenPattiWinner.Clear();
            teenPattiWinner.Add(winner);
        }
        else
        {
            teenPattiWinner.Clear();
        }*/

        foreach (var player in teenPattiPlayers)
        {
            player.SumOfPlayerCards();
        }

        List<TeenPattiPlayer> sortedNumbersDescending = teenPattiPlayers.OrderByDescending(n => n.sumOfCards).ToList();

        for (int i = 0; i < sortedNumbersDescending.Count; i++)
        {
            //calculate sum of 3 card value and store in a varible
            if (sortedNumbersDescending[i].gameObject.activeSelf == true && (sortedNumbersDescending[i].isSeen || sortedNumbersDescending[i].isBlind) && sortedNumbersDescending[i].isPack == false)
            {
                bool isEnter = false;
                for (int j = 0; j < teenPattiWinner.Count; j++)
                {
                    if (teenPattiWinner[j].ruleNo > sortedNumbersDescending[i].ruleNo)
                    {
                        isEnter = true;
                    }
                }
                // Highest rule number player will be added to teenpattiwinner list
                if (isEnter == true)
                {
                    teenPattiWinner.Clear();

                    teenPattiWinner.Add(sortedNumbersDescending[i]);
                    //print("Clear");
                }
                else if (teenPattiWinner.Count == 0)
                {
                    teenPattiWinner.Add(sortedNumbersDescending[i]);

                }//print("Add");
            }
        }


        /*for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].gameObject.activeSelf == true && (teenPattiPlayers[i].isSeen || teenPattiPlayers[i].isBlind) && teenPattiPlayers[i].isPack == false)
            {
                bool isEnter = false;
                for (int j = 0; j < teenPattiWinner.Count; j++)
                {
                    if (teenPattiWinner[j].ruleNo > teenPattiPlayers[i].ruleNo)
                    {
                        isEnter = true;
                    }
                }
                if (isEnter == true)
                {
                    teenPattiWinner.Clear();

                    teenPattiWinner.Add(teenPattiPlayers[i]);
                    //print("Clear");
                }
                else if (teenPattiWinner.Count == 0)
                {
                    teenPattiWinner.Add(teenPattiPlayers[i]);

                }//print("Add");
            }
        }*/
        Debug.Log("------ ShowWinPlayer ---- ");

        // ShowWinPlayer(type, teenPattiWinner);

        CreditWinnerAmount(teenPattiWinner[0].playerId);
        SetTeenPattiWon(teenPattiWinner[0].playerId);
        return teenPattiWinner[0].playerId;
    }

    public void HandelTeenPattiWinData(string winnerPlayerId)
    {
        List<TeenPattiPlayer> winnerPlayer = teenPattiPlayers.Where(p => p.playerId == winnerPlayerId).ToList();

        if (winnerPlayer.Count > 0)
        {
            ShowCardToAllUser();
            Debug.Log("------ ShowWinPlayer ---- ");
            CreditWinnerAmount(winnerPlayerId);

            ShowWinPlayer("Show", winnerPlayer);
        }
    }

    public void ShowWinPlayer(string type, List<TeenPattiPlayer> teenPattiWinner)
    {
        isBotActivate = false;
        if (teenPattiWinner.Count == 1)
        {
            int rule = teenPattiWinner[0].ruleNo;
            string winValue = ",";
            winValue += teenPattiWinner[0].playerNo + ",";
            if (teenPattiWinner[0].playerNo == playerNo)
            {
                if (teenPattiWinner[0].playerNo == playerNo)
                {
                    //SetTeenPattiWon(winValue);
                    //Debug.LogWarning("------------------won is called-------------------------------------");
                }
            }

            //print("Rule 1 : " + rule);
            //win
            foreach (var t in teenPattiWinner[0].playerWinObj)
            {
                t.SetActive(true);
            }
            SoundManager.Instance.CasinoWinSound();

            if (isGameStarted)
            {
                Debug.Log("------ RestartGamePlay ---- ");
                StartCoroutine(RestartGamePlay());

            }

        }
        else if (teenPattiWinner.Count > 1)
        {
            /*int rule = teenPattiWinner[0].ruleNo;

            //print("Rule 2 : " + rule);
            switch (rule)
            {
                case 1:
                {
                    int highestNo1 = teenPattiWinner[0].card1.cardNo;
                    highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

                    List<TeenPattiPlayer> playerList1 =
                        teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

                    if (playerList1.Count == 1)
                    {
                        //win
                        string winValue = ",";
                        winValue += playerList1[0].playerNo + ",";
                        if (playerList1[0].playerNo == playerNo)
                        {
                            if (playerList1[0].playerNo == playerNo)
                            {
                                SetTeenPattiWon(winValue); // with player id // Moved in click
                                Debug.LogWarning(
                                    "------------------won is called-------------------------------------");
                            }
                        }

                        foreach (var t in playerList1[0].playerWinObj)
                        {
                            t.SetActive(true);
                        }

                        StartCoroutine(RestartGamePlay());
                    }

                    break;
                }
                case 5:
                {
                    int highestNo1 = teenPattiWinner[0].card1.cardNo;
                    highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

                    List<TeenPattiPlayer> playerList1 =
                        teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

                    if (playerList1.Count == 1)
                    {
                        //win
                        string winValue = ",";
                        winValue += playerList1[0].playerNo + ",";
                        if (playerList1[0].playerNo == playerNo)
                        {
                            SetTeenPattiWon(winValue);
                            Debug.LogWarning("------------------won is called-------------------------------------");
                        }

                        foreach (var t in playerList1[0].playerWinObj)
                        {
                            t.SetActive(true);
                        }

                        StartCoroutine(RestartGamePlay());
                    }
                    else
                    {
                        int highestNo3 = teenPattiWinner[0].card3.cardNo;
                        highestNo3 = teenPattiWinner.Select(t => t.card3.cardNo).Prepend(highestNo3).Max();

                        List<TeenPattiPlayer> playerList3 =
                            teenPattiWinner.Where(t => highestNo3 == t.card3.cardNo).ToList();

                        if (playerList3.Count == 1)
                        {
                            //win
                            string winValue = ",";
                            winValue += playerList3[0].playerNo + ",";
                            if (playerList3[0].playerNo == playerNo)
                            {
                                SetTeenPattiWon(winValue);
                                Debug.LogWarning(
                                    "------------------won is called-------------------------------------");
                            }

                            foreach (var t in playerList3[0].playerWinObj)
                            {
                                t.SetActive(true);
                            }

                            StartCoroutine(RestartGamePlay());
                        }
                        else
                        {
                            //win
                            if (type == "Show")
                            {
                                ChangeCardStatus("PACK", player1.playerNo);
                                //ChangePlayerTurn(player1.playerNo);
                            }
                            else
                            {
                                string winValue = ",";
                                winValue += playerList1[0].playerNo + ",";
                                foreach (var t in playerList3)
                                {
                                    winValue += t.playerNo + ",";
                                    foreach (var t1 in t.playerWinObj)
                                    {
                                        t1.SetActive(true);
                                    }
                                }

                                StartCoroutine(RestartGamePlay());
                                if (playerList3[0].playerNo == playerNo)
                                {
                                    SetTeenPattiWon(winValue);
                                    Debug.LogWarning(
                                        "------------------won is called-------------------------------------");
                                }
                            }
                        }
                    }

                    break;
                }
                default:
                {
                    int highestNo1 = teenPattiWinner[0].card1.cardNo;
                    highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

                    List<TeenPattiPlayer> playerList1 =
                        teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

                    if (playerList1.Count == 1)
                    {
                        //win
                        string winValue = ",";
                        winValue += playerList1[0].playerNo + ",";
                        if (playerList1[0].playerNo == playerNo)
                        {
                            SetTeenPattiWon(winValue);
                            Debug.LogWarning("------------------won is called-------------------------------------");
                        }

                        foreach (var t in playerList1[0].playerWinObj)
                        {
                            t.SetActive(true);
                        }

                        StartCoroutine(RestartGamePlay());
                    }
                    else
                    {
                        int highestNo2 = teenPattiWinner[0].card2.cardNo;
                        highestNo2 = teenPattiWinner.Select(t => t.card2.cardNo).Prepend(highestNo2).Max();

                        List<TeenPattiPlayer> playerList2 =
                            teenPattiWinner.Where(t => highestNo2 == t.card2.cardNo).ToList();

                        if (playerList2.Count == 1)
                        {
                            //win
                            string winValue = ",";
                            winValue += playerList2[0].playerNo + ",";
                            if (playerList2[0].playerNo == playerNo)
                            {
                                SetTeenPattiWon(winValue);
                                Debug.LogWarning(
                                    "------------------won is called-------------------------------------");
                            }

                            foreach (var t in playerList2[0].playerWinObj)
                            {
                                t.SetActive(true);
                            }

                            StartCoroutine(RestartGamePlay());
                        }
                        else
                        {
                            int highestNo3 = teenPattiWinner[0].card3.cardNo;
                            highestNo3 = teenPattiWinner.Select(t => t.card3.cardNo).Prepend(highestNo3).Max();

                            List<TeenPattiPlayer> playerList3 =
                                teenPattiWinner.Where(t => highestNo3 == t.card3.cardNo).ToList();

                            if (playerList3.Count == 1)
                            {
                                //win
                                string winValue = ",";
                                winValue += playerList3[0].playerNo + ",";
                                if (playerList3[0].playerNo == playerNo)
                                {
                                    SetTeenPattiWon(winValue);
                                    Debug.LogWarning(
                                        "------------------won is called-------------------------------------");
                                }

                                foreach (var t in playerList3[0].playerWinObj)
                                {
                                    t.SetActive(true);
                                }

                                StartCoroutine(RestartGamePlay());
                            }
                            else
                            {
                                //win
                                if (type == "Show")
                                {
                                    ChangeCardStatus("PACK", player1.playerNo);
                                    //ChangePlayerTurn(player1.playerNo);
                                }
                                else
                                {
                                    string winValue = ",";
                                    foreach (var t in playerList3)
                                    {
                                        winValue += t.playerNo + ",";
                                        foreach (var t1 in t.playerWinObj)
                                        {
                                            t1.SetActive(true);
                                        }
                                    }

                                    StartCoroutine(RestartGamePlay());
                                    if (playerList3[0].playerNo == playerNo)
                                    {
                                        SetTeenPattiWon(winValue);
                                        Debug.LogWarning(
                                            "------------------won is called-------------------------------------");
                                    }
                                }
                            }
                        }
                    }

                    break;
                }
            }*/
        }
    }

    #endregion

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