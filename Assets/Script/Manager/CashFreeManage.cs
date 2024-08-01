using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class CashFreeManage : MonoBehaviour
{
    public static CashFreeManage Instance;

    //[SerializeField] private InputField amountfiled;
    //[SerializeField] private InputField note;
    [SerializeField] private string appid;
    //[SerializeField] private string secretKey;
    //[SerializeField] private GameObject sucesspopup;
    //[SerializeField] private GameObject failpopup;
    string amount_order;
    string orderid = "";
    string notifyUrl = "";
    int balance;

    public string couponId;
    public GameObject popUpObj;
    public Text popUpTitleObj;
    public GameObject successObj;
    public GameObject failedObj;
    public Text popUpTxt;
    public bool isRun;
    public bool isRun1;

    public Color greenColor;
    public Color redColor;
    private string paymentLink;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    //public void upi_payment()
    //{
    //    //StartCoroutine(getToken());
    //}

    #region Cash Free




    //public IEnumerator getToken(int amount, string couponId11)
    //{
    //    isRun = true;
    //    print("Amount : " + amount);
    //    WWWForm tokeform = new WWWForm();
    //    tokeform.AddField("amount", amount);
    //    tokeform.AddField("note", "Add Money");

    //    tokeform.AddField("coupon_id", couponId11);



    //    //string sendWinJson = JsonUtility.ToJson(tokeform);

    //    print("Coupon Manage : " + amount);
    //    print("coupon Id : " + couponId11);

    //    //WaitPanelManager.Instance.OpenPanel();
    //    UnityWebRequest unityWeb = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/payments/cashfree/token", tokeform);
    //    unityWeb.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
    //    //UnityWebRequest www = new UnityWebRequest("https://api.cashfree.com/api/v1/cftoken/order", "POST", downloadHandlerBuffer, uploadHandlerRaw);
    //    // www.SetRequestHeader("x-client-id", appid);
    //    // www.SetRequestHeader("x-client-secret", secretKey);
    //    //  print("Cash Free URl :  "+ DataManager.Instance.url + "/api/v1/payments/cashfree/token");

    //    yield return unityWeb.SendWebRequest();

    //    if (unityWeb.isNetworkError || unityWeb.isHttpError)
    //    {
    //        Debug.Log(unityWeb.error);
    //    }
    //    else
    //    {
    //        JSONNode token = SimpleJSON.JSON.Parse(unityWeb.downloadHandler.text);
    //        print("data Cash Free:::::" + token["data"].ToString());
    //        JSONNode data = SimpleJSON.JSON.Parse(token["data"].ToString());
    //        print("Status::::" + data["status"]);
    //        if (data["status"] == "OK")
    //        {
    //            //WaitPanelManager.Instance.ClosePanel();

    //            appid = data["appId"];
    //            orderid = data["orderId"];
    //            amount_order = data["orderAmount"];
    //            notifyUrl = data["notifyUrl"];
    //            StartCoroutine(upi_pay(data["cftoken"].Value));
    //            //StartCoroutine(CashFreeNotifiy(data));
    //        }
    //    }
    //}
    public IEnumerator getToken(int amount, string couponId11, bool isPhonePay)
    {
        isRun = true;
        print("Amount : " + amount);
        WWWForm tokeform = new WWWForm();
        tokeform.AddField("amount", amount);
        tokeform.AddField("note", "Add Money");
        tokeform.AddField("coupon_id", couponId11);
        //tokeform.AddField("amount", 1);
        //tokeform.AddField("email_field", "Tester@gmail.com");



        //string sendWinJson = JsonUtility.ToJson(tokeform);

        print("Coupon Manage : " + amount);
        print("coupon Id : " + couponId11);

        //WaitPanelManager.Instance.OpenPanel();
        //UnityWebRequest unityWeb = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/payments/upiman/getlink", tokeform); //Manual payment
        paymentLink = isPhonePay ? "/api/v1/payments/phonepaypg/token" : "/api/v1/payments/zeropg/token";
        UnityWebRequest unityWeb = UnityWebRequest.Post(DataManager.Instance.url + paymentLink, tokeform); // Zero payment Gateway
        //UnityWebRequest unityWeb = UnityWebRequest.Post("https://zgw.oynxdigital.com/payment1.php?i=bkGN82", tokeform); // Zero payment Gateway
        unityWeb.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //UnityWebRequest www = new UnityWebRequest("https://api.cashfree.com/api/v1/cftoken/order", "POST", downloadHandlerBuffer, uploadHandlerRaw);
        // www.SetRequestHeader("x-client-id", appid);
        // www.SetRequestHeader("x-client-secret", secretKey);
        //  print("Cash Free URl :  "+ DataManager.Instance.url + "/api/v1/payments/cashfree/token");

        yield return unityWeb.SendWebRequest();

        if (unityWeb.isNetworkError || unityWeb.isHttpError)
        {
            Debug.Log(unityWeb.error);
        }
        else
        {
            print("Success : " + unityWeb.downloadHandler.text);
            JSONNode token = JSON.Parse(unityWeb.downloadHandler.text);
            bool success = token["success"].AsBool;
            //WaitPanelManager.Instance.ClosePanel();

            if (!success) yield break;
            JSONNode data = token["data"];
            string url = "";
            
            url = isPhonePay ? data["url"] : data["response"]["upiString"]["upi_intent"]["bhim"];

            DataManager.Instance.cashFreeId = data["id"];
            // Open the URL
            Application.OpenURL(url);
            /*string tokenJson = unityWeb.downloadHandler.text;
            ProcessTokenData(tokenJson);*/
        }
    }


    /*private void ProcessTokenData(string tokenJson) //For RazorPay
    {
        var tokenData = JSON.Parse(tokenJson);

        if (tokenData["success"].AsBool)
        {
            var responseData = tokenData["data"]["response"];
            var orderId = responseData["id"].Value;
            //transactionId = orderId;
            var paymentUrl = tokenData["data"]["order"]["url"].Value;
            var keyId = tokenData["data"]["order"]["key_id"].Value;
            var amount = tokenData["data"]["order"]["amount"].AsInt;
            var userName = tokenData["data"]["order"]["name"].Value;
            var phone = tokenData["data"]["order"]["contact"].Value;
            //string image = tokenData["data"]["order"]["image"].Value;
            var image = "http://134.209.144.144/assets/img/logo/logo.png";
            var email = tokenData["data"]["order"]["email"].Value;
            //transactionId = tokenData["data"]["order"]["orderId"].Value;
            //string currency = tokenData["data"]["order"]["name"].Value;
            var currency = "LUDO BAZI";

            // Now, perform the second web request
            StartCoroutine(OpenPaymentUrl(paymentUrl, keyId, currency, amount, orderId, userName, phone, email, image));
        }
        else
        {
            Debug.LogError("Token request failed.");
        }
    }

    private IEnumerator OpenPaymentUrl(string paymentUrl, string keyId, string name, int amount, string id,
        string userName, string contact, string mail, string image)
    {
        // Construct the URL with the query string
        var urlWithParams =
            $"{paymentUrl}?key_id={keyId}&name={name}&amount={amount}&order_id={id}&prefill[name]={userName}&prefill[contact]={contact}&prefill[email]={mail}";

        // Open the URL using Application.OpenURL
        Application.OpenURL(urlWithParams);

        yield return null;
    }*/
    
    public void OpenPaymentURL(string paymentUrl, string amount, string email)
    {
        /*// Construct the URL with prefilled parameters
        string urlWithParams = $"{paymentUrl}&amount={amount}&email_field={email}";

        // Open the URL using Application.OpenURL
        print(urlWithParams);
        Application.OpenURL(urlWithParams);*/
        StartCoroutine(SendPaymentRequest(paymentUrl, amount, email));
    }
    
    IEnumerator SendPaymentRequest(string paymentUrl, string amount, string email)
    {
        // Construct the form
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("email_field", email);

        // Send the request
        using (UnityWebRequest www = UnityWebRequest.Post(paymentUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST request failed: " + www.error);
            }
            else
            {
                // Check response headers
                foreach (var header in www.GetResponseHeaders())
                {
                    Debug.Log("Header: " + header.Key + " Value: " + header.Value);
                }

                // Get the redirected URL
                string redirectUrl = www.GetResponseHeader("Location");

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    Debug.Log("Redirect URL: " + redirectUrl);
                    // Open the redirected URL using Application.OpenURL
                    Application.OpenURL(redirectUrl);
                }
                else
                {
                    Debug.LogError("Redirect URL is empty.");
                }
            }
        }
    }
    #endregion


    #region Pre

    public void SendPremium(int no)
    {
        if (no == 1)
        {
            StartCoroutine(getPreToken(49, "month"));
        }
        else if (no == 2)
        {
            StartCoroutine(getPreToken(499, "year"));
        }
    }
    public IEnumerator getPreToken(int amount, string time)
    {
        isRun1 = true;
        print("Amount : " + amount);
        WWWForm tokeform = new WWWForm();
        tokeform.AddField("amount", amount);
        tokeform.AddField("note", "Add Premium");

        tokeform.AddField("membership_id", time);

        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest unityWeb = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/payments/cashfree/token", tokeform);
        unityWeb.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //UnityWebRequest www = new UnityWebRequest("https://api.cashfree.com/api/v1/cftoken/order", "POST", downloadHandlerBuffer, uploadHandlerRaw);
        // www.SetRequestHeader("x-client-id", appid);
        // www.SetRequestHeader("x-client-secret", secretKey);
        //  print("Cash Free URl :  "+ DataManager.Instance.url + "/api/v1/payments/cashfree/token");

        yield return unityWeb.SendWebRequest();

        if (unityWeb.isNetworkError || unityWeb.isHttpError)
        {
            Debug.Log(unityWeb.error);
        }
        else
        {

            JSONNode token = SimpleJSON.JSON.Parse(unityWeb.downloadHandler.text);
            print("data Cash Free:::::" + token["data"].ToString());
            JSONNode data = SimpleJSON.JSON.Parse(token["data"].ToString());
            print("Status::::" + data["status"]);
            if (data["status"] == "OK")
            {
                //WaitPanelManager.Instance.ClosePanel();

                appid = data["appId"];
                orderid = data["orderId"];
                amount_order = data["orderAmount"];
                notifyUrl = data["notifyUrl"];
                StartCoroutine(upi_pay(data["cftoken"].Value));
                //StartCoroutine(CashFreeNotifiy(data));
            }
        }
    }
    #endregion

    #region Cash free notify
    //public IEnumerator CashFreeNotifiy(JSONNode nod)
    //{

    //    WWWForm form = new WWWForm();
    //    //tokeform.AddField("amount", amount);
    //    //tokeform.AddField("note", "Add Money");

    //    form.AddField("orderId", nod["orderId"].ToString());
    //    form.AddField("orderAmount", nod["orderAmount"].ToString());
    //    form.AddField("referenceId", nod["appId"].ToString());
    //    form.AddField("txStatus	", nod["status"].ToString());
    //    form.AddField("paymentMode", "");
    //    form.AddField("txMsg", "");
    //    form.AddField("txTime	", "");
    //    form.AddField("signature	", "");

    //    UnityWebRequest unityWeb = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/payments/cashfree/notify", form);
    //    unityWeb.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
    //    //UnityWebRequest www = new UnityWebRequest("https://api.cashfree.com/api/v1/cftoken/order", "POST", downloadHandlerBuffer, uploadHandlerRaw);
    //    // www.SetRequestHeader("x-client-id", appid);
    //    // www.SetRequestHeader("x-client-secret", secretKey);


    //    yield return unityWeb.SendWebRequest();

    //    if (unityWeb.isNetworkError || unityWeb.isHttpError)
    //    {
    //        Debug.Log(unityWeb.error);
    //    }
    //    else
    //    {

    //        JSONNode token = SimpleJSON.JSON.Parse(unityWeb.downloadHandler.text);
    //        print("data:::::" + token["data"].ToString());
    //        JSONNode data = SimpleJSON.JSON.Parse(token["data"].ToString());
    //        print("Status::::" + data["status"]);
    //        if (data["status"] == "OK")
    //        {
    //            appid = data["appId"];
    //            orderid = data["orderId"];
    //            amount_order = data["orderAmount"];
    //            notifyUrl = data["notifyUrl"];
    //            StartCoroutine(upi_pay(data["cftoken"].Value));
    //        }
    //    }
    //}

    #endregion

    IEnumerator upi_pay(string token)
    {
        print("token:::::::" + token);
        WWWForm form = new WWWForm();
        form.AddField("appId", appid);
        form.AddField("orderId", orderid);
        form.AddField("source", "app-sdk");
        form.AddField("tokenData", token);
        form.AddField("orderAmount", amount_order);
        form.AddField("orderCurrency", "INR");
        form.AddField("notifyUrl", notifyUrl);
        print("notifyUrl::::::::" + notifyUrl);
        form.AddField("orderNote", PlayerPrefs.GetString("playerid") + "_" + amount_order + "-purchase");

        if (DataManager.Instance.playerData.email.IsNullOrEmpty())
        {
            form.AddField("customerEmail", "");
        }
        else
        {
            form.AddField("customerEmail", DataManager.Instance.playerData.email);
        }



        if (DataManager.Instance.playerData.phone.IsNullOrEmpty())
        {
            form.AddField("customerPhone", "");
        }
        else
        {
            form.AddField("customerPhone", DataManager.Instance.playerData.phone);
        }
        //form.AddField("customerPhone", DataManager.Instance.playerData.phone);


        //form.AddField("customerName", PlayerPrefs.GetString("playerid"));

        //https://test.cashfree.com/api/v1/cftoken/order' i have used  this for token
        //https://api.cashfree.com/billpay/checkout/post/upi-submit
        //WaitPanelManager.Instance.OpenPanel();

        using (UnityWebRequest www = UnityWebRequest.Post("https://www.cashfree.com/checkout/post/upi-submit", form))
        {

            print("web Request sended::::");
            yield return www.SendWebRequest();
            print("web Request sended::::1");
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                //loader.SetActive(false);
                //transaction_message.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "Alert";
                //transaction_message.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Check Internet Connection";
                //transaction_message.transform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = fail;
                //transaction_message.SetActive(true);
            }
            else
            {
                print("jsonNode : " + www.downloadHandler.text);

                JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text);

                if (jsonNode["status"] == "OK")
                {
                    //WaitPanelManager.Instance.ClosePanel();

                    PlayerPrefs.SetString("orderid", orderid);
                    Debug.Log("UPI Respone:::::::::::::" + jsonNode.ToString());
                    Application.OpenURL(jsonNode["payLink"].Value);
                    //loader.SetActive(false);

                }

            }
        }
    }

    #region Add Money Main

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (isRun)
            {
                //StartCoroutine(getorderstatus());
                GetOrderStatus();
            }
            else if (isRun1)
            {
                StartCoroutine(GetMembershipStatus());
            }
        }
    }

    //void OnApplicationPause(bool paused)
    //{
    //    // Display the app open ad when the app is foregrounded
    //    if (!paused)
    //    {
    //        StartCoroutine(getorderstatus());
    //    }
    //}

    /*IEnumerator getorderstatus()
    {
        WWWForm form = new WWWForm();
        form.AddField("orderId", orderid);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/addMoney", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        Debug.Log("order status:::::::" + request.downloadHandler.text);
        JSONNode node = SimpleJSON.JSON.Parse(request.downloadHandler.text);
        if (node["success"] == true)
        {
            JSONNode data = SimpleJSON.JSON.Parse(node["data"].ToString());

            print("Cashfree Data Balance 1: " + (float.Parse(data["balance"].ToString()) * 10));
            print("Cash Free Org Balance 2: " + DataManager.Instance.playerData.balance);

            if ((float.Parse(data["balance"].ToString())).ToString("F2") != DataManager.Instance.playerData.balance)
            {
                isRun = false;
                DataManager.Instance.playerData.balance = data["balance"].ToString();
                DataManager.Instance.playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
                DataManager.Instance.playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
                DataManager.Instance.playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];

                if (MainMenuManager.Instance != null)
                {
                    MainMenuManager.Instance.Getdata();
                }

                popUpObj.SetActive(true);
                popUpTitleObj.text = "Transaction Success";
                popUpTitleObj.color = greenColor;
                successObj.SetActive(true);
                failedObj.SetActive(false);
                popUpTxt.text = "Transaction Success \n₹" + amount_order.ToString() + "\nSucessfully loaded";

                //if (MainMenuManager.Instance.screenObj.Count != 0)
                //{
                //    for (int i = 0; i < MainMenuManager.Instance.screenObj.Count; i++)
                //    {
                //        if (MainMenuManager.Instance.screenObj[i].GetComponent<AddCashPanel>())
                //        {
                //            MainMenuManager.Instance.screenObj[i].GetComponent<AddCashPanel>().UpdateAddCash();
                //        }
                //    }
                //}
                //else
                //{
                //    MainMenuManager.Instance.UpdateAddCash();
                //}

                if (MainMenuManager.Instance != null)
                {
                    MainMenuManager.Instance.UpdateAllData();
                }
                //PlayerInfoUI.Instance.balance.text = Datamanger.Intance.balance.ToString();
            }
            else
            {

                isRun = false;
                popUpObj.SetActive(true);
                popUpTitleObj.text = "Transaction Failed";
                popUpTitleObj.color = redColor;
                successObj.SetActive(false);
                failedObj.SetActive(true);
                popUpTxt.text = "Transaction Fail \n₹" + amount_order.ToString() + "\nfail to load";
            }
        }

    }*/

    private void GetOrderStatus()
    {
        isRun = false;
        popUpObj.SetActive(true);
        //popUpTitleObj.text = "Processing";
        popUpTitleObj.color = greenColor;
        successObj.SetActive(true);
        failedObj.SetActive(false);
        //popUpTxt.text = "Transaction Will get \n₹" + amount_order.ToString() + "\nUpdate soon!";
    }


    IEnumerator GetMembershipStatus()
    {
        WWWForm form = new WWWForm();
        form.AddField("orderId", orderid);
        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/membership", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();
        Debug.Log("order status:::::::" + request.downloadHandler.text);
        JSONNode node = SimpleJSON.JSON.Parse(request.downloadHandler.text);
        if (node["success"] == true)
        {
            //WaitPanelManager.Instance.ClosePanel();

            JSONNode data = SimpleJSON.JSON.Parse(node["data"].ToString());

            if (data[DataManager.Instance.playerData.membership].ToString() != "free")
            {
                isRun1 = false;
                DataManager.Instance.playerData.balance = data["balance"].ToString();
                DataManager.Instance.playerData.deposit = data[nameof(DataManager.Instance.playerData.deposit)];
                DataManager.Instance.playerData.winings = data[nameof(DataManager.Instance.playerData.winings)];
                DataManager.Instance.playerData.bonus = data[nameof(DataManager.Instance.playerData.bonus)];
                DataManager.Instance.playerData.membership = data[nameof(DataManager.Instance.playerData.membership)];

                popUpObj.SetActive(true);
                popUpTitleObj.text = "Transaction Success";
                popUpTitleObj.color = greenColor;
                successObj.SetActive(true);
                failedObj.SetActive(false);
                popUpTxt.text = "Transaction Success \n₹" + amount_order.ToString() + "\nSucessfully loaded";



                if (MainMenuManager.Instance != null)
                {
                    MainMenuManager.Instance.UpdateAllData();
                }
                //PlayerInfoUI.Instance.balance.text = Datamanger.Intance.balance.ToString();
            }
            if (DataManager.Instance.playerData.membership == "free")
            {

                isRun1 = false;
                popUpObj.SetActive(true);
                popUpTitleObj.text = "Transaction Failed";
                popUpTitleObj.color = redColor;
                successObj.SetActive(false);
                failedObj.SetActive(true);
                popUpTxt.text = "Transaction Fail \n₹" + amount_order.ToString() + "\nfail to load";
            }
        }

    }

    public void PopUpOkyButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.DestroyShop();
        popUpObj.SetActive(false);
    }

    #endregion
    //public void quicaddbtns(int amount)
    //{
    //    amountfiled.text = amount.ToString();
    //}

    #region Class
    public class PaymentClass
    {
        public string orderId;
        public int orderAmount;
        public string orderCurrency;
    }
    public class cashtoken
    {
        public string success;
        public string data;
    }
    public class data
    {
        public string status;
        public string message;
        public string cftoken;
        public string orderId;
        public string appId;
        public string notifyUrl;
        public string source;
        public string orderCurrency;
        public string customerPhone;
        public string orderAmount;
    }

    #endregion
}

