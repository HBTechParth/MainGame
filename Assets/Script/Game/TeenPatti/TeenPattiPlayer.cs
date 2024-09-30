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
                    TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
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
            print("---------------------------Bot is called------------------" + playerNo);
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

        int num1;

        if (!isSeen)
        {
            num1 = Random.Range(0, 2);
            Debug.Log("NUM  => " + num1);
            if (num1 == 0)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                TeenPattiManager.Instance.ChangeCardStatus("Blind", playerNo);
                yield return new WaitForSeconds(1.5f);
            }
            else if (num1 == 1)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                TeenPattiManager.Instance.ChangeCardStatus("SEEN", playerNo);
                yield return new WaitForSeconds(1.5f);  // Add a 2-second delay
            }
        }
        float currentPrice;
        int priceIndex;
        if (delearObj.activeInHierarchy && TeenPattiManager.Instance.roundCounter == 0)
        {
            currentPrice = TeenPattiManager.Instance.minLimitValue;
            priceIndex = TeenPattiManager.Instance.currentPriceIndex;
        }
        else
            GetAdjacentPlayersPrice(playerNo, out currentPrice, out priceIndex);
        print("-------- > " + currentPrice + "---" + priceIndex + "---");

        TeenPattiManager.Instance.currentPriceValue = currentPrice;
        TeenPattiManager.Instance.currentPriceIndex = priceIndex;

        int num = Random.Range(1, 6);
        //SendBotBetNo(num, playerNo, currentPrice, priceIndex);

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
        Debug.LogError("NUM => " + num);
        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            if (playerBalanceValue <= TeenPattiManager.Instance.currentPriceValue)
            {
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                Debug.Log("<color=red>------------------------------------------- Not money----------------------------------------------</color>");
                /* SoundManager.Instance.ThreeBetSound();
                 TeenPattiManager.Instance.ChangePlayerTurn(playerNo);*/
                return;
            }

        }
        if (num != 5)
        {
            SendBotBetNo(num, playerNo, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
            Debug.LogError("mahadeV -bOT1");

            TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
            SoundManager.Instance.ThreeBetSound();
            TeenPattiManager.Instance.ChangePlayerTurn(playerNo);

        }
        else if (TeenPattiManager.Instance.winningBotNo != -1 && TeenPattiManager.Instance.winningBotNo == this.playerNo)
        {
            SendBotBetNo(num, playerNo, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
            Debug.LogError("mahadeV -bOT2");

            TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);

            SoundManager.Instance.ThreeBetSound();
            Debug.Log("ChangeCardStatus   =>  " + playerNo);

            TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
        }
        else
        {
            Debug.Log("ChangeCardStatus   =>  " + playerNo);

            TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
        }
    }

    private void HandleBetForOtherRounds(int num)
    {
        float playerBalanceValue;
        if (float.TryParse(playerBalence.text, out playerBalanceValue))
        {
            if (playerBalanceValue <= TeenPattiManager.Instance.currentPriceValue)
            {
                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                Debug.Log("<color=red>-------------------------------------------Not money----------------------------------------------<color>");
                return;
                /* SoundManager.Instance.ThreeBetSound();
                 TeenPattiManager.Instance.ChangePlayerTurn(playerNo);*/
            }

        }

        switch (num)
        {
            case 1:
            case 2:
            case 4:
            case 5:
                SendBotBetNo(num, playerNo, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                Debug.LogError("mahadeV -bOT3");

                TeenPattiManager.Instance.BetAnim(this, TeenPattiManager.Instance.currentPriceValue, TeenPattiManager.Instance.currentPriceIndex);
                SoundManager.Instance.ThreeBetSound();
                Debug.Log("ChangeCardStatus   =>  " + playerNo);

                TeenPattiManager.Instance.ChangePlayerTurn(playerNo);
                break;

            case 3:
                /*TeenPattiManager.Instance.ShowCardToAllUser();
                TeenPattiManager.Instance.CheckFinalWinner("Show");*/
                TeenPattiManager.Instance.ChangeCardStatus("PACK", playerNo);
                break;
        }

    }

    public void GetAdjacentPlayersPrice(int playerNo, out float currentPriceValue, out int currentPriceIndex)
    {
        int totalPlayers = TeenPattiManager.Instance.teenPattiPlayers.Count;

        int previousPlayerIndex = (playerNo - 2 + totalPlayers) % totalPlayers;
        var prevPlayer = GetNonPackPlayer(previousPlayerIndex, totalPlayers, -1);

        var currPlayer = /*TeenPattiManager.Instance.teenPattiPlayers[playerNo - 1]*/this;
        print("current bot player = " + currPlayer + " playerNo = " + playerNo + " global playerNo = " + this.playerNo);
        if (prevPlayer.isBlind && currPlayer.isBlind)
        {
            currentPriceValue = TeenPattiManager.Instance.numbers[TeenPattiManager.Instance.currentPriceIndex];
            currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;
        }
        else if (prevPlayer.isSeen && currPlayer.isSeen)
        {
            currentPriceValue = TeenPattiManager.Instance.numbers[TeenPattiManager.Instance.currentPriceIndex];
            currentPriceIndex = TeenPattiManager.Instance.currentPriceIndex;
        }
        else if (prevPlayer.isBlind && currPlayer.isSeen)
        {
            currentPriceValue = TeenPattiManager.Instance.numbers[(TeenPattiManager.Instance.currentPriceIndex + 1) % TeenPattiManager.Instance.numbers.Length];
            currentPriceIndex = (TeenPattiManager.Instance.currentPriceIndex + 1) % TeenPattiManager.Instance.numbers.Length;
        }
        else if (currPlayer.isBlind && prevPlayer.isSeen)
        {
            currentPriceValue = TeenPattiManager.Instance.numbers[(TeenPattiManager.Instance.currentPriceIndex - 1 + TeenPattiManager.Instance.numbers.Length) % TeenPattiManager.Instance.numbers.Length];
            currentPriceIndex = (TeenPattiManager.Instance.currentPriceIndex - 1 + TeenPattiManager.Instance.numbers.Length) % TeenPattiManager.Instance.numbers.Length;
        }
        else
        {
            currentPriceValue = TeenPattiManager.Instance.numbers[(TeenPattiManager.Instance.currentPriceIndex - 1 + TeenPattiManager.Instance.numbers.Length) % TeenPattiManager.Instance.numbers.Length];
            currentPriceIndex = (TeenPattiManager.Instance.currentPriceIndex - 1 + TeenPattiManager.Instance.numbers.Length) % TeenPattiManager.Instance.numbers.Length;
        }
    }

    private TeenPattiPlayer GetNonPackPlayer(int playerIndex, int totalPlayers, int step)
    {
        while (TeenPattiManager.Instance.teenPattiPlayers[playerIndex].isPack)
        {
            playerIndex = (playerIndex + step + totalPlayers) % totalPlayers;
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
