using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public InputField field;
    public float amount;
    public Text transactionAmount;
    public GameObject tryAgainText;
    public GameObject transactionObject;
    public GameObject paymentMode;
    public bool isPhonePay;
    // Start is called before the first frame update
    void Start()
    {
        transactionObject.gameObject.SetActive(false);
        paymentMode.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PaymentCheck()
    {
        CashFreeManage.Instance.OpenPaymentURL("https://zgw.oynxdigital.com/payment1.php?i=bkGN82", "10", "Tester@gmail.com");
    }

    public void AddCashButton(int no)
    {
        SoundManager.Instance.ButtonClick();
        print(no);
        StartCoroutine(CashFreeManage.Instance.getToken((int)(no), CashFreeManage.Instance.couponId, false));
    }

    public void ContinueButton()
    {
        print("Amount = " + amount);
        SoundManager.Instance.ButtonClick();
        /*if(amount >= 50)
            StartCoroutine(CashFreeManage.Instance.getToken((int)(amount), CashFreeManage.Instance.couponId));*/
        paymentMode.gameObject.SetActive(true);

    }

    public void ClosePaymentMode()
    {
        SoundManager.Instance.ButtonClick();
        paymentMode.gameObject.SetActive(false);
    }
    
    public void SelectPaymentMode(bool phonepay)
    {
        SoundManager.Instance.ButtonClick();
        isPhonePay = phonepay;
        if(amount >= 50)
            StartCoroutine(CashFreeManage.Instance.getToken((int)(amount), CashFreeManage.Instance.couponId, isPhonePay));
        paymentMode.gameObject.SetActive(false);
    }

    public void SelectAmount(float amt)
    {
        SoundManager.Instance.ButtonClick();
        amount = amt;
        transactionAmount.text = "₹" + amount.ToString();
        GetCoupon(amount);
    }
    public void InputAmountButton()
    {
        if (field.text != "")
        {
            amount = float.Parse(field.text);
            transactionAmount.text = "₹" +  amount.ToString();
            transactionObject.SetActive(true);
            GetCoupon(amount);
        }
        else
            StartCoroutine(TryAgain());
    }
    
    private void GetCoupon(float amount)
    {
        CashFreeManage.Instance.couponId = "";
        foreach (var couponData in MainMenuManager.Instance.couponDatas)
        {
            if (couponData.minAmount != amount) continue;
            CashFreeManage.Instance.couponId = couponData.id;
            break;
        }
    }

    public IEnumerator TryAgain()
    {
        tryAgainText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        tryAgainText.SetActive(false);
    }

    public void SoundClick()
    {
        SoundManager.Instance.ButtonClick();
    }

    public void ShopCloseButton()
    {
        SoundManager.Instance.ButtonClick();
        //Destroy(this.gameObject);
        MainMenuManager.Instance.DestroyShop();
    }
}
