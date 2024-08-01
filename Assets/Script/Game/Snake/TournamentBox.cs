using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TournamentBox : MonoBehaviour
{

    public Text playerCntTxt;
    public Text tournamentName;
    public Text prizePool;
    public Text timer;
    public Text entryTxt;
    public Text bonusTxt;

    public Color tournamentFreeColor;
    public Color tournamentNotFreeColor;
    public Color tournamentJoinedColor;

    public Button joinBtn;
    public Image joinImg;

    public Image timerBox;
    public Color timerBoxNormalColor;
    public Color timerBoxRedColor;

    public TournamentData tData;
    public int flag;
    public int playerCnt;
    public float secondsCount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }


    public void DataDisplay()
    {

        tournamentName.text = tData.name;
        
        //data.betAmount = tournamentData[i].betAmount * 1;
        //data.totalWinAmount = tournamentData[i].totalWinAmount;
        //int index = i;
        entryTxt.text = "₹ " + tData.betAmount;



        joinBtn.onClick.AddListener(() => JoinButtonClick());
        PlayerIncrease();
        bonusTxt.text = "Use ₹" + tData.bonusAmountDeduction + " bonus";
        if (tData.bonusAmountDeduction == 0)
        {
            bonusTxt.gameObject.SetActive(false);
        }

        //data.prizeTxt.text = "₹ " + tournamentData[i].totalWinAmount;
        if(prizePool.text.ToString().Length != 0)
        {
            prizePool.text = "₹ " + tData.totalWinAmount * 1;
        }
        
        if (tData.time.ToString().Length == 1)
        {
            timer.text = "0" + tData.time + ":"; /*03 min 32s*/
        }
        else
        {
            timer.text = tData.time + ":"; /*03 min 32s*/
        }
        GetDiffMinute();

        if (DataManager.Instance.tournamentID == tData._id)
        {
            entryTxt.text = "Joined";
            joinImg.color = tournamentJoinedColor;
        }
        else if (tData.betAmount == 0)
        {
            entryTxt.text = "Free";
            joinImg.color = tournamentFreeColor;
        }
        else
        {
            entryTxt.text = "₹" + tData.betAmount;
            joinImg.color = tournamentNotFreeColor;

        }



    }

    public void UpdateBtn()
    {
        if (DataManager.Instance.tournamentID == tData._id)
        {
            entryTxt.text = "Joined";
            joinImg.color = tournamentJoinedColor;
        }
        else if (tData.betAmount == 0)
        {
            entryTxt.text = "Free";
            joinImg.color = tournamentFreeColor;
        }
        else
        {
            entryTxt.text = "₹" + tData.betAmount;
            joinImg.color = tournamentNotFreeColor;

        }
    }

    void JoinButtonClick()
    {
        if (MainMenuManager.Instance.isPressJoin == false)
        {
            MainMenuManager.Instance.isPressJoin = true;
            if (entryTxt.text == "Joined")
            {
                return;
            }
            entryTxt.text = "Joined";
            joinImg.color = tournamentJoinedColor;

            if (!string.IsNullOrEmpty(DataManager.Instance.tournamentID)) return;
            if (tData.betAmount > float.Parse(DataManager.Instance.playerData.balance))
            {
                //tourMsg.SetActive(true);
                //Invoke(nameof(OffObj), 2f);
                return;
            }
            TestSocketIO.Instace.playTime = tData.time;
            DataManager.Instance.playerNo = 0;
            DataManager.Instance.diceManageCnt = 0;
            DataManager.Instance.tournamentID = tData._id;
            DataManager.Instance.tourEntryMoney = tData.betAmount;
            DataManager.Instance.winAmount = tData.totalWinAmount * 1;
            //DataManager.Instance.tourBonuseCutAmount = tData.bonusAmountDeduction;

            BotManager.Instance.isBotAvalible = tData.bot;
            int complex = tData.complexity;
            //print("Cnt Move : "+cntMove+" Complex : " + complex);

            if (complex == 1)
            {
                BotManager.Instance.botType = BotType.Easy;
            }
            else if (complex == 2)
            {
                BotManager.Instance.botType = BotType.Medium;
            }
            else if (complex == 3)
            {
                BotManager.Instance.botType = BotType.Hard;
            }

            Debug.Log("*****Joining*****");
            TestSocketIO.Instace.SnakeJoinRoom();
        }


    }

    #region Old Tour Maintain

    public void PlayerIncrease()
    {
        if (playerCnt >= 0 && playerCnt < 10)
        {
            //print("Enter the conditon");
            playerCnt++;
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(0.5f, 1f));
        }
        else if (playerCnt >= 10 && playerCnt < 30)
        {
            playerCnt += UnityEngine.Random.Range(2, 6);
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(1f, 2f));
        }
        else if (playerCnt >= 30 && playerCnt < 60)
        {
            playerCnt += UnityEngine.Random.Range(4, 9);
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(1.5f, 3f));
        }
        else if (playerCnt >= 60 && playerCnt < 100)
        {
            playerCnt += UnityEngine.Random.Range(8, 14);
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(2f, 4f));
        }
        else if (playerCnt >= 100 && playerCnt < 200)
        {
            playerCnt += UnityEngine.Random.Range(10, 20);
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(2.5f, 4.5f));
        }
        else if (playerCnt >= 200 && playerCnt < 350)
        {
            playerCnt += UnityEngine.Random.Range(15, 25);
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(3f, 5f));
        }
        else if (playerCnt >= 350)
        {
            playerCnt += UnityEngine.Random.Range(15, 25);
            playerCntTxt.text = playerCnt + "+";
            Invoke(nameof(PlayerIncrease), UnityEngine.Random.Range(3.5f, 6f));
        }
    }


    public void GetDiffMinute()
    {
        flag = 0;
        //int createHour = int.Parse(createDate.Split("T")[1].Split(":")[0]);
        int createHour = 0;
        int createMinute = int.Parse(tData.createdAt.Split("T")[1].Split(":")[1]);
        int createSecond = int.Parse(tData.createdAt.Split("T")[1].Split(":")[2].Split(".")[0]);

        DateTime date = DateTime.Now;
        string curDate = date.ToString();
        int currHour = int.Parse(curDate.Split(" ")[1].Split(":")[0]);
        int currMinute = int.Parse(curDate.Split(" ")[1].Split(":")[1]);
        int currSecond = int.Parse(curDate.Split(" ")[1].Split(":")[2]);

        //print("Current Hour : " + currHour);
        //print("Current Minute : " + currMinute);
        //print("Current Second : " + currSecond);

        DateTime dateTime1 = DateTime.Parse(createHour + ":" + createMinute + ":" + createSecond);
        DateTime dateTime2 = DateTime.Parse(currHour + ":" + currMinute + ":" + currSecond);

        var diff = (dateTime2 - dateTime1).TotalSeconds;
        //print("Before Diff : " + diff);
        string changeString = diff.ToString();
        char[] ch = changeString.ToCharArray();
        if (ch[0] == '-')
        {
            changeString = changeString.Substring(1, changeString.Length - 1);
        }
        long diffInSeconds = long.Parse(changeString);
        long diff1 = 0;
        if (diffInSeconds != 0)
        {
            //print("Diff In Second : " + diffInSeconds);
            //print("Interveal : " + interval);
            diff1 = diffInSeconds % (tData.interval * 10);
        }
        //secondsCount = ((interval * 60) - (int)diff1);

        //print("Main : " + );
        secondsCount = Mathf.Abs((int)diff1 - (tData.interval * 10));

        //print("Main : " + );
        //print("Date Diff Second : " + diffInSeconds);


        //print("Main : " + );
        //print("Date Diff Second : " + diffInSeconds);


    }

    void Timer()
    {
        if (flag == 0)
        {
            secondsCount -= Time.deltaTime;
            float minutes = Mathf.Floor(secondsCount / 60);
            float seconds = secondsCount % 60;


            string Min = minutes.ToString();
            string Sec = Mathf.RoundToInt(seconds).ToString();
            if (Min.Length == 1)
            {
                Min = "0" + Min;
            }
            if (Sec.Length == 1)
            {
                Sec = "0" + Sec;
            }
            if (Min.Length != 1 && Sec.Length != 1)
            {
                Min = Min;
                Sec = Sec;
            }

            string timeValue = Min + " min " + Sec + "s";
            if (timeValue.Equals("00 min 00s"))
            {
                print("Time Over");
                if (joinBtn.transform.GetChild(0).GetComponent<Text>().text == "Joined")
                {
                    MainMenuManager.Instance.SetSnakeGame(joinBtn.transform.GetChild(0).GetComponent<Text>());
                    // MainMenuManager.Instance.OpenTournamentLoadScreen();
                }
                timer.text = "00 min 00s";
                flag = 1;
            }
            if (flag != 1)
            {
                int s = int.Parse(Sec);
                if (s <= 5)
                {
                    timerBox.color = timerBoxRedColor;
                }
                else if (s <= 5 && MainMenuManager.Instance.isPressJoin)
                {
                    //Open Forxe
                    //TournamentScreen.Instance.tournamentWait.SetActive(true);
                }
                else
                {
                    timerBox.color = timerBoxNormalColor;
                }
                timer.text = timeValue;
            }
        }
        else
        {
            GetDiffMinute();
        }
    }

    #region  Application Pause
    private void OnApplicationPause(bool pause)
    {
        //print("Pause : " + pause);
        if (pause)
        {
        }
        else
        {

            GetDiffMinute();

        }
    }




    void GetDiffPause()
    {



        GetDiffMinute();

    }



    #endregion

    #endregion
}
