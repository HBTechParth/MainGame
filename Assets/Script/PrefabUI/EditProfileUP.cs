using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;

public class EditProfileUP : MonoBehaviour
{
    // Start is called before the first frame update


    public InputField mobileInput;
    public Text nameTxt;
    //public Text emailTxt;
    public Text joinTxt;
    public Image ProfileImage;

    public Text msgTextRefer;

    public InputField refferalField;

    public GameObject add_Profile_1;
    public GameObject add_Profile_2;

    [Header("--- OTP Screen ---")]
    public GameObject otpScreenObj;
    public InputField otpField;
    public GameObject sendBtnObj;
    public Text sendBtnTxt;
    public Text timerTxt;
    public Text errorOTPTxt;
    public float timerValue;
    public float secondsCount;
    bool isOpenOTP;
    public string sendStr;
    public string reSendStr;

    [Header("--- phone, aadhar and pan ---")]

    public GameObject mobileObject;
    public GameObject kycObject;
    
    public InputField phoneNumberInput;
    public InputField otpNumber;
    public InputField nameInput;
    public InputField panNumberInput;
    public InputField dobInput;
    public Text errorText;
    public Text otpTimer;
    public GameObject tryAgain;
    public GameObject changeNumber;
    public string mobileNumberTxt;
    public GameObject getOtpButton;
    public GameObject verifyButton;
    public GameObject panSaveButton;
    public GameObject updatePanButton;

    private string phonePrefsKey = "PhoneNumber";
    private string aadharPrefsKey = "AadharNumber";
    private string panPrefsKey = "PanNumber";
    private string mobileVerifiedPrefsKey = "MobileVerified";
    private string kycVerifiedPrefsKey = "KYCVerified";

    public GameObject mobileVerified;
    public GameObject kycVerified;
    
    private bool isMobileVerified;
    private bool isKYCVerified;
    
    private float otpTimerDuration = 120f;  // 2 minutes
    private float currentOtpTimer = 0f;
    private bool isTimerRunning = false;
    
    [Header("--- Avatar ---")] 
    public Button defaultAvatarButton;
    public Button[] avatarButtons;
    public string avatarFolderPath = "Avatar"; // Path to the folder containing avatar sprites
    private Sprite[] avatars;
    private int selectedAvatarIndex;
    private int savedAvatarIndex;
    private bool isDefaultAvatar = false;
    public Image defaultImage;


    void Start()
    {
        MainMenuManager.Instance.screenObj.Add(this.gameObject);
        mobileInput.text = DataManager.Instance.playerData.phone;
        if (mobileInput.text != null && mobileInput.text.Length != 0)
        {
            add_Profile_2.SetActive(true);
            add_Profile_1.SetActive(false);
        }
        else
        {
            add_Profile_1.SetActive(true);
            add_Profile_2.SetActive(false);
        }

        StartCoroutine(LoadKYCData());


        nameTxt.text = "Name : " + DataManager.Instance.playerData.firstName;
        //emailTxt.text = "Email : " + DataManager.Instance.playerData.email;
        string curDateStr = DateTime.Parse(DataManager.Instance.playerData.createdAt).ToLocalTime().ToString();
        DateTime dateT1 = DateTime.Parse(curDateStr.Split(" ")[0]);
        DateTime dateT2 = DateTime.Parse(curDateStr.Split(" ")[1]);
        joinTxt.text = "Joined : " + dateT1.ToString("dd") + " " + dateT1.ToString("MMM") + " " + dateT1.ToString("yyyy") + "-" + dateT2.ToString("hh:mm tt");

        //StartCoroutine(GetImages(PlayerPrefs.GetString("ProfileURL"), ProfileImage));
        OpenProfileSection();
    }
    
    
    
    IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            if (image != null)
            {
                var texture = DownloadHandlerTexture.GetContent(request);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
                image.color = new Color(255, 255, 255, 255);
            }
        }
    }

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        MainMenuManager.Instance.LoadProfileImage();
        MainMenuManager.Instance.screenObj.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    #region Refferal Dialog


    public void RefferButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (refferalField.text == null)
        {
            msgTextRefer.text = "Please Enter Refer Code";

            Invoke(nameof(ReferMessageTxtNull), 3f);

        }
        else
        {
            RefferSendServer(refferalField.text);

        }
    }




    public void RefferSendServer(string refferValue)
    {
        StartCoroutine(SendRefferal(refferValue));
    }

    IEnumerator SendRefferal(string refferValue)
    {
        WWWForm form = new WWWForm();
        form.AddField("referId", refferValue.ToString());
        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/refer", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));

        yield return request.SendWebRequest();
        if (request.error == null)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            if (values["success"] == true)
            {
                //WaitPanelManager.Instance.ClosePanel();
                msgTextRefer.text = "Success";
                Invoke(nameof(ReferMessageTxtNull), 3f);
                //DataManager.Instance.playerData.balance = data["balance"].ToString().Trim('"');
                //Balance_Txt.text = Datamanger.Intance.balance.ToString().Trim('"');
            }
            else
            {
                //WaitPanelManager.Instance.ClosePanel();
                msgTextRefer.text = "Invalid Refer Code";
                Invoke(nameof(ReferMessageTxtNull), 3f);
            }
        }
        else
        {
            //WaitPanelManager.Instance.ClosePanel();
            msgTextRefer.text = "Invalid Refer Code";
            Invoke(nameof(ReferMessageTxtNull), 3f);
            //Reffer code is invalid.
        }
    }


    #endregion

    void ReferMessageTxtNull()
    {
        msgTextRefer.text = "";
    }

    #region Save Profile Button

    public void SaveButtonClick()
    {
        if (mobileInput.text.IsNullOrEmpty() || mobileInput.text.Length < 10)
        {
            msgTextRefer.text = "Please Enter Mobile No";
            Invoke(nameof(ReferMessageTxtNull), 3f);
        }
        else if (mobileInput.text.Length == 10)
        {
            //StartCoroutine(Profiledatasave());

            OpenOTPScreen(mobileInput.text);
        }
    }

    IEnumerator Profiledatasave()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", mobileInput.text);
        //WaitPanelManager.Instance.OpenPanel();

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            //WaitPanelManager.Instance.ClosePanel();

            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());



            JSONNode datas = JSON.Parse(values["data"].ToString());
            //Debug.Log("User Data===:::" + datas.ToString());

            if (datas != null)
            {
                MainMenuManager.Instance.Setplayerdata(datas, false);
                MainMenuManager.Instance.screenObj.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                msgTextRefer.text = (values["error"]);
                Invoke(nameof(ReferMessageTxtNull), 3f);
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }

    }


    #endregion

    #region OTP Maintain

    string mobileNo = "";
    public void OpenOTPScreen(string phone)
    {
        otpScreenObj.SetActive(true);
        sendBtnTxt.text = sendStr;
        sendBtnObj.SetActive(true);
        mobileNo = phone;

    }

    private void Update()
    {
        if (!isTimerRunning) return;
        currentOtpTimer -= Time.deltaTime;
        otpTimer.text = FormatTimer(currentOtpTimer);
        if (!(currentOtpTimer <= 0f)) return;
        isTimerRunning = false;
        tryAgain.SetActive(true);
    }
    
    string FormatTimer(float timer)
    {
        if (timer <= 0f)
        {
            return "--:--";
        }
        // Format the timer as minutes and seconds
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SendButtonClick(Text btnTxt)
    {
        SoundManager.Instance.ButtonClick();
        if (btnTxt.text.Equals(sendStr))
        {
            sendBtnObj.SetActive(false);
        }
        else
        {
            sendBtnObj.SetActive(false);
        }
        isOpenOTP = true;
        secondsCount = timerValue;
        StartCoroutine(sendOTP());

    }
    
    public void VerifyOtpButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (otpNumber != null && !string.IsNullOrEmpty(otpNumber.text))
        {
            StartCoroutine(VerifyOtp());
        }
        else
        {
            StartCoroutine(ShowError("Please enter a OTP number"));
        }
    }

   

    void OTPMessage()
    {
        errorOTPTxt.text = "";
    }


    IEnumerator sendOTP()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", mobileNumberTxt);

        print("Send OTP phoneNo : " + mobileNumberTxt);
        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/sendotp", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print("Send OTP : " + request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());
            
            if (values["success"] == false)
            {
                StartCoroutine(ShowError((values["error"])));
            }
            else
            {
                print("Otp received Successfully");
                StartOtpTimer();
                getOtpButton.gameObject.SetActive(false);
                verifyButton.gameObject.SetActive(true);
            }

            //Debug.Log("User Data===:::" + datas.ToString());
            //Setplayerdata(datas, false);
        }
        else
        {
            //errorOTPTxt.text = request.error.ToString();
            StartCoroutine(ShowError(request.error));
        }

    }
    
    IEnumerator VerifyOtp()
    {
        WWWForm form = new WWWForm();
        form.AddField("phone", mobileNumberTxt);
        form.AddField("code", otpNumber.text);

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/verify-phone", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
    
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());

            if (values["success"] == false)
            {
                StartCoroutine(ShowError((values["error"])));
            }
            else
            {
                StartCoroutine(ShowError("Phone Number Saved"));
                mobileNumberTxt = "";
                print("Otp Verified");
                NumberVerified();
                UpdatePhoneVerificationStatus();
                OpenMobileVerify();
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }
    }

    public void ChangeNumberButtonClick()
    {
        phoneNumberInput.interactable = true;
        phoneNumberInput.text = "";
        otpNumber.gameObject.SetActive(true);
        getOtpButton.gameObject.SetActive(true);
        changeNumber.gameObject.SetActive(false);
    }
    
    public void NumberVerified()
    {
        PlayerPrefs.SetInt(DataManager.Instance.OtpVerifiedKey, 1);
        PlayerPrefs.Save();
    }
    
    void UpdatePhoneVerificationStatus()
    {
        
        bool isPhoneVerified = DataManager.Instance.IsOtpVerified();
        bool isPanVerified = DataManager.Instance.IsPanSaved();
        
        if (isPhoneVerified)
        {
            phoneNumberInput.text = DataManager.Instance.playerData.phone;
            phoneNumberInput.interactable = false;
            mobileVerified.gameObject.SetActive(true);
            otpNumber.gameObject.SetActive(false);
            verifyButton.gameObject.SetActive(false);
            getOtpButton.gameObject.SetActive(false);
            tryAgain.gameObject.SetActive(false);
            changeNumber.gameObject.SetActive(true);
        }
        else
        {
            mobileVerified.gameObject.SetActive(false);
            getOtpButton.gameObject.SetActive(true);
            verifyButton.gameObject.SetActive(false);
            tryAgain.gameObject.SetActive(false);
        }

        if (isPanVerified)
        {
            kycVerified.gameObject.SetActive(true);
            panSaveButton.gameObject.SetActive(false);
            updatePanButton.gameObject.SetActive(true);
        }
        else
        {
            kycVerified.gameObject.SetActive(false);
            panSaveButton.gameObject.SetActive(true);
            updatePanButton.gameObject.SetActive(false);
        }
    }

    
    void StartOtpTimer()
    {
        otpTimer.gameObject.SetActive(true);
        currentOtpTimer = otpTimerDuration;
        isTimerRunning = true;
    }
    
    void StopAndResetOtpTimer()
    {
        if (!isTimerRunning) return;
        isTimerRunning = false;
        otpTimer.gameObject.SetActive(false);
        tryAgain.SetActive(false);
        currentOtpTimer = otpTimerDuration;
    }


    IEnumerator PhoneVerify(string phoneNo, string code)
    {

        //WaitPanelManager.Instance.OpenPanel();
        WWWForm form = new WWWForm();
        form.AddField("phone", phoneNo);
        form.AddField("code", code);


        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/auth/player/verify", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        //Datamanger.Intance.Avtar = Avtarint;
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print("Verify Message : " + values.ToString());
            //WaitPanelManager.Instance.ClosePanel();

            if (values["success"] == true)
            {
                otpScreenObj.SetActive(false);

                add_Profile_1.SetActive(false);
                add_Profile_2.SetActive(true);

            }
            else
            {
                errorOTPTxt.text = values["error"];
                Invoke(nameof(OTPMessage), 4f);
            }

            //Debug.Log("User Data===:::" + datas.ToString());
            //Setplayerdata(datas, false);
        }
        else
        {
            //WaitPanelManager.Instance.ClosePanel();
            errorOTPTxt.text = request.error.ToString();
            Invoke(nameof(OTPMessage), 4f);
        }

    }

    public void OpenMobileVerify()
    {
        SoundManager.Instance.ButtonClick();
        mobileObject.gameObject.SetActive(!mobileObject.activeSelf);
        StopAndResetOtpTimer();
    }
    
    public void GetOtpButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (phoneNumberInput != null && IsMobileNumberValid(phoneNumberInput.text))
        {
            mobileNumberTxt = phoneNumberInput.text;
            StartCoroutine(sendOTP());
            getOtpButton.gameObject.SetActive(false);
            verifyButton.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowError("Please enter a valid 10-digit mobile number"));
        }
    }
    
    bool IsMobileNumberValid(string number)
    {
        return !string.IsNullOrEmpty(number) && number.Length == 10 && number.All(char.IsDigit);
    }
    
    public void ResendOtpButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        StartOtpTimer();
        tryAgain.SetActive(false);
        GetOtpButtonClick();
    }
    
    #endregion

    #region PAN Verification

    public void OpenKYCVerify()
    {
        SoundManager.Instance.ButtonClick();
        kycObject.gameObject.SetActive(!kycObject.activeSelf);
    }
    
    public void PanVerifySubmit()
    {
        SoundManager.Instance.ButtonClick();
        if (string.IsNullOrEmpty(nameInput.text))
        {
            StartCoroutine(ShowError("Please enter your name"));
            return;
        }
        if (!IsValidPanFormat(panNumberInput.text))
        {
            StartCoroutine(ShowError("Please enter a valid PAN number"));
            return;
        }
        if (!IsValidDateFormat(dobInput.text))
        {
            StartCoroutine(ShowError("Please enter a valid date of birth in DD/MM/YYYY format"));
            return;
        }
        StartCoroutine(PanDataSave());
    }
    
    bool IsValidPanFormat(string panNumber)
    {
        string panPattern = @"^[A-Za-z]{5}\d{4}[A-Za-z]{1}$";
        return Regex.IsMatch(panNumber, panPattern);
    }
    
    bool IsValidDateFormat(string date)
    {
        return Regex.IsMatch(date, @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$");
    }
    
    IEnumerator PanDataSave()
    {
        yield return SaveProfileData(nameInput.text, panNumberInput.text, dobInput.text);
    }
    
    IEnumerator SaveProfileData(string name, string number, string dob)
    {
        WWWForm form = new WWWForm();
        form.AddField("firstName", name);
        form.AddField("panNumber", number);
        form.AddField("dob", dob);

        UnityWebRequest request = UnityWebRequest.Post(DataManager.Instance.url + "/api/v1/players/profile", form);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
    
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text.ToString());
            print(request.downloadHandler.text);
            Logger.log.Log("Save Data", values.ToString());

            if (values["success"] == false)
            {
                StartCoroutine(ShowError((values["error"])));
            }
            else
            {
                StartCoroutine(ShowError("Success"));
                SavePan();
                print("Data Saving Successful");
                kycVerified.SetActive(true);
                OpenKYCVerify();
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }
    }
    
    public void SavePan()
    {
        PlayerPrefs.SetInt(DataManager.Instance.PanSavedKey, 1);
        PlayerPrefs.Save();
    }
    
    IEnumerator LoadKYCData()
    {
        UnityWebRequest request = UnityWebRequest.Get(DataManager.Instance.url + "/api/v1/players/profile");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.error == null && !request.isNetworkError)
        {
            JSONNode values = JSON.Parse(request.downloadHandler.text);
            print(request.downloadHandler.text);
            Logger.log.Log("Load KYC Data", values.ToString());

            if (values["success"] == true)
            {
                phoneNumberInput.text = values["data"]["phone"];
                nameInput.text = values["data"]["firstName"];
                panNumberInput.text = values["data"]["panNumber"];
                dobInput.text = values["data"]["dob"];
                
                if (!string.IsNullOrEmpty(phoneNumberInput.text))
                {
                    NumberVerified();
                    print("Number Has Verified!");
                }
                
                if (!string.IsNullOrEmpty(nameInput.text) && !string.IsNullOrEmpty(dobInput.text) && !string.IsNullOrEmpty(panNumberInput.text))
                {
                    SavePan();
                    print("Pan Has Verified!");
                }
                UpdatePhoneVerificationStatus();
            }
            else
            {
                StartCoroutine(ShowError((values["error"])));
            }
        }
        else
        {
            Logger.log.Log(request.error.ToString());
        }
    }


    public void UpdatePanButtonClick()
    {
        nameInput.text = "";
        panNumberInput.text = "";
        dobInput.text = "";
        updatePanButton.gameObject.SetActive(false);
        panSaveButton.gameObject.SetActive(true);
    }



    #endregion
    
    private IEnumerator ShowError(string errorMessage)
    {
        errorText.text = errorMessage;
        errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorText.gameObject.SetActive(false);
    }

    #region Profile Selection

    public void OpenProfileSection()
    {
        LoadDefaultProfileImage();
        LoadProfileImage();
    }

    private void LoadDefaultProfileImage()
    {
        StartCoroutine(GetImages(PlayerPrefs.GetString("ProfileURL"), defaultImage));
    }

    private void LoadProfileImage()
    {
        LoadAvatars();
        savedAvatarIndex = PlayerPrefs.GetInt("SelectedAvatarIndex", -1);

        if (savedAvatarIndex != -1)
        {
            selectedAvatarIndex = savedAvatarIndex;
        }
        else
        {
            selectedAvatarIndex = -1;
        }

        UpdateProfileImage();

        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int index = i;
            avatarButtons[i].onClick.AddListener(() => OnAvatarButtonClick(index));
        }

        defaultAvatarButton.onClick.AddListener(OnDefaultAvatarButtonClick);

        if (selectedAvatarIndex == -1)
        {
            StartCoroutine(GetImages(PlayerPrefs.GetString("ProfileURL"), ProfileImage));
        }
    }
    
    private void LoadAvatars()
    {
        avatars = Resources.LoadAll<Sprite>(avatarFolderPath);
    }
    
    private void OnAvatarButtonClick(int index)
    {
        selectedAvatarIndex = index;
        UpdateProfileImage();
    }

    private void OnDefaultAvatarButtonClick()
    {
        selectedAvatarIndex = -1;
        UpdateProfileImage();
    }

    public void OnSaveButtonClick()
    {
        savedAvatarIndex = selectedAvatarIndex;
        PlayerPrefs.SetInt("SelectedAvatarIndex", savedAvatarIndex);
        PlayerPrefs.Save();
        
        
        Sprite selectedSprite;
        if (selectedAvatarIndex >= 0 && selectedAvatarIndex < avatars.Length)
        {
            selectedSprite = avatars[selectedAvatarIndex];
        }
        else
        {
            selectedSprite = null; // Use a default sprite if desired
        }

        DataManager.Instance.SetSelectedAvatarSprite(selectedSprite);
        

        // Add additional save logic if needed

        // Close the avatar selection interface or perform other actions
    }

    private void OnCancelButtonClick()
    {
        selectedAvatarIndex = savedAvatarIndex;
        UpdateProfileImage();

        // Close the avatar selection interface or perform other actions
    }

    private void UpdateProfileImage()
    {
        Sprite sprite;
        if (selectedAvatarIndex >= 0 && selectedAvatarIndex < avatars.Length)
        {
            sprite = avatars[selectedAvatarIndex];
        }
        else
        {
            sprite = null; // Use a default sprite if desired
        }

        ProfileImage.sprite = sprite;

        if (selectedAvatarIndex == -1)
        {
            StartCoroutine(GetImages(PlayerPrefs.GetString("ProfileURL"), ProfileImage));
        }
        OnSaveButtonClick();
    }


    #endregion
    
}
