using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealRummyPlayerManager : MonoBehaviour
{
    public Image avatarImg;
    public Text playerNameTxt;
    public Image cardImg1;
    public Image cardImg2;
    public Image cardImg3;
    public GameObject delearObj;

    public Text gameScoreText;
    public float playerGamePoints;

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
    public bool isMyTimerComplete = false;
    public bool isGameComplete = false;

    public string playerId;
    public string lobbyId;

    public DealRummyManager.CardSuffle card1;
    public DealRummyManager.CardSuffle card2;
    public DealRummyManager.CardSuffle card3;

    public int ruleNo;
    public string avatar;

    public bool isBot;
    public bool isCalled;
    private bool _isFunctionCalled;
    public int userTurnCount;
    public GameObject[] boxArray;
    public int inactiveCount = 0;

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
    private void Update()
    {
        if (isGameComplete && DealRummyManager.Instance.isTimerComplete == false)
        {
            fillLine.fillAmount -= 1.0f / DealRummyManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isPack == false)
            {
                DealRummyManager.Instance.SubmitButtonClick();
            }
        }
    }
    private void FixedUpdate()
    {
        //if (DealRummyManager.Instance.player1 == this)
        //{
        //    DealRummyManager.Instance. = 

        if (DealRummyManager.Instance.isGameComplete)
            return;
        //}
        if (!DealRummyManager.Instance.isBotActivate) return;

        if (playerWinObj[0].activeSelf == true && DealRummyManager.Instance.isWin == false)
        {
            DealRummyManager.Instance.isWin = true;
        }
        else if (playerWinObj[0].activeSelf == false)
        {
            DealRummyManager.Instance.isWin = false;
        }

        //if(isPack) return;
        if (isTurn && DealRummyManager.Instance.isWin == false)
        {
            fillLine.fillAmount -= 1.0f / DealRummyManager.Instance.timerSpeed * Time.deltaTime;
            if (fillLine.fillAmount == 0 && isOneTimeEnter == false)
            {
                isOneTimeEnter = true;
                isTurn = false;
                userTurnCount++;
                if (userTurnCount >= 4)
                {
                    DealRummyManager.Instance.ChangeCardStatus("PACK", playerNo);
                }
                CheckLife();
                if (DealRummyManager.Instance.isMyTurn)
                {
                    if (DealRummyManager.Instance.player.cards.Count == 14)
                    {
                        if (DealRummyManager.Instance.cardsToGroup.Count == 1)
                        {
                            DealRummyManager.Instance.DiscardButton();
                        }
                        else if (DealRummyManager.Instance.cardsToGroup.Count > 1)
                        {
                            DealRummyManager.Instance.cardsToGroup.Clear();
                            int rng = Random.Range(0, DealRummyManager.Instance.player.cards.Count);
                            DealRummyManager.Instance.cardsToGroup.Add(DealRummyManager.Instance.player.cards[rng].gameObject);
                            DealRummyManager.Instance.DiscardButton();
                        }
                        else if (DealRummyManager.Instance.cardsToGroup.Count == 0)
                        {
                            int rng = Random.Range(0, DealRummyManager.Instance.player.cards.Count);
                            DealRummyManager.Instance.cardsToGroup.Add(DealRummyManager.Instance.player.cards[rng].gameObject);
                            DealRummyManager.Instance.DiscardButton();
                        }
                    }
                    //DealRummyManager.Instance.dropButtonImage.sprite = DealRummyManager.Instance.disabledDiscardButton;
                }
                DealRummyManager.Instance.ChangePlayerTurn(playerNo);

            }

            if (isCalled) return;
            if (!isTurn || !isBot) return;
            if (!DealRummyManager.Instance.isAdmin) return;
            //StartCoroutine(CallBotFunction());
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
        BotAutoBet();
        //DealRummyManager.Instance.BetAnim(this, 0.1f);
    }

    private void BotAutoBet()
    {
        int num = Random.Range(1, 6);

        SendBotBetNo(num, playerNo);

        switch (DealRummyManager.Instance.roundCounter)
        {
            //after 1st round
            case <= 1:
                switch (num)
                {
                    case 1:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 2:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 3:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 4:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 5:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
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
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 2:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 3:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 4:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 5:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.ChangeCardStatus("PACK", playerNo);
                            //DealRummyManager.Instance.BetAnim(this, 5f);
                            break;
                        }
                }

                break;

            case 3:
                switch (num)
                {
                    case 1:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.ChangeCardStatus("PACK", playerNo);
                            break;
                        }
                    case 2:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 3:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 4:
                        {
                            if (isPack) return;
                            if (!isSeen)
                            {
                                DealRummyManager.Instance.ChangeCardStatus("SEEN", playerNo);
                            }
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 5:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.ChangeCardStatus("PACK", playerNo);
                            //DealRummyManager.Instance.BetAnim(this, 5f);
                            break;
                        }
                }

                break;
            case >= 4:
                switch (num)
                {
                    case 1:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 2:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 3:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.ChangeCardStatus("PACK", playerNo);
                            break;
                        }
                    case 4:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                    case 5:
                        {
                            if (isPack) return;
                            DealRummyManager.Instance.BetAnim(this, DealRummyManager.Instance.currentPriceValue, DealRummyManager.Instance.currentPriceIndex);
                            SoundManager.Instance.ThreeBetSound();
                            break;
                        }
                }

                break;
        }
        DealRummyManager.Instance.ChangePlayerTurn(playerNo);

        _isFunctionCalled = true;
    }

    public void SumOfPlayerCards()
    {
        sumOfCards = card1.cardNo + card2.cardNo + card3.cardNo;
    }



    public void CardGenerate()
    {
        cardImg1.sprite = DealRummyManager.Instance.simpleCardSprite;
        cardImg2.sprite = DealRummyManager.Instance.simpleCardSprite;
        cardImg3.sprite = DealRummyManager.Instance.simpleCardSprite;
        int startIndex = (playerNo - 1) * 3;
        if (startIndex >= 0)
        {
            card1 = new DealRummyManager.CardSuffle();
            card2 = new DealRummyManager.CardSuffle();
            card3 = new DealRummyManager.CardSuffle();
            print("Start Index : " + startIndex);
            card1 = DealRummyManager.Instance.cardSuffles[DealRummyManager.Instance.mainList[startIndex] - 1];
            card2 = DealRummyManager.Instance.cardSuffles[DealRummyManager.Instance.mainList[startIndex + 1] - 1];
            card3 = DealRummyManager.Instance.cardSuffles[DealRummyManager.Instance.mainList[startIndex + 2] - 1];

            print("This is card1 no  -> " + (DealRummyManager.Instance.mainList[startIndex] - 1));
            print("This is card2 no  -> " + (DealRummyManager.Instance.mainList[startIndex + 1] - 1));
            print("This is card3 no  -> " + (DealRummyManager.Instance.mainList[startIndex + 2] - 1));

            //TeenPattiWinMaintain winMaintain = DealRummyManager.Instance.MatchResult(card1, card2, card3);
            //ruleNo = winMaintain.ruleNo;
            //if (winMaintain.ruleNo == 1 || winMaintain.ruleNo == 5)
            //{
            //    card1 = winMaintain.winList[0];
            //    card2 = winMaintain.winList[1];
            //    card3 = winMaintain.winList[2];
            //}
            //if (winMaintain.ruleNo == 2 || winMaintain.ruleNo == 3 || winMaintain.ruleNo == 4 || winMaintain.ruleNo == 6)
            //{
            //    card1 = winMaintain.winList[0];
            //    card2 = winMaintain.winList[1];
            //    card3 = winMaintain.winList[2];
            //}
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
        card1.cardSprite = DealRummyManager.Instance.packCardSprite;
        card2.cardSprite = DealRummyManager.Instance.packCardSprite;
        card3.cardSprite = DealRummyManager.Instance.packCardSprite;
    }

    public void SendBotBetNo(int no, int botPlayerNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("BotPlayerNo", botPlayerNo);
        obj.AddField("BotNo", no);
        obj.AddField("Action", "BotBetData");
        TestSocketIO.Instace.Senddata("TeenPattiBotBetNo", obj);
    }




    public void RestartFillLine()
    {
        //DealRummyManager.Instance.ShowTextChange();
        fillLine.fillAmount = 1;
        isOneTimeEnter = false;
        isTurn = true;
        isCalled = false;
        _isFunctionCalled = false;
        //if (this == DealRummyManager.Instance.player1)
        //{
        //    DealRummyManager.Instance.bottomBox.SetActive(true);
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
