using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeGameLoading : MonoBehaviour
{
    public static SnakeGameLoading Instance;
    public float secondsCount;
    public bool isTwoPlayerReady;
    public Text timeTxt;
    public bool isTourEnter;
    bool isEnter1 = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
        
        DataManager.Instance.SetPlayedGame(DataManager.Instance.GetPlayedGame() + 1);
    }

    private void Update()
    {
        Timer();
    }
    
    private void Timer()
    {
        if (secondsCount > 0)
        {
            secondsCount -= Time.deltaTime;
            float seconds = secondsCount % 60;
            if (seconds.ToString("0").Length == 1)
            {
                timeTxt.text = "Starting....." + "0" + seconds.ToString("0");
            }
            else
            {
                timeTxt.text = "Starting....." + seconds.ToString("0");
            }
        }
        else if (isTourEnter == false)
        {
            isTourEnter = true;
            OpenAPlayMode();
        }

        // if (DataManager.Instance.isTwoPlayer && DataManager.Instance.joinPlayerDatas.Count == 2)
        // {
        //     OpenAPlayMode();
        // }
        // else if(DataManager.Instance.isFourPlayer && DataManager.Instance.joinPlayerDatas.Count == 4)
        // {
        //     OpenAPlayMode();
        // }
    }

    private void OpenAPlayMode()
    {
        isEnter1 = false;
        if (isTwoPlayerReady && isEnter1 == false)
        {
            isEnter1 = true;
            StartCoroutine(MainMenuManager.Instance.LoadSnakeScene());
        }
        else if (BotManager.Instance.isConnectBot && isEnter1 == false)
        {
            isEnter1 = true;
            StartCoroutine(MainMenuManager.Instance.LoadSnakeScene());
        }
        //MainMenuManager.Instance.SetSnakeGame();
        
        //TestSocketIO.Instace.SetGameId(DataManager.Instance.tournamentID);
    }
}
