using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public static DailyReward Instance;

    public GameObject connectServerObj;
    public GameObject sessionRestoreObj;
    //public Button claimBtn;
    //public Text Showtxt;
    public string SystemTime = "", GameTime = "";
    int flag;
    int[] novalue = { 3600, 60, 1 };
    public float secondsCount; // 60 1 minute // 3600 hours // 86400 1 days
    public int totalDayDiff;
    public Text timeShow;
    bool isPause;

    public Button isClaimButton;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    void Start()
    {
        //if(Instance == null)
        //{
        //    Instance = this; 
        //}
        int DailyRewardValue = PlayerPrefs.GetInt("DailyRewardValue", 0);
        print("Daily Reward Value : "+DailyRewardValue);
        if (DailyRewardValue != 1)
        {
            isClaimButton.interactable = false;
            TimeStartHandle();
        }
        else 
        {
            timeShow.text = "00:00:00";
            isClaimButton.interactable = true;
        }

     

    }

    public void OpenConnect()
    {
        connectServerObj.SetActive(true);
    }
    public void CloseConnet()
    {
        connectServerObj.SetActive(false);
    }

    
    public void OpenSession()
    {
        sessionRestoreObj.SetActive(true);
    }
    public void CloseSession()
    {
        sessionRestoreObj.SetActive(false);
    }
    void Update()
    {
        
            GetTime();
            CounterTimeFunction();
        
    }

    #region Start Time Handle

    void TimeStartHandle()
    {
        string GetSystemTime = PlayerPrefs.GetString("SystemTimeStore");
        string GetGameTime = PlayerPrefs.GetString("GameTimeStore");
        print("Get game Time : " + GetGameTime);

        //print("Get Game Time : " + GetGameTime);
        if (GetGameTime != "")
        {
            float systemValue = 0;
            float gameValue = 0;

            string[] totalsystemvaluestring = GetSystemTime.Split(':');
            string[] totalgamevaluestring = GetGameTime.Split(':');



            for (int i = 0; i < totalsystemvaluestring.Length; i++)
            {
                //print("total system string : " + totalsystemvaluestring[i]);
                if (totalsystemvaluestring.Length - 1 != i)
                {
                    systemValue = systemValue + (int.Parse(totalsystemvaluestring[i]) * novalue[i]);
                }
                else
                {
                    systemValue = systemValue + int.Parse(totalsystemvaluestring[i]);
                }
            }

            for (int i = 0; i < totalgamevaluestring.Length; i++)
            {
                //print("total game string : " + totalgamevaluestring[i]);
                if (totalgamevaluestring.Length - 1 != i)
                {
                    gameValue = gameValue + (int.Parse(totalgamevaluestring[i]) * novalue[i]);
                }
                else
                {
                    gameValue = gameValue + int.Parse(totalgamevaluestring[i]);
                }
            }

            //print("total System Value : " + systemValue);
            //print("total game value : " + gameValue);


            float latestHour, latestHourMinute, latestSecond;

            string[] LatestTimeStr = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss").Split(':');

            latestHour = float.Parse(LatestTimeStr[0]);
            latestHourMinute = float.Parse(LatestTimeStr[1]);
            latestSecond = float.Parse(LatestTimeStr[2]);

            float latestTotalValue = (latestHour * 3600) + (latestHourMinute * 60) + latestSecond;

            //print("Latest Total Value : " + latestTotalValue);

            int currentYear, currentDate, currentMonth;

            currentYear = int.Parse(System.DateTime.UtcNow.ToLocalTime().ToString("yyyy"));
            currentMonth = int.Parse(System.DateTime.UtcNow.ToLocalTime().ToString("MM"));
            currentDate = int.Parse(System.DateTime.UtcNow.ToLocalTime().ToString("dd"));

            string lastDateStr = PlayerPrefs.GetString("LastTimeDate");


            int lastYear, lastMonth, lastDay;

            string[] SplitLastDate = lastDateStr.Split('-');

            lastYear = int.Parse(SplitLastDate[0]);
            lastMonth = int.Parse(SplitLastDate[1]);
            lastDay = int.Parse(SplitLastDate[2]);



            System.DateTime datevalue1 = new System.DateTime(currentYear, currentMonth, currentDate);
            System.DateTime datevalue2 = new System.DateTime(lastYear, lastMonth, lastDay);

            int totalDays = Mathf.Abs((int)(datevalue2 - datevalue1).TotalDays);
            if (totalDays == 0)
            {
                float diff = latestTotalValue - systemValue;
                //print("diff Total Value : " + diff);
                //print("Total Days :  " + totalDays);
                secondsCount = gameValue - diff;
            }
            else if (totalDays == 1)
            {
                float diff = systemValue - latestTotalValue;
                //print("diff Total Value : " + diff);
                //print("Total Days :  " + totalDays);
                secondsCount = gameValue - diff;
            }
            else if (totalDays >= 2)
            {
                totalDayDiff = totalDays;

                PlayerPrefs.SetInt("DailyRewardValue", 1);


                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                //print("Total Day 2 or more : " + totalDays);
                //print("Direct Reward Open");
            }

        }

    }
    #endregion

    #region System Time Get & Game Time Get (Game Continue Time)
    void GetTime()
    {
        string time = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm:ss");
        SystemTime = time;
    }

    void CounterTimeFunction()
    {

        int DailyRewardValue = PlayerPrefs.GetInt("DailyRewardValue", 0);
        if (DailyRewardValue != 1)
        {
            if (secondsCount > 0)
            {
                secondsCount -= Time.deltaTime;
            }
            else
            {
                secondsCount = 0;
                
            }

            float minutes = Mathf.Floor(secondsCount / 60) % 60;
            float hours = Mathf.Floor((Mathf.Floor(secondsCount / 60)) / 60);
            float seconds = secondsCount % 60;


            string hour = hours.ToString();
            string Min = minutes.ToString();
            string Sec = Mathf.RoundToInt(seconds).ToString();

            if (hour.Length == 1)
            {
                hour = "0" + hour;
            }
            if (Min.Length == 1)
            {
                Min = "0" + Min;
            }
            if (Sec.Length == 1)
            {
                Sec = "0" + Sec;
            }
            //print(hour + ":" + minutes + ":" + seconds);
            if (Min.Length != 1 && Sec.Length != 1 && hour.Length != 1)
            {
                Min = Min;
                Sec = Sec;
                hour = hour;
            }

            if (Mathf.Abs(int.Parse(hour)).ToString().Length == 1)
            {
                hour = "0" + Mathf.Abs(int.Parse(hour)).ToString();
            }
            if (Mathf.Abs(int.Parse(Min)).ToString().Length == 1)
            {
                Min = "0" + Mathf.Abs(int.Parse(Min)).ToString();
            }
            if (Mathf.Abs(int.Parse(Sec)).ToString().Length == 1)
            {
                Sec = "0" + Mathf.Abs(int.Parse(Sec)).ToString();
            }

            string timeValue = hour + ":" + Min + ":" + Sec;
            if (timeValue.Equals("00:00:00"))
            {
                print("Time Over");
                timeShow.text = "00:00:00";

                isClaimButton.interactable = true;
                PlayerPrefs.SetInt("DailyRewardValue", 1);

               
                flag = 1;
            }
            if (flag != 1)
            {

                timeShow.text = timeValue;
                GameTime = timeValue;
            }
        }
    }
    #endregion

    #region Button Click Event
    public void addOneHours()
    {
        float no = secondsCount - 3600;
        if (no > 0)
        {
            secondsCount -= 3600;
        }
        else
        {
            print("Sorry 10 minute is not enough");
        }
    }

    public void add10Minute()
    {
        float no = secondsCount - 600;
        if (no > 0)
        {
            secondsCount -= 600;
        }
        else
        {

            print("Sorry 1 hour is not enough");
        }
    }
    public void add1Minute()
    {
        float no = secondsCount - 60;
        if (no > 0)
        {
            secondsCount -= 60;
        }
        else
        {

            print("Sorry 1 minute is not enough");
        }
    }
    public void ClaimButton()
    {
        isClaimButton.interactable = false;
        secondsCount = 86400f;
        flag = 0; 
        PlayerPrefs.DeleteKey("DailyRewardValue");
        PlayerPrefs.DeleteKey("SystemTimeStore");
        PlayerPrefs.DeleteKey("GameTimeStore");
        PlayerPrefs.DeleteKey("LastTimeDate");

     


        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Successfully Claim Success");
    }



    public void RestartTimer()
    {
        DeleteKey();
    }
    void DeleteKey()
    {
        PlayerPrefs.DeleteKey("DailyRewardValue");
        PlayerPrefs.DeleteKey("SystemTimeStore");
        PlayerPrefs.DeleteKey("GameTimeStore");
        PlayerPrefs.DeleteKey("LastTimeDate");

    }

    #endregion

    #region Application Quit and Pause Time

    private void OnApplicationQuit()
    {
        // print("Application Quit Event Enter");
        TimeCountPauseandQuit();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            isPause = true;
            // print("Application Pause Event Enter");
            TimeCountPauseandQuit();

        }
        else if(!pause)
        {
            print("Pause is not proper");
            if(isPause == true)
            {
                isPause = false;
                int DailyRewardValue = PlayerPrefs.GetInt("DailyRewardValue", 0);
                print("Daily Reward Value : " + DailyRewardValue);
                if (DailyRewardValue != 1)
                {
                    isClaimButton.interactable = false;
                    TimeStartHandle();
                }
                else
                {
                    timeShow.text = "00:00:00";
                    isClaimButton.interactable = true;
                }
            }
        }
    }

    public void TimeCountPauseandQuit()
    {
        PlayerPrefs.SetString("SystemTimeStore", SystemTime);
        PlayerPrefs.SetString("GameTimeStore", GameTime);
        //print("Store Game TIme : " + GameTime);
        PlayerPrefs.SetString("LastTimeDate", System.DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd"));
    }
    #endregion

}
