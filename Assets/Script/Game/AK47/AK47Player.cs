using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AK47Player : MonoBehaviour
{
    public Image avatarImg;
    public Text playerNameTxt;
    public Text playerBalence;

    public Image cardImg1;
    public Image cardImg2;
    public Image cardImg3;
    public GameObject delearObj;

    public GameObject[] seeObj;
    public GameObject[] playerWinObj;
    public Image fillLine;

    public GameObject seenImg;
    public GameObject packImg;
    public GameObject blindIMG;

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

    public CardSuffle card1;
    public CardSuffle card2;
    public CardSuffle card3;

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
        if (playerId == DataManager.Instance.playerData._id)
            DataManager.Instance.LoadProfileImage(avatar, avatarImg);
        else
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        //if (AK47Manager.Instance.player1 == this)
        //{
        //    AK47Manager.Instance. = 
        //}
        //Debug.Log("Gaurav HERE 1 ");
        if (!AK47Manager.Instance.isBotActivate) return;

        if (playerWinObj[0].activeSelf == true && AK47Manager.Instance.isWin == false)
        {
            AK47Manager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            AK47Manager.Instance.isWin = false;
        }

        //if(isPack) return;
        if (isTurn && AK47Manager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / AK47Manager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                //Debug.Log("Gaurav HERE 2 ");
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
                    //AK47Manager.Instance.CheckWin();
                    AK47Manager.Instance.skippedChanceObject.SetActive(true);
                    TestSocketIO.Instace.LeaveRoom();
                }
                //else
                //{
                //userTurnCount++;
                //if (userTurnCount >= 4)
                //{
                //}
                //     CheckLife();
                AK47Manager.Instance.ChangeCardStatus("PACK", playerNo, false);
                //}
                Debug.Log("ChangePlayerTurn   =>  " + playerNo);

                AK47Manager.Instance.ChangePlayerTurn(playerNo);
                //if (this == AK47Manager.Instance.player1)
                //{
                //}
                //Pack and Change Turn
            }

            if (isCalled) return;
            if (!isTurn || !isBot) return;
            if (!AK47Manager.Instance.isAdmin) return;
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
        //isTurn = false;
        if (_isFunctionCalled) yield break;
        //BotAutoBet();
        StartCoroutine(BotAutoBetCoroutine());
        //AK47Manager.Instance.BetAnim(this, 0.1f);
    }


    private IEnumerator BotAutoBetCoroutine()
    {
        _isFunctionCalled = true;
        if (isPack) yield break;
        isTurn = true;
        int num1;

        if (!isSeen)
        {
            num1 = Random.Range(0, 2);
            Debug.Log("NUM  => " + num1);
            if (num1 == 0)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                AK47Manager.Instance.ChangeCardStatus("Blind", playerNo, false);
                yield return new WaitForSeconds(1.5f);
            }
            else if (num1 == 1)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);
                AK47Manager.Instance.ChangeCardStatus("SEEN", playerNo, false);
                yield return new WaitForSeconds(1.5f);  // Add a 2-second delay
            }
        }
        int num = Random.Range(1, 6);
        Debug.Log("Randomly generated num => " + num);

        float currentPrice;
        int priceIndex;

        Debug.Log("delearObj.activeInHierarchy  =>  " + delearObj.activeInHierarchy + "  AK47Manager.Instance.roundCounter  =>  " + AK47Manager.Instance.roundCounter);

        if (delearObj.activeInHierarchy && AK47Manager.Instance.roundCounter == 0)
        {
            if (isSeen)
                currentPrice = AK47Manager.Instance.minLimitValue * 2;
            else
                currentPrice = AK47Manager.Instance.minLimitValue;

            priceIndex = AK47Manager.Instance.currentPriceIndex;
        }
        else
        {
            Debug.Log("--------------------------------------------------------------------------------");

            // Check if the player is going to pack based on num == 5 and other conditions
            /*  if (num == 5 && AK47Manager.Instance.winningBotNo != -1 && AK47Manager.Instance.winningBotNo != this.playerNo)
              {
                  // Player is going to pack, no need to call GetAdjacentPlayersPrice
                  Debug.Log("Player will pack, skipping GetAdjacentPlayersPrice...");
                  AK47Manager.Instance.ChangeCardStatus("PACK", playerNo, false);
                  yield break; // Exit coroutine as the player is packing
              }*/

            // Call GetAdjacentPlayersPrice if packing condition is not met
            GetAdjacentPlayersPrice(playerNo, out currentPrice, out priceIndex);
        }

        print("delearObj-------- > " + currentPrice + "---" + priceIndex + "---");

        AK47Manager.Instance.currentPriceValue = currentPrice;
        AK47Manager.Instance.currentPriceIndex = priceIndex;
        Debug.Log("currentPriceValue   =>  " + AK47Manager.Instance.currentPriceValue);

        // Now handle the bets based on the round counter
        switch (AK47Manager.Instance.roundCounter)
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

    }

    private void HandleBetForRounds(int num)
    {
        Debug.Log("SHIGHAM AGAIN   =>  " + num);
        AK47Manager.Instance.CheckActivePlayer();
        Debug.LogError("NUM => " + num);
        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            Debug.Log("playerBalence => " + playerBalence.text + "   currentPriceValue  =>  " + AK47Manager.Instance.currentPriceValue);

            if (playerBalanceValue < AK47Manager.Instance.currentPriceValue)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo + "  ");
                AK47Manager.Instance.ChangeCardStatus("PACK", playerNo, false);
                Debug.Log("<color=red>------------------------------------------- Not enough money ----------------------------------------------</color>");
                return;
            }
            else
            {
                bool isDoubleBet = isSeen && Random.Range(0, 5) == 4;

                float potentialBetAmount = AK47Manager.Instance.currentPriceValue * 2;

                float betAmount = (isDoubleBet && potentialBetAmount <= MainMenuManager.Instance.challLimit)
                    ? potentialBetAmount
                    : AK47Manager.Instance.currentPriceValue;

                AK47Manager.Instance.currentPriceValue = betAmount;
                Debug.Log("BOT CHALL     =>    " + betAmount);
                if (num != 5)
                {
                    SendBotBetNo(num, playerNo, betAmount, AK47Manager.Instance.currentPriceIndex);
                    Debug.LogError("mahadeV - BOT1 =>  " + betAmount);
                    AK47Manager.Instance.BetAnim(this, betAmount, AK47Manager.Instance.currentPriceIndex);
                    SoundManager.Instance.ThreeBetSound();
                    AK47Manager.Instance.ChangePlayerTurn(playerNo);
                }
                /* else if (AK47Manager.Instance.winningBotNo != -1 && AK47Manager.Instance.winningBotNo == this.playerNo)
                 {
                     SendBotBetNo(num, playerNo, betAmount, AK47Manager.Instance.currentPriceIndex);
                     Debug.LogError("mahadeV - BOT2 => " + betAmount);
                     AK47Manager.Instance.BetAnim(this, betAmount, AK47Manager.Instance.currentPriceIndex);
                     SoundManager.Instance.ThreeBetSound();
                     AK47Manager.Instance.ChangePlayerTurn(playerNo);
                 }*/
                else
                {
                    int n = Random.Range(0, 2);

                    Debug.Log("SHIGHAM AGAIN   =  " + n);

                    if (n == 0)
                    {
                        Debug.Log("ChangeCardStatus PACK  =>  " + playerNo);
                        BotShow();
                    }
                    else
                    {
                        SendBotBetNo(num, playerNo, betAmount, AK47Manager.Instance.currentPriceIndex);
                        Debug.LogError("mahadeV - BOT1 =>  " + betAmount);
                        AK47Manager.Instance.BetAnim(this, betAmount, AK47Manager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        AK47Manager.Instance.ChangePlayerTurn(playerNo);
                    }
                }
            }
        }
    }

    private void HandleBetForOtherRounds(int num)
    {
        Debug.Log("SHIGHAM AGAIN  1   =  " + num);
        AK47Manager.Instance.CheckActivePlayer();

        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            if (playerBalanceValue < AK47Manager.Instance.currentPriceValue)
            {
                AK47Manager.Instance.ChangeCardStatus("PACK", playerNo, false);
                Debug.Log("<color=red>------------------------------------------- Not enough money ----------------------------------------------<color>");
                return;
            }
        }

        bool isDoubleBet = isSeen && Random.Range(0, 5) == 4;

        float potentialBetAmount = AK47Manager.Instance.currentPriceValue * 2;

        float betAmount = (isDoubleBet && potentialBetAmount <= MainMenuManager.Instance.challLimit)
            ? potentialBetAmount
            : AK47Manager.Instance.currentPriceValue;

        AK47Manager.Instance.currentPriceValue = betAmount;
        Debug.Log("BOT CHALL     =>    " + betAmount);

        switch (num)
        {
            case 1:
            case 2:
            case 4:
            case 5:
                SendBotBetNo(num, playerNo, betAmount, AK47Manager.Instance.currentPriceIndex);
                Debug.LogError("mahadeV - BOT3");

                AK47Manager.Instance.BetAnim(this, betAmount, AK47Manager.Instance.currentPriceIndex);
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                AK47Manager.Instance.ChangePlayerTurn(playerNo);
                break;

            case 3:
                BotShow();
                break;
        }
    }
    public void BotShow()
    {
        if (AK47Manager.Instance.activePlayerOnTable == 2)
        {
            int n = Random.Range(0, 3);
            if (n == 2)
            {
                AK47Manager.Instance.ShowCardToAllUser();
                AK47Manager.Instance.CheckAllPlayers(AK47Manager.Instance.winnerPlayer);
            }
            else
            {
                Debug.Log("PLAYER PACK =  " + playerNo);
                AK47Manager.Instance.ChangeCardStatus("PACK", playerNo, false);
            }
        }
        else
        {
            Debug.Log("PLAYER PACK =  " + playerNo);
            AK47Manager.Instance.ChangeCardStatus("PACK", playerNo, false);
        }
    }
    AK47Player prevPlayer;
    AK47Player currPlayer;
    public void GetAdjacentPlayersPrice(int playerNo, out float currentPriceValue, out int currentPriceIndex)
    {
        int previousPlayerIndex;
        previousPlayerIndex = (playerNo - 1);
        for (int i = 0; i < AK47Manager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("Checking player: " + previousPlayerIndex);

            // If this player has not packed, we can exit the loop
            if (isPrevPlayerPack(previousPlayerIndex))
            {
                Debug.Log("Valid Player Turn: " + previousPlayerIndex);
                for (int j = 0; j < AK47Manager.Instance.teenPattiPlayers.Count; j++)
                {
                    Debug.Log("pre =  " + previousPlayerIndex + " AK47Manager.Instance.teenPattiPlayers[i].no   " + AK47Manager.Instance.teenPattiPlayers[i].playerNo);
                    if (previousPlayerIndex == AK47Manager.Instance.teenPattiPlayers[j].playerNo)
                    {
                        prevPlayer = AK47Manager.Instance.teenPattiPlayers[j];
                        Debug.Log("PRE   " + prevPlayer.name);
                        

                    }
                }

                break;
            }

            // Otherwise, increment to the next player
            previousPlayerIndex--;

            // If we've exceeded the number of players, wrap around to player 1
            Debug.Log("PREV INDEX =>  " + previousPlayerIndex);
            if (previousPlayerIndex < 1)
            {
                previousPlayerIndex = DataManager.Instance.joinPlayerDatas.Count;
                Debug.Log("PREV INDEX =>  " + previousPlayerIndex);
            }
        }



        var currPlayer = this;
        Debug.Log("prevPlayer => " + prevPlayer.name + "  currPlayer =>  " + currPlayer.name);

        print("current bot player = " + currPlayer + " playerNo = " + playerNo + " global playerNo = " + this.playerNo);
        Debug.Log("AK47Manager.Instance.currentPriceValue   => " + AK47Manager.Instance.currentPriceValue);
        if ((AK47Manager.Instance.currentPriceValue) > MainMenuManager.Instance.challLimit || (AK47Manager.Instance.currentPriceValue * 2) > MainMenuManager.Instance.challLimit)
        {
            Debug.LogError("AK47Manager.Instance.currentPriceValue /////=>  " + AK47Manager.Instance.currentPriceValue);
            if (AK47Manager.Instance.player1.isTurn)
                AK47Manager.Instance.doubleBUtton.SetActive(false);
            Debug.Log("crossChalLimitLastChallSave  =>  " + AK47Manager.Instance.crossChalLimitLastChallSave);
            if (AK47Manager.Instance.crossChalLimitLastChallSave == -1f)
            {
                // This block will execute only the first time
                currentPriceValue = AK47Manager.Instance.currentPriceValue;  // Set current price value
                Debug.LogError("AK47Manager.Instance.currentPriceValue /////=>  " + currentPriceValue);
                currentPriceIndex = AK47Manager.Instance.currentPriceIndex;  // Set current price index

                // Store the initial price value
                AK47Manager.Instance.crossChalLimitLastChallSave = AK47Manager.Instance.currentPriceValue;
                Debug.LogError("AK47Manager.Instance.currentPriceValue /////=>  " + AK47Manager.Instance.crossChalLimitLastChallSave);
            }
            else
            {
                // Use the stored price value on subsequent entries
                Debug.LogError("AK47Manager.Instance.currentPriceValue /////=>  " + AK47Manager.Instance.crossChalLimitLastChallSave);
                currentPriceValue = AK47Manager.Instance.crossChalLimitLastChallSave;
                Debug.LogError("AK47Manager.Instance.currentPriceValue /////=>  " + currentPriceValue);
                currentPriceIndex = AK47Manager.Instance.currentPriceIndex;  // Set current price index

            }
        }
        else
        {
            if (prevPlayer.isBlind && prevPlayer.blindIMG.activeInHierarchy && currPlayer.isBlind)
            {
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                Debug.Log("Both me and previous player are blind." + AK47Manager.Instance.currentPriceValue);

                currentPriceValue = AK47Manager.Instance.currentPriceValue;

                currentPriceIndex = AK47Manager.Instance.currentPriceIndex;
            }
            else if (prevPlayer.isSeen && prevPlayer.seenImg.activeInHierarchy && currPlayer.isSeen)
            {
                // Do not change the value if both players are already seen
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                Debug.Log("Both me and previous player are seen (no change in amount)." + AK47Manager.Instance.currentPriceValue);
                currentPriceValue = AK47Manager.Instance.currentPriceValue;
                currentPriceIndex = AK47Manager.Instance.currentPriceIndex;
            }
            else if (prevPlayer.isBlind && prevPlayer.blindIMG.activeInHierarchy && currPlayer.isSeen)
            {
                // First transition from blind to seen — double the value.
                Debug.Log("I am seen, previous player is blind." + AK47Manager.Instance.currentPriceValue);
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                currentPriceValue = AK47Manager.Instance.currentPriceValue * 2;
                currentPriceIndex = (AK47Manager.Instance.currentPriceIndex + 1) % AK47Manager.Instance.numbers.Length;
            }
            else if (currPlayer.isBlind && prevPlayer.isSeen && prevPlayer.seenImg.activeInHierarchy)
            {
                // If the current player is blind and the previous is seen, halve the value.
                Debug.Log("I am blind, previous player is seen." + AK47Manager.Instance.currentPriceValue);
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                if (AK47Manager.Instance.minLimitValue > AK47Manager.Instance.currentPriceValue / 2)
                {
                    currentPriceValue = AK47Manager.Instance.minLimitValue;
                }
                else
                {
                    currentPriceValue = AK47Manager.Instance.currentPriceValue / 2;

                }
                currentPriceIndex = (AK47Manager.Instance.currentPriceIndex - 1 + AK47Manager.Instance.numbers.Length) % AK47Manager.Instance.numbers.Length;
            }
            else
            {
                // Default case — retain the current value and index.
                Debug.Log("Default case — retain the current value and index.");
                currentPriceValue = AK47Manager.Instance.currentPriceValue;
                currentPriceIndex = AK47Manager.Instance.currentPriceIndex;
            }
            Debug.LogError("currentPriceValue =   " + currentPriceValue);
        }
        // Check player states and determine the appropriate price value and index.
    }
    bool isPrevPlayerPack(int prevPlayerNo)
    {
        for (int i = 0; i < AK47Manager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("I =>  " + i + "  playerSquList[i].gameObject.activeInHierarchy  => " + AK47Manager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy + "  playerSquList[i].playerNo = " + AK47Manager.Instance.teenPattiPlayers[i].playerNo + "   nextPlayerNo = > " + prevPlayerNo + "   playerSquList[i].isPack  =  " + AK47Manager.Instance.teenPattiPlayers[i].isPack);
            if (AK47Manager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy && AK47Manager.Instance.teenPattiPlayers[i].playerNo == prevPlayerNo && AK47Manager.Instance.teenPattiPlayers[i].isPack == false)
            {
                Debug.Log("RETURN TRUE ");
                return true;
            }
        }
        Debug.Log("RETURN FALSE ");
        return false;
    }
    private AK47Player GetNonPackPlayer(int playerIndex, int totalPlayers, int step)
    {
        // Store the initial index to avoid infinite loops
        int startIndex = playerIndex;

        // Iterate until we find a non-packed player or return to the starting index
        while (AK47Manager.Instance.teenPattiPlayers[playerIndex].isPack)
        {
            playerIndex = (playerIndex + step + totalPlayers) % totalPlayers;

            // If we have circled back to the start, break to avoid infinite loop
            if (playerIndex == startIndex)
            {
                return null; // All players are packed; handle this case as needed
            }
        }
        return AK47Manager.Instance.teenPattiPlayers[playerIndex];
    }

    public void SumOfPlayerCards()
    {
        if (card1.cardNo == 4 || card1.cardNo == 7 || card1.cardNo == 13 || card1.cardNo == 14)
            card1.cardNo = 14;
        sumOfCards = card1.cardNo + card2.cardNo + card3.cardNo;
    }



    public void CardGenerate()
    {
        cardImg1.sprite = AK47Manager.Instance.simpleCardSprite;
        cardImg2.sprite = AK47Manager.Instance.simpleCardSprite;
        cardImg3.sprite = AK47Manager.Instance.simpleCardSprite;
        int startIndex = (playerNo - 1) * 3;
        if (startIndex >= 0)
        {
            card1 = new CardSuffle();
            card2 = new CardSuffle();
            card3 = new CardSuffle();
            print("Start Index : " + startIndex);
            card1 = AK47Manager.Instance.cardSuffles[AK47Manager.Instance.mainList[startIndex] - 1];
            card2 = AK47Manager.Instance.cardSuffles[AK47Manager.Instance.mainList[startIndex + 1] - 1];
            card3 = AK47Manager.Instance.cardSuffles[AK47Manager.Instance.mainList[startIndex + 2] - 1];
            print("This is card1 no  -> " + (AK47Manager.Instance.mainList[startIndex] - 1) + "");
            print("This is card2 no  -> " + (AK47Manager.Instance.mainList[startIndex + 1] - 1));
            print("This is card3 no  -> " + (AK47Manager.Instance.mainList[startIndex + 2] - 1));

            TeenPattiWinMaintain winMaintain = AK47Manager.Instance.MatchResult(card1, card2, card3);
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

    }


    public void CardDisplay()
    {
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;
        cardImg3.sprite = card3.cardSprite;
    }


    public void CardPackDisplay()
    {
        card1.cardSprite = AK47Manager.Instance.packCardSprite;
        card2.cardSprite = AK47Manager.Instance.packCardSprite;
        card3.cardSprite = AK47Manager.Instance.packCardSprite;
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
        //AK47Manager.Instance.ShowTextChange();
        fillLine.fillAmount = 1;
        isOneTimeEnter = false;
        isTurn = true;
        isCalled = false;
        _isFunctionCalled = false;
        //if (this == AK47Manager.Instance.player1)
        //{
        //    AK47Manager.Instance.bottomBox.SetActive(true);
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
