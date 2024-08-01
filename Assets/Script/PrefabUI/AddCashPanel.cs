using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class AddCashPanel : MonoBehaviour
{
    public static AddCashPanel Instance;
    //public Text addcashBalanceTxt;
    //public InputField addCashCustomAmount;

    //public GameObject dkcPanel;
    //public GameObject cashPanel;
    //public GameObject depositPanelObj;
    //public GameObject withdrawPanelObj;
    //public GameObject processedScreen;
    //public GameObject tranactionEntryScreen;
    //public GameObject sendDataScreen;
    public GameObject successScreen;
    public GameObject failedScreen;
    public InputField transactionId;
    //public Text transactionAmount;
    //public float selectedAmount;
    //public Image depositBtn;
    //public Image withdrawBtn;
    //public Sprite simpleGreenBtn;
    //public Sprite simpleGreenOffBtn;
    //public Text switchBtnTxt;

    //public GameObject premiumScreenObj;
    //public GameObject addCashPanelParent;
    //public Text addCash_ApplyText;
    //public Text addCash_CouponText;
    //float addcash_amount = 0;
    public GameObject errorText;
    //private bool _isDefaultAmountClicked = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //MainMenuManager.Instance.screenObj.Add(this.gameObject);
        //addcash_amount = 0;
        //string cashBalance = DataManager.Instance.playerData.balance;
        //addcashBalanceTxt.text = "₹ " + cashBalance;

        ////dkcPanel.SetActive(true);
        ////cashPanel.SetActive(false);

        //dkcPanel.SetActive(false);
        //cashPanel.SetActive(true);
        ////switchBtnTxt.text = "Switch To DKC";
        //OpenDKC();
        ////CashFreeManage.Instance.couponId = "";
        //addCash_ApplyText.gameObject.SetActive(true);
        //addCash_CouponText.gameObject.SetActive(false);
        //sendDataScreen.gameObject.SetActive(true);
        //successScreen.gameObject.SetActive(false);
    }


    void OpenDKC()
    {
        // depositPanelObj.SetActive(true);
        //withdrawPanelObj.SetActive(false);

        //depositBtn.sprite = simpleGreenBtn;
        //withdrawBtn.sprite = simpleGreenOffBtn;
    }


    //public void AddCash_CustomAmount_Dynamic(string str)
    //{
    //    if (str.IsNullOrEmpty())
    //    {
    //        //CashFreeManage.Instance.couponId = "";
    //        UpdateCouponName();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < MainMenuManager.Instance.couponDatas.Count; i++)
    //        {
    //            if (CashFreeManage.Instance.couponId.Equals(MainMenuManager.Instance.couponDatas[i].id))
    //            {
    //                CashFreeManage.Instance.couponId = MainMenuManager.Instance.couponDatas[i].id;
    //                float amount = float.Parse(GetMessageCheck());
    //                float minAmount = MainMenuManager.Instance.couponDatas[i].minAmount;
    //                float maxAmount = MainMenuManager.Instance.couponDatas[i].maxAmount;
    //                if (amount >= minAmount && amount <= maxAmount)
    //                {

    //                }
    //                else
    //                {
    //                    CashFreeManage.Instance.couponId = "";
    //                    UpdateCouponName();
    //                }
    //            }
    //        }
    //    }
    //}

    //public void UpdateCouponName()
    //{
    //    if (!CashFreeManage.Instance.couponId.IsNullOrEmpty())
    //    {
    //        for (int i = 0; i < MainMenuManager.Instance.couponDatas.Count; i++)
    //        {
    //            if (CashFreeManage.Instance.couponId.Equals(MainMenuManager.Instance.couponDatas[i].id))
    //            {
    //                CashFreeManage.Instance.couponId = MainMenuManager.Instance.couponDatas[i].id;
    //                addCash_ApplyText.gameObject.SetActive(false);
    //                addCash_CouponText.gameObject.SetActive(true);
    //                addCash_CouponText.text = MainMenuManager.Instance.couponDatas[i].name + " Applied";
    //            }
    //        }
    //    }
    //    else
    //    {
    //        addCash_ApplyText.gameObject.SetActive(true);
    //        addCash_CouponText.gameObject.SetActive(false);
    //    }
    //}

    //void OpenCashTime()
    //{

    //    addcash_amount = 0;
    //    string cashBalance = DataManager.Instance.playerData.balance;
    //    addcashBalanceTxt.text ="₹ " +  cashBalance.ToString();
    //}

    //public void UpdateCashBalance()
    //{
    //    string cashBalance = DataManager.Instance.playerData.balance;
    //    addcashBalanceTxt.text = "₹ " +  cashBalance.ToString();
    //}

    //public void AddCash_Crypto_ButtonClick(int no)
    //{
    //    SoundManager.Instance.ButtonClick();

    //    if (no == 1)
    //    {
    //        //depositPanelObj.SetActive(true);
    //        withdrawPanelObj.SetActive(false);

    //        depositBtn.sprite = simpleGreenBtn;
    //        withdrawBtn.sprite = simpleGreenOffBtn;
    //    }
    //    else if (no == 2)
    //    {

    //        depositPanelObj.SetActive(false);
    //        withdrawPanelObj.SetActive(true);

    //        depositBtn.sprite = simpleGreenOffBtn;
    //        withdrawBtn.sprite = simpleGreenBtn;
    //    }
    //}

    //public void ActiveProcessScreen()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    transactionAmount.text = "₹ " + selectedAmount.ToString(CultureInfo.InvariantCulture);
    //    processedScreen.gameObject.SetActive(true);
    //    _isDefaultAmountClicked = true;
    //}
    
    //public void ActiveProcessScreenForCustomAmount()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    if (string.IsNullOrEmpty(addCashCustomAmount.text))
    //    {
    //        // If input field is empty, show error message
    //        StartCoroutine(ShowErrorMessage("Please enter an amount.", 2f));
    //        return;
    //    }
    //    transactionAmount.text ="₹ " +  addCashCustomAmount.text;
    //    //processedScreen.gameObject.SetActive(true);
    //    _isDefaultAmountClicked = false;
    //}

    //public void ProceedButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    if (_isDefaultAmountClicked)
    //    {
    //        print("coupon Id Send Time : " + CashFreeManage.Instance.couponId);
    //        StartCoroutine(CashFreeManage.Instance.getToken((int)addcash_amount, CashFreeManage.Instance.couponId));
    //    }
    //    else
    //    {
    //        AddCash_Custom_ButtonClick();
    //    }

    //    StartCoroutine(ActivateTranactionScreen());
    //}
    
    
    IEnumerator ActivateTranactionScreen()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Activate the GameObject
        //tranactionEntryScreen.SetActive(true);
    }


    //private void AddCash_Custom_ButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    if (addCashCustomAmount.text.Length == 0)
    //    {
    //        //print("MSG Please Enter a Custome Amount");
    //    }
    //    else
    //    {
    //        addcash_amount = float.Parse(addCashCustomAmount.text);
    //        addCashCustomAmount.text = "";

    //        //StartCoroutine(CashFreeManage.Instance.getToken((int)addcash_amount)));
    //        print("coupon Id Send Time : " + CashFreeManage.Instance.couponId);
    //        StartCoroutine(CashFreeManage.Instance.getToken((int)addcash_amount, CashFreeManage.Instance.couponId));


    //    }

    //    if (addcash_amount != 0)
    //    {
    //        // print("Add Cash Click : " + addcash_amount);
    //    }
    //}

    //public void AddCash_Offer_ButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    if (addCashCustomAmount.text.Length == 0)
    //    {
    //        addcash_amount = 0;
    //    }
    //    else
    //    {
    //        addcash_amount = float.Parse(addCashCustomAmount.text);
    //    }
    //    MainMenuManager.Instance.GenerateCoupon(addcash_amount);

    //}

    //public void SwitchButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    if (switchBtnTxt.text == "Switch To INR")
    //    {
    //        switchBtnTxt.text = "Switch To DKC";
    //        //dkcPanel.SetActive(false);
    //        //cashPanel.SetActive(true);
    //        OpenCashTime();
    //    }
    //    else if (switchBtnTxt.text == "Switch To DKC")
    //    {
    //        switchBtnTxt.text = "Switch To INR";
    //        //dkcPanel.SetActive(true);
    //        //cashPanel.SetActive(false);
    //    }
    //}




    public void AddCash_Explore_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //if (addCashCustomAmount.text.Length == 0)
        //{
        //    print("MSG Please Enter a Custome Amount");
        //}
        //else
        //{
        //    addcash_amount = float.Parse(addCashCustomAmount.text);
        //    StartCoroutine(CashFreeManage.Instance.getToken((int)addcash_amount));

        //}
        //Instantiate(premiumScreenObj, addCashPanelParent.transform);

        //if (addcash_amount != 0)
        //{
        //    print("Add Cash Click : " + addcash_amount);
        //}
    }



    //public void Addcash_Money_ButtonClick(int no)
    //{
    //    SoundManager.Instance.ButtonClick();
    //    addcash_amount = no switch
    //    {
    //        1 => 100,
    //        2 => 500,
    //        3 => 1000,
    //        4 => 2500,
    //        _ => addcash_amount
    //    };
    //    selectedAmount = addcash_amount;
    //    //ActiveProcessScreen();
    //    //StartCoroutine(CashFreeManage.Instance.getToken((int)addcash_amount, "none"));
    //}

    //public void ApplyButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();

    //    Instantiate(MainMenuManager.Instance.couponApplyPanel, MainMenuManager.Instance.parentObj.transform);

    //}

    //public string GetMessageCheck()
    //{

    //    return addCashCustomAmount.text;
    //}


    //public void UpdateAddCash()
    //{
    //    string cashBalance = DataManager.Instance.playerData.balance;
    //    addcashBalanceTxt.text = "₹ " +  cashBalance.ToString();
    //}

    //public void BackButtonClick()
    //{
    //    SoundManager.Instance.ButtonClick();
    //    MainMenuManager.Instance.screenObj.Remove(this.gameObject);
    //    MainMenuManager.Instance.TopBarDataSet();
    //    this.gameObject.SetActive(false);
    //    Destroy(this.gameObject);

    //}

    private IEnumerator ShowErrorMessage(string message, float duration)
    {
        errorText.SetActive(true);
        errorText.GetComponent<Text>().text = message;

        yield return new WaitForSeconds(duration);

        errorText.SetActive(false);
    }

    public void SubmitButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        StartCoroutine(SubmitTransId());
    }


    IEnumerator SubmitTransId()
    {
        if (string.IsNullOrEmpty(transactionId.text))
        {
            // If input field is empty, show error message
            StartCoroutine(ShowErrorMessage("Please enter ID.", 2f));
            yield break;
        }
        WWWForm tokeform = new WWWForm();
        tokeform.AddField("id", DataManager.Instance.cashFreeId);
        tokeform.AddField("paymentId", transactionId.text);
        

        //WaitPanelManager.Instance.OpenPanel();
        UnityWebRequest unityWeb = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/paymentadd", tokeform);
        unityWeb.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return unityWeb.SendWebRequest();

        if (unityWeb.isNetworkError || unityWeb.isHttpError)
        {
            Debug.Log(unityWeb.error);
        }
        else
        {

            JSONNode token = SimpleJSON.JSON.Parse(unityWeb.downloadHandler.text);
            print("data Cash Free:::::" + token["data"].ToString());
            bool success = token["success"].AsBool;
            if (!success)
            {
                failedScreen.gameObject.SetActive(true);
                yield break;
            }
            //WaitPanelManager.Instance.ClosePanel();

            //sendDataScreen.gameObject.SetActive(false);
            successScreen.gameObject.SetActive(true);
            //StartCoroutine(TurnOffTranactionScreen());
        }
    }
    
    IEnumerator TurnOffTranactionScreen()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(3f);

        // Activate the GameObject
        //tranactionEntryScreen.SetActive(false);
        //processedScreen.gameObject.SetActive(false);
    }
}
