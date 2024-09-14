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
    public float multiplierSpeed = 1f;
    public Text totalBetText;
    public Text myBetText;
    public Text rightCashOutText;
    public Text leftCashOutText;
    public float playerWinAmount;

    [Header("--- Fake Bot ---")] 
    public float botBettingDuration = 5f;
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
        BottomChipAnim(1);
        SetChipBtnInteractable(false);
        StartGamePlay();
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

    private IEnumerator GameLoop()
    {
        float elapsedTime = 0f;
        // Making line and rocket visible 
        lineCanvas.SetActive(true);
        rocketCanvas.SetActive(true);
        rightCashOutButton.interactable = true;
        leftCashOutButton.interactable = true;
        rightCashOutButton.gameObject.SetActive(true);
        leftCashOutButton.gameObject.SetActive(true);
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
    }
    
    private IEnumerator UpdateMultiplierText()
    {
        float elapsedTime = 0f;

        while (isGameRunning)
        {
            elapsedTime += Time.deltaTime * multiplierSpeed;
            multiplier = elapsedTime;
            multiplierText.text = multiplier.ToString("F2") + "X";
            UpdateCashOutText();
            yield return null;
        }
    }
    
    private IEnumerator BlinkMultiplier()
    {
        yield return multiplierText.DOFade(0f, 0.2f).SetLoops(6, LoopType.Yoyo).WaitForCompletion();
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
        totalBetText.text = "0";
        totalBetText.text = "0";
        myBetText.text = "0";
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
        crashTime = UnityEngine.Random.Range(minCrashTime, maxCrashTime);
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
            historyData = "0.5X,20X,5X,15X,2.2X,6X,5.63X,6.66X,5.5X,5X";
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

    private IEnumerator BettingTimer()
    {
        float remainingTime = fixTimerValue;
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerTxt.text = seconds.ToString("D2");
            yield return null;
        }
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
    
    private void CalculateBetAreaBounds()
    {
        float betAreaWidth = Mathf.Abs(maxBetAreaX - minBetAreaX);
        float betAreaHeight = Mathf.Abs(maxBetAreaY - minBetAreaY);
        betAreaSize = new Vector3(betAreaWidth, betAreaHeight, 1f);
        betAreaCenter = new Vector3((minBetAreaX + maxBetAreaX) / 2f, (minBetAreaY + maxBetAreaY) / 2f, 0f);
    }

    public void BetButtonClick()
    {
        bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
        if (isMoneyAv == false)
        {
            SoundManager.Instance.ButtonClick();
            OpenErrorScreen();
            return;
        }

        SoundManager.Instance.ThreeBetSound();
        DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,"Aviator-Bet-" + DataManager.Instance.gameId, "game", 2);
        
        betAmount += chipPrice[selectChipNo];
        totalBetAmount += chipPrice[selectChipNo];
        totalBetText.text = totalBetAmount.ToString("F2");
        myBetText.text = betAmount.ToString("F2");
        
        Vector3 rPos = GetRandomPositionWithinBettingArea();
        GameObject chipGen = Instantiate(chipObj, bettingArea.transform);
        chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
        chipGen.transform.position = avatarImg.transform.position;
        betChipList.Add(chipGen);
        ChipGenerate(chipGen, rPos);
        
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
            float winReward = investPrice - betAmount;
            float adminCommission = DataManager.Instance.adminPercentage / 100f;
            float winAmount = winReward - (winReward * adminCommission);
            playerWinAmount = betAmount + winAmount;

            rightCashOutButton.interactable = false;
            leftCashOutButton.interactable = false;
        
            if (playerWinAmount != 0)
            {
                SoundManager.Instance.CasinoWinSound();
                Debug.Log("Player Win Amount   =  " + playerWinAmount);
                DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId, "aviator-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommission), 1);
            }
        }
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
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL") , avatarImg);
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
                totalBetText.text = totalBetAmount.ToString("F2");
            }

            yield return new WaitForSeconds(botBettingDuration);
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
