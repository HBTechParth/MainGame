using System.Collections;
using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;


public class SpinManager : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Text uiSpinButtonText;

    [SerializeField] private PickerWheel pickerWheel;
    private int _numberOfTurns;
    public Text turnsText;
    public GameObject popupObject;


    private void Start()
    {
        _numberOfTurns = PlayerPrefs.GetInt("RemainingTurns", 3);
        UpdateTurnsText();

        uiSpinButton.onClick.AddListener(() =>
        {
            Debug.Log("Click");
            SoundManager.Instance.ButtonClick();
            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "";

            pickerWheel.OnSpinEnd(wheelPiece =>
            {
                Debug.Log(
                   @" <b>Index:</b> " + wheelPiece.Index + "           <b>Label:</b> " + wheelPiece.Label
                   + "\n <b>Amount:</b> " + wheelPiece.Amount + "      <b>Chance:</b> " + wheelPiece.Chance + "%"
                );
                UserEarnManage(wheelPiece.Index);
                uiSpinButton.interactable = true;
                uiSpinButtonText.text = "SPIN";
            });

            pickerWheel.Spin();

        });
        UpdateTurnsText();
    }


    void UserEarnManage(int index)
    {

        if (index is 0 or 2 or 4 or 6 or 8 or 10)
        {
            //Free Spin
            DecreaseTurn();
            Debug.Log("INDEX  = > " + index);
        }
        else
        {
            int winMoney = 0;

            if (index == 1)
            {
                winMoney = 1;
            }
            else if (index == 3)
            {
                winMoney = 5;
            }
            else if (index == 5)
            {
                winMoney = 10;
            }
            else if (index == 7)
            {
                winMoney = 20;
            }
            else if (index == 9)
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
            if ((lastDate + 1) == DataManager.Instance.thisMonthDays)
            {
                //Clear Playerprefs
                for (int i = 0; i < DataManager.Instance.thisMonthDays; i++)
                {
                    DataManager.Instance.SetDayValue(i, 0);
                }
                //DailyReward.Instance.ClaimButton();
                MainMenuManager.Instance.GenerateSpinDialogPrefab(winMoney);
                //DataManager.Instance.AddAmount(winMoney, "spinwin", "Spin Reward", "won", 0, 0);

                DataManager.Instance.BonusDebitAmount_Credit((winMoney / 1).ToString(), "Spin Reward", "won");

            }
            else
            {
                DataManager.Instance.SetDayValue(lastDate + 1, 1);
                DataManager.Instance.SetDayRewardValue(lastDate + 1, winMoney);
                //DailyReward.Instance.ClaimButton();
                MainMenuManager.Instance.GenerateSpinDialogPrefab(winMoney);
                DataManager.Instance.BonusDebitAmount_Credit((winMoney / 1).ToString(), "Spin Reward", "won");
                //DataManager.Instance.AddAmount(winMoney, "spinwin", "Spin Reward", "won", 0, 0);
            }
            SetTurnsToZeroOnWin();
        }
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
        Debug.Log("NUM TIME ZERO ="+PlayerPrefs.GetInt("RemainingTurns"));
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

}
