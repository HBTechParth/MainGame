using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        if (amount >= 100)
        {
            SoundManager.Instance.ButtonClick();
            /*if(amount >= 50)
                StartCoroutine(CashFreeManage.Instance.getToken((int)(amount), CashFreeManage.Instance.couponId));*/
            paymentMode.gameObject.SetActive(true);
        }
        else
        {
            StartCoroutine(TryAgain());
        }

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
        if (amount >= 100)
            StartCoroutine(CashFreeManage.Instance.getToken((int)(amount), CashFreeManage.Instance.couponId, isPhonePay));
        paymentMode.gameObject.SetActive(false);
    }

    public TextMeshProUGUI amountText;
    public TextMeshProUGUI bonusAmountText;
    public TextMeshProUGUI totalAmountText;

    public void FieldNull(bool isnull)
    {
        if (isnull)
            field.text = "";
    }
    public void OnAmountInputEnd()
    {
        SoundManager.Instance.ButtonClick();
        int amt;
        if (!string.IsNullOrEmpty(field.text))
        {
            amt = int.Parse(field.text);
            Debug.Log("INPUT Amount " + amt);
            if (amt >= 100)
            {
                SelectAmount(amt);
            }
            else
            {
                field.text = "";
                SelectAmount(0);
            }
        }




    }
    float bonusAmount = 0f;
    float totalAmount = 0f;
    public void SelectAmount(float amt)
    {
        SoundManager.Instance.ButtonClick();


        amount = amt;

        // Update transaction and amount texts
        transactionAmount.text = "₹" + amount.ToString();
        amountText.text = "₹" + amount.ToString();



        // Determine bonus and total amount based on the selected amount
        if (amount >= 100 && amount <= 5000)
        {
            // Calculate 100% extra bonus
            bonusAmount = amount * 1.0f; // 100% of the amount
            totalAmount = amount + bonusAmount; // Total is original amount + bonus
            bonusAmountText.text = bonusAmount.ToString() + " extra (100%)";
        }
        else if (amount > 5000)
        {
            // Calculate 140% extra bonus
            bonusAmount = amount * 1.4f; // 140% of the amount
            totalAmount = amount + bonusAmount; // Total is original amount + bonus
            bonusAmountText.text = bonusAmount.ToString() + " extra (140%)";
        }
        else
        {
            // No bonus for amounts less than 100
            bonusAmountText.text = "₹ 0";
            totalAmount = amount; // If no bonus, total is just the original amount
        }

        // Display the total amount in the totalAmountText
        totalAmountText.text = totalAmount.ToString();

        // Call GetCoupon with the selected amount
        GetCoupon(amount);
    }

    public void InputAmountButton()
    {
        if (field.text != "")
        {
            amount = float.Parse(field.text);
            transactionAmount.text = "₹" + amount.ToString();
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
