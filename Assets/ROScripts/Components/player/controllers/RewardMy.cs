using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class RewardMy : MonoBehaviour
{
    public Button myButton;         
    public Text timerText;          

    private DateTime nextButtonTime;

    private void Awake()
    {
       
    }
    void Start()
    {
        LoadCooldown();
        UpdateUI(); 
    }

    void Update()
    {
        if (IsCooldownActive())
        {
            TimeSpan timeLeft = nextButtonTime - DateTime.Now;
            timerText.text = $"Next action in: {timeLeft.Hours}h : {timeLeft.Minutes}m : {timeLeft.Seconds}s";
            myButton.interactable = false;
        }
        else
        {
            timerText.text = "Let's Go!";
            myButton.interactable = true;
        }
    }

    public void StartCooldown()
    {
        nextButtonTime = DateTime.Now.AddHours(3);
        PlayerPrefs.SetString("NextButtonTime", nextButtonTime.ToString());
        PlayerPrefs.Save();

        UpdateUI(); 
    }

    bool IsCooldownActive()
    {
        return DateTime.Now < nextButtonTime;
    }

    void LoadCooldown()
    {
        if (PlayerPrefs.HasKey("NextButtonTime"))
        {
            nextButtonTime = DateTime.Parse(PlayerPrefs.GetString("NextButtonTime"));
        }
        else
        {
            nextButtonTime = DateTime.Now;
        }
    }

    void UpdateUI()
    {
        if (IsCooldownActive())
        {
            myButton.interactable = false;
        }
        else
        {
            myButton.interactable = true;
        }
    }
}
