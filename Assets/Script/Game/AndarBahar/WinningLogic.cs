using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinningLogic : MonoBehaviour
{
    public static WinningLogic instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() { }

    // Card structure matching AndarBaharManager's CardSuffle
    [System.Serializable]
    public class Card
    {
        public int cardNo; // Card number
        public string color; // Card color (e.g., Clubs, Spades)
        public Sprite cardSprite; // Card sprite
    }

    public List<Card> generatedCards; // List of 20 shuffled cards
    public Card winningCard; // Winning card
    public bool isAndarWin; // True for Andar win, false for Bahar win

    public GameObject leftCardPrefab, rightCardPrefab; // Prefabs for Andar and Bahar cards
    public Transform cardParent; // Parent for generated cards
    public Transform startPosition; // Start position for animation

    public float animationDuration = 0.4f; // Duration of each card animation
    public float delayBetweenCards = 0.5f; // Delay between card animations

    // Method to set the winning card from an external script
    public void SetWinningCard(CardSuffle externalWinningCard, bool andarWin)
    {
        // Convert CardSuffle to WinningLogic's Card type
        winningCard = new Card
        {
            cardNo = externalWinningCard.cardNo,
            color = externalWinningCard.color.ToString(), // Assuming color is an enum in CardSuffle
            cardSprite = externalWinningCard.cardSprite
        };

        isAndarWin = andarWin;
    }

    public void ONCALLWINNING()
    {
        GenerateShuffledCards(20); // Generate a random list of cards
        EnsureWinningCardPlacement(); // Ensure winning card is placed on the correct side
        StartCoroutine(PlayCardAnimations());
    }

    // Generate 20 shuffled cards from the deck
    private void GenerateShuffledCards(int count)
    {
        // Convert AndarBaharManager.Instance.cardSuffles to this script's Card type
        generatedCards = new List<Card>();
        foreach (var cardSuffle in AndarBaharManager.Instance.cardSuffles)
        {
            Card newCard = new Card
            {
                cardNo = cardSuffle.cardNo,
                color = cardSuffle.color.ToString(), // Convert enum to string
                cardSprite = cardSuffle.cardSprite
            };
            generatedCards.Add(newCard);
        }

        // Shuffle the cards
        Shuffle(generatedCards);

        // If winning card is not already set, select a random winning card
        if (winningCard == null)
        {
            // Determine if the winning side is Andar or Bahar
            bool isAndarWin = Random.Range(0, 2) == 0;

            // Select a winning card from the correct indices (even for Andar, odd for Bahar)
            List<Card> potentialWinningCards = new List<Card>();
            for (int i = 0; i < generatedCards.Count; i++)
            {
                if ((isAndarWin && i % 2 == 0) || (!isAndarWin && i % 2 != 0))
                {
                    potentialWinningCards.Add(generatedCards[i]);
                }
            }

            // Randomly pick a winning card
            if (potentialWinningCards.Count > 0)
            {
                winningCard = potentialWinningCards[Random.Range(0, potentialWinningCards.Count)];
            }

            Debug.Log(isAndarWin ? "Winning side: Andar" : "Winning side: Bahar");
            Debug.Log($"Winning card: {winningCard.cardNo} {winningCard.color}");
        }

        // Remove all instances of the winning card number from the list
        generatedCards = generatedCards.Where(card => card.cardNo != winningCard.cardNo).ToList();

        // Shuffle again to randomize the remaining cards
        Shuffle(generatedCards);

        // Add the winning card to the first valid position (even for Andar, odd for Bahar)
        if (count > 0)
        {
            if (count % 2 == 0)
            {
                // Insert at the first Andar position (even index)
                generatedCards.Insert(0, winningCard);
            }
            else
            {
                // Insert at the first Bahar position (odd index)
                generatedCards.Insert(1, winningCard);
            }
        }

        // Limit to the required count
        generatedCards = generatedCards.GetRange(0, count);
    }



    // Shuffle the list of cards
    private void Shuffle(List<Card> cards)
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    // Ensure the winning card is placed on the correct side
    // Ensure the winning card is placed on the correct side
    private void EnsureWinningCardPlacement()
    {
        if (generatedCards == null || generatedCards.Count == 0)
        {
            Debug.LogError("The generatedCards list is empty or null.");
            return;
        }

        // Remove the winning card if it is already in the list
        generatedCards.RemoveAll(card => card.cardNo == winningCard.cardNo && card.color == winningCard.color);

        // Determine the possible positions based on the win condition
        List<int> validPositions = new List<int>();
        for (int i = 0; i < generatedCards.Count; i++)
        {
            if ((isAndarWin && i % 2 == 0) || (!isAndarWin && i % 2 != 0))
            {
                validPositions.Add(i);
            }
        }

        if (validPositions.Count == 0)
        {
            Debug.LogError("No valid positions available for the winning card.");
            return;
        }

        // Pick a random valid position
        int randomIndex = validPositions[Random.Range(0, validPositions.Count)];

        // Insert the winning card at the selected position
        generatedCards.Insert(randomIndex, winningCard);
        Debug.Log($"Winning card inserted at index {randomIndex}. AndarWin: {isAndarWin}");
    }


    // Play card animations
    private IEnumerator PlayCardAnimations()
    {
        for (int i = 0; i < generatedCards.Count; i++)
        {
            bool isAndar = i % 2 == 0;
            GameObject cardObj = Instantiate(isAndar ? leftCardPrefab : rightCardPrefab, cardParent);
            cardObj.transform.position = startPosition.position;
            cardObj.SetActive(true);

            Card currentCard = generatedCards[i];
            Vector3 targetPosition = isAndar ? leftCardPrefab.transform.position : rightCardPrefab.transform.position;

            // Move the card to its target position
            cardObj.transform.DOMove(targetPosition, animationDuration).OnComplete(() =>
            {
                // Play flip animation
                SoundManager.Instance.CasinoCardSwipeSound(); // Play card swipe sound
                cardObj.transform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(() =>
                {
                    // Set the sprite once the card is "flipped"
                    cardObj.GetComponent<Image>().sprite = currentCard.cardSprite;
                    cardObj.transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() =>
                    {
                        // Check for winning card
                        if (currentCard.cardNo == winningCard.cardNo)
                        {
                            Debug.Log(isAndarWin ? "Andar Wins!" : "Bahar Wins!");
                            AndarBaharManager.Instance.AFTERWINNING(isAndarWin);
                            generatedCards.Clear();
                            StopAllCoroutines(); // Stop further animations if the winning card is found
                        }
                    });
                });
            });

            // Wait before the next card animation
            yield return new WaitForSeconds(animationDuration + delayBetweenCards);
        }
    }
    private IEnumerator abc()
    {
        yield return new WaitForSeconds(2f);
        generatedCards.Clear();
    }
}
