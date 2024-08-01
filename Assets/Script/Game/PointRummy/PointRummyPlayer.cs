using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointRummyPlayer : MonoBehaviour
{
    public List<Image> cardImages;

    //public List<PointRummyManager.CardSuffle> cards;
    public List<CardScript> cards;
        public List<int> shuffledList = new List<int>();
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardDistribute()
    {
        for (int i = 0; i < DataManager.Instance.joinPlayerDatas.Count; i++)
        {
            if(DataManager.Instance.playerData._id == DataManager.Instance.joinPlayerDatas[i].userId)
            {
                PointRummyManager.Instance.player1.playerNo = i + 1;
            }
        }
        switch (PointRummyManager.Instance.player1.playerNo)
        {
            case 1:
                for (int i = 0; i < 13; i++)
                    shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
                break;
            case 2:
                for (int i = 13; i < 26; i++)
                    shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
                break;

            case 3:
                for (int i = 26; i < 39; i++)
                    shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
                break;
            case 4:
                for (int i = 39; i < 52; i++)
                    shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
                break;
            case 5:
                for (int i = 52; i < 65; i++)
                    shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
                break;
            case 6:
                for (int i = 64; i < 78; i++)
                    shuffledList.Add(PointRummyManager.Instance.distributedCardsList[i]);
                break;
        }
        for (int i = 0; i < cards.Count; i++)
        {
            
            cards[i].card.cardNo = PointRummyManager.Instance.cardShuffles[shuffledList[i]].cardNo;
            cards[i].card.color = PointRummyManager.Instance.cardShuffles[shuffledList[i]].color;
            cards[i].card.cardSprite = PointRummyManager.Instance.cardShuffles[shuffledList[i]].cardSprite;
            cards[i].card.isWildJoker = PointRummyManager.Instance.cardShuffles[shuffledList[i]].isWildJoker;
            if (cards[i].card.isWildJoker)
                cards[i].wildJoker.SetActive(true);
            cardImages[i].sprite = cards[i].card.cardSprite;
            //PointRummyManager.Instance.discardCardList.Add(shuffledList[i]);
            //PointRummyManager.Instance.cardShuffles.RemoveAt(rng);
        }
        RemoveDistributedCardsFromDeck(PointRummyManager.Instance.distributedCardsList);
        
    }

    public void RemoveDistributedCardsFromDeck(List<int> cardList)
    {
        List<int> tempCardList = new List<int>(cardList);
        tempCardList.Sort();
        tempCardList.Reverse();
        foreach (int item in tempCardList)
        {
            if (PointRummyManager.Instance.closedDeck.Contains(PointRummyManager.Instance.cardShuffles[item]))
            {
                PointRummyManager.Instance.closedDeck.Remove(PointRummyManager.Instance.cardShuffles[item]);
            }
            else
            {
                print("index out of range" + item);
            }
        }
    }

    public IEnumerator CardDistributeAnimation()
    {
        for (int i = 0; i < cardImages.Count; i++)
        {

        }
        yield return null;
    }
}
