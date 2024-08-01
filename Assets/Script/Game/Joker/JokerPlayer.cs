using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokerPlayer : MonoBehaviour
{
    public Image avatarImg;
    public Text playerNameTxt;
    public Image cardImg1;
    public Image cardImg2;
    public Image cardImg3;
    public GameObject delearObj;

    public GameObject[] seeObj;
    public GameObject[] playerWinObj;
    public Image fillLine;

    public GameObject seenImg;
    public GameObject packImg;

    public int playerNo;
    public GameObject sendBetObj;

    public int sumOfCards = 0;

    public bool isTurn;
    public bool isOneTimeEnter;

    public bool isPack;
    public bool isSeen;
    public bool isBlind;

    public string playerId;
    public string lobbyId;

    public JokerManager.CardSuffle card1;
    public JokerManager.CardSuffle card2;
    public JokerManager.CardSuffle card3;
    public JokerManager.CardSuffle jokerCard;

    public int ruleNo;
    public string avatar;

    public bool isBot;
    public bool isCalled;
    private bool _isFunctionCalled;
    public int userTurnCount;
    public GameObject[] boxArray;
    public int inactiveCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        isCalled = false;
        isBot = false;
        _isFunctionCalled = false;
        userTurnCount = 0;
    }

    public void UpdateAvatar()
    {
        if(playerId == DataManager.Instance.playerData._id)
            DataManager.Instance.LoadProfileImage(avatar, avatarImg);
        else
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if (TeenPattiManager.Instance.player1 == this)
        //{
        //    TeenPattiManager.Instance. = 
        //}
        if (!JokerManager.Instance.isBotActivate) return;

        if (playerWinObj[0].activeSelf == true && JokerManager.Instance.isWin == false)
        {
            JokerManager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            JokerManager.Instance.isWin = false;
        }

        //if(isPack) return;
        if (isTurn && JokerManager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / JokerManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                isOneTimeEnter = true;
                isTurn = false;
                if (playerId.Equals(DataManager.Instance.playerData._id))
                {
                    //isPack = true;
                    //isBlind = false;
                    //isSeen = false;
                    //for (int j = 0; j < seeObj.Length; j++)
                    //{
                    //    seeObj[j].SetActive(false);
                    //}
                    //packImg.SetActive(true);
                    //TeenPattiManager.Instance.CheckWin();
                    JokerManager.Instance.skippedChanceObject.SetActive(true);
                    TestSocketIO.Instace.LeaveRoom();
                }
                //else
                //{
                //userTurnCount++;
                //if (userTurnCount >= 4)
                //{
                //    JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
                //}
                CheckLife();
                //}
                JokerManager.Instance.ChangePlayerTurn(playerNo);
                //if (this == TeenPattiManager.Instance.player1)
                //{
                //}
                //Pack and Change Turn
            }

            if (isCalled) return;
            if (!isTurn || !isBot) return;
            if (!JokerManager.Instance.isAdmin) return;
            StartCoroutine(CallBotFunction());
            print("---------------------------Bot is called-----------------------------------");
        }
    }

    private void CheckLife()
    {
        if (inactiveCount >= boxArray.Length)
        {
            // All game objects are already inactive, do nothing
            return;
        }

        boxArray[boxArray.Length - 1 - inactiveCount].SetActive(false);
        inactiveCount++;
    }


    public void SetActiveTrue()
    {
        foreach (var t in boxArray)
        {
            t.gameObject.SetActive(true);
        }
    }



    private IEnumerator CallBotFunction()
    {
        StartCoroutine(BotTurn());
        yield return new WaitForFixedUpdate();
        isCalled = true;
    }


    private IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(3f);
        isOneTimeEnter = true;
        isTurn = false;
        if (_isFunctionCalled) yield break;
        //BotAutoBet();
        StartCoroutine(BotAutoBetCoroutine());
        //TeenPattiManager.Instance.BetAnim(this, 0.1f);
    }

    //private void BotAutoBet()
    //{
    //    int num = Random.Range(1, 6);

    //    SendBotBetNo(num, playerNo);

    //    switch (JokerManager.Instance.roundCounter)
    //    {
    //        //after 1st round
    //        case <= 1:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //            }

    //            break;
    //        case 2:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
    //                        //TeenPattiManager.Instance.BetAnim(this, 5f);
    //                        break;
    //                    }
    //            }

    //            break;

    //        case 3:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        if (isPack) return;
    //                        if (!isSeen)
    //                        {
    //                            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
    //                        }
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
    //                        //JokerManager.Instance.BetAnim(this, 5f);
    //                        break;
    //                    }
    //            }

    //            break;
    //        case >= 4:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        if (isPack) return;
    //                        JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //            }

    //            break;
    //    }
    //    JokerManager.Instance.ChangePlayerTurn(playerNo);

    //    _isFunctionCalled = true;
    //}

    private IEnumerator BotAutoBetCoroutine()
    {
        _isFunctionCalled = true;
        if (isPack) yield break;

        if (!isSeen)
        {
            JokerManager.Instance.ChangeCardStatus("SEEN", playerNo);
            yield return new WaitForSeconds(1.5f);  // Add a 2-second delay
        }
        float currentPrice;
        int priceIndex;
        if (delearObj.activeInHierarchy && JokerManager.Instance.roundCounter == 0)
        {
            currentPrice = JokerManager.Instance.minLimitValue;
            priceIndex = JokerManager.Instance.currentPriceIndex;
        }
        else
            GetAdjacentPlayersPrice(playerNo, out currentPrice, out priceIndex);
        print("-------- > " + currentPrice + "---" + priceIndex + "---");

        JokerManager.Instance.currentPriceValue = currentPrice;
        JokerManager.Instance.currentPriceIndex = priceIndex;

        int num = Random.Range(1, 6);
        //SendBotBetNo(num, playerNo, currentPrice, priceIndex);

        switch (JokerManager.Instance.roundCounter)
        {
            case <= 1:
            case 2:
            case 3:
            case 4:
            case 5:
                HandleBetForRounds(num);
                break;

            default:
                HandleBetForOtherRounds(num);
                break;
        }

        //JokerManager.Instance.ChangePlayerTurn(playerNo);
    }

    private void HandleBetForRounds(int num)
    {
        if (num != 5)
        {
            SendBotBetNo(num, playerNo, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
            JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
            SoundManager.Instance.ThreeBetSound();
            JokerManager.Instance.ChangePlayerTurn(playerNo);

        }
        else if (JokerManager.Instance.winningBotNo != -1 && JokerManager.Instance.winningBotNo == this.playerNo)
        {
            SendBotBetNo(num, playerNo, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
            JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
            SoundManager.Instance.ThreeBetSound();
            JokerManager.Instance.ChangePlayerTurn(playerNo);
        }
        else
        {
            JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
        }
    }

    private void HandleBetForOtherRounds(int num)
    {
        switch (num)
        {
            case 1:
            case 2:
            case 4:
            case 5:
                SendBotBetNo(num, playerNo, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
                JokerManager.Instance.BetAnim(this, JokerManager.Instance.currentPriceValue, JokerManager.Instance.currentPriceIndex);
                SoundManager.Instance.ThreeBetSound();
                JokerManager.Instance.ChangePlayerTurn(playerNo);
                break;

            case 3:
                /*JokerManager.Instance.ShowCardToAllUser();
                JokerManager.Instance.CheckFinalWinner("Show");*/
                JokerManager.Instance.ChangeCardStatus("PACK", playerNo);
                break;
        }
    }

    public void GetAdjacentPlayersPrice(int playerNo, out float currentPriceValue, out int currentPriceIndex)
    {
        int totalPlayers = JokerManager.Instance.teenPattiPlayers.Count;

        int previousPlayerIndex = (playerNo - 2 + totalPlayers) % totalPlayers;
        var prevPlayer = GetNonPackPlayer(previousPlayerIndex, totalPlayers, -1);

        var currPlayer = /*JokerManager.Instance.teenPattiPlayers[playerNo - 1]*/this;

        if (prevPlayer.isBlind && currPlayer.isBlind)
        {
            currentPriceValue = JokerManager.Instance.numbers[JokerManager.Instance.currentPriceIndex];
            currentPriceIndex = JokerManager.Instance.currentPriceIndex;
        }
        else if (prevPlayer.isSeen && currPlayer.isSeen)
        {
            currentPriceValue = JokerManager.Instance.numbers[JokerManager.Instance.currentPriceIndex];
            currentPriceIndex = JokerManager.Instance.currentPriceIndex;
        }
        else if (prevPlayer.isBlind && currPlayer.isSeen)
        {
            currentPriceValue = JokerManager.Instance.numbers[(JokerManager.Instance.currentPriceIndex + 1) % JokerManager.Instance.numbers.Length];
            currentPriceIndex = (JokerManager.Instance.currentPriceIndex + 1) % JokerManager.Instance.numbers.Length;
        }
        else if (currPlayer.isBlind && prevPlayer.isSeen)
        {
            currentPriceValue = JokerManager.Instance.numbers[(JokerManager.Instance.currentPriceIndex - 1 + JokerManager.Instance.numbers.Length) % JokerManager.Instance.numbers.Length];
            currentPriceIndex = (JokerManager.Instance.currentPriceIndex - 1 + JokerManager.Instance.numbers.Length) % JokerManager.Instance.numbers.Length;
        }
        else
        {
            currentPriceValue = JokerManager.Instance.numbers[(JokerManager.Instance.currentPriceIndex - 1 + JokerManager.Instance.numbers.Length) % JokerManager.Instance.numbers.Length];
            currentPriceIndex = (JokerManager.Instance.currentPriceIndex - 1 + JokerManager.Instance.numbers.Length) % JokerManager.Instance.numbers.Length;
        }
    }

    private JokerPlayer GetNonPackPlayer(int playerIndex, int totalPlayers, int step)
    {
        while (JokerManager.Instance.teenPattiPlayers[playerIndex].isPack)
        {
            playerIndex = (playerIndex + step + totalPlayers) % totalPlayers;
        }
        return JokerManager.Instance.teenPattiPlayers[playerIndex];
    }

    public void SumOfPlayerCards()
    {
        sumOfCards = card1.cardNo + card2.cardNo + card3.cardNo;
    }



    public void CardGenerate()
    {
        cardImg1.sprite = JokerManager.Instance.simpleCardSprite;
        cardImg2.sprite = JokerManager.Instance.simpleCardSprite;
        cardImg3.sprite = JokerManager.Instance.simpleCardSprite;
        int startIndex = (playerNo - 1) * 3;
        if (startIndex >= 0)
        {
            card1 = new JokerManager.CardSuffle();
            card2 = new JokerManager.CardSuffle();
            card3 = new JokerManager.CardSuffle();
            print("Start Index : " + startIndex);
            card1 = JokerManager.Instance.cardSuffles[JokerManager.Instance.mainList[startIndex] - 1];
            card2 = JokerManager.Instance.cardSuffles[JokerManager.Instance.mainList[startIndex + 1] - 1];
            //card3 = JokerManager.Instance.cardSuffles[JokerManager.Instance.mainList[startIndex + 2] - 1];//card3 will always be JOKER
            card3 = jokerCard;
            print("This is card1 no  -> " + (JokerManager.Instance.mainList[startIndex] - 1));
            print("This is card2 no  -> " + (JokerManager.Instance.mainList[startIndex + 1] - 1));
            print("This is card3 no  -> " + (JokerManager.Instance.mainList[startIndex + 2] - 1));

            JokerWinMaintain winMaintain = JokerManager.Instance.MatchResult(card1, card2, card3);
            ruleNo = winMaintain.ruleNo;
            if (winMaintain.ruleNo == 1 || winMaintain.ruleNo == 5)
            {
                card1 = winMaintain.winList[0];
                card2 = winMaintain.winList[1];
                card3 = winMaintain.winList[2];
            }
            if (winMaintain.ruleNo == 2 || winMaintain.ruleNo == 3 || winMaintain.ruleNo == 4 || winMaintain.ruleNo == 6)
            {
                card1 = winMaintain.winList[0];
                card2 = winMaintain.winList[1];
                card3 = winMaintain.winList[2];
            }
        }
        //else if(startIndex == 0)//This is to give custom cards to player for testing cards logic (Remove >= and add > in first condition to use this)
        //{
        //    card1 = new JokerManager.CardSuffle();
        //    card2 = new JokerManager.CardSuffle();
        //    card3 = new JokerManager.CardSuffle();
        //    print("Start Index : " + startIndex);
        //    card1 = JokerManager.Instance.cardSuffles[23];
        //    card2 = JokerManager.Instance.cardSuffles[12];
        //    //card3 = JokerManager.Instance.cardSuffles[JokerManager.Instance.mainList[startIndex + 2] - 1];//card3 will always be JOKER
        //    card3 = jokerCard;
        //    //print("This is card1 no  -> " + (JokerManager.Instance.mainList[startIndex] - 1));
        //    //print("This is card2 no  -> " + (JokerManager.Instance.mainList[startIndex + 1] - 1));
        //    //print("This is card3 no  -> " + (JokerManager.Instance.mainList[startIndex + 2] - 1));

        //    JokerWinMaintain winMaintain = JokerManager.Instance.MatchResult(card1, card2, card3);
        //    ruleNo = winMaintain.ruleNo;
        //    if (winMaintain.ruleNo == 1 || winMaintain.ruleNo == 5)
        //    {
        //        card1 = winMaintain.winList[0];
        //        card2 = winMaintain.winList[1];
        //        card3 = winMaintain.winList[2];
        //    }
        //    if (winMaintain.ruleNo == 2 || winMaintain.ruleNo == 3 || winMaintain.ruleNo == 4 || winMaintain.ruleNo == 6)
        //    {
        //        card1 = winMaintain.winList[0];
        //        card2 = winMaintain.winList[1];
        //        card3 = winMaintain.winList[2];
        //    }
        //}

    }


    public void CardDisplay()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;
        cardImg3.sprite = card3.cardSprite;
    }


    public void CardPackDisplay()
    {
        card1.cardSprite = JokerManager.Instance.packCardSprite;
        card2.cardSprite = JokerManager.Instance.packCardSprite;
        card3.cardSprite = JokerManager.Instance.packCardSprite;
    }

    public void SendBotBetNo(int no, int botPlayerNo, float prize, int index)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("BotPlayerNo", botPlayerNo);
        obj.AddField("BotNo", no);
        obj.AddField("CurrentAmount", prize);
        obj.AddField("CurrentIndex", index);
        obj.AddField("Action", "BotBetData");
        TestSocketIO.Instace.Senddata("TeenPattiBotBetNo", obj);
    }




    public void RestartFillLine()
    {
        //TeenPattiManager.Instance.ShowTextChange();
        fillLine.fillAmount = 1;
        isOneTimeEnter = false;
        isTurn = true;
        isCalled = false;
        _isFunctionCalled = false;
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
        fillLine.fillAmount = 0;
    }
}
