using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


public class PokerPlayer : MonoBehaviour
{
    public Image avatarImg;
    public Text playerNameTxt;
    public Image cardImg1;
    public Image cardImg2;

    public Text playerBalanceTxt;

    public GameObject[] playerWinObj;
    public Image fillLine;
    public GameObject delearObj;
    public GameObject foldImg;

    public GameObject betObj;
    public float betAmount;
    public float currentBotBetAmount;
    public Text betTxt;


    public int playerNo;

    public string playerId;
    public string lobbyId;


    public int ruleNo;

    public CardSuffle card1;
    public CardSuffle card2;


    public bool isOneTimeEnter = false;
    public bool isTurn = false;
    public bool isFold;
    public bool admin;

    public string avatar;
    public bool isBot;
    public bool isCalled;
    public bool isCheck;
    public bool isAllIn;

    private bool _isFunctionCalled;
    private bool isBotTurnInProgress = false;
    public bool isSB;
    public bool isBB;

    public GameObject sbIcon;
    public GameObject bbIcon;
    // Start is called before the first frame update
    void Start()
    {
        isCalled = false;
        isBot = false;
        betTxt.text = "0";
        _isFunctionCalled = false;
    }

    public void Setup(JoinPlayerData playerData)
    {
        playerNameTxt.text = playerData.userName;
        playerBalanceTxt.text = playerData.balance;
        playerId = playerData.userId;
        lobbyId = playerData.lobbyId;
        playerNo = playerData.playerNo;
        avatar = playerData.avtar;

        // Assuming you have an UpdateAvatar method to handle avatar updates
        UpdateAvatar();

        // Other setup logic specific to your player
    }

    public void UpdateAvatar()
    {
        if (playerId == DataManager.Instance.playerData._id)
            DataManager.Instance.LoadProfileImage(avatar, avatarImg);
        else
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        // if(!PokerGameManager.Instance.isBotActivate) return;

        //if (!PokerGameManager.Instance.isGameStop) return;

        if (!PokerGameManager.Instance.isGameStop)
        {
            UpdateWinStatus();
            return;
        }

        /*if (playerWinObj[0].activeSelf == true && PokerGameManager.Instance.isWin == false)
        {
            PokerGameManager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            PokerGameManager.Instance.isWin = false;
        }

        if (isTurn && PokerGameManager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / PokerGameManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                isOneTimeEnter = true;
                isTurn = false;

                //PokerGameManager.Instance.ChangeCardStatus("PACK", playerNo);
                if (PokerGameManager.Instance.isAdmin == true)
                {
                    admin = true;
                    PokerGameManager.Instance.SendPokerPlayerFold(playerId);
                }
                //}
                PokerGameManager.Instance.ChangePlayerTurn(playerNo);

                //Pack and Change Turn
            }
            if (isCalled) return;
            if(!isTurn || !isBot) return;
            /*if (!TeenPattiManager.Instance.isAdmin) return;
            {
            }#1#
            if (!PokerGameManager.Instance.isAdmin) return;
            StartCoroutine(CallBotFunction());
        }*/

        //UpdateWinStatus();
        if (fillLine.gameObject.activeInHierarchy)
            isTurn = true;

      //  Debug.Log("is turn =>  " + isTurn + "  PokerGameManager.Instance.isWin => " + PokerGameManager.Instance.isWin + "   isAllIn =>  " + isAllIn);
        if (isTurn && PokerGameManager.Instance.isWin == false && !isAllIn)
        {
            TurnOnFiller();
            UpdateFillLine();

            if (fillLine.fillAmount == 0 && !isOneTimeEnter)
            {
                isOneTimeEnter = true;
                EndTurn();
            }
            else if (!isCalled && isBot && !isBotTurnInProgress && !isAllIn)
            {
                StartCoroutine(CallBotFunction());
            }
        }
    }

    private void UpdateWinStatus()
    {
        PokerGameManager pokerGameManager = PokerGameManager.Instance;
        if (playerWinObj[0].activeSelf && !pokerGameManager.isWin)
        {
            pokerGameManager.isWin = true;
        }
        else if (!playerWinObj[0].activeSelf)
        {
            pokerGameManager.isWin = false;
        }
    }

    private void UpdateFillLine()
    {

        float decrement = 1.0f / PokerGameManager.Instance.timerSpeed * Time.fixedDeltaTime;
        fillLine.fillAmount -= decrement;


    }


    private void TurnOnFiller()
    {

        if (!fillLine.gameObject.activeSelf)
        {
            fillLine.gameObject.SetActive(true);
            fillLine.fillAmount = 1;
        }
    }

    private void EndTurn()
    {
        Debug.Log("is turn  =. " + isTurn);
        isTurn = false;
        Debug.Log("is turn  =. " + isTurn);
        fillLine.gameObject.SetActive(false);

        /*// Reset isOneTimeEnter
        isOneTimeEnter = false;*/

        /* if (PokerGameManager.Instance.isAdmin)
         {
             admin = true;
             Debug.Log("SendPokerPlayerFold  =>  " + playerId);
             PokerGameManager.Instance.SendPokerPlayerFold(playerId);
         }*/
        PokerGameManager.Instance.SendPokerPlayerFold(playerId);
        Debug.Log("POKER TURN Player NO  => " + playerNo);
        if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
            PokerGameManager.Instance.ChangePlayerTurn(playerNo);
        else
            RestartFillLine();

    }


    private IEnumerator CallBotFunction()
    {
        /*StartCoroutine(BotTurn());
        yield return new WaitForFixedUpdate();
        isCalled = true;*/
        if (!PokerGameManager.Instance.isAdmin) yield break;
        isBotTurnInProgress = true;
        yield return BotTurn();
        isBotTurnInProgress = false;
        isCalled = true;
    }

    public IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(Random.Range(3, 7));
        if (!PokerGameManager.Instance.isGameStop) yield break;
        isOneTimeEnter = true;
        Debug.Log("is turn  =. " + isTurn);

        isTurn = false;
        Debug.Log("is turn  =. " + isTurn);

        if (_isFunctionCalled) yield break;
        BotAutoTurn();
        //TeenPattiManager.Instance.BetAnim(this, 0.1f);
    }
    int lastPlayer;
    public void BotAutoTurn()
    {

        int num = Random.Range(1, 4);

        GetBotBetAmount();
        print(num + "This is the Card Number");
        print(lastPlayer + "lastPlayer");
        if (PokerGameManager.Instance.prePlayerTurn != -1)
        {
            int[] playerOrder = { 5, 4, 3, 2, 1 }; // Default order for player 1

            if (playerNo == 2) playerOrder = new int[] { 1, 5, 4, 3, 2 };
            else if (playerNo == 3) playerOrder = new int[] { 2, 1, 5, 4, 3 };
            else if (playerNo == 4) playerOrder = new int[] { 3, 2, 1, 5, 4 };
            else if (playerNo == 5) playerOrder = new int[] { 4, 3, 2, 1, 5 };

            lastPlayer = playerNo; // Default to itself if all are folded

            foreach (int prevPlayer in playerOrder)
            {
                var player = PokerGameManager.Instance.GetPlayer(prevPlayer);

                Debug.Log($"Checking Player {prevPlayer}: isFold = {player.isFold}");

                if (!player.isFold)
                {
                    lastPlayer = prevPlayer;
                    break; // Stop once we find the valid last player
                }
            }

            Debug.Log($"Final Last Player: {lastPlayer}");
        }
        Debug.Log($"Final Last Player: {lastPlayer}");

        if (lastPlayer != 0)
        {
            Debug.Log("PokerGameManager.Instance.playerSquList[lastPlayer - 1].isAllIn  =>  " + PokerGameManager.Instance.playerSquList[lastPlayer - 1].isAllIn);
            if (!PokerGameManager.Instance.playerSquList[lastPlayer - 1].isAllIn) // Check only if lastPlayer is NOT All-In
            {
                if (currentBotBetAmount > float.Parse(playerBalanceTxt.text)) // Prevent fold if All-In
                {
                    isFold = true;
                    Debug.Log("Is Fold   => " + playerId);
                    Debug.Log("Is Fold   => " + playerNo);
                    PokerGameManager.Instance.SendPokerPlayerFold(playerId);
                    Debug.Log("POKER TURN Player NO  => " + playerNo);
                    if (!PokerGameManager.Instance.AreAllPlayersAllIn1())
                        PokerGameManager.Instance.ChangePlayerTurn(playerNo);
                    else
                        RestartFillLine();

                    return;
                }
            }
        }





        if (PokerGameManager.Instance.prePlayerTurn != -1 && PokerGameManager.Instance.playerSquList[lastPlayer - 1].isAllIn)
        {

            int n = Random.Range(0, 10);
            Debug.Log("random num => " + n);
            if (n <= 9)
            {
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("playerBalanceTxt.text  = >   " + playerBalanceTxt.text);
                PokerGameManager.Instance.AddPlayerBet(this, float.Parse(playerBalanceTxt.text));
                PokerGameManager.Instance.BetAnimForAllIn(this, float.Parse(playerBalanceTxt.text));
                UpdateBotBalanceAndTextAllIN();
                Debug.Log("BET AMOUNT POKER +>" + 0);
                //    PokerGameManager.Instance.MatchBet(playerNo, 0);
                SendBotBetNo(num, playerNo, float.Parse(playerBalanceTxt.text));

                isAllIn = true;
                PokerGameManager.Instance.lastPlayerdub = playerNo;

                PokerGameManager.Instance.prePlayerTurn = playerNo;
                if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
                    PokerGameManager.Instance.ChangePlayerTurn(playerNo);
                else
                    RestartFillLine();
            }
            else
            {
                isFold = true;
                Debug.Log("Is Fold   => " + playerId);
                Debug.Log("Is Fold   => " + playerNo);
                PokerGameManager.Instance.SendPokerPlayerFold(playerId);
                Debug.Log("POKER TURN Player NO  => " + playerNo);
                if (!PokerGameManager.Instance.AreAllPlayersAllIn1())
                    PokerGameManager.Instance.ChangePlayerTurn(playerNo);
                else
                    RestartFillLine();


            }
            _isFunctionCalled = true;

            return;
        }

        if (PokerGameManager.Instance.prePlayerTurn == -1)
        {
            int n = Random.Range(0, 10);
            Debug.Log("random num => " + n);
            if (n < 1)
            {
                Debug.Log("Random.value");
                PokerGameManager.Instance.AddPlayerBet(this, float.Parse(playerBalanceTxt.text));

                PokerGameManager.Instance.BetAnimForAllIn(this, float.Parse(playerBalanceTxt.text));
                SoundManager.Instance.ThreeBetSound();
                UpdateBotBalanceAndTextAllIN();

                Debug.Log("BET AMOUNT POKER +>" + 0);
                //    PokerGameManager.Instance.MatchBet(playerNo, 0);
                SendBotBetNo(num, playerNo, float.Parse(playerBalanceTxt.text));
                isAllIn = true;
                PokerGameManager.Instance.lastPlayerdub = playerNo;

                PokerGameManager.Instance.prePlayerTurn = playerNo;
                if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
                    PokerGameManager.Instance.ChangePlayerTurn(playerNo);
                else
                    RestartFillLine();

                _isFunctionCalled = true;
                return;
            }
        }


        Debug.Log("PokerGameManager.Instance.prePlayerTurn  =>  " + PokerGameManager.Instance.prePlayerTurn);

        if (PokerGameManager.Instance.prePlayerTurn != -1 && PokerGameManager.Instance.playerSquList[lastPlayer - 1].isCheck)
        {
            int n = Random.Range(0, 10);
            Debug.Log("random num => " + n);
            if (n <= 5)
            {
                Debug.Log("Random.value");
                PokerGameManager.Instance.BetAnimForCheck(this, 0);
                SoundManager.Instance.ThreeBetSound();
                UpdateBotBalanceAndText();
                Debug.Log("BET AMOUNT POKER +>" + 0);
                //    PokerGameManager.Instance.MatchBet(playerNo, 0);
                SendBotBetNo(num, playerNo, 0);
                isCheck = true;
                PokerGameManager.Instance.lastPlayerdub = playerNo;

                PokerGameManager.Instance.prePlayerTurn = playerNo;
                if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
                    PokerGameManager.Instance.ChangePlayerTurn(playerNo);
                else
                    RestartFillLine();

                _isFunctionCalled = true;

                return;
            }
        }

        if (PokerGameManager.Instance.prePlayerTurn == -1)
        {

            int n = Random.Range(0, 10);
            Debug.Log("random num => " + n);
            if (n < 3)
            {
                PokerGameManager.Instance.BetAnimForCheck(this, 0);
                SoundManager.Instance.ThreeBetSound();
                UpdateBotBalanceAndText();
                Debug.Log("BET AMOUNT POKER +>" + 0);
                // PokerGameManager.Instance.MatchBet(playerNo, 0);
                SendBotBetNo(num, playerNo, 0);
                isCheck = true;
                PokerGameManager.Instance.lastPlayerdub = playerNo;

                PokerGameManager.Instance.prePlayerTurn = playerNo;
                if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
                    PokerGameManager.Instance.ChangePlayerTurn(playerNo);
                else
                    RestartFillLine();

                _isFunctionCalled = true;

                return;
            }
        }

        UpdateBotBalanceAndText();
        Debug.Log("BET AMOUNT POKER +>" + currentBotBetAmount);
        SendBotBetNo(num, playerNo, currentBotBetAmount);
        switch (num)
        {
            case 1:
                {
                    PokerGameManager.Instance.BetAnim(this, currentBotBetAmount);
                    SoundManager.Instance.ThreeBetSound();
                    break;
                }
            case 2:
                {
                    // if (PokerGameManager.Instance.raisePrice == 100)
                    // {
                    //     PokerGameManager.Instance.SendPokerBet(playerNo, PokerGameManager.Instance.player1BetAmount, "allin");
                    //
                    // }
                    // else
                    // {
                    //     PokerGameManager.Instance.SendPokerBet(playerNo, PokerGameManager.Instance.player1BetAmount, "raise");
                    // }
                    // break;
                    // PokerGameManager.Instance.SendPokerBet(playerNo, PokerGameManager.Instance.lastPrice, "call");
                    PokerGameManager.Instance.BetAnim(this, currentBotBetAmount);
                    SoundManager.Instance.ThreeBetSound();
                    break;
                }
            case 3:
                {
                    PokerGameManager.Instance.BetAnim(this, currentBotBetAmount);
                    SoundManager.Instance.ThreeBetSound();
                    break;
                }
            case 4:
                {
                    break;
                }
        }
        Debug.Log("POKER TURN Player NO  => " + playerNo);

        PokerGameManager.Instance.prePlayerTurn = playerNo;
        if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
            PokerGameManager.Instance.ChangePlayerTurn(playerNo);
        else
            RestartFillLine();

        _isFunctionCalled = true;
        // if (PokerGameManager.Instance.counter == 5)
        //{

        //     PokerGameManager.Instance.WinPoker();
        // }
    }


    public void PlaceBotStartingBet(float amount)
    {
        if (!CheckSufficientFunds())
        {
            isFold = true;
            Debug.Log("Is Fold   => " + playerId);
            Debug.Log("POKER TURN Player NO  => " + playerNo);

            PokerGameManager.Instance.SendPokerPlayerFold(playerId);
            if (PokerGameManager.Instance.AreAllPlayersAllIn1() == false)
                PokerGameManager.Instance.ChangePlayerTurn(playerNo);
            else
                RestartFillLine();

            return;
        }
        Debug.Log("BET AMOUNT POKER +>" + amount);

        SendBotBetNo(1, playerNo, amount);
        PokerGameManager.Instance.BetAnim(this, amount);
        SoundManager.Instance.ThreeBetSound();
        UpdateBotBalanceAndText();

    }

    private bool CheckSufficientFunds()
    {

        float currentBalance = float.Parse(playerBalanceTxt.text);
        float updatedBalance = currentBalance - currentBotBetAmount;
        Debug.Log("updatedBalance  =>  " + updatedBalance);
        Debug.Log("currentBotBetAmount  =>  " + currentBotBetAmount);
        if (updatedBalance >= 0)
        {
            return true;
        }
        else
        {
            Debug.LogError("Bot does not have sufficient funds to bet.");
            return false;
        }
    }



    public float GetBotBetAmount()
    {
        float botBetAmount = 0f;

        // Find the maximum bet amount among all players
        float maxBetAmount = PokerGameManager.Instance.playerSquList
            .Select(player => player.betAmount)
            .Prepend(0f)
            .Max();

        Debug.Log($"[BOT BET] Max Bet Amount Among Players: {maxBetAmount}");
        Debug.Log($"[BOT BET] Current Bot Bet Amount: {betAmount}");

        // Calculate the bot's bet amount based on the maximum bet amount and the bot's current bet amount
        if (maxBetAmount > betAmount)
        {
            botBetAmount = maxBetAmount - betAmount;
            Debug.Log($"[BOT BET] Bot needs to match max bet. New Bet: {botBetAmount}");
        }

        if (botBetAmount == 0)
        {
            botBetAmount = PokerGameManager.Instance.bbAmount;
            Debug.Log($"[BOT BET] Bot had 0 bet, setting to BB Amount: {botBetAmount}");
        }

        currentBotBetAmount = botBetAmount;
        Debug.Log($"[BOT BET] Final Bot Bet Amount: {botBetAmount}");

        return botBetAmount;
    }


    public void UpdateBotBalanceAndText()
    {
        Debug.Log("UpdateBotBalanceAndText");
        if (!isFold && isBot)
        {
            Debug.Log("playerBalanceTxt => " + playerBalanceTxt.text);
            Debug.Log("playerBalanceTxt => " + currentBotBetAmount);
            float currentBalance = float.Parse(playerBalanceTxt.text);
            float updatedBalance = currentBalance - currentBotBetAmount;
            playerBalanceTxt.text = updatedBalance.ToString();

            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId == playerId)
                {
                    DataManager.Instance.joinPlayerDatas[i].balance = updatedBalance.ToString();
                }
            }
        }
    }
    public void UpdateBotBalanceAndTextAllIN()
    {
        Debug.Log("UpdateBotBalanceAndText");
        if (!isFold && isBot)
        {

            playerBalanceTxt.text = "" + 0;

            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId == playerId)
                {
                    DataManager.Instance.joinPlayerDatas[i].balance = 0.ToString();
                }
            }
        }
    }



    public void CardGenerate()
    {

        int startIndex = (playerNo - 1) * 2;
        if (startIndex >= 0)
        {
            card1 = new CardSuffle();
            card2 = new CardSuffle();
            //print("Start Index : " + startIndex);
            card1 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex] - 1];
            card2 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex + 1] - 1];


            /*if (playerNo == 1)
            {
                cardImg1.sprite = card1.cardSprite;
                cardImg2.sprite = card2.cardSprite;
            }
            else
            {
                cardImg1.sprite = PokerGameManager.Instance.simpleCardSprite;
                cardImg2.sprite = PokerGameManager.Instance.simpleCardSprite;
            }*/
            cardImg1.sprite = PokerGameManager.Instance.simpleCardSprite;
            cardImg2.sprite = PokerGameManager.Instance.simpleCardSprite;

        }


        int startIndex5 = (DataManager.Instance.joinPlayerDatas.Count * 2);
        PokerGameManager.Instance.card1 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5] - 1];
        PokerGameManager.Instance.card2 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 1] - 1];
        PokerGameManager.Instance.card3 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 2] - 1];
        PokerGameManager.Instance.card4 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 3] - 1];
        PokerGameManager.Instance.card5 = PokerGameManager.Instance.cardSuffles[PokerGameManager.Instance.mainList[startIndex5 + 4] - 1];

    }

    public void RestartFillLine()
    {
        fillLine.gameObject.SetActive(false);
        Debug.Log("Plyer   =>  " + playerNo + "    fillLine.gameObject  =>  " + fillLine.gameObject.activeInHierarchy);
        fillLine.fillAmount = 1;
        isOneTimeEnter = false;
        Debug.Log("is turn  =. " + isTurn);

        isTurn = true;
        Debug.Log("is turn  =. " + isTurn);

        isCalled = false;
        isCheck = false;
        _isFunctionCalled = false;
        if (isAllIn)
        {
            PokerGameManager.Instance.downObjectOnObj.SetActive(false);
        }
        //if (this == TeenPattiManager.Instance.player1)
        //{
        //    TeenPattiManager.Instance.bottomBox.SetActive(true);
        //}

    }
    public void NotATurn()
    {
        isOneTimeEnter = false;
        isTurn = false;
        _isFunctionCalled = false;
        fillLine.gameObject.SetActive(false);
        fillLine.fillAmount = 1;
    }

    public void DisplayPlayerCard()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;
    }

    public PokerWinDataMaintain CardDisplay()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;

        Debug.Log("card1  => " + card1.cardNo + "  card2  =>" + card2.cardNo + "    Player No =  " + playerNo);
        return PokerGameManager.Instance.MatchResult(card1, card2, PokerGameManager.Instance.card1, PokerGameManager.Instance.card2, PokerGameManager.Instance.card3, PokerGameManager.Instance.card4, PokerGameManager.Instance.card5);
    }

    public void PlayerSetBet(float amount, string betType)
    {
        PokerGameManager.Instance.BetAnim(this, amount);
    }

    public void PlayerSetSocketBet(float amount, string betType)
    {
        PokerGameManager.Instance.BetAnim(this, amount);
    }


    public void SendBotBetNo(int no, int botPlayerNo, float botBetAmount)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("BetAmount", botBetAmount);
        obj.AddField("BotPlayerNo", botPlayerNo);
        obj.AddField("BotNo", no);
        obj.AddField("Action", "BotBetData");
        TestSocketIO.Instace.Senddata("PokerBotBetNo", obj);
    }
}
