using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class JhandiMundaManager : MonoBehaviour
{
    public static JhandiMundaManager Instance;

    public GameObject dealer;

    public Image playerAvatar;
    public Text playerName;

    public Text playerBalanceText;
    public GameObject winParticles;
    public Transform winParticleParent;
    public Text winAnimationTxt;
    public int DownRatio = 2;
    public int UpRatio = 2;
    public int OnRatio = 5;


    public List<GameObject> Chips;

    [Header("--- Error Screen ---")]
    public GameObject errorScreenObject;


    public int downBetValue;
    public int upBetValue;
    public int onBetValue;
    
    public int[] betValueOnSymbols;//this array will store the value player has betted on from each of the 6 symbols individually
    public Text[] betText;
    public Text[] totalBetText;
    public int totalBet;


    public List<GameObject> downChips;
    public List<GameObject> upChips;
    public List<GameObject> onChips;
    //these list will contain chips for each individual symbol on the table
    public List<GameObject> firstSymbolChips;
    public List<GameObject> secondSymbolChips;
    public List<GameObject> thirdSymbolChips;
    public List<GameObject> fourthSymbolChips;
    public List<GameObject> fifthSymbolChips;
    public List<GameObject> sixthSymbolChips;


    [Header("--- Players Controller ---")]
    public List<JhandiMundaPlayerManager> playerList = new List<JhandiMundaPlayerManager>();


    [Header("--- Dice Controller ---")]
    public Sprite[] diceNumbers;//contains sprites of 6 symbols in jhandi munda
    public GameObject diceRollObject;
    public GameObject diceContainerObject;
    public GameObject dice1;
    public GameObject dice2;
    public GameObject[] dice;//dices which will be seen rolling to the player
    public Vector3[] initialDicePositions;
    public int dice1Result;
    public int dice2Result;
    public int[] diceResults = new int[6];


    //these contain positions of dice at the symbol on the table.
    //For e.g. if 3 dices came for symbol 1 then 3 dices will move over to firstSymbol
    [Header("--- Transform positions of dice for individual symbol came in the result ---")]
    public Transform[] firstSymbolResult;
    public Transform[] secondSymbolResult;
    public Transform[] thirdSymbolResult;
    public Transform[] fourthSymbolResult;
    public Transform[] fifthSymbolResult;
    public Transform[] sixthSymbolResult;

    //these list contain the result dices which came after rolling the dices.
    //Each dice will be added to the list corresponding to the dice result.
    // If the list count is 0, then no result came for that particular symbol
    public List<GameObject> firstSymbolDices;
    public List<GameObject> secondSymbolDices;
    public List<GameObject> thirdSymbolDices;
    public List<GameObject> fourthSymbolDices;
    public List<GameObject> fifthSymbolDices;
    public List<GameObject> sixthSymbolDices;

    public List<int> symbolDices;

    [Header("--- Start Stop Bet ---")]
    public GameObject startBetObj;
    public GameObject stopBetObj;


    public GameObject playerProfile;
    public Transform otherProfile;
    public GameObject chipPrefab;
    public Sprite[] chipSprite;//0 = 10//1 = 50//2 = 100//3 = 500// 4 = 1000
    public float[] chipValue;
    public Transform downArea;
    public Transform upArea;
    public Transform onArea;
    public Transform[] symbolArea;//Areas where chips will be placed

    public int selectedChipNo;//0 = 10//1 = 50//2 = 100//3 = 500// 4 = 1000
    public float fixTimerValue;
    public float timerValue;
    public float secondsCount;
    public Text timerText;
    bool isEnterBetStop;
    private bool _isClickAvailable;



    [Header("--- Chip Generate Position ---")]
    private float min7Downx;//50 for down // -550 for up
    private float max7Downx;//600 for down // -100 for up
    private float min7Downy;
    private float max7Downy;
    
    public float minFirstSymbolX = 0f;//0
    public float maxFirstSymbolX = 725f;//725
    public float minFirstSymbolY = -45;//-45
    public float maxFirstSymbolY = 113;//113
    
    public float minSecondSymbolX = -420f;//-420
    public float maxSecondSymbolX = 420f;//420
    public float minSecondSymbolY = -75f;//-75
    public float maxSecondSymbolY = 113f;//113
    
    public float minThirdSymbolX = -725f;//-725
    public float maxThirdSymbolX = -45f;//-45
    public float minThirdSymbolY = -75f;//-75
    public float maxThirdSymbolY = 113f;//113
    
    public float minFourthSymbolX = 0f;//0
    public float maxFourthSymbolX = 725f;//725
    public float minFourthSymbolY = -150f;//-150
    public float maxFourthSymbolY = 150f;//150
    
    public float minFifthSymbolX = -420f;//-420
    public float maxFifthSymbolX = 420f;//420
    public float minFifthSymbolY = -200f;//-200
    public float maxFifthSymbolY = 115f;//115
    
    public float minSixthSymbolX = -725f;//-725
    public float maxSixthSymbolX = -45f;//-45
    public float minSixthSymbolY = -150f;//-150
    public float maxSixthSymbolY = 113f;//113


    public float min7Onx;//50 for down // -550 for up
    public float max7Onx;//600 for down // -100 for up
    public float min7Ony;
    public float max7Ony;

    public float min7Upx;//50 for down // -550 for up
    public float max7Upx;//600 for down // -100 for up
    public float min7Upy;
    public float max7Upy;

    public float upValue;//value by which the chip button would go up when selected
    public float downValue;//value by which the chip button would go down when deselected

    public GameObject waitNextRoundScreenObject;

    [Header("--- Win Object ---")]
    public GameObject down7Win;
    public GameObject up7Win;
    public GameObject on7Win;
    public GameObject[] winObjects;

    [Header("--- HistoryTracking---")]
    public Transform[] historyHolder;
    public GameObject resultPrefab;
    public List<int> winList;
    public List<int> diceResultList;
    public Transform[] resultArea;

    public int maxWinListCount = 10;
    public List<GameObject> chipsHistory = new List<GameObject>();

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

    private string playerPrefsKey = "SevenUpDownHistory";

    // Start is called before the first frame update

    public bool isAdmin;

    public enum BetValue
    {
        Bet10, Bet50, Bet100, Bet500, Bet1000, Bet5000, Bet0
    }
    public List<BetValue> TestBV = new List<BetValue> { BetValue.Bet10, BetValue.Bet50, BetValue.Bet100, BetValue.Bet500, BetValue.Bet1000, BetValue.Bet5000 };

    GameObject Manager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void UpdateBalance()
    {
        playerBalanceText.text = DataManager.Instance.playerData.balance.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in playerList)
        {
            item.gameObject.SetActive(false);
        }
        SoundManager.Instance.StopBackgroundMusic();
        for (int i = 0; i < initialDicePositions.Length; i++)//storing the dice position for new round
            initialDicePositions[i] = dice[i].transform.position;
        
        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), playerAvatar));
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL"), playerAvatar);
        playerName.text = DataManager.Instance.playerData.firstName;
        ChipButtonClick(0);
        UpdateBalance();
        GetSetBotPlayer();
        HistoryLoader(DataManager.Instance.sevenUpDownWinHistory, DataManager.Instance.sevenUpDownDiceHistory);
        InitialiseScene();
        CheckSound();
    }


    public void InitialiseScene()
    {
        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.sevenUpDownRequirePlayer)
        {
            CreateAdmin(); //Creating Admin
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && isAdmin)
            {
                SetUpPlayers();
                StartCoroutine(StartBet());
            }
            else
            {
                if (isAdmin)
                    return;
                if (_isClickAvailable == false && isEnterBetStop == false)
                    waitNextRoundScreenObject.SetActive(true);
                print("Non admin player waiting for next round");
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerValue == 0 && isEnterBetStop == false && waitNextRoundScreenObject.activeSelf == false)
        {
            isEnterBetStop = true;
            StartCoroutine(StopBet());
            //Call Stop Betting function

        }
        else if (timerText.text.Equals("0") == false)
        {
            secondsCount -= Time.deltaTime;
            timerValue = ((int)secondsCount);
            timerText.text = timerValue.ToString();
        }
    }



    #region GamePlay

    public bool CheckMoney(float money)
    {
        float currentBalance = float.Parse(DataManager.Instance.playerData.balance);
        if (currentBalance - money < 0)
            return false;
        else
            return true;
    }

    public void PlaceBetButton(int number)
    {
        if (_isClickAvailable == false)
            return;

        bool hasMoney = CheckMoney(chipValue[selectedChipNo]);
        if (hasMoney == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }
        switch (number)//0 = 1st symbol// 1 = 2nd symbol and so on
        {
            case 0:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Jhandi_Munda_Bet-" + DataManager.Instance.gameId, "game", 18);
                    betValueOnSymbols[0] += (int)chipValue[selectedChipNo];
                    betText[0].text = betValueOnSymbols[0].ToString();
                    Vector3 randomPosition = new Vector3(Random.Range(minFirstSymbolX, maxFirstSymbolX), Random.Range(minFirstSymbolY, maxFirstSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[0]);
                    chip.transform.position = playerProfile.transform.position;
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    firstSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 1:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Jhandi_Munda_Bet-" + DataManager.Instance.gameId, "game", 18);
                    betValueOnSymbols[1] += (int)chipValue[selectedChipNo];
                    betText[1].text = betValueOnSymbols[1].ToString();
                    Vector3 randomPosition = new Vector3(Random.Range(minSecondSymbolX, maxSecondSymbolX), Random.Range(minSecondSymbolY, maxSecondSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[1]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    secondSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

            case 2:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Jhandi_Munda_Bet-" + DataManager.Instance.gameId, "game", 18);
                    betValueOnSymbols[2] += (int)chipValue[selectedChipNo];
                    betText[2].text = betValueOnSymbols[2].ToString();
                    Vector3 randomPosition = new Vector3(Random.Range(minThirdSymbolX, maxThirdSymbolX), Random.Range(minThirdSymbolY, maxThirdSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[2]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    thirdSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 3:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Jhandi_Munda_Bet-" + DataManager.Instance.gameId, "game", 18);
                    betValueOnSymbols[3] += (int)chipValue[selectedChipNo];
                    betText[3].text = betValueOnSymbols[3].ToString();
                    Vector3 randomPosition = new Vector3(Random.Range(minFourthSymbolX, maxFourthSymbolX), Random.Range(minFourthSymbolY, maxFourthSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[3]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    fourthSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 4:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Jhandi_Munda_Bet-" + DataManager.Instance.gameId, "game", 18);
                    betValueOnSymbols[4] += (int)chipValue[selectedChipNo];
                    betText[4].text = betValueOnSymbols[4].ToString();
                    Vector3 randomPosition = new Vector3(Random.Range(minFifthSymbolX, maxFifthSymbolX), Random.Range(minFifthSymbolY, maxFifthSymbolY));
                    //Vector3 randomPosition = new Vector3(minFifthSymbolX, minFifthSymbolY);
                    GameObject chip = Instantiate(chipPrefab, symbolArea[4]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    fifthSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 5:
                {
                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(chipValue[selectedChipNo].ToString(), DataManager.Instance.gameId, "Jhandi_Munda_Bet-" + DataManager.Instance.gameId, "game", 18);
                    betValueOnSymbols[5] += (int)chipValue[selectedChipNo];
                    betText[5].text = betValueOnSymbols[5].ToString();
                    Vector3 randomPosition = new Vector3(Random.Range(minSixthSymbolX, maxSixthSymbolX), Random.Range(minSixthSymbolY, maxSixthSymbolY));
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(chipPrefab, symbolArea[5]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[selectedChipNo];
                    chip.transform.position = playerProfile.transform.position;
                    sixthSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

        }
        //totalBetText.text = totalBet.ToString();
        JhandiMundaBet(number, selectedChipNo);//SocketEvent to send to admin/other players
        UpdateBalance();
    }

    public void ChipButtonClick(int no)
    {
        selectedChipNo = no;
        for (int i = 0; i < Chips.Count; i++)
        {
            if (i == no)
                Chips[i].transform.DOLocalMoveY(upValue, 0.05f);
            else
                Chips[i].transform.DOLocalMoveY(downValue, 0.05f);
        }
    }

    public void RollDice()//Animation for rolling dice will play in this function
    {
        diceRollObject.SetActive(true);
        diceRollObject.transform.DOScale(new Vector3(3f, 3f, 3f), .2f).OnComplete(() =>
        {


            
            diceRollObject.transform.DOShakePosition(1.5f, 10, 10, 60, true, true, ShakeRandomnessMode.Harmonic).OnComplete(() =>
            {
                diceRollObject.SetActive(false);
                diceRollObject.transform.localScale = Vector3.one;
                for (int i = 0; i < dice.Length; i++)
                    dice[i].SetActive(true);
            });

        });
        //diceContainerObject.transform.DOLocalMove();
        SoundManager.Instance.RollDice_Start_Sound();



        //dice1.GetComponent<Animator>().enabled = true;
        //dice2.GetComponent<Animator>().enabled = true;
        foreach (var item in dice)
            item.GetComponent<Animator>().enabled = true;
        

        Invoke(nameof(GenerateDiceNumbers), 2f);
    }

    public void GenerateDiceNumbers()
    {
        //dice1.GetComponent<Animator>().enabled = false;
        //dice2.GetComponent<Animator>().enabled = false;
        SoundManager.Instance.RollDice_Stop_Sound();
        //dice1.GetComponent<Image>().sprite = diceNumbers[dice1Result - 1];
        //dice2.GetComponent<Image>().sprite = diceNumbers[dice2Result - 1];
        for (int i = 0; i < diceResults.Length; i++)
        {
            dice[i].GetComponent<Animator>().enabled = false;
            dice[i].GetComponent<Image>().sprite = diceNumbers[diceResults[i]];
            print("sprite name at start = " + dice[i].GetComponent<Image>().sprite.name);
        }
        MoveDices();
        SegregateDiceAsPerSymbols();
        GetDice();
    }
    private void SegregateDiceAsPerSymbols()//This will add the dices to the list of the area which defines the area that the result came for
    {
        for (int i = 0; i < 6; i++)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            gameObjects = AddDicesToList(i);
            symbolDices.Add(gameObjects.Count);
        }
    }

    private void MoveDices()//Call for each area for dice to move to
    {
        MoveDicesToSymbols(firstSymbolResult, 0);
        MoveDicesToSymbols(secondSymbolResult, 1);
        MoveDicesToSymbols(thirdSymbolResult, 2);
        MoveDicesToSymbols(fourthSymbolResult, 3);
        MoveDicesToSymbols(fifthSymbolResult, 4);
        MoveDicesToSymbols(sixthSymbolResult, 5);
    }

    private void MoveDicesToSymbols(Transform[] symbolArea, int diceResult)//This will move the dices to the specific symbols that the dices gave after rolling
    {
        //for (int i = 0; i < dice.Length; i++)
        //{
        //    if(dice[i].GetComponent<Image>().sprite == diceNumbers[diceResults[diceResult]])
        //    {
        //        for (int j = 0; j < symbolArea.Length; j++)
        //        {
        //            dice[i].transform.DOLocalMove(symbolArea[j].transform.position, 0.3f);
        //        }
        //    }
        //}
        List<GameObject> tempList = new List<GameObject>();
        
            for (int j = 0; j < dice.Length; j++)
            {
                if (dice[j].GetComponent<Image>().sprite == diceNumbers[diceResult])
                {
                    tempList.Add(dice[j]);
                print("sprite name while adding to the list " + dice[j].GetComponent<Image>().sprite.name + " compared with = " + diceNumbers[diceResults[diceResult]].name);
                }

            }
        for (int i = 0; i < tempList.Count; i++)
        {
            tempList[i].transform.DOMove(symbolArea[i].transform.position, 0.3f);
        }
    }

    private List<GameObject> AddDicesToList(int result)//This function adds the list of dice to the Arraylist
    {
        List<GameObject> symbolAssignment = new List<GameObject>();
        foreach (var item in dice)
        {
            if (item.GetComponent<Image>().sprite == diceNumbers[result])
                symbolAssignment.Add(item);
        }

        return symbolAssignment;
    }

    public void GetDice()
    {
        List<int> winnerList = new List<int>();//key will contain the index of the dice result // value will contain the number of times it has repeated in the result

        for (int i = 0; i < symbolDices.Count; i++)
        {
            winnerList.Add(symbolDices[i]);
        }

        int diceTotal = 0;
        print("Dice1 = " + dice1Result + " Dice2 = " + dice2Result);

        diceTotal = dice1Result + dice2Result;

        if (isAdmin)
        {
            UpdateHistoryChips(winnerList);
        }


        

        StartCoroutine(RoundOverAnimation(winnerList));
    }

    public IEnumerator RoundOverAnimation(List<int> winnerList)
    {
        yield return new WaitForSeconds(3f);
        diceRollObject.SetActive(false);
        float animationSpeed = 0.2f;

        
        


        if (winnerList[0] < 2)
        {
            yield return new WaitForSeconds(animationSpeed);
            MoveChips(firstSymbolChips, animationSpeed);
            for (int i = 0; i < symbolArea[0].childCount; i++)
            {
                symbolArea[0].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (winnerList[1] < 2)
        {
            yield return new WaitForSeconds(animationSpeed);
            MoveChips(secondSymbolChips, animationSpeed);
            for (int i = 0; i < symbolArea[1].childCount; i++)
            {
                symbolArea[1].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (winnerList[2] < 2)
        {
            yield return new WaitForSeconds(animationSpeed);
            MoveChips(thirdSymbolChips, animationSpeed);
            for (int i = 0; i < symbolArea[2].childCount; i++)
            {
                symbolArea[2].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (winnerList[3] < 2)
        {
            yield return new WaitForSeconds(animationSpeed);
            MoveChips(fourthSymbolChips, animationSpeed);
            for (int i = 0; i < symbolArea[3].childCount; i++)
            {
                symbolArea[3].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (winnerList[4] < 2)
        {
            yield return new WaitForSeconds(animationSpeed);
            MoveChips(fifthSymbolChips, animationSpeed);
            for (int i = 0; i < symbolArea[4].childCount; i++)
            {
                symbolArea[4].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (winnerList[5] < 2)
        {
            yield return new WaitForSeconds(animationSpeed);
            MoveChips(sixthSymbolChips, animationSpeed);
            for (int i = 0; i < symbolArea[5].childCount; i++)
            {
                symbolArea[5].transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        

        //float investPrice = 0;
        //float betPrice = 0;
        float winAmount = 0;
        int winNo = 0;

        //var temp = winnerList.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        //winnerList.Clear();
        //winnerList = new Dictionary<int, int>(temp);

        for (int i = 0; i < winnerList.Count; i++)
        {
            if (betValueOnSymbols[i] > 0 && winnerList[i] == 2)
                winAmount += betValueOnSymbols[i] * 3;
            else if (betValueOnSymbols[i] > 0 && winnerList[i] == 3)
                winAmount += betValueOnSymbols[i] * 5;
            else if (betValueOnSymbols[i] > 0 && winnerList[i] == 4)
                winAmount += betValueOnSymbols[i] * 10;
            else if (betValueOnSymbols[i] > 0 && winnerList[i] == 5)
                winAmount += betValueOnSymbols[i] * 20;
            else if (betValueOnSymbols[i] > 0 && winnerList[i] == 6)
                winAmount += betValueOnSymbols[i] * 100;

            if(winnerList[i] >= 2)
                winObjects[i].SetActive(true);
            
            
        }

        

        float adminPercentage = DataManager.Instance.adminPercentage;
        if(winAmount > 0)//if player wins anything
        {
            float adminCommission = adminPercentage / 100;
            float winReward = winAmount - (winAmount * adminCommission);
            winAnimationTxt.gameObject.SetActive(true);
            winAnimationTxt.text = "+" + winAmount;
            Invoke(nameof(WinAmountTextOff), 1.25f);
            //winning animation and credit amount
            GameObject particleEffect = Instantiate(DataManager.Instance.winParticles, winParticleParent);
            Destroy(particleEffect, 2.5f);
            SoundManager.Instance.CasinoWinSound();
            DataManager.Instance.AddAmount(winReward, DataManager.Instance.gameId, "Jhandi_Munda_Win" + DataManager.Instance.gameId, "won", adminCommission, winNo);
            UpdateBalance();

        }

        
        yield return new WaitForSeconds(3f);
        //resetting the dice values, positions, everything for new round

        for (int i = 0; i < diceResults.Length; i++)
        {
            diceResults[i] = 0;
            winObjects[i].SetActive(false);
            betValueOnSymbols[i] = 0;
            dice[i].SetActive(false);
            dice[i].transform.position = initialDicePositions[i];
            betText[i].text = "0";
            totalBetText[i].text = "0";
            JhandiMundaAIManager.Instance.totalBetValues[i] = 0;
        }
        symbolDices.Clear();
        totalBet = 0;
        //totalBetText.text = totalBet.ToString();
        ClearChips();
        WinAfterRoundChange();
    }

    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);
    }


    public void MoveChips(List<GameObject> chips, float animationSpeed)
    {
        for (int i = 0; i < chips.Count; i++)
        {
            int no = i;
            //Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
            chips[no].transform.DOScale(Vector3.zero, animationSpeed);
            chips[no].transform.DOMove(dealer.transform.position, animationSpeed).OnComplete(() =>
            {
                //chips[no].transform.SetParent(winArea);
                //chips[no].transform.DOLocalMove(randomPosition, animationSpeed).OnComplete(() =>
                //{

                //    //chips.RemoveAt(no);

                //});
                //chips[no].transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), animationSpeed);
                //chips[no].transform.DOScale(Vector3.one, animationSpeed);
                //addChips.Add(chips[no]);
            });
        }

    }

    #endregion

    #region Admin Maintain


    public void SetRoomData(int[] diceresult)
    {
        JSONObject obj = new JSONObject();
        JSONObject[] diceData = new JSONObject[6];
        for (int i = 0; i < diceData.Length; i++)
        {
            diceData[i] = new JSONObject();
            diceData[i].AddField("diceData", diceresult[i]);
        }
        obj.AddField("WinData", new JSONObject(diceData));
        obj.AddField("dateTime", System.DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 18);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void SetDiceData(int[] diceResults)
    {
        JSONObject obj = new JSONObject();
        JSONObject[] diceData = new JSONObject[6];
        for (int i = 0; i < diceData.Length; i++)
        {
            diceData[i] = new JSONObject();
            diceData[i].AddField("diceData", diceResults[i]);
        }
        obj.AddField("WinData", new JSONObject(diceData));
        obj.AddField("room", TestSocketIO.Instace.roomid);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        print("Sending new dice data");
        TestSocketIO.Instace.Senddata("SendDiceDataJhandiMunda", obj);
    }

    public void GetRoomData(int[] diceInfo)
    {
        for (int i = 0; i < diceInfo.Length; i++)
        {
            diceResults[i] = diceInfo[i];
        }

        if (isAdmin)
            return;
        StartCoroutine(StartBet());
        SetUpPlayers();
        if (waitNextRoundScreenObject.activeSelf)
            waitNextRoundScreenObject.SetActive(false);
    }
    private void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
            isAdmin = true;
    }

    public void ChangeAdmin(string leftPlayerID)
    {
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObject.activeSelf == true)
            {
                StartCoroutine(StartBet());
                SetUpPlayers();
                waitNextRoundScreenObject.SetActive(false);
            }
        }
        else
            isAdmin = false;
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        System.Random rand = new System.Random();
        var shuffledAvatars = avatars.OrderBy(_ => rand.Next()).ToList();
        int[] randomAvatars = shuffledAvatars.Take(playerList.Count).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        var shuffledName = names.OrderBy(_ => rand.Next()).ToList();
        int[] randomNames = shuffledName.Take(playerList.Count).ToArray();

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerId.Equals(leftPlayerID))
            {
                playerList[i].isPlayer = false;
                playerList[i].avatarURL = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                playerList[i].userName.text = BotManager.Instance.botUserName[randomNames[i]];
                StartCoroutine(DataManager.Instance.GetImages(playerList[i].avatarURL, playerList[i].avatarImage));
                break;
            }
        }

        SetUpPlayers();
    }

    #endregion

    #region Socket Events

    public void GetResultData(List <int> dice)
    {
        if (isAdmin)
            return;
        for (int i = 0; i < dice.Count; i++)
        {
            diceResults[i] = dice[i];
        }
        print("new dice results received from admin");
    }

    public void JhandiMundaBet(int area, int chipNo)// area = 7Up || 7Down || 7
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("roomId", TestSocketIO.Instace.roomid);
        obj.AddField("area", area);
        obj.AddField("chipNo", chipNo);
        TestSocketIO.Instace.Senddata("JhandiMundaBet", obj);
    }

    public void AddWinData(List<int> winListData)
    {
        JSONObject obj = new JSONObject();
        JSONObject[] winData = new JSONObject[6];

        for (int i = 0; i < winData.Length; i++)
        {
            winData[i] = new JSONObject();
            winData[i].AddField("symbolIndex", winListData[i]);
        }
        obj.AddField("WinData", new JSONObject(winData));
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("roomId", TestSocketIO.Instace.roomid);
        //obj.AddField("WinList", winListData);
        //obj.AddField("DiceList", diceListData);

        TestSocketIO.Instace.Senddata("SetWinDataForJhandiMunda", obj);
    }

    public void GetSevenUpDownBet(int area, int chipNo, string playerID)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerId.Equals(playerID))
            {
                otherProfile = playerList[i].transform;
                break;
            }
        }
        JhandiMundaDownBet(area, chipNo, otherProfile);



    }

    private void JhandiMundaDownBet(int area, int chipNo, Transform profile)
    {
        switch (area)
        {
            case 0:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(minFirstSymbolX, maxFirstSymbolX), Random.Range(minFirstSymbolY, maxFirstSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[0]);
                    chip.transform.position = profile.transform.position;
                    chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
                    firstSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 1:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(minSecondSymbolX, maxSecondSymbolX), Random.Range(minSecondSymbolY, maxSecondSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[1]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
                    chip.transform.position = profile.transform.position;
                    secondSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

            case 2:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(minThirdSymbolX, maxThirdSymbolX), Random.Range(minThirdSymbolY, maxThirdSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[2]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
                    chip.transform.position = profile.transform.position;
                    thirdSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 3:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(minFourthSymbolX, maxFourthSymbolX), Random.Range(minFourthSymbolY, maxFourthSymbolY));
                    GameObject chip = Instantiate(chipPrefab, symbolArea[3]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
                    chip.transform.position = profile.transform.position;
                    fourthSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 4:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(minFifthSymbolX, maxFifthSymbolX), Random.Range(minFifthSymbolY, maxFifthSymbolY));
                    //Vector3 randomPosition = new Vector3(minFifthSymbolX, minFifthSymbolY);
                    GameObject chip = Instantiate(chipPrefab, symbolArea[4]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
                    chip.transform.position = profile.transform.position;
                    fifthSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 5:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(minSixthSymbolX, maxSixthSymbolX), Random.Range(minSixthSymbolY, maxSixthSymbolY));
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(chipPrefab, symbolArea[5]);
                    chip.transform.GetComponent<Image>().sprite = chipSprite[chipNo];
                    chip.transform.position = profile.transform.position;
                    sixthSymbolChips.Add(chip);
                    ChipGenerate(chip, randomPosition);
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
        }
    }



    #endregion

    #region Round Maintain

    public IEnumerator StartBet()
    {
        isEnterBetStop = true;
        _isClickAvailable = true;
        if (isAdmin)
        {
            //dice1Result = Random.Range(1, 7);
            //dice2Result = Random.Range(1, 7);
            for (int i = 0; i < diceResults.Length; i++)
                diceResults[i] = Random.Range(0, 6);
            
            SetRoomData(diceResults);
            //SetDiceData(dice1Result, dice2Result);//testing to see if it works without this function call
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        startBetObj.SetActive(true);
        yield return new WaitForSeconds(1.35f);
        startBetObj.SetActive(false);

        RestartTimer();
    }

    public IEnumerator StopBet()
    {
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        _isClickAvailable = false;
        isEnterBetStop = true;
        stopBetObj.SetActive(true);
        JhandiMundaAIManager.Instance.isActive = false;
        if (isAdmin)
        {
            //if (upBetValue + downBetValue + onBetValue > 0)
            //{
            for (int i = 0; i < diceResults.Length; i++)
                diceResults[i] = Random.Range(0, 6);
            
            SetDiceData(diceResults);
            //}
        }
        yield return new WaitForSeconds(1.5f);
        stopBetObj.SetActive(false);
        RollDice();

    }

    public void RestartTimer()
    {
        for (int i = 0; i < betValueOnSymbols.Length; i++)
            betValueOnSymbols[i] = 0;
        

        timerValue = fixTimerValue;
        secondsCount = timerValue;
        timerText.text = timerValue.ToString();
        isEnterBetStop = false;
        JhandiMundaAIManager.Instance.isActive = true;
    }

    void WinAfterRoundChange()//called after round is over
    {

        if (isAdmin)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 1 || isAdmin)
            {
                StartCoroutine(StartBet());
                SetUpPlayers();
            }
            else
                SetUpPlayers();

        }
    }

    public void SetUpPlayers()
    {
        int counter = 0;
        playerName.text = DataManager.Instance.playerData.firstName;
        UpdateBalance();
        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), playerAvatar));
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL"), playerAvatar);
        for (int i = 0; i < playerList.Count; i++)
        {
            if (counter < DataManager.Instance.joinPlayerDatas.Count)
            {
                if (DataManager.Instance.joinPlayerDatas[counter].userId.Equals(DataManager.Instance.playerData._id))
                    counter++;
            }
            if (counter < DataManager.Instance.joinPlayerDatas.Count && DataManager.Instance.joinPlayerDatas.Count > 1)
            {
                playerList[i].gameObject.SetActive(true);
                playerList[i].isPlayer = true;
                playerList[i].playerId = DataManager.Instance.joinPlayerDatas[counter].userId;
                playerList[i].userName.text = DataManager.Instance.joinPlayerDatas[counter].userName;
                playerList[i].avatarURL = DataManager.Instance.joinPlayerDatas[counter].avtar;
                StartCoroutine(DataManager.Instance.GetImages(playerList[i].avatarURL, playerList[i].avatarImage));
            }
            else
            {
                playerList[i].playerId = "Bot" + i.ToString();
                playerList[i].isPlayer = false;
                //print("Player not present/left, adding bot");
            }
            counter++;
        }
        foreach (var item in playerList)
        {
            if (item.gameObject.activeInHierarchy == true && item.isPlayer == false)
                return;
        }
        GetSetBotPlayer();
    }

    #endregion

    #region ChipsControl

    public void ChipGenerate(GameObject chip, Vector3 endPosition)
    {
        chip.transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), 0.2f);
        chip.transform.DOLocalMove(endPosition, 0.2f).OnComplete(() =>
        {
            chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
            {
                chip.transform.DOScale(Vector3.one, 0.07f);
            });
            print("LocalPosition = " + chip.transform.localPosition + " Position = " + chip.transform.position);
        });
        //chip.transform.localPosition = endPosition;
        //print("LocalPosition = " + chip.transform.localPosition + " Position = " + chip.transform.position);
    }

    public void ClearChips()
    {
        DestroyChips(firstSymbolChips);
        DestroyChips(secondSymbolChips);
        DestroyChips(thirdSymbolChips);
        DestroyChips(fourthSymbolChips);
        DestroyChips(fifthSymbolChips);
        DestroyChips(sixthSymbolChips);

        firstSymbolChips.Clear();
        secondSymbolChips.Clear();
        thirdSymbolChips.Clear();
        fourthSymbolChips.Clear();
        fifthSymbolChips.Clear();
        sixthSymbolChips.Clear();
    }

    private void DestroyChips(List<GameObject> chips)
    {
        for (int i = 0; i < chips.Count; i++)
        {
            Destroy(chips[i]);
        }
    }

    #endregion

    #region Player/Bot History

    public void GetSetBotPlayer()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        System.Random rand = new System.Random();
        var shuffledAvatars = avatars.OrderBy(_ => rand.Next()).ToList();
        int[] randomAvatars = shuffledAvatars.Take(playerList.Count).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        var shuffledName = names.OrderBy(_ => rand.Next()).ToList();
        int[] randomNames = shuffledName.Take(playerList.Count).ToArray();

        for (int i = 0; i < playerList.Count; i++)
        {
            //item.
            //history.avatar = BotManager.Instance.botUser_Profile_URL[randomAvatars]
            if (playerList[i].isPlayer == false)
            {
                playerList[i].gameObject.SetActive(true);
                //playerList[i].playerId = "Bot" + i.ToString();
                playerList[i].avatarURL = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
                playerList[i].userName.text = BotManager.Instance.botUserName[randomNames[i]];
                StartCoroutine(DataManager.Instance.GetImages(playerList[i].avatarURL, playerList[i].avatarImage));
            }
            else
                continue;
        }
    }

    #endregion

    #region Error Screen

    public void OpenErrorScreen()
    {
        errorScreenObject.SetActive(true);
    }

    public void Error_OKButtonClick()
    {
        SoundManager.Instance.ButtonClick();
    }

    #endregion

    #region Menu Screen

    public void LobbyButtonClick()
    {
        TestSocketIO.Instace.LeaveRoom();
        SoundManager.Instance.StartBackgroundMusic();
        SceneManager.LoadScene("Main");
    }

    private void OnApplicationQuit()
    {
        TestSocketIO.Instace.LeaveRoom();
    }

    #endregion

    public void HistoryTracker(List<int> win)//1 = 7Down //2 = 7Up //3 = 7
    {
        for (int i = 0; i < historyHolder.Length; i++)
        {

        if (historyHolder[i].childCount == maxWinListCount)
            Destroy(historyHolder[i].GetChild(0).gameObject);
        var chip = Instantiate(resultPrefab, historyHolder[i]);
        chip.GetComponent<JhandiMundaResult>().ChipDetails(win);
        }

    }

    private void UpdateHistoryChips(List<int> winNo)
    {
        if (winList.Count >= maxWinListCount)
            winList.RemoveAt(0);
        if (diceResultList.Count >= maxWinListCount)
            diceResultList.RemoveAt(0);
        //winList.Add(winNo);
        //diceResultList.Add(diceResult);



        HistoryTracker(winNo);


        if (!isAdmin)
            return;
        //DataManager.Instance.sevenUpDownWinHistory = string.Join(",", winList.Select(x => x.ToString()).ToArray());
        //DataManager.Instance.sevenUpDownDiceHistory = string.Join(",", diceResultList.Select(x => x.ToString()).ToArray());

        AddWinData(winNo);
    }

    private void LoadHistoryChips()
    {
        string listString = PlayerPrefs.GetString(playerPrefsKey, "");
        if (listString != "")
            winList = new List<int>(listString.Split(',').Select(x => int.Parse(x)));
        else
            return;
    }

    public void GetUpdatedHistory(List<int> winData)
    {
        if (isAdmin)
            return;
        //if (winData != "")
        //    winList = new List<int>(winData.Split(',').Select(x => int.Parse(x)));
        //if (diceWinData != "")
        //    diceResultList = new List<int>(diceWinData.Split(',').Select(x => int.Parse(x)));
        //else
        //    return;

        //HistoryTracker(item);


        //for (int i = 0; i < historyHolder.childCount; i++)
        //{
        //    Destroy(historyHolder.GetChild(i).gameObject);
        //}
        for (int i = 0; i < historyHolder.Length; i++)
        {
            if (historyHolder[i].childCount == maxWinListCount)
                Destroy(historyHolder[i].GetChild(0).gameObject);
        }

        //for (int i = 0; i < winList.Count; i++)
        //{
        HistoryTracker(winData);
        //}

    }

    public void HistoryLoader(string data, string diceData)
    {
        if (diceData != "")
            diceResultList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        if (data != "")
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        else
            return;

        for (int i = 0; i < winList.Count; i++)
        {
            //HistoryTracker(winList[i], diceResultList[i]);
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
}
