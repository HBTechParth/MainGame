using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinDialogPanel : MonoBehaviour
{
    public Text middleTxt;
    public int earnAmount;
    // Start is called before the first frame update
    void Start()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        //middleTxt.text = "";
        //titleTxt.text = "";
    }

    public void DisplayText()
    {
        middleTxt.text = "You are Lucky you won "+ earnAmount+" Coin add your bonus.";
    }


    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        //this.gameObject.SetActive(false);
        //MainMenuManager.Instance.UpdateUICallAPI();
        //MainMenuManager.Instance.OpenDailyReward();
        MainMenuManager.Instance.CloseSpinnerWheel();
        Destroy(this.gameObject);
    }
}
