using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;

public class LudoUIManager : MonoBehaviour
{
    public static LudoUIManager Instance;
    public GameObject listObject;
    [Header("--- Sounds ---")]
    public Image soundImg;
    public Image vibrationImg;
    public Image musicImg;
    public Sprite soundonSprite;
    public Sprite soundoffSprite;
    public Sprite vibrationonSprite;
    public Sprite vibrationoffSprite;
    public Sprite musiconSprite;
    public Sprite musicoffSprite;

    [Header("---Leave Game Screen---")]
    public GameObject leaveGameScreenObj;
    public GameObject disconnectGameScreenObj;

    [Header("--- Rule Game Screen---")]
    public GameObject ruleScreenObj;
    public GameObject[] ruleSubScreenObj;
    public Button ruleLeftBtn;
    public Button ruleRightBtn;
    public int ruleScreenNo;

    [Header("--- Turn Screen---")]
    public GameObject turnSkipScreenObj;
    public Text turnSkipTitleTxt;
    public Text turnSkipSubtitleTxt;

    [Header("---Others--- ")]
    public GameObject potObj;
    public Text potTxt;
    public GameObject countObject;
    public Text countTxt;

    [Header("---Tournament Info---")] 
    public GameObject informationScene;
    public Text gameType;
    public Text entryAmount;
    public Text winAmount;
    public Text tournamentId;

    [Header("--- Chatting ---")] 
    public GameObject[] playersPlace;
    public GameObject chatBoxCanvas;
    public GameObject giftScreenObj;
    public GameObject giftParentObj;
    public GameObject giftPrefab;
    public List<GiftBox> giftBoxes = new List<GiftBox>();
    //public List<Sprite> giftBoxes = new List<Sprite>();
    public GameObject chatPanelParent;
    public GameObject chatMePrefab;
    public GameObject chatOtherPrefab;
    public GameObject[] popTxtMessage;
    

    public GameObject timeObj;
    public GameObject dicelessNumberObj;
    public GameObject bottomOneObj;
    public GameObject bottomOneLineParent;
    public GameObject bottomThreeObj;
    public GameObject bottomThreeLineParent;

    public int moveCnt = 0;
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

        //if (DataManager.Instance.playerData.firstName == "" || DataManager.Instance.playerData.firstName == null)
        //{
        //    player1Txt.text = UserNameStringManage(DataManager.Instance.GetDefaultPlayerName().ToString().Trim('"'));
        //}
        //else
        //{
        //    player1Txt.text = UserNameStringManage(DataManager.Instance.playerData.firstName.ToString().Trim('"'));
        //}

        if (DataManager.Instance.modeType == 3)
        {
            countTxt.text = 24.ToString();
            timeObj.SetActive(false);
            bottomOneObj.SetActive(true);
            bottomThreeObj.SetActive(false);
        }
        else
        {
            potObj.SetActive(true);
            countObject.SetActive(false);
            potTxt.text = DataManager.Instance.winAmount.ToString();//Disply the winamount
            bottomOneObj.SetActive(false);
            bottomThreeObj.SetActive(false);
        }

        if (DataManager.Instance.modeType == 4) return;
        for (int i = 0; i < LudoManager.Instance.mainDicelist.Count; i++)
        {
            GameObject genObj = Instantiate(dicelessNumberObj, bottomOneLineParent.transform);
            genObj.transform.GetChild(0).GetComponent<Text>().text = LudoManager.Instance.mainDicelist[i].ToString();
        }
        if (DataManager.Instance.modeType == 3 && LudoManager.Instance.isAdmin)
        {
            //LudoManager.Instance.DiceLessPasaButton();
            Invoke(nameof(CallDiceLessPasaButton), 0.2f);
        }
    }

    private void CallDiceLessPasaButton()
    {
        LudoManager.Instance.DiceLessPasaButton();
    }



    public void FirstNumberRemove()
    {
        if (bottomOneLineParent.transform.childCount >=0)
        {
            moveCnt++;
            countTxt.text = (24 - moveCnt).ToString();
            //Destroy(bottomOneLineParent.transform.GetChild(0).gameObject);
        }
    }
    public void FirstNumberYellow()
    {
        if (bottomOneLineParent.transform.childCount > 0)
        {
            bottomOneLineParent.transform.GetChild(0).GetComponent<Image>().color = LudoManager.Instance.yellowColor;
        }
    }

    public void View_One_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        bottomThreeObj.SetActive(true);
        for (int i = 0; i < LudoManager.Instance.mainDicelist.Count; i++)
        {
            GameObject genObj = Instantiate(dicelessNumberObj, bottomThreeLineParent.transform);
            genObj.transform.GetChild(0).GetComponent<Text>().text = LudoManager.Instance.mainDicelist[i].ToString();
            if (i < moveCnt)
            {
                genObj.transform.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void UpdateBottomDropDown()
    {
        if (bottomThreeObj.activeInHierarchy)
        {
            for (int i = 0; i < LudoManager.Instance.mainDicelist.Count; i++)
            {
                //GameObject genObj = Instantiate(dicelessNumberObj, bottomThreeLineParent.transform);
                //genObj.transform.GetChild(0).GetComponent<Text>().text = LudoManager.Instance.mainDicelist[i].ToString();
                bottomThreeLineParent.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = LudoManager.Instance.mainDicelist[i].ToString();
                if (i < moveCnt)
                {
                    //genObj.transform.GetComponent<Button>().interactable = false;
                    bottomThreeLineParent.transform.GetChild(i).transform.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void View_Two_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        bottomThreeObj.SetActive(false);
        for (int i = 0; i < bottomThreeLineParent.transform.childCount; i++)
        {
            Destroy(bottomThreeLineParent.transform.GetChild(i).gameObject);
        }

    }





    // Update is called once per frame
    void Update()
    {

    }

    #region Play Screen

    public void HomeButtonClick()
    {
        //SoundManager.Instance.ButtonClick();
        OpenLeaveScreen();
    }

    public void SettingButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        
    }

    public void ListButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenRuleScreen();
    }

    public void ScoreButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenTurnSkip();

    }

    #endregion

    #region Setting Screen

    void OpenSettingScreen()
    {
        soundImg.sprite = DataManager.Instance.GetSound() == 0 ? soundonSprite : soundoffSprite;
        vibrationImg.sprite = DataManager.Instance.GetVibration() == 0 ? vibrationonSprite : vibrationoffSprite;
        musicImg.sprite = DataManager.Instance.GetMusic() == 0 ? musiconSprite : musicoffSprite;
    }
    
    public void SoundButtonClick()
    {
        if (soundImg.sprite == soundonSprite)
        {
            DataManager.Instance.SetSound(1);
            soundImg.sprite = soundoffSprite;
        }
        else if (soundImg.sprite == soundoffSprite)
        {
            DataManager.Instance.SetSound(0);
            soundImg.sprite = soundonSprite;
        }
    }
    

    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibrationImg.sprite == vibrationonSprite)
        {
            DataManager.Instance.SetVibration(1);
            vibrationImg.sprite = vibrationoffSprite;
        }
        else if (vibrationImg.sprite == vibrationoffSprite)
        {
            DataManager.Instance.SetVibration(0);
            vibrationImg.sprite = vibrationonSprite;
        }
    }
    
    public void MusicButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (musicImg.sprite == musiconSprite)
        {
            DataManager.Instance.SetMusic(1);
            musicImg.sprite = musicoffSprite;
            SoundManager.Instance.StartBackgroundMusic();
        }
        else if (musicImg.sprite == musicoffSprite)
        {
            DataManager.Instance.SetMusic(0);
            musicImg.sprite = musiconSprite;
            SoundManager.Instance.StartBackgroundMusic();
            SoundManager.Instance.ButtonClick();
        }
    }


    public void Setting_LeaveMatch_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        //Send Event
        TestSocketIO.Instace.LeaveRoom();
        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.isConnectBot = false;
        //SoundManager.Instance.StartBackgroundMusic();
        DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
        SceneManager.LoadScene("Main");
        //print("Leave Match Button Click");
    }

    public void LeftButtonClick()//when internet is unstable
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenList_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        listObject.gameObject.SetActive(true);
        OpenSettingScreen();
    }
    
    public void CloseList_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        listObject.gameObject.SetActive(false);
    }

    #endregion

    #region Leave Game

    public void OpenLeaveScreen()
    {
        //leaveGameScreenObj.SetActive(true);
        OpenPanelAnimation(leaveGameScreenObj);
    }

    public void Leave_LeaveGame_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        //Send Event
        TestSocketIO.Instace.LeaveRoom();

        BotManager.Instance.isBotAvalible = false;
        BotManager.Instance.isConnectBot = false;
        SoundManager.Instance.StartBackgroundMusic();
        DataManager.Instance.hasCalledOpenTournamentLoadScreen = false;
        SceneManager.LoadScene("Main");
        print("Leave a Game");
    }

    public void Leave_KeepPlaying_ButtonClick()
    {
        //SoundManager.Instance.ButtonClick();

        //leaveGameScreenObj.SetActive(false);
        ClosePanelAnimation(leaveGameScreenObj);
    }


    #endregion

    #region Animation

    private void OpenPanelAnimation(GameObject panel)
    {
        SoundManager.Instance.ButtonClick();
        panel.gameObject.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        Image background = panel.GetComponent<Image>();
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
        sequence.Join(background.DOFade(165 / 255f, 0.5f));
    }

    private void ClosePanelAnimation(GameObject panel)
    {
        SoundManager.Instance.ButtonClick();
        Image background = panel.gameObject.GetComponent<Image>();
    
        float duration = 0.3f;
        Ease easeType = Ease.InBack;
    
        Sequence sequence = DOTween.Sequence();
        
        sequence.Join(panel.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.Join(background.DOFade(0f, duration).SetEase(easeType));
        
        sequence.OnComplete(() => panel.gameObject.SetActive(false));
    }


    #endregion

    #region Rule Screen

    void OpenRuleScreen()
    {
        ruleScreenObj.SetActive(true);
        ruleScreenNo = 0;
        RuleSubScreenSet();
    }

    void RuleSubScreenSet()
    {
        for (int i = 0; i < ruleSubScreenObj.Length; i++)
        {
            if (i == ruleScreenNo)
            {
                ruleSubScreenObj[i].SetActive(true);
            }
            else
            {
                ruleSubScreenObj[i].SetActive(false);
            }
        }

        if (ruleScreenNo == 0)
        {
            ruleLeftBtn.interactable = false;
            ruleRightBtn.interactable = true;
        }
        else if (ruleScreenNo == ruleSubScreenObj.Length - 1)
        {
            ruleRightBtn.interactable = false;
            ruleLeftBtn.interactable = true;
        }
        else
        {
            ruleLeftBtn.interactable = true;
            ruleRightBtn.interactable = true;
        }
    }

    public void Rule_Left_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        ruleScreenNo--;
        RuleSubScreenSet();
    }
    public void Rule_Right_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        ruleScreenNo++;
        RuleSubScreenSet();
    }

    public void Rule_Close_ButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        ruleScreenObj.SetActive(false);
    }

    #endregion

    #region Turn Skip

    void OpenTurnSkip()
    {
        turnSkipScreenObj.SetActive(true);
        turnSkipTitleTxt.text = "Turn Skipped";
        turnSkipSubtitleTxt.text = "Third 6 in row, can't be played.\nNext players turn.";
        Invoke(nameof(CloseTurnSkip), 1.75f);
    }

    void CloseTurnSkip()
    {
        SoundManager.Instance.ButtonClick();

        turnSkipScreenObj.SetActive(false);
    }
    #endregion

    #region Inforamtion

    public void InformationButtonClick()
    {
        informationScene.SetActive(!informationScene.activeSelf);
        UpdateInformation();
    }

    private void UpdateInformation()
    {
        /*gameType.text =
            (DataManager.Instance.logoType == 0) ? "Classic" :
            (DataManager.Instance.logoType == 1) ? "Boost" :
            (DataManager.Instance.logoType == 2) ? "Tournament" :
            (DataManager.Instance.logoType == 3) ? "Timer" :
            "";*/
        entryAmount.text ="Entry: " + DataManager.Instance.tourEntryMoney;
        winAmount.text ="Winner prize - " + DataManager.Instance.winAmount;
        tournamentId.text = DataManager.Instance.tournamentID;
    }
    
    #endregion

    #region Chatting

    public void ChatBoxButtonClick()
    {
        chatBoxCanvas.gameObject.SetActive(!chatBoxCanvas.activeSelf);
    }
    
    public void GiftButtonClick(Text otherPlayerId)
    {
        SoundManager.Instance.ButtonClick();
        giftScreenObj.SetActive(true);
        GiftSendManager.Instance.gameName = "Ludo";
        GiftSendManager.Instance.ludoOtherPlayer = otherPlayerId.text;
    }

    public void GetChat(string playerID, string msg)
    {
        if (playerID.Equals(DataManager.Instance.playerData._id))
        {
            TypeMessageBox typeMessageBox =
                Instantiate(chatMePrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
            typeMessageBox.Update_Message_Box(msg);
        }
        else
        {
            TypeMessageBox typeMessageBox =
                Instantiate(chatOtherPrefab, chatPanelParent.transform).GetComponent<TypeMessageBox>();
            typeMessageBox.Update_Message_Box(msg);
        }

        Canvas.ForceUpdateCanvases();
    }

    public void GetGift(string sendPlayerID, string receivePlayerId, int giftNo, int type, string message)//type = 1 for emoji, type = 2 for text.
    {
        if (type == 1)
        {
            GameObject sendPlayerObj = null;

            GameObject giftGen = Instantiate(giftPrefab, giftParentObj.transform);

            for (var i = 0; i < giftBoxes.Count; i++)
            {
                if (giftBoxes[i].giftNo == giftNo)
                {
                    giftGen.transform.GetComponent<Image>().sprite = giftBoxes[i].giftSprite;
                }
            }

            // Find sender and receiver player objects
            GameObject senderPlayerObj = FindPlayerObject(sendPlayerID);
            GameObject receiverPlayerObj = FindPlayerObject(receivePlayerId);

            if (senderPlayerObj == null || receiverPlayerObj == null) return;
            // Set the initial position of the gift
            giftGen.transform.position = senderPlayerObj.transform.position;

            // Animate the gift from sender to receiver
            giftGen.transform.DOMove(receiverPlayerObj.transform.position, 0.4f)
                .OnComplete(() =>
                {
                    giftGen.transform.DOMove(receiverPlayerObj.transform.position, 1f)
                        .OnComplete(() =>
                        {
                            giftGen.transform.DOScale(Vector3.zero, 0.5f)
                                .OnComplete(() => { Destroy(giftGen); });
                        });
                });
        }
        else if (type == 2)
        {
            for (int i = 0; i < playersPlace.Length; i++)
            {
                if (playersPlace[i].GetComponent<Text>().text == receivePlayerId)
                {
                    // Set the text for the receiving player
                    popTxtMessage[i].transform.GetChild(0).GetComponent<Text>().text = message;
                    // Set this pop-up message active
                    popTxtMessage[i].SetActive(true);

                    // Use a coroutine to deactivate the pop-up message after a delay
                    StartCoroutine(DeactivatePopUp(popTxtMessage[i], 1.5f));
                }
                else
                {
                    // Set other pop-up messages inactive
                    popTxtMessage[i].SetActive(false);
                }
            }
        }
    }
    
    // Function to find the player object based on player ID
    private GameObject FindPlayerObject(string playerID)
    {
        for (int i = 0; i < playersPlace.Length; i++)
        {
            string currentPlayerID = playersPlace[i].GetComponent<Text>().text;
            if (currentPlayerID == playerID)
            {
                return playersPlace[i].gameObject;
            }
        }

        return null; // Return null if player object is not found
    }
    
    private IEnumerator DeactivatePopUp(GameObject popUp, float delay)
    {
        yield return new WaitForSeconds(delay);
        popUp.SetActive(false);
    }

    #endregion
}
