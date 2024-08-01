using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingDialog : MonoBehaviour
{

    public static SettingDialog Instance;
    public Button soundBtn;
    public Button vibrationBtn;
    public Button musicBtn;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite vibrationonSprite;
    public Sprite vibrationoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        MainMenuManager.Instance.screenObj.Add(this.gameObject);

        soundBtn.image.sprite = DataManager.Instance.GetSound() == 0 ? soundonSprite : soundoffSprite;

        vibrationBtn.image.sprite = DataManager.Instance.GetVibration() == 0 ? vibrationonSprite : vibrationoffSprite;

        musicBtn.image.sprite = DataManager.Instance.GetMusic() == 0 ? musiconSprite : musicoffSprite;
    }

    public void CloseSetting()
    {
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void CloseSettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSetting();
    }



    public void SoundButtonClick()
    {
        if (soundBtn.image.sprite == soundonSprite)
        {
            DataManager.Instance.SetSound(1);
            soundBtn.image.sprite = soundoffSprite;
        }
        else if (soundBtn.image.sprite == soundoffSprite)
        {
            DataManager.Instance.SetSound(0);
            soundBtn.image.sprite = soundonSprite;
        }
    }
    

    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibrationBtn.image.sprite == vibrationonSprite)
        {
            DataManager.Instance.SetVibration(1);
            vibrationBtn.image.sprite = vibrationoffSprite;
        }
        else if (vibrationBtn.image.sprite == vibrationoffSprite)
        {
            DataManager.Instance.SetVibration(0);
            vibrationBtn.image.sprite = vibrationonSprite;
        }
    }
    
    public void MusicButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (musicBtn.image.sprite == musiconSprite)
        {
            DataManager.Instance.SetMusic(1);
            musicBtn.image.sprite = musicoffSprite;
            SoundManager.Instance.StartBackgroundMusic();
        }
        else if (musicBtn.image.sprite == musicoffSprite)
        {
            DataManager.Instance.SetMusic(0);
            musicBtn.image.sprite = musiconSprite;
            SoundManager.Instance.StartBackgroundMusic();
            SoundManager.Instance.ButtonClick();
        }
    }


    


    public void BottomButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();

        if (no == 1)
        {
            // Facebook
            //Signout call
            SoundManager.Instance.ButtonClick();
            //GoogleSignInManager.Instance.SignOutFromGoogle();
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Splash");
        }
        else if (no == 2)
        {
            // Rate Us
        }
        else if (no == 3)
        {
            // Support
        }
        else if (no == 4)
        {
            // Policy
        }
        else if (no == 5)
        {
            // Share
        }
    }


}
