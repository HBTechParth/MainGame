using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
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

    public GameObject pleaseWaitScreen;
    public Text waitTxt;
    public GameObject scorllParent;
    public GameObject tranPrefab;

    public Color greenColor;
    public Color redColor;
    public Color yellowColor;

    public List<TransactionForBonus> transactions = new List<TransactionForBonus>();

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
        GetTransaction();
    }
    public void GetTransaction()
    {
        DestroyPrefeb();
        pleaseWaitScreen.SetActive(true);
        waitTxt.text = "Please Wait...";
        StartCoroutine(GetTransactions());
    }
    public List<GameObject> bonusObjs = new List<GameObject>();
    public void DestroyPrefeb()
    {
        for (int i = 0; i < bonusObjs.Count; i++)
        {
            Destroy(bonusObjs[i]);
        }
    }
    IEnumerator GetTransactions()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/transactions/player/bonus");

        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            print("tran Data : " + request.downloadHandler.text.ToString());
            JSONNode keys = JSON.Parse(request.downloadHandler.text.ToString());
            JSONNode data = JSON.Parse(keys["data"].ToString());
            if (data.Count == 0)
            {
                waitTxt.text = "No History...";
            }
            else
            {
                pleaseWaitScreen.SetActive(false);
                bonusObjs.Clear();

                for (int i = 0; i < data.Count; i++)
                {
                    TransactionForBonus t = new TransactionForBonus();
                    t.paymentStatus = data[i]["paymentStatus"];
                    t.logType = data[i]["logType"];
                    t._id = data[i]["_id"];
                    t.amount = data[i]["amount"];
                    t.transactionType = data[i]["transactionType"];
                    t.note = data[i]["note"];
                    t.createdAt = data[i]["createdAt"];
                    transactions.Add(t);

                    GameObject tObj = Instantiate(tranPrefab, scorllParent.transform);
                    bonusObjs.Add(tObj);

                    Text t1 = tObj.transform.GetChild(0).GetComponent<Text>();
                    Text t2 = tObj.transform.GetChild(1).GetComponent<Text>();
                    Text t3 = tObj.transform.GetChild(2).GetComponent<Text>();

                    string curDateStr = DateTime.Parse(t.createdAt).ToLocalTime().ToString();
                    DateTime dateT1 = DateTime.Parse(curDateStr.Split(" ")[0]);
                    DateTime dateT2 = DateTime.Parse(curDateStr.Split(" ")[1]);
                    //t1.text = "Joined : " + dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + " " + dateT1.ToString("yyyy") + "-" + dateT2.ToString("hh:mm tt");
                    t1.text = dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + ", " + dateT2.ToString("hh:mm tt");
                    t2.text = t.note;
                    if (t.transactionType == "debit")
                    {
                        t3.text = "-" + (t.amount).ToString("F2");
                        t1.color = redColor;
                        t2.color = redColor;
                        t3.color = redColor;
                    }
                    else if (t.transactionType == "credit" && t.paymentStatus == "SUCCESS")
                    {
                        t3.text = "+" + (t.amount).ToString("F2");
                        t1.color = greenColor;
                        t2.color = greenColor;
                        t3.color = greenColor;
                    }
                    else if (t.transactionType == "credit" && t.paymentStatus == "PROCESSING")
                    {
                        t3.text = "+" + (t.amount).ToString("F2");
                        t1.color = yellowColor;
                        t2.color = yellowColor;
                        t3.color = yellowColor;
                    }
                    else
                    {
                        t3.text = "-" + (t.amount).ToString("F2");
                        t1.color = redColor;
                        t2.color = redColor;
                        t3.color = redColor;
                    }


                }
            }
        }
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
[System.Serializable]
public class TransactionForBonus
{
    public string paymentStatus;
    public string logType;
    public string _id;
    public float amount;
    public string transactionType;
    public string note;
    public string createdAt;
    public string tournamentId;
}