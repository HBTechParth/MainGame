using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class DTPlayerManager : MonoBehaviour
{
    public GameObject winParticleObj;
    public Image avatarImg;
    public Text playerNameTxt;
    public Text playerBalanceTxt;
    public float balance;
    
    public string avatar;

    public int playerNo;
    public string playerId;
    public string tournamentId;
        

    private void Start()
    {
        //balance = 85126f;
        foreach (var t in DragonTigerManager.Instance.DTPlayerList)
        {
            var randomIndex = Random.Range(0, ExtensionMethods.BotPlayerBalance.Length);
            t.balance = ExtensionMethods.BotPlayerBalance[randomIndex];
        }
        
        playerBalanceTxt.text = balance.ToString(CultureInfo.InvariantCulture);
        GetPlayerImage();
    }

    private void Update()
    {
        if (balance < 10)
        {
            balance = 85126f;
        }
    }

    public void GetPlayerImage()
    {
        for (int i = 0; i < DragonTigerManager.Instance.DTPlayerList.Count; i++)
        {
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
        }
    }
    
}
