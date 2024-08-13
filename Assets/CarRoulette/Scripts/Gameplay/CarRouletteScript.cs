using System;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CarRouletteScript : MonoBehaviour
{
    [Header("---PlayerDetais---")]
    public Image avatarImg;
    public Text balanceTxt;
    public Text userNameTxt;
    public Transform botSpawnLocation;
    public bool isEnterTheCarRoulette = false;
    public bool isGameRunning;
    public bool isAdmin;
    public bool isStopBet;
    public Text timerTxt;
    public float fixTimeSet;
    float secondCount = 0;
    public int timerValue = 0;
    public float totalCurrentInvest = 0;
    float interval = 0.5f;
    float nextTime = 0;
    public bool isActive;
    private bool called = false;
    private bool _isTimeSet;
    public int noGen;
    public int winNumber;
    private bool isStoppingOnWinNumber = false;
    private bool _isClickAvailable;

    [Header("--- GamePlay Data---")]
    public List<int> easyNumbers;
    public List<int> mediumNumbers;
    public List<int> hardNumbers;
    public int selectChipNo;
    public GameObject[] chipBtn;
    public float[] chipPrice;
    public float downValue;
    public float upValue;
    public BoxCollider2D[] spawnBox;
    public Transform[] spawnLocation;
    public Sprite[] chipsSprite;
    public float[] betPriceValue;
    public Text[] betText;
    public GameObject chipObj;
    public LoadFakeBotPlayers botPlayers;

    [Header("--- Info Screen ---")]
    public GameObject infoScreenObj;
    public GameObject[] innerInfoObj;
    public Button leftButton;
    public Button rightButton;
    public GameObject startBettingObj;
    public Animator startBetAnim;
    public GameObject stopBettingObj;
    public Animator stopBetAnim;
    public GameObject[] winParticles;
    public Text winAnimationTxt;

    [Header("--- History Recorder ---")]
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject PounCarrier;
    public List<GameObject> Pouns;
    public List<int> winList;
    private float _transformPosition = 175.3074f;

    [Header("--- Menu Screen ---")]
    public GameObject menuScreenObj;
    [Header("--- Player List---")]
    public GameObject playerListObj;

    [Header("--- Wait New Round Screen ---")]
    public GameObject waitNextRoundScreenObj;

    [Header("--- Error Screen ---")]
    public GameObject errorScreenObj;

    [Header("--- Shop Prefab ---")]
    public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    public static CarRouletteScript Instance;
    public List<GameObject> selectorObjects;
    public float delayBetweenSelectors = 0.5f;
    public float spinDuration;
    public List<GameObject> genChipList_Lambo = new List<GameObject>();
    public List<GameObject> genChipList_Bmw = new List<GameObject>();
    public List<GameObject> genChipList_Benz = new List<GameObject>();
    public List<GameObject> genChipList_Jaguar = new List<GameObject>();
    public List<GameObject> genChipList_Land = new List<GameObject>();
    public List<GameObject> genChipList_Nissan = new List<GameObject>();
    public List<GameObject> genChipList_Mazad = new List<GameObject>();
    public List<GameObject> genChipList_Volks = new List<GameObject>();

    private Queue<GameObject> selectorPool;
    public List<GameObject> activeSelectors;

    [Header("--- Sounds ---")]
    public Image soundImg;
    public Image vibrationImg;
    public Image musicImg;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;
    private bool isSelectionRunning;
    private int currentIndex;
    private bool _isTimesUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateNameBalance();
    }

    private void Start()
    {
        InitializeSelectors();
        GetProfileImage();
        NewPlayerEnter();
        HistoryLoader(DataManager.Instance.historyRecord);
        ChipButtonClick(0);
        CheckSound();
    }

    private void FixedUpdate()
    {
        if (timerValue == 0 && isEnterTheCarRoulette == false && waitNextRoundScreenObj.activeSelf == false)
        {
            isEnterTheCarRoulette = true;
            _isTimeSet = false;
            StartCoroutine(StopBettingOff());
        }
        else if (!timerTxt.text.Equals("0"))
        {
            secondCount -= Time.deltaTime;
            timerValue = ((int)secondCount);
            timerTxt.text = timerValue.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ReGenerateBoard();
        //    rouletteBoardObj.SetActive(true);
        //}

        if (isAdmin && !_isTimeSet)
        {
            if (timerValue == 5)
            {
                AdjustTime();
                _isTimeSet = true;
                print("---------------------------Time is sent-------------------------");
            }
        }
        if (!isActive) return;
        if (Time.time >= nextTime)
        {
            //CallBetCoins();
            nextTime += interval;
        }
    }


    private void GetProfileImage()
    {
        SoundManager.Instance.StopBackgroundMusic();
        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL"), avatarImg);
    }

    public void UpdateNameBalance()
    {
        userNameTxt.text = DataManager.Instance.playerData.firstName.ToString();
        balanceTxt.text = "₹ " + DataManager.Instance.playerData.balance.ToString();
    }

    public void OpenBotScreen()
    {
        botPlayers.LoadBotPlayers();
    }

    #region New Player Entry Time Maintain

    public void NewPlayerEnter()
    {
        print("______________________New player entered is called_________________________________");
        if (DataManager.Instance.joinPlayerDatas.Count < TestSocketIO.Instace.rouletteRequirePlayer) return;
        CreateAdmin();
        if (DataManager.Instance.joinPlayerDatas.Count == 1 && isAdmin)
        {
            isEnterTheCarRoulette = true;
            //RestartTimer();
            StartCoroutine(StartBettingOff());
        }
        else
        {
            if (isAdmin) return;

            if (isGameRunning) return;
            isEnterTheCarRoulette = true;
            print("Enter The New Player Time");
            waitNextRoundScreenObj.SetActive(true);

            //FindDataAdminRouletee();
            //Get Details Admin
        }
    }
    #endregion

    private void InitializeSelectors()
    {
        selectorPool = new Queue<GameObject>();
        activeSelectors = new List<GameObject>();

        if (selectorObjects.Count == 0)
        {
            Debug.LogError("No selector objects assigned. Please assign selector objects in the inspector.");
            return;
        }

        /*for (int i = 0; i < selectorObjects.Count; i++)
        {
            if (i < selectorObjects.Count)
            {
                GameObject selectorObject = selectorObjects[i];
                selectorObject.SetActive(false);
                selectorPool.Enqueue(selectorObject);
            }
            else
            {
                Debug.LogWarning("Not enough selector objects assigned for the specified number of selectors. " +
                    "Make sure to assign enough selector objects in the inspector.");
                break;
            }
        }*/

        foreach (GameObject selectorObject in selectorObjects)
        {
          //  selectorObject.SetActive(false);
            selectorPool.Enqueue(selectorObject);
        }
    }
    public List<GameObject> rouletteObjects;
    public float delay = 1;
    public Sprite[] images;
    private Coroutine rotateCoroutine;

    IEnumerator RotateRoulette(int yourDesiredIndex)
    {
        int startIndex = yourDesiredIndex;  // Set this to the index you want to start from

        while (true)
        {
            foreach (GameObject obj in rouletteObjects)
            {
                obj.SetActive(false);
            }

            int index = startIndex % rouletteObjects.Count;
            rouletteObjects[index].SetActive(true);

            for (int i = 0; i < 5; i++)
            {
                int index1;

                if (i == 0)
                {
                    index1 = (startIndex + i) % rouletteObjects.Count;
                }
                else if (i == 1)
                {
                    index1 = (startIndex + rouletteObjects.Count - i) % rouletteObjects.Count;
                }
                else
                {
                    index1 = (startIndex + rouletteObjects.Count - i) % rouletteObjects.Count;
                }

                rouletteObjects[index1].SetActive(true);

                if (i < images.Length)
                {
                    Image imgComponent = rouletteObjects[index1].GetComponent<Image>();
                    if (imgComponent != null)
                    {
                        imgComponent.sprite = images[i];
                    }
                }
            }

            yield return new WaitForSeconds(delay);

            startIndex++;
            if (startIndex >= rouletteObjects.Count)
            {
                startIndex = 0;
            }
        }


    }

    public void ResetRoulette()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }

        foreach (GameObject obj in rouletteObjects)
        {
            obj.SetActive(false);
        }
        delay = 1f;
    }
    public void StopAnimation()
    {

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }


        int startIndex = 5;

        foreach (GameObject obj in rouletteObjects)
        {
            obj.SetActive(false);
        }

        if (rouletteObjects.Count > 0)
        {
            rouletteObjects[startIndex].SetActive(true);
        }
    }
    public int SelectRandomNumber(int mode)
    {
        int selectedNumber = 0;
        List<int> numberList;

        switch (mode)
        {
            case 1:
                numberList = easyNumbers;
                break;
            case 2:
                numberList = mediumNumbers;
                break;
            case 3:
                numberList = hardNumbers;
                break;
            default:
                Debug.LogError("Invalid game mode.");
                return selectedNumber;
        }

        if (numberList.Count > 0)
        {
            int randomIndex = Random.Range(0, numberList.Count);
            selectedNumber = numberList[randomIndex];
        }
        else
        {
            Debug.LogWarning("Number list is empty.");
        }

        return selectedNumber;
    }

    public void StartSelection()
    {
        print("Start is clicked");
        if (isSelectionRunning)
            return;

        isSelectionRunning = true;
        currentIndex = 0;

        StartCoroutine(ActivateSelectors());
    }

    public void StopSelection()
    {
        print("Stop is clicked");
        SoundManager.Instance.CarStopSound();
        if (!isSelectionRunning)
            return;

        isSelectionRunning = false;
        _isTimesUp = false;

        foreach (GameObject selectorObject in activeSelectors)
        {
            SymbolScript symbolScript = selectorObject.GetComponent<SymbolScript>();
            if (symbolScript != null)
            {
                CarNames car = symbolScript.Car;
                // Use the car variable as needed
                CalculateWinAmount(car);
                print("This is called value -> " + car);
            }

            /*selectorPool.Enqueue(selectorObject);
            selectorObject.SetActive(false);*/
            selectorPool.Enqueue(selectorObject);
            StartCoroutine(ClearSelectedObject(selectorObject));
        }

        activeSelectors.Clear();
    }

    private IEnumerator ClearSelectedObject(GameObject selectorObject)
    {
        int carPosition = selectorObject.GetComponent<SymbolScript>().carPosition;
        startAniIndex = (carPosition + 1) % rouletteObjects.Count;

        rouletteObjects[selectorObject.GetComponent<SymbolScript>().carPosition].GetComponent<Image>().sprite = images[0];
        rouletteObjects[selectorObject.GetComponent<SymbolScript>().carPosition].SetActive(true);
        yield return new WaitForSeconds(7f);
       // selectorObject.SetActive(false);
        ResetData();
    }

    public float delayGlowRing = 1f;
    private IEnumerator ActivateSelectors()
    {
        print("Activated Selector");
        StartCoroutine(RingAnimation());

        while (isSelectionRunning)
        {
            if (selectorPool.Count > 0)
            {
                GameObject selectorObject = selectorPool.Dequeue();
                selectorObject.SetActive(true);
                activeSelectors.Add(selectorObject);
            }
            else
            {
                Debug.LogWarning("Selector pool is empty. Consider increasing the number of selectors or implementing dynamic object pooling.");
                break;
            }

            if (_isTimesUp)
            {
                Debug.Log("IS TIME => " + _isTimeSet);
                //After time is up then it will select the index number from activeSelector list.
                if (activeSelectors.Select(selectorObject => selectorObject.GetComponent<SymbolScript>()).Any(symbolScript => symbolScript != null && symbolScript.carPosition == winNumber))
                {
                    Debug.Log("DELEAY  = >  " + delay);
                    yield return new WaitForSeconds(delay);
                    ResetRoulette();
                    StopSelection();

                    //StopAnimation();
                }
            }

            yield return new WaitForSeconds(delay);
            // yield return new WaitForSeconds(delayBetweenSelectors);

            if (activeSelectors.Count > 0)
            {
                GameObject selectorObject = activeSelectors[0];
                activeSelectors.RemoveAt(0);
              //  selectorObject.SetActive(false);
                selectorPool.Enqueue(selectorObject);
            }

            currentIndex = (currentIndex + 1) % selectorObjects.Count;
        }
    }
    public int startAniIndex = 0;

    private IEnumerator RingAnimation()
    {
        rotateCoroutine = StartCoroutine(RotateRoulette(startAniIndex));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ChangeDelay(0.01f, 4f));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(ChangeDelay(0.3f, 4f));
    }
    IEnumerator ChangeDelay(float targetDelay, float duration)
    {
        float startDelay = delay;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            delay = Mathf.Lerp(startDelay, targetDelay, elapsed / duration);
            yield return null;
        }

        delay = targetDelay;
    }

    public IEnumerator StartSpinning()
    {
        SoundManager.Instance.CarStartSound();
        isStoppingOnWinNumber = true;
        StartSelection();

        yield return new WaitForSeconds(spinDuration);
        //StopSelection();
        _isTimesUp = true;
    }

    #region Betting
    public void GameBetBoxClick(int no)
    {
        if (!_isClickAvailable) return;
        switch (no)
        {
            case 1:// lamborghini
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[0]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[0]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[0] += chipPrice[selectChipNo];
                    genChipList_Lambo.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 2:// BMW
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[1]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[1]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[1] += chipPrice[selectChipNo];
                    genChipList_Bmw.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 3:// Benz
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[2]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[2]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[2] += chipPrice[selectChipNo];
                    genChipList_Benz.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 4:// Jaguar
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[3]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[3]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[3] += chipPrice[selectChipNo];
                    genChipList_Jaguar.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 5:// LandRover
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[4]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[4]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[4] += chipPrice[selectChipNo];
                    genChipList_Land.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 6:// Nissan
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[5]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[5]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[5] += chipPrice[selectChipNo];
                    genChipList_Nissan.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 7:// Mazda
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[6]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[6]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[6] += chipPrice[selectChipNo];
                    genChipList_Mazad.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 8:// volkswagen
                {
                    bool isMoneyAv = CheckMoney(chipPrice[selectChipNo]);
                    if (isMoneyAv == false)
                    {
                        SoundManager.Instance.ButtonClick();
                        OpenErrorScreen();
                        return;
                    }

                    SoundManager.Instance.ThreeBetSound();
                    DataManager.Instance.DebitAmount(((float)(chipPrice[selectChipNo])).ToString(), DataManager.Instance.gameId,
                        "CarRoulette-Bet-" + DataManager.Instance.gameId, "game", 2);

                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[7]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[7]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = avatarImg.transform.position;
                    betPriceValue[7] += chipPrice[selectChipNo];
                    genChipList_Volks.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }

        }
        UpdateBoardPrice();
        SendCarRouletteBet(no, selectChipNo);
    }

    private Vector3 GetRandomPosInBoxCollider2D(BoxCollider2D boxCollider)
    {
        Bounds bounds = boxCollider.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 90f);
    }

    public void ChipGenerate(GameObject chip, Vector3 endPos)
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

    void UpdateBoardPrice()
    {
        for (int i = 0; i < betText.Length; i++)
        {
            betText[i].text = betPriceValue[i].ToString(CultureInfo.InvariantCulture);
        }
    }



    public void ChipButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        ChipAnimMaintain(no);
    }

    void ChipAnimMaintain(int no)
    {
        selectChipNo = no;
        for (int i = 0; i < chipBtn.Length; i++)
        {
            if (i == no)
            {
                chipBtn[i].transform.DOMoveY(upValue, 0.05f);
            }
            else
            {
                chipBtn[i].transform.DOMoveY(downValue, 0.05f);
            }
        }
    }

    public bool CheckMoney(float money)
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

    #endregion

    #region Winning Logic 
    public List<GameObject> winGlowObject;
    private void CalculateWinAmount(CarNames car)
    {
        SoundManager.Instance.CarWinSound();
        float winAmount = 0;

        if (car == CarNames.Lamborghini || car == CarNames.Bmw || car == CarNames.Benz || car == CarNames.Jaguar)
        {
            winAmount = betPriceValue[(int)car] * 20;
        }
        else
        {
            winAmount = betPriceValue[(int)car] * 5;
        }

        // Use the car and winAmount variables as needed
        print("Car Name: " + car + ", Win Amount: " + winAmount);
        float adminPercentage = DataManager.Instance.adminPercentage;

        float winnningAmount = winAmount;
        float adminCommssion = (adminPercentage / 100);
        float playerWinAmount = winnningAmount - (winnningAmount * adminCommssion);
        switch (car)
        {
            case CarNames.Lamborghini:
                winParticles[0].SetActive(true);
                winGlowObject[0].SetActive(true);
                break;
            case CarNames.Bmw:
                winParticles[1].SetActive(true);
                winGlowObject[1].SetActive(true);
                break;
            case CarNames.Benz:
                winParticles[2].SetActive(true);
                winGlowObject[2].SetActive(true);
                break;
            case CarNames.Jaguar:
                winParticles[3].SetActive(true);
                winGlowObject[3].SetActive(true);
                break;
            case CarNames.LandRover:
                winParticles[4].SetActive(true);
                winGlowObject[4].SetActive(true);
                break;
            case CarNames.Nissan:
                winParticles[5].SetActive(true);
                winGlowObject[5].SetActive(true);
                break;
            case CarNames.Mazda:
                winParticles[6].SetActive(true);
                winGlowObject[6].SetActive(true);
                break;
            case CarNames.Volkswagen:
                winParticles[7].SetActive(true);
                winGlowObject[7].SetActive(true);
                break;
        }

        if (playerWinAmount != 0)
        {
            SoundManager.Instance.CasinoWinSound();
            winAnimationTxt.gameObject.SetActive(true);
            winAnimationTxt.text = "+" + playerWinAmount;
            Invoke(nameof(WinAmountTextOff), 1.5f);

            DataManager.Instance.AddAmount((float)(playerWinAmount), TestSocketIO.Instace.roomid, "CarRoulette-Win-" + TestSocketIO.Instace.roomid, "won", (float)(adminCommssion), noGen);
        }
        UpdateHistoryRecord(winNumber);
        Invoke(nameof(WinAnimationOff), 5.5f);
    }

    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);
    }

    public void WinAnimationOff()
    {
        foreach (GameObject particle in winParticles)
        {
            particle.gameObject.SetActive(false);
        }
        foreach (GameObject particle in winGlowObject)
        {
            particle.gameObject.SetActive(false);
        }
    }

    #endregion

    private void Update()
    {

    }

    #region Menu Screen


    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    public void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    public void HomeButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        TestSocketIO.Instace.LeaveRoom();
        SoundManager.Instance.StartBackgroundMusic();
        SceneManager.LoadScene("Main");
    }

    public void InfoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenInfoScreenObj();
    }

    public void StoreButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Instantiate(shopPrefab, shopPrefabParent.transform);
    }
    public void CloseMenuScreen()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
    }



    #endregion


    #region Error Screen
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
        Instantiate(shopPrefab, shopPrefabParent.transform);
        errorScreenObj.SetActive(false);
    }


    #endregion

    #region Info Screen

    int infoScreenNo = 0;
    public void OpenInfoScreenObj()
    {
        infoScreenObj.SetActive(true);
        infoScreenNo = 0;
        UpdateInfo();
    }

    public void LeftButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenNo--;
        UpdateInfo();
    }

    public void RightButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenNo++;
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (infoScreenNo == 0)
        {
            leftButton.interactable = false;
            rightButton.interactable = true;
        }
        else if (infoScreenNo == innerInfoObj.Length - 1)
        {
            leftButton.interactable = true;
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
        }

        for (int i = 0; i < innerInfoObj.Length; i++)
        {
            if (i == infoScreenNo)
            {
                innerInfoObj[i].SetActive(true);
            }
            else
            {
                innerInfoObj[i].SetActive(false);
            }
        }


    }


    public void BackToInfoButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        infoScreenObj.SetActive(false);
    }

    #endregion

    #region Admin Maintain

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
            /*SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);*/
        }
    }

    public void SetRoomData()
    {
        JSONObject obj = new JSONObject();
        // obj.AddField("DeckNo", 2);
        obj.AddField("DeckNo", winNumber);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 4);
        obj.AddField("WinList", DataManager.Instance.historyRecord);
        obj.AddField("gameStatus", DataManager.Instance.rouletteGameStatus);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int no, string data)
    {
        noGen = no;
        winNumber = noGen;
        if (isAdmin) return;
        if (waitNextRoundScreenObj.activeSelf)
        {
            isEnterTheCarRoulette = true;
            waitNextRoundScreenObj.SetActive(false);
            timerValue = (int)fixTimeSet;
            secondCount = timerValue;
            SoundManager.Instance.CasinoTurnSound();
            DataManager.Instance.UserTurnVibrate();
            StartCoroutine(StartBettingOff());
            //CenterToAddUser();
        }

        HistoryLoader(data);
    }

    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {
        //DataManager.Instance.joinPlayerDatas.Clear();
        /*for (int i = 0; i < DataManager.Instance.leaveUpdatePlayerDatas.Count; i++)
        {
            DataManager.Instance.joinPlayerDatas.Add(DataManager.Instance.leaveUpdatePlayerDatas[i]);
        }*/
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObj.activeSelf)
            {
                //RoundGenerate();
                StartCoroutine(StartBettingOff());
                if (waitNextRoundScreenObj.activeSelf)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
            }
        }
        else
        {
            isAdmin = false;
        }


    }

    public void HistoryLoader(string data)
    {
        if (data != "")
        {
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        }

        int childCount = PounCarrier.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(PounCarrier.transform.GetChild(i).gameObject);
        }

        foreach (var t in winList)
        {
            Instantiate(Pouns[t], PounCarrier.transform);
        }

        //UpdateScrollRect();
        ScrollToLast();
    }

    private void UpdateScrollRect()
    {
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, _transformPosition);
    }

    public void ScrollToLast()
    {
        float scrollPosition = 0f;
        scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, scrollPosition);
    }

    public int LogoSelector(int num)
    {
        return num switch
        {
            0 => 4,
            7 => 4,
            11 => 4,
            1 => 1,
            14 => 1,
            19 => 1,
            2 => 3,
            16 => 3,
            21 => 3,
            3 => 5,
            12 => 5,
            20 => 5,
            4 => 6,
            8 => 6,
            17 => 6,
            5 => 7,
            13 => 7,
            18 => 7,
            6 => 0,
            9 => 0,
            23 => 0,
            10 => 2,
            15 => 2,
            22 => 2,
            _ => num
        };
    }

    private void UpdateHistoryRecord(int num)
    {
        int carSymbol = LogoSelector(num);
        if (!isAdmin) return;
        winList.RemoveAt(0);
        winList.Add(carSymbol);

        int childCount = PounCarrier.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(PounCarrier.transform.GetChild(i).gameObject);
        }

        foreach (var t in winList)
        {
            Instantiate(Pouns[t], PounCarrier.transform);
        }
        //UpdateScrollRect();
        ScrollToLast();
        DataManager.Instance.historyRecord = string.Join(",", winList.Select(x => x.ToString()).ToArray());
        //SetWinData(DataManager.Instance.winList);
    }

    public void SendCarRouletteBet(int boxNo, int chipNo)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("boxNo", boxNo);
        obj.AddField("chipNo", chipNo);
        TestSocketIO.Instace.Senddata("CarRouletteSendBetData", obj);
    }

    public void GetCarRouletteBet(int boxNo, int chipNo)
    {
        switch (boxNo)
        {
            case 1:// lamborghini
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[0]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[0]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[0] += chipPrice[chipNo];
                    genChipList_Lambo.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 2:// BMW
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[1]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[1]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[1] += chipPrice[chipNo];
                    genChipList_Bmw.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 3:// Benz
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[2]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[2]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[2] += chipPrice[chipNo];
                    genChipList_Benz.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 4:// Jaguar
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[3]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[3]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[3] += chipPrice[chipNo];
                    genChipList_Jaguar.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 5:// LandRover
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[4]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[4]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[4] += chipPrice[chipNo];
                    genChipList_Land.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 6:// Nissan
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[5]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[5]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[5] += chipPrice[chipNo];
                    genChipList_Nissan.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 7:// Mazda
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[6]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[6]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[6] += chipPrice[chipNo];
                    genChipList_Mazad.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
            case 8:// volkswagen
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 rPos = GetRandomPosInBoxCollider2D(spawnBox[7]);
                    GameObject chipGen = Instantiate(chipObj, spawnLocation[7]);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
                    chipGen.transform.position = botSpawnLocation.position;
                    betPriceValue[7] += chipPrice[chipNo];
                    genChipList_Volks.Add(chipGen);
                    ChipGenerate(chipGen, rPos);
                    break;
                }
        }
        UpdateBoardPrice();
    }


    public void FindDataAdminRouletee()
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);

        TestSocketIO.Instace.Senddata("FindDataCarRouletteAdmin", obj);
    }

    public void SendAdminDataPlayer(string playerID)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("AdminPlayerID", DataManager.Instance.playerData._id);
        obj.AddField("ReceivePlayerID", playerID);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("Time", timerValue);
        obj.AddField("RouletteNumber", noGen);
        TestSocketIO.Instace.Senddata("SendAdminCarRouleteeData", obj);
    }

    public void AdjustTime()
    {
        JSONObject obj = new JSONObject();
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("Time", timerValue);
        TestSocketIO.Instace.Senddata("SendAdminCarRouleteeData", obj);
    }

    public void GetAdminDataPlayer(int time, int no)
    {
        //isEnterTheRoulette = true;
        // waitNextRoundScreenObj.SetActive(false);
        timerValue = time;
        secondCount = timerValue;
        //noGen = no;
        // CenterToAddUser();
        //aaa
    }


    /*void CenterToAddUser()
    {
        isStopBet = true;
        totalCurrentInvest = 0;
        for (int i = 0; i < blackPanelObj.Count; i++)
        {
            blackPanelObj[i].SetActive(false);
        }
        for (int i = 0; i < btnParentPanelObj.Count; i++)
        {
            for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
            {
                Destroy(btnParentPanelObj[i].transform.GetChild(j).transform.gameObject);
            }
        }

        rouleteeBetsBefore.Clear();
        for (int i = 0; i < rouleteeBets.Count; i++)
        {
            RouleteeBetClass rouleteeBet = rouleteeBets[i];
            rouleteeBetsBefore.Add(rouleteeBet);
        }
        rouleteeBets.Clear();



        isStopBet = false;
        isEnterTheRoulette = false;
        //timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        //secondCount = fixTimeSet;
    }*/

    #endregion

    public void GiveUserData()
    {
        print("______________________New game is called_________________________________");
        //RestartTimer();
        StartCoroutine(StartBettingOff());
    }


    IEnumerator StartBettingOff()
    {
        print("______________________start betting is called_________________________________");
        foreach (GameObject obj in rouletteObjects)
        {
            obj.SetActive(false);
        }
        isGameRunning = true;
        if (isAdmin)
        {
            winNumber = SelectRandomNumber(DataManager.Instance.gameComplexity);
            DataManager.Instance.rouletteGameStatus = true;
            SetRoomData();
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }
        SoundManager.Instance.CasinoTurnSound();
        isStopBet = true;
        /*for (int i = 0; i < blackPanelObj.Count; i++)
        {
            blackPanelObj[i].SetActive(false);
        }
        for (int i = 0; i < btnParentPanelObj.Count; i++)
        {
            for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
            {
                Destroy(btnParentPanelObj[i].transform.GetChild(j).transform.gameObject);
            }
        }*/
        startBettingObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Vector3 customZoomScale = new Vector3(5.0f, 5.0f, 5.0f);
        StartAnimationPlay(objects, customZoomScale, 0.1f, 0.009f);

        //TurnOnButtons();
        yield return new WaitForSeconds(2.5f);

        //rouleteeBetsBefore.Clear();


        /*for (int i = 0; i < rouleteeBets.Count; i++)
        {
            RouleteeBetClass rouleteeBet = rouleteeBets[i];
            rouleteeBetsBefore.Add(rouleteeBet);
        }
        
        if (rouleteeBetsBefore.Count > 0)
        {
            rebetButton.interactable = true;
        }
        rouleteeBets.Clear();*/
        //startBetAnim.SetInteger("FirstClipComplete", 1);
        yield return new WaitForSeconds(1f);
        startBettingObj.SetActive(false);
        betAnimationONOff(true);
        isActive = true;
        RestartTimer();
        _isClickAvailable = true;
    }

    IEnumerator StopBettingOff()
    {
        _isClickAvailable = false;
        print("______________________Stop betting is called_________________________________");
        isActive = false;
        if (isAdmin)
        {
            DataManager.Instance.rouletteGameStatus = false;
            TestSocketIO.Instace.GetCarBetData();
        }
        stopBettingObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Vector3 customZoomScale = new Vector3(4.0f, 4.0f, 4.0f);
        StartAnimationPlay(stopObjects, customZoomScale, 0.1f, 0.1f);
        //ClearAllChips();
        //TurnOffButtons();
        yield return new WaitForSeconds(3f);
        // stopBetAnim.SetInteger("FirstClipComplete", 1);
        yield return new WaitForSeconds(1f);
        stopBettingObj.SetActive(false);
        betAnimationONOff(false);
        //ReGenerateBoard();
        // rouletteBoardObj.SetActive(true);
        StartCoroutine(StartSpinning());
    }
    public List<GameObject> objects;  // List of objects to animate

    public void StartAnimationPlay(List<GameObject> objects, Vector3 zoomScale, float zoomDuration, float delayBetweenAnimations)
    {
        Sequence sequence = DOTween.Sequence();

        foreach (GameObject obj in objects)
        {
            sequence.AppendCallback(() => obj.SetActive(true));
            sequence.Append(obj.transform.DOScale(zoomScale, zoomDuration).SetEase(Ease.OutQuad));
            sequence.Append(obj.transform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.OutQuad));
            sequence.AppendInterval(delayBetweenAnimations);
        }

        // Play the sequence
        sequence.Play();
    }
    public List<GameObject> stopObjects;
    public void StopAnimationPlay()
    {
       /* Sequence sequence = DOTween.Sequence();

        foreach (GameObject obj in stopObjects)
        {
            sequence.AppendCallback(() => obj.SetActive(true));
            sequence.Append(obj.transform.DOScale(zoomScale, zoomDuration).SetEase(Ease.OutQuad));
            sequence.Append(obj.transform.DOScale(Vector3.one, zoomDuration).SetEase(Ease.OutQuad));
            sequence.AppendInterval(delayBetweenAnimations);
        }

        // Play the sequence
        sequence.Play();*/
    }

    public void betAnimationONOff(bool isStart)
    {
        if (isStart)
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);

            }

        }
        else
        {
            for (int i = 0; i < stopObjects.Count; i++)
            {
                stopObjects[i].SetActive(false);
            }
        }
    }
    public void ResetData()
    {
        // Reset betPriceValue array
        for (int i = 0; i < betPriceValue.Length; i++)
        {
            betPriceValue[i] = 0f;
        }

        // Clear and destroy genChipList_Lambo
        foreach (GameObject chipObject in genChipList_Lambo)
        {
            Destroy(chipObject);
        }
        genChipList_Lambo.Clear();

        // Clear and destroy genChipList_Bmw
        foreach (GameObject chipObject in genChipList_Bmw)
        {
            Destroy(chipObject);
        }
        genChipList_Bmw.Clear();

        // Clear and destroy genChipList_Benz
        foreach (GameObject chipObject in genChipList_Benz)
        {
            Destroy(chipObject);
        }
        genChipList_Benz.Clear();

        // Clear and destroy genChipList_Jaguar
        foreach (GameObject chipObject in genChipList_Jaguar)
        {
            Destroy(chipObject);
        }
        genChipList_Jaguar.Clear();

        // Clear and destroy genChipList_Land
        foreach (GameObject chipObject in genChipList_Land)
        {
            Destroy(chipObject);
        }
        genChipList_Land.Clear();

        // Clear and destroy genChipList_Nissan
        foreach (GameObject chipObject in genChipList_Nissan)
        {
            Destroy(chipObject);
        }
        genChipList_Nissan.Clear();

        // Clear and destroy genChipList_Mazad
        foreach (GameObject chipObject in genChipList_Mazad)
        {
            Destroy(chipObject);
        }
        genChipList_Mazad.Clear();

        // Clear and destroy genChipList_Volks
        foreach (GameObject chipObject in genChipList_Volks)
        {
            Destroy(chipObject);
        }
        genChipList_Volks.Clear();
        UpdateBoardPrice();
        GiveUserData();
    }

    public void RestartTimer()
    {
        //for (int i = 0; i < blackPanelObj.Count; i++)
        //{
        //    blackPanelObj[i].SetActive(false);
        //}
        //for (int i = 0; i < btnParentPanelObj.Count; i++)
        //{
        //    for (int j = 0; j < btnParentPanelObj[i].transform.childCount; j++)
        //    {
        //        Destroy(btnParentPanelObj[i].transform.GetChild(j));
        //    }
        //}

        isStopBet = false;
        isEnterTheCarRoulette = false;
        timerValue = ((int)fixTimeSet);
        timerTxt.text = timerValue.ToString();
        secondCount = fixTimeSet;
    }

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


    #endregion


}
