
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp;
using Random = UnityEngine.Random;


public class SpinAndWinManager : MonoBehaviour
{

    [System.Serializable]
    public class SpinAndWinBetClass
    {
        public int no;
        public float price;
        public GameObject betObj;
    }

    public Image avatarImg;

    public static SpinAndWinManager Instance;


    public GameObject dragonAnim;
    public GameObject tigerAnim;

    public GameObject winParticles;
    public Text winAnimationTxt;

    [Header("--- Menu Screen ---")] public Text userNameTxt;
    public Text balanceTxt;

    [Header("--- Menu Screen ---")] public GameObject menuScreenObj;

    [Header("--- Rule Screen ---")] public GameObject ruleScreenObj;

    [Header("--- Error Screen ---")] public GameObject errorScreenObj;

    [Header("--- Prefab ---")] public GameObject shopPrefab;
    public GameObject shopPrefabParent;

    public GameObject chip;

    [Header("--- History Coins ---")]
    public GameObject CoinsCarrier;
    public GameObject TigerCoin;
    public GameObject DragonCoin;
    public GameObject TieCoin;

    [Header("--- Chip Generate Position ---")]
    public float minDragonX;

    public float maxDragonX;
    public float minDragonY;
    public float maxDragonY;

    public float minTieX;
    public float maxTieX;
    public float minTieY;
    public float maxTieY;


    public float minTigerX;
    public float maxTigerX;
    public float minTigerY;
    public float maxTigerY;

    public float downValue;
    public float upValue;

    [Header("--- Start Bet ---")]
    public GameObject startBetObj;
    public GameObject stopBetObj;

    [Header("--- Game Play Maintain ---")]
    public GameObject waitNextRoundScreenObj;

    [Header("--- PlayersList ---")]
    public SpinAndWinPlayer players1;
    public SpinAndWinPlayer players2;
    public SpinAndWinPlayer players3;
    public SpinAndWinPlayer players4;
    public SpinAndWinPlayer players5;
    public SpinAndWinPlayer players6;
    public SpinAndWinPlayer players7;

    public List<SpinAndWinPlayer> SAWPlayerList = new List<SpinAndWinPlayer>();


    [Header("--- Bot PlayersList ---")]
    public SpinAndWinPlayerManager player1;
    public SpinAndWinPlayerManager player2;
    public SpinAndWinPlayerManager player3;
    public SpinAndWinPlayerManager player4;
    public SpinAndWinPlayerManager player5;
    public SpinAndWinPlayerManager player6;
    public List<SpinAndWinPlayerManager> SpinAndWinPlayerList = new List<SpinAndWinPlayerManager>();
    public List<SpinAndWinPlayerHistory> PlayerHistories = new List<SpinAndWinPlayerHistory>();

    public List<CardSuffle> cardSuffles = new List<CardSuffle>();
    public CardSuffle cardSuffle1;
    public CardSuffle cardSuffle2;
    public int cardNo1;
    public int cardNo2;
    public GameObject[] chipBtn;
    public GameObject chipObj;
    public float[] chipPrice;
    public Sprite[] chipsSprite;
    public GameObject dragonParent;
    public GameObject tigerParent;
    public GameObject tieParent;
    public GameObject ourProfile;
    public GameObject otherProfile;
    public GameObject cardObj;
    public GameObject cardCenterObj;
    public GameObject cardGen1;
    public GameObject cardGen2;
    public GameObject cardGenPre1;
    public GameObject cardGenPre2;

    public Text dragonPriceTxt;
    public Text tigerPriceTxt;
    public Text tiePriceTxt;

    public Text dragonPTxt;
    public Text tigerPTxt;
    public Text tiePTxt;

    public float dragonTotalPrice;
    public float tigerTotalPrice;
    public float tieTotalPrice;

    public float dragonPrice;
    public float tigerPrice;
    public float tiePrice;

    public int selectChipNo;
    public Text timerTxt;
    public float fixTimerValue;
    public float timerValue;
    public float secondCount;
    bool isEnterBetStop;
    private bool _isClickAvailable;

    public bool isWin = true;
    public int winNo = 0;


    bool isAdmin = false;
    public int maxWinList = 16;
    public List<int> winList;
    public List<GameObject> historyChips = new List<GameObject>();
    public List<GameObject> genChipList_Dragon = new List<GameObject>();
    public List<GameObject> genChipList_Tiger = new List<GameObject>();
    public List<GameObject> genChipList_Tie = new List<GameObject>();

    public List<SpinAndWinBetClass> dragonTigerBetClasses = new List<SpinAndWinBetClass>();
    private string playerPrefsKey = "HistoryChipsList";

    //public float[] botPlayerBalance = { 15995f, 11225f, 10000f, 50000f, 85624f, 96545f, 65826f, 94584f, 72665f, 38561f };

    [Header("--- Sounds ---")]
    public Image soundImg;
    public Image musicImg;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _isClickAvailable = false;
    }

    public void UpdateNameBalance()
    {
        userNameTxt.text = DataManager.Instance.playerData.firstName.ToString();
        balanceTxt.text = DataManager.Instance.playerData.balance.ToString();
        //GetPlayerHistory();
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopBackgroundMusic();

        //StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));

        for (int i = 0; i < SAWPlayerList.Count; i++)
        {
            SAWPlayerList[i].gameObject.SetActive(false);
        }

        UpdateNameBalance();
        GetPlayerHistory();

        OpenScene();
        DataManager.Instance.LoadProfileImage(PlayerPrefs.GetString("ProfileURL"), avatarImg);
        //PlayerPrefs.DeleteKey(playerPrefsKey);
        //LoadHistoryChips();

        //StartCoroutine(StartBet());
        ChipAnimMaintain(1);
        HistoryLoader(DataManager.Instance.listString);
        CheckSound();
    }

    public void OpenScene()
    {
        Debug.Log("DragonTiger 1 IN");

        if (DataManager.Instance.joinPlayerDatas.Count >= TestSocketIO.Instace.dragonTigerRequirePlayer)
        {
            Debug.Log("DragonTiger 1 IN");
            CreateAdmin(); // Creating admin
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && isAdmin)
            {
                StartCoroutine(StartBet());
                SetUpPlayer();
            }
            else
            {
                if (isAdmin) return;
                if (!_isClickAvailable && isEnterBetStop == false)
                {
                    waitNextRoundScreenObj.SetActive(true);
                }
                // foreach (var t in SAWPlayerList)
                // {
                //     t.gameObject.SetActive(false);
                // }
                //SetUpPlayer();
                print("-----------------Got setactive false 237 --------------------------------------");
            }
        }
        else // if there is no player do following things
        {
            foreach (var t in SAWPlayerList)
            {
                t.gameObject.SetActive(false);
            }
            //playerFindUserNameTxt.text = DataManager.Instance.playerData.firstName;
            //playerFindScreenObj.SetActive(true);
            print("-----------------Got setactive false 247 --------------------------------------");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (timerValue == 0 && isEnterBetStop == false && waitNextRoundScreenObj.activeSelf == false)
        {
            isEnterBetStop = true;
            //Stop Bet
            StartCoroutine(StopBet());
        }
        else if (!timerTxt.text.Equals("0"))
        {
            secondCount -= Time.deltaTime;
            timerValue = ((int)secondCount);
            timerTxt.text = timerValue.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    RestartTimer();
        //}
    }

    /*private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3((minDragonX + maxDragonX) / 2f, (minDragonY + maxDragonY) / 2f), new Vector3(maxDragonX - minDragonX, maxDragonY - minDragonY));
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((minTigerX + maxTigerX) / 2f, (minTigerY + maxTigerY) / 2f), new Vector3(maxTigerX - minTigerX, maxTigerY - minTigerY));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3((minTieX + maxTieX) / 2f, (minTieY + maxTieY) / 2f), new Vector3(maxTieX - minTieX, maxTieY - minTieY));
    }*/


    private void LoadHistoryChips()
    {
        string listString = PlayerPrefs.GetString(playerPrefsKey, "");
        if (listString != "")
        {
            winList = new List<int>(listString.Split(',').Select(x => int.Parse(x)));
        }

        foreach (var t in winList)
        {
            HistoryTacker(t);
        }
    }

    private void UpdateHistoryChips(int num)
    {
        print("WinNumber -> " + num);
        winList.RemoveAt(0);
        winList.Add(num);

        foreach (var t in winList)
        {
            HistoryTacker(t);
        }

        if (!isAdmin) return;
        DataManager.Instance.listString = string.Join(",", winList.Select(x => x.ToString()).ToArray());
        SetWinData(DataManager.Instance.listString);
        print(DataManager.Instance.listString);
    }

    public void GetUpdatedHistory(string data)
    {
        if (isAdmin) return;
        if (data != "")
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
        }
    }

    public void HistoryLoader(string data)
    {
        if (data != "")
        {
            winList = new List<int>(data.Split(',').Select(x => int.Parse(x)));
        }

        foreach (var t in winList)
        {
            HistoryTacker(t);
        }
    }


    #region Cards Maintain

    public void Card_Open_Match()
    {
        SoundManager.Instance.CasinoCardSwipeSound();
        cardGenPre1.transform.DOScale(new Vector3(0, 1, 1), 0.25f).OnComplete(() =>
        {
            cardGenPre1.transform.GetComponent<Image>().sprite = cardSuffle1.cardSprite;
            cardGenPre1.transform.DOScale(new Vector3(1, 1, 1), 0.25f).OnComplete(() =>
            {
                cardGenPre1.transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.25f).OnComplete(() =>
                {
                    cardGenPre1.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f).OnComplete(() =>
                    {
                        SecondCardNo();
                    });
                });
            });
        });
    }

    void SecondCardNo()
    {
        SoundManager.Instance.CasinoCardSwipeSound();

        cardGenPre2.transform.DOScale(new Vector3(0, 1, 1), 0.25f).OnComplete(() =>
        {
            cardGenPre2.transform.GetComponent<Image>().sprite = cardSuffle2.cardSprite;
            cardGenPre2.transform.DOScale(new Vector3(1, 1, 1), 0.25f).OnComplete(() =>
            {
                cardGenPre2.transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.25f).OnComplete(() =>
                {
                    cardGenPre2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f).OnComplete(() =>
                    {
                        CardMatch();
                    });
                });
            });
        });
    }

    public void CardMatch()
    {
        isWin = true;
        winNo = 0;
        if (cardSuffle1.cardNo == cardSuffle2.cardNo)
        {
            winNo = 1; // tie
        }
        else if (cardSuffle1.cardNo > cardSuffle2.cardNo)
        {
            winNo = 2; // dragon
        }
        else if (cardSuffle1.cardNo < cardSuffle2.cardNo)
        {
            winNo = 3; // tiger
        }

        print("Win No : " + winNo);

        StartCoroutine(AnimationOpen(winNo));
        StartCoroutine(SpinAndWinAIManager.Instance.CoinDestroy(winNo));

        if (isAdmin)
        {
            UpdateHistoryChips(winNo);
        }
    }
    //a

    public IEnumerator AnimationOpen(int winNo)
    {
        float waitTime = 0;
        if (winNo == 2 || winNo == 3)
        {
            waitTime = 3.16f;
            if (winNo == 2)
            {
                dragonAnim.SetActive(true);

            }
            else if (winNo == 3)
            {
                tigerAnim.SetActive(true);

            }
        }

        yield return new WaitForSeconds(waitTime);
        dragonAnim.SetActive(false);
        tigerAnim.SetActive(false);
        if (winNo == 1)
        {

            float animSpeed = 0.3f;
            int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;

            for (int i = 0; i < genChipList_Dragon.Count; i++)
            {
                int no = i;

                genChipList_Dragon[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Dragon[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX),
                        UnityEngine.Random.Range(minTieY, maxTieY));
                    genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Dragon[no].transform.SetParent(tieParent.transform);
                    genChipList_Dragon.Add(genChipList_Dragon[no]);
                    genChipList_Dragon[no].transform
                        .DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            UpdateList(tNum, genChipList_Tie, genChipList_Dragon);
                        });
                });
            }

            for (int i = 0; i < genChipList_Tiger.Count; i++)
            {

                int no = i;
                genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Tiger[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {

                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX),
                        UnityEngine.Random.Range(minTieY, maxTieY));
                    genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tiger[no].transform.SetParent(tieParent.transform);
                    genChipList_Tiger.Add(genChipList_Dragon[no]);
                    genChipList_Tiger[no].transform
                        .DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tie.RemoveAt(no);

                            UpdateList(tNum, genChipList_Tie, genChipList_Tiger);

                        });
                });
            }
        }
        else if (winNo == 2)
        {
            float animSpeed = 0.3f;


            int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;
            for (int i = 0; i < genChipList_Tie.Count; i++)
            {
                int no = i;

                genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Tie[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX),
                        UnityEngine.Random.Range(minDragonY, maxDragonY));
                    genChipList_Tie[no].transform.DOMove(rPos, animSpeed);

                    genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tie[no].transform.SetParent(dragonParent.transform);
                    genChipList_Dragon.Add(genChipList_Tie[no]);

                    genChipList_Tie[no].transform
                        .DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tie.RemoveAt(no);

                            UpdateList(tNum, genChipList_Dragon, genChipList_Tie);
                        });
                });
            }

            for (int i = 0; i < genChipList_Tiger.Count; i++)
            {

                int no = i;
                genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Tiger[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX),
                        UnityEngine.Random.Range(minDragonY, maxDragonY));
                    genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tiger[no].transform.SetParent(dragonParent.transform);
                    genChipList_Dragon.Add(genChipList_Tiger[no]);
                    genChipList_Tiger[no].transform
                        .DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tiger.Remove(genChipList_Tiger[no]);
                            UpdateList(tNum, genChipList_Dragon, genChipList_Tiger);
                        });
                });
            }
        }
        else if (winNo == 3)
        {
            float animSpeed = 0.3f;

            int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;
            for (int i = 0; i < genChipList_Dragon.Count; i++)
            {
                int no = i;
                genChipList_Dragon[no].transform.DOScale(Vector3.zero, animSpeed);
                genChipList_Dragon[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX),
                        UnityEngine.Random.Range(minTigerY, maxTigerY));
                    genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);

                    genChipList_Dragon[no].transform.SetParent(tigerParent.transform);
                    genChipList_Tiger.Add(genChipList_Dragon[no]);

                    genChipList_Dragon[no].transform
                        .DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Dragon.RemoveAt(no);

                            UpdateList(tNum, genChipList_Tiger, genChipList_Dragon);
                        });
                });
            }

            for (int i = 0; i < genChipList_Tie.Count; i++)
            {
                int no = i;
                genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);

                genChipList_Tie[no].transform.DOMove(cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                {
                    Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX),
                        UnityEngine.Random.Range(minTigerY, maxTigerY));
                    genChipList_Tie[no].transform.DOMove(rPos, animSpeed);
                    genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                    genChipList_Tie[no].transform.SetParent(tigerParent.transform);
                    genChipList_Tiger.Add(genChipList_Tie[no]);
                    genChipList_Tie[no].transform
                        .DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tie.Remove(genChipList_Tie[no]);

                            UpdateList(tNum, genChipList_Tiger, genChipList_Tie);
                        });
                });
            }
        }

        int cNo = 0;
        if (winNo == 1)
        {
            cNo = 3;

        }
        else if (winNo == 2)
        {
            cNo = 1;
        }
        else if (winNo == 3)
        {
            cNo = 2;
        }

        float investPrice = 0;
        float otherPrice = 0;
        float betPrice = 0;
        for (int i = 0; i < dragonTigerBetClasses.Count; i++)
        {
            if (dragonTigerBetClasses[i].no == cNo)
            {
                investPrice += dragonTigerBetClasses[i].price;
                betPrice = investPrice;
            }
            else
            {
                otherPrice += dragonTigerBetClasses[i].price;
            }
        }

        print("invest Price : " + investPrice);
        print("Other Price : " + otherPrice);
        float mainInvestPrice = investPrice;

        if (winNo == 1)
        {
            investPrice *= 8;
        }
        else if (winNo == 2)
        {
            investPrice *= 2;
        }
        else if (winNo == 3)
        {
            investPrice *= 2;
        }


        float adminPercentage = DataManager.Instance.adminPercentage;




        /*float winAmount = investPrice;
        float adminCommssion = (adminPercentage / 100);
        float playerWinAmount = winAmount - (winAmount * adminCommssion);*/
        //float winAmount = investPrice;
        float winReward = investPrice - betPrice;
        float adminCommission = adminPercentage / 100;
        float winAmount = winReward - (winReward * adminCommission);
        float playerWinAmount = betPrice + winAmount;

        otherPrice -= playerWinAmount;

        if (playerWinAmount != 0)
        {
            winParticles.SetActive(true);
            SoundManager.Instance.CasinoWinSound();
            winAnimationTxt.gameObject.SetActive(true);
            winAnimationTxt.text = "+" + playerWinAmount;
            Invoke(nameof(WinAmountTextOff), 1.25f);
            DataManager.Instance.AddAmount((float)(playerWinAmount), DataManager.Instance.gameId,
                "Dragon_Tiger-Win-" + DataManager.Instance.gameId, "won", (float)(adminCommission), winNo);

        }

        if (otherPrice != 0)
        {
            //otherPrice Direct Profit
        }

        Invoke(nameof(ClearChips), 1f);

        int childCount = tieParent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(tieParent.transform.GetChild(i).gameObject);
        }

        Invoke(nameof(WinAfterRoundChange), 3.5f);
    }

    #region PlayerHistory

    public void GetPlayerHistory()
    {
        //StartCoroutine(GetPlayerHistoryData());
        GetBotPlayer();
        SetBotPlayer();
    }

    /*IEnumerator GetPlayerHistoryData()
    {
    // recent player data
        UnityWebRequest request =
            UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/winnertop/6389923aae033c4bb7429405");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("tran Data : " + request.downloadHandler.text.ToString());
            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(keys["data"].ToString());
            if (data.Count == 0)
            {
                print("there is no data");
                //waitTxt.text = "No History...";
            }
            else
            {
                for (int i = 0; i < data.Count; i++)
                {

                    PlayerHistory t = new PlayerHistory();
                    t.balance = data[i]["balance"];
                    t.name = data[i]["firstName"];
                    t.avatar = data[i]["picture"];
                    PlayerHistories.Add(t);
                }
            }
        }

        for (int i = 0; i < DTPlayerList.Count; i++)
        {
            DTPlayerList[i].playerNameTxt.text = PlayerHistories[i].name;
            DTPlayerList[i].playerBalanceTxt.text = PlayerHistories[i].balance.ToString();
            DTPlayerList[i].avatar = PlayerHistories[i].avatar;
            DTPlayerList[i].GetPlayerImage();
        }

        print("there is error");
    }*/

    public void OpenBotPlayer()
    {
        if (DataManager.Instance.joinPlayerDatas.Count == 1)
        {
            foreach (var t in SpinAndWinPlayerList)
            {
                t.gameObject.SetActive(true);
            }
        }

        else
        {
            // for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count - 1; i++)
            // {
            //     DTPlayerList[i].gameObject.SetActive(false);
            // }
            // return;
            for (int i = 0; i < SpinAndWinPlayerList.Count; i++)
            {
                SpinAndWinPlayerList[i].gameObject.SetActive(i >= (DataManager.Instance.joinPlayerDatas.Count - 1));
            }
            return;
        }


        GetBotPlayer();
        SetBotPlayer();
    }

    private void GetBotPlayer()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int[] randomAvatars = avatars.Take(SpinAndWinPlayerList.Count).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int[] randomNames = names.Take(SpinAndWinPlayerList.Count).ToArray();


        for (int i = 0; i < SpinAndWinPlayerList.Count; i++)
        {
            SpinAndWinPlayerHistory t = new SpinAndWinPlayerHistory();
            t.avatar = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
            t.name = BotManager.Instance.botUserName[randomNames[i]];
            PlayerHistories.Add(t);
        }
    }


    private void SetBotPlayer()
    {
        for (int i = 0; i < SpinAndWinPlayerList.Count; i++)
        {
            SpinAndWinPlayerList[i].playerNameTxt.text = PlayerHistories[i].name;
            SpinAndWinPlayerList[i].avatar = PlayerHistories[i].avatar;
            SpinAndWinPlayerList[i].GetPlayerImage();
            // int randomIndex = Random.Range(0, botPlayerBalance.Length + 1);
            // DTPlayerList[i].balance = botPlayerBalance[randomIndex];
            //DTPlayerList[i].playerBalanceTxt.text = 99999f.ToString(CultureInfo.InvariantCulture);
        }
    }

    #endregion


    public void HistoryTacker(int winNo)
    {
        switch (winNo)
        {
            case 1:
                {
                    var chip = Instantiate(TieCoin, CoinsCarrier.transform);
                    historyChips.Add(chip);
                    var firstObject = historyChips[0];
                    historyChips.RemoveAt(0);
                    Destroy(firstObject);
                    break;
                }
            case 2:
                {
                    var chip = Instantiate(DragonCoin, CoinsCarrier.transform);
                    historyChips.Add(chip);
                    var firstObject = historyChips[0];
                    historyChips.RemoveAt(0);
                    Destroy(firstObject);
                    break;
                }
            case 3:
                {
                    var chip = Instantiate(TigerCoin, CoinsCarrier.transform);
                    historyChips.Add(chip);
                    var firstObject = historyChips[0];
                    historyChips.RemoveAt(0);
                    Destroy(firstObject);
                    break;
                }
            default:
                {
                    print("No data to track");
                    break;
                }
        }
        // string jsonString = JsonConvert.SerializeObject(historyChips);
        // PlayerPrefs.SetString(playerPrefsKey, jsonString);
        // print(jsonString);
        // PlayerPrefs.Save();
    }


    public void WinAmountTextOff()
    {
        winAnimationTxt.gameObject.SetActive(false);

    }

    void WinAfterRoundChange()
    {
        winParticles.SetActive(false);
        //ClearChips();

        if (isAdmin)
        {
            if (DataManager.Instance.joinPlayerDatas.Count == 1 || isAdmin)
            {
                StartCoroutine(StartBet());
                SetUpPlayer();
            }
            else
            {
                for (int i = 0; i < SAWPlayerList.Count; i++)
                {
                    SAWPlayerList[i].gameObject.SetActive(false);
                }
                //waitNextRoundScreenObj.SetActive(true);
                SetUpPlayer();
            }
        }


    }

    private void ClearChips()
    {
        // for bot
        foreach (var t in SpinAndWinAIManager.Instance.genChipList_Dragon) { Destroy(t); }
        SpinAndWinAIManager.Instance.genChipList_Dragon.Clear();
        foreach (var t in SpinAndWinAIManager.Instance.genChipList_Tiger) { Destroy(t); }
        SpinAndWinAIManager.Instance.genChipList_Tiger.Clear();
        foreach (var t in SpinAndWinAIManager.Instance.genChipList_Tie) { Destroy(t); }
        SpinAndWinAIManager.Instance.genChipList_Tie.Clear();
        // for user
        foreach (var t in genChipList_Dragon) { Destroy(t); }
        genChipList_Dragon.Clear();
        foreach (var t in genChipList_Tiger) { Destroy(t); }
        genChipList_Tiger.Clear();
        foreach (var t in genChipList_Tie) { Destroy(t); }
        genChipList_Tie.Clear();
        //
        if (dragonParent.transform.childCount <= 0) return;
        for (int i = 0; i < dragonParent.transform.childCount; i++)
        {
            var child = dragonParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        if (tigerParent.transform.childCount <= 0) return;
        for (int i = 0; i < tigerParent.transform.childCount; i++)
        {
            var child = tigerParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
        if (tieParent.transform.childCount <= 0) return;
        for (int i = 0; i < tieParent.transform.childCount; i++)
        {
            var child = tieParent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    void UpdateList(int no, List<GameObject> list, List<GameObject> list1)
    {

        if (no == list.Count)
        {
            list1.Clear();
        }
        else
        {
            return;
        }

        float moveSpeed = 0.2f;

        if (winNo == 1)
        {
            if (genChipList_Dragon.Count == 0 && genChipList_Tiger.Count == 0)
            {
                for (int i = 0; i < genChipList_Tie.Count; i++)
                {
                    bool isEnter = false;
                    for (int j = 0; j < dragonTigerBetClasses.Count; j++)
                    {
                        if (dragonTigerBetClasses[j].betObj == genChipList_Tie[i])
                        {
                            isEnter = true;
                        }
                    }

                    int no1 = i;
                    if (isEnter == false)
                    {

                        genChipList_Tie[i].transform.DOMove(otherProfile.transform.position, moveSpeed);

                        genChipList_Tie[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tie[i]);
                            if (no1 == genChipList_Tie.Count)
                            {
                                //Restart
                            }
                        });
                    }
                    else
                    {

                        genChipList_Tie[i].transform.DOMove(otherProfile.transform.position, moveSpeed);

                        genChipList_Tie[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tie[i]);
                            if (no1 == genChipList_Tie.Count)
                            {
                                //Restart
                            }
                        });
                    }


                }
            }
        }
        else if (winNo == 2)
        {
            if (genChipList_Tie.Count == 0 && genChipList_Tiger.Count == 0)
            {
                for (int i = 0; i < genChipList_Dragon.Count; i++)
                {
                    bool isEnter = false;
                    for (int j = 0; j < dragonTigerBetClasses.Count; j++)
                    {
                        if (dragonTigerBetClasses[j].betObj == genChipList_Dragon[i])
                        {
                            isEnter = true;
                        }
                    }

                    int no1 = i;
                    if (isEnter == false)
                    {

                        genChipList_Dragon[i].transform.DOMove(otherProfile.transform.position, moveSpeed);
                        genChipList_Dragon[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            if (no1 == genChipList_Dragon.Count)
                            {
                                //Restart
                            }

                            Destroy(genChipList_Dragon[i]);
                        });
                    }
                    else
                    {

                        genChipList_Dragon[i].transform.DOMove(otherProfile.transform.position, moveSpeed);
                        genChipList_Dragon[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            if (no1 == genChipList_Dragon.Count)
                            {
                                //Restart
                            }

                            Destroy(genChipList_Dragon[i]);
                        });
                    }


                }
            }
        }
        else if (winNo == 3)
        {
            if (genChipList_Dragon.Count == 0 && genChipList_Tie.Count == 0)
            {
                for (int i = 0; i < genChipList_Tiger.Count; i++)
                {
                    bool isEnter = false;
                    for (int j = 0; j < dragonTigerBetClasses.Count; j++)
                    {
                        if (dragonTigerBetClasses[j].betObj == genChipList_Tiger[i])
                        {
                            isEnter = true;
                        }
                    }

                    int no1 = i;
                    if (isEnter == false)
                    {

                        genChipList_Tiger[i].transform.DOMove(otherProfile.transform.position, moveSpeed);
                        genChipList_Tiger[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tiger[i]);
                            if (no1 == genChipList_Tiger.Count)
                            {
                                //Restart
                            }
                        });
                    }
                    else
                    {

                        genChipList_Tiger[i].transform.DOMove(otherProfile.transform.position, moveSpeed);
                        genChipList_Tiger[i].transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(genChipList_Tiger[i]);
                            if (no1 == genChipList_Tiger.Count)
                            {
                                //Restart
                            }
                        });
                    }


                }
            }
        }

    }

    void UpdateBoardPrice()
    {
        dragonPriceTxt.text = "Bet: " + dragonTotalPrice.ToString("F2");
        tigerPriceTxt.text = "Bet: " + tigerTotalPrice.ToString("F2");
        tiePriceTxt.text = "Bet: " + tieTotalPrice.ToString("F2");

        dragonPTxt.text = "Bet: " + dragonPrice.ToString("F2");
        tigerPTxt.text = "Bet: " + tigerPrice.ToString("F2");
        tiePTxt.text = "Bet: " + tiePrice.ToString("F2");
    }

    #endregion


    #region Round Maintain

    public IEnumerator StartBet()
    {
        SoundManager.Instance.CasinoTurnSound();
        DataManager.Instance.UserTurnVibrate();
        Destroy(cardGenPre1);
        Destroy(cardGenPre2);
        // if (cardGenPre1 != null)
        // {
        // }
        //
        // if (cardGenPre2 != null)
        // {
        // }


        isEnterBetStop = true;
        startBetObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Vector3 customZoomScale = new Vector3(4.0f, 4.0f, 4.0f);
        StartAnimationPlay(objects, customZoomScale, 0.1f, 0.009f);
        _isClickAvailable = true;
        if (isAdmin)
        {
            cardNo1 = UnityEngine.Random.Range(0, 52);
            cardNo2 = UnityEngine.Random.Range(0, 52);

            //cardNo1 = 0;
            //cardNo2 = 1;


            while (cardNo1 == cardNo2)
            {
                cardNo2 = UnityEngine.Random.Range(0, 52);
            }


            //cardSuffle1 = cardSuffles[cardNo1];
            //cardSuffle2 = cardSuffles[cardNo2];
            //SetRoomData(cardNo1, cardNo2);

            cardSuffle1 = cardSuffles[cardNo1];
            cardSuffle2 = cardSuffles[cardNo2];
            SetRoomData(cardNo1, cardNo2, DataManager.Instance.listString);
            TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
        }


        yield return new WaitForSeconds(1.35f);
        startBetObj.SetActive(false);
        betAnimationONOff(true);
        RestartTimer();

        // SpinAndWinAIManager.Instance.isActive = true;

    }
    public List<GameObject> objects;  // List of objects to animate
    public List<GameObject> stopObjects;

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
    public void SetUpPlayer()
    {
        int cnt = 1;
        for (int i = 0; i < SAWPlayerList.Count; i++)
        {
            if (i < DataManager.Instance.joinPlayerDatas.Count)
            {
                if (DataManager.Instance.joinPlayerDatas[i].userId.Equals(DataManager.Instance.playerData._id))
                {
                    SAWPlayerList[0].gameObject.SetActive(true);
                    SAWPlayerList[0].playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    SAWPlayerList[0].playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    //SAWPlayerList[0].playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    SAWPlayerList[0].avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    SAWPlayerList[0].GetPlayerImage();
                }
                else
                {
                    SAWPlayerList[cnt].gameObject.SetActive(true);
                    SAWPlayerList[cnt].playerId = DataManager.Instance.joinPlayerDatas[i].userId;
                    SAWPlayerList[cnt].playerNameTxt.text = DataManager.Instance.joinPlayerDatas[i].userName;
                    SAWPlayerList[cnt].playerBalanceTxt.text = DataManager.Instance.joinPlayerDatas[i].balance;
                    SAWPlayerList[cnt].avatar = DataManager.Instance.joinPlayerDatas[i].avtar;
                    SAWPlayerList[cnt].GetPlayerImage();

                    cnt++;
                }
            }

            else
            {
                SAWPlayerList[i].gameObject.SetActive(false);
                SAWPlayerList[i].playerId = "";
                SAWPlayerList[i].playerNameTxt.text = "";
                print("-------------------- Player is getting active false --------------------------");

            }
        }

        OpenBotPlayer();
    }

    public IEnumerator StopBet()
    {
        _isClickAvailable = false;
        GetLargestBet();

        isEnterBetStop = true;
        stopBetObj.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Vector3 customZoomScale = new Vector3(4.0f, 4.0f, 4.0f);
        StartAnimationPlay(stopObjects, customZoomScale, 0.1f, 0.1f);
        SpinAndWinAIManager.Instance.isActive = false;
        if (isAdmin)
        {
            if (dragonTotalPrice + tigerTotalPrice + tieTotalPrice > 0)
            {
                cardSuffle1 = cardSuffles[cardNo1];
                cardSuffle2 = cardSuffles[cardNo2];
                SetDeckData(cardNo1, cardNo2);
            }
        }
        yield return new WaitForSeconds(1.5f);
        stopBetObj.SetActive(false);
        betAnimationONOff(false);

        Card_Open_Match();
    }

    public void GetDeckData(int no1, int no2)
    {
        if (isAdmin) return;
        cardNo1 = no1;
        cardNo2 = no2;

        cardSuffle1 = cardSuffles[cardNo1];
        cardSuffle2 = cardSuffles[cardNo2];
    }

    public void RestartTimer()
    {
        dragonTigerBetClasses.Clear();
        isWin = false;
        timerValue = fixTimerValue;
        secondCount = timerValue;
        timerTxt.text = timerValue.ToString();
        isEnterBetStop = false;

        dragonTotalPrice = 0;
        tigerTotalPrice = 0;
        tieTotalPrice = 0;

        dragonPrice = 0;
        tigerPrice = 0;
        tiePrice = 0;
        UpdateBoardPrice();

        //        if (isAdmin)
        //        {
        //            cardNo1 = UnityEngine.Random.Range(0, 53);
        //            cardNo2 = UnityEngine.Random.Range(0, 53);

        //            //cardNo1 = 0;
        //            //cardNo2 = 1;


        //            while (cardNo1 == cardNo2)
        //            {
        //                cardNo2 = UnityEngine.Random.Range(0, 53);
        //            }

        //            cardSuffle1 = cardSuffles[cardNo1];
        //            cardSuffle2 = cardSuffles[cardNo2];
        //            SetRoomData(cardNo1, cardNo2)
        //;
        //        }

        GenerateCards();
    }

    void GenerateCards()
    {
        Debug.Log("<color=yellow> ------------------- </color>");
        float moveSpeed = 0.5f;
        cardGenPre1 = Instantiate(cardObj, cardGen1.transform);
        cardGenPre1.transform.position = cardCenterObj.transform.position;
        cardGenPre1.transform.localScale = Vector3.zero;
        cardGenPre1.transform.DOMove(cardGen1.transform.position, moveSpeed);
        cardGenPre1.transform.DOScale(Vector3.one, moveSpeed + 0.1f);
        SoundManager.Instance.CasinoCardMoveSound();

        cardGenPre2 = Instantiate(cardObj, cardGen2.transform);
        cardGenPre2.transform.position = cardCenterObj.transform.position;
        cardGenPre2.transform.localScale = Vector3.zero;
        cardGenPre2.transform.DOMove(cardGen2.transform.position, moveSpeed);
        SoundManager.Instance.CasinoCardMoveSound();
        cardGenPre2.transform.DOScale(Vector3.one, moveSpeed + 0.1f).OnComplete(() =>
        {

            //Game Continue
        });
        SpinAndWinAIManager.Instance.isActive = true;

    }

    #endregion

    #region Other Button

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }

    #endregion

    #region Menu Screen

    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }

    public void CloseMenuScreenButton()
    {
        SoundManager.Instance.ButtonClick();
        menuScreenObj.SetActive(false);
    }

    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            TestSocketIO.Instace.LeaveRoom();
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
            Instantiate(shopPrefab, shopPrefabParent.transform);
        }
    }

    #endregion

    #region Rule Panel

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
    }

    public void CloseRuleButton()
    {
        SoundManager.Instance.ButtonClick();
        ruleScreenObj.SetActive(false);
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

    #region GamePlay

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


    public void GameThreeButton(int no)
    {
        if (!_isClickAvailable) return;
        switch (no)
        {
            case 1:
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
                        "Dragon_Tiger-Bet-" + DataManager.Instance.gameId, "game", 2);

                    /* Vector3 rPos = new Vector3(Random.Range(minDragonX, maxDragonX),
                         Random.Range(minDragonY, maxDragonY)); */
                    Vector3 rPos = SpinAndWinAIManager.Instance.GetRandomPositionWithinTransform(dragonParent.transform);
                    GameObject chipGen = Instantiate(chipObj, dragonParent.transform);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = ourProfile.transform.position;
                    dragonTotalPrice += chipPrice[selectChipNo];
                    dragonPrice += chipPrice[selectChipNo];
                    SpinAndWinAIManager.Instance._dMinBalance += chipPrice[selectChipNo];
                    genChipList_Dragon.Add(chipGen);
                    ChipGenerate(chipGen, rPos);

                    SpinAndWinBetClass betClass = new SpinAndWinBetClass();
                    betClass.no = no;
                    betClass.price = chipPrice[selectChipNo];
                    betClass.betObj = chipGen;
                    dragonTigerBetClasses.Add(betClass);
                    break;
                }
            case 2:
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
                        "Dragon_Tiger-Bet-" + DataManager.Instance.gameId, "game", 3);

                    /* Vector3 rPos = new Vector3(Random.Range(minTigerX, maxTigerX),
                         Random.Range(minTigerY, maxTigerY));*/
                    Vector3 rPos = SpinAndWinAIManager.Instance.GetRandomPositionWithinTransform(tigerParent.transform);
                    GameObject chipGen = Instantiate(chipObj, tigerParent.transform);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    chipGen.transform.position = ourProfile.transform.position;
                    tigerTotalPrice += chipPrice[selectChipNo];
                    tigerPrice += chipPrice[selectChipNo];
                    SpinAndWinAIManager.Instance._tMinBalance += chipPrice[selectChipNo];
                    genChipList_Tiger.Add(chipGen);
                    ChipGenerate(chipGen, rPos);

                    SpinAndWinBetClass betClass = new SpinAndWinBetClass();
                    betClass.no = no;
                    betClass.price = chipPrice[selectChipNo];
                    betClass.betObj = chipGen;

                    dragonTigerBetClasses.Add(betClass);
                    break;
                }
            case 3:
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
                        "Dragon_Tiger-Bet-" + DataManager.Instance.gameId, "game", 1);

                    /* Vector3 rPos = new Vector3(Random.Range(minTieX, maxTieX),
                         Random.Range(minTieY, maxTieY));*/
                    Vector3 rPos = SpinAndWinAIManager.Instance.GetRandomPositionWithinTransform(tieParent.transform);
                    GameObject chipGen = Instantiate(chipObj, tieParent.transform);
                    chipGen.transform.GetComponent<Image>().sprite = chipsSprite[selectChipNo];
                    genChipList_Tiger.Add(chipGen);
                    chipGen.transform.position = ourProfile.transform.position;
                    tieTotalPrice += chipPrice[selectChipNo];
                    tiePrice += chipPrice[selectChipNo];
                    SpinAndWinAIManager.Instance._tiMinBalance += chipPrice[selectChipNo];
                    genChipList_Tie.Add(chipGen);
                    ChipGenerate(chipGen, rPos);

                    SpinAndWinBetClass betClass = new SpinAndWinBetClass();
                    betClass.no = no;
                    betClass.price = chipPrice[selectChipNo];
                    betClass.betObj = chipGen;
                    dragonTigerBetClasses.Add(betClass);
                    break;
                }
        }
        SpinAndWinAIManager.Instance.UpdateTiePrice();
        SendDargonTigerBet(no, selectChipNo);
        UpdateBoardPrice();
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

    #region Chip Genearte

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

    #endregion

    #endregion

    #region Socket Maintain

    public void SendDargonTigerBet(int boxNo, int chipNo)
    {

        JSONObject obj = new JSONObject();
        obj.AddField("PlayerID", DataManager.Instance.playerData._id);
        obj.AddField("TournamentID", DataManager.Instance.tournamentID);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("boxNo", boxNo);
        obj.AddField("chipNo", chipNo);
        TestSocketIO.Instace.Senddata("SendSpinAndWinBet", obj);

        Debug.Log("<color=yellow> SendDragonTigerBet </color> " + obj);
    }


    public void GetDragonTigerBet(int boxNo, int chipNo)
    {
        if (boxNo == 1)
        {
            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minDragonX, maxDragonX),
                UnityEngine.Random.Range(minDragonY, maxDragonY));
            GameObject chipGen = Instantiate(chipObj, dragonParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
            chipGen.transform.position = otherProfile.transform.position;
            dragonTotalPrice += chipPrice[chipNo];
            genChipList_Dragon.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }
        else if (boxNo == 2)
        {
            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTigerX, maxTigerX),
                UnityEngine.Random.Range(minTigerY, maxTigerY));
            GameObject chipGen = Instantiate(chipObj, tigerParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
            chipGen.transform.position = otherProfile.transform.position;
            tigerTotalPrice += chipPrice[chipNo];
            genChipList_Tiger.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }
        else if (boxNo == 3)
        {
            Vector3 rPos = new Vector3(UnityEngine.Random.Range(minTieX, maxTieX),
                UnityEngine.Random.Range(minTieY, maxTieY));
            GameObject chipGen = Instantiate(chipObj, tieParent.transform);
            chipGen.transform.GetComponent<Image>().sprite = chipsSprite[chipNo];
            genChipList_Tiger.Add(chipGen);
            chipGen.transform.position = otherProfile.transform.position;
            tieTotalPrice += chipPrice[chipNo];
            genChipList_Tie.Add(chipGen);
            ChipGenerate(chipGen, rPos);
        }

        UpdateBoardPrice();
    }

    #endregion


    #region Admin Maintain

    void CreateAdmin()
    {
        if (DataManager.Instance.joinPlayerDatas[0].userId.Equals(DataManager.Instance.playerData._id))
        {
            isAdmin = true;
        }
    }

    public void SetRoomData(int no1, int no2, string winListData)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("DeckNo1", no1);
        obj.AddField("DeckNo2", no2);
        //obj.AddField("DeckNo", 300);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        obj.AddField("gameMode", 20);
        TestSocketIO.Instace.SetRoomdata(TestSocketIO.Instace.roomid, obj);
    }

    public void SetDeckData(int no1, int no2)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("DeckNo1", no1);
        obj.AddField("DeckNo2", no2);
        obj.AddField("room", TestSocketIO.Instace.roomid);
        obj.AddField("dateTime", DateTime.UtcNow.ToString());
        TestSocketIO.Instace.Senddata("SendSAWDeckBet", obj);
    }

    public void SetWinData(string winListData)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("WinList", winListData);
        //obj.AddField("DeckNo", 300);
        TestSocketIO.Instace.SetWinData(TestSocketIO.Instace.roomid, obj);
    }

    public void GetRoomData(int no1, int no2, string winData)
    {
        cardNo1 = no1;
        cardNo2 = no2;


        cardSuffle1 = cardSuffles[cardNo1];
        cardSuffle2 = cardSuffles[cardNo2];

        if (isAdmin) return;
        //deckNo = no;
        StartCoroutine(StartBet());
        SetUpPlayer();
        //RoundGenerate();
        if (waitNextRoundScreenObj.activeSelf)
        {
            waitNextRoundScreenObj.SetActive(false);
        }
    }



    public void ChangeAAdmin(string leavePlayerId, string adminId)
    {
        if (DataManager.Instance.playerData._id.Equals(DataManager.Instance.joinPlayerDatas[0].userId))
        {
            isAdmin = true;
            if (DataManager.Instance.joinPlayerDatas.Count == 1 && waitNextRoundScreenObj.activeSelf == true)
            {
                StartCoroutine(StartBet());
                SetUpPlayer();
                if (waitNextRoundScreenObj.activeSelf == true)
                {
                    waitNextRoundScreenObj.SetActive(false);
                }
            }
        }
        else
        {
            isAdmin = false;
        }

        //var inventoryItem = SAWPlayerList.Find((x) => x.playerId == leavePlayerId);

        for (int i = 0; i < SAWPlayerList.Count; i++)
        {
            if (SAWPlayerList[i].playerId.Equals(leavePlayerId))
            {
                SAWPlayerList[i].gameObject.SetActive(false);
                print("---------------------Player got Removed---------------------------------");
            }
        }

        for (int i = 0; i < SAWPlayerList.Count; i++)
        {
            if (SAWPlayerList[i].gameObject.activeSelf == true)
            {
                string playerIdGet = SAWPlayerList[i].playerId;

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
                    SAWPlayerList[i].gameObject.SetActive(false);

                    print("---------------------Player got Removed from 1683---------------------------------");
                }
            }
        }

        SetUpPlayer();
    }

    #endregion



    #region Algorithm

    // To find the largest number among Dragon, Tiger & Tie

    // for selecting winner
    public void GetLargestBet()
    {
        if (!isAdmin) return;
        int gameNum = 0;
        // 1 for dragon, 2 for tiger, 3 for tie.
        // if Dragon is more
        if (dragonTotalPrice >= tigerTotalPrice)
        {
            gameNum = dragonTotalPrice >= tieTotalPrice ? 1 : 3;
        }
        else
        {
            gameNum = tigerTotalPrice >= tieTotalPrice ? 2 : 3;
        }

        GenerateNumber(gameNum);
    }



    private int set = 0;
    private bool _iscardGenPre1NotNull;
    private bool _iscardGenPre2NotNull;

    public void PickCardSet()
    {
        var shuffleNum = Random.Range(1, 5);

        set = shuffleNum switch
        {
            1 => Random.Range(1, 13),
            2 => Random.Range(13, 26),
            3 => Random.Range(26, 39),
            4 => Random.Range(39, 52),
            _ => set
        };
    }


    public void GenerateNumber(int num)
    {
        int card1 = 0;
        int card2 = 0;
        int newNum = 0;
        if (num == 1)
        {
            //Dragon win
            PickCardSet();
            card1 = set;
            PickCardSet();
            card2 = set;
            if (cardSuffles[card1].cardNo < cardSuffles[card2].cardNo)
            {
                newNum = CheckCardValue(card1);
                cardNo1 = newNum;
                cardNo2 = card2;
            }
            else
            {
                cardNo1 = card2;
                cardNo2 = card1;
            }
            //print(card1 + card2);
            print("Dragon Win");

        }
        else if (num == 2)
        {
            //tiger win
            PickCardSet();
            card1 = set;
            PickCardSet();
            card2 = set;
            if (cardSuffles[card1].cardNo > cardSuffles[card2].cardNo)
            {
                newNum = CheckCardValue(card2);
                cardNo1 = card1;
                cardNo2 = newNum;
            }
            else
            {
                cardNo1 = card2;
                cardNo2 = card1;
            }
            //print(card1 + card2);
            print("tiger Win");
        }
        else if (num == 3)
        {
            //tie win
            PickCardSet();
            card1 = set;
            PickCardSet();
            card2 = set;
            if (cardSuffles[card1].cardNo == cardSuffles[card2].cardNo)
            {
                cardNo2 = card2 - 1;
            }
            else
            {

            }
            //print(card1 + card2);
            print("Tie Win");
        }


    }

    private int CheckCardValue(int num)
    {
        // to check if card is = A
        switch (num)
        {
            case 12:
            case 25:
            case 38:
            case 51:
                return num - 9;
            default:
                return num;
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


    #endregion

}

[System.Serializable]
public class SpinAndWinPlayerHistory
{
    public string name;
    public string avatar;
    public float balance;
}


