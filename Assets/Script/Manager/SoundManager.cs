
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [Header("Background Music")]
    public AudioSource bgAudio;
    public AudioClip bgClip;


    [Header("Button Audio")]
    public AudioSource btnAudio;
    public AudioClip btnClip;

    [Header("Roll Dice")]
    public AudioSource rollDiceAudio;
    public AudioClip rollDiceClip;

    [Header("Tick Timer")]
    public AudioSource tickTimerAudio;
    public AudioClip tickTimerClip;

    [Header("Time Out")]
    public AudioSource timeOutAudio;
    public AudioClip timeOutClip;

    [Header("Token Home")]
    public AudioSource tokenHomeAudio;
    public AudioClip tokenHomeClip;


    [Header("Token Move")]
    public AudioSource tokenMoveAudio;
    public AudioClip tokenMoveClip;

    [Header("Token Kill")]
    public AudioSource tokenKillAudio;
    public AudioClip tokenKillClip;

    [Header("User Turn")]
    public AudioSource userTurnAudio;
    public AudioClip userTurnClip;

    [Header("Winning")]
    public AudioSource winAudio;
    public AudioClip winClip;
    
    public AudioSource winClapAudio;
    public AudioClip winClapClip;


    [Header("Dragon Tiger-Andar Bahar-Roulette")]
    public AudioSource threeBetAudio;
    public AudioClip threeBetClip;

    [Header("Casino Win")]
    public AudioSource casinoWinAudio;
    public AudioClip casinoWinClip;

    [Header("Casino Turn")]
    public AudioSource casinoTurnAudio;
    public AudioClip casinoTurnClip;


    [Header("Casino Card Move")]
    public AudioSource casinoCardMoveAudio;
    public AudioClip casinoCardMoveClip;


    [Header("Casino Card Swipe")]
    public AudioSource casinoCardSwipeAudio;
    public AudioClip casinoCardSwipeClip;
    
    [Header("CarRunning")]
    public AudioSource carRunningAudio;
    public AudioClip carRunningClip;
    
    [Header("CarStop")]
    public AudioSource carStopAudio;
    public AudioClip carStopClip;
    
    [Header("Show Scratch Card")]
    public AudioSource cardPopAudio;
    public AudioClip cardPopClip;
    
    [Header("Lost Scratch Card")]
    public AudioSource cardLostAudio;
    public AudioClip cardLostClip;
    
    [Header("Aviator")]
    public AudioSource rocketThrustSound;
    public AudioClip rocketThrustClip;
    public AudioSource blastSound;
    public AudioClip blastSoundClip;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }

    }

    private void Start()
    {
        StartBackgroundMusic();
        /*if (DataManager.Instance.GetSound() == 0)
        {
        }
        else if (DataManager.Instance.GetSound() == 1)
        {
            StopBackgroundMusic();
        }*/
    }
    //private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(DateTime.UtcNow);
            //DateTime ist = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            //DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //print();
        }
    }*/


    public void StartBackgroundMusic()
    {
        if (DataManager.Instance.GetMusic() == 0)
        {
            bgAudio.clip = bgClip;
            bgAudio.Play();
            bgAudio.volume = 1.0f;
        }
        else
        {
            bgAudio.Stop();
        }
    }
    public void StopBackgroundMusic()
    {
        bgAudio.volume = 0.01f;
        //bgAudio.Stop();
    }

    public void ButtonClick()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        btnAudio.clip = btnClip;
        btnAudio.Play();
    }

    public void RollDice_Start_Sound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        rollDiceAudio.clip = rollDiceClip;
        rollDiceAudio.Play();
    }

    public void RollDice_Stop_Sound()
    {
        rollDiceAudio.Stop();
    }

    public void CarStartSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        carRunningAudio.clip = carRunningClip;
        carRunningAudio.Play();
    }
    
    public void CarStopSound()
    {
        carRunningAudio.Stop();
    }
    
    public void CarWinSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        carStopAudio.clip = carStopClip;
        carStopAudio.Play();
    }
    
    public void CarWinStopSound()
    {
        carStopAudio.Stop();
    }

    public void TickTimerSound()
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            tickTimerAudio.clip = tickTimerClip;
            tickTimerAudio.Play();
        }
    }
    public void TickTimerStop()
    {
        tickTimerAudio.Stop();
    }

    public void TimeOutSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        timeOutAudio.clip = timeOutClip;
        timeOutAudio.Play();
    }

    public void TokenHomeSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        tokenHomeAudio.clip = tokenHomeClip;
        tokenHomeAudio.Play();
    }


    public void TokenMoveSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        tokenMoveAudio.clip = tokenMoveClip;
        tokenMoveAudio.Play();
    }

    public void TokenKillSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        tokenKillAudio.clip = tokenKillClip;
        tokenKillAudio.Play();
    }

    public void UserTurnSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        userTurnAudio.clip = userTurnClip;
        userTurnAudio.Play();
    }

    public void WinSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        winAudio.clip = winClip;
        winAudio.Play();
    }
    
    public void WinClapSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        winClapAudio.clip = winClapClip;
        winClapAudio.Play();
    }

    public void StopAllSound()
    {
        TickTimerStop();
        TickTimerStop();
    }


    #region Casino Game

    public void ThreeBetSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        threeBetAudio.clip = threeBetClip;
        threeBetAudio.Play();
    }

    public void CasinoWinSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        casinoWinAudio.clip = casinoWinClip;
        casinoWinAudio.Play();
    }
    public void CasinoTurnSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        casinoTurnAudio.clip = casinoTurnClip;
        casinoTurnAudio.Play();
    }

    public void CasinoCardMoveSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        casinoCardMoveAudio.clip = casinoCardMoveClip;
        casinoCardMoveAudio.Play();
    }

    public void CasinoCardSwipeSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        casinoCardSwipeAudio.clip = casinoCardSwipeClip;
        casinoCardSwipeAudio.Play();
    }

    #endregion
    
    public void CardPopSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        cardPopAudio.clip = cardPopClip;
        cardPopAudio.Play();
    }
    
    public void CardLostSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        cardLostAudio.clip = cardLostClip;
        cardLostAudio.Play();
    }


    public void PlayRocketThrustSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        rocketThrustSound.clip = rocketThrustClip;
        rocketThrustSound.Play();
    }
    
    public void StopRocketThrustSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        rocketThrustSound.clip = rocketThrustClip;
        rocketThrustSound.Stop();
    }
    
    public void RocketBlastSound()
    {
        if (DataManager.Instance.GetSound() != 0) return;
        blastSound.clip = blastSoundClip;
        blastSound.Play();
    }
    
    



}
