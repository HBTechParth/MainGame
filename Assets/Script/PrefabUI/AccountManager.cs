using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    public GameObject withdrawwindow;
    public GameObject balancewindow;
    public GameObject cashbackwindow;
    public GameObject bonuswindow;
    public Text winningBalance;
    public Text balWinningBalance;
    public Text totalBalance;
    public Text totalWinning;
    public Text totalBonus;
    
    // Start is called before the first frame update
    void Start()
    {
       LoadData();
    }

    private void LoadData()
    {
        string winningAmount = DataManager.Instance.playerData.winings;
        string bonusAmount = DataManager.Instance.playerData.bonus;
        string balanceAmount = DataManager.Instance.playerData.balance;
        winningBalance.text = "WITHDRAWABLE BALANCE: ₹ " + winningAmount;
        balWinningBalance.text = "WITHDRAWABLE BALANCE: ₹ " + winningAmount;
        totalBalance.text = balanceAmount;
        totalWinning.text = winningAmount;
        totalBonus.text = bonusAmount;
    }

    public void OpenWithDraw()
    {
        CloseAll();
        withdrawwindow.SetActive(true);
    }
    public void OpenBalance()
    {
        CloseAll();
        balancewindow.SetActive(true);
    }
    public void OpenCashback()
    {
        CloseAll();
        cashbackwindow.SetActive(true);
    }
    public void OpenBonus()
    {
        CloseAll();
        bonuswindow.SetActive(true);
    }

    public void CloseAll()
    {
        bonuswindow.SetActive(false);
        cashbackwindow.SetActive(false);
        withdrawwindow.SetActive(false);
        balancewindow.SetActive(false);
    }

    public void CloseAccountDialog()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        MainMenuManager.Instance.UpdateAllData();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

}
