using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GiftBox
{
    public string giftName;
    public int giftNo;
    public string giftId;
    public float price;
    public Sprite giftSprite;
}

public class GiftSendManager : MonoBehaviour
{

    public static GiftSendManager Instance;

    public List<GiftBox> giftBoxes = new List<GiftBox>();
    public GameObject giftBox;
    public GameObject giftBoxParent;

    public string gameName;

    public TeenPattiPlayer teenPattiOtherPlayer;
    public AndarBaharPlayer andarBaharOtherPlayer;
    public PokerPlayer pokerOtherPlayer;
    // Start is called before the first frame update
    
    public string ludoOtherPlayer;
    public string popMessage;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        if (gameName == "Ludo") return;
        for (int i = 0; i < giftBoxes.Count; i++)
        {
            GameObject giftGenObj = Instantiate(giftBox, giftBoxParent.transform);
            int no = i;
            giftGenObj.transform.GetChild(1).GetComponent<Text>().text = giftBoxes[no].price.ToString();
            giftGenObj.transform.GetChild(2).GetComponent<Image>().sprite = giftBoxes[no].giftSprite;

            giftGenObj.transform.GetComponent<Button>().onClick.AddListener(() => GiftBtnClick(no, giftBoxes[no].price));
        }
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void GiftBtnClick(int no, float price)
    {

        SoundManager.Instance.ButtonClick();
        if (gameName == "TeenPatti")
        {
            //TeenPattiManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);
            SendMessgaeSocket(no, teenPattiOtherPlayer.playerId);
            this.gameObject.SetActive(false);
            DataManager.Instance.DebitAmount(price.ToString(CultureInfo.InvariantCulture), TestSocketIO.Instace.roomid, "TeenPatti-Gif-" + TestSocketIO.Instace.roomid, "game", 0);

        }
        else if (gameName == "AndarBahar")
        {
            //AndarBaharManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);

            SendMessgaeSocket(no, andarBaharOtherPlayer.playerId);
            this.gameObject.SetActive(false);
            DataManager.Instance.DebitAmount(price.ToString(CultureInfo.InvariantCulture), TestSocketIO.Instace.roomid, "AndarBahar-Gif-" + TestSocketIO.Instace.roomid, "game", 0);

        }
        else if (gameName == "Poker")
        {
            //AndarBaharManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);

            SendMessgaeSocket(no, pokerOtherPlayer.playerId);
            this.gameObject.SetActive(false);
            DataManager.Instance.DebitAmount(price.ToString(CultureInfo.InvariantCulture), TestSocketIO.Instace.roomid, "Poker-Gif-" + TestSocketIO.Instace.roomid, "game", 0);

        }
        
    }
    
    public void GiftBtnClick(int no)
    {

        SoundManager.Instance.ButtonClick();
        if (gameName == "Ludo")
        {
            //TeenPattiManager.Instance.GetGift(DataManager.Instance.playerData._id, teenPattiOtherPlayer.playerId, no);
            SendMessgaeSocket(no, ludoOtherPlayer, 1);
            LudoUIManager.Instance.giftScreenObj.SetActive(false);
            //DataManager.Instance.DebitAmount(price.ToString(CultureInfo.InvariantCulture), TestSocketIO.Instace.roomid, "TeenPatti-Gif-" + TestSocketIO.Instace.roomid, "game", 0);
        }
    }
    
    public void PopMessageButtonClick(int no)
    {
        popMessage = no switch
        {
            1 => "Hi...",
            2 => "Hury up!",
            3 => "Hahahaha!",
            4 => "Well played!",
            5 => "Sorry!",
            6 => "Thank you!",
            _ => ""
        };
        SendMessgaeSocket(0,ludoOtherPlayer,2);
        LudoUIManager.Instance.giftScreenObj.SetActive(false);
    }
    
    private void SendMessgaeSocket(int giftNo, string otherPlayerId, int type)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("SendPlayerID", DataManager.Instance.playerData._id);
        obj.AddField("ReceivePlayerID", otherPlayerId);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("GiftNo", giftNo);
        obj.AddField("gameName", gameName);
        obj.AddField("Type", type);
        obj.AddField("Message", popMessage);
        TestSocketIO.Instace.Senddata("SendGiftMessage", obj);
    }



    public void SendMessgaeSocket(int giftNo, string otherPlayerId)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("SendPlayerID", DataManager.Instance.playerData._id);
        obj.AddField("ReceivePlayerID", otherPlayerId);
        obj.AddField("RoomId", TestSocketIO.Instace.roomid);
        obj.AddField("GiftNo", giftNo);
        obj.AddField("gameName", gameName);
        TestSocketIO.Instace.Senddata("SendGiftMessage", obj);
    }
    public void CloseButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
