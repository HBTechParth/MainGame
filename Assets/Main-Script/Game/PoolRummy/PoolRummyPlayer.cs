using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolRummyPlayer : MonoBehaviour
{
    public List<Image> cardImages;

    public List<int> shuffledList = new List<int>();
    //public List<PoolRummyManager.CardSuffle> cards;
    public List<PoolCardScript> cards;

    public void CardDistribute()
    {
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if (DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas[i].userId)
            {
                PoolRummyManager.Instance.player1.playerNo = i + 1;
            }
        }
        switch (PoolRummyManager.Instance.player1.playerNo)
        {
            case 1:
                for (int i = 0; i < 13; i++)
                    shuffledList.Add(PoolRummyManager.Instance.distributedCardsList[i]);
                break;
            case 2:
                for (int i = 13; i < 26; i++)
                    shuffledList.Add(PoolRummyManager.Instance.distributedCardsList[i]);
                break;

            case 3:
                for (int i = 26; i < 39; i++)
                    shuffledList.Add(PoolRummyManager.Instance.distributedCardsList[i]);
                break;
            case 4:
                for (int i = 39; i < 52; i++)
                    shuffledList.Add(PoolRummyManager.Instance.distributedCardsList[i]);
                break;
            case 5:
                for (int i = 52; i < 65; i++)
                    shuffledList.Add(PoolRummyManager.Instance.distributedCardsList[i]);
                break;
            case 6:
                for (int i = 64; i < 78; i++)
                    shuffledList.Add(PoolRummyManager.Instance.distributedCardsList[i]);
                break;
        }
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].card.cardNo = PoolRummyManager.Instance.cardShuffles[shuffledList[i]].cardNo;
            cards[i].card.color = PoolRummyManager.Instance.cardShuffles[shuffledList[i]].color;
            cards[i].card.cardSprite = PoolRummyManager.Instance.cardShuffles[shuffledList[i]].cardSprite;
            cards[i].card.isWildJoker = PoolRummyManager.Instance.cardShuffles[shuffledList[i]].isWildJoker;
            if (cards[i].card.isWildJoker)
                cards[i].wildJoker.SetActive(true);
            cardImages[i].sprite = cards[i].card.cardSprite;
            //PoolRummyManager.Instance.discardCardList.Add(shuffledList[i]);
            //PoolRummyManager.Instance.cardShuffles.RemoveAt(rng);
        }
        RemoveDistributedCardsFromDeck(PoolRummyManager.Instance.discardCardList);
    }

    public IEnumerator CardDistributeAnimation()
    {
        for (int i = 0; i < cardImages.Count; i++)
        {

        }
        yield return null;
    }

    public void RemoveDistributedCardsFromDeck(List<int> cardList)
    {
        List<int> tempCardList = new List<int>(cardList);
        tempCardList.Sort();
        tempCardList.Reverse();
        foreach (int item in tempCardList)
        {
            if (PoolRummyManager.Instance.closedDeck.Contains(PoolRummyManager.Instance.cardShuffles[item]))
            {
                PoolRummyManager.Instance.closedDeck.Remove(PoolRummyManager.Instance.cardShuffles[item]);
            }
            else
            {
                print("index out of range" + item);
            }
        }
    }
}
