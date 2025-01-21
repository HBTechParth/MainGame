using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AviatorGameManager : MonoBehaviour
{
    public static AviatorGameManager Instance { get; private set; }

    public event Action OnGameStart;
    public event Action OnGameCrash;
    public event Action OnGameRestart;

    public float betAmount = 0f;
    private float totalBetAmount = 0f;
    public float minCrashTime = 1f;
    public float maxCrashTime = 10f;
    public float crashTime;
    public float gameRestartDelay = 10f;

    [Header("--- Supporting Scripts ---")]
    public RocketController controller;
    public GraphManager graph;

    [Header("--- User Data ---")]
    public Image avatarImg;
    public Text userNameTxt;
    public Text balanceTxt;

    [Header("--- Menu UI ---")]
    public GameObject menuScreenObj;
    public GameObject ruleScreenObj;
    public GameObject errorScreenObj;
    public GameObject errorScreenObjONBET;

    public GameObject waitNextRoundScreenObj;

    [Header("--- Canvas Objects ---")]
    public GameObject lineCanvas;
    public GameObject rocketCanvas;
    public GameObject bettingScene;

    [Header("--- Game Betting ---")]
    public Text timerTxt;
    public float fixTimerValue;
    public float downValue;
    public float upValue;
    public float minBetAreaX;
    public float maxBetAreaX;
    public float minBetAreaY;
    public float maxBetAreaY;
    private Vector3 betAreaSize;
    private Vector3 betAreaCenter;
    public int selectChipNo;
    public GameObject bettingArea;
    public GameObject[] chipBtn;
    public Button rightCashOutButton;
    public Button leftCashOutButton;
    public GameObject chipObj;
    public float[] chipPrice;
    public Sprite[] chipsSprite;
    public List<GameObject> betChipList = new List<GameObject>();

    [Header("--- GamePlay ---")]
    public GameObject multiplayerObj;
    public Text multiplierText;
    public float multiplierSpeed = 0.3f;
    public Text totalBetText;
    public Text myBetText;
    public Text rightCashOutText;
    public Text leftCashOutText;
    public float playerWinAmount;

    [Header("--- Fake Bot ---")]
    public float botBettingDuration;
    public GameObject botBettingStartPoint;
    public bool shouldBotBet = true;
    public GameObject botPlayersList;
    public Image[] botPlayers;
    public Text[] botPlayersName;
    public Text[] botPlayersCoins;

    [Header("--- History ---")]
    public GameObject historyPrefab;
    public Transform historyParent;
    public List<string> historyList = new List<string>();


    [Header("--- Sounds ---")]
    public Image soundImg;
    public Image musicImg;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;

    private float multiplier;
    private bool isGameRunning = false;
    //private bool isAdmin = false;
    private bool isBettingSceneActive = false;

    public Text winTxt;
    public float aviatorAdminCommission;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        Debug.Log("adminCommission => " + TestSocketIO.Instace.adminCommission);
        aviatorAdminCommission = TestSocketIO.Instace.adminCommission;
        CreateAdmin();
        CalculateBetAreaBounds();
        InitializeSupportingScripts();
        SoundManager.Instance.StopBackgroundMusic();
        UpdateUserData();
        lineCanvas.SetActive(false);
        rocketCanvas.SetActive(false);
        multiplayerObj.gameObject.SetActive(false);
        rightCashOutButton.gameObject.SetActive(false);
        leftCashOutButton.gameObject.SetActive(false);
        bettingScene.gameObject.SetActive(false);
        botPlayersList.gameObject.SetActive(false);
        isGameRunning = false;
        multiplier = 1f;
        CheckSound();
        BottomChipAnimDOWN();
        SetChipBtnInteractable(false);
        StartGamePlay();
        UpdateToggleSprite();
        UpdateInputFieldInteractivity();
        //ResetScripts();
    }

    private void InitializeSupportingScripts()
    {
        controller.InitializeRocketController();
        graph.InitializeGraphManager();
    }

    private void ResetScripts()
    {
        controller.HandleGameRestart();
        graph.HandleGameRestart();
    }

    public bool IsGameRunning()
    {
        return isGameRunning;
    }

    public void StartGame()
    {
        isGameRunning = true;
        OnGameStart?.Invoke();
        StartCoroutine(GameLoop());
        StartCoroutine(UpdateMultiplierText());
        foreach (GameObject chip in betChipList)
        {
            Destroy(chip);
        }
        betChipList.Clear();
    }

    /*  private IEnumerator GameLoop()
      {
          float elapsedTime = 0f;
          // Making line and rocket visible 
          lineCanvas.SetActive(true);
          rocketCanvas.SetActive(true);
          rightCashOutButton.interactable = true;
          // leftCashOutButton.interactable = true;
          rightCashOutButton.gameObject.SetActive(true);
          //     leftCashOutButton.gameObject.SetActive(true);
          multiplayerObj.gameObject.SetActive(true);

          while (isGameRunning)
          {
              elapsedTime += Time.deltaTime;

              if (elapsedTime >= crashTime)
              {
                  isGameRunning = false;
                  OnGameCrash?.Invoke();
                  StartCoroutine(BlinkMultiplier()); // Blink 3 times with a duration of 0.2 seconds
                  rightCashOutButton.gameObject.SetActive(false);
                  leftCashOutButton.gameObject.SetActive(false);
                  // Update history data
                  string newHistory = multiplier.ToString("F2") + "X";
                  if (historyList.Count >= 10)
                  {
                      historyList.RemoveAt(0);
                  }
                  historyList.Add(newHistory);
                  UpdateHistoryPrefabs();
                  // Update DataManager.Instance.historyPoints
                  DataManager.Instance.historyPoints = string.Join(",", historyList);
                  SetWinData(DataManager.Instance.historyPoints);

                  Invoke(nameof(RestartGame), gameRestartDelay); // Delay to reset the game
              }

              yield return null;
          }
      }*/

    private List<bool> last10Results = new List<bool>(); // Stores the last 10 results (true = win, false = loss)

    private List<bool> lastResults = new List<bool>(); // Stores the last few bet results (true = win, false = loss)



    private IEnumerator GameLoop()
    {
        float elapsedTime = 0f;

        // Calculate crashTime based on total bet amount and recent streak
        float totalBet = betAmount; // Assuming you have a way to get the total bet amount
        crashTime = CalculateCrashTime(totalBet);

        // Making line and rocket visible
        lineCanvas.SetActive(true);
        rocketCanvas.SetActive(true);
        rightCashOutButton.interactable = true;
        rightCashOutButton.gameObject.SetActive(true);
        multiplayerObj.gameObject.SetActive(true);

        while (isGameRunning)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= crashTime)
            {
                isGameRunning = false;
                OnGameCrash?.Invoke();
                StartCoroutine(BlinkMultiplier()); // Blink 3 times with a duration of 0.2 seconds
                rightCashOutButton.gameObject.SetActive(false);
                leftCashOutButton.gameObject.SetActive(false);

                // Update history data
                string newHistory = multiplier.ToString("F2") + "X";
                if (historyList.Count >= 10)
                {
                    historyList.RemoveAt(0);
                }
                historyList.Add(newHistory);
                UpdateHistoryPrefabs();

                // Record the result of this round
                if (betAmount != 0)
                {
                    bool isWin = betAmount < playerWinAmount; // Assume anything above 2x is a win
                    Debug.Log("betAmount => " + betAmount + "       playerWinAmount=> " + playerWinAmount);
                    RecordResult(isWin);
                }

                // Update DataManager.Instance.historyPoints
                DataManager.Instance.historyPoints = string.Join(",", historyList);
                SetWinData(DataManager.Instance.historyPoints);

                Invoke(nameof(RestartGame), gameRestartDelay); // Delay to reset the game
            }

            yield return null;
        }
    }

    // Method to calculate crash time based on bet amount and streak
    private float CalculateCrashTime(float totalBet)
    {
        // Streak ke hisaab se agar 1x force crash ho, toh direct 1x hi return karo
        if (ShouldForceCrashBasedOnStreak())
        {
            Debug.Log("Forced crash at 1x due to streak conditions.");
            rightCashOutButton.gameObject.SetActive(false); 
            return 1f;  // Yaha par 1x return karenge agar streak condition match kar gayi
        }

        // Agar streak force nahi karta, tab niche ka code chalega
        float minCrashTime = 1f; // Minimum crash time (loss)
        float maxCrashTime = 20f; // Maximum crash time (profit)

        // Bet range ke hisaab se loss probability decide karo
        float lossProbability;
        if (totalBet >= 10 && totalBet <= 100)
        {
            lossProbability = 0.2f; // 40% loss chance
        }
        else if (totalBet > 100 && totalBet <= 500)
        {
            lossProbability = 0.5f; // 50% loss chance
        }
        else
        {
            lossProbability = 0.6f; // 60% loss chance
        }

        // Yaha par decide karo ki loss ya profit hoga
        bool isLoss = UnityEngine.Random.value < lossProbability;

        // Agar loss hai, toh crash time ko 1x se 2x ke beech rakhna
        if (isLoss)
        {
            return UnityEngine.Random.Range(minCrashTime, 3f); // Loss (1x–2x)
        }
        else
        {
            return UnityEngine.Random.Range(3f, maxCrashTime); // Profit (2x–6x)
        }
    }



    // Method to check if a crash should be forced at 1x based on streak
    private bool ShouldForceCrashBasedOnStreak()
    {
        if (lastResults.Count < 1)
        {
            return false; // Not enough data to analyze
        }

        // Calculate consecutive wins
        int consecutiveWins = 0;
        for (int i = lastResults.Count - 1; i >= 0; i--)
        {
            if (lastResults[i])
            {
                consecutiveWins++;
            }
            else
            {
                break; // Stop counting if we find a loss
            }
        }
        Debug.Log("consecutiveWins => " + consecutiveWins);

        // Force crash based on streak
        if (consecutiveWins >= 4)
        {
            if (UnityEngine.Random.value < 0.9f) // 90% chance
            {
                Debug.Log("Clearing data due to streak condition: consecutiveWins >= 4");
                ClearData(); // Data clear karenge
                return true; // Force crash
            }
        }
        else if (consecutiveWins == 3)
        {
            if (UnityEngine.Random.value < 0.8f) // 80% chance
            {
                Debug.Log("Clearing data due to streak condition: consecutiveWins == 3");
                ClearData(); // Data clear karenge
                return true; // Force crash
            }
        }
        else if (consecutiveWins == 2)
        {
            if (UnityEngine.Random.value < 0.6f) // 60% chance
            {
                Debug.Log("Clearing data due to streak condition: consecutiveWins == 2");
                ClearData(); // Data clear karenge
                return true; // Force crash
            }
        }
        else if (consecutiveWins == 1)
        {
            if (UnityEngine.Random.value < 0.3f) // 30% chance
            {
                Debug.Log("Clearing data due to streak condition: consecutiveWins == 1");
                ClearData(); // Data clear karenge
                return true; // Force crash
            }
        }
        // Default crash logic
        return false;
    }


    private void ClearData()
    {
        lastResults.Clear();
        Debug.Log("Cleared lastResults data after a forced crash.");
    }

    // Method to record the result of a bet
    private void RecordResult(bool isWin)
    {
        // Ensure only the last 10 results are stored
        if (lastResults.Count >= 10)
        {
            lastResults.RemoveAt(0); // Remove the oldest result
        }

        lastResults.Add(isWin); // Add the latest result
        Debug.Log("Updated Last Results: " + string.Join(", ", lastResults));
    }




    private IEnumerator UpdateMultiplierText()
    {
        float elapsedTime = 1f;

        while (isGameRunning)
        {

            elapsedTime += Time.deltaTime * multiplierSpeed;

            // Ensure elapsedTime doesn't exceed crashTime
            if (elapsedTime >= crashTime)
            {
                elapsedTime = crashTime;  // Stop at crashTime
                multiplier = crashTime;   // Set multiplier to crashTime value
            }
            multiplier = elapsedTime;
            multiplierText.text = multiplier.ToString("F2") + "X";

            if (isOn && betAmount > 0)
            {
                float inputValue;
                if (float.TryParse(autoCashOutinputField.text, out inputValue))
                {
                    if (inputValue < 1.1f)
                    {
                        inputValue = 1.1f;
                        autoCashOutinputField.text = inputValue.ToString("F2"); // Update the InputField to 1.1
                        Debug.Log("Value was below 1.1. Set to 1.1 by default.");
                    }
                    if (Mathf.Abs(multiplier - inputValue) < 0.01f) // Using a small tolerance value
                    {
                        Debug.Log("Match found! Auto cash-out triggered.");
                        CashOutButtonClick();
                        // Your auto cash-out logic here
                    }
                    else
                    {
                        //  Debug.Log($"No match: multiplier = {multiplier:F2}, inputValue = {inputValue:F2}");
                    }
                }
                else
                {
                    Debug.LogError("Invalid input in the auto cash-out InputField.");
                }
            }
            UpdateCashOutText();

            yield return null;
        }
    }


    private void UpdateCashOutText()
    {
        if (betAmount > 0)
        {
            float cashOutAmount = betAmount * multiplier;
            rightCashOutText.text = cashOutAmount.ToString("F2");
            leftCashOutText.text = cashOutAmount.ToString("F2");

        }
        else
        {
            rightCashOutText.text = "0.00";
            leftCashOutText.text = "0.00";

        }
    }
    private IEnumerator BlinkMultiplier()
    {
        yield return multiplierText.DOFade(0f, 0.2f).SetLoops(6, LoopType.Yoyo).WaitForCompletion();
    }


    private void RestartGame()
    {
        multiplier = 1f;
        OnGameRestart?.Invoke();
        ResetValues();
    }

    private void ResetValues()
    {
        lineCanvas.SetActive(false);
        rocketCanvas.SetActive(false);
        rightCashOutButton.gameObject.SetActive(false);
        leftCashOutButton.gameObject.SetActive(false);
        multiplayerObj.gameObject.SetActive(false);
        betAmount = 0f;
        playerWinAmount = 0f;
        totalBetAmount = 0f;
        totalBetText.text = "Total Bet : " + "0";
        totalBetText.text = "Total Bet : " + "0";
        myBetText.text = "PLACE BET : 0";
        rightCashOutText.text = "0";
        leftCashOutText.text = "0";
        EnableBettingScene();
    }

    private void EnableBettingScene()
    {
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        isBettingSceneActive = true;
        SetChipBtnInteractable(true);
        bettingScene.gameObject.SetActive(true);
        StartCoroutine(BettingTimer());
        StartCoroutine(BotBettingCoroutine());
    }

    public float GetCurrentMultiplier()
    {
        return multiplier * betAmount;
    }

    private void GenerateRandomCrashTime()
    {
        //  crashTime = UnityEngine.Random.Range(minCrashTime, maxCrashTime);
        /*if (isAdmin)
        {
            crashTime = UnityEngine.Random.Range(minCrashTime, maxCrashTime);
        }*/
    }

    public void StartGameByAdmin()
    {
        if (!isGameRunning && !isBettingSceneActive)
        {
            SoundManager.Instance.CasinoTurnSound();
            DataManager.Instance.UserTurnVibrate();
            StartCoroutine(BettingTimer());
            isBettingSceneActive = true;
            lineCanvas.SetActive(false);
            rocketCanvas.SetActive(false);
            SetChipBtnInteractable(true);
            bettingScene.gameObject.SetActive(true);
            rightCashOutButton.gameObject.SetActive(false);
            leftCashOutButton.gameObject.SetActive(false);
            multiplayerObj.gameObject.SetActive(false);
            SetChipBtnInteractable(true);
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
            StartCoroutine(BotBettingCoroutine());
            LoadHistoryData();
        }
    }

    #region Game History

    private void LoadHistoryData()
    {
        string historyData = DataManager.Instance.historyPoints;

        if (string.IsNullOrEmpty(historyData))
        {
            historyData = "0.5X,20X,5X,15X,2.2X,6X,5.63X";
        }

        string[] historyArray = historyData.Split(',');

        historyList.Clear();
        foreach (string history in historyArray)
        {
            if (!string.IsNullOrEmpty(history))
            {
                historyList.Add(history);
            }
        }

        UpdateHistoryPrefabs();
    }

    private void UpdateHistoryPrefabs()
    {
        // Clear existing history prefabs
        foreach (Transform child in historyParent)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new history prefabs
        foreach (var t in historyList)
        {
            GameObject historyInstance = Instantiate(historyPrefab, historyParent);
            Text historyText = historyInstance.transform.GetChild(0).GetComponent<Text>();
            historyText.text = t;
        }
    }

    public void GetUpdatedHistory(string data)
    {
        //if (isAdmin) return;
        /*if(data != "")
        {
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        }
        else
        {
            return;
        }
        
        foreach (var t in winList)
        {
            HistoryTacker(t);
        }*/
    }

    #endregion

    #region Timer
    private int previousTimerValue = -1;
    public Image countdownTimmer;
    private IEnumerator BettingTimer()
    {
        float remainingTime = fixTimerValue;
        previousTimerValue = -1; // Initialize to an invalid value to ensure the first comparison works

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerTxt.text = seconds.ToString();

            if (seconds == 5) // Trigger sound safely
            {
                SoundManager.Instance.AlertSound();
            }

            if (seconds <= 3 && seconds >= 0 && seconds != previousTimerValue) // Trigger animations for countdown
            {
                countdownTimmer.gameObject.SetActive(true);
                previousTimerValue = seconds; // Update the previous value

                if (seconds < DataManager.Instance.countdownSprites.Count) // Ensure valid sprite index
                {
                    countdownTimmer.sprite = DataManager.Instance.countdownSprites[seconds];
                }

                // Scale animation
                countdownTimmer.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => countdownTimmer.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f));
            }

            yield return null;
        }

        // Ensure timer reaches zero
        timerTxt.text = "00";
        countdownTimmer.gameObject.SetActive(false);

        // End betting phase
        isBettingSceneActive = false;
        bettingScene.gameObject.SetActive(false);
        botPlayersList.gameObject.SetActive(false);
        SetChipBtnInteractable(false);
        GenerateRandomCrashTime();
        StartGame();
    }


    #endregion

    #region Betting

    public void ChipButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        BottomChipAnim(no);
    }

    void BottomChipAnim(int no)
    {
        selectChipNo = no;
        for (int i = 0; i < chipBtn.Length; i++)
        {
            chipBtn[i].transform.DOMoveY(i == no ? upValue : downValue, 0.05f);
        }
    }
    void BottomChipAnimDOWN()
    {

        for (int i = 0; i < chipBtn.Length; i++)
        {
            chipBtn[i].transform.DOMoveY(downValue, 0.05f);
        }
        chipBtn[0].transform.DOMoveY(upValue, 0.05f);
    }

    private void CalculateBetAreaBounds()
    {
        float betAreaWidth = Mathf.Abs(maxBetAreaX - minBetAreaX);
        float betAreaHeight = Mathf.Abs(maxBetAreaY - minBetAreaY);
        betAreaSize = new Vector3(betAreaWidth, betAreaHeight, 1f);
        betAreaCenter = new Vector3((minBetAreaX + maxBetAreaX) / 2f, (minBetAreaY + maxBetAreaY) / 2f, 0f);
    }
    public Text limitOutText;
    public void BetButtonClick()
    {
        bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
        if (isMoneyAv == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }

        //if (betAmount + chipPrice[selectChipNo] <= 600)
        {
            SoundManager.Instance.ThreeBetSound();
            DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId, "Aviator-Bet-" + DataManager.Instance.gameId, "game", 2);

            betAmount += chipPrice[selectChipNo];
            totalBetAmount += chipPrice[selectChipNo];
            totalBetText.text = "Total Bet : " + totalBetAmount.ToString("F2");
            myBetText.text = "PLACE BET : " + betAmount.ToString("F2");

            Vector3 rPos = GetRandomPositionWithinTransform(bettingArea.transform);
            GameObject chipGen = Instantiate(chipObj, bettingArea.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
            chipGen.transform.position = avatarImg.transform.position;
            betChipList.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }
        /* else
         {
             limitOutText.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
             limitOutText.text = "Maximum Bet Limit Under 600 INR";
             DOVirtual.DelayedCall(1f, () =>
             {
                 limitOutText.rectTransform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
             });
         }*/

    }
    public Vector3 GetRandomPositionWithinTransform(Transform targetTransform)
    {
        RectTransform rectTransform = targetTransform.GetComponent<RectTransform>();

        // Calculate the local bounds
        Vector2 size = rectTransform.rect.size;
        Vector3 localRandomPos = new Vector3(
            UnityEngine.Random.Range(-size.x / 2, size.x / 2),
            UnityEngine.Random.Range(-size.y / 2, size.y / 2),
            0
        );

        // Convert local position to world position
        Vector3 worldRandomPos = targetTransform.TransformPoint(localRandomPos);

        return worldRandomPos;
    }
    private void ChipGenerate(GameObject chip, Vector3 endPos)
    {
        chip.transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), 0.2f);
        chip.transform.DOMove(endPos, 0.2f).OnComplete(() =>
        {
            chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
            {
                chip.transform.DOScale(Vector3.one, 0.07f);
            });
        });
    }

    private Vector3 GetRandomPositionWithinBettingArea()
    {
        float randomX = UnityEngine.Random.Range(minBetAreaX, maxBetAreaX);
        float randomY = UnityEngine.Random.Range(minBetAreaY, maxBetAreaY);

        return new Vector3(randomX, randomY, 0f);
    }

    //To see the betting area
    /*private void OnDrawGizmos()
    {
        CalculateBetAreaBounds();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(betAreaCenter, betAreaSize);
    }*/

    private bool CheckMoney(float money)
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

    public void CashOutButtonClick()
    {
        if (isGameRunning && betAmount > 0)
        {
            float investPrice = betAmount * multiplier;

            Debug.Log("investPrice  => " + investPrice);

            // Set admin commission percentage dynamically
            float adminCommissionPercentage = aviatorAdminCommission; // Set commission percentage (e.g., 10 for 10%, 20 for 20%, etc.)
            float adminCommission = investPrice * (adminCommissionPercentage / 100f); // Calculate commission
            Debug.Log("aviatorAdminCommission (" + adminCommissionPercentage + "%) = > " + adminCommission);

            // Deduct commission from investPrice
            float winAmount = investPrice - adminCommission; // Final winAmount after commission
            Debug.Log("winAmount (after " + adminCommissionPercentage + "% commission) = > " + winAmount);

            // Calculate player's total win amount
            playerWinAmount = betAmount + (winAmount - betAmount); // Add profit to the initial bet
            Debug.Log("playerWinAmount = > " + playerWinAmount);






            rightCashOutButton.interactable = false;
            leftCashOutButton.interactable = false;

            if (playerWinAmount != 0)
            {
                SoundManager.Instance.CasinoWinSound();
                Debug.Log("Player Win Amount   =  " + playerWinAmount);
                winTxt.rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
                winTxt.text = "You won " + playerWinAmount.ToString("F2");
                DOVirtual.DelayedCall(3f, () =>
                {
                    winTxt.rectTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack);
                });
                DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "aviator-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommission), 1);
            }

            float randomValue = UnityEngine.Random.Range(0f, 1f); // Random value between 0 and 1.
            if (randomValue <= 0.4f) // 30% chance to increase crash time
            {
                float multiplierFactor = UnityEngine.Random.Range(0f, 2f); // Increase crash time by a factor of 1.5 to 3
                crashTime = crashTime * multiplierFactor;
                Debug.Log("Crash time increased by factor: " + multiplierFactor + ", New Crash Time: " + crashTime);
            }
        }
    }
    public Image toggleImage; // Reference to the Image component of the button
    public Sprite onSprite;  // Sprite for the "On" state
    public Sprite offSprite; // Sprite for the "Off" state

    private bool isOn = false; // Track the toggle state
    public InputField autoCashOutinputField;


    // Call this method when the button is clicked
    public void OnAUTOCASHButtonClick()
    {
        Debug.Log("OnAUTOCASHButtonClick= " + isOn);

        isOn = !isOn; // Toggle the state
        UpdateToggleSprite();
        UpdateInputFieldInteractivity();
        Debug.Log("Auto Cash Out is " + (isOn ? "On" : "Off"));
    }
    private void UpdateInputFieldInteractivity()
    {
        autoCashOutinputField.interactable = isOn;
    }
    public void ValidateInputField()
    {
        string inputValue = autoCashOutinputField.text; // Get the current value from the InputField

        if (float.TryParse(inputValue, out float enteredValue)) // Try to parse the input to a float
        {
            if (enteredValue < 1.1f) // Check if the value is less than 1.1
            {
                autoCashOutinputField.text = "1.01"; // Set to 1.01 if the condition is met
                Debug.Log("Input value was less than 1.1. Automatically set to 1.01.");
            }
        }
        else
        {
            Debug.LogError("Invalid input. Please enter a valid number.");
        }
    }
    // Update the button sprite based on the state
    private void UpdateToggleSprite()
    {
        toggleImage.sprite = isOn ? onSprite : offSprite;
    }
    // Optional: Add a getter for external scripts to check the state
    public bool IsToggleOn()
    {
        return isOn;
    }

    #endregion

    #region UserData

    private void StartGamePlay()
    {
        StartGameByAdmin();
        /*if (isAdmin)
        {
            StartGameByAdmin();
        }
        else
        {
            waitNextRoundScreenObj.gameObject.SetActive(true);
            print("Not an admin");
        }*/
    }

    private void UpdateUserData()
    {
        userNameTxt.text = DataManager.Instance.playerData.firstName.ToString();
        balanceTxt.text = DataManager.Instance.playerData.balance.ToString();
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL"), avatarImg);
    }

    public void UpdateBalance()
    {
        balanceTxt.text = DataManager.Instance.playerData.balance.ToString();
    }

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            //isAdmin = true;
        }
    }

    #endregion

    #region Buttons & UI

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    public void CloseMenuScreenButton()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
    }

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }

    public void CloseRuleButton()
    {
        SoundManager.Instance.ButtonClick();
        ruleScreenObj.SetActive(false);
    }

    public void OpenErrorScreen()
    {
        errorScreenObj.SetActive(true);
    }
    public void OpenErrorScreenONBET()
    {
        errorScreenObjONBET.SetActive(true);
    }
    public void Error_Ok_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        errorScreenObj.SetActive(false);
    }

    public void Error_Shop_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //Instantiate(shopPrefab, shopPrefabParent.transform);
        errorScreenObj.SetActive(false);
    }


    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StopRocketThrustSound();
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
        }
    }

    private void SetChipBtnInteractable(bool isInteractable)
    {
        foreach (GameObject btn in chipBtn)
        {
            btn.GetComponent<Button>().interactable = isInteractable;
        }
    }


    #endregion

    #region FakeBots

    public void OpenPlayersButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        LoadBotPlayers();
        botPlayersList.gameObject.SetActive(true);
    }

    public void ClosePlayersButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        botPlayersList.gameObject.SetActive(false);
    }

    private IEnumerator BotBettingCoroutine()
    {
        while (isBettingSceneActive)
        {
            if (shouldBotBet)
            {
                SoundManager.Instance.ThreeBetSound();
                int randomChipIndex = UnityEngine.Random.Range(1, chipPrice.Length);
                Vector3 randomPosition = GetRandomPositionWithinBettingArea();
                GameObject chipGen = Instantiate(chipObj, bettingArea.transform);
                chipGen.transform.GetComponent<Image>().sprite = chipsSprite[randomChipIndex];
                chipGen.transform.position = botBettingStartPoint.transform.position;
                betChipList.Add(chipGen);
                ChipGenerate(chipGen, randomPosition);
                totalBetAmount += chipPrice[randomChipIndex];
                totalBetText.text = "Total Bet : " + totalBetAmount.ToString("F2");
            }
            botBettingDuration = UnityEngine.Random.Range(0.1f, 0.3f);
            yield return new WaitForSeconds(botBettingDuration);
        }
    }

    public void CancelBEt()
    {
        Debug.Log("---  CancelBEt CALL ");
        if (!isGameRunning && betAmount > 0)
        {
            DataManager.Instance.ReverseAmount(betAmount, DataManager.Instance.gameId, "Aviator-return-" + DataManager.Instance.gameId, "reverse", 1);
            betAmount = 0;
            myBetText.text = "PLACE BET : " + betAmount.ToString("F2");

        }
    }

    private void LoadBotPlayers()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int[] randomAvatars = avatars.Take(botPlayers.Length).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int[] randomNames = names.Take(botPlayers.Length).ToArray();

        for (int i = 0; i < botPlayers.Length; i++)
        {
            // Load random avatar
            string avatarURL = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
            StartCoroutine(DataManager.Instance.GetImages(avatarURL, botPlayers[i].GetComponent<Image>()));

            // Load random name
            string playerName = BotManager.Instance.botUserName[randomNames[i]];
            botPlayersName[i].text = playerName;

            // Load random balance
            int randomBalance = UnityEngine.Random.Range(0, ExtensionMethods.BotPlayerBalance.Length);
            botPlayersCoins[i].text = ExtensionMethods.BotPlayerBalance[randomBalance].ToString();
        }
    }

    #endregion

    #region Sounds


    private void CheckSound()
    {
        soundImg.sprite = DataManager.Instance.GetSound() == 0 ? soundonSprite : soundoffSprite;
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

    /*public void PlayRocketThrustSound()
    {
        if (!rocketThrustSound.isPlaying && DataManager.Instance.GetSound() == 0)
        {
            rocketThrustSound.Play();
        }
    }

    public void StopRocketThrustSound()
    {
        if (rocketThrustSound.isPlaying && DataManager.Instance.GetSound() == 0)
        {
            rocketThrustSound.Stop();
        }
    }

    public void PlayBlastSound()
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            blastSound.Play();
        }
    }
    */

    #endregion

    #region Socket

    public void SetWinData(string winListData)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PointList", winListData);
        TestSocketIO.Instace.SetWinData(TestSocketIO.Instace.roomid, obj);
    }

    #endregion
}
