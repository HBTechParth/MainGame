using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferralPanel : MonoBehaviour
{
    public Image ReferalBtn;
    public Image EarnBtn;
    public Image rulesBtn;
    public Sprite OnImage;
    public Sprite OffImage;
    public GameObject ReferalPannel;
    public GameObject EarnPannel;
    public GameObject rulesPannel;
    public static ReferralPanel Instance;
    public Text refferalTxt;
    private bool _referBtnClk;
    private bool _earnBtnClk;
    public Text referTotalRewardEarnTxt;
    public Text referTotalPeopleTxt;
    public Image avatarImg;
    public Text playerNameTxt;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        ReferButtonClick();
    }

    private void OnEnable()
    {
        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Add(this.gameObject);
        }
        refferalTxt.text = DataManager.Instance.playerData.refer_code;
        //refferalTxt.text = "Your Referral Code : " + DataManager.Instance.playerData.refer_code;
    }

    public void ReferButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        ReferalBtn.sprite = OnImage;
        EarnBtn.sprite = OffImage;
        rulesBtn.sprite = OffImage;
        ReferalPannel.SetActive(true);
        EarnPannel.SetActive(false);
        rulesPannel.SetActive(false);
        UpdateProfile();
    }
    
    public void EarnButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        EarnBtn.sprite = OnImage;
        ReferalBtn.sprite = OffImage;
        rulesBtn.sprite = OffImage;
        EarnPannel.SetActive(true);
        ReferalPannel.SetActive(false);
        rulesPannel.SetActive(false);
    }
    
    public void RulesButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        rulesBtn.sprite = OnImage;
        EarnBtn.sprite = OffImage;
        ReferalBtn.sprite = OffImage;
        rulesPannel.SetActive(true);
        ReferalPannel.SetActive(false);
        EarnPannel.SetActive(false);
    }

    
    public void CopyButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        TextEditor textEditor = new TextEditor();
        textEditor.text = refferalTxt.text;
        textEditor.SelectAll();
        textEditor.Copy();
        print(textEditor.text);
    }



    public void CloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        if (MainMenuManager.Instance != null)
        {
            MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        }

        Destroy(this.gameObject);
    }


    public void ShareButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        string shareTxt = "Download Latest TeenpattiBlack apk from Link Here : \n\n" + DataManager.Instance.appUrl + " \n\nUse this referral code :" + DataManager.Instance.playerData.refer_code;
        
        print(shareTxt);

        new NativeShare().Share(shareTxt);
    }

    public void UpdateProfile()
    {
        referTotalRewardEarnTxt.text = DataManager.Instance.playerData.refrer_amount_total;
        referTotalPeopleTxt.text = DataManager.Instance.playerData.refer_count;
        playerNameTxt.text = DataManager.Instance.playerData.firstName;
        StartCoroutine(DataManager.Instance.GetImages(PlayerPrefs.GetString("ProfileURL"), avatarImg));
    }

}
