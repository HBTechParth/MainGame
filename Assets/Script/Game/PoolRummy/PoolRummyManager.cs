using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class PoolRummyManager : MonoBehaviour
{
    public enum CardColorType
    {
        Clubs = 1,
        Diamonds = 2,
        Spades = 3,
        Hearts = 4,
        JOKER = 5

    }
    [System.Serializable]
    public class CardSuffle
    {
        public int cardNo;
        public CardColorType color;//1234-1-fulli,2-red cerkat,3-black,4-red heart, 5 = JOKER
        public Sprite cardSprite;
        public bool isWildJoker;
    }
    [System.Serializable]
    public class ListStoreData
    {
        public List<int> noList = new List<int>();
    }

    public class RummyWinMaintain
    {
        public int ruleNo;
        public List<CardSuffle> winList = new List<CardSuffle>();
    }

    public static PoolRummyManager Instance;

    public GameObject waitNextRoundScreenObj;
    public GameObject playerFindScreenObj;

    [Header("---Game Play---")]
    public int gameDealerNo;
    public bool isWin = false;
    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public List<CardSuffle> cardShuffles = new List<CardSuffle>();
    public List<CardSuffle> closedDeck = new List<CardSuffle>();

    public List<ListStoreData> listStoreDatas = new List<ListStoreData>();
    public List<int> mainList = new List<int>();
    public List<CardSuffle> cardSufflesGen = new List<CardSuffle>();
    public List<CardSuffle> cardSufflesSort = new List<CardSuffle>();

    public List<CardSuffle> newCardSS = new List<CardSuffle>();
    public List<CardSuffle> newCardSS1 = new List<CardSuffle>();


    public List<PoolRummyPlayerManager> teenPattiPlayers = new List<PoolRummyPlayerManager>();
    public List<PoolRummyPlayerManager> playerSquList = new List<PoolRummyPlayerManager>();
    public PoolRummyPlayer player;

    public PoolRummyPlayerManager player1;
    public PoolRummyPlayerManager player2;
    public PoolRummyPlayerManager player3;
    public PoolRummyPlayerManager player4;
    public PoolRummyPlayerManager player5;
    public PoolRummyPlayerManager player6;

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
    public GameObject waitforTurnObject;
    public GameObject discardCardObject;
    public GameObject drawCardObject;
    public GameObject errorScreenObj;
    public GameObject slideShowPanel;
    public Text totalPriceTxt;
    public Button showButton;
    public GameObject bottomBox;
    public GameObject rulesTab;
    public Text rulesText;
    public Text betAmountTxt;
    public Text priceBtnTxt;
    public Button plusBtn;
    public Button minusBtn;
    public GameObject blindx2button;
    public bool isAdmin;
    public int playerNo;
    public int currentPlayer;
    public float timerSpeed;
    public bool isGameStop;
    private int[] numbers = { 5, 10, 50, 100, 250, 500, 1000, 5000 };
    public int currentPriceIndex = 0;
    public int runningPriceIndex;
    public Image sideShowPopupImage;
    public float delay;
    private bool isPopupOpen = false;
    public GameObject exitPanel;
    public GameObject entryPopup;
    public Text winAnimationTxt;
    public GameObject waitForNewRound;


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
    public float maxLimitValue;
    public float incrementNo;
    public float bonusUseValue;

    public float currentPriceValue;
    public float winAmount;

    public bool firstCardDrawn = false;

    public TeenPattiPlayer slideShowPlayer;
    public List<TeenPattiWinMaintain> winMaintain = new List<TeenPattiWinMaintain>();
    public bool isGameStarted;

    public bool isBotActivate;
    public int roundCounter;
    public int boxDisplayCount;
    public int totalScore;
    

    public Text totalScoreText;


    public GameObject cardPrefab;

    [Header("Buttons")]
    public Image discardButtonImage;
    public Button discardButton;
    public Button drawDiscardedCard;
    public Sprite enabledDiscardButton;
    public Sprite disabledDiscardButton;
    public Image groupButtonImage;
    public Sprite enabledGroupButton;
    public Sprite enableFinishButton;
    public Sprite enableDropButton;
    public Image finishButtonImage;
    public Image dropButtonImage;


    [Header("SelectedCardsList")]
    public List<GameObject> cardsToGroup;//cards selected by the player to group stored in this list
    public Transform cardsHolder;//transform which is holding all player cards
    public int playerCardsCount = 0;//number of cards player is currently holding
    [Header("ValidityCheck")]
    public Transform validCheckerTransform;
    public GameObject validCheckPrefab;
    public Sprite correct;
    public Sprite wrong;
    public Transform newGroup;//prefab to make a new group for sequence of cards
    public bool isValidDeclaration = true;

    public Transform deck;
    public Image wildJokerIndicator;
    public Transform discardPile;
    public Transform takeDiscardedCardButton;
    public Image discardedCard;
    public List<PoolCardScript> discardedCardDeck;
    private int wildJokerIndex;//index of the wild joker 
    private int openCardIndex;//First index for open Deck/discardPile after distributing cards
    public PoolCardScript cardFromDiscardPile;// this object will be assigned when a card is drawn from discard pile and to ensure that the same card is not discarded again

    [Header("Socket")]
    public List<int> discardCardList = new List<int>();//this will contain the index number of cards to be removed from the deck after distributing cards to player No 1
    public List<int> distributedCardsList = new List<int>();
    public Transform[] resultTransform;
    public Transform[] resultCardsHolder;
    public Text[] resultNamesText;
    public Text[] resultPointsText;
    public Text[] resultStatusText;
    public Text[] resultAmountText;//score in this variant
    public Text winText;

    public float[] playerPoints = new float[6];
    

    [Header("Finishing Game")]
    public Button finishButton;
    public Button submitButton;
    public GameObject resultDialog;
    public GameObject finishDialog;
    public GameObject arrangeCardsDialog;
    public Text finishText;
    public bool isGameComplete = false;
    public bool isTimerComplete = false;
    public List<int> finalCards = new List<int>();//this list is for final submission








    [Header("Turn Maintain")]
    public bool isMyTurn = false;


    public Transform finishSlot;

    public bool pureSequenceExists;
    
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
            Instance = this;
        Input.multiTouchEnabled = false;
        Time.timeScale = 1;
    }
    void Start()
    {
        Invoke(nameof(DebitEntryFees), 0.5f);
        isValidDeclaration = true;
        Time.timeScale = 1;
        SoundManager.Instance.StopBackgroundMusic();
        roundCounter = 0;
        boxDisplayCount = 0;
        winAmount = DataManager.Instance.betPrice * DataManager.Instance.joinPlayerDatas.Count;

        //closedDeck = cardShuffles;
        
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            teenPattiPlayers[i].gameObject.SetActive(false);
        }
        playerFindScreenObj.SetActive(true);
        CreateAdmin();
        //StartGamePlay();
        DisplayCurrentBalance();
        PlayerFound();
        CheckSound();
    }

    private void DebitEntryFees()
    {
        DataManager.Instance.DebitAmount(DataManager.Instance.betPrice.ToString(), DataManager.Instance.gameId, "PoolRummy_Entry-" + DataManager.Instance.gameId, "game", 11);
    }
    public void PlayerCards(int count)
    {
        List<int> historyList = new List<int>();
        int rng;
        for (int i = 0; i < count * 13; i++)
        {
            do
            {
                rng = UnityEngine.Random.Range(0, cardShuffles.Count);
            } while (historyList.Contains(rng));
            {
                historyList.Add(rng);
            }
            distributedCardsList.Add(rng);
        }
    }

    public void InitializeCards()
    {

        if (!isAdmin)
            return;

        PlayerCards(DataManager.Instance.joinPlayerDatas.Count);


        player.CardDistribute();

    }

    public void CardClick(GameObject obj)
    {
        print("cardbutton clicked");
        if (obj.transform.localPosition.y == 0f)
        {
            cardsToGroup.Add(obj);
            obj.transform.DOLocalMoveY(60f, 0.2f);
        }
        else if (obj.transform.localPosition.y == 60f)
        {
            cardsToGroup.Remove(obj);
            obj.transform.DOLocalMoveY(0f, 0.2f);
        }
        if (cardsToGroup.Count > 0)
        {
            groupButtonImage.sprite = enabledGroupButton;
        }
        else if (cardsToGroup.Count == 0)
            groupButtonImage.sprite = disabledDiscardButton;
        if (isMyTurn)
        {
            if (player.cards.Count == 13)
                dropButtonImage.sprite = enableDropButton;
            else
                dropButtonImage.sprite = disabledDiscardButton;
            if (cardsToGroup.Count == 1 && player.cards.Count == 14)
            {
                discardButtonImage.sprite = enabledDiscardButton;
                finishButtonImage.sprite = enableFinishButton;
            }
            else
            {
                finishButtonImage.sprite = disabledDiscardButton;
                discardButtonImage.sprite = disabledDiscardButton;
            }
        }



    }
    public void DrawCardFromDeck()
    {
        if (player.cards.Count == 13 && isMyTurn)
        {
            if (closedDeck.Count > 0)
            {
                firstCardDrawn = true;
                int rng = UnityEngine.Random.Range(0, closedDeck.Count);
                GameObject newCard = Instantiate(cardPrefab, cardsHolder.GetChild(cardsHolder.childCount - 1));
                var card = newCard.GetComponent<PoolCardScript>();
                card.card = closedDeck[rng];
                card.card.cardNo = closedDeck[rng].cardNo;
                card.card.color = closedDeck[rng].color;
                card.card.cardSprite = closedDeck[rng].cardSprite;
                card.card.isWildJoker = closedDeck[rng].isWildJoker;
                player.cards.Add(card);
                player.cardImages.Add(card.GetComponent<Image>());
                newCard.GetComponent<Image>().sprite = card.card.cardSprite;
                if (card.card.isWildJoker)
                    card.wildJoker.SetActive(true);
                newCard.transform.position = deck.transform.position;

                closedDeck.RemoveAt(rng);
                newCard.transform.DOLocalMove(cardsHolder.GetChild(cardsHolder.childCount - 1).GetChild(0).localPosition, 0.05f).OnComplete(() =>
                {
                    newCard.transform.DOBlendableLocalMoveBy(new Vector3(-60f, 0, 0), 0.05f);
                });
                cardsHolder.GetChild(cardsHolder.childCount - 1).transform.DOBlendableLocalMoveBy(new Vector3(60f, 0, 0), 0.1f);
                validCheckerTransform.GetChild(cardsHolder.childCount - 1).transform.DOBlendableLocalMoveBy(new Vector3(60f, 0, 0), 0.1f);
                newCard.transform.SetAsFirstSibling();

                CardsChecker();
                if (closedDeck.Count == 1)
                {
                    for (int i = discardedCardDeck.Count - 1; i > 0; i--)
                    {
                        closedDeck.Add(discardedCardDeck[i].card);
                        discardedCardDeck.RemoveAt(i);
                    }

                }
                DrawCardFromClosedDeckSocket(player1.playerNo, rng);
            }
        }
        else if (isMyTurn && player.cards.Count == 14)
            discardCardObject.SetActive(true);
        else if (!isMyTurn)
            waitforTurnObject.SetActive(true);


    }

    public void DrawCardFromDiscardPile()
    {
        if (player.cards.Count == 13 && isMyTurn)
        {
            firstCardDrawn = true;
            player.cards.Add(discardedCardDeck[0]);
            player.cardImages.Add(discardedCardDeck[0].GetComponent<Image>());
            cardFromDiscardPile = discardedCardDeck[0];
            discardedCardDeck[0].transform.SetParent(cardsHolder.GetChild(cardsHolder.childCount - 1));
            discardedCardDeck[0].transform.DOScale(Vector3.one, 0.1f);
            discardedCardDeck[0].transform.DOLocalMove(cardsHolder.GetChild(cardsHolder.childCount - 1).GetChild(0).localPosition, 0.05f).OnComplete(() =>
            {
                discardedCardDeck[0].transform.DOBlendableLocalMoveBy(new Vector3(-60f, 0, 0), 0.05f);
                discardedCardDeck.RemoveAt(0);
            });
            cardsHolder.GetChild(cardsHolder.childCount - 1).transform.DOBlendableLocalMoveBy(new Vector3(60f, 0, 0), 0.1f);
            discardedCardDeck[0].transform.SetAsFirstSibling();
            discardedCardDeck[0].button.interactable = true;
            CardsChecker();
            DrawCardFromDiscardPileSocket(player1.playerNo);
        }
        else if (!isMyTurn && player.cards.Count == 14)
            discardCardObject.SetActive(true);
        else if (!isMyTurn)
            waitforTurnObject.SetActive(true);

    }

    #region Bottom List Buttons
    public void DiscardButton()
    {
        if (cardsToGroup.Count == 1 && isMyTurn && player.cards.Count == 14)
        {
            var _removedCard = cardsToGroup[0].GetComponent<PoolCardScript>();
            if (_removedCard != cardFromDiscardPile)
            {
                cardFromDiscardPile = null;
                player.cards.Remove(_removedCard);
                player.cardImages.Remove(_removedCard.GetComponent<Image>());
                Transform temp = _removedCard.transform.parent;
                Transform sequenceCheck_Of_Removed_Card = validCheckerTransform.transform.GetChild(temp.transform.GetSiblingIndex());
                discardedCardDeck.Insert(0, _removedCard);
                cardsToGroup.Clear();
                discardedCardDeck[0].transform.SetParent(discardPile);
                discardedCardDeck[0].button.interactable = false;
                discardedCardDeck[0].transform.SetAsLastSibling();
                discardedCardDeck[0].transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f);
                discardedCardDeck[0].transform.DOLocalMove(Vector3.zero, 0.1f).OnComplete(() =>
                {
                    if (temp.childCount > 0)
                    {
                        for (int i = 0; i < temp.childCount; i++)
                            cardsToGroup.Add(temp.GetChild(i).gameObject);
                        //GroupButtonClick();
                        for (int i = 0; i < temp.childCount; i++)//sorting cards in a way that player can properly see and does not overlap over any other card
                        {
                            temp.GetChild(i).transform.localPosition = new Vector3(-i * 60f, 0f, 0f);
                            temp.GetChild(i).transform.SetAsFirstSibling();
                        }
                    }
                    else if (temp.childCount == 0)
                    {
                        Destroy(temp.gameObject);
                        Destroy(sequenceCheck_Of_Removed_Card.gameObject);
                    }
                    cardsToGroup.Clear();
                    //CardsChecker();
                    if (player1.isOneTimeEnter == false)
                        ChangePlayerTurn(player1.playerNo);
                    isMyTurn = false;
                    int cardIndex = -1;
                    for (int i = 0; i < cardShuffles.Count; i++)
                    {
                        if (_removedCard.card.cardNo == cardShuffles[i].cardNo && _removedCard.card.color == cardShuffles[i].color && _removedCard.card.cardSprite == cardShuffles[i].cardSprite)
                        {
                            cardIndex = i;
                            break;
                        }
                    }
                    print("index = " + cardIndex + " count = " + cardShuffles.Count);
                    //var list = cardShuffles.Find(x => x.cardNo == _removedCard.card.cardNo && x.color == _removedCard.card.color && x.cardSprite == _removedCard.card.cardSprite);
                    DiscardCardSocket(player1.playerNo, /*cardShuffles.IndexOf(_removedCard.card)*/cardIndex);
                    takeDiscardedCardButton.SetAsLastSibling();
                    dropButtonImage.sprite = disabledDiscardButton;

                    for (int i = 0; i < cardsHolder.childCount; i++)
                    {
                        for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
                        {
                            cardsHolder.GetChild(i).GetChild(j).transform.localPosition = new Vector3(cardsHolder.GetChild(i).GetChild(j).transform.localPosition.x, 0f, 0f);
                        }
                    }
                    for (int i = 0; i < temp.childCount; i++)
                    {
                        cardsToGroup.Add(temp.GetChild(i).gameObject);
                    }
                    GroupButtonClick();
                });
            }
            else
            {

            }
            //CardsChecker();
        }
        else if (cardsToGroup.Count != 1 && isMyTurn)
            discardCardObject.SetActive(true);
        else if (!isMyTurn)
            waitforTurnObject.SetActive(true);
        else if (player.cards.Count != 14)
            drawCardObject.SetActive(true);
    }

    public void GroupButtonClick()
    {
        if (cardsToGroup.Count > 1)
            StartCoroutine(GroupCards());

    }

    public void FinishButtonClick()
    {
        //int cardCount = 0;
        //for (int i = 0; i < cardsHolder.childCount; i++)
        //    cardCount += cardsHolder.GetChild(i).childCount;
        //if(cardCount == 14 && cardsToGroup.Count == 1)
        //{
        //    cardsToGroup[0].transform.SetParent(finishSlot);
        //    cardsToGroup[0].transform.DOLocalMove(Vector3.zero, 0.05f);
        //    cardsToGroup[0].transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.05f);
        //    cardsToGroup.RemoveAt(0);
        //}

    }
    #endregion

    public void FinishConfirmation()
    {
        int cardCount = 0;
        for (int i = 0; i < cardsHolder.childCount; i++)
            cardCount += cardsHolder.GetChild(i).childCount;
        if (cardCount == 14 && cardsToGroup.Count == 1 && isMyTurn)
        {
            SoundManager.Instance.ButtonClick();

            finishDialog.SetActive(true);
        }
    }

    public void FinishYesButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        int cardCount = 0;
        for (int i = 0; i < cardsHolder.childCount; i++)
            cardCount += cardsHolder.GetChild(i).childCount;
        if (player.cards.Count == 14 && cardsToGroup.Count == 1)
        {
            cardsToGroup[0].transform.SetParent(finishSlot);
            cardsToGroup[0].transform.DOLocalMove(Vector3.zero, 0.05f);
            cardsToGroup[0].transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.05f);
            player.cards.Remove(cardsToGroup[0].GetComponent<PoolCardScript>());
            player.cardImages.Remove(cardsToGroup[0].GetComponent<Image>());
            cardsToGroup.RemoveAt(0);
            CardsChecker();
            //resultDialog.SetActive(true);
            //FinishGameCallSocket(totalScore);
            finishDialog.SetActive(false);
            if (totalScore == 0)
                isValidDeclaration = true;
            else
            {
                totalScore = 80;
                isValidDeclaration = false;
                PackButtonClick();
                return;
            }
            foreach (var item in playerSquList)
            {
                item.NotATurn();
                item.fillLine.fillAmount = 1f;
            }
            //player1.NotATurn();
            player1.isGameComplete = true;
            player1.fillLine.fillAmount = 1f;
            isGameComplete = true;
            finishButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
            ArrangeCardsCall();
        }
    }

    public void SubmitButtonClick()
    {
        arrangeCardsDialog.SetActive(false);
        if (player1.isPack == false)
        {
            CardsChecker();
            isTimerComplete = true;
        }
        InsertCardDataForSocket();
        resultDialog.SetActive(true);
        player1.isMyTimerComplete = true;
        FinishGameCallSocket(totalScore, "Complete");
        //resultDialog.SetActive(true);
    }

    private IEnumerator GroupCards()//Selected cards will be grouped together
    {
        GameObject obj = Instantiate(newGroup.gameObject, cardsHolder);
        GameObject checker = Instantiate(validCheckPrefab.gameObject, validCheckerTransform);
        //obj.transform.SetSiblingIndex(cardsHolder.childCount - 2);
        obj.transform.SetAsLastSibling();
        checker.transform.SetAsLastSibling();
        obj.transform.localPosition = new Vector3(660f, -180f, 0f);//450f previous value
        checker.transform.localPosition = new Vector3(660f, 0f, 0f);

        ////Sorting Cards by Numbers in ascending order////
        List<PoolCardScript> groupedCards = new List<PoolCardScript>();
        for (int i = 0; i < cardsToGroup.Count; i++)
            groupedCards.Add(cardsToGroup[i].GetComponent<PoolCardScript>());

        groupedCards = groupedCards.OrderBy(x => x.card.cardNo).ToList();
        cardsToGroup.Clear();

        for (int i = groupedCards.Count - 1; i >= 0; i--)
            cardsToGroup.Add(groupedCards[i].gameObject);

        ////Sorting Cards by Numbers in ascending order////

        for (int i = 0; i < cardsToGroup.Count; i++)
        {
            cardsToGroup[i].transform.SetParent(obj.transform, true);
            cardsToGroup[i].transform.DOLocalMove(new Vector3(i * -60f, 0f, 0f), 0.1f);
            cardsToGroup[i].transform.SetAsFirstSibling();
        }

        int pairCount = 0;
        int singleCount = 0;
        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            if (cardsHolder.GetChild(i).childCount == 2)
                pairCount++;
            else if (cardsHolder.GetChild(i).childCount == 1)
                singleCount++;
        }
        if (pairCount > 2)
        {
            for (int i = 0; i < cardsHolder.childCount; i++)
            {
                if (cardsHolder.GetChild(i).childCount == 2 && i != cardsHolder.childCount - 1)
                {
                    for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
                    {
                        if (cardsHolder.GetChild(i).childCount != 0)
                            cardsHolder.GetChild(i).GetChild(0).SetParent(cardsHolder.GetChild(cardsHolder.childCount - 2));
                    }
                }
            }
        }

        if (singleCount > 2)
        {
            for (int i = 0; i < cardsHolder.childCount; i++)
            {
                if (cardsHolder.GetChild(i).childCount == 1)
                {
                    for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
                    {
                        if (cardsHolder.GetChild(i).childCount != 0)
                            cardsHolder.GetChild(i).GetChild(0).SetParent(cardsHolder.GetChild(cardsHolder.childCount - 2));
                    }
                }
            }
        }



        Transform tempTransform = obj.transform;
        for (int i = cardsHolder.childCount - 2; i >= 0; i--)
        {
            if (cardsHolder.GetChild(i) != tempTransform && cardsHolder.GetChild(i).childCount != 0)
            {
                //if(pairCount == 2 && cardsHolder.GetChild(i).childCount == 2)
                //{
                //    for (int k = 0; k < cardsHolder.GetChild(i).childCount; k++)
                //    {
                //        cardsHolder.GetChild(i).GetChild(i)
                //    }
                //}
                for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
                {
                    cardsHolder.GetChild(i).GetChild(j).localPosition = new Vector3(j * -60f, 0f, 0f);
                    cardsHolder.GetChild(i).GetChild(j).SetAsFirstSibling();
                }
                cardsHolder.GetChild(i).transform.DOLocalMoveX(tempTransform.localPosition.x - 130f - (tempTransform.childCount * 60f), 0.1f);
                validCheckerTransform.GetChild(i).transform.DOLocalMoveX(tempTransform.localPosition.x - 130f - (tempTransform.childCount * 60f), 0.1f);
                yield return new WaitForSeconds(0.1f);
                //checker.transform.DOLocalMoveX()
                tempTransform = cardsHolder.GetChild(i);
            }
        }



        cardsToGroup.Clear();


        CardsChecker();
    }

    #region Game Logic
    public void CardsChecker()//checks each group of cards for valid sequences/impure sequences/ pairs and calculates points
    {
        playerCardsCount = 0;
        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
                playerCardsCount++;
        }

        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            List<CardSuffle> groupedCards = new List<CardSuffle>();
            for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
            {
                var card = cardsHolder.GetChild(i).GetChild(j).GetComponent<PoolCardScript>().card;
                groupedCards.Add(card);
            }
            bool pureSequenceCheck = CheckForPureSequence(groupedCards);
            if (pureSequenceCheck == true)
            {
                pureSequenceExists = true;
                break;
            }
            else
                pureSequenceExists = false;

        }
        int impureSequenceCount = 0;//should at least be 1 if pair is to be considered
        int pureSequenceCount = 0;//should at least be 1 if pair is to be considered
        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            List<CardSuffle> groupedCards = new List<CardSuffle>();
            for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
            {
                var card = cardsHolder.GetChild(i).GetChild(j).GetComponent<PoolCardScript>().card;
                groupedCards.Add(card);
            }
            bool impureSequenceCheck = CheckForImpureSequence(groupedCards);
            bool pureSequenceCheck = CheckForPureSequence(groupedCards);
            if (impureSequenceCheck == true)
                impureSequenceCount++;
            else if (pureSequenceCheck)
                pureSequenceCount++;

        }

        List<PointRummyWinCheck> scoreList = new List<PointRummyWinCheck>();

        int pairAmount = 0;//cannot be > 2
        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            List<CardSuffle> groupedCards = new List<CardSuffle>();
            PointRummyWinCheck validCheck = new PointRummyWinCheck();
            for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
            {
                if (validCheckerTransform.GetChild(i).localPosition.x == cardsHolder.GetChild(i).localPosition.x)
                {
                    validCheck = validCheckerTransform.GetChild(i).GetComponent<PointRummyWinCheck>();
                }
                var card = cardsHolder.GetChild(i).GetChild(j).GetComponent<PoolCardScript>().card;
                groupedCards.Add(card);
            }
            if (groupedCards.Count != 0)
            {
                if (validCheck != null && pureSequenceExists == true)
                {
                    bool pureSequenceCheck = CheckForPureSequence(groupedCards);
                    bool impureSequenceCheck = CheckForImpureSequence(groupedCards);
                    bool pairCheck = CheckForPair(groupedCards);

                    if (pureSequenceCheck == true)
                    {
                        validCheck.validCheckImage.sprite = correct;
                        validCheck.validCheckText.text = "Finished";
                        validCheck.score = 0;
                        scoreList.Add(validCheck);
                    }
                    else if (impureSequenceCheck == true)
                    {
                        validCheck.validCheckImage.sprite = correct;
                        //impureSequenceCount++;
                        validCheck.validCheckText.text = "Finished";
                        validCheck.score = 0;
                        scoreList.Add(validCheck);
                    }
                    else if (pairCheck == true && pairAmount < 3 && ((impureSequenceCount == 1 && pureSequenceCount == 1) || impureSequenceCount > 1 || pureSequenceCount > 1))
                    {
                        pairAmount++;
                        validCheck.validCheckImage.sprite = correct;
                        validCheck.validCheckText.text = "Finished";
                        validCheck.score = 0;
                        scoreList.Add(validCheck);
                    }
                    else
                    {
                        validCheck.validCheckImage.sprite = wrong;
                        validCheck.score = GroupedCardScoreCalculation(groupedCards);
                        validCheck.validCheckText.text = "Invalid" + "(" + validCheck.score.ToString() + ")";
                        //totalScore += validCheck.score;
                        scoreList.Add(validCheck);
                    }
                }
                else if (validCheck != null && pureSequenceExists == false)
                {
                    //TotalScoreCalculation(groupedCards);
                    validCheck.score = GroupedCardScoreCalculation(groupedCards);
                    validCheck.validCheckImage.sprite = wrong;
                    validCheck.validCheckText.text = "Invalid" + "(" + validCheck.score.ToString() + ")";
                    scoreList.Add(validCheck);
                }


            }
        }
        totalScore = 0;
        foreach (var item in scoreList)
            totalScore += item.score;

        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            if (cardsHolder.GetChild(i).childCount == 0)
            {
                Destroy(cardsHolder.GetChild(i).gameObject);
                Destroy(validCheckerTransform.GetChild(i).gameObject);
            }
        }

        totalScoreText.text = "Score: " + totalScore.ToString();
    }

    public bool CheckForPureSequence(List<CardSuffle> cards)
    {
        cards = cards.OrderBy(x => x.cardNo).ToList();
        bool isSameColour = cards.All(x => x.color == cards.First().color);

        List<int> cardNumbers = new List<int>();

        for (int i = 0; i < cards.Count; i++)
            cardNumbers.Add(cards[i].cardNo);



        if (isSameColour == true && cards.Count > 2)
        {

            if (cardNumbers.Distinct().ToList().Count == cards.Count)
            {
                int listCount = cards.Count;
                CardSuffle acesCard = new CardSuffle();
                if (cards.Any(x => x.cardNo == 14))
                {
                    for (int i = cards.Count - 1; i >= 0; i--)
                    {
                        if (cards[i].cardNo == 14)
                        {
                            acesCard = cards[i];
                            cards.RemoveAt(i);
                        }
                    }
                }

                bool isSequence = cards.Zip(cards.Skip(1), (first, second) => second.cardNo - first.cardNo == 1).All(x => x);
                if (listCount != cards.Count)
                    cards.Add(acesCard);
                if (isSequence)
                {

                }
                return isSequence;
            }
        }
        return false;
    }

    public bool CheckForImpureSequence(List<CardSuffle> cards)
    {
        cards = cards.OrderBy(x => x.cardNo).ToList();//sorting by card numbers
        List<CardSuffle> jokerCard = new List<CardSuffle>();
        List<CardSuffle> wildJoker = new List<CardSuffle>();
        List<int> cardNumbers = new List<int>();
        if (cards.Any(x => x.color == CardColorType.JOKER) || cards.Any(x => x.isWildJoker == true))
        {
            int jokerCounter = 0;
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                if (cards[i].color == CardColorType.JOKER)
                {
                    jokerCounter++;
                    jokerCard.Add(cards[i]);
                    cards.RemoveAt(i);
                }
                else if (cards[i].isWildJoker == true)
                {
                    jokerCounter++;
                    wildJoker.Add(cards[i]);
                    cards.RemoveAt(i);
                }
            }

            for (int i = 0; i < cards.Count; i++)
                cardNumbers.Add(cards[i].cardNo);
            int sequence = 0;
            if (cards.Count > 1 && cardNumbers.Distinct().ToList().Count == cards.Count && cards.All(x => x.color == cards.First().color))
            {
                int temp = 0;
                if (cards.Any(x => x.cardNo == 14))
                {
                    if (cards.Any(x => x.cardNo == 12 || x.cardNo == 13 || x.cardNo == 11 || x.cardNo == 10))
                    {
                        //do nothing
                    }
                    else if (cards.Any(x => x.cardNo == 2 || x.cardNo == 3 || x.cardNo == 4 || x.cardNo == 5))
                    {
                        foreach (var item in cards)
                        {
                            if (item.cardNo == 14)//exception for aces
                                item.cardNo = 1;
                        }
                    }

                }
                for (int i = 0; i < cards.Count; i++)
                {
                    if (cards[i].cardNo > temp)
                    {
                        if (temp != 0)
                        {
                            if (cards[i].cardNo == temp + 1)
                            {
                                sequence++;
                            }
                            else if (cards[i].cardNo == temp + 2 && jokerCounter > 0)
                            {
                                sequence++;
                                jokerCounter--;
                            }
                            else if (cards[i].cardNo == temp + 3 && jokerCounter >= 2)
                            {
                                sequence++;
                                jokerCounter -= 2;
                            }
                            else if (cards[i].cardNo == temp + 4 && jokerCounter >= 3)
                            {
                                sequence++;
                                jokerCounter -= 3;
                            }
                        }
                        temp = cards[i].cardNo;
                    }
                }
                if (cards.Any(x => x.cardNo == 1))
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].cardNo == 1)
                            cards[i].cardNo = 14;
                    }
                }
                if (sequence == cards.Count - 1)
                {
                    return true;
                }
            }
            else if (cards.Count == 1 && jokerCounter > 1)
            {
                cards.AddRange(jokerCard);
                cards.AddRange(wildJoker);
                return true;
            }
            else if (cards.Distinct().ToList().Count != cards.Count)
            {
                cards.AddRange(jokerCard);
                cards.AddRange(wildJoker);
                return false;
            }

        }

        return false;
    }

    public bool CheckForPair(List<CardSuffle> cards)
    {
        if (cards.Count >= 3)
        {
            List<CardColorType> colourCheck = new List<CardColorType>();//list for checking if the cards have same colour
            bool isSameColour = false;

            List<CardSuffle> jokers = new List<CardSuffle>();
            int jokerCount = 0;
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                if (cards[i].color == CardColorType.JOKER || cards[i].isWildJoker)
                {
                    jokerCount++;
                    jokers.Add(cards[i]);
                    cards.RemoveAt(i);
                }
            }
            foreach (var item in cards)
                colourCheck.Add(item.color);
            if (colourCheck.Distinct().ToList().Count == cards.Count)
                isSameColour = false;
            else if (colourCheck.Distinct().ToList().Count != cards.Count)
                isSameColour = true;
            if (isSameColour)
            {
                cards.AddRange(jokers);
                return false;
            }

            if (cards.All(x => x.cardNo == cards.First().cardNo))//condition to check if all the cards are same in the list
            {
                cards.AddRange(jokers);
                return true;
            }
        }
        return false;
    }

    #endregion
    //public void TotalScoreCalculation(List<CardSuffle> cards)
    //{
    //    foreach (var item in cards)
    //    {
    //        if (item.cardNo >= 10 && item.isWildJoker == false)
    //            totalScore += 10;
    //        else if(item.isWildJoker == false)
    //            totalScore += item.cardNo;
    //    }
    //}
    public int GroupedCardScoreCalculation(List<CardSuffle> cards)
    {
        int score = 0;
        foreach (var item in cards)
        {
            if (item.cardNo >= 10 && item.isWildJoker == false)
                score += 10;
            else if (item.isWildJoker == false)
                score += item.cardNo;
        }
        return score;
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
    // Start is called before the first frame update


    private void ShowPopUp()
    {
        entryPopup.gameObject.SetActive(true);
        //Time.timeScale = 0;
    }

    public void CloseEntryPopup()
    {
        entryPopup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayerFound()
    {

        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.pointRummyRequirePlayer)
        {
            CreateAdmin();
            if (DataManager.Instance.joinPlayerDatas.Count > 5)
            {
                StartCoroutine(WaitGameToComplete(CheckNewPlayers));
            }
            if (DataManager.Instance.joinPlayerDatas.Count >= 2 && isAdmin)
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
        //if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        //{
        //    MainMenuManager.Instance.CheckPlayers();
        //}
        //ResetBot();
        ////Activating bots
        //ActivateBotPlayers();
    }

    public void DisplayCurrentBalance()
    {
        totalPriceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }



    //#region GamePlay Manager

    //public RummyWinMaintain MatchResult(CardSuffle card1, CardSuffle card2, CardSuffle card3)
    //{
    //    RummyWinMaintain teenPattiWinMaintain = new RummyWinMaintain();

    //    List<CardSuffle> newData = new List<CardSuffle>();
    //    newData.Add(card1);
    //    newData.Add(card2);
    //    newData.Add(card3);

    //    newCardSS = newData;


    //    newCardSS1 = NewSort(newData);


    //    bool isColor = IsColorMatch(newCardSS1);
    //    List<CardSuffle> threeCards = GetThreeCard(newCardSS1);
    //    List<CardSuffle> twoCards = GetTwoCard(newCardSS1);
    //    List<CardSuffle> ronCards = RonValue(newCardSS1);
    //    List<CardSuffle> highCards = HighCard(newCardSS1);



    //    //ronCard

    //    if (threeCards.Count == 3)
    //    {
    //        //Three List
    //        teenPattiWinMaintain.ruleNo = 1;
    //        teenPattiWinMaintain.winList = threeCards;
    //    }
    //    else if (isColor && ronCards.Count == 3)
    //    {
    //        teenPattiWinMaintain.ruleNo = 2;
    //        teenPattiWinMaintain.winList = ronCards;
    //    }
    //    else if (ronCards.Count == 3)
    //    {
    //        //ron List
    //        teenPattiWinMaintain.ruleNo = 3;
    //        teenPattiWinMaintain.winList = ronCards;
    //    }
    //    else if (isColor)
    //    {
    //        //High Card
    //        teenPattiWinMaintain.ruleNo = 4;
    //        teenPattiWinMaintain.winList = highCards;
    //    }
    //    else if (twoCards.Count == 3)
    //    {
    //        //Two Cards
    //        teenPattiWinMaintain.ruleNo = 5;
    //        teenPattiWinMaintain.winList = twoCards;
    //    }
    //    else if (highCards.Count == 3)
    //    {
    //        //High Cards
    //        teenPattiWinMaintain.ruleNo = 6;
    //        teenPattiWinMaintain.winList = highCards;
    //    }


    //    return teenPattiWinMaintain;
    //    //GetTwoCard(newCardSS1);
    //}

    //List<CardSuffle> GetTwoCard(List<CardSuffle> cards)
    //{
    //    List<CardSuffle> twoCardSuffle = new List<CardSuffle>();
    //    int cnt1 = 0;
    //    int cnt2 = 0;
    //    int startNo = cards[0].cardNo;
    //    int endNo = cards[1].cardNo;
    //    print("Card Satrt No : " + cards.Count);
    //    print("Card End No : " + endNo);
    //    for (int i = 0; i < cards.Count; i++)
    //    {


    //        if (cards[i].cardNo == startNo)
    //        {
    //            cnt1++;
    //        }
    //        else if (cards[i].cardNo == endNo)
    //        {
    //            cnt2++;
    //        }
    //    }
    //    print("card cnt 1 : " + cnt1);
    //    print("card cnt 2 : " + cnt2);
    //    if (cnt1 == 2)
    //    {
    //        int noEnter = -1;
    //        for (int i = 0; i < cards.Count; i++)
    //        {
    //            print("Card No : " + cards[i].cardNo);
    //            if (cards[i].cardNo == startNo)
    //            {
    //                print("Enter Card");
    //                twoCardSuffle.Add(cards[i]);
    //            }
    //            else
    //            {
    //                noEnter = i;
    //            }
    //        }
    //        //print("No Enter : " + noEnter);

    //        twoCardSuffle.Add(cards[noEnter]);

    //    }
    //    else if (cnt2 == 2)
    //    {
    //        int noEnter = -1;
    //        for (int i = 0; i < cards.Count; i++)
    //        {
    //            if (cards[i].cardNo == endNo)
    //            {
    //                twoCardSuffle.Add(cards[i]);
    //            }
    //            else
    //            {
    //                noEnter = i;
    //            }
    //        }
    //        twoCardSuffle.Add(cards[noEnter]);
    //    }


    //    print("Enter twoCard Suffle Count : " + twoCardSuffle.Count);
    //    return twoCardSuffle;

    //}

    //bool IsColorMatch(List<CardSuffle> cards)
    //{
    //    int cnt1 = 0;
    //    int cnt2 = 0;
    //    int cnt3 = 0;
    //    int cnt4 = 0;
    //    for (int i = 0; i < cards.Count; i++)
    //    {
    //        if (cards[i].color == CardColorType.Clubs)
    //        {
    //            cnt1++;
    //        }
    //        else if (cards[i].color == CardColorType.Diamonds)
    //        {
    //            cnt2++;
    //        }
    //        else if (cards[i].color == CardColorType.Spades)
    //        {
    //            cnt3++;
    //        }
    //        else if (cards[i].color == CardColorType.Hearts)
    //        {
    //            cnt4++;
    //        }
    //    }

    //    if (cnt1 >= 3 || cnt2 >= 3 || cnt3 >= 3 || cnt4 >= 3)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //List<CardSuffle> RonValue(List<CardSuffle> cards)
    //{


    //    List<CardSuffle> ronvalue = new List<CardSuffle>();
    //    bool isRon = false;
    //    if (cards[1].cardNo == cards[0].cardNo + 1 && cards[2].cardNo == cards[0].cardNo + 2)
    //    {
    //        isRon = true;
    //    }
    //    else if (cards[0].cardNo == 2 && cards[1].cardNo == 3 && cards[2].cardNo == 14)
    //    {
    //        isRon = true;
    //    }

    //    if (isRon)
    //    {
    //        ronvalue = cards;
    //    }



    //    print("is Ron : " + isRon);
    //    return ronvalue;
    //}

    //List<CardSuffle> GetThreeCard(List<CardSuffle> cards)
    //{
    //    List<CardSuffle> threeCardSuffle = new List<CardSuffle>();
    //    int cnt = 0;
    //    int startNo = cards[0].cardNo;

    //    for (int i = 0; i < cards.Count; i++)
    //    {
    //        if (cards[i].cardNo == startNo)
    //        {
    //            cnt++;
    //        }
    //    }

    //    if (cnt == 3)
    //    {
    //        threeCardSuffle = cards;
    //    }

    //    for (int i = 0; i < threeCardSuffle.Count; i++)
    //    {
    //        print(i + "-" + threeCardSuffle[i].cardNo);
    //    }

    //    return threeCardSuffle;

    //}

    //List<CardSuffle> HighCard(List<CardSuffle> cards)
    //{
    //    print("high cards count : " + cards.Count);
    //    List<CardSuffle> highCards = new List<CardSuffle>();
    //    for (int i = cards.Count - 1; i >= 0; i--)
    //    {
    //        highCards.Add(cards[i]);
    //    }

    //    return highCards;
    //}

    //List<CardSuffle> NewSort(List<CardSuffle> cards)
    //{
    //    List<CardSuffle> newCards = new List<CardSuffle>();
    //    //newCards = cards;
    //    for (int i = 0; i < cardSufflesSort.Count; i++)
    //    {
    //        for (int j = 0; j < cards.Count; j++)
    //        {
    //            if (cardSufflesSort[i].cardNo == cards[j].cardNo && cardSufflesSort[i].color == cards[j].color)
    //            {
    //                CardSuffle c = new CardSuffle();
    //                c.cardNo = cardSufflesSort[i].cardNo;
    //                c.color = cardSufflesSort[i].color;
    //                c.cardSprite = cardSufflesSort[i].cardSprite;
    //                //newCards.Add(cardSufflesSort[i]);
    //                newCards.Add(c);
    //                break;
    //            }
    //        }
    //    }
    //    print("new cards Count : " + newCards.Count);
    //    for (int i = 0; i < newCards.Count; i++)
    //    {
    //        if (newCards[i].cardNo == 1)
    //        {
    //            newCards[i].cardNo = 14;
    //        }
    //        else if (newCards[i].cardNo == 11)
    //        {
    //            newCards[i].cardNo = 13;
    //        }
    //        else if (newCards[i].cardNo == 13)
    //        {
    //            newCards[i].cardNo = 11;

    //        }
    //    }

    //    return newCards;
    //}

    //#endregion

    public void EnableSeeCards()
    {
        boxDisplayCount++;
        if (boxDisplayCount != 4) return;
        //SeeButtonClick();
    }

    //#region GamePlay Button And Other Manage

    public void SeeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        for (int i = 0; i < player1.seeObj.Length; i++)
        {
            player1.seeObj[i].SetActive(false);
        }

        player1.CardDisplay();
        DisplayRules();

        currentPriceValue = minLimitValue * 2;
        player1.isSeen = true;
        player1.isBlind = false;
        player1.isPack = false;
        currentPriceIndex = 1;
        blindx2button.SetActive(false);
        plusBtn.gameObject.SetActive(true);
        minusBtn.gameObject.SetActive(true);
        priceBtnTxt.gameObject.transform.parent.transform.localPosition = new Vector3(490.00f, 90.81f, 0.00f);
        priceBtnTxt.text = "Chaal\n" + currentPriceValue;
        ChangeCardStatus("SEEN", player1.playerNo);
    }

    //public void GiftButtonClick(TeenPattiPlayer giftPlayer)
    //{
    //    SoundManager.Instance.ButtonClick();
    //    giftScreenObj.SetActive(true);
    //    GiftSendManager.Instance.gameName = "TeenPatti";
    //    GiftSendManager.Instance.teenPattiOtherPlayer = giftPlayer;
    //}

    public IEnumerator RestartGamePlay()
    {
        if (player1.playerGamePoints > DataManager.Instance.pointLimit)
        {
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        resultReceivedCount = 0;
        waitForNewRound.SetActive(false);
        finalCards.Clear();
        finishButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        for (int i = 0; i < resultCardsHolder.Length; i++)//disabling the cards from previous round to show new cards
        {
            for (int j = 0; j < resultCardsHolder[i].childCount; j++)
                    resultCardsHolder[i].GetChild(j).gameObject.SetActive(false);
        }
        isMyTurn = false;
        closedDeck.Clear();
        player.shuffledList.Clear();
        for (int i = 0; i < cardShuffles.Count; i++)
            cardShuffles[i].isWildJoker = false;
        
        //closedDeck.Clear();
        distributedCardsList.Clear();
        isValidDeclaration = true;
        isGameStarted = false;
        player1.isGameComplete = false;
        player1.isMyTimerComplete = false;
        isGameComplete = false;
        resultDialog.SetActive(false);
        firstCardDrawn = false;
        discardedCardDeck.Clear();
        foreach (var item in playerSquList)
            item.isPack = false;


        //DeleteAllCoins();
        //yield return new WaitForSeconds(5f);
        yield return null;

        //print("Enther The Generate Player");
        if (isAdmin)
        {
            //CheckNewPlayers();
            StartGamePlay();
            //SetRoomData();
            //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
            print("Enther The Generate Player1");
            //isBotActivate = true;

        }
    }

    private void WildJokerSetUp()
    {
        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
                cardsHolder.GetChild(i).GetChild(j).GetChild(0).gameObject.SetActive(false);
        }
        cardShuffles[wildJokerIndex].isWildJoker = true;
        closedDeck[wildJokerIndex].isWildJoker = true;
        for (int i = 0; i < cardShuffles.Count; i++)
        {
            if (cardShuffles[wildJokerIndex].cardNo == cardShuffles[i].cardNo)
            {
                cardShuffles[i].isWildJoker = true;
                closedDeck[i].isWildJoker = true;
                wildJokerIndicator.sprite = cardShuffles[i].cardSprite;
                wildJokerIndicator.sprite = closedDeck[i].cardSprite;
            }
            else
            {
                cardShuffles[i].isWildJoker = false;
                closedDeck[i].isWildJoker = false;
            }
        }
    }

    private void DiscardPileSetUp()
    {
        //logic to set up the first card for the discard pile
        GameObject newCard = Instantiate(cardPrefab, discardPile);
        newCard.transform.position = deck.position;
        newCard.transform.localScale = new Vector2(0.8f, 0.8f);
        var card = newCard.GetComponent<PoolCardScript>();
        card.button.interactable = false;
        card.card = closedDeck[openCardIndex];
        card.card.cardNo = closedDeck[openCardIndex].cardNo;
        card.card.cardSprite = closedDeck[openCardIndex].cardSprite;
        card.card.isWildJoker = closedDeck[openCardIndex].isWildJoker;
        newCard.GetComponent<Image>().sprite = card.card.cardSprite;
        if (card.card.isWildJoker)
            card.wildJoker.SetActive(true);
        newCard.transform.position = deck.transform.position;
        discardedCardDeck.Add(card);

        closedDeck.RemoveAt(openCardIndex);
        newCard.transform.DOLocalMove(Vector3.zero, 0.05f);
        drawDiscardedCard.transform.SetAsLastSibling();
    }

    public void StartGamePlay()
    {
        //StartCoroutine(RestartGamePlay());

        if (isAdmin)
        {
            var copyList = new List<CardSuffle>(cardShuffles);
            closedDeck = copyList;
            wildJokerIndex = UnityEngine.Random.Range(0, cardShuffles.Count - 5);
            WildJokerSetUp();//SetUp Joker before distributing cards

            InitializeCards();//Cards will be distributed first to admin 

            openCardIndex = UnityEngine.Random.Range(0, closedDeck.Count);
            DiscardPileSetUp();//Set up discard pile
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        SoundManager.Instance.CasinoTurnSound();
        isGameStop = true;
        waitNextRoundScreenObj.SetActive(false);
        playerFindScreenObj.SetActive(false);
        bootValue = 1f;
        potLimitValue = 30f;
        minLimitValue = 5f;
        maxLimitValue = 1000f;
        incrementNo = 5f;
        //minChaalValue = minLimitValue * 2;
        //maxChaalValue = maxLimitValue * 2;
        //minBlindValue = minLimitValue;
        //maxBlindValue = maxLimitValue;


        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            //teenPattiPlayers[i].isSeen = false;
            //teenPattiPlayers[i].isPack = false;
            //teenPattiPlayers[i].isBlind = true;
            teenPattiPlayers[i].userTurnCount = 0;
            teenPattiPlayers[i].SetActiveTrue();
            teenPattiPlayers[i].inactiveCount = 0;
        }

        currentPriceValue = minLimitValue;
        //priceBtnTxt.text = "Blind\n" + currentPriceValue;
        //minusBtn.interactable = false;
        rulesTab.SetActive(false);
        roundCounter = 0;
        //totalBetAmount = 0;
        boxDisplayCount = 0;
        //runningPriceIndex = 0;
        //currentPriceIndex = 0;
        //StartBet();//Greejesh
        //betAmountTxt.text = totalBetAmount.ToString();
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            PoolRummyPlayerManager currentPlayer = teenPattiPlayers[i];
            for (int j = 0; j < currentPlayer.playerWinObj.Length; j++)
            {
                //currentPlayer.playerWinObj[j].SetActive(false);
            }
            //currentPlayer.packImg.SetActive(false);
            //currentPlayer.seenImg.SetActive(false);
            //currentPlayer.cardImg1.gameObject.SetActive(false);
            //currentPlayer.cardImg2.gameObject.SetActive(false);
            //currentPlayer.cardImg3.gameObject.SetActive(false);
            //currentPlayer.delearObj.SetActive(false);

            //for (int j = 0; j < currentPlayer.seeObj.Length; j++)
            //{
            //    currentPlayer.seeObj[j].SetActive(false);
            //}
        }

        isGameStarted = true;

        for (int i = 0; i < cardsHolder.GetChild(0).childCount; i++)
            cardsToGroup.Add(cardsHolder.GetChild(0).GetChild(i).gameObject);
        GroupButtonClick();


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
            player6.gameObject.SetActive(false);

            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.playerNo = (i + 1);
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                }
                else
                {
                    player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);

                playerSquList.Add(player1);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {

                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {

                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 2)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                    player1.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    player1.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                    player1.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    player1.UpdateAvatar();
                    player1.playerNo = (i + 1);
                }
            }

            if (player1.playerNo == 1)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                player6.gameObject.SetActive(false);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 3)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(false);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 4)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
        else if (DataManager.Instance.joinPlayerDatas.Count == 6)
        {
            player1.gameObject.SetActive(true);
            for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    player1.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                player6.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 5)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 4)
                        {
                            player6.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player6.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player6.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player6.playerNo = (i + 1);
                            player6.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player6.UpdateAvatar();
                            playerSquList.Add(player6);
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
                player6.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 5)
                    {
                        if (cntPlayer == 0)
                        {
                            player6.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player6.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player6.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player6.playerNo = (i + 1);
                            player6.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player6.UpdateAvatar();
                            playerSquList.Add(player6);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 4)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
            else if (player1.playerNo == 3)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                player6.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 5)
                    {
                        if (cntPlayer == 0)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player6.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player6.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player6.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player6.playerNo = (i + 1);
                            player6.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player6.UpdateAvatar();
                            playerSquList.Add(player6);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 2)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 4)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
            else if (player1.playerNo == 4)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                player6.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 5)
                    {
                        if (cntPlayer == 0)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player4.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player4.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player4.playerNo = (i + 1);
                            player4.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player4.UpdateAvatar();
                            playerSquList.Add(player4);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player5.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player6.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player6.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player6.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player6.playerNo = (i + 1);
                            player6.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player6.UpdateAvatar();
                            playerSquList.Add(player6);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 3)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player2.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player2.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player2.playerNo = (i + 1);
                            player2.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player2.UpdateAvatar();
                            playerSquList.Add(player2);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 4)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
            else if (player1.playerNo == 5)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                player6.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 5)
                    {
                        if (cntPlayer == 0)
                        {
                            player3.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player3.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player3.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player3.playerNo = (i + 1);
                            player3.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player3.UpdateAvatar();
                            playerSquList.Add(player3);
                            // playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 1)
                        {
                            player4.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player6.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player6.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player6.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player6.playerNo = (i + 1);
                            player6.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player6.UpdateAvatar();
                            playerSquList.Add(player6);
                            //playerSquList.Add(player1);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 4)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
            else if (player1.playerNo == 6)
            {
                player2.gameObject.SetActive(true);
                player3.gameObject.SetActive(true);
                player4.gameObject.SetActive(true);
                player5.gameObject.SetActive(true);
                player6.gameObject.SetActive(true);
                playerSquList.Add(player1);

                int cntPlayer = 0;
                for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
                {
                    if (!DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id) && cntPlayer < 5)
                    {
                        if (cntPlayer == 0)
                        {
                            player2.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
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
                            player5.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player5.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player5.playerNo = (i + 1);
                            player5.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player5.UpdateAvatar();
                            playerSquList.Add(player5);
                            cntPlayer++;
                        }
                        else if (cntPlayer == 4)
                        {
                            player6.playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                            player6.playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                            player6.lobbyId = DataManager.Instance.joinPlayerDatas[i].lobbyId;
                            player6.playerNo = (i + 1);
                            player6.avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                            player6.UpdateAvatar();
                            playerSquList.Add(player6);
                            cntPlayer++;
                        }
                    }
                }
            }

        }

        int playerSend = DataManager.Instance.joinPlayerDatas.Count;

        float speed = 0.2f;

        // first card animation
        //for (int i = 0; i < 13; i++)
        //{
        //    //if(teenPattiPlayers[i].)
        //    //while (p < playerSend)
        //    //{
        //    if (i < playerSend)
        //    {
        //        GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
        //        SoundManager.Instance.CasinoCardMoveSound();
        //        obj.transform.position = cardTmpStart.transform.position;

        //        obj.transform.DOMove(teenPattiPlayers[i].cardImg1.transform.position, speed).OnComplete(() =>
        //        {
        //            Destroy(obj);
        //            teenPattiPlayers[i].cardImg1.gameObject.SetActive(true);
        //        });

        //        yield return new WaitForSeconds(speed);
        //    }

        //}
        //yield return new WaitForSeconds(speed);
        // second card animation
        //for (int i = 0; i < teenPattiPlayers.Count; i++)
        //{
        //    //if(teenPattiPlayers[i].)
        //    //while (p < playerSend)
        //    //{
        //    if (i < playerSend)
        //    {
        //        GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
        //        SoundManager.Instance.CasinoCardMoveSound();
        //        obj.transform.position = cardTmpStart.transform.position;


        //        obj.transform.DOMove(teenPattiPlayers[i].cardImg2.transform.position, speed).OnComplete(() =>
        //        {
        //            Destroy(obj);
        //            teenPattiPlayers[i].cardImg2.gameObject.SetActive(true);
        //        });
        //        yield return new WaitForSeconds(speed);
        //    }

        //}
        //yield return new WaitForSeconds(speed);
        //Third card animation
        //for (int i = 0; i < teenPattiPlayers.Count; i++)
        //{
        //    //if(teenPattiPlayers[i].)
        //    //while (p < playerSend)
        //    //{
        //    if (i < playerSend)
        //    {
        //        GameObject obj = Instantiate(cardTmpPrefab, prefabParent.transform);
        //        SoundManager.Instance.CasinoCardMoveSound();
        //        obj.transform.position = cardTmpStart.transform.position;


        //        obj.transform.DOMove(teenPattiPlayers[i].cardImg3.transform.position, speed).OnComplete(() =>
        //        {
        //            Destroy(obj);
        //            teenPattiPlayers[i].cardImg3.gameObject.SetActive(true);
        //        });
        //        yield return new WaitForSeconds(speed);
        //    }

        //}

        //yield return new WaitForSeconds(speed);
        //for (int i = 0; i < player1.seeObj.Length; i++)
        //{
        //    player1.seeObj[i].SetActive(true);
        //}
        yield return null;

        for (int i = 0; i < playerSquList.Count; i++)
        {
            bottomBox.SetActive(true);
            if (playerSquList[i].playerNo == gameDealerNo)
            {
                playerSquList[i].RestartFillLine();
                //playerSquList[i].delearObj.SetActive(true);
                if (playerSquList[i].playerNo == player1.playerNo)
                {
                    isMyTurn = true;
                    dropButtonImage.sprite = enableDropButton;
                    bottomBox.SetActive(true);
                    DataManager.Instance.UserTurnVibrate();
                }
            }
            else
            {
                //playerSquList[i].delearObj.SetActive(false);
                playerSquList[i].NotATurn();
            }
        }

        isGameStop = false;

        //for (int i = 0; i < playerSquList.Count; i++)
        //{
        //    if (playerSquList[i].gameObject.activeSelf == true)
        //    {
        //        playerSquList[i].CardGenerate();
        //    }
        //}

        ActivateBotPlayers();
        isBotActivate = true;
    }

    public void ResetBot()
    {
        player1.isBot = false;
        player2.isBot = false;
        player3.isBot = false;
        player4.isBot = false;
        player5.isBot = false;
        player6.isBot = false;
    }

    private void ActivateBotPlayers()
    {

        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.joinPlayerDatas[i].userId.EndsWith("TeenPatti"))
            {
                playerSquList[i].isBot = true;
            }
        }
    }



    //#endregion

    //#region Panel Button

    //public void MenuButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    OpenMenuScreen();
    //}

    //public void MessageButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    messageScreeObj.SetActive(true);
    //}


    public void PackButtonClick()//Drop Button
    {
        if (!isGameStop && isMyTurn && player.cards.Count == 13)
        {
            //if (player.cards.Count == 14)
            //{
            //    int rng = UnityEngine.Random.Range(0, player.cards.Count);
            //    player.cards.RemoveAt(rng);
            //    Destroy(player.cards[rng].gameObject);
            //    cardsToGroup.Clear();
            //}
            if (isGameComplete)
            {
                isGameComplete = false;
                player1.isGameComplete = false;
            }
            SoundManager.Instance.ButtonClick();
            ChangeCardStatus("PACK", player1.playerNo);
            if (isValidDeclaration)
            {
                if (firstCardDrawn)
                    totalScore = 40;
                else
                    totalScore = 20;
            }
            ReturnCardsToDeck();
            //bottomBox.SetActive(false);
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

    //public void ShowButtonClick(Text t)
    //{
    //    if (isGameStop) return;
    //    SoundManager.Instance.ButtonClick();
    //    //BetAnim(player1, currentPriceValue);


    //    switch (t.text)
    //    {
    //        case "Show":
    //            ShowCardToAllUser();
    //            CheckFinalWinner("Show");
    //            //SendTeenPattiBet(player1.playerNo, 0, "Show", "", "");
    //            //ChangePlayerTurn(player1.playerNo);
    //            break;
    //        case "Side\nShow":
    //            if (CheckMoney(currentPriceValue) == false)
    //            {
    //                SoundManager.Instance.ButtonClick();
    //                OpenErrorScreen();
    //                return;
    //            }
    //            // BetAnim(player1, currentPriceValue);
    //            //SendTeenPattiBet(player1.playerNo, currentPriceValue, "SideShow", slideShowPlayer.playerId, player1.playerId);
    //            //DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 1);
    //            //playerBetAmount += currentPriceValue;
    //            //Game Stop and check the card and one card is pack
    //            showButton.interactable = false;
    //            Invoke(nameof(OnPopupButtonClick), delay);
    //            break;
    //    }
    //}

    //private void OnPopupButtonClick()
    //{
    //    if (isPopupOpen) return;
    //    isPopupOpen = true;
    //    sideShowPopupImage.gameObject.SetActive(true);
    //    //sideShowPopupImage.transform.localScale = Vector3.zero;
    //    /*sideShowPopupImage.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
    //    {
    //    });*/
    //    StartCoroutine(WaitForPopupClose());
    //}

    //private IEnumerator WaitForPopupClose()
    //{
    //    yield return new WaitUntil(() => !isPopupOpen);
    //    showButton.interactable = true;
    //}

    //public void ClosePopup()
    //{
    //    if (!isPopupOpen) return;
    //    isPopupOpen = false;
    //    sideShowPopupImage.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
    //    {
    //        sideShowPopupImage.gameObject.SetActive(false);
    //    });
    //}

    //public void MinusButtonClick()
    //{
    //    if (isGameStop) return;
    //    /*SoundManager.Instance.ButtonClick();
    //        minusBtn.interactable = false;
    //        plusBtn.interactable = true;

    //        currentPriceValue /= 2;
    //        if (!player1.isPack && player1.isBlind)
    //        {

    //            priceBtnTxt.text = "Blind\n" + currentPriceValue;
    //        }
    //        else if (!player1.isPack && player1.isSeen)
    //        {

    //            priceBtnTxt.text = "Chaal\n" + currentPriceValue;
    //        }*/
    //    SoundManager.Instance.ButtonClick();
    //    // minusBtn.interactable = false;
    //    // plusBtn.interactable = true;
    //    if (currentPriceIndex > 0 && currentPriceIndex > runningPriceIndex)
    //    {
    //        currentPriceIndex--;
    //        currentPriceValue = numbers[currentPriceIndex];
    //        //PriceTxt.text = price.ToString();
    //        if (currentPriceIndex == numbers.Length - 1)
    //        {
    //            plusBtn.interactable = false;
    //            minusBtn.interactable = true;
    //        }
    //        else if (currentPriceIndex == 0)
    //        {
    //            plusBtn.interactable = true;
    //            minusBtn.interactable = false;
    //        }
    //        else
    //        {
    //            plusBtn.interactable = true;
    //            minusBtn.interactable = true;
    //        }
    //    }
    //    else
    //    {
    //        plusBtn.interactable = true;
    //        minusBtn.interactable = false;
    //    }

    //    priceBtnTxt.text = player1.isPack switch
    //    {
    //        //currentPriceValue /= 2;
    //        false when player1.isBlind => "Blind\n" + currentPriceValue,
    //        false when player1.isSeen => "Chaal\n" + currentPriceValue,
    //        _ => priceBtnTxt.text
    //    };
    //}

    //public void PlusButtonClick()
    //{
    //    if (isGameStop) return;
    //    /*SoundManager.Instance.ButtonClick();
    //        plusBtn.interactable = false;
    //        minusBtn.interactable = true;

    //        currentPriceValue *= 2;
    //        if (!player1.isPack && player1.isBlind)
    //        {

    //            priceBtnTxt.text = "Blind\n" + currentPriceValue;
    //        }
    //        else if (!player1.isPack && player1.isSeen)
    //        {

    //            priceBtnTxt.text = "Chaal\n" + currentPriceValue;
    //        }*/
    //    SoundManager.Instance.ButtonClick();
    //    // plusBtn.interactable = false;
    //    // minusBtn.interactable = true;

    //    if (currentPriceIndex < numbers.Length - 1)
    //    {
    //        currentPriceIndex++;
    //        currentPriceValue = numbers[currentPriceIndex];
    //        //PriceTxt.text = price.ToString();
    //        if (currentPriceIndex == numbers.Length - 1)
    //        {
    //            plusBtn.interactable = false;
    //            minusBtn.interactable = true;
    //        }
    //        else if (currentPriceIndex == 0)
    //        {
    //            plusBtn.interactable = true;
    //            minusBtn.interactable = false;
    //        }
    //        else
    //        {
    //            plusBtn.interactable = true;
    //            minusBtn.interactable = true;
    //        }
    //    }

    //    priceBtnTxt.text = player1.isPack switch
    //    {
    //        //currentPriceValue *= 2;
    //        false when player1.isBlind => "Blind\n" + currentPriceValue,
    //        false when player1.isSeen => "Chaal\n" + currentPriceValue,
    //        _ => priceBtnTxt.text
    //    };
    //}

    //public void StartBet()
    //{
    //    if (CheckMoney(currentPriceValue) == false)
    //    {
    //        SoundManager.Instance.ButtonClick();
    //        OpenErrorScreen();
    //        return;
    //    }
    //    SoundManager.Instance.ThreeBetSound();
    //    BetAnim(player1, currentPriceValue, currentPriceIndex);
    //    DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 2);
    //    playerBetAmount += currentPriceValue;
    //}

    public void BetButtonClick()//this can be clicked when it is the current player's turn
    {
        if (!isGameStop)
        {
            // bonusUseValue
            // User Maintain
            ChangePlayerTurn(player1.playerNo);
        }
    }

    //#endregion

    //#region Menu Panel

    //void OpenMenuScreen()
    //{
    //    menuScreenObj.SetActive(true);
    //}

    //public void CloseMenuScreenButton()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    menuScreenObj.SetActive(false);
    //}

    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            Time.timeScale = 1;
            if (!isGameComplete && isGameStarted)
            {
                if (player1.isPack)//if player has dropped the match and then tries to leave
                {
                    InsertCardDataForSocket();
                    
                    FinishGameCallSocket(totalScore, "Dropped");
                }
                else//if player tries to leave from in between the match
                {
                    InsertCardDataForSocket();
                    ReturnCardsToDeck();
                    FinishGameCallSocket(80, "Left");
                }
            }
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else if (no == 2)
        {
            //OpenRuleScreen();
        }
    }

    private void OnApplicationQuit()
    {
        if (!isGameComplete)
        {
            if (player1.isPack)//if player has dropped the match and then tries to leave
            {
                InsertCardDataForSocket();
                FinishGameCallSocket(totalScore, "Dropped");
            }
            else//if player tries to leave from in between the match
            {
                InsertCardDataForSocket();
                ReturnCardsToDeck();
                FinishGameCallSocket(80, "Left");
            }
        }
        //TestSocketIO.Instace.LeaveRoom();
    }


    //#endregion

    //#region Rule Panel

    //void OpenRuleScreen()
    //{
    //    ruleScreenObj.SetActive(true);
    //}

    //public void CloseRuleButton()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    ruleScreenObj.SetActive(false);
    //}

    //#endregion

    //#region Bet Maintain

    public void BetAnim(PoolRummyPlayerManager player, float amount, int priceIndex)
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
        totalBetAmount += amount;
        betAmountTxt.text = totalBetAmount.ToString();

        //SpawnCoin(priceIndex);
    }

    //public void GetBotBetNo(int num, int botPlayerNo)
    //{
    //    if (isAdmin) return;
    //    switch (roundCounter)
    //    {
    //        case <= 1:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
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
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[botPlayerNo].isPack) return;
    //                        break;
    //                    }
    //            }

    //            break;
    //        case 3:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[botPlayerNo].isPack) return;
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[botPlayerNo].isPack) return;
    //                        break;
    //                    }
    //            }

    //            break;
    //        case >= 4:
    //            switch (num)
    //            {
    //                case 1:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 2:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 3:
    //                    {
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[botPlayerNo].isPack) return;
    //                        break;
    //                    }
    //                case 4:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //                case 5:
    //                    {
    //                        int index = playerSquList.FindIndex(playerSqu => playerSqu.playerNo == botPlayerNo);
    //                        //ChangeCardStatus("SEEN", botPlayerNo);
    //                        if (playerSquList[index].isPack) return;
    //                        BetAnim(playerSquList[index], currentPriceValue, currentPriceIndex);
    //                        SoundManager.Instance.ThreeBetSound();
    //                        break;
    //                    }
    //            }

    //            break;
    //    }

    //}


    //#endregion

    //#region SlideShowPanel
    //public void Accept_SlideShow(string sendId, string currentId)
    //{
    //    SlideShowSendSocket(sendId, currentId, "Accept");
    //    StartCoroutine(CheckSlideShowWinner(sendId, currentId, false));
    //}

    //public void Cancel_SlideShow(string sendId, string currentId)
    //{
    //    SlideShowSendSocket(sendId, currentId, "Cancel");
    //}

    //private void SpawnCoin(int priceIndex)
    //{
    //    //Instantiate(chipObj, boxCollider.transform);
    //    Vector3 dPos = GetRandomPosInBoxCollider2D();
    //    GameObject coin = Instantiate(chipObj, playerPosition[currentPlayer - 1]);
    //    coin.transform.GetComponent<Image>().sprite = chipsSprite[priceIndex];
    //    //coin.transform.position = new Vector3(targetBetObj.transform.position.x, targetBetObj.transform.position.y, 0f);
    //    ChipGenerate(coin, dPos);
    //    spawnedCoins.Add(coin);
    //    /*GameObject genBetObj = Instantiate(chipObj, playerPosition[currentPlayer - 1]);
    //    genBetObj.transform.GetComponent<Image>().sprite = chipsSprite[currentPriceIndex];
    //    genBetObj.transform.position = playerPosition[currentPlayer - 1].transform.position;
    //    genBetObj.transform.DOMove(targetBetObj.transform.position, 0.3f).OnComplete(() =>
    //    {
    //        spawnedCoins.Add(genBetObj);
    //    });*/
    //}

    //private Vector3 GetRandomPosInBoxCollider2D()
    //{
    //    Bounds bounds = boxCollider.bounds;
    //    float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
    //    float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
    //    return new Vector3(x, y, 90f);
    //}
    //public void ChipGenerate(GameObject chip, Vector3 endPos)
    //{
    //    chip.transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), 0.2f);
    //    chip.transform.DOMove(endPos, 0.2f).OnComplete(() =>
    //    {
    //        chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
    //        {
    //            chip.transform.DOScale(Vector3.one, 0.07f);
    //        });
    //    });
    //}


    //public void DeleteAllCoins()
    //{
    //    foreach (GameObject coin in spawnedCoins)
    //    {
    //        Destroy(coin);
    //    }
    //    spawnedCoins.Clear();
    //}

    //#endregion

    //#region Error Screen
    //public void OpenErrorScreen()
    //{
    //    errorScreenObj.SetActive(true);
    //}

    //public void Error_Ok_ButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    errorScreenObj.SetActive(false);
    //}

    //public void Error_Shop_ButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    Instantiate(shopPrefab, shopPrefabParent.transform);
    //    errorScreenObj.SetActive(false);
    //}

    //public bool CheckMoney(float money)
    //{

    //    float currentBalance = float.Parse(DataManager.Instance.playerData.balance);
    //    if ((currentBalance - money) < 0)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //    return false;
    //}


    //#endregion

    //#region Show Maintain

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
                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                break;
            case >= 3:
                showButton.interactable = true;
                showButton.transform.GetChild(0).GetComponent<Text>().text = "Side\nShow";
                break;
        }

        List<PoolRummyPlayerManager> availablePlayer = playerSquList.Where(t => t.gameObject.activeSelf && !t.isPack).ToList();
        // showing next player for side show
        var myIndex = availablePlayer.IndexOf(player1);
        var nextIndex = (myIndex + 1) % availablePlayer.Count;
        var nextPlayer = teenPattiPlayers[nextIndex];
        //slideShowPlayer = nextPlayer;

        if (availablePlayer.Count >= 3) return;
        showButton.interactable = true;
        showButton.transform.GetChild(0).GetComponent<Text>().text = "Show";
    }


    //#endregion

    //#region Game Restart Round Maintain

    //public void GameRestartRound()
    //{

    //}

    //#endregion

    //#region Socket Manager

    //public void GetChat(string playerID, string msg)
    //{
    //    if (playerID.Equals(DataManager.Instance.playerData._id))
    //    {
    //        TypeMessageBox typeMessageBox = Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
    //        typeMessageBox.Update_Message_Box(msg);
    //    }
    //    else
    //    {
    //        TypeMessageBox typeMessageBox = Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
    //        typeMessageBox.Update_Message_Box(msg);
    //    }
    //    Canvas.ForceUpdateCanvases();
    //}

    //public void GetGift(string sendPlayerID, string receivePlayerId, int giftNo)
    //{
    //    GameObject sendPlayerObj = null;
    //    GameObject receivePlayerObj = null;

    //    //print("Send Id : " + sendPlayerID);
    //    //print("Receive Id : " + receivePlayerId);
    //    for (int i = 0; i < teenPattiPlayers.Count; i++)
    //    {
    //        if (teenPattiPlayers[i].playerId == sendPlayerID)
    //        {
    //            sendPlayerObj = teenPattiPlayers[i].fillLine.gameObject;
    //        }
    //        else if (teenPattiPlayers[i].playerId == receivePlayerId)
    //        {
    //            receivePlayerObj = teenPattiPlayers[i].fillLine.gameObject;
    //        }
    //    }

    //    GameObject giftGen = Instantiate(giftPrefab, giftParentObj.transform);

    //    for (int i = 0; i < giftBoxes.Count; i++)
    //    {
    //        if (i == giftNo)
    //        {
    //            giftGen.transform.GetComponent<Image>().sprite = giftBoxes[i].giftSprite;
    //        }
    //    }
    //    giftGen.transform.position = sendPlayerObj.transform.position;
    //    giftGen.transform.DOMove(receivePlayerObj.transform.position, 0.4f).OnComplete(() =>
    //    {
    //        giftGen.transform.DOMove(receivePlayerObj.transform.position, 1f).OnComplete(() =>
    //        {

    //            giftGen.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
    //            {
    //                Destroy(giftGen);
    //            });

    //        });
    //    });



    //    //if (playerID.Equals(DataManager.Instance.playerData._id))
    //    //{
    //    //    TypeMessageBox typeMessageBox = Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
    //    //    typeMessageBox.Update_Message_Box(msg);
    //    //}
    //    //else
    //    //{
    //    //    TypeMessageBox typeMessageBox = Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
    //    //    typeMessageBox.Update_Message_Box(msg);
    //    //}
    //    //Canvas.ForceUpdateCanvases();
    //}


    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
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
        JSONObject[] newDeck = new JSONObject[78];//for 6 players

        for (int i = 0; i < newDeck.Length; i++)
        {

            if (i < distributedCardsList.Count)
            {
                newDeck[i] = new JSONObject();
                newDeck[i].AddField("Index", distributedCardsList[i]);
            }
        }


        int dealerNo = 1;

        //dealerNo = UnityEngine.Random.Range(1, DataManager.Instance.joinPlayerDatas.Count + 1);
        currentPlayer = dealerNo;
        playerCardsCount = 13;
        gameDealerNo = dealerNo;
        obj.AddField("UpdatedDeck", new JSONObject(newDeck));
        obj.AddField("Joker", wildJokerIndex);
        obj.AddField("DiscardedCard", openCardIndex);
        //dealerNo = UnityEngine.Random.Range(1, DataManager.Instance.joinPlayerDatas.Count + 1);
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("DeckNo2", dealerNo);
        //obj.AddField("DeckNo", 154);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 15);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(string playerId, List<int> newDeck, int joker, int discardIndex, int dealer)
    {
        //if (playerId != DataManager.Instance.playerData._id) return;
        //print("Deck no : " + deckNo);`
        // changing card sprite to default
        if (isAdmin) return;
        var copyList = new List<CardSuffle>(cardShuffles);
        closedDeck = copyList;
        playerCardsCount = 13;
        currentPlayer = dealer;
        gameDealerNo = dealer;
        wildJokerIndex = joker;
        WildJokerSetUp();
        distributedCardsList = newDeck;
        player.CardDistribute();
        openCardIndex = discardIndex;
        DiscardPileSetUp();
        if (waitNextRoundScreenObj.activeSelf)
        {
            waitNextRoundScreenObj.SetActive(false);
        }

        //MainMenuManager.Instance.CheckPlayers();

        StartGamePlay();
    }




    public void ChangePlayerTurn(int pNo)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("Action", "ChangePlayerTurn");
        TestSocketIO.Instace.Senddata("TeenPattiChangeTurnData", obj);
    }

    public void DrawCardFromClosedDeckSocket(int pNo, int index)//sending info to other players about the card drawn from the closed deck byy the current player
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("CardIndex", index);
        TestSocketIO.Instace.Senddata("CardDrawnFromClosedDeck", obj);
    }

    public void DrawCardFromDiscardPileSocket(int pNo)//sending info to other players about the card drawn from the discard pile by the current player
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        TestSocketIO.Instace.Senddata("CardDrawnFromDiscardPile", obj);
    }

    public void DiscardCardSocket(int pNo, int index)//sending info to other players about the card discarded by the current player
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("PlayerNo", pNo);
        obj.AddField("CardIndex", index);
        TestSocketIO.Instace.Senddata("CardDiscarded", obj);
    }

    public void ArrangeCardsCall()//sending info to all other players to start arranging cards for final show
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("PlayerNo", player1.playerNo);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        TestSocketIO.Instace.Senddata("RequestShow", obj);
    }

    public void ReturnCardsToDeck()//this is the event when player drops/leaves in between the game 
    {
        JSONObject obj = new JSONObject();

        JSONObject[] cards = new JSONObject[13];
        int _cardCounterIndex = 0;
        for (int j = 0; j < cardsHolder.childCount; j++)
        {

            for (int i = 0; i < cardsHolder.GetChild(j).childCount; i++)
            {
                //if(j != 0 && i == 0)
                //{
                //    cardIndex = cardsHolder.GetChild(j - 1).childCount;
                //}
                var card = cardsHolder.GetChild(j).GetChild(i).GetComponent<PoolCardScript>();
                int _indexNo = -1;
                for (int k = 0; k < cardShuffles.Count; k++)
                {
                    if (cardShuffles[k].cardNo == card.card.cardNo && cardShuffles[k].color == card.card.color && cardShuffles[k].cardSprite == card.card.cardSprite && cardShuffles[k].isWildJoker == card.card.isWildJoker)
                    {
                        _indexNo = k;
                        break;
                    }
                }
                cards[_cardCounterIndex] = new JSONObject();
                cards[_cardCounterIndex].AddField("cardIndex", _indexNo);
                _cardCounterIndex++;
            }
        }
        obj.AddField("ReturnCards", new JSONObject(cards));
        obj.AddField("PlayerNo", player1.playerNo);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        TestSocketIO.Instace.Senddata("ReturnCardsToDeck", obj);
    }


    public void InsertCardDataForSocket()//add card index in list to be sent in socket when game gets over for cards to display on win screen fro other players
    {
        for (int i = 0; i < cardsHolder.childCount; i++)
        {
            for (int j = 0; j < cardsHolder.GetChild(i).childCount; j++)
            {
                var card = cardsHolder.GetChild(i).GetChild(j).GetComponent<PoolCardScript>();
                for (int k = 0; k < cardShuffles.Count; k++)
                {
                    if (cardShuffles[k].cardNo == card.card.cardNo && cardShuffles[k].cardSprite == card.card.cardSprite && cardShuffles[k].color == card.card.color)
                    {
                        finalCards.Add(k);
                        break;
                    }
                }
            }
        }
    }
    public void FinishGameCallSocket(int score, string status)//sending info to other players that the current players has requested show
    {
        player1.playerGamePoints += score;
        player1.gameScoreText.text = player1.playerGamePoints.ToString();
        JSONObject obj = new JSONObject();
        JSONObject[] group = new JSONObject[7];
        JSONObject[] cards;
        int cardIndex = 0;
        for (int j = 0; j < cardsHolder.childCount; j++)
        {
            group[j] = new JSONObject();
            cards = new JSONObject[13];
            for (int i = 0; i < cardsHolder.GetChild(j).childCount; i++)
            {
                //if(j != 0 && i == 0)
                //{
                //    cardIndex = cardsHolder.GetChild(j - 1).childCount;
                //}
                cards[i] = new JSONObject();
                cards[i].AddField("cardIndex", finalCards[cardIndex]);//////////////////////
                cardIndex++;
            }
            //Array.Clear(cards, 0, cards.Length);
            //cardIndex++;
            group[j].AddField("Group", new JSONObject(cards));
        }

        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("PlayerNo", player1.playerNo);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", DataManager.Instance.gameId);
        obj.AddField("Points", score);
        obj.AddField("GamePoints", player1.playerGamePoints);
        obj.AddField("Status", status);
        if (isValidDeclaration)
            obj.AddField("ValidDeclaration", 0);//0 = true
        else
            obj.AddField("ValidDeclaration", 1);// 1 = false
        if (player1.isPack)
            obj.AddField("Dropped", 0);//0 = true
        else
            obj.AddField("Dropped", 1);//1 = false
        obj.AddField("Cards", new JSONObject(group));

        TestSocketIO.Instace.Senddata("SubmitShow", obj);


    }
    public int resultReceivedCount = 0;
    public void FinalResult()
    {
        if (totalScore > 0 && isValidDeclaration)
        {
            //winText.text = "You Lost ₹" + (totalScore * 0.1f) + " Better Luck Next Time.";
            //winText.gameObject.SetActive(true);
        }
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
            resultTransform[i].gameObject.SetActive(true);


        if (resultReceivedCount >= DataManager.Instance.joinPlayerDatas.Count)
        {
            resultDialog.SetActive(true);

            if (totalScore == 0)
            {
                //float adminPercentage = DataManager.Instance.adminPercentage;
                //winAmount = (0.1f * totalPoints);
                //winAmount = winAmount - (winAmount * adminPercentage);
                //winText.text = " Congratulations! You Won ₹" + winAmount;
                //winText.gameObject.SetActive(true);
                //for (int i = 0; i < resultNamesText.Length; i++)
                //{
                //    if (resultNamesText[i].text == DataManager.Instance.playerData.firstName)
                //    {
                //        resultAmountText[i].text = "₹" + winAmount;
                //    }
                //}

            }
            int dqPlayers = 0;//disqualified players Count
            for (int i = 0; i < playerSquList.Count; i++)
            {
                if (playerSquList[i] == player1)
                    continue;

                if (playerSquList[i].playerGamePoints > DataManager.Instance.pointLimit)
                    dqPlayers++;
            }
            if(dqPlayers == DataManager.Instance.joinPlayerDatas.Count - 1 && player1.playerGamePoints < DataManager.Instance.pointLimit)
            {
                float adminPercentage = DataManager.Instance.adminPercentage;
                winAmount = winAmount - ((winAmount * adminPercentage) / 100);
                DataManager.Instance.AddAmount(winAmount, DataManager.Instance.gameId, "PoolRummy-Win-" + DataManager.Instance.gameId, "won", adminPercentage, player1.playerNo);
                winText.text = " Congratulations! You Won ₹" + winAmount;
                winText.gameObject.SetActive(true);
                return;
            }
            else if(DataManager.Instance.joinPlayerDatas.Count == 1)//if everyone dropped
            {
                float adminPercentage = DataManager.Instance.adminPercentage;
                winAmount = winAmount - ((winAmount * adminPercentage) / 100);
                DataManager.Instance.AddAmount(winAmount, DataManager.Instance.gameId, "PoolRummy-Win-" + DataManager.Instance.gameId, "won", adminPercentage, player1.playerNo);
                winText.text = " Congratulations! You Won ₹" + winAmount;
                winText.gameObject.SetActive(true);
                return;
            }


            if(player1.playerGamePoints > DataManager.Instance.pointLimit)//standard DQ criteria
            {
                winText.gameObject.SetActive(true);
                winText.text = " You have been eliminated from this game. Better luck next time.";
                Invoke(nameof(CloseWinScreen), 8f);
                return;
            }
            //if (totalScore != 0)
            //{
            //    for (int i = 0; i < resultStatusText.Length; i++)
            //    {
            //        if (resultStatusText[i].text == "Won")
            //        {
            //            float adminPercentage = DataManager.Instance.adminPercentage;
            //            winAmount = (0.1f * totalPoints);
            //            winAmount = winAmount - (winAmount * adminPercentage);
            //            resultAmountText[i].text = "₹" + winAmount;
            //            break;
            //        }
            //    }
            //}
            //else if (isValidDeclaration == true && DataManager.Instance.joinPlayerDatas.Count == 2)
            //{
            //    float adminPercentage = DataManager.Instance.adminPercentage;
            //    winAmount = (0.1f * totalPoints) - (0.1f * totalScore);
            //    winAmount = winAmount - (winAmount * adminPercentage);
            //    winText.text = " Congratulations! You Won ₹" + winAmount;
            //    winText.gameObject.SetActive(true);
            //}
            //else if (totalScore > 0)
            //{
            //    winText.text = "You Lost ₹" + (totalScore * 0.1f) + " Better Luck Next Time.";
            //    winText.gameObject.SetActive(true);
            //    float amountLost = DataManager.Instance.pointValue * totalScore;
            //    DataManager.Instance.DebitAmount(amountLost.ToString(), DataManager.Instance.gameId, "PointRummy_Lost-" + DataManager.Instance.gameId, "game", 11);
            //}
            Invoke(nameof(StartNewRound), 10f);
        }
    }

    public void StartNewRound()
    {
        StartCoroutine(RestartGamePlay());
    }
    public void CloseWinScreen()
    {
        if(player1.playerGamePoints > DataManager.Instance.pointLimit)
        {
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else if(winText.gameObject.activeInHierarchy)
        {
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else
        {
            //pop up to start new game
            waitForNewRound.SetActive(true);
        }
    }

    public void PlayerDrawClosedCard(int index, int playerNo)//updating the closed deck in our game after socket player draws card from closed deck during their turn
    {
        GameObject newCard = Instantiate(cardPrefab, playerSquList[playerNo - 1].transform);
        newCard.transform.position = playerSquList[playerNo - 1].transform.position;
        var card = newCard.GetComponent<PoolCardScript>();
        card.card = closedDeck[index];
        card.card.color = closedDeck[index].color;
        card.card.cardNo = closedDeck[index].cardNo;
        //card.card.cardSprite = closedDeck[index].cardSprite;
        card.button.interactable = false;
        card.card.isWildJoker = closedDeck[index].isWildJoker;
        if (card.card.isWildJoker)
            card.wildJoker.SetActive(true);
        newCard.transform.position = deck.transform.position;
        print("card drawn by opponent = " + card.card.cardNo + " " + card.card.color);
        closedDeck.RemoveAt(index);
        if (closedDeck.Count == 1)
        {
            for (int i = discardedCardDeck.Count - 1; i > 0; i--)
            {
                closedDeck.Add(discardedCardDeck[i].card);
                discardedCardDeck.RemoveAt(i);
            }
        }
        newCard.transform.DOScale(Vector3.zero, 0.1f);
        newCard.transform.DOLocalMove(Vector3.zero, 0.1f).OnComplete(() =>
        {

        });
        //cardsHolder.GetChild(cardsHolder.childCount - 1).transform.DOBlendableLocalMoveBy(new Vector3(60f, 0, 0), 0.1f);
        //newCard.transform.SetAsFirstSibling();
    }

    public void PlayerDiscardCard(int index, int playerNo)//updating the discard pile in our game after socket player discards card during their turn
    {
        GameObject newCard = Instantiate(cardPrefab, playerSquList[playerNo - 1].transform);
        newCard.transform.position = playerSquList[playerNo - 1].transform.position;
        newCard.transform.SetParent(discardPile);
        newCard.transform.SetAsLastSibling();
        newCard.transform.DOLocalMove(Vector3.zero, 0.1f);
        newCard.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f);
        print("index = " + index + " count = " + cardShuffles.Count);
        var _removedCard = newCard.GetComponent<PoolCardScript>();
        _removedCard.card = cardShuffles[index];
        _removedCard.card.cardNo = cardShuffles[index].cardNo;
        _removedCard.card.color = cardShuffles[index].color;
        _removedCard.card.cardSprite = cardShuffles[index].cardSprite;
        _removedCard.card.isWildJoker = cardShuffles[index].isWildJoker;
        _removedCard.button.interactable = false;
        _removedCard.GetComponent<Image>().sprite = cardShuffles[index].cardSprite;
        if (_removedCard.card.isWildJoker)
            _removedCard.wildJoker.SetActive(true);

        discardedCardDeck.Insert(0, _removedCard);
        takeDiscardedCardButton.SetAsLastSibling();
    }

    public void PlayerDrawCardFromDiscardPile(int playerNo)//removing the card from discard pile in our game when another player draws card from discard pile
    {
        discardedCardDeck[0].transform.SetParent(playerSquList[playerNo - 1].transform);
        discardedCardDeck[0].transform.DOScale(Vector3.zero, 0.1f);
        discardedCardDeck[0].transform.DOLocalMove(Vector3.zero, 0.1f).OnComplete(() =>
        {
            discardedCardDeck.RemoveAt(0);
        });
        //discardedCardDeck[0].transform.SetAsFirstSibling();
        //discardedCardDeck[0].button.interactable = true;
    }

    public void PlayerGetCardsReturnedToDeck(List<int> indexNo)
    {
        foreach (int item in indexNo)
        {
            PoolCardScript addedCard = new PoolCardScript();
            addedCard.card = cardShuffles[item];
            addedCard.card.cardNo = cardShuffles[item].cardNo;
            addedCard.card.cardSprite = cardShuffles[item].cardSprite;
            addedCard.card.isWildJoker = cardShuffles[item].isWildJoker;
            discardedCardDeck.Add(addedCard);
        }
    }

    public void PlayerRequestFinishGame(int playerNo)//
    {
        finishText.text = DataManager.Instance.joinPlayerDatas[playerNo - 1].userName + " has requested show. Arrange your cards into groups & submit.";
        isValidDeclaration = true;
        finishButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(true);
        foreach (var item in playerSquList)
            item.NotATurn();
        if (player1.isPack == false)
            player1.fillLine.fillAmount = 1f;
        player1.isGameComplete = true;
        isGameComplete = true;
        if (player1.isPack == false)
            arrangeCardsDialog.SetActive(true);
        else
            resultDialog.SetActive(true);
    }


    //public void SlideShowSendSocket(string slideShowCancelPlayerID, string slideShowPlayerID, string type)
    //{

    //    JSONObject obj = new JSONObject();
    //    obj.AddField("PlayerID", DataManager.Instance.playerData._id);
    //    obj.AddField("TournamentID", DataManager.Instance.tournamentID);
    //    obj.AddField("RoomId", DataManager.Instance.gameId);
    //    obj.AddField("SlideShowCancelPlayerId", slideShowCancelPlayerID);
    //    obj.AddField("SlideShowPlayerId", slideShowPlayerID);
    //    obj.AddField("SlideShowType", type);
    //    obj.AddField("Action", "SideShowRequest");
    //    TestSocketIO.Instace.Senddata("TeenPattiSlideShowData", obj);
    //}


    //public void SendTeenPattiBet(int pNo, float amount, string betType, string playerSlideShowSend, string playerIdSlideShow)
    //{
    //    JSONObject obj = new JSONObject();
    //    obj.AddField("PlayerID", DataManager.Instance.playerData._id);
    //    obj.AddField("TournamentID", DataManager.Instance.tournamentID);
    //    obj.AddField("RoomId", DataManager.Instance.gameId);
    //    obj.AddField("PlayerNo", pNo);
    //    obj.AddField("BetAmount", amount);
    //    obj.AddField("BetType", betType);
    //    obj.AddField("playerSlideShowSendId", playerSlideShowSend);
    //    obj.AddField("playerIdSlideShowId", playerIdSlideShow);
    //    obj.AddField("Action", "PlaceBet");
    //    TestSocketIO.Instace.Senddata("TeenPattiSendBetData", obj);
    //}


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

        player1.isSeen = true;
        player1.isPack = true;

        TestSocketIO.Instace.Senddata("TeenPattiChangeCardStatus", obj);
    }

    bool isCheckTurnPack(int nextPlayerNo)
    {
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].playerNo == nextPlayerNo && playerSquList[i].isPack == true)
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
        //if (playerNo == DataManager.Instance.joinPlayerDatas.Count)//5
        if (playerNo == DataManager.Instance.joinPlayerDatas.Count)
        {
            nextPlayerNo = 1;
            roundCounter++;
        }
        //if (playerNo == 6)
        //{
        //    nextPlayerNo = 1;
        //    roundCounter++;
        //}
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
                nextPlayerNo = 6;
                if (isCheckTurnPack(nextPlayerNo) == false)
                {
                    nextPlayerNo = 6;
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
            }
        }
        else if (nextPlayerNo == 6)
        {
            if (isCheckTurnPack(nextPlayerNo) == false)
            {
                nextPlayerNo = 6;
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
        }
        //nextPlayerNo = playerNo;

        //if(player1.isPack == false)
        //{
        //    int dropCounter = 0;
        //    for (int i = 0; i < playerSquList.Count; i++)
        //    {
        //        if (playerSquList[i].isPack && playerSquList[i] != player1)
        //            dropCounter++;
        //    }
        //    if(dropCounter == playerSquList.Count - 1)
        //    {
        //        totalScore = 0;
        //        isTimerComplete = true;
        //        player1.isMyTimerComplete = true;
        //        SubmitButtonClick();
        //        //FinishGameCallSocket(0);
        //    }
        //}

        print("Next Player No : " + nextPlayerNo);
        currentPlayer = nextPlayerNo;
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].playerNo == nextPlayerNo)
            {
                playerSquList[i].RestartFillLine();
                if (playerSquList[i].playerNo == nextPlayerNo && playerSquList[i] == player1)
                {
                    //ShowTextChange();
                    dropButtonImage.sprite = enableDropButton;
                    isMyTurn = true;
                    //bottomBox.SetActive(true);
                    DataManager.Instance.UserTurnVibrate();
                    //EnableSeeCards();
                }
                else
                {
                    //isMyTurn = false;
                    //bottomBox.SetActive(false);
                }

            }
            else
            {
                playerSquList[i].NotATurn();
            }
        }
    }



    //public void CreditWinnerAmount(string playerID)
    //{
    //    float winnerAmount = (float)totalBetAmount;

    //    //print("Win No : " + winnerNo[i]);
    //    for (int j = 0; j < teenPattiPlayers.Count; j++)
    //    {
    //        if (teenPattiPlayers[j].playerId == playerID && teenPattiPlayers[j].gameObject.activeSelf == true)
    //        {

    //            //Generate Number
    //            GameObject genBetObj = Instantiate(betPrefab, prefabParent.transform);
    //            genBetObj.transform.GetChild(1).GetComponent<Text>().text = winnerAmount.ToString();
    //            genBetObj.transform.position = targetBetObj.transform.position;
    //            totalBetAmount = 0;
    //            //betAmountTxt.text = winnerAmount.ToString();
    //            genBetObj.transform.DOMove(teenPattiPlayers[j].sendBetObj.transform.position, 0.3f).OnComplete(() =>
    //            {
    //                //betAmountTxt.text = winnerAmount.ToString();
    //                /*if (teenPattiPlayers[j].playerNo == player1.playerNo)
    //                {
    //                    //Add to  winnner Amount

    //                    float adminPercentage = DataManager.Instance.adminPercentage;

    //                    float winAmount = winnerAmount;
    //                    float adminCommssion = (adminPercentage / 100);
    //                    float playerWinAmount = winAmount - (winAmount * adminCommssion);

    //                    print(playerWinAmount + "<-------- Crediting amount in animation");

    //                    if (playerWinAmount != 0)
    //                    {
    //                        SoundManager.Instance.CasinoWinSound();
    //                        DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "TeenPatti-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
    //                    }
    //                }*/
    //            });

    //            // Happening outside Dotween animation
    //            if (teenPattiPlayers[j].playerNo == player1.playerNo)
    //            {
    //                //Add to  winnner Amount

    //                float adminPercentage = DataManager.Instance.adminPercentage;

    //                float winAmount = winnerAmount;
    //                float adminCommssion = (adminPercentage / 100);
    //                float playerWinAmount = winAmount - (winAmount * adminCommssion);

    //                print(playerWinAmount + "<-------- Crediting amount Outside animation");

    //                if (playerWinAmount != 0)
    //                {
    //                    SoundManager.Instance.CasinoWinSound();
    //                    winAnimationTxt.gameObject.SetActive(true);
    //                    winAnimationTxt.text = "+" + playerWinAmount;
    //                    Invoke(nameof(WinAmountTextOff), 1.5f);
    //                    DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "TeenPatti-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommssion), player1.playerNo);
    //                }
    //            }

    //            Destroy(genBetObj, 0.4f);
    //        }
    //    }


    //    Invoke(nameof(GameRestartRound), 0.4f);
    //}

    //public void WinAmountTextOff()
    //{
    //    winAnimationTxt.gameObject.SetActive(false);

    //}

    public void GetBet(int playerNo, float amount, string type, string playerSlideShowSendId, string playerIdSlideShowId)
    {

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
            BetAnim(teenPattiPlayers[playerIndex], amount, currentPriceIndex);
        }
        if (teenPattiPlayers[playerIndex].isBlind)
        {
            isB = true;
        }
        else if (teenPattiPlayers[playerIndex].isSeen)
        {
            isS = true;
        }
        //currentPriceValue = amount;
        if (!player1.isPack && player1.isBlind)
        {
            if (isS)
            {
                //currentPriceValue /= 2;
                currentPriceValue = currentPriceValue;
            }
            else if (isB)
            {
                currentPriceValue = currentPriceValue;
            }
            priceBtnTxt.text = "Blind\n" + currentPriceValue;
        }
        else if (!player1.isPack && player1.isSeen)
        {
            if (isS)
            {
                currentPriceValue = currentPriceValue;
            }
            else if (isB)
            {
                currentPriceValue = currentPriceValue * 2;
            }
            priceBtnTxt.text = "Chaal\n" + currentPriceValue;
        }
    }


    //public void SlideShow_Accpet_Socket(string playerId1, string playerId2)
    //{
    //    if (DataManager.Instance.playerData._id.Equals(playerId1) || DataManager.Instance.playerData._id.Equals(playerId2))
    //    {
    //        StartCoroutine(CheckSlideShowWinner(playerId1, playerId2, true));
    //    }

    //}

    //public void SlideShow_Cancel_Socket()
    //{
    //    ChangePlayerTurn(player1.playerNo);
    //}

    public void GetCardStatus(string value, int playerNo)
    {
        //print("Card Status : " + value + "    Player No  :" + playerNo);
        for (int i = 0; i < playerSquList.Count; i++)
        {
            if (playerSquList[i].gameObject.activeSelf == true && playerSquList[i].playerNo == playerNo)
            {
                //if (value.Equals("SEEN"))
                //{
                //    playerSquList[i].isSeen = true;
                //    playerSquList[i].isBlind = false;
                //    playerSquList[i].isPack = false;
                //    ShowTextChange();
                //    //ShowTextChange(teenPattiPlayers[i]);

                //    playerSquList[i].seenImg.SetActive(true);
                //}
                if (value.Equals("PACK"))
                {
                    playerSquList[i].isPack = true;
                    playerSquList[i].isBlind = false;
                    playerSquList[i].isSeen = false;
                    //for (int j = 0; j < playerSquList[i].seeObj.Length; j++)
                    //{
                    //    playerSquList[i].seeObj[j].SetActive(false);
                    //}
                    //playerSquList[i].packImg.SetActive(true);
                    //CheckWin();
                    CheckPackTime(playerSquList[i]);
                    // Greejesh Pack Check
                }
            }
        }


    }


    void CheckPackTime(PoolRummyPlayerManager packPlayer)
    {
        print("Enter The Check Player");
        List<PoolRummyPlayerManager> livePlayers = new List<PoolRummyPlayerManager>();
        print(teenPattiPlayers.Count);
        for (int i = 0; i < teenPattiPlayers.Count; i++)
        {
            if (teenPattiPlayers[i].isPack == false && teenPattiPlayers[i].gameObject.activeSelf == true)
            {
                livePlayers.Add(teenPattiPlayers[i]);
            }
        }
        print("livePlayers.Count : " + livePlayers.Count);
        packPlayer.isTurn = false;
        if (livePlayers.Count == 1)
        {
            isGameComplete = true;
            livePlayers[0].isTurn = false;
            string winValue = ",";
            winValue += livePlayers[0].playerNo + ",";
            if (livePlayers[0].playerNo == playerNo)
            {
                //SetTeenPattiWon(livePlayers[0].playerId);
                //float adminPercentage = DataManager.Instance.adminPercentage;
                //winAmount = 0.1f * totalScore;
                //winAmount = winAmount - (winAmount * adminPercentage);
                //winText.text = " Congratulations! You Won ₹" + winAmount;
                //winText.gameObject.SetActive(true);
                totalScore = 0;
                isTimerComplete = true;
                player1.isMyTimerComplete = true;
                resultDialog.SetActive(true);
                player1.isMyTimerComplete = true;
                //for (int i = 0; i < player.cards.Count; i++)
                //{
                //    for (int k = 0; k < cardShuffles.Count; k++)
                //    {
                //        if (player.cards[i].card.cardNo == cardShuffles[k].cardNo && player.cards[i].card.color == cardShuffles[k].color)
                //        {
                //            finalCards.Add(k);
                //            break;
                //        }
                //    }
                //}
                InsertCardDataForSocket();
                isValidDeclaration = true;
                FinishGameCallSocket(totalScore, "Complete");
                //SubmitButtonClick();

                Debug.LogWarning("------------------won is called-------------------------------------");
            }
            else
            {
                resultDialog.SetActive(true);
                player1.isMyTimerComplete = true;
                //for (int i = 0; i < player.cards.Count; i++)
                //{
                //    for (int k = 0; k < cardShuffles.Count; k++)
                //    {
                //        if (player.cards[i].card.cardNo == cardShuffles[k].cardNo && player.cards[i].card.color == cardShuffles[k].color)
                //        {
                //            finalCards.Add(k);
                //            break;
                //        }
                //    }
                //}
                InsertCardDataForSocket();
                //isValidDeclaration = true;
                FinishGameCallSocket(totalScore, "Complete");
            }

            //foreach (var t in livePlayers[0].playerWinObj)
            //{
            //    t.SetActive(true);
            //}

            //StartCoroutine(RestartGamePlay());
        }
        else
        {
            if (isAdmin && packPlayer.isTurn)
            {
                ChangePlayerTurn(packPlayer.playerNo);
            }
        }
    }


    public void ChangeAAdmin(string leavePlayerId, string adminId, int playerNo)
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
                teenPattiPlayers[i].isPack = true;
                teenPattiPlayers[i].isBlind = false;
                teenPattiPlayers[i].isSeen = false;
                //for (int j = 0; j < teenPattiPlayers[i].seeObj.Length; j++)
                //{
                //    teenPattiPlayers[i].seeObj[j].SetActive(false);
                //}
                //teenPattiPlayers[i].packImg.SetActive(true);

                //teenPattiPlayers[i].gameObject.SetActive(false);
                //DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[i]);
                StartCoroutine(WaitGameToCompleteRemovePlayer(CheckLeftPlayer, i));
                if (teenPattiPlayers[i].isTurn)
                {
                    CheckPackTime(teenPattiPlayers[i]);
                    ChangePlayerTurn(teenPattiPlayers[i].playerNo);
                }
                else
                    CheckPackTime(teenPattiPlayers[i]);
                teenPattiPlayers[i].gameObject.SetActive(false);
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
        //switch (playerNo)
        //{
        //    case 1:
        //        for (int i = 0; i < 13; i++)
        //            shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
        //        break;
        //    case 2:
        //        for (int i = 13; i < 26; i++)
        //            shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
        //        break;

        //    case 3:
        //        for (int i = 26; i < 39; i++)
        //            shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
        //        break;
        //    case 4:
        //        for (int i = 39; i < 52; i++)
        //            shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
        //        break;
        //    case 5:
        //        for (int i = 52; i < 65; i++)
        //            shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
        //        break;
        //    case 6:
        //        for (int i = 64; i < 78; i++)
        //            shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
        //        break;
        //}



        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (/*DataManager.Instance.joinPlayerDatas.Count == 5 &&*/ waitNextRoundScreenObj.activeSelf)
            {
                DataManager.Instance.joinPlayerDatas.RemoveAt(0);
                //RoundGenerate();
                CheckJoinedPlayers();
                //StartGamePlay();
                if (waitNextRoundScreenObj.activeSelf)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
                //ResetBot();
                //ActivateBotPlayers();
            }
        }
        else
        {
            if (!isGameStarted)
            {
                isAdmin = false;
            }

            if (DataManager.Instance.joinPlayerDatas.Count < 6) return;
            int index = DataManager.Instance.joinPlayerDatas.FindIndex(leftPlayer => leftPlayer.userId == leavePlayerId);
            DataManager.Instance.joinPlayerDatas.Remove(DataManager.Instance.joinPlayerDatas[index]);
        }
    }

    public void CheckJoinedPlayers()
    {
        DataManager.Instance.joinPlayerDatas = DataManager.Instance.joinPlayerDatas.Where(player => !player.avtar.StartsWith("http://139.84.132.115/assets/img/profile-picture/")).ToList();
        // assiging new remaining bot players
        if (DataManager.Instance.joinPlayerDatas.Count <= 4)
        {
            //MainMenuManager.Instance.CheckPlayers();
        }
    }
    
    
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


    //#endregion

    //#region CheckWin

    //public IEnumerator CheckSlideShowWinner(string playerId1, string playerId2, bool isSocket)
    //{
    //    int cnt = 0;

    //    //List<TeenPattiPlayer> teenSlideShowPlayers = new List<TeenPattiPlayer>();
    //    for (int i = 0; i < teenPattiPlayers.Count; i++)
    //    {
    //        if (isSocket && teenPattiPlayers[i].gameObject.activeSelf == true && teenPattiPlayers[i].isPack == false && (teenPattiPlayers[i].playerId.Equals(playerId1) || teenPattiPlayers[i].playerId.Equals(playerId2)))
    //        {
    //            cnt++;
    //            teenPattiPlayers[i].CardDisplay();
    //            //teenSlideShowPlayers.Add(teenPattiPlayers[i]);
    //        }
    //    }
    //    if (CheckMoney(currentPriceValue) == false)
    //    {
    //        SoundManager.Instance.ButtonClick();
    //        OpenErrorScreen();
    //        yield break;
    //    }
    //    BetAnim(player1, currentPriceValue, currentPriceIndex);
    //    DataManager.Instance.DebitAmount((currentPriceValue).ToString(), DataManager.Instance.gameId, "TeenPatti-Bet-" + DataManager.Instance.gameId, "game", 1);
    //    playerBetAmount += currentPriceValue;
    //    yield return new WaitForSeconds(0.75f);

    //    /*if (teenSlideShowPlayers.Count == 2)
    //    {
    //        TeenPattiPlayer slidePlayer1 = teenPattiPlayers[0];
    //        TeenPattiPlayer slidePlayer2 = teenPattiPlayers[1];

    //        if (slidePlayer1.ruleNo < slidePlayer2.ruleNo)
    //        {
    //            if (isSocket)
    //            {
    //                ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                ChangePlayerTurn(slidePlayer2.playerNo);
    //            }
    //            //pack slideplayer2
    //        }
    //        else if (slidePlayer2.ruleNo < slidePlayer1.ruleNo)
    //        {
    //            //pack slideplayer1
    //            if (isSocket)
    //            {
    //                ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                ChangePlayerTurn(slidePlayer1.playerNo);
    //            }
    //        }
    //        else if (slidePlayer2.ruleNo == slidePlayer1.ruleNo)
    //        {
    //            if (slidePlayer1.ruleNo == 1)
    //            {
    //                if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
    //                {
    //                    //pack slide player 2
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                        ChangePlayerTurn(slidePlayer2.playerNo);
    //                    }
    //                }
    //                else if (slidePlayer2.card1.cardNo > slidePlayer1.card1.cardNo)
    //                {
    //                    //pack slide player 1
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                        ChangePlayerTurn(slidePlayer1.playerNo);
    //                    }
    //                }
    //                else if (slidePlayer1.card1.cardNo == slidePlayer2.card1.cardNo)
    //                {
    //                    //pack slide player 1
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                        ChangePlayerTurn(slidePlayer2.playerNo);
    //                    }
    //                }
    //            }
    //            else if (slidePlayer1.ruleNo == 5)
    //            {
    //                if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
    //                {
    //                    //pack slide player 2
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                        ChangePlayerTurn(slidePlayer2.playerNo);
    //                    }
    //                }
    //                else if (slidePlayer2.card1.cardNo > slidePlayer1.card1.cardNo)
    //                {
    //                    //pack slide player 1
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                        ChangePlayerTurn(slidePlayer1.playerNo);
    //                    }
    //                }
    //                else if (slidePlayer1.card1.cardNo == slidePlayer2.card1.cardNo)
    //                {

    //                    if (slidePlayer1.card3.cardNo > slidePlayer2.card3.cardNo)
    //                    {
    //                        //pack slide player 2
    //                        if (isSocket)
    //                        {
    //                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                            ChangePlayerTurn(slidePlayer2.playerNo);
    //                        }
    //                    }
    //                    else if (slidePlayer2.card3.cardNo > slidePlayer1.card3.cardNo)
    //                    {
    //                        //pack slide player 1
    //                        if (isSocket)
    //                        {
    //                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                            ChangePlayerTurn(slidePlayer1.playerNo);
    //                        }
    //                    }
    //                    else if (slidePlayer1.card3.cardNo == slidePlayer2.card3.cardNo)
    //                    {
    //                        //pack slide player 1
    //                        if (isSocket)
    //                        {
    //                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                            ChangePlayerTurn(slidePlayer1.playerNo);
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                int highestNo1 = 0;
    //                if (slidePlayer1.card1.cardNo > slidePlayer2.card1.cardNo)
    //                {
    //                    //pack slide player 2
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                        ChangePlayerTurn(slidePlayer2.playerNo);
    //                    }
    //                }
    //                else if (slidePlayer1.card1.cardNo < slidePlayer2.card1.cardNo)
    //                {
    //                    //pack slide player 1
    //                    if (isSocket)
    //                    {
    //                        ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                        ChangePlayerTurn(slidePlayer1.playerNo);
    //                    }
    //                }
    //                else
    //                {
    //                    if (slidePlayer1.card2.cardNo > slidePlayer2.card2.cardNo)
    //                    {
    //                        //pack slide player 2
    //                        if (isSocket)
    //                        {
    //                            ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                            ChangePlayerTurn(slidePlayer2.playerNo);
    //                        }
    //                    }
    //                    else if (slidePlayer1.card2.cardNo < slidePlayer2.card2.cardNo)
    //                    {
    //                        //pack slide player 1
    //                        if (isSocket)
    //                        {
    //                            ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                            ChangePlayerTurn(slidePlayer1.playerNo);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (slidePlayer1.card3.cardNo > slidePlayer2.card3.cardNo)
    //                        {
    //                            //pack slide player 2
    //                            if (isSocket)
    //                            {
    //                                ChangeCardStatus("PACK", slidePlayer2.playerNo);
    //                                ChangePlayerTurn(slidePlayer2.playerNo);
    //                            }
    //                        }
    //                        else if (slidePlayer1.card3.cardNo < slidePlayer2.card3.cardNo)
    //                        {
    //                            //pack slide player 1
    //                            if (isSocket)
    //                            {
    //                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                                ChangePlayerTurn(slidePlayer1.playerNo);
    //                            }
    //                        }
    //                        else if (slidePlayer1.card3.cardNo == slidePlayer2.card3.cardNo)
    //                        {
    //                            //pack slide player 1
    //                            if (isSocket)
    //                            {
    //                                ChangeCardStatus("PACK", slidePlayer1.playerNo);
    //                                ChangePlayerTurn(slidePlayer1.playerNo);
    //                            }
    //                        }
    //                    }
    //                }

    //            }

    //        }
    //    }*/

    //}


    //public void ShowCardToAllUser()
    //{
    //    winMaintain.Clear();
    //    foreach (var t in teenPattiPlayers.Where(t => t.gameObject.activeSelf == true && (t.isSeen || t.isBlind) && t.isPack == false))
    //    {
    //        t.CardDisplay();
    //    }
    //    //CheckFinalWinner(type);
    //    bottomBox.SetActive(false);
    //}

    //public string CheckFinalWinner(string type)
    //{
    //    List<TeenPattiPlayer> teenPattiWinner = new List<TeenPattiPlayer>();

    //    foreach (var t in teenPattiPlayers.Where(t => t.gameObject.activeSelf == true && (t.isSeen || t.isBlind) && t.isPack == false))
    //    {
    //        foreach (var t1 in t.seeObj)
    //        {
    //            t1.SetActive(false);
    //        }
    //    }

    //    /*List<TeenPattiPlayer> teenPattiWinner = teenPattiPlayers.Where(p => p.gameObject.activeSelf && (p.isSeen || p.isBlind) && !p.isPack).OrderByDescending(p => p.ruleNo).ThenByDescending(p => p.isBot && p.ruleNo == 6).ToList();

    //    if (teenPattiWinner.Count > 0)
    //    {
    //        TeenPattiPlayer winner = teenPattiWinner[0];
    //        teenPattiWinner.Clear();
    //        teenPattiWinner.Add(winner);
    //    }
    //    else
    //    {
    //        teenPattiWinner.Clear();
    //    }*/

    //    foreach (var player in teenPattiPlayers)
    //    {
    //        player.SumOfPlayerCards();
    //    }

    //    List<TeenPattiPlayer> sortedNumbersDescending = teenPattiPlayers.OrderByDescending(n => n.sumOfCards).ToList();

    //    for (int i = 0; i < sortedNumbersDescending.Count; i++)
    //    {
    //        //calculate sum of 3 card value and store in a varible
    //        if (sortedNumbersDescending[i].gameObject.activeSelf == true && (sortedNumbersDescending[i].isSeen || sortedNumbersDescending[i].isBlind) && sortedNumbersDescending[i].isPack == false)
    //        {
    //            bool isEnter = false;
    //            for (int j = 0; j < teenPattiWinner.Count; j++)
    //            {
    //                if (teenPattiWinner[j].ruleNo > sortedNumbersDescending[i].ruleNo)
    //                {
    //                    isEnter = true;
    //                }
    //            }
    //            // Highest rule number player will be added to teenpattiwinner list
    //            if (isEnter == true)
    //            {
    //                teenPattiWinner.Clear();

    //                teenPattiWinner.Add(sortedNumbersDescending[i]);
    //                //print("Clear");
    //            }
    //            else if (teenPattiWinner.Count == 0)
    //            {
    //                teenPattiWinner.Add(sortedNumbersDescending[i]);

    //            }//print("Add");
    //        }
    //    }


    //    /*for (int i = 0; i < teenPattiPlayers.Count; i++)
    //    {
    //        if (teenPattiPlayers[i].gameObject.activeSelf == true && (teenPattiPlayers[i].isSeen || teenPattiPlayers[i].isBlind) && teenPattiPlayers[i].isPack == false)
    //        {
    //            bool isEnter = false;
    //            for (int j = 0; j < teenPattiWinner.Count; j++)
    //            {
    //                if (teenPattiWinner[j].ruleNo > teenPattiPlayers[i].ruleNo)
    //                {
    //                    isEnter = true;
    //                }
    //            }
    //            if (isEnter == true)
    //            {
    //                teenPattiWinner.Clear();

    //                teenPattiWinner.Add(teenPattiPlayers[i]);
    //                //print("Clear");
    //            }
    //            else if (teenPattiWinner.Count == 0)
    //            {
    //                teenPattiWinner.Add(teenPattiPlayers[i]);

    //            }//print("Add");
    //        }
    //    }*/
    //    ShowWinPlayer(type, teenPattiWinner);

    //    CreditWinnerAmount(teenPattiWinner[0].playerId);
    //    SetTeenPattiWon(teenPattiWinner[0].playerId);
    //    return teenPattiWinner[0].playerId;
    //}

    //public void HandelTeenPattiWinData(string winnerPlayerId)
    //{
    //    List<TeenPattiPlayer> winnerPlayer = teenPattiPlayers.Where(p => p.playerId == winnerPlayerId).ToList();

    //    if (winnerPlayer.Count > 0)
    //    {
    //        ShowCardToAllUser();
    //        ShowWinPlayer("Show", winnerPlayer);
    //    }
    //}

    //public void ShowWinPlayer(string type, List<TeenPattiPlayer> teenPattiWinner)
    //{
    //    isBotActivate = false;
    //    if (teenPattiWinner.Count == 1)
    //    {
    //        int rule = teenPattiWinner[0].ruleNo;
    //        string winValue = ",";
    //        winValue += teenPattiWinner[0].playerNo + ",";
    //        if (teenPattiWinner[0].playerNo == playerNo)
    //        {
    //            if (teenPattiWinner[0].playerNo == playerNo)
    //            {
    //                //SetTeenPattiWon(winValue);
    //                //Debug.LogWarning("------------------won is called-------------------------------------");
    //            }
    //        }

    //        //print("Rule 1 : " + rule);
    //        //win
    //        foreach (var t in teenPattiWinner[0].playerWinObj)
    //        {
    //            t.SetActive(true);
    //        }
    //        SoundManager.Instance.CasinoWinSound();

    //        StartCoroutine(RestartGamePlay());
    //    }
    //    else if (teenPattiWinner.Count > 1)
    //    {
    //        /*int rule = teenPattiWinner[0].ruleNo;

    //        //print("Rule 2 : " + rule);
    //        switch (rule)
    //        {
    //            case 1:
    //            {
    //                int highestNo1 = teenPattiWinner[0].card1.cardNo;
    //                highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

    //                List<TeenPattiPlayer> playerList1 =
    //                    teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

    //                if (playerList1.Count == 1)
    //                {
    //                    //win
    //                    string winValue = ",";
    //                    winValue += playerList1[0].playerNo + ",";
    //                    if (playerList1[0].playerNo == playerNo)
    //                    {
    //                        if (playerList1[0].playerNo == playerNo)
    //                        {
    //                            SetTeenPattiWon(winValue); // with player id // Moved in click
    //                            Debug.LogWarning(
    //                                "------------------won is called-------------------------------------");
    //                        }
    //                    }

    //                    foreach (var t in playerList1[0].playerWinObj)
    //                    {
    //                        t.SetActive(true);
    //                    }

    //                    StartCoroutine(RestartGamePlay());
    //                }

    //                break;
    //            }
    //            case 5:
    //            {
    //                int highestNo1 = teenPattiWinner[0].card1.cardNo;
    //                highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

    //                List<TeenPattiPlayer> playerList1 =
    //                    teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

    //                if (playerList1.Count == 1)
    //                {
    //                    //win
    //                    string winValue = ",";
    //                    winValue += playerList1[0].playerNo + ",";
    //                    if (playerList1[0].playerNo == playerNo)
    //                    {
    //                        SetTeenPattiWon(winValue);
    //                        Debug.LogWarning("------------------won is called-------------------------------------");
    //                    }

    //                    foreach (var t in playerList1[0].playerWinObj)
    //                    {
    //                        t.SetActive(true);
    //                    }

    //                    StartCoroutine(RestartGamePlay());
    //                }
    //                else
    //                {
    //                    int highestNo3 = teenPattiWinner[0].card3.cardNo;
    //                    highestNo3 = teenPattiWinner.Select(t => t.card3.cardNo).Prepend(highestNo3).Max();

    //                    List<TeenPattiPlayer> playerList3 =
    //                        teenPattiWinner.Where(t => highestNo3 == t.card3.cardNo).ToList();

    //                    if (playerList3.Count == 1)
    //                    {
    //                        //win
    //                        string winValue = ",";
    //                        winValue += playerList3[0].playerNo + ",";
    //                        if (playerList3[0].playerNo == playerNo)
    //                        {
    //                            SetTeenPattiWon(winValue);
    //                            Debug.LogWarning(
    //                                "------------------won is called-------------------------------------");
    //                        }

    //                        foreach (var t in playerList3[0].playerWinObj)
    //                        {
    //                            t.SetActive(true);
    //                        }

    //                        StartCoroutine(RestartGamePlay());
    //                    }
    //                    else
    //                    {
    //                        //win
    //                        if (type == "Show")
    //                        {
    //                            ChangeCardStatus("PACK", player1.playerNo);
    //                            //ChangePlayerTurn(player1.playerNo);
    //                        }
    //                        else
    //                        {
    //                            string winValue = ",";
    //                            winValue += playerList1[0].playerNo + ",";
    //                            foreach (var t in playerList3)
    //                            {
    //                                winValue += t.playerNo + ",";
    //                                foreach (var t1 in t.playerWinObj)
    //                                {
    //                                    t1.SetActive(true);
    //                                }
    //                            }

    //                            StartCoroutine(RestartGamePlay());
    //                            if (playerList3[0].playerNo == playerNo)
    //                            {
    //                                SetTeenPattiWon(winValue);
    //                                Debug.LogWarning(
    //                                    "------------------won is called-------------------------------------");
    //                            }
    //                        }
    //                    }
    //                }

    //                break;
    //            }
    //            default:
    //            {
    //                int highestNo1 = teenPattiWinner[0].card1.cardNo;
    //                highestNo1 = teenPattiWinner.Select(t => t.card1.cardNo).Prepend(highestNo1).Max();

    //                List<TeenPattiPlayer> playerList1 =
    //                    teenPattiWinner.Where(t => highestNo1 == t.card1.cardNo).ToList();

    //                if (playerList1.Count == 1)
    //                {
    //                    //win
    //                    string winValue = ",";
    //                    winValue += playerList1[0].playerNo + ",";
    //                    if (playerList1[0].playerNo == playerNo)
    //                    {
    //                        SetTeenPattiWon(winValue);
    //                        Debug.LogWarning("------------------won is called-------------------------------------");
    //                    }

    //                    foreach (var t in playerList1[0].playerWinObj)
    //                    {
    //                        t.SetActive(true);
    //                    }

    //                    StartCoroutine(RestartGamePlay());
    //                }
    //                else
    //                {
    //                    int highestNo2 = teenPattiWinner[0].card2.cardNo;
    //                    highestNo2 = teenPattiWinner.Select(t => t.card2.cardNo).Prepend(highestNo2).Max();

    //                    List<TeenPattiPlayer> playerList2 =
    //                        teenPattiWinner.Where(t => highestNo2 == t.card2.cardNo).ToList();

    //                    if (playerList2.Count == 1)
    //                    {
    //                        //win
    //                        string winValue = ",";
    //                        winValue += playerList2[0].playerNo + ",";
    //                        if (playerList2[0].playerNo == playerNo)
    //                        {
    //                            SetTeenPattiWon(winValue);
    //                            Debug.LogWarning(
    //                                "------------------won is called-------------------------------------");
    //                        }

    //                        foreach (var t in playerList2[0].playerWinObj)
    //                        {
    //                            t.SetActive(true);
    //                        }

    //                        StartCoroutine(RestartGamePlay());
    //                    }
    //                    else
    //                    {
    //                        int highestNo3 = teenPattiWinner[0].card3.cardNo;
    //                        highestNo3 = teenPattiWinner.Select(t => t.card3.cardNo).Prepend(highestNo3).Max();

    //                        List<TeenPattiPlayer> playerList3 =
    //                            teenPattiWinner.Where(t => highestNo3 == t.card3.cardNo).ToList();

    //                        if (playerList3.Count == 1)
    //                        {
    //                            //win
    //                            string winValue = ",";
    //                            winValue += playerList3[0].playerNo + ",";
    //                            if (playerList3[0].playerNo == playerNo)
    //                            {
    //                                SetTeenPattiWon(winValue);
    //                                Debug.LogWarning(
    //                                    "------------------won is called-------------------------------------");
    //                            }

    //                            foreach (var t in playerList3[0].playerWinObj)
    //                            {
    //                                t.SetActive(true);
    //                            }

    //                            StartCoroutine(RestartGamePlay());
    //                        }
    //                        else
    //                        {
    //                            //win
    //                            if (type == "Show")
    //                            {
    //                                ChangeCardStatus("PACK", player1.playerNo);
    //                                //ChangePlayerTurn(player1.playerNo);
    //                            }
    //                            else
    //                            {
    //                                string winValue = ",";
    //                                foreach (var t in playerList3)
    //                                {
    //                                    winValue += t.playerNo + ",";
    //                                    foreach (var t1 in t.playerWinObj)
    //                                    {
    //                                        t1.SetActive(true);
    //                                    }
    //                                }

    //                                StartCoroutine(RestartGamePlay());
    //                                if (playerList3[0].playerNo == playerNo)
    //                                {
    //                                    SetTeenPattiWon(winValue);
    //                                    Debug.LogWarning(
    //                                        "------------------won is called-------------------------------------");
    //                                }
    //                            }
    //                        }
    //                    }
    //                }

    //                break;
    //            }
    //        }*/
    //    }
    //}
    //#endregion

}
