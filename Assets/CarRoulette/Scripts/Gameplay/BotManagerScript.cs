using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BotManagerScript : MonoBehaviour
{
    public Image[] botPlayerImage;
    public Text[] botPlayerNames;
    public int minPlayerRequired = 3;
    public int botPlayers;


    private void Start()
    {
        //SetBotData();
    }


    private void SetBotData()
    {
        botPlayers = minPlayerRequired - DataManager.Instance.joinPlayerDatas.Count;
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int[] randomAvatars = avatars.Take(botPlayerImage.Length).ToArray();
        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int[] randomNames = names.Take(botPlayerImage.Length).ToArray();
        
        for (int i = botPlayerImage.Length - 1; i >= 0; i--)
        {
            StartCoroutine(DataManager.Instance.GetImages(BotManager.Instance.botUser_Profile_URL[randomAvatars[i]], botPlayerImage[i]));
            botPlayerNames[i].text = BotManager.Instance.botUserName[randomNames[i]];
        }
    }
}