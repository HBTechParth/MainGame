using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokerPlayer : MonoBehaviour
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
        if (playerId == DataManager.Instance.playerData._id)
            DataManager.Instance.LoadProfileImage(avatar, avatarImg);
        else
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //if (JokerManager.Instance.player1 == this)
        //{
        //    JokerManager.Instance. = 
        //}
        //Debug.Log("Gaurav HERE 1 ");
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
                    //JokerManager.Instance.CheckWin();
                    JokerManager.Instance.skippedChanceObject.SetActive(true);
                    TestSocketIO.Instace.LeaveRoom();
                }
                //else
                //{
                //userTurnCount++;
                //if (userTurnCount >= 4)
                //{
                //}
                //     CheckLife();
                JokerManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                //}
                Debug.Log("ChangePlayerTurn   =>  " + playerNo);

                JokerManager.Instance.ChangePlayerTurn(playerNo);
                //if (this == JokerManager.Instance.player1)
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
        //isTurn = false;
        if (_isFunctionCalled) yield break;
        //BotAutoBet();
        StartCoroutine(BotAutoBetCoroutine());
        //JokerManager.Instance.BetAnim(this, 0.1f);
    }

    
    private IEnumerator BotAutoBetCoroutine()
    {
        // yield return new WaitForSeconds(1f);
        Debug.Log("ININININ");
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
                JokerManager.Instance.ChangeCardStatus("Blind", playerNo, false);
                yield return new WaitForSeconds(1.5f);
            }
            else if (num1 == 1)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);
                JokerManager.Instance.ChangeCardStatus("SEEN", playerNo, false);
                yield return new WaitForSeconds(1.5f);  // Add a 2-second delay
            }
        }

        // Generate the random 'num' value here before deciding on the betting logic
        int num = Random.Range(1, 6);
        Debug.Log("Randomly generated num => " + num);

        float currentPrice;
        int priceIndex;

        Debug.Log("delearObj.activeInHierarchy  =>  " + delearObj.activeInHierarchy + "  JokerManager.Instance.roundCounter  =>  " + JokerManager.Instance.roundCounter);

        if (delearObj.activeInHierarchy && JokerManager.Instance.roundCounter == 0)
        {
            if (isSeen)
                currentPrice = JokerManager.Instance.minLimitValue * 2;
            else
                currentPrice = JokerManager.Instance.minLimitValue;

            priceIndex = JokerManager.Instance.currentPriceIndex;
        }
        else
        {
            Debug.Log("--------------------------------------------------------------------------------");

            // Check if the player is going to pack based on num == 5 and other conditions
            /*  if (num == 5 && JokerManager.Instance.winningBotNo != -1 && JokerManager.Instance.winningBotNo != this.playerNo)
              {
                  // Player is going to pack, no need to call GetAdjacentPlayersPrice
                  Debug.Log("Player will pack, skipping GetAdjacentPlayersPrice...");
                  JokerManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                  yield break; // Exit coroutine as the player is packing
              }*/

            // Call GetAdjacentPlayersPrice if packing condition is not met
            GetAdjacentPlayersPrice(playerNo, out currentPrice, out priceIndex);
        }

        print("delearObj-------- > " + currentPrice + "---" + priceIndex + "---");

        JokerManager.Instance.currentPriceValue = currentPrice;
        JokerManager.Instance.currentPriceIndex = priceIndex;
        Debug.Log("currentPriceValue   =>  " + JokerManager.Instance.currentPriceValue);

        // Now handle the bets based on the round counter
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
        Debug.Log("SHIGHAM AGAIN   =>  " + num);
        JokerManager.Instance.CheckActivePlayer();
        Debug.LogError("NUM => " + num);
        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            Debug.Log("playerBalence => " + playerBalence.text + "   currentPriceValue  =>  " + JokerManager.Instance.currentPriceValue);

            if (playerBalanceValue < JokerManager.Instance.currentPriceValue)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo + "  ");
                JokerManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                Debug.Log("<color=red>------------------------------------------- Not enough money ----------------------------------------------</color>");
                return;
            }
            else
            {
                bool isDoubleBet = isSeen && Random.Range(0, 5) == 4;

                float potentialBetAmount = JokerManager.Instance.currentPriceValue * 2;

                float betAmount = (isDoubleBet && potentialBetAmount <= MainMenuManager.Instance.challLimit)
                    ? potentialBetAmount
                    : JokerManager.Instance.currentPriceValue;

                JokerManager.Instance.currentPriceValue = betAmount;
                Debug.Log("BOT CHALL     =>    " + betAmount);
                if (num != 5)
                {
                    SendBotBetNo(num, playerNo, betAmount, JokerManager.Instance.currentPriceIndex);
                    Debug.LogError("mahadeV - BOT1 =>  " + betAmount);
                    JokerManager.Instance.BetAnim(this, betAmount, JokerManager.Instance.currentPriceIndex);
                    SoundManager.Instance.ThreeBetSound();
                    JokerManager.Instance.ChangePlayerTurn(playerNo);
                }
                /* else if (JokerManager.Instance.winningBotNo != -1 && JokerManager.Instance.winningBotNo == this.playerNo)
                 {
                     SendBotBetNo(num, playerNo, betAmount, JokerManager.Instance.currentPriceIndex);
                     Debug.LogError("mahadeV - BOT2 => " + betAmount);
                     JokerManager.Instance.BetAnim(this, betAmount, JokerManager.Instance.currentPriceIndex);
                     SoundManager.Instance.ThreeBetSound();
                     JokerManager.Instance.ChangePlayerTurn(playerNo);
                 }*/
                else
                {
                    int n = Random.Range(0, 2);

                    Debug.Log("SHIGHAM AGAIN   =  " + n);

                  /*  if (n == 0)
                    {
                        Debug.Log("ChangeCardStatus PACK  =>  " + playerNo);
                        BotShow();
                    }*/
                 //   else
                    {
                        SendBotBetNo(num, playerNo, betAmount, JokerManager.Instance.currentPriceIndex);
                        Debug.LogError("mahadeV - BOT1 =>  " + betAmount);
                        JokerManager.Instance.BetAnim(this, betAmount, JokerManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        JokerManager.Instance.ChangePlayerTurn(playerNo);
                    }
                }
            }
        }
    }

    private void HandleBetForOtherRounds(int num)
    {
        Debug.Log("SHIGHAM AGAIN  1   =  " + num);
        JokerManager.Instance.CheckActivePlayer();

        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            if (playerBalanceValue < JokerManager.Instance.currentPriceValue)
            {
                JokerManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                Debug.Log("<color=red>------------------------------------------- Not enough money ----------------------------------------------<color>");
                return;
            }
        }

        bool isDoubleBet = isSeen && Random.Range(0, 5) == 4;

        float potentialBetAmount = JokerManager.Instance.currentPriceValue * 2;

        float betAmount = (isDoubleBet && potentialBetAmount <= MainMenuManager.Instance.challLimit)
            ? potentialBetAmount
            : JokerManager.Instance.currentPriceValue;

        JokerManager.Instance.currentPriceValue = betAmount;
        Debug.Log("BOT CHALL     =>    " + betAmount);

        switch (num)
        {
            case 1:
            case 2:
            case 4:
            case 5:
                SendBotBetNo(num, playerNo, betAmount, JokerManager.Instance.currentPriceIndex);
                Debug.LogError("mahadeV - BOT3");

                JokerManager.Instance.BetAnim(this, betAmount, JokerManager.Instance.currentPriceIndex);
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                JokerManager.Instance.ChangePlayerTurn(playerNo);
                break;

            case 3:
                BotShow();
                break;
        }
    }
    public void BotShow()
    {
        if (JokerManager.Instance.activePlayerOnTable == 2)
        {
            int n = Random.Range(0, 3);
            if (n == 2)
            {
                JokerManager.Instance.ShowCardToAllUser();
                JokerManager.Instance.CheckAllPlayers(JokerManager.Instance.winnerPlayer);
            }
            else
            {
                Debug.Log("PLAYER PACK =  " + playerNo);
                JokerManager.Instance.ChangeCardStatus("PACK", playerNo, false);
            }
        }
        else
        {
            Debug.Log("PLAYER PACK =  " + playerNo);
            JokerManager.Instance.ChangeCardStatus("PACK", playerNo, false);
        }
    }
    JokerPlayer prevPlayer;
    JokerPlayer currPlayer;
    public void GetAdjacentPlayersPrice(int playerNo, out float currentPriceValue, out int currentPriceIndex)
    {
        Debug.Log("<color=red> ============= Enter GetAdjacentPlayersPrice ========  </color>");



        int previousPlayerIndex;
        previousPlayerIndex = (playerNo - 1);
        for (int i = 0; i < JokerManager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("Checking player: " + previousPlayerIndex);

            // If this player has not packed, we can exit the loop
            if (isPrevPlayerPack(previousPlayerIndex))
            {
                Debug.Log("Valid Player Turn: " + previousPlayerIndex);
                for (int j = 0; j < JokerManager.Instance.teenPattiPlayers.Count; j++)
                {
                    Debug.Log("pre =  " + previousPlayerIndex + " JokerManager.Instance.teenPattiPlayers[i].no   " + JokerManager.Instance.teenPattiPlayers[i].playerNo);
                    if (previousPlayerIndex == JokerManager.Instance.teenPattiPlayers[j].playerNo)
                    {
                        prevPlayer = JokerManager.Instance.teenPattiPlayers[j];
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
        Debug.Log("JokerManager.Instance.currentPriceValue   => " + JokerManager.Instance.currentPriceValue);
        if ((JokerManager.Instance.currentPriceValue) > MainMenuManager.Instance.challLimit || (JokerManager.Instance.currentPriceValue * 2) > MainMenuManager.Instance.challLimit)
        {
            Debug.LogError("JokerManager.Instance.currentPriceValue /////=>  " + JokerManager.Instance.currentPriceValue);
            if (JokerManager.Instance.player1.isTurn)
                JokerManager.Instance.doubleBUtton.SetActive(false);
            Debug.Log("crossChalLimitLastChallSave  =>  " + JokerManager.Instance.crossChalLimitLastChallSave);
            if (JokerManager.Instance.crossChalLimitLastChallSave == -1f)
            {
                // This block will execute only the first time
                currentPriceValue = JokerManager.Instance.currentPriceValue;  // Set current price value
                Debug.LogError("JokerManager.Instance.currentPriceValue /////=>  " + currentPriceValue);
                currentPriceIndex = JokerManager.Instance.currentPriceIndex;  // Set current price index

                // Store the initial price value
                JokerManager.Instance.crossChalLimitLastChallSave = JokerManager.Instance.currentPriceValue;
                Debug.LogError("JokerManager.Instance.currentPriceValue /////=>  " + JokerManager.Instance.crossChalLimitLastChallSave);
            }
            else
            {
                // Use the stored price value on subsequent entries
                Debug.LogError("JokerManager.Instance.currentPriceValue /////=>  " + JokerManager.Instance.crossChalLimitLastChallSave);
                currentPriceValue = JokerManager.Instance.crossChalLimitLastChallSave;
                Debug.LogError("JokerManager.Instance.currentPriceValue /////=>  " + currentPriceValue);
                currentPriceIndex = JokerManager.Instance.currentPriceIndex;  // Set current price index

            }
        }
        else
        {
            if (prevPlayer.isBlind && prevPlayer.blindIMG.activeInHierarchy && currPlayer.isBlind)
            {
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                Debug.Log("Both me and previous player are blind." + JokerManager.Instance.currentPriceValue);

                currentPriceValue = JokerManager.Instance.currentPriceValue;

                currentPriceIndex = JokerManager.Instance.currentPriceIndex;
            }
            else if (prevPlayer.isSeen && prevPlayer.seenImg.activeInHierarchy && currPlayer.isSeen)
            {
                // Do not change the value if both players are already seen
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                Debug.Log("Both me and previous player are seen (no change in amount)." + JokerManager.Instance.currentPriceValue);
                currentPriceValue = JokerManager.Instance.currentPriceValue;
                currentPriceIndex = JokerManager.Instance.currentPriceIndex;
            }
            else if (prevPlayer.isBlind && prevPlayer.blindIMG.activeInHierarchy && currPlayer.isSeen)
            {
                // First transition from blind to seen — double the value.
                Debug.Log("I am seen, previous player is blind." + JokerManager.Instance.currentPriceValue);
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                currentPriceValue = JokerManager.Instance.currentPriceValue * 2;
                currentPriceIndex = (JokerManager.Instance.currentPriceIndex + 1) % JokerManager.Instance.numbers.Length;
            }
            else if (currPlayer.isBlind && prevPlayer.isSeen && prevPlayer.seenImg.activeInHierarchy)
            {
                // If the current player is blind and the previous is seen, halve the value.
                Debug.Log("I am blind, previous player is seen." + JokerManager.Instance.currentPriceValue);
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                if (JokerManager.Instance.minLimitValue > JokerManager.Instance.currentPriceValue / 2)
                {
                    currentPriceValue = JokerManager.Instance.minLimitValue;
                }
                else
                {
                    currentPriceValue = JokerManager.Instance.currentPriceValue / 2;

                }
                currentPriceIndex = (JokerManager.Instance.currentPriceIndex - 1 + JokerManager.Instance.numbers.Length) % JokerManager.Instance.numbers.Length;
            }
            else
            {
                // Default case — retain the current value and index.
                Debug.Log("Default case — retain the current value and index.");
                currentPriceValue = JokerManager.Instance.currentPriceValue;
                currentPriceIndex = JokerManager.Instance.currentPriceIndex;
            }
            Debug.LogError("currentPriceValue =   " + currentPriceValue);
        }
        // Check player states and determine the appropriate price value and index.

    }

    bool isPrevPlayerPack(int prevPlayerNo)
    {
        for (int i = 0; i < JokerManager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("I =>  " + i + "  playerSquList[i].gameObject.activeInHierarchy  => " + JokerManager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy + "  playerSquList[i].playerNo = " + JokerManager.Instance.teenPattiPlayers[i].playerNo + "   nextPlayerNo = > " + prevPlayerNo + "   playerSquList[i].isPack  =  " + JokerManager.Instance.teenPattiPlayers[i].isPack);
            if (JokerManager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy && JokerManager.Instance.teenPattiPlayers[i].playerNo == prevPlayerNo && JokerManager.Instance.teenPattiPlayers[i].isPack == false)
            {
                Debug.Log("RETURN TRUE ");
                return true;
            }
        }
        Debug.Log("RETURN FALSE ");
        return false;
    }

    private JokerPlayer GetNonPackPlayer(int playerIndex, int totalPlayers, int step)
    {
        // Store the initial index to avoid infinite loops
        int startIndex = playerIndex;

        // Iterate until we find a non-packed player or return to the starting index
        while (JokerManager.Instance.teenPattiPlayers[playerIndex].isPack)
        {
            playerIndex = (playerIndex + step + totalPlayers) % totalPlayers;

            // If we have circled back to the start, break to avoid infinite loop
            if (playerIndex == startIndex)
            {
                return null; // All players are packed; handle this case as needed
            }
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
        //Debug.Log(card1.cardSprite);
        //Debug.Log(card2.cardSprite);
        //Debug.Log(card3.cardSprite);

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
            if (winMaintain.winList != null && winMaintain.winList.Count == 3)
            {
                card1 = winMaintain.winList[0];
                card2 = winMaintain.winList[1];
                card3 = winMaintain.winList[2];
            }
        }
        Debug.Log("1 Card : " + card1.cardSprite);
        Debug.Log("2 Card : " + card2.cardSprite);
        Debug.Log("3 Card : " + card3.cardSprite);

        //if (card1.cardSprite.name == "Joker_Card 1")
        //    cardImg1.sprite = card1.cardSprite;
        //if (card2.cardSprite.name == "Joker_Card 1")
        //    cardImg2.sprite = card2.cardSprite;
        //if (card3.cardSprite.name == "Joker_Card 1")
        //    cardImg3.sprite = card3.cardSprite;

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
        //JokerManager.Instance.ShowTextChange();
        fillLine.fillAmount = 1;
        isOneTimeEnter = false;
        isTurn = true;
        isCalled = false;
        _isFunctionCalled = false;
        //if (this == JokerManager.Instance.player1)
        //{
        //    JokerManager.Instance.bottomBox.SetActive(true);
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
