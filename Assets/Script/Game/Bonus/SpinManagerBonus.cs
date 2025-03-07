using System.Collections;
using UnityEngine;
using EasyUI.PickerWheelUIBonus;
using UnityEngine.UI;
using System;

public class SpinManagerBonus : MonoBehaviour
{

    public static SpinManagerBonus instance;

    [SerializeField] private Button uiSpinButton;
    // [SerializeField] private Text uiSpinButtonText;

    [SerializeField] private PickerWheelBonus pickerWheel;
    private int _numberOfTurns;
    public Text turnsText;
    public GameObject popupObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {
        // _numberOfTurns = PlayerPrefs.GetInt("RemainingTurns", 3);
        // UpdateTurnsText();

        uiSpinButton.onClick.AddListener(() =>
        {
            Debug.Log("Click Bonus  => " + DataManager.Instance.playerData.bonus);
            if (int.Parse(DataManager.Instance.playerData.bonus) < 50) return;
            if (IsCooldownActive()) return;
            DataManager.Instance.BonusDebitAmount(50.ToString());

            SoundManager.Instance.ButtonClick();
            uiSpinButton.interactable = false;
            // uiSpinButtonText.text = "";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log(
                   @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
                   + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
                );
                UserEarnManage(wheelPiece.Index);
                uiSpinButton.interactable = true;
                // uiSpinButtonText.text = "SPIN";
            });

            pickerWheel.Spin();

        });
        UpdateTurnsText();
    }


    void UserEarnManage(int index)
    {


        int winMoney = 0;

        if (index == 0)
        {
            winMoney = 2;
        }
        else if (index == 1)
        {
            winMoney = 3;
        }
        else if (index == 2)
        {
            winMoney = 5;
        }
        else if (index == 3)
        {
            winMoney = 10;
        }
        else if (index == 4)
        {
            winMoney = 12;
        }
        else if (index == 5)
        {
            winMoney = 15;
        }
        else if (index == 6)
        {
            winMoney = 20;
        }
        else if (index == 7)
        {
            winMoney = 25;
        }
        else if (index == 8)
        {
            winMoney = 30;
        }
        else if (index == 9)
        {
            winMoney = 40;
        }
        else if (index == 10)
        {
            winMoney = 50;
        }
        else if (index == 11)
        {
            winMoney = 100;
        }

        winMoney = winMoney * 1;
        Debug.Log("winMoney  = > " + winMoney);
        int lastDate = -1;
        for (int i = 0; i < DataManager.Instance.thisMonthDays; i++)
        {
            int getDayValue = DataManager.Instance.GetDayValue(i);
            if (getDayValue != 0)
            {
                lastDate = i;
            }
            else if (lastDate == 0)
            {
                break;
            }
        }
        Debug.Log("LAst date =>  " + lastDate);
        if ((lastDate + 1) == DataManager.Instance.thisMonthDays)
        {
            //Clear Playerprefs
            for (int i = 0; i < DataManager.Instance.thisMonthDays; i++)
            {
                DataManager.Instance.SetDayValue(i, 0);
            }
            //DailyReward.Instance.ClaimButton();
            MainMenuManager.Instance.GenerateSpinDialogPrefab(winMoney, true);
            //DataManager.Instance.AddAmount(winMoney, "spinwin", "Spin Reward", "won", 0, 0);

            // DataManager.Instance.BonusDebitAmount_Credit((winMoney / 1).ToString(), "Spin Reward", "won");

            DataManager.Instance.AddAmountBonus((float)((winMoney / 1)));
            StartCooldown();
            Debug.Log("winMoney  =>  " + (winMoney / 1));

        }
        else
        {
            DataManager.Instance.SetDayValue(lastDate + 1, 1);
            DataManager.Instance.SetDayRewardValue(lastDate + 1, winMoney);
            //DailyReward.Instance.ClaimButton();
            MainMenuManager.Instance.GenerateSpinDialogPrefab(winMoney, true);
            DataManager.Instance.AddAmountBonus((float)((winMoney / 1)));
            StartCooldown();
            //DataManager.Instance.AddAmount(winMoney, "spinwin", "Spin Reward", "won", 0, 0);
        }
        //  SetTurnsToZeroOnWin();

    }


    private void DecreaseTurn()
    {
        if (_numberOfTurns > 0)
        {
            Debug.Log("_numberOfTurns  = > " + _numberOfTurns);

            _numberOfTurns--;
            Debug.Log("_numberOfTurns 1  = > " + _numberOfTurns);
            PlayerPrefs.SetInt("RemainingTurns", _numberOfTurns);
            Debug.Log("_numberOfTurns 2  = > " + PlayerPrefs.GetInt("RemainingTurns"));
            UpdateTurnsText();

            if (_numberOfTurns == 0)
            {
                StartCoroutine(ShowPopupAndReset());
            }
        }
    }

    private void SetTurnsToZeroOnWin()
    {
        _numberOfTurns = 0;
        PlayerPrefs.SetInt("RemainingTurns", _numberOfTurns);
        Debug.Log("NUM TIME ZERO =" + PlayerPrefs.GetInt("RemainingTurns"));
    }

    void UpdateTurnsText()
    {
        turnsText.text = "Remaining Turns : <b>" + _numberOfTurns + "</b>";
    }

    IEnumerator ShowPopupAndReset()
    {
        popupObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupObject.SetActive(false);

        MainMenuManager.Instance.CloseSpinnerWheel();
        UpdateTurnsText();
    }


    public Button spinButton;   // UI Spin Button
    public Text timerText;      // UI Timer Text

    private DateTime nextSpinTime;
    public GameObject timertextOb;
    void Update()
    {
        if (IsCooldownActive())
        {
            timerText.text = "";
            timertextOb.SetActive(true);
            TimeSpan timeLeft = nextSpinTime - DateTime.Now;
            timerText.text = $"Next spin in: {timeLeft.Hours}h : {timeLeft.Minutes}m : {timeLeft.Seconds}s";
            spinButton.interactable = false;
        }
        else
        {
            // timerText.text = "Spin Ready!";
            timertextOb.SetActive(false);
            spinButton.interactable = true;
        }

        if (int.Parse(DataManager.Instance.playerData.bonus) < 50)
        {
            timerText.text = "";
            timertextOb.SetActive(true);
            timerText.text = "Bonus is low, Minimum 50 bonus is required.";
        }
        else
        {
            timertextOb.SetActive(false);
        }
    }
    void StartCooldown()
    {
        nextSpinTime = DateTime.Now.AddHours(3);
        PlayerPrefs.SetString("NextSpinTime", nextSpinTime.ToString());
        PlayerPrefs.Save();
    }

    bool IsCooldownActive()
    {
        return DateTime.Now < nextSpinTime;
    }

    public void LoadCooldown()
    {
        if (PlayerPrefs.HasKey("NextSpinTime"))
        {
            nextSpinTime = DateTime.Parse(PlayerPrefs.GetString("NextSpinTime"));
        }
        else
        {
            nextSpinTime = DateTime.Now;
        }
    }
}
