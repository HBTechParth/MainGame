using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[System.Serializable]
public class PokerWinDataMaintain
{
    public int ruleNo;
    public List<CardSuffle> winList;

}
public class PokerWinDataWithPlayer
{
    public int ruleNo;
    public List<CardSuffle> winList;
    public PokerPlayer player;

}

public class PokerGameManager : MonoBehaviour
{
    public static PokerGameManager Instance;


    [Header("---Game Bet---")]
    public float sbAmount;
    public float bbAmount;
    public float lastPrice;
    public bool isWin;
    public float timerSpeed;
    public bool isAllIn;

    [Header("--- Bet ---")]
    public GameObject betPrefab;
    public GameObject targetBetObj;
    public float player1BetAmount;
    public float player2BetAmount;
    public float player3BetAmount;
    public float player4BetAmount;
    public float player5BetAmount;

    [Header("---Poker Game UI---")]
    public GameObject errorScreenObj;
    public bool isAdmin;
    public int playerNo;
    public bool isGameStop;
    public GameObject cardTmpPrefab;
    public GameObject prefabParent;
    public GameObject cardTmpStart;
    public GameObject playerFindScreenObj;
    public Sprite simpleCardSprite;
    public Sprite packCardSprite;
    public Text potTxt;
    public float potAmount;
    public float totalBetAmount;
    private int[] numbers = { 5, 10, 50, 100, 250, 500, 1000 };
    private int currentIndex = 0;

    [Header("---Poker Down On Object---")]
    public GameObject downObjectOnObj;
    public GameObject allInOnObj;
    public GameObject flodBtn;
    public GameObject callBtn;
    public GameObject checkbtn;
    public GameObject allInBtn;
    public GameObject raiseBtn;
    public Text callPriceTxt;
    public Text raisePriceTxt;
    public Text allInPriceTxt;
    public float raisePrice = 0;


    [Header("---Second Panel---")]
    public GameObject secondScreenObj;
    public GameObject secondSubScreenObj;
    public GameObject secondUpBtnObj;
    public Slider sliderValue;
    public Button minusBtn;
    public Button plusBtn;

    [Header("---Game Play---")]
    public int gameDealerNo;
    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public List<ListStoreData> listStoreDatas = new List<ListStoreData>();
    public List<int> mainList = new List<int>();
    public List<CardSuffle> cardSufflesGen = new List<CardSuffle>();
    public List<CardSuffle> cardSufflesSort = new List<CardSuffle>();

    public List<CardSuffle> newCardSS = new List<CardSuffle>();
    public List<CardSuffle> newCardSS1 = new List<CardSuffle>();
    public List<CardSuffle> cardResult = new List<CardSuffle>();


    public List<PokerPlayer> pokerPlayers = new List<PokerPlayer>();
    public List<PokerPlayer> playerSquList = new List<PokerPlayer>();
    public PokerPlayer player1;
    public PokerPlayer player2;
    public PokerPlayer player3;
    public PokerPlayer player4;
    public PokerPlayer player5;

    [Header("--- Down Object Off ---")]

    public GameObject waitNextRoundScreenObj;
    public GameObject downObjectOff;
    public GameObject[] tickObj;
    public Image[] blackBtnObj;
    public Sprite blackBtnOn;
    public Sprite blackBtnOff;


    [Header("--- Menu Screen ---")]
    //public GameObject menuScreenObj;
    public GameObject exitPanel;
    public GameObject settingsScreenObj;

    [Header("--- Rule Screen ---")]
    public GameObject ruleScreenObj;

    [Header("--- Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;
    public GameObject lowBalanceError;

    [Header("--- Open Message Screen ---")]
    public GameObject messageScreeObj;
    public GameObject giftScreenObj;

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


    [Header("--- Chat Panel ---")]
    public GameObject chatPanelParent;
    public GameObject chatMePrefab;
    public GameObject chatOtherPrefab;

    [Header("--- Gift Maintain ---")]
    public GameObject giftParentObj;
    public GameObject giftPrefab;
    public List<GiftBox> giftBoxes = new List<GiftBox>();


    [Header("--- Cards Maintain ---")]
    public CardSuffle card1;
    public CardSuffle card2;
    public CardSuffle card3;
    public CardSuffle card4;
    public CardSuffle card5;

    public GameObject card1Pos;
    public GameObject card2Pos;
    public GameObject card3Pos;
    public GameObject card4Pos;
    public GameObject card5Pos;

    public GameObject startCard;
    public GameObject commonCard;
    public Sprite commonCardImg;

    public bool isGameStarted;

    public float pokerAdminCommission;

    bool isCheck_Off = false;
    bool isFold_Off = false;
    bool isCall_Off = false;
    private bool _allBetEqual;
    private bool _isFlopShowDone;
    private bool _isRiverShowDone;
    private bool _isResultAnnounced;


    public bool isBotActivate;
    public int counter;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }



    public void PlayerFound()
    {
        print("Enter The Player Found Screen");
        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.pokerRequirePlayer)
        {
            CreateAdmin();
            if (DataManager.Instance.joinPlayerDatas.Count > 5)
            {
                StartCoroutine(WaitGameToComplete(CheckNewPlayers));
            }
            if (DataManager.Instance.joinPlayerDatas.Count == 5 && isAdmin)
            {
                if (!isGameStarted)
                {
                    StartGamePlay();
                }
            }
            else
            {
                if (isAdmin) return;
                if (!isGameStarted)
                {
                    waitNextRoundScreenObj.SetActive(true);
                }
            }
        }
        else
        {
            playerFindScreenObj.SetActive(true);
        }
    }

    private IEnumerator WaitGameToComplete(Action callback)
    {
        yield return new WaitUntil(() => !isGameStarted);
        callback();
    }

    private IEnumerator WaitGameToCompleteRemovePlayer(System.Action<int> callback, int parameter)
    {
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
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            MainMenuManager.Instance.CheckPlayers();
        }
        ResetBot();
        //Activating bots
        ActivateBotPlayers();
    }


    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopBackgroundMusic();
        Debug.Log("adminCommission => " + TestSocketIO.Instace.adminCommission);
        pokerAdminCommission = TestSocketIO.Instace.adminCommission;
        sbAmount = MainMenuManager.Instance.selectedValue;
        bbAmount = MainMenuManager.Instance.minBuyINValue;
        //OpenOffScreen();
        potTxt.text = "0";
        lastPrice = 5f;
        PlayerFound();
        sliderValue.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        //StartCoroutine(DisplayCards());
        ManageSoundButtons();
    }

    void Update()
    {
        playerNo = player1.playerNo;
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            //Invoke(nameof(DisplayCards), 2.5f);
        }*/

        UpdateBetAmount();
    }

    public IEnumerator DisplayCards()
    {
        yield return new WaitForSeconds(1.02f);

    }

    private void CheckBalance()
    {
        if (!float.TryParse(DataManager.Instance.joinPlayerDatas[0].balance, out float playerBalance)) return;
        if (!(playerBalance <= 10)) return;
        lowBalanceError.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AddBalanceClick()
    {
        DataManager.Instance.isShopRequest = true;
        TestSocketIO.Instace.LeaveRoom();
        SoundManager.Instance.StartBackgroundMusic();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void LeaveClick()
    {
        TestSocketIO.Instace.LeaveRoom();
        SoundManager.Instance.StartBackgroundMusic();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public GameObject textPrefab;
    public Transform traTextPrefeb;
    void WinBeforeAllDataManage(bool isallin)
    {
        CancelInvoke(nameof(CheckBetAmount));

        List<PokerWinDataWithPlayer> winData = new List<PokerWinDataWithPlayer>();
        PokerWinDataWithPlayer bestPlayer = null;
        int bestRank = int.MaxValue;

        // Iterate over each active, non-folded player
        foreach (PokerPlayer player in playerSquList)
        {
            if (!player.isFold && player.gameObject.activeSelf)
            {
                PokerWinDataMaintain playerData = player.CardDisplay();  // Get player's best hand

                Debug.Log("Player: " + player.name + " | Rule No: " + playerData.ruleNo);

                if (playerData.winList.Count > 0)
                {
                    PokerWinDataWithPlayer entry = new PokerWinDataWithPlayer
                    {
                        ruleNo = playerData.ruleNo,
                        winList = playerData.winList,
                        player = player
                    };

                    winData.Add(entry);

                    // Update best hand if a lower ruleNo is found
                    if (playerData.ruleNo < bestRank)
                    {
                        bestRank = playerData.ruleNo;
                        bestPlayer = entry;
                    }
                }
            }
        }

        if (bestPlayer != null)
        {
            // Collect all players with the same best rank
            List<PokerWinDataWithPlayer> bestPlayers = winData.Where(p => p.ruleNo == bestRank).ToList();

            // If multiple players have the same best rank, compare high cards
            PokerWinDataWithPlayer finalWinner = (bestPlayers.Count == 1) ? bestPlayers[0] : CompareByHighCard(bestPlayers);

            // Ensure only one winner is selected by checking personal high card
            List<PokerWinDataWithPlayer> finalWinners = bestPlayers
                .Where(player => CompareHighCards(player, finalWinner) == player) // Only select the highest player
                .ToList();

            if (finalWinners.Count > 1)
            {
                float splitAmount = totalBetAmount / finalWinners.Count;
                Debug.Log($"[SPLIT POT] Pot split among {finalWinners.Count} players. Each gets: {splitAmount}");

                foreach (var winner in finalWinners)
                {
                    SetPokerWonData(winner.player.playerId);
                    ShowWinAmount(winner.player.playerId, splitAmount.ToString("F2"));

                }
            }
            else
            {
                Debug.Log("Final Winner: " + finalWinner.player.name);
                SetPokerWonData(finalWinner.player.playerId);
                Debug.Log("WIn Amount =>  " + totalBetAmount);

                ShowWinAmount(finalWinner.player.playerId, totalBetAmount.ToString());

            }
            if (isallin)
            {
                if (finalWinners.Count > 1)
                {
                    DistributePotsToWinner(finalWinners[0].player.playerId);
                }
                else
                {
                    DistributePotsToWinner(finalWinner.player.playerId);
                }
            }
        }

        StartCoroutine(DestroyCards());
    }

    // ShowWinAmount remains the same
    public void ShowWinAmount(string winnerId, string amount)
    {

        Debug.Log("WIn Amount =>  " + amount);
        PokerPlayer winner = playerSquList.Find(player => player.playerId == winnerId);
        if (winner == null)
        {
            Debug.LogError("Winner not found!");
            return;
        }

        GameObject textObj = Instantiate(textPrefab, traTextPrefeb);
        textObj.GetComponent<Text>().text = $"+{amount}";

        textObj.transform.DOMove(winner.transform.position, 2f).SetEase(Ease.InOutQuad)
            .OnComplete(() => Destroy(textObj));

        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.joinPlayerDatas[i].userId == winnerId)
            {
                float currentBalance = float.Parse(DataManager.Instance.joinPlayerDatas[i].balance);
                float addAmount = float.Parse(amount);

                // Perform addition
                currentBalance += addAmount;

                // Update the balance in DataManager
                DataManager.Instance.joinPlayerDatas[i].balance = currentBalance.ToString(); // Convert back to string

                // Update UI text
                winner.playerBalanceTxt.text = currentBalance.ToString("G");
                // Show 2 decimal places

            }
        }
    }

    // Compare two players based on their high cards
    PokerWinDataWithPlayer CompareByHighCard(List<PokerWinDataWithPlayer> tiedPlayers)
    {
        PokerWinDataWithPlayer bestPlayer = tiedPlayers[0];

        foreach (var player in tiedPlayers.Skip(1))
        {
            bestPlayer = CompareHighCards(bestPlayer, player) ?? bestPlayer;
        }

        return bestPlayer;
    }

    // Compare high cards and return the winner. If tie, return null.
    // Compare high cards and return the winner. If tie, return null.
    PokerWinDataWithPlayer CompareHighCards(PokerWinDataWithPlayer player1, PokerWinDataWithPlayer player2)
    {
        // Access the first card from personalCards (assuming there are two cards per player)
        int card1Player1 = ConvertAceToHigh(player1.player.card1.cardNo);  // Card 1 of player 1
        int card2Player1 = ConvertAceToHigh(player1.player.card2.cardNo);  // Card 2 of player 1

        int card1Player2 = ConvertAceToHigh(player2.player.card1.cardNo);  // Card 1 of player 2
        int card2Player2 = ConvertAceToHigh(player2.player.card2.cardNo);  // Card 2 of player 2

        // Determine the highest card for each player
        int highestCardPlayer1 = Math.Max(card1Player1, card2Player1);
        int highestCardPlayer2 = Math.Max(card1Player2, card2Player2);

        // If player 1's highest card is greater, return player 1
        if (highestCardPlayer1 > highestCardPlayer2)
            return player1;

        // If player 2's highest card is greater, return player 2
        else if (highestCardPlayer1 < highestCardPlayer2)
            return player2;

        // If both players have the same highest card, compare the second highest card (the kicker)
        int kickerPlayer1 = (card1Player1 == highestCardPlayer1) ? card2Player1 : card1Player1;
        int kickerPlayer2 = (card1Player2 == highestCardPlayer2) ? card2Player2 : card1Player2;

        // Compare the kickers
        if (kickerPlayer1 > kickerPlayer2)
            return player1;
        else if (kickerPlayer1 < kickerPlayer2)
            return player2;

        // If both players have the same cards, return null (a tie)
        return null;
    }


    int ConvertAceToHigh(int cardNo)
    {
        return (cardNo == 1) ? 14 : cardNo;
    }


    public void CallFinalWinner(string winnerPlayerId)
    {
        ResetFillLines();
        var winningData = playerSquList.Find(player => player.playerId == winnerPlayerId);
        if (winningData != null)
        {
            Debug.Log("PLNO  => " + winningData.playerNo + "   pla = " + playerNo);
            if (winningData.playerNo == playerNo)
            {
                SetPokerWon(winningData.playerNo.ToString());
            }

            foreach (GameObject obj in winningData.playerWinObj)
            {
                obj.SetActive(true);
            }
            SoundManager.Instance.CasinoWinSound();
        }

        StartCoroutine(RestartGamePlay());

    }

    private IEnumerator DestroyCards()
    {
        yield return new WaitForSeconds(6f);
        Destroy(card1Pos.transform.GetChild(0).gameObject);
        Destroy(card2Pos.transform.GetChild(0).gameObject);
        Destroy(card3Pos.transform.GetChild(0).gameObject);
        Destroy(card4Pos.transform.GetChild(0).gameObject);
        Destroy(card5Pos.transform.GetChild(0).gameObject);
    }


    #region Second Panel

    public void OpenSecondPanel()
    {
        secondScreenObj.SetActive(true);
    }
    public int prePlayerTurn;
    public int lastPlayerdub;
    public void OpenOnScreen()
    {
        Debug.Log("lastPlayerdub   =>  " + prePlayerTurn);
        if (prePlayerTurn > 0)
        {
            if (playerSquList[prePlayerTurn - 1].isCheck)
            {
                checkbtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                checkbtn.GetComponent<Button>().interactable = false;

            }
            if (playerSquList[prePlayerTurn - 1].isAllIn)
            {
                callBtn.GetComponent<Button>().interactable = false;
                secondUpBtnObj.GetComponent<Button>().interactable = false;
            }
            else
            {
                callBtn.GetComponent<Button>().interactable = true;
                secondUpBtnObj.GetComponent<Button>().interactable = true;

            }
        }
        if (prePlayerTurn == -1 )
        {
            checkbtn.GetComponent<Button>().interactable = true;
        } 
        if ( prePlayerTurn == 0)
        {
            callBtn.GetComponent<Button>().interactable = true;
        }
        if (!_isResultAnnounced)
            downObjectOnObj.SetActive(true);
        downObjectOff.SetActive(false);
        allInOnObj.SetActive(false);

        float callPrice = GetCallAmount();
        Debug.Log("GetCallAmount  " + callPrice);
        Debug.Log("last player   " + callPrice);

        //callPriceTxt.text = lastPrice.ToString();
        if (prePlayerTurn > 0)
        {
            if (playerSquList[prePlayerTurn - 1].isAllIn)
            {
                Debug.Log("INININIINININININIININIININININIINIINININIIIN  ");
                callPriceTxt.text = "Call";
                raisePriceTxt.text = "ALLIN : " + player1.playerBalanceTxt.text;
                raisePrice = float.Parse(player1.playerBalanceTxt.text);
                Debug.Log("raisePrice  -------------->  " + raisePrice);

            }
            else
            {

                callPriceTxt.text = "Call : " + callPrice.ToString();
                raisePriceTxt.text = "Raise : " + lastPrice.ToString();
                raisePrice = lastPrice;

            }
        }
        else
        {
            callPriceTxt.text = "Call : " + callPrice.ToString();
            raisePriceTxt.text = "Raise : " + lastPrice.ToString();
            raisePrice = lastPrice;
        }
        Debug.Log("isFold_Off  => " + isFold_Off);
        Debug.Log("isAllIn  => " + isAllIn);
        Debug.Log("isCheck_Off  => " + isCheck_Off);
        Debug.Log("isCall_Off  => " + isCall_Off);
        Debug.Log("prePlayerTurn  => " + prePlayerTurn);
        if (isFold_Off && FindPrevPlayerFOLD())
        {
            //SendPokerPlayerFold(player1.playerId);
            Second_Fold_ButtonClick();
        }
        else if (isAllIn)
        {
            callBtn.SetActive(false);
            raiseBtn.SetActive(false);
            allInBtn.SetActive(true);
        }
        else if (isCheck_Off && FindPrevPlayerCHECK())
        {

            Second_Check_ButtonCLick();
        }
        else if (isCall_Off && FindPrevPlayerCALL())
        {
            //SendPokerBet(player1.playerNo, lastPrice, "call");
            Second_Call_ButtonClick();
        }

        ResetChecks();
    }
    private bool FindPrevPlayerCHECK()
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == prePlayerTurn)
            {
                return pokerPlayers[i].isCheck; // Directly return isCheck value
            }
        }

        // If no matching player is found, return false as default
        return false;
    }
    private bool FindPrevPlayerFOLD()
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == prePlayerTurn)
            {
                return pokerPlayers[i].isFold; // Directly return isCheck value
            }
        }

        // If no matching player is found, return false as default
        return false;
    }
    private bool FindPrevPlayerCALL()
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == prePlayerTurn)
            {
                return pokerPlayers[i].isCalled; // Directly return isCheck value
            }
        }

        // If no matching player is found, return false as default
        return false;
    }


    private void ResetChecks()
    {
        for (int i = 0; i < tickObj.Length; i++)
        {
            tickObj[i].SetActive(false);
            //blackBtnObj[i].sprite = blackBtnOff;
        }
        isCheck_Off = false;
        isFold_Off = false;
        isCall_Off = false;
    }

    private void ResetFillLines()
    {
        foreach (var players in pokerPlayers.Where(players => players.fillLine.gameObject.activeSelf))
        {
            players.fillLine.gameObject.SetActive(false);
        }

        downObjectOnObj.SetActive(false);
        downObjectOff.SetActive(false);
        allInOnObj.SetActive(false);
    }

    public void OpenAllInScreen()
    {
        downObjectOnObj.SetActive(false);
        downObjectOff.SetActive(false);
        allInOnObj.SetActive(true);
        float allInPrice = float.Parse(DataManager.Instance.playerData.balance);
        allInPriceTxt.text = allInPrice.ToString();
    }

    private float GetCallAmount()
    {
        float maxBetAmount = playerSquList.Max(player => player.betAmount);
        float callAmount = Mathf.Max(0, maxBetAmount - player1.betAmount);

        return (callAmount == 0) ? lastPrice : callAmount;
    }

    public IEnumerator RestartGamePlay()
    {
        if (!isGameStarted)
        {
            Debug.LogWarning("Game is not started yet!");
            yield break;  // Fix for CS1622
        }

        isGameStarted = false;
        yield return new WaitForSeconds(6f);
        ResetAmountAfterAllin();
        ResetRound();
        CheckAllPlayerIsValidOrNot();
        yield return new WaitForSeconds(1f);


        //print("Enther The Generate Player");
        CheckBalance();
        if (isAdmin)
        {
            StartGamePlay();
            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
            print("Enther The Generate Player1");
            //isBotActivate = true;

        }
    }
    public void CheckAllPlayerIsValidOrNot()
    {
        for (int j = 0; j < DataManager.Instance.joinPlayerDatas.Count; j++)
        {
            // Convert balance from string to float
            if (DataManager.Instance.joinPlayerDatas[j].userId != player1.playerId)
            {

                int num = UnityEngine.Random.Range(0, 10);
                Debug.Log("NUM  =>  " + num);
                if (num > 2)
                {

                    if (float.TryParse(DataManager.Instance.joinPlayerDatas[j].balance, out float balanceValue))
                    {
                        if (balanceValue < bbAmount) // If balance is too low, assign new bot data
                        {
                            // Assign a random balance
                            DataManager.Instance.joinPlayerDatas[j].balance = UnityEngine.Random.Range(1000, 5000).ToString();

                            // Assign a random bot name
                            int randomIndex = UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count);
                            DataManager.Instance.joinPlayerDatas[j].userName = BotManager.Instance.botUserName[randomIndex];

                            // Assign a random avatar URL
                            int randomIndex1 = UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count);
                            string avatarUrl = BotManager.Instance.botUser_Profile_URL[randomIndex1];
                            DataManager.Instance.joinPlayerDatas[j].avtar = avatarUrl;
                            // Update Player UI
                            for (int i = 0; i < playerSquList.Count; i++)
                            {
                                if (DataManager.Instance.joinPlayerDatas[j].userId == playerSquList[i].playerId)
                                {
                                    playerSquList[i].playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[j].balance;
                                    playerSquList[i].playerNameTxt.text = DataManager.Instance.joinPlayerDatas[j].userName;
                                    playerSquList[i].avatar = avatarUrl;
                                    // Start coroutine to load image from URL
                                    StartCoroutine(LoadAvatarFromURL(avatarUrl, playerSquList[i].avatarImg));
                                }
                            }
                        }
                    }
                }
                else
                {
                    DataManager.Instance.joinPlayerDatas[j].balance = UnityEngine.Random.Range(1000, 5000).ToString();

                    // Assign a random bot name
                    int randomIndex = UnityEngine.Random.Range(0, BotManager.Instance.botUserName.Count);
                    DataManager.Instance.joinPlayerDatas[j].userName = BotManager.Instance.botUserName[randomIndex];

                    // Assign a random avatar URL
                    int randomIndex1 = UnityEngine.Random.Range(0, BotManager.Instance.botUser_Profile_URL.Count);
                    string avatarUrl = BotManager.Instance.botUser_Profile_URL[randomIndex1];
                    DataManager.Instance.joinPlayerDatas[j].avtar = avatarUrl;
                    // Update Player UI
                    for (int i = 0; i < playerSquList.Count; i++)
                    {
                        if (DataManager.Instance.joinPlayerDatas[j].userId == playerSquList[i].playerId)
                        {
                            playerSquList[i].playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[j].balance;
                            playerSquList[i].playerNameTxt.text = DataManager.Instance.joinPlayerDatas[j].userName;
                            playerSquList[i].avatar = avatarUrl;
                            // Start coroutine to load image from URL
                            StartCoroutine(LoadAvatarFromURL(avatarUrl, playerSquList[i].avatarImg));
                        }
                    }
                }
            }
        }
    }

    // Coroutine to download the avatar image and set it as a sprite
    private IEnumerator LoadAvatarFromURL(string url, UnityEngine.UI.Image avatarImage)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                if (texture != null)
                {
                    avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
            else
            {
                Debug.LogError("Failed to load avatar image: " + request.error);
            }
        }
    }
    public void OpenOffScreen()
    {
        downObjectOnObj.SetActive(false);
        secondSubScreenObj.SetActive(false);
        allInOnObj.SetActive(false);
        secondUpBtnObj.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
        if (!player1.isFold)
            downObjectOff.SetActive(true);
        /*for (int i = 0; i < tickObj.Length; i++)
        {
            tickObj[i].SetActive(false);
            blackBtnObj[i].sprite = blackBtnOff;
        }
        isCheck_Off = false;
        isFold_Off = false;
        isCall_Off = false;*/
    }

    public void OffButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 0)
        {
            // fold button
            isFold_Off = true;
            isCheck_Off = false;
            isCall_Off = false;

        }
        else if (no == 1)
        {
            // check button
            isFold_Off = false;
            isCheck_Off = true;
            isCall_Off = false;
        }
        else if (no == 2)
        {
            //call button
            isFold_Off = false;
            isCheck_Off = false;
            isCall_Off = true;
        }
        for (int i = 0; i < tickObj.Length; i++)
        {
            if (i == no)
            {
                tickObj[i].SetActive(true);
                //blackBtnObj[i].sprite = blackBtnOn;
            }
            else
            {
                tickObj[i].SetActive(false);
                //blackBtnObj[i].sprite = blackBtnOff;
            }
        }
    }

    public void Second_Check_ButtonCLick()
    {
        SoundManager.Instance.ButtonClick();
        PokerGameManager.Instance.BetAnimForCheck(player1, 0);
        SendPokerBet(player1.playerNo, 0, "check");
        player1.isCheck = true;
        prePlayerTurn = player1.playerNo;
        ChangePlayerTurn(player1.playerNo);
    }

    public void Second_Fold_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Debug.Log("SendPokerPlayerFold  =>  " + player1.playerId);
        SendPokerPlayerFold(player1.playerId);
        ChangePlayerTurn(player1.playerNo);
    }

    public void Second_Call_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //SoundManager.Instance.ThreeBetSound();
        float callAmount = GetCallAmount();
        Debug.Log("GetCallAmount  " + callAmount);
        if (CheckMoney(callAmount) == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        SoundManager.Instance.ThreeBetSound();
        BetAnim(player1, callAmount);
        DataManager.Instance.DebitAmount((callAmount).ToString(), DataManager.Instance.gameId, "Poker-Bet-" + DataManager.Instance.gameId, "game", 1);

        Debug.Log("BET AMOUNT POKER +>" + callAmount);

        SendPokerBet(player1.playerNo, callAmount, "call");
        ChangePlayerTurn(player1.playerNo);
        DisplayCurrentBalance();
    }
    public List<float> playerAmounts = new List<float>();  // Players ki amount list
    private List<bool> playerBetsMade = new List<bool>();    // Players ki bet status list
    public int currentRound = 1;  // Current round
    public int currentPlayerIndex = 0;  // Current player index


    // Ensure list size before accessing index




    public void Second_Raise_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        SoundManager.Instance.ThreeBetSound();
        //if (raisePrice == 100)
        //{
        //    SendPokerBet(player1.playerNo, raisePrice, "allin");

        //}
        //else
        //{
        //}
        if (CheckMoney(raisePrice) == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        Debug.Log("BET AMOUNT POKER +>" + raisePrice);
        Debug.Log("player1.playerBalanceTxt.text +>" + float.Parse(player1.playerBalanceTxt.text));
        if (raisePrice == float.Parse(player1.playerBalanceTxt.text))
        {
            Debug.Log("ITS A ALL IN");
            SendPokerBet(player1.playerNo, raisePrice, "AllIn");
            PokerGameManager.Instance.AddPlayerBet(player1, raisePrice);
            player1.isAllIn = true;
        }
        else
        {
            SendPokerBet(player1.playerNo, raisePrice, "raise");

        }
        prePlayerTurn = player1.playerNo;
        SoundManager.Instance.ThreeBetSound();
        lastPrice = raisePrice;
        BetAnim(player1, raisePrice);
        DataManager.Instance.DebitAmount((raisePrice).ToString(), DataManager.Instance.gameId, "Poker-Bet-" + DataManager.Instance.gameId, "game", 2);
        if (!AreAllPlayersAllIn1())
        {
            ChangePlayerTurn(player1.playerNo);
        }
        else
        {
            ResetFillLines();
        }
        DisplayCurrentBalance();
    }
    public bool AreAllPlayersAllIn1()
    {
        foreach (PokerPlayer player in playerSquList)
        {
            if (!player.isAllIn) // Check if any player is not all-in
            {
                return false; // Return false if any player is not all-in
            }
        }

        return true; // Return true if all players are all-in
    }


    public void Second_AllIn_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        float allInAmount = float.Parse(DataManager.Instance.playerData.balance);

        BetAnim(player1, allInAmount);
        DataManager.Instance.DebitAmount((allInAmount).ToString(), DataManager.Instance.gameId, "Poker-Bet-" + DataManager.Instance.gameId, "game", 1);
        SendPokerBet(player1.playerNo, raisePrice, "allin");
        ChangePlayerTurn(player1.playerNo);
        DisplayCurrentBalance();

    }

    public void Second_Up_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (secondUpBtnObj.transform.rotation.z != 0)
        {
            secondSubScreenObj.SetActive(false);
            secondUpBtnObj.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
        }
        else
        {
            secondSubScreenObj.SetActive(true);
            raisePriceTxt.text = "Raise : " + raisePrice.ToString();
            secondUpBtnObj.transform.DORotate(new Vector3(0, 0, 180), 0.1f);

        }

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
        }
        else if (musicImg.sprite == musicoffSprite)
        {
            DataManager.Instance.SetMusic(0);
            musicImg.sprite = musiconSprite;
            SoundManager.Instance.ButtonClick();
        }
    }

    private void ManageSoundButtons()
    {
        soundImg.sprite = DataManager.Instance.GetSound() == 0 ? soundonSprite : soundoffSprite;
        vibrationImg.sprite = DataManager.Instance.GetVibration() == 0 ? vibrationonSprite : vibrationoffSprite;
        musicImg.sprite = DataManager.Instance.GetMusic() == 0 ? musiconSprite : musicoffSprite;
    }


    /*public void Second_DropDown_Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sliderValue.value > 0)
        {
            if (sliderValue.value > 0 && sliderValue.value <= 0.25)
            {
                raisePrice -= 5f;
                sliderValue.value = 0;
                minusBtn.interactable = false;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.25 && sliderValue.value <= 0.5)
            {
                raisePrice -= 5f;
                sliderValue.value = 0.25f;

                minusBtn.interactable = true;
                plusBtn.interactable = true;

            }
            else if (sliderValue.value > 0.5 && sliderValue.value <= 0.75)
            {
                raisePrice -= 5f;
                sliderValue.value = 0.5f;


                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.75 && sliderValue.value <= 1)
            {
                raisePrice -= 5;
                sliderValue.value = 0.75f;


                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
        }
        raisePriceTxt.text = raisePrice.ToString();
    }
    public void Second_DropDown_Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sliderValue.value > 0)
        {
            if (sliderValue.value > 0 && sliderValue.value <= 0.25)
            {
                raisePrice += 5;
                sliderValue.value = 0.25f;
                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.25 && sliderValue.value <= 0.5)
            {
                raisePrice += 5f;
                sliderValue.value = 0.5f;
                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.5 && sliderValue.value <= 0.75)
            {
                raisePrice += 5f;
                sliderValue.value = 0.75f;
                minusBtn.interactable = true;
                plusBtn.interactable = true;
            }
            else if (sliderValue.value > 0.75 && sliderValue.value <= 1)
            {
                raisePrice += 5f;
                sliderValue.value = 1f;
                minusBtn.interactable = true;
                plusBtn.interactable = false;
            }
            raisePriceTxt.text = raisePrice.ToString();
        }
    }*/

    public void Second_DropDown_Minus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        /*if (raisePrice > 10)
        {
            raisePrice -= 10;
            raisePriceTxt.text = raisePrice.ToString();
            sliderValue.value = raisePrice / 100f;

            // Enable plus button if value is not at max
            plusBtn.interactable = (raisePrice < 100);

            // Disable minus button if value is at min
            minusBtn.interactable = (raisePrice > 10);
        }*/
        if (raisePrice > 10)
        {
            raisePrice -= 10;
            Debug.Log("raisePrice   => " + raisePrice);

            UpdateUI();
        }
    }

    public void Second_DropDown_Plus_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        /*if (raisePrice < 100)
        {
            raisePrice += 10;
            raisePriceTxt.text = raisePrice.ToString();
            sliderValue.value = raisePrice / 100f;

            // Enable minus button if value is not at min
            minusBtn.interactable = (raisePrice > 10);

            // Disable plus button if value is at max
            plusBtn.interactable = (raisePrice < 100);
        }*/
        float playerBalance = float.Parse(DataManager.Instance.playerData.balance);
        if (raisePrice < playerBalance)
        {
            raisePrice += 10;
            UpdateUI();
        }
    }

    public void Second_DropDown_Menu_ButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        /*if (no == 1)
        {
            //max
            raisePrice = 100;

        }
        else if (no == 2)
        {
            //pot
            raisePrice = 50;
        }
        else if (no == 3)
        {
            //3/4
            raisePrice = lastPrice * 4;

        }
        else if (no == 4)
        {
            //1/2
            raisePrice = lastPrice * 2;
        }
        else if (no == 5)
        {
            //Min
            raisePrice = lastPrice;
        }
        sliderValue.value = raisePrice / 100f;
        raisePriceTxt.text = raisePrice.ToString();*/

        switch (no)
        {
            case 1:
                // Max
                raisePrice = float.Parse(DataManager.Instance.playerData.balance);
                break;
            case 2:
                // Pot All In
                raisePrice = float.Parse(DataManager.Instance.playerData.balance);
                Debug.Log("raisePrice   => " + raisePrice);
                Debug.Log("DataManager.Instance.playerData.balance   => " + DataManager.Instance.playerData.balance);
                break;
            case 3:
                // ×3
                float threeFourthBalance = lastPrice * 3f;
                raisePrice = Mathf.Ceil(threeFourthBalance);
                break;
            case 4:
                // ×2
                float halfBalance = lastPrice * 2f;
                raisePrice = Mathf.Ceil(halfBalance);
                break;
            case 5:
                // Min
                raisePrice = (raisePrice >= 10) ? 10 : 0; // or set a minimum value
                break;
        }
        Debug.Log("raisePrice   => " + raisePrice);

        UpdateUI();
    }

    public void OnSliderValueChanged()
    {
        /*raisePrice = Mathf.RoundToInt(sliderValue.value * 100f);
        raisePriceTxt.text = raisePrice.ToString();
        
        minusBtn.interactable = (raisePrice > 10);
        
        plusBtn.interactable = (raisePrice < 100);*/

        float playerBalance = float.Parse(DataManager.Instance.playerData.balance);
        decimal preciseRaisePrice = Math.Round((decimal)(sliderValue.value * playerBalance), 2, MidpointRounding.AwayFromZero);
        raisePrice = (float)preciseRaisePrice;
        Debug.Log("raisePrice   => " + raisePrice);


        UpdateUI();
    }

    private void UpdateUI()
    {
        Debug.Log("update ui  ---   : ");
        float playerBalance = float.Parse(DataManager.Instance.playerData.balance);

        // Ensure raisePrice is clamped and rounded to 2 decimal places
        decimal roundedValue = Math.Round((decimal)raisePrice, 2, MidpointRounding.AwayFromZero);
        raisePrice = (float)roundedValue;


        Debug.Log("Raise Price  ---   : " + raisePrice);

        // Display with exactly 2 decimal places
        raisePriceTxt.text = "Raise : " + raisePrice.ToString("F2");

        sliderValue.value = raisePrice / playerBalance;

        minusBtn.interactable = (raisePrice > 10);
        plusBtn.interactable = (raisePrice < playerBalance);
    }


    #endregion

    #region Game Play Manager

    public void DisplayCurrentBalance()
    {
        player1.playerBalanceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    public void UpdateBetAmount()
    {
        player1BetAmount = player1.betAmount;
        player2BetAmount = player2.betAmount;
        player3BetAmount = player3.betAmount;
        player4BetAmount = player4.betAmount;
        player5BetAmount = player5.betAmount;

        //CheckBetAmount();
    }

    /*  public void CheckBetAmount()
      {

          bool equal = true;
          float betAmount = 0f;
          bool isFirstPlayer = true;

          foreach (var playerSquare in playerSquList.Where(playerSquare => !playerSquare.isFold))
          {

              if (playerSquare.isCheck== true)
              {
                  equal = false;
                  break;
              }
              if (isFirstPlayer)
              {
                  betAmount = playerSquare.betAmount;
                  isFirstPlayer = false;
              }
              else
              {
                  if (!Mathf.Approximately(playerSquare.betAmount, betAmount))
                  {
                      equal = false;
                      break;
                  }
              }
          }

          // If not all non-folded players have bet the same amount, exit the method
          if (!equal) return;

          // If no non-folded player has bet anything, exit the method
          if (betAmount <= 0f) return;

          // Switch on the game state based on which cards have been shown
          switch (_allBetEqual)
          {
              case false:
                  StartCoroutine(FlopCardShow());
                  _allBetEqual = true;
                  Player1BetIn();
                  Player2BetIn();
                  Player3BetIn();
                  Player4BetIn();
                  Player5BetIn();
                  ResetBetAmount();
                  break;
              case true when !_isFlopShowDone:
                  StartCoroutine(TurnCardShow());
                  _isFlopShowDone = true;
                  Player1BetIn();
                  Player2BetIn();
                  Player3BetIn();
                  Player4BetIn();
                  Player5BetIn();
                  ResetBetAmount();
                  break;
              case true when (_isFlopShowDone && !_isRiverShowDone):
                  //isGameStop = false;
                  StartCoroutine(RiverCardShow());
                  _isRiverShowDone = true;
                  Player1BetIn();
                  Player2BetIn();
                  Player3BetIn();
                  Player4BetIn();
                  Player5BetIn();
                  ResetBetAmount();
                  //WinPoker();
                  break;
              case true when _isFlopShowDone && _isRiverShowDone && !_isResultAnnounced:
                  AnnounceResults();
                  _isResultAnnounced = true;
                  break;
              default:
                  print("All values are equal");
                  break;
          }
      }*/

    /* public void CheckBetAmount()
     {
         bool allEqual = true;
         bool allCheckSame = true; // Track if all isCheck values are the same
         bool allAllInSame = true; // Track if all isAllIn values are the same
         float betAmount = -1f;
         bool? firstCheckValue = null;
         bool? firstAllInValue = null;
         bool allPlayersChecked = true; // Flag to check if all players have checked
         bool allPlayersAllIn = true;   // Flag to check if all players are All-In

         foreach (var playerSquare in playerSquList.Where(player => !player.isFold))
         {
             // Track first player's isCheck value
             if (firstCheckValue == null)
             {
                 firstCheckValue = playerSquare.isCheck;
             }
             else if (firstCheckValue != playerSquare.isCheck)
             {
                 allCheckSame = false; // If any isCheck value is different
             }

             // Track first player's isAllIn value
             if (firstAllInValue == null)
             {
                 firstAllInValue = playerSquare.isAllIn;
             }
             else if (firstAllInValue != playerSquare.isAllIn)
             {
                 allAllInSame = false; // If any isAllIn value is different
             }

             // Check if all players are All-In
             if (!playerSquare.isAllIn)
             {
                 allPlayersAllIn = false;
             }

             // Check if all players have checked
             if (!playerSquare.isCheck)
             {
                 allPlayersChecked = false;
             }

             // Track first player's betAmount
             if (betAmount == -1f)
             {
                 betAmount = playerSquare.betAmount;
             }
             else if (!Mathf.Approximately(playerSquare.betAmount, betAmount))
             {
                 allEqual = false; // If any betAmount is different
             }
         }

         // If all active players are All-In, process the next game stage
         if (allPlayersAllIn)
         {
             ProcessNextGameStage();
             return;
         }

         // If all players have checked (betAmount = 0) and bets are the same, skip bet logic and move forward
         if (allPlayersChecked)
         {
             ProcessNextGameStage();
             return;
         }

         // If bet amounts are not equal or check values are different, exit
         if (!allEqual || !allCheckSame || betAmount <= 0f) return;

         // Proceed with game progression
         ProcessNextGameStage();
     }


     private void ProcessNextGameStage()
     {
         if (!_allBetEqual)
         {
             StartCoroutine(FlopCardShow());
             _allBetEqual = true;
         }
         else if (!_isFlopShowDone)
         {
             StartCoroutine(TurnCardShow());
             _isFlopShowDone = true;
         }
         else if (!_isRiverShowDone)
         {
             StartCoroutine(RiverCardShow());
             _isRiverShowDone = true;
         }
         else if (!_isResultAnnounced)
         {
             AnnounceResults();
             _isResultAnnounced = true;
         }
         else
         {
             print("All values are equal");
         }

         // Reset players' betting state after each stage
         ResetAllPlayersBetState();
     }

     private void ResetAllPlayersBetState()
     {

         Debug.Log("ResetAllPlayersBetState()");
         Player1BetIn();
         Player2BetIn();
         Player3BetIn();
         Player4BetIn();
         Player5BetIn();
         ResetBetAmount();
     }

     public void ResetBetAmount()
     {
         foreach (var activePlayers in playerSquList)
         {
             // Reset bet amount to 0 if needed
             activePlayers.betAmount = 0f;
             activePlayers.betTxt.text = activePlayers.betAmount.ToString();

             // Optionally reset the isCheck value if needed for the next round
             activePlayers.isCheck = false;
         }
     }
 */

    public void CheckBetAmount()
    {
        bool allEqual = true;
        bool allCheckSame = true; // Track if all isCheck values are the same
        bool allAllInSame = true; // Track if all isAllIn values are the same
        float betAmount = -1f;
        bool? firstCheckValue = null;
        bool? firstAllInValue = null;
        bool allPlayersChecked = true; // Flag to check if all players have checked
        bool allPlayersAllIn = true;   // Flag to check if all players are All-In

        foreach (var playerSquare in playerSquList.Where(player => !player.isFold))
        {
            // Track first player's isCheck value
            if (firstCheckValue == null)
            {
                firstCheckValue = playerSquare.isCheck;
            }
            else if (firstCheckValue != playerSquare.isCheck)
            {
                allCheckSame = false; // If any isCheck value is different
            }

            // Track first player's isAllIn value
            if (firstAllInValue == null)
            {
                firstAllInValue = playerSquare.isAllIn;
            }
            else if (firstAllInValue != playerSquare.isAllIn)
            {
                allAllInSame = false; // If any isAllIn value is different
            }

            // Check if all players are All-In
            if (!playerSquare.isAllIn)
            {
                allPlayersAllIn = false;
            }

            // Check if all players have checked
            if (!playerSquare.isCheck)
            {
                allPlayersChecked = false;
            }

            // Track first player's betAmount
            if (betAmount == -1f)
            {
                betAmount = playerSquare.betAmount;
            }
            else if (!Mathf.Approximately(playerSquare.betAmount, betAmount))
            {
                allEqual = false; // If any betAmount is different
            }
        }

        // If all players are All-In, process the next game stages
        if (allPlayersAllIn)
        {
            // If FlopCardShow hasn't been done yet, call all stages (Flop, Turn, River)
            if (!_allBetEqual)
            {
                StartCoroutine(FlopCardShow());
                _allBetEqual = true; // Ensure bet animation happens only once
            }
            // If FlopCardShow is already done, only call Turn and River
            else if (!_isFlopShowDone)
            {
                StartCoroutine(TurnCardShow());
                _isFlopShowDone = true;
            }
            else if (!_isRiverShowDone)
            {
                StartCoroutine(RiverCardShow());
                _isRiverShowDone = true;
            }
            else if (!_isResultAnnounced)
            {
                _isResultAnnounced = true;
                CreateSidePotsAndDistribute();
                AnnounceResultsFOrALLIN();
            }

            // After all stages are done, reset players' betting state
            //   ResetAllPlayersBetState();
            return; // Exit early as the stages have already been processed
        }

        // If all players have checked (betAmount = 0) and bets are the same, skip bet logic and move forward
        if (allPlayersChecked)
        {
            ProcessNextGameStage();
            return;
        }

        // If bet amounts are not equal or check values are different, exit
        if (!allEqual || !allCheckSame || betAmount <= 0f) return;

        // Proceed with game progression
        ProcessNextGameStage();
    }

    private void ProcessNextGameStage()
    {
        if (!_allBetEqual)
        {
            StartCoroutine(FlopCardShow());
            _allBetEqual = true;
        }
        else if (!_isFlopShowDone)
        {
            StartCoroutine(TurnCardShow());
            _isFlopShowDone = true;
        }
        else if (!_isRiverShowDone)
        {
            StartCoroutine(RiverCardShow());
            _isRiverShowDone = true;
        }
        else if (!_isResultAnnounced)
        {
            AnnounceResults();
            _isResultAnnounced = true;
        }
        else
        {
            print("All values are equal");
        }

        // Check if all players are All-In; if they are, skip resetting bet states
        if (!AreAllPlayersAllIn()) // Calls ResetAllPlayersBetState() only if NOT All-In
        {
            ResetAllPlayersBetState();
        }
    }

    // Function to check if all active players are All-In
    private bool AreAllPlayersAllIn()
    {
        foreach (var player in playerSquList.Where(p => !p.isFold))
        {
            if (!player.isAllIn)
            {
                return false; // If any player is NOT All-In, return false
            }
        }
        return true; // All players are All-In
    }
    private bool AreAllPlayersFoldedExceptOne()
    {
        // Count total folded players
        int foldedCount = playerSquList.Count(p => p.isFold);

        // Check if exactly one player is NOT folded
        return foldedCount == playerSquList.Count - 1;
    }
    public void AllPlayerFoldAndShowWin()
    {
        if (AreAllPlayersFoldedExceptOne())
        {
            if (!firstround)
            {
                FlopCardShow();
            }
            else if(!secondRound)
            {
                TurnCardShow();
            }else if (!thirdRound)
            {
                RiverCardShow();
            }
            AnnounceResults();
            _isResultAnnounced = true;
        }
    }
    private void ResetAllPlayersBetState()
    {
        Debug.Log("ResetAllPlayersBetState() called");
        Player1BetIn();
        Player2BetIn();
        Player3BetIn();
        Player4BetIn();
        Player5BetIn();
        ResetBetAmount();
    }
    public bool firstround;
    public bool secondRound;
    public bool thirdRound;
    public void ResetBetAmount()
    {
        foreach (var activePlayer in playerSquList)
        {
            activePlayer.betAmount = 0f;
            activePlayer.betTxt.text = activePlayer.betAmount.ToString();
            activePlayer.isCheck = false;
        }
    }
    public void ResetBetAmountForALLINReset()
    {
        foreach (var activePlayer in playerSquList)
        {
            activePlayer.betAmount = 0f;
            activePlayer.betTxt.text = activePlayer.betAmount.ToString();
            activePlayer.isCheck = false;
            activePlayer.isAllIn = false;
            activePlayer.currentBotBetAmount = 0;
        }
        prePlayerTurn = 0;
        firstround = false;
        secondRound = false;
        thirdRound = false;
    }



    public IEnumerator FlopCardShow()
    {
        firstround = true;
        GameObject obj = Instantiate(commonCard, card1Pos.transform);
        prePlayerTurn = -1;

        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 firstPos = card1Pos.transform.position;
        firstPos.x -= 0.2f;
        obj.transform.DOMove(firstPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card1.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card1.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card1");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });

        });
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(SecondCardShow());
    }
    public IEnumerator SecondCardShow()
    {
        GameObject obj = Instantiate(commonCard, card2Pos.transform);
        prePlayerTurn = -1;

        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 secondPos = card2Pos.transform.position;
        secondPos.x -= 0.2f;
        obj.transform.DOMove(secondPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card2.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card2.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card2");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });

        });
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ThirdCardShow());
    }
    public IEnumerator ThirdCardShow()
    {
        GameObject obj = Instantiate(commonCard, card3Pos.transform);
        prePlayerTurn = -1;

        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 thirdPos = card3Pos.transform.position;
        thirdPos.x -= 0.2f;
        obj.transform.DOMove(thirdPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card3.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card3.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card3");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });

        });
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator TurnCardShow()
    {
        secondRound = true;
        GameObject obj = Instantiate(commonCard, card4Pos.transform);
        prePlayerTurn = -1;

        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 fourthPos = card4Pos.transform.position;
        fourthPos.x -= 0.2f;
        obj.transform.DOMove(fourthPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card4.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card4.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card4");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });

        });
        yield return new WaitForSeconds(2f);
    }
    public PokerPlayer GetPlayer(int playerNo)
    {
        switch (playerNo)
        {
            case 1: return player1;
            case 2: return player2;
            case 3: return player3;
            case 4: return player4;
            case 5: return player5;
            default: return null;
        }
    }
    public IEnumerator RiverCardShow()
    {
        thirdRound = true;
        GameObject obj = Instantiate(commonCard, card5Pos.transform);
        prePlayerTurn = -1;
        SoundManager.Instance.CasinoCardMoveSound();
        obj.transform.position = startCard.transform.position;
        obj.SetActive(true);
        Vector3 fifthPos = card5Pos.transform.position;
        fifthPos.x -= 0.2f;
        obj.transform.DOMove(fifthPos, 0.4f).OnComplete(() =>
        {
            SoundManager.Instance.CasinoCardSwipeSound();

            obj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
            {
                obj.transform.GetComponent<Image>().sprite = card5.cardSprite;
                obj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                {
                    if (card5.cardNo == cardSufflesGen[0].cardNo)
                    {
                        print("Card5");
                    }
                    else
                    {
                        print("card didnt match");
                    }
                });
            });

        });
        yield return new WaitForSeconds(4f);
    }

    private void AnnounceResults()
    {
        Debug.Log("ISgame Stop  " + isGameStop);
        isGameStop = false;
        Debug.Log("ISgame Stop  " + isGameStop);
        Player1BetIn();
        Player2BetIn();
        Player3BetIn();
        Player4BetIn();
        Player5BetIn();


        /*// Reveal cards for all active players
        foreach (var playerSquare in playerSquList.Where(playerSquare => !playerSquare.isFold))
        {
            playerSquare.DisplayPlayerCard();
        }*/

        ResetBetAmount();

        WinPoker(false);
    }

    private void AnnounceResultsFOrALLIN()
    {
        Debug.Log("ISgame Stop  " + isGameStop);

        isGameStop = false;
        Debug.Log("ISgame Stop  " + isGameStop);
        ResetAmountAfterAllin();

        ResetBetAmountForALLINReset();
        /*// Reveal cards for all active players
        foreach (var playerSquare in playerSquList.Where(playerSquare => !playerSquare.isFold))
        {
            playerSquare.DisplayPlayerCard();
        }*/


        WinPoker(true);
    }


   
    #region SIDE POT LOGIC

    public Transform sidePotParent;
    public GameObject sidePotPrefab;

    private List<PokerPlayer> players = new List<PokerPlayer>();
    private List<float> betAmounts = new List<float>();
    string playerId;
    public void AddPlayerBet(PokerPlayer player, float betAmount)
    {
        // Add player and their bet
        players.Add(player);
        betAmounts.Add(betAmount);
        Debug.Log($"Player {player.name} with ID {player.playerId} placed a bet of ${betAmount:F2}");
    }
    Dictionary<string, List<float>> sidePotHistory = new Dictionary<string, List<float>>();

    public void CreateSidePotsAndDistribute()
    {
        Debug.Log("CreateSidePotsAndDistribute called");

        if (betAmounts.Count == 0) return; // No bets placed

        List<int> sortedIndexes = new List<int>();
        for (int i = 0; i < betAmounts.Count; i++)
        {
            sortedIndexes.Add(i);
        }
        sortedIndexes.Sort((i1, i2) => betAmounts[i1].CompareTo(betAmounts[i2]));

        float totalMainPot = betAmounts[sortedIndexes[0]];
        List<float> sidePots = new List<float>();

        float lastBet = totalMainPot;

        foreach (int index in sortedIndexes)
        {
            if (index >= 0 && index < playerSquList.Count)
            {
                playerId = playerSquList[index].playerId;
                Debug.Log("Player ID: " + playerId);
            }
            else
            {
                Debug.LogError("Invalid index! Out of range: " + index);
                continue;
            }

            float currentBet = betAmounts[index];

            if (currentBet > lastBet)
            {
                float potContribution = 0;
                List<float> contributions = new List<float>();

                foreach (int otherIndex in sortedIndexes)
                {
                    if (betAmounts[otherIndex] >= lastBet)
                    {
                        float individualContribution = Mathf.Min(currentBet - lastBet, betAmounts[otherIndex] - lastBet);
                        contributions.Add(individualContribution);
                        potContribution += individualContribution;
                    }
                }

                if (potContribution > 0)
                {
                    sidePots.Add(potContribution);
                    Debug.Log($"[DEBUG] New Side Pot Created: {potContribution}");

                    if (!sidePotHistory.ContainsKey(playerId))
                    {
                        sidePotHistory[playerId] = new List<float>();
                    }
                    sidePotHistory[playerId].Add(potContribution);
                }

                lastBet = currentBet;
            }
        }

        foreach (var entry in sidePotHistory)
        {
            Debug.Log($"SidePotHistory -> Player: {entry.Key}, Pots: {string.Join(", ", entry.Value)}");
        }

        // **NEW FUNCTION: Add Side Pots to UI**
        AddSidePotsToUI(sidePots, totalMainPot);
    }

    private void AddSidePotsToUI(List<float> sidePotAmounts, float mainPotAmount)
    {
        Debug.Log($"Main Pot Amount: ${mainPotAmount}");
        potAmount += mainPotAmount;
        potTxt.text = "" + potAmount;
        totalBetAmount = potAmount;

        // Destroy existing side pots before creating new ones
        foreach (Transform child in sidePotParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and display new side pots
        foreach (float amount in sidePotAmounts)
        {
            GameObject sidePot = Instantiate(sidePotPrefab, sidePotParent);
            sidepotAdded.Add(sidePot);
            sidePot.GetComponent<Text>().text = $"{amount}";
        }
    }

    public List<GameObject> sidepotAdded = new List<GameObject>();

    public void DistributePotsToWinner(string winnerId)
    {
        Debug.Log($"DistributePotsToWinner called for Winner ID: {winnerId}");

        float totalWinningAmount = 0;
        totalWinningAmount += potAmount;
        Debug.Log($"Main Pot Winner: Player {winnerId} won Main Pot: {potAmount}");

        Dictionary<string, List<float>> sidePotWinners = new Dictionary<string, List<float>>();
        sidePotWinners.Clear();

        foreach (var entry in sidePotHistory)
        {
            string playerId = entry.Key;
            List<float> playerSidePots = entry.Value;

            foreach (float sidePot in playerSidePots)
            {
                if (!sidePotWinners.ContainsKey(playerId))
                {
                    sidePotWinners[playerId] = new List<float>();
                }
                sidePotWinners[playerId].Add(sidePot);

                if (playerId != winnerId)
                {
                    Debug.Log($"Side Pot Winner: Player {playerId} won Side Pot: {sidePot}");
                }
            }
        }



        Debug.Log($"Final Winning Amount for {winnerId}: {totalWinningAmount}");

        foreach (var entry in sidePotWinners)
        {
            Debug.Log($"[DEBUG] Side Pot Distribution: Player {entry.Key} -> {string.Join(", ", entry.Value)}");
        }

        // **NEW FUNCTION: Move Side Pots to Winners**
        MoveSidePotsToWinners(sidePotWinners);

        WinnerResult result = new WinnerResult()
        {
            WinnerId = winnerId,
            MainPotAmount = potAmount,
            SidePotDetails = sidePotWinners
        };
    }

    public void MoveSidePotsToWinners(Dictionary<string, List<float>> sidePotWinners)
    {
        Dictionary<float, List<string>> sidePotWinnersByAmount = new Dictionary<float, List<string>>();
        sidePotWinnersByAmount.Clear();
        // Group winners by side pot amount
        foreach (var entry in sidePotWinners)
        {
            string winnerId = entry.Key;
            List<float> wonAmounts = entry.Value;

            foreach (float amount in wonAmounts)
            {
                if (!sidePotWinnersByAmount.ContainsKey(amount))
                {
                    sidePotWinnersByAmount[amount] = new List<string>();
                }
                sidePotWinnersByAmount[amount].Add(winnerId);
            }
        }

        // Process each side pot amount
        foreach (var entry in sidePotWinnersByAmount)
        {
            float potAmount = entry.Key;
            List<string> winners = entry.Value;

            List<GameObject> matchingSidePots = sidepotAdded.FindAll(pot => pot.GetComponent<Text>().text == potAmount.ToString());

            if (matchingSidePots.Count < winners.Count)
            {
                Debug.LogWarning($"[WARNING] Not enough side pots found for amount {potAmount}!");
                continue;
            }

            for (int i = 0; i < winners.Count; i++)
            {
                if (i < matchingSidePots.Count)
                {
                    MoveSidePotToWinner(winners[i], potAmount, matchingSidePots[i]);
                }
            }
        }
    }

    private void SplitAndMoveSidePot(List<string> winners, float totalAmount)
    {
        float splitAmount = totalAmount / winners.Count;

        // Find the original side pot
        GameObject originalSidePot = sidepotAdded.Find(pot => pot.GetComponent<Text>().text == totalAmount.ToString());
        if (originalSidePot == null)
        {
            Debug.LogWarning($"[WARNING] No matching Side Pot found for amount {totalAmount}!");
            return;
        }

        List<GameObject> newSplitPots = new List<GameObject>();
        newSplitPots.Clear();
        // Create new prefabs for split amounts
        foreach (string winnerId in winners)
        {
            GameObject newPot = Instantiate(sidePotPrefab, originalSidePot.transform.parent);
            newPot.GetComponent<Text>().text = splitAmount.ToString();
            newSplitPots.Add(newPot);

            MoveSidePotToWinner(winnerId, splitAmount, newPot);
        }

        // Destroy original pot after split
        Destroy(originalSidePot);
        sidepotAdded.Remove(originalSidePot);
    }


    private void MoveSidePotToWinner(string winnerId, float amount, GameObject sidePot)
    {
        PokerPlayer winner = playerSquList.Find(player => player.playerId == winnerId);
        if (winner == null)
        {
            Debug.LogError($"[ERROR] Winner {winnerId} not found in MoveSidePotToWinner!");
            return;
        }

        // **Check if winner is folded**
        if (winner.isFold) // Assuming isFolded is a boolean in PokerPlayer
        {
            Debug.Log($"[INFO] Player {winnerId} is folded. Destroying side pot instead of moving.");
            Destroy(sidePot);
            sidepotAdded.Remove(sidePot);
            return;
        }

        // Move Side Pot to Winner's Position
        sidePot.transform.DOMove(winner.transform.position, 1.5f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Destroy(sidePot);
                sidepotAdded.Remove(sidePot);
            });

        // Balance update
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.joinPlayerDatas[i].userId == winnerId)
            {
                float currentBalance = float.Parse(DataManager.Instance.joinPlayerDatas[i].balance);
                currentBalance += amount;
                DataManager.Instance.joinPlayerDatas[i].balance = currentBalance.ToString();
                if (DataManager.Instance.joinPlayerDatas[0].userId == winnerId)
                {
                    DataManager.Instance.AddAmount((float)(currentBalance), DataManager.Instance.gameId, "Poker-Win-" + DataManager.Instance.gameId, "won", (float)(0), player1.playerNo);
                }
                winner.playerBalanceTxt.text = currentBalance.ToString("G");
            }
        }
    }



    // Class to store winner details
    public class WinnerResult
    {
        public string WinnerId { get; set; }
        public float MainPotAmount { get; set; }
        public Dictionary<string, List<float>> SidePotDetails { get; set; }
    }
    public void ResetRound()
    {
        Debug.Log("Resetting round...");

        // Clear player bets and side pot history
        players.Clear();
        betAmounts.Clear();
        sidePotHistory.Clear();
        sidepotAdded.Clear();

        // Reset main pot amount
        potAmount = 0;
        totalBetAmount = 0;
        potTxt.text = "0";

        // Destroy all side pot UI elements
        foreach (Transform child in sidePotParent)
        {
            Destroy(child.gameObject);
        }
        foreach (PokerPlayer player in playerSquList)
        {
            player.sbIcon.SetActive(false);
            player.bbIcon.SetActive(false);
            player.isBB = false;
            player.isSB = false;
        }
        // Reset player balances UI (optional, if needed)

        Debug.Log("Round reset complete.");
    }



    /* public void DisplayPlayerContributionsInSidePots(string winnerId)
     {
         Debug.Log("\n===== Side Pot Distribution =====");

         // Step 1: Prepare the tracking dictionaries for pot amounts and eligible players
         Dictionary<int, float> potAmounts = new Dictionary<int, float>();
         Dictionary<int, List<string>> potEligiblePlayers = new Dictionary<int, List<string>>();

         // Step 2: Loop through sidePotHistory and populate potAmounts and potEligiblePlayers
         foreach (var entry in sidePotHistory)
         {
             string playerId = entry.Key;
             List<float> contributions = entry.Value;

             // Loop through all contributions (side pots) for this player
             for (int i = 0; i < contributions.Count; i++)
             {
                 // Ensure each pot index is tracked
                 if (!potAmounts.ContainsKey(i + 1))
                 {
                     potAmounts[i + 1] = 0;
                     potEligiblePlayers[i + 1] = new List<string>();
                 }

                 // Add contribution to the respective pot
                 potAmounts[i + 1] += contributions[i];

                 // Add player to the list of eligible players for this pot
                 if (!potEligiblePlayers[i + 1].Contains(playerId))
                 {
                     potEligiblePlayers[i + 1].Add(playerId);
                 }
             }
         }

         // Step 3: Display the pot distributions and handle winner's eligibility
         foreach (var pot in potAmounts.OrderBy(p => p.Key)) // Sort by pot index
         {
             int potIndex = pot.Key;
             float potValue = pot.Value;
             List<string> playersInPot = potEligiblePlayers[potIndex];

             Debug.Log($"\nSide Pot {potIndex}: Amount = ${potValue:F2}");
             Debug.Log("Eligible Players: " + string.Join(", ", playersInPot));

             // Now, check if the winner is eligible for this pot
             if (playersInPot.Contains(winnerId))
             {
                 // If the winner is eligible for this pot
                 Debug.Log($"✅ Winner {winnerId} wins ${potValue:F2} from Side Pot {potIndex}");
             }
             else
             {
                 // If the winner is NOT eligible for this pot
                 Debug.Log($"❌ Winner {winnerId} is NOT eligible for Side Pot {potIndex}, pot split among remaining players");

                 // Split the pot value among eligible players (excluding winner if they're not eligible)
                 float amountPerPlayer = potValue / playersInPot.Count;
                 foreach (string player in playersInPot)
                 {
                     Debug.Log($"💰 Player {player} receives ${amountPerPlayer:F2} from Side Pot {potIndex}");
                 }
             }
         }

         Debug.Log("\n===== Distribution Complete =====");
     }*/








    // New function to distribute side pot winnings
    public void DistributeSidePots(List<string> winnerIds)
    {
        // Distribute winnings to each winner
        for (int i = 0; i < winnerIds.Count; i++)
        {
            string winnerId = winnerIds[i];

            if (i < betAmounts.Count) // Ensure we don't go out of bounds
            {
                float amount = betAmounts[i]; // Winner gets their bet amount from side pot
                                              // ShowWinAmount(winnerId, amount.ToString("F2"));
            }
        }
    }

    // Function to distribute the winnings to the winners based on side pot
    public void DistributeWinnings(string winnerId)
    {
        // Find the winner's bet and distribute the winnings
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerId == winnerId) // Match based on the winner's ID
            {
                float winnings = CalculateWinnings(betAmounts[i]);
                Debug.Log($"Player {players[i].name} with ID {players[i].playerId} wins ${winnings:F2} from the side pots");
                // Here, you can add the winnings to the player's chips (e.g., UpdatePlayerChips(players[i], winnings))
            }
        }
    }

    // Calculate the winnings based on the bet amount (simplified logic for now)
    private float CalculateWinnings(float betAmount)
    {
        // Example calculation: Winner gets all the side pots plus their bet (can be modified as needed)
        float totalWinnings = 0;
        foreach (float bet in betAmounts)
        {
            totalWinnings += bet;
        }
        return totalWinnings;
    }



    #endregion

    public PokerWinDataMaintain MatchResult(params CardSuffle[] cards)
    {
        PokerWinDataMaintain pokerWinData = new PokerWinDataMaintain();
        List<CardSuffle> allCards = cards.ToList();
        allCards = NewSort(allCards);

        Debug.Log("All sorted cards: " + string.Join(", ", allCards.Select(c => c.cardNo + "-" + c.color)));

        // Generate all 5-card combinations (distinct cards)
        List<List<CardSuffle>> possibleHands = GetAllFiveCardCombinations(allCards);
        List<CardSuffle> bestHand = null;
        int bestRank = int.MaxValue;
        List<List<CardSuffle>> bestHands = new List<List<CardSuffle>>();

        foreach (var hand in possibleHands)
        {
            int rank = EvaluateHandRank(hand);
            if (rank < bestRank)
            {
                bestRank = rank;
                bestHand = hand;
                bestHands.Clear();
                bestHands.Add(hand);
            }
            else if (rank == bestRank)  // If hand ranks are equal, check high cards
            {
                bestHands.Add(hand);
            }
        }

        // If there's a tie, determine the winner based on high cards
        if (bestHands.Count > 1)
        {
            bestHand = DetermineWinnerByHighCard(bestHands);
        }

        pokerWinData.ruleNo = bestRank;
        pokerWinData.winList = bestHand;
        Debug.Log("Best hand: " + string.Join(", ", bestHand.Select(c => c.cardNo + "-" + c.color)) + " | Final Rank: " + bestRank);

        return pokerWinData;
    }

    private List<CardSuffle> DetermineWinnerByHighCard(List<List<CardSuffle>> hands)
    {
        List<CardSuffle> bestHand = hands[0];
        foreach (var hand in hands.Skip(1))
        {
            bestHand = CompareHighCards(bestHand, hand);
        }
        return bestHand;
    }

    private List<CardSuffle> CompareHighCards(List<CardSuffle> bestHand, List<CardSuffle> currentHand)
    {
        var sortedBestHand = bestHand.OrderByDescending(c => GetCardValue(c.cardNo)).ToList();
        var sortedCurrentHand = currentHand.OrderByDescending(c => GetCardValue(c.cardNo)).ToList();

        for (int i = 0; i < 5; i++)
        {
            int bestCardValue = GetCardValue(sortedBestHand[i].cardNo);
            int currentCardValue = GetCardValue(sortedCurrentHand[i].cardNo);
            if (bestCardValue > currentCardValue)
            {
                return bestHand;  // Best hand remains the same
            }
            else if (bestCardValue < currentCardValue)
            {
                return currentHand;  // Current hand is better
            }
        }

        return bestHand;  // If all cards are the same, no change
    }

    // Add the hand rank evaluations (Royal Flush, etc.) as previously described


    private List<List<CardSuffle>> GetAllFiveCardCombinations(List<CardSuffle> cards)
    {
        var combinations = new List<List<CardSuffle>>();

        // Generate all 5-card combinations without duplicates
        for (int i = 0; i < cards.Count - 4; i++)
        {
            for (int j = i + 1; j < cards.Count - 3; j++)
            {
                for (int k = j + 1; k < cards.Count - 2; k++)
                {
                    for (int l = k + 1; l < cards.Count - 1; l++)
                    {
                        for (int m = l + 1; m < cards.Count; m++)
                        {
                            combinations.Add(new List<CardSuffle> { cards[i], cards[j], cards[k], cards[l], cards[m] });
                        }
                    }
                }
            }
        }

        return combinations;
    }

    private int EvaluateHandRank(List<CardSuffle> hand)
    {
        if (IsRoyalFlush(hand)) return 1;
        if (IsStraightFlush(hand)) return 2;
        if (IsFourOfAKind(hand)) return 3;
        if (IsFullHouse(hand)) return 4;
        if (IsFlush(hand)) return 5;
        if (IsStraight(hand)) return 6;
        if (IsThreeOfAKind(hand)) return 7;
        if (IsTwoPair(hand)) return 8;
        if (IsOnePair(hand)) return 9;
        return 10; // High Card
    }

    private bool IsRoyalFlush(List<CardSuffle> hand)
    {
        return IsStraightFlush(hand) && hand.Any(c => GetCardValue(c.cardNo) == 14); // Ace is treated as 14
    }

    private bool IsStraightFlush(List<CardSuffle> hand)
    {
        return IsFlush(hand) && IsStraight(hand);
    }

    private bool IsFourOfAKind(List<CardSuffle> hand)
    {
        var grouped = hand.GroupBy(c => GetCardValue(c.cardNo));
        return grouped.Any(g => g.Count() == 4);
    }

    private bool IsFullHouse(List<CardSuffle> hand)
    {
        var groups = hand.GroupBy(c => GetCardValue(c.cardNo)).Select(g => g.Count()).OrderByDescending(x => x).ToList();
        return groups.SequenceEqual(new List<int> { 3, 2 });
    }

    private bool IsFlush(List<CardSuffle> hand)
    {
        return hand.All(c => c.color == hand[0].color);
    }

    private bool IsStraight(List<CardSuffle> hand)
    {
        var sorted = hand.Select(c => GetCardValue(c.cardNo)).OrderBy(n => n).ToList();
        return sorted.SequenceEqual(Enumerable.Range(sorted.First(), 5)) ||
               sorted.SequenceEqual(new List<int> { 14, 5, 4, 3, 2 }); // Ace-low straight
    }

    private bool IsThreeOfAKind(List<CardSuffle> hand)
    {
        return hand.GroupBy(c => GetCardValue(c.cardNo)).Any(g => g.Count() == 3);
    }

    private bool IsTwoPair(List<CardSuffle> hand)
    {
        return hand.GroupBy(c => GetCardValue(c.cardNo)).Count(g => g.Count() == 2) == 2;
    }

    private bool IsOnePair(List<CardSuffle> hand)
    {
        return hand.GroupBy(c => GetCardValue(c.cardNo)).Any(g => g.Count() == 2);
    }

    // Function to get card value, treating Ace as 14
    private int GetCardValue(int cardNo)
    {
        return cardNo == 1 ? 14 : cardNo;
    }

    private List<CardSuffle> NewSort(List<CardSuffle> cards)
    {
        return cards.OrderByDescending(c => GetCardValue(c.cardNo)).ToList();
    }






   
    #endregion


    #region Other Button

    public void GiftButtonClick(PokerPlayer giftPlayer)
    {
        print("gift Button Click");
        giftScreenObj.SetActive(true);
        GiftSendManager.Instance.gameName = "Poker";
        GiftSendManager.Instance.pokerOtherPlayer = giftPlayer;
    }

    public void MessageButtonClick()
    {
        messageScreeObj.SetActive(true);
    }

    /*public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    public void MenuCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseMenuScreen();
    }*/

    public void SettingsButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenSettingsScreen();
    }

    public void SettingsCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSettingsScreen();
    }



    #endregion

    #region Menu Screen

    /*void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    void CloseMenuScreen()
    {
        menuScreenObj.SetActive(false);
    }*/

    void OpenSettingsScreen()
    {
        settingsScreenObj.SetActive(true);
    }

    void CloseSettingsScreen()
    {
        settingsScreenObj.SetActive(false);
    }


    public void LobbyButtonClick()
    {
        settingsScreenObj.SetActive(false);
        exitPanel.gameObject.SetActive(true);
        //Time.timeScale = 0;
    }

    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
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
            //Instantiate(shopPrefab, shopPrefabParent.transform);
            AddBalanceClick();
        }
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

    #region Rule Panel

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }

    public void CloseRuleButton()
    {
        ruleScreenObj.SetActive(false);
    }

    #endregion

    #region Socket

    public void GetWinners(List<int> winnerNo)
    {
        float winnerAmount = potAmount / winnerNo.Count;
        for (int i = 0; i < winnerNo.Count; i++)
        {
            print("Win No : " + winnerNo[i]);
            for (int j = 0; j < playerSquList.Count; j++)
            {
                if (playerSquList[j].playerNo == winnerNo[i] && playerSquList[j].gameObject.activeSelf == true)
                {
                    //Generate Number
                    GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
                    genBetObj.transform.GetChild(1).GetComponent<Text>().text = winnerAmount.ToString();
                    genBetObj.transform.position = targetBetObj.transform.position;
                    totalBetAmount = 0;
                    Debug.Log($"Main Pot after  : ${ potTxt.text}");
                    Debug.Log($"Main Pot after winnerAmount : ${ winnerAmount}");

                    potTxt.text = winnerAmount.ToString();
                    if (playerSquList[j].playerNo == player1.playerNo)
                    {
                        //Add to  winnner Amount

                        float adminPercentage = pokerAdminCommission;

                        float winAmount = winnerAmount;
                        float adminCommssion = (adminPercentage / 100);
                        float playerWinAmount = winAmount - (winAmount * adminCommssion);

                        print(playerWinAmount);

                        if (playerWinAmount != 0)
                        {
                            SoundManager.Instance.CasinoWinSound();
                            DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "Poker-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
                        }
                    }
                    DisplayCurrentBalance();
                    Destroy(genBetObj, 0.4f);
                }

            }
        }
    }

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
            // SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
    }
    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        int noGet = 0;


        noGet = UnityEngine.Random.Range(1, DataManager.Instance.joinPlayerDatas.Count + 1);


        obj.AddField("DeckNo1", UnityEngine.Random.Range(0, 300));
        obj.AddField("DeckNo2", noGet);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 5);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void SetPokerWon(string value)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("WinnerList", value);
        TestSocketIO.Instace.Senddata("PokerWinnerData", obj);
    }

    public void GetRoomData(int deckNo, int dealearNo)
    {
        //print("Deck no : " + deckNo);
        mainList = listStoreDatas[deckNo].noList;
        gameDealerNo = dealearNo;

        foreach (var t in playerSquList.Where(t => t.gameObject.activeSelf == true))
        {
            t.CardGenerate();
        }

        if (isAdmin) return;
        if (waitNextRoundScreenObj.activeSelf)
        {
            waitNextRoundScreenObj.SetActive(false);
        }
        StartGamePlay();
        //for (int i = 0; i < teenPattiPlayers.Count; i++)
        //{
        //    teenPattiPlayers[i].CardGenerate();
        //}
        // mainList = listStoreDatas[deckNo].noList;
        // for (int i = 0; i < mainList.Count; i++)
        // {
        //     for (int j = 0; j < cardSuffles.Count; j++)
        //     {
        //         if (j == mainList[i])
        //         {
        //             cardSufflesGen.Add(cardSuffles[j]);
        //         }
        //     }
        // }
    }

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

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerId == sendPlayerID)
            {
                sendPlayerObj = pokerPlayers[i].fillLine.gameObject;
            }
            else if (pokerPlayers[i].playerId == receivePlayerId)
            {
                receivePlayerObj = pokerPlayers[i].fillLine.gameObject;
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

    }


    public void ChangePlayerTurn(int pNo)
    {

        Debug.Log("PLAYER TURN => " + pNo);

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", pNo);
        TestSocketIO.Instace.Senddata("PokerChangeTurnData", obj);
    }


    bool isCheckTurnPack(int nextPlayerNo)
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].gameObject.activeSelf == true && pokerPlayers[i].playerNo == nextPlayerNo && pokerPlayers[i].isFold == true)
            {
                return true;
            }
        }
        return false;
    }

    public void GetPlayerTurn(int playerNo)
    {
        bool isPlayerNotEnter = false;
        int nextPlayerNo = 0;
        if (playerNo == 5)//5
        {
            nextPlayerNo = 1;
        }
        else
        {
            nextPlayerNo = playerNo + 1;
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
                        }
                    }
                }
            }
        }

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == nextPlayerNo)
            {
                pokerPlayers[i].RestartFillLine();
                if (pokerPlayers[i].playerNo == nextPlayerNo && pokerPlayers[i] == player1)
                {
                    //bottomBox.SetActive(true);
                    Debug.Log("OpenOnScreen");
                    OpenOnScreen();
                }
                else
                {
                    // bottomBox.SetActive(false);
                    OpenOffScreen();
                }

            }
            else
            {
                pokerPlayers[i].NotATurn();

            }
        }
    }

    #endregion

    #region Game Play UI Player

    public void StartGamePlay()
    {
        if (isAdmin)
        {
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        //Checking bet amount is equal of current players
        InvokeRepeating(nameof(CheckBetAmount), 0f, 1f);
        waitNextRoundScreenObj.SetActive(false);
        playerFindScreenObj.SetActive(false);
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            pokerPlayers[i].isOneTimeEnter = false;
            pokerPlayers[i].isFold = false;
            pokerPlayers[i].isTurn = false;
            Debug.Log("Player =>  " + pokerPlayers[i].name + "   pokerPlayers[i].isTurn  => " + pokerPlayers[i].isTurn);
            pokerPlayers[i].isCalled = false;
            pokerPlayers[i].isBot = false;
            pokerPlayers[i].cardImg1.gameObject.SetActive(false);
            pokerPlayers[i].cardImg2.gameObject.SetActive(false);
            pokerPlayers[i].foldImg.SetActive(false);
            pokerPlayers[i].cardImg1.sprite = commonCardImg;
            pokerPlayers[i].cardImg2.sprite = commonCardImg;
            for (int j = 0; j < pokerPlayers[i].playerWinObj.Length; j++)
            {
                pokerPlayers[i].playerWinObj[j].SetActive(false);
            }
        }

        totalBetAmount = 0f;
        potAmount = 0f;
        Debug.Log($"Main Pot after  : ${ potTxt.text}");

        potTxt.text = potAmount.ToString();
        _allBetEqual = false;
        _isFlopShowDone = false;
        _isRiverShowDone = false;
        _isResultAnnounced = false;

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            pokerPlayers[i].gameObject.SetActive(false);
        }

        isGameStarted = true;

        StartCoroutine(DataMaintain());

    }
    IEnumerator DataMaintain()
    {
        playerSquList.Clear();
        if (DataManager.Instance.joinPlayerDatas.Count == 2)
        {
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
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
                else
                {
                    player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            // playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player2.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player3.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player4.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
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
                            player5.playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                    }
                }
            }
        }

        int playerSend = DataManager.Instance.joinPlayerDatas.Count;

        float speed = 0.2f;

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
            SoundManager.Instance.CasinoCardMoveSound();
            obj.transform.position = cardTmpStart.transform.position;

            obj.transform.DOMove(pokerPlayers[i].cardImg1.transform.position, speed).OnComplete(() =>
            {
                Destroy(obj);
                pokerPlayers[i].cardImg1.gameObject.SetActive(true);
            });

            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(speed);
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
            SoundManager.Instance.CasinoCardMoveSound();
            obj.transform.position = cardTmpStart.transform.position;


            obj.transform.DOMove(pokerPlayers[i].cardImg2.transform.position, speed).OnComplete(() =>
            {
                Destroy(obj);
                pokerPlayers[i].cardImg2.gameObject.SetActive(true);
            });
            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(speed);


        DisplayAndSetDealer();



        bool isSB = false;
        bool isBB = false;

        SetSBAndBBFlags(gameDealerNo);

        lastPrice = bbAmount;



        if (player1.delearObj.activeSelf == true)
        {
            //player1.RestartFillLine();
            //player1.PlayerSetBet(lastPrice,);
            //OpenOnScreen();///////////////////////////////////////////
            print("Player is dealer");
        }
        else
        {
            player1.NotATurn();
            OpenOffScreen();
        }



        ResetChecks();
        ResetFillLines();


        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true)
            {
                playerSquList[i].CardGenerate();
            }
        }


        //StartCoroutine(TurnCardShow());
        //StartCoroutine(RiverCardShow());

        player1.DisplayPlayerCard();
        Debug.Log("isGameStop  => " + isGameStop);
        isGameStop = true;
        Debug.Log("isGameStop  => " + isGameStop);
        ActivateBotPlayers();
        PlaceInitialSB_BBBets();
        StartTheTurn();

    }

    void DisplayAndSetDealer()
    {
        int gameDealerNo = this.gameDealerNo;

        for (int i = 0; i < playerSquList.Count; i++)
        {
            PokerPlayer player = playerSquList[i];
            player.delearObj.SetActive(player.playerNo == gameDealerNo);
        }
    }

    private void SetSBAndBBFlags(int dealerPosition)
    {
        if (!isAdmin) return;
        int playerCount = pokerPlayers.Count;

        for (int i = 0; i < playerCount; i++)
        {
            pokerPlayers[i].isSB = false;
            pokerPlayers[i].isBB = false;
            pokerPlayers[i].sbIcon.gameObject.SetActive(false);
            pokerPlayers[i].bbIcon.gameObject.SetActive(false);
        }

        int sbPosition = (dealerPosition) % playerCount;
        int bbPosition = (dealerPosition + 1) % playerCount;

        pokerPlayers[sbPosition].isSB = true;
        pokerPlayers[sbPosition].sbIcon.SetActive(true);
        pokerPlayers[bbPosition].isBB = true;
        pokerPlayers[bbPosition].bbIcon.SetActive(true);

        int sbPlayerNo = pokerPlayers[sbPosition].playerNo;
        int bbPlayerNo = pokerPlayers[bbPosition].playerNo;

        SendSB_BBFlags(sbPlayerNo, bbPlayerNo);
    }

    public void GetSBAndBBFlags(int sbPlayerNo, int bbPlayerNo)
    {
        int playerCount = pokerPlayers.Count;
        sbPlayerNo = 3;
        bbPlayerNo = 4;
        for (int i = 0; i < playerCount; i++)
        {
            pokerPlayers[i].isSB = false;
            pokerPlayers[i].isBB = false;
            pokerPlayers[i].sbIcon.gameObject.SetActive(false);
            pokerPlayers[i].bbIcon.gameObject.SetActive(false);
        }

        int sbPlayerIndex = pokerPlayers.FindIndex(player => player.playerNo == sbPlayerNo);
        int bbPlayerIndex = pokerPlayers.FindIndex(player => player.playerNo == bbPlayerNo);

        if (sbPlayerIndex != -1)
        {
            pokerPlayers[sbPlayerIndex].isSB = true;
            pokerPlayers[sbPlayerIndex].sbIcon.SetActive(true);
        }

        if (bbPlayerIndex != -1)
        {
            pokerPlayers[bbPlayerIndex].isBB = true;
            pokerPlayers[bbPlayerIndex].bbIcon.SetActive(true);
        }

    }
    public int firstPlayer;
    public int lastPlayer;
    private void StartTheTurn()
    {
        int playerCount = pokerPlayers.Count;

        for (int i = 0; i < playerCount; i++)
        {
            PokerPlayer currentPlayer = pokerPlayers[i];
            PokerPlayer nextPlayer = pokerPlayers[(i + 1) % playerCount];

            if (!currentPlayer.isBB) continue;
            Debug.Log("nextPlayer  =>   " + nextPlayer);
            firstPlayer = nextPlayer.playerNo;
            lastPlayer = (nextPlayer.playerNo == 1) ? 5 : nextPlayer.playerNo - 1;
            Debug.Log("lastPlayer  =>  " + lastPlayer);
            nextPlayer.isTurn = true;
            Debug.Log(" nextPlayer.isTurn  =>  " + nextPlayer.isTurn);

            if (nextPlayer.playerId == DataManager.Instance.playerData._id)
            {
                Debug.Log("OpenOnScreen");
                OpenOnScreen();
            }

            return;
        }
    }

    private void PlaceInitialSB_BBBets()
    {
        if (!isAdmin) return;
        int playerCount = pokerPlayers.Count;

        for (int i = 0; i < playerCount; i++)
        {
            PokerPlayer currentPlayer = pokerPlayers[i];

            if (currentPlayer.isSB)
            {
                float sbAmount = this.sbAmount;
                PlaceBet(currentPlayer, sbAmount);
            }
            else if (currentPlayer.isBB)
            {
                float bbAmount = this.bbAmount;
                PlaceBet(currentPlayer, bbAmount);
            }
        }
    }

    private void PlaceBet(PokerPlayer player, float amount)
    {
        if (!player.isBot)
        {
            print("--Real Player--");
            SoundManager.Instance.ButtonClick();

            if (CheckMoney(amount) == false)
            {
                SoundManager.Instance.ButtonClick();
                OpenErrorScreen();
                return;
            }
            SoundManager.Instance.ThreeBetSound();
            BetAnim(player, amount);
            DataManager.Instance.DebitAmount((amount).ToString(), DataManager.Instance.gameId, "Poker-Bet-" + DataManager.Instance.gameId, "game", 1);
            Debug.Log("BET AMOUNT POKER +>" + amount);
            SendPokerBet(player.playerNo, amount, "start");
        }
        else
        {
            print("--Bot Player--" + amount);
            player.PlaceBotStartingBet(amount);
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
        var joinPlayerDatas = DataManager.Instance.joinPlayerDatas;
        int playerCount = joinPlayerDatas.Count;

        for (int i = 0; i < playerCount; i++)
        {
            if (joinPlayerDatas[i].userId.EndsWith("TeenPatti"))
            {
                playerSquList[i].isBot = true;
            }
        }
    }

    #endregion

    #region Bet Anim

    public void GetBotBetNo(int num, int botPlayerNo, float betAmount)
    {
        if (isAdmin) return;
        switch (num)
        {
            case 1:
                {
                    int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                    if (playerSquList[index].isFold) return;
                    BetAnim(playerSquList[index], betAmount);
                    SoundManager.Instance.ThreeBetSound();
                    break;
                }
            case 2:
                {
                    int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                    if (playerSquList[index].isFold) return;
                    BetAnim(playerSquList[index], betAmount);
                    SoundManager.Instance.ThreeBetSound();
                    break;
                }
            case 3:
                {
                    int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
                    if (playerSquList[index].isFold) return;
                    BetAnim(playerSquList[index], betAmount);
                    SoundManager.Instance.ThreeBetSound();
                    break;
                }
            case 4:
                {
                    break;
                }
        }
    }

    public void BetAnim(PokerPlayer player, float amount)
    {
        GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = amount.ToString();
        genBetObj.transform.position = player.avatarImg.transform.position;
        genBetObj.transform.DOMove(player.betObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            player.betAmount += amount;
            totalBetAmount += player.betAmount;
            player.betTxt.text = player.betAmount.ToString();
        });
    }
    public void BetAnimForAllIn(PokerPlayer player, float amount)
    {
        GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = amount.ToString();
        genBetObj.transform.position = player.avatarImg.transform.position;
        genBetObj.transform.DOMove(player.betObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            player.betAmount += amount;

            player.betTxt.text = player.betAmount.ToString();
        });
    }
    public void BetAnimForCheck(PokerPlayer player, float amount)
    {
        GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = amount.ToString();
        genBetObj.transform.position = player.avatarImg.transform.position;
        genBetObj.transform.DOMove(player.betObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            player.betAmount += amount;
            totalBetAmount += player.betAmount;
            player.betTxt.text = "✓";
        });
    }
    public void ResetAmountAfterAllin()
    {
        for (int i = 0; i < playerSquList.Count; i++)
        {
            playerSquList[i].betTxt.text = "" + 0;
            playerSquList[i].betAmount = 0f;
        }
    }
    public void Player1BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player1.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player1.betAmount.ToString();
        genBetObj.transform.position = player1.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            Debug.Log($"Main Pot after  : ${ potTxt.text}");

            potTxt.text = potAmount.ToString();
        });

    }
    public void Player2BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player2.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player2.betAmount.ToString();
        genBetObj.transform.position = player2.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            Debug.Log($"Main Pot after  : ${ potTxt.text}");

            potTxt.text = potAmount.ToString();
        });

    }
    public void Player3BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player3.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player3.betAmount.ToString();
        genBetObj.transform.position = player3.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            Debug.Log($"Main Pot after  : ${ potTxt.text}");

            potTxt.text = potAmount.ToString();
        });

    }
    public void Player4BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player4.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player4.betAmount.ToString();
        genBetObj.transform.position = player4.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            Debug.Log($"Main Pot after  : ${ potTxt.text}");

            potTxt.text = potAmount.ToString();
        });

    }
    public void Player5BetIn()
    {
        GameObject genBetObj = Instantiate(betPrefab, player5.betObj.transform);
        genBetObj.transform.GetChild(1).GetComponent<Text>().text = player5.betAmount.ToString();
        genBetObj.transform.position = player5.betObj.transform.position;
        genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
        {
            Destroy(genBetObj);
            potAmount = totalBetAmount;
            Debug.Log($"Main Pot after  : ${ potTxt.text}");

            potTxt.text = potAmount.ToString();
        });

    }

    public void SendSB_BBFlags(int SbId, int BbId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("SBPlayer", SbId);
        obj.AddField("BBPlayer", BbId);
        TestSocketIO.Instace.Senddata("PokerSendSBBBData", obj);
    }

    public void SendPokerBet(int pNo, float amount, string betType)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("BetAmount", amount);
        obj.AddField("BetType", betType);//raise,allin,call,start
        TestSocketIO.Instace.Senddata("PokerSendBetData", obj);
    }

    public void SendPokerPlayerFold(string foldPlayer)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("FoldPlayerId", foldPlayer);
        TestSocketIO.Instace.Senddata("PokerSendFlodData", obj);

        AllPlayerFoldAndShowWin();
    }

    public void SetPokerWonData(string winnerPlayerId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("WinnerPlayerId", winnerPlayerId);
        //obj.AddField("WinnerList", value);
        obj.AddField("Action", "WinData");
        TestSocketIO.Instace.Senddata("PokerFinalWinnerData", obj);
    }

    public void GetPokerBet(int playerNo, float betAmount, string betType)
    {
        if (betType == "allin")
        {
            isAllIn = true;
            lastPrice = betAmount;
        }
        else if (betType == "raise")
        {
            lastPrice = betAmount;
        }
        else if (betType == "call")
        {
            lastPrice = betAmount;
        }
        else if (betType == "start")
        {

        }

        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerNo == playerNo && pokerPlayers[i].gameObject.activeSelf == true)
            {
                pokerPlayers[i].PlayerSetSocketBet(betAmount, betType);
            }
        }
    }
    public void GetPokerFold(string playerId)
    {
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerId == playerId && pokerPlayers[i].gameObject.activeSelf == true)
            {
                pokerPlayers[i].isFold = true;
                pokerPlayers[i].foldImg.SetActive(true);
            }
        }
        Debug.Log("POKER TURN Player NO  => " + playerNo);

        // ChangePlayerTurn(playerNo);
    }
    #endregion

    #region Win

    public void WinPoker(bool isallin)
    {
        WinBeforeAllDataManage(isallin);
    }

    #endregion


    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {

        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;

            if (DataManager.Instance.joinPlayerDatas.Count == 5 && waitNextRoundScreenObj.activeSelf)
            {
                if (waitNextRoundScreenObj.activeSelf == true)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
            }
        }
        else
        {
            if (!isGameStarted)
            {
                isAdmin = false;
            }
        }
        for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].playerId.Equals(leavePlayerId))
            {
                pokerPlayers[i].isFold = true;
                pokerPlayers[i].foldImg.SetActive(true);
                //pokerPlayers[i].gameObject.SetActive(false);
                StartCoroutine(WaitGameToCompleteRemovePlayer(CheckLeftPlayer, i));
                if (pokerPlayers[i].isTurn)
                {
                    Debug.Log("POKER TURN Player NO  => " + playerNo);

                    ChangePlayerTurn(pokerPlayers[i].playerNo);
                }
            }
        }

        /*for (int i = 0; i < pokerPlayers.Count; i++)
        {
            if (pokerPlayers[i].gameObject.activeSelf == true)
            {
                string playerIdGet = pokerPlayers[i].playerId;

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
                    pokerPlayers[i].isFold = true;
                    pokerPlayers[i].foldImg.SetActive(true);
                    //pokerPlayers[i].gameObject.SetActive(false);
                }
            }

        }*/

        if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            WinPoker(false);
        }


    }
}


