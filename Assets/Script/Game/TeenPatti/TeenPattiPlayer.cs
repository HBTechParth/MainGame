using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TeenPattiPlayer : MonoBehaviour
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
        //if (TeenPattiManager.Instance.player1 == this)
        //{
        //    TeenPattiManager.Instance. = 
        //}
        if (!TeenPattiManager.Instance.isBotActivate) return;

        if (playerWinObj[0].activeSelf == true && TeenPattiManager.Instance.isWin == false)
        {
            TeenPattiManager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            TeenPattiManager.Instance.isWin = false;
        }

        //if(isPack) return;
        if (isTurn && TeenPattiManager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / TeenPattiManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                isOneTimeEnter = true;
                isTurn = false;
                Debug.Log("ChangeCardStatus PACK   " + this.name + " Player NO =>  " + playerNo);
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
                    TeenPattiManager.Instance.skippedChanceObject.SetActive(true);
                    Debug.Log("LeaveRoom  " + this.name);

                    TestSocketIO.Instace.LeaveRoom();

                }
                //else

                //{
                //userTurnCount++;
                //if (userTurnCount >= 4)
                //{ 

                //}
                //CheckLife();
                //}
                //if (this == TeenPattiManager.Instance.player1)
                Debug.Log("ChangeCardStatus   =>  " + playerNo);
                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                //{
                //}
                //Pack and Change Turn
                Debug.Log("ChangePlayerTurn   =>  " + playerNo);
                TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
            }

            if (isCalled) return;
            if (!isTurn || !isBot) return;
            if (!TeenPattiManager.Instance.isAdmin) return;
            StartCoroutine(CallBotFunction());
            print("---------------------------   Bot is called   ------------------" + playerNo);
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


    /*  private IEnumerator BotTurn()
      {
          float waitTime = GetRandomWaitTime(); // Get the random wait time
          Debug.Log("Wait time: " + waitTime + " seconds");

          yield return new WaitForSeconds(waitTime); // Wait for the calculated random time
          isOneTimeEnter = true;
          isTurn = false;
          if (_isFunctionCalled) yield break;
          StartCoroutine(BotAutoBetCoroutine());
          //TeenPattiManager.Instance.BetAnim(this, 0.1f);
      }

      private float GetRandomWaitTime()
      {
          float randomValue = Random.Range(0f, 100f); // Generate a random value between 0 and 100

          // Assign probabilities
          if (randomValue <= 90f) // 90% chance for a wait between 0 and 10 seconds
          {
              return Random.Range(1f, 3f);
          }
          else if (randomValue <= 95f) // 5% chance for a wait between 10 and 23 seconds
          {
              return Random.Range(10f, 23f);
          }
          else // 5% chance for a wait of 25 seconds
          {
              return 25f;
          }
      }
  */
    private IEnumerator BotTurn()
    {
        yield return new WaitForSeconds(3f);
        isOneTimeEnter = true;
        isTurn = false;
        if (_isFunctionCalled) yield break;
        StartCoroutine(BotAutoBetCoroutine());
        //TeenPattiManager.Instance.BetAnim(this, 0.1f);
    }

    /*private void BotAutoBet()
    {
        int num = Random.Range(1, 6);
        SendBotBetNo(num, playerNo);
        /*if(playerNo == TeenPattiManager.Instance.winningBotNo)//this makes sure that bot does not pack when it has better cards than player
        {
            if (isPack) return;
            if (TeenPattiManager.Instance.boxDisplayCount > 10)
            {
                TeenPattiManager.Instance.ShowCardToAllUser();
                TeenPattiManager.Instance.CheckFinalWinner("Show");
            }
            if (!isSeen)
            {
                TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
            }
            TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
            SoundManager.Instance.ThreeBetSound();
            TeenPattiManager.Instance.ChangePlayerTurn(playerNo);

            _isFunctionCalled = true;
            return;
        }#1#
        switch (TeenPattiManager.Instance.roundCounter)
        {
            //after 1st round
            case <= 1:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
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
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        //TeenPattiManager.Instance.BetAnim(this, 5f);
                        break;
                    }
                }

                break;
            
            case 3:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        //TeenPattiManager.Instance.BetAnim(this, 5f);
                        break;
                    }
                }

                break;
            case  4:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }
                break;
            case 5:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        if (!isSeen)
                        {
                            TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                        }
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                        //TeenPattiManager.Instance.BetAnim(this, 5f);
                        break;
                    }
                }
                break;
            case >= 7:
                switch (num)
                {
                    case 1:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 2:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 3:
                    {
                        if(isPack) return;
                        //TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                            TeenPattiManager.Instance.ShowCardToAllUser();
                            TeenPattiManager.Instance.CheckFinalWinner("Show");
                            break;
                    }
                    case 4:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                    case 5:
                    {
                        if(isPack) return;
                        TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        break;
                    }
                }

                break;
            
            
        }
        TeenPattiManager.Instance.ChangePlayerTurn(playerNo);

        _isFunctionCalled = true;
    }*/


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
                TeenPattiManager.Instance.ChangeCardStatus("Blind", playerNo, false);
                yield return new WaitForSeconds(1.5f);
            }
            else if (num1 == 1)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);
                TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo, false);
                yield return new WaitForSeconds(1.5f);  // Add a 2-second delay
            }
        }

        // Generate the random 'num' value here before deciding on the betting logic
        int num = Random.Range(1, 6);
        Debug.Log("Randomly generated num => " + num);

        float currentPrice;
        int priceIndex;

        Debug.Log("delearObj.activeInHierarchy  =>  " + delearObj.activeInHierarchy + "  TeenPattiManager.Instance.roundCounter  =>  " + TeenPattiManager.Instance.roundCounter);

        if (delearObj.activeInHierarchy && TeenPattiManager.Instance.roundCounter == 0)
        {
            if (isSeen)
                currentPrice = TeenPattiManager.Instance.minLimitValue * 2;
            else
                currentPrice = TeenPattiManager.Instance.minLimitValue;

            priceIndex = TeenPattiManager.Instance.currentPriceIndex;
        }
        else
        {
            Debug.Log("--------------------------------------------------------------------------------");

            // Check if the player is going to pack based on num == 5 and other conditions
            /*  if (num == 5 && TeenPattiManager.Instance.winningBotNo != -1 && TeenPattiManager.Instance.winningBotNo != this.playerNo)
              {
                  // Player is going to pack, no need to call GetAdjacentPlayersPrice
                  Debug.Log("Player will pack, skipping GetAdjacentPlayersPrice...");
                  TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                  yield break; // Exit coroutine as the player is packing
              }*/

            // Call GetAdjacentPlayersPrice if packing condition is not met
            GetAdjacentPlayersPrice(playerNo, out currentPrice, out priceIndex);
        }

        print("delearObj-------- > " + currentPrice + "---" + priceIndex + "---");

        TeenPattiManager.Instance.currentPriceValue = currentPrice;
        TeenPattiManager.Instance.currentPriceIndex = priceIndex;
        Debug.Log("currentPriceValue   =>  " + TeenPattiManager.Instance.currentPriceValue);

        // Now handle the bets based on the round counter
        switch (TeenPattiManager.Instance.roundCounter)
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

        //TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
    }



    private void HandleBetForRounds(int num)
    {
        Debug.Log("SHIGHAM AGAIN   =>  " + num);
        TeenPattiManager.Instance.CheckActivePlayer();
        Debug.LogError("NUM => " + num);
        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            Debug.Log("playerBalence => " + playerBalence.text + "   currentPriceValue  =>  " + TeenPattiManager.Instance.currentPriceValue);

            if (playerBalanceValue < TeenPattiManager.Instance.currentPriceValue)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo + "  ");
                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                Debug.Log("<color=red>------------------------------------------- Not enough money ----------------------------------------------</color>");
                return;
            }
            else
            {
                bool isDoubleBet = isSeen && Random.Range(0, 5) == 4;

                float potentialBetAmount = TeenPattiManager.Instance.currentPriceValue * 2;

                float betAmount = (isDoubleBet && potentialBetAmount <= MainMenuManager.Instance.challLimit)
                    ? potentialBetAmount
                    : TeenPattiManager.Instance.currentPriceValue;

                TeenPattiManager.Instance.currentPriceValue = betAmount;
                Debug.Log("BOT CHALL     =>    " + betAmount);
                if (num != 5)
                {
                    SendBotBetNo(num, playerNo, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                    Debug.LogError("mahadeV - BOT1 =>  " + betAmount);
                    TeenPattiManager.Instance.BetAnim(this, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                    SoundManager.Instance.ThreeBetSound();
                    TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
                }
                /* else if (TeenPattiManager.Instance.winningBotNo != -1 && TeenPattiManager.Instance.winningBotNo == this.playerNo)
                 {
                     SendBotBetNo(num, playerNo, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                     Debug.LogError("mahadeV - BOT2 => " + betAmount);
                     TeenPattiManager.Instance.BetAnim(this, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                     SoundManager.Instance.ThreeBetSound();
                     TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
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
                        SendBotBetNo(num, playerNo, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                        Debug.LogError("mahadeV - BOT1 =>  " + betAmount);
                        TeenPattiManager.Instance.BetAnim(this, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                        SoundManager.Instance.ThreeBetSound();
                        TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
                    }
                }
            }
        }
    }

    private void HandleBetForOtherRounds(int num)
    {
        Debug.Log("SHIGHAM AGAIN  1   =  " + num);
        TeenPattiManager.Instance.CheckActivePlayer();

        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            if (playerBalanceValue < TeenPattiManager.Instance.currentPriceValue)
            {
                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo, false);
                Debug.Log("<color=red>------------------------------------------- Not enough money ----------------------------------------------<color>");
                return;
            }
        }

        bool isDoubleBet = isSeen && Random.Range(0, 5) == 4;

        float potentialBetAmount = TeenPattiManager.Instance.currentPriceValue * 2;

        float betAmount = (isDoubleBet && potentialBetAmount <= MainMenuManager.Instance.challLimit)
            ? potentialBetAmount
            : TeenPattiManager.Instance.currentPriceValue;

        TeenPattiManager.Instance.currentPriceValue = betAmount;
        Debug.Log("BOT CHALL     =>    " + betAmount);

        switch (num)
        {
            case 1:
            case 2:
            case 4:
            case 5:
                SendBotBetNo(num, playerNo, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                Debug.LogError("mahadeV - BOT3");

                TeenPattiManager.Instance.BetAnim(this, betAmount, TeenPattiManager.Instance.currentPriceIndex);
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
                break;

            case 3:
                BotShow();
                break;
        }
    }
    public void BotShow()
    {
        if (TeenPattiManager.Instance.activePlayerOnTable == 2)
        {
            int n = Random.Range(0, 3);
            if (n == 2)
            {
                TeenPattiManager.Instance.ShowCardToAllUser();
                TeenPattiManager.Instance.CheckAllPlayers(TeenPattiManager.Instance.winnerPlayer);
            }
            else
            {
                Debug.Log("PLAYER PACK =  " + playerNo);
                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo, false);
            }
        }
        else
        {
            Debug.Log("PLAYER PACK =  " + playerNo);
            TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo, false);
        }
    }


    TeenPattiPlayer prevPlayer;
    TeenPattiPlayer currPlayer;
    public void GetAdjacentPlayersPrice(int playerNo, out float currentPriceValue, out int currentPriceIndex)
    {
        Debug.Log("<color=red> ============= Enter GetAdjacentPlayersPrice ========  </color>");


        // Check if we have reached the last player in the list
        /* int totalPlayers = DataManager.Instance.joinPlayerDatas.Count;

         //int totalPlayers = TeenPattiManager.Instance.teenPattiPlayers.Count;
         // int previousPlayerIndex = playerNo - 1;  // Start from the player before the current player
         //int totalPlayers = DataManager.Instance.joinPlayerDatas.Count;

         int previousPlayerIndex;

         // Get the total number of players in the game.

         // Calculate the starting previous index.
         previousPlayerIndex = (playerNo - 1);

         // If playerNo is 1, set previous to the last player.
         if (previousPlayerIndex < 1)
         {
             previousPlayerIndex = totalPlayers;
         }


         // Loop backward until we find a non-packed player.
         while (TeenPattiManager.Instance.teenPattiPlayers[previousPlayerIndex - 1].gameObject.activeInHierarchy && TeenPattiManager.Instance.teenPattiPlayers[previousPlayerIndex - 1].isPack)
         {
             // Decrement index to move backwards.
             previousPlayerIndex--;

             // If index goes below 1, wrap around to the last player.
             if (previousPlayerIndex < 1)
             {
                 previousPlayerIndex = totalPlayers;
             }
         }




         *//*    // Loop until we find a valid previous player
             while (true)
             {
                 // Wrap around if the index goes below 1
                 if (previousPlayerIndex == 1)
                 {
                     previousPlayerIndex = totalPlayers;
                 }

                 // Get the player at the calculated index
                 var player = TeenPattiManager.Instance.teenPattiPlayers[previousPlayerIndex - 1];

                 // Check if the player is active and not packed
                 if (player.gameObject.activeInHierarchy && !player.isPack)
                 {
                     // Found the previous active, non-packed player
                     Debug.Log("Previous active, non-packed player index: " + previousPlayerIndex);
                     break;
                 }

                 // Decrement index to check the next player in reverse
                 previousPlayerIndex--;
             }
     */
        /*   // If `playerNo` is 1, set previous to the last player.
           if (previousPlayerIndex < 1)
           {
               previousPlayerIndex = totalPlayers;
           }

           // Loop backward until we find a non-packed player.
           while (TeenPattiManager.Instance.teenPattiPlayers[previousPlayerIndex - 1].gameObject.activeInHierarchy && TeenPattiManager.Instance.teenPattiPlayers[previousPlayerIndex - 1].isPack)
           {
               // Decrement index to move backwards.
               previousPlayerIndex--;

               // If index goes below 1, wrap around to the last player.
               if (previousPlayerIndex < 1)
               {
                   previousPlayerIndex = totalPlayers;
               }
           }*//*

        for (int i = 0; i < TeenPattiManager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("activeInHierarchy  =>  " + TeenPattiManager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy + "   playerNo   =>  " + TeenPattiManager.Instance.teenPattiPlayers[i].playerNo + "  previousPlayerIndex =>  " + previousPlayerIndex + "  isPack  => " + !TeenPattiManager.Instance.teenPattiPlayers[i].isPack);
            if (TeenPattiManager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy && TeenPattiManager.Instance.teenPattiPlayers[i].playerNo == previousPlayerIndex && !TeenPattiManager.Instance.teenPattiPlayers[i].isPack)
            {
                prevPlayer = TeenPattiManager.Instance.teenPattiPlayers[i];
                Debug.Log("PRE   " + prevPlayer.name);
            }
        }*/
        int previousPlayerIndex;
        previousPlayerIndex = (playerNo - 1);
        for (int i = 0; i < TeenPattiManager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("Checking player: " + previousPlayerIndex);

            // If this player has not packed, we can exit the loop
            if (isPrevPlayerPack(previousPlayerIndex))
            {
                Debug.Log("Valid Player Turn: " + previousPlayerIndex);
                for (int j = 0; j < TeenPattiManager.Instance.teenPattiPlayers.Count; j++)
                {
                    Debug.Log("pre =  " + previousPlayerIndex + " TeenPattiManager.Instance.teenPattiPlayers[i].no   " + TeenPattiManager.Instance.teenPattiPlayers[i].playerNo);
                    if(previousPlayerIndex== TeenPattiManager.Instance.teenPattiPlayers[j].playerNo)
                    {
                        prevPlayer = TeenPattiManager.Instance.teenPattiPlayers[j];
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
        Debug.Log("TeenPattiManager.Instance.currentPriceValue   => " + TeenPattiManager.Instance.currentPriceValue);
        if ((TeenPattiManager.Instance.currentPriceValue) > MainMenuManager.Instance.challLimit || (TeenPattiManager.Instance.currentPriceValue * 2) > MainMenuManager.Instance.challLimit)
        {
            Debug.LogError("TeenPattiManager.Instance.currentPriceValue /////=>  " + TeenPattiManager.Instance.currentPriceValue);
            TeenPattiManager.Instance.doubleBUtton.SetActive(false);
            Debug.Log("crossChalLimitLastChallSave  =>  " + TeenPattiManager.Instance.crossChalLimitLastChallSave);
            if (TeenPattiManager.Instance.crossChalLimitLastChallSave == -1f)
            {
                // This block will execute only the first time
                currentPriceValue = TeenPattiManager.Instance.currentPriceValue;  // Set current price value
                Debug.LogError("TeenPattiManager.Instance.currentPriceValue /////=>  " + currentPriceValue);
                currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;  // Set current price index

                // Store the initial price value
                TeenPattiManager.Instance.crossChalLimitLastChallSave = TeenPattiManager.Instance.currentPriceValue;
                Debug.LogError("TeenPattiManager.Instance.currentPriceValue /////=>  " + TeenPattiManager.Instance.crossChalLimitLastChallSave);
            }
            else
            {
                // Use the stored price value on subsequent entries
                Debug.LogError("TeenPattiManager.Instance.currentPriceValue /////=>  " + TeenPattiManager.Instance.crossChalLimitLastChallSave);
                currentPriceValue = TeenPattiManager.Instance.crossChalLimitLastChallSave;
                Debug.LogError("TeenPattiManager.Instance.currentPriceValue /////=>  " + currentPriceValue);
                currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;  // Set current price index

            }
        }
        else
        {
            if (prevPlayer.isBlind && prevPlayer.blindIMG.activeInHierarchy && currPlayer.isBlind)
            {
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                Debug.Log("Both me and previous player are blind." + TeenPattiManager.Instance.currentPriceValue);

                currentPriceValue = TeenPattiManager.Instance.currentPriceValue;

                currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;
            }
            else if (prevPlayer.isSeen && prevPlayer.seenImg.activeInHierarchy && currPlayer.isSeen)
            {
                // Do not change the value if both players are already seen
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                Debug.Log("Both me and previous player are seen (no change in amount)." + TeenPattiManager.Instance.currentPriceValue);
                currentPriceValue = TeenPattiManager.Instance.currentPriceValue;
                currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;
            }
            else if (prevPlayer.isBlind && prevPlayer.blindIMG.activeInHierarchy && currPlayer.isSeen)
            {
                // First transition from blind to seen — double the value.
                Debug.Log("I am seen, previous player is blind." + TeenPattiManager.Instance.currentPriceValue);
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                currentPriceValue = TeenPattiManager.Instance.currentPriceValue * 2;
                currentPriceIndex = (TeenPattiManager.Instance.currentPriceIndex + 1) % TeenPattiManager.Instance.numbers.Length;
            }
            else if (currPlayer.isBlind && prevPlayer.isSeen && prevPlayer.seenImg.activeInHierarchy)
            {
                // If the current player is blind and the previous is seen, halve the value.
                Debug.Log("I am blind, previous player is seen." + TeenPattiManager.Instance.currentPriceValue);
                Debug.Log("Prev Name =>  " + prevPlayer.name + "   Curr Name  =>  " + currPlayer.name);
                if (TeenPattiManager.Instance.minLimitValue > TeenPattiManager.Instance.currentPriceValue / 2)
                {
                    currentPriceValue = TeenPattiManager.Instance.minLimitValue;
                }
                else
                {
                    currentPriceValue = TeenPattiManager.Instance.currentPriceValue / 2;

                }
                currentPriceIndex = (TeenPattiManager.Instance.currentPriceIndex - 1 + TeenPattiManager.Instance.numbers.Length) % TeenPattiManager.Instance.numbers.Length;
            }
            else
            {
                // Default case — retain the current value and index.
                Debug.Log("Default case — retain the current value and index.");
                currentPriceValue = TeenPattiManager.Instance.currentPriceValue;
                currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;
            }
            Debug.LogError("currentPriceValue =   " + currentPriceValue);
        }
        // Check player states and determine the appropriate price value and index.




    }

    bool isPrevPlayerPack(int prevPlayerNo)
    {
        for (int i = 0; i < TeenPattiManager.Instance.teenPattiPlayers.Count; i++)
        {
            Debug.Log("I =>  " + i + "  playerSquList[i].gameObject.activeInHierarchy  => " + TeenPattiManager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy + "  playerSquList[i].playerNo = " + TeenPattiManager.Instance.teenPattiPlayers[i].playerNo + "   nextPlayerNo = > " + prevPlayerNo + "   playerSquList[i].isPack  =  " + TeenPattiManager.Instance.teenPattiPlayers[i].isPack);
            if (TeenPattiManager.Instance.teenPattiPlayers[i].gameObject.activeInHierarchy && TeenPattiManager.Instance.teenPattiPlayers[i].playerNo == prevPlayerNo && TeenPattiManager.Instance.teenPattiPlayers[i].isPack == false)
            {
                Debug.Log("RETURN TRUE ");
                return true;
            }
        }
        Debug.Log("RETURN FALSE ");
        return false;
    }

    private TeenPattiPlayer GetNonPackPlayer(int playerIndex, int totalPlayers, int step)
    {
        // Store the initial index to avoid infinite loops
        int startIndex = playerIndex;

        // Iterate until we find a non-packed player or return to the starting index
        while (TeenPattiManager.Instance.teenPattiPlayers[playerIndex].isPack)
        {
            playerIndex = (playerIndex + step + totalPlayers) % totalPlayers;

            // If we have circled back to the start, break to avoid infinite loop
            if (playerIndex == startIndex)
            {
                return null; // All players are packed; handle this case as needed
            }
        }
        return TeenPattiManager.Instance.teenPattiPlayers[playerIndex];
    }

    public void SumOfPlayerCards()
    {
        sumOfCards = card1.cardNo + card2.cardNo + card3.cardNo;
    }



    public void CardGenerate()
    {
        cardImg1.sprite = TeenPattiManager.Instance.simpleCardSprite;
        cardImg2.sprite = TeenPattiManager.Instance.simpleCardSprite;
        cardImg3.sprite = TeenPattiManager.Instance.simpleCardSprite;
        int startIndex = (playerNo - 1) * 3;
        if (startIndex >= 0)
        {
            card1 = new CardSuffle();
            card2 = new CardSuffle();
            card3 = new CardSuffle();
            print("Start Index : " + startIndex);
            card1 = TeenPattiManager.Instance.cardSuffles[TeenPattiManager.Instance.mainList[startIndex] - 1];
            card2 = TeenPattiManager.Instance.cardSuffles[TeenPattiManager.Instance.mainList[startIndex + 1] - 1];
            card3 = TeenPattiManager.Instance.cardSuffles[TeenPattiManager.Instance.mainList[startIndex + 2] - 1];

            /*   print("This is card1 no  -> " + (TeenPattiManager.Instance.mainList[startIndex] - 1));
               print("This is card2 no  -> " + (TeenPattiManager.Instance.mainList[startIndex + 1] - 1));
               print("This is card3 no  -> " + (TeenPattiManager.Instance.mainList[startIndex + 2] - 1));*/

            TeenPattiWinMaintain winMaintain = TeenPattiManager.Instance.MatchResult(card1, card2, card3);
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

        Debug.Log("card1.cardSprite => " + card1.cardSprite);
        Debug.Log("card2.cardSprite => " + card2.cardSprite);
        Debug.Log("card3.cardSprite => " + card3.cardSprite);
        cardImg1.sprite = card1.cardSprite;
        cardImg2.sprite = card2.cardSprite;
        cardImg3.sprite = card3.cardSprite;
    }


    public void CardPackDisplay()
    {
        card1.cardSprite = TeenPattiManager.Instance.packCardSprite;
        card2.cardSprite = TeenPattiManager.Instance.packCardSprite;
        card3.cardSprite = TeenPattiManager.Instance.packCardSprite;
    }

    public void SendBotBetNo(int no, int botPlayerNo, float prize, int index)
    {
        Debug.Log("SendBotBetNo  Amount    =>  " + prize);


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
        Debug.LogError("mahadeV -bOT");

    }




    public void RestartFillLine()
    {
        print("chance given to :" + this.playerNo + " Teenpattiplayer = " + this.gameObject.name + " isbot = " + isBot);
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
