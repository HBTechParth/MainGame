using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    public DailyRewardData[] rewards;
    public Text totalEarningsText;
    public GameObject winAnimation;
    private int currentDay;
    private int totalEarnings;
    private const string RewardKey = "DailyRewardStatus";
    private const string DayKey = "CurrentDay";
    private const string EarningsKey = "TotalEarnings";
    private const string LastClaimDateKey = "LastClaimDate";

    void Start()
    {
        // Load current day from PlayerPrefs
        currentDay = PlayerPrefs.GetInt(DayKey, 1);
        // Load total earnings from PlayerPrefs
        totalEarnings = PlayerPrefs.GetInt(EarningsKey, 0);
        UpdateTotalEarningsText();
        LoadRewardsStatus();
        UpdateRewards();
    }

    private void LoadRewardsStatus()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].claimed = PlayerPrefs.GetInt(RewardKey + i, 0) == 1;
            UpdateRewardStatus(i);
        }
    }

    private void SaveRewardStatus(int dayIndex, bool claimed)
    {
        PlayerPrefs.SetInt(RewardKey + dayIndex, claimed ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    private void SaveTotalEarnings()
    {
        PlayerPrefs.SetInt(EarningsKey, totalEarnings);
        PlayerPrefs.Save();
    }
    
    private void UpdateTotalEarningsText()
    {
        totalEarningsText.text = "₹ " + totalEarnings;
    }

    private void UpdateRewardStatus(int index)
    {
        if (rewards[index].claimed)
        {
            rewards[index].status.text = "Claimed";
            rewards[index].claimBtn.SetActive(false);
            rewards[index].status.color = new Color(236, 156, 57);
        }
        else if (index == currentDay - 1)
        {
            rewards[index].status.text = "Claim Now";
            rewards[index].status.color = new Color(0, 0, 0, 255);
            rewards[index].claimBtn.SetActive(true);
        }
        else
        {
            rewards[index].status.text = "Wait for the day";
            rewards[index].claimBtn.SetActive(false);
            rewards[index].status.color = new Color(236, 156, 57);
        }
    }

    private void UpdateRewards()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            int dayIndex = i; // Capture the current value of i
            if (dayIndex == currentDay - 1 && !rewards[dayIndex].claimed)
            {
                rewards[dayIndex].button.interactable = true;
                rewards[dayIndex].button.onClick.RemoveAllListeners(); // Remove previous listeners to avoid duplication
                rewards[dayIndex].button.onClick.AddListener(() => ClaimReward(dayIndex));
            }
            else
            {
                rewards[dayIndex].button.interactable = false;
            }
            UpdateRewardStatus(dayIndex);
        }
    }

    public void ClaimReward(int index)
    {
        if (!rewards[index].claimed)
        {
            SoundManager.Instance.ButtonClick();
            rewards[index].claimed = true;
            totalEarnings += rewards[index].amount;
            UpdateTotalEarningsText();
            SaveRewardStatus(index, true);
            SaveTotalEarnings();

            // Perform reward action
            DataManager.Instance.BonusDebitAmount_Credit((rewards[index].amount / 1).ToString(), "Daily Reward", "won");
            SoundManager.Instance.CasinoWinSound();
            
            // Update last claim date to today
            PlayerPrefs.SetString(LastClaimDateKey, System.DateTime.Now.ToString());
            PlayerPrefs.Save();

            // Increment day
            currentDay++;
            if (currentDay > rewards.Length)
            {
                currentDay = 1; // Reset to day 1 after 7 days
                ResetRewards();
            }

            PlayerPrefs.SetInt(DayKey, currentDay);
            PlayerPrefs.Save();
            
            UpdateRewardStatus(index);
            SoundManager.Instance.WinClapSound();
            winAnimation.gameObject.SetActive(true);

            StartCoroutine(DestroyAfterAnimation());
        }
    }
    
    private IEnumerator DestroyAfterAnimation()
    {
        
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        winAnimation.gameObject.SetActive(false);

        Destroy(gameObject); // Destroy the prefab after animation
    }

    private void ResetRewards()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].claimed = false;
            SaveRewardStatus(i, false);
            UpdateRewardStatus(i);
        }
    }
    
    public void OnBackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        Destroy(gameObject);
    }
    
}

[System.Serializable]
public class DailyRewardData
{
    public int day;
    public string title;
    public int amount;
    public bool claimed; // true if claimed, false otherwise
    public Button button;
    public Text status; // "Claim Now", "Claimed", "Wait for the day"
    public GameObject claimBtn;
}

