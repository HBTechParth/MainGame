using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

namespace ScratchCard
{
    public class ScratchCardController : MonoBehaviour
    {
        public List<RewardItem> rewardItems = new List<RewardItem>();

        public GameObject cardObject;
        public Text messageText;
        public ScratchCardMaskUGUI scratchCardMask;
        private RewardItem selectedReward;
        private void Start()
        {
            // Intro animation for the card
            cardObject.transform.localScale = Vector3.zero;
            cardObject.transform.DOScale(Vector3.one, 1f)
                .SetEase(Ease.OutBack)
                .SetDelay(0.5f);
            
            selectedReward = SelectRandomReward();
            messageText.text = selectedReward.message;
            scratchCardMask.OnScratchCardCleared += HandleScratchCardCleared;
            SoundManager.Instance.CardPopSound();
        }
        
        private void HandleScratchCardCleared()
        {
            if (selectedReward.isMoney)
            {
                // Credit the bonus amount
                print("Add amount is called");
                DataManager.Instance.BonusDebitAmount_Credit((selectedReward.amount / 1).ToString(), "Card Reward", "won");
                SoundManager.Instance.CasinoWinSound();
            }
            else
            {
                // Handle non-money reward (leave for future use)
                HandleNonMoneyReward(selectedReward);
            }
        }

        private RewardItem SelectRandomReward()
        {
            // Calculate total priority sum
            int totalPriority = 0;
            foreach (RewardItem item in rewardItems)
            {
                totalPriority += item.priority;
            }

            // Generate a random value within the total priority range
            int randomValue = Random.Range(0, totalPriority);

            // Select the reward item based on priority
            int currentPriority = 0;
            foreach (RewardItem item in rewardItems)
            {
                currentPriority += item.priority;
                if (randomValue < currentPriority)
                {
                    return item;
                }
            }

            // Return the last reward item as a fallback
            return rewardItems[rewardItems.Count - 1];
        }

        private void HandleNonMoneyReward(RewardItem reward)
        {
            SoundManager.Instance.CardLostSound();
            print("Better luck next time");
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from the OnScratchCardCleared event
            scratchCardMask.OnScratchCardCleared -= HandleScratchCardCleared;
        }
    }
    
    [System.Serializable]
    public class RewardItem
    {
        public bool isMoney;
        public string message;
        public int amount;
        [Range(0, 100)]
        public int priority;
    }
}
