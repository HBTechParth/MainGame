using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class WinBarManager : MonoBehaviour
{
    public TextMeshProUGUI[] winBarTexts; // Array of 3 UI Text elements
    private List<string> playerNames;
    private string[] gameNames = { "Joker", "Deal Rummy", "Poker", "Teen Patti", "Ludo", "Jhandi Munda", "Andar Bahar", "Roulette", "CarRoulette", "AK47 TeenPatti", "7Up Down", "Point Rummy", "Pool Rummy", "Dragon Tiger", "Avaitor" };

    private List<string> remainingNames;

    void Start()
    {
        playerNames = GenerateNamesList();
        remainingNames = new List<string>(playerNames);
        StartCoroutine(UpdateWinBar());
    }

    List<string> GenerateNamesList()
    {
        List<string> names = new List<string>
        {
            // Boys' Names
            "Aarav", "Vivaan", "Aditya", "Vihaan", "Arjun", "Sai", "Krishna", "Ishaan", "Shaurya", "Ayaan",
            // Girls' Names
            "Aaradhya", "Aarushi", "Aarya", "Aashvi", "Aayushi", "Abha", "Aditi", "Advika", "Aishwarya", "Akanksha",
        };

        for (int i = 0; i < 480; i++)
        {
            names.Add($"Name{i + 1}");
        }

        return names;
    }

    IEnumerator UpdateWinBar()
    {
        while (true)
        {
            if (remainingNames.Count == 0)
            {
                remainingNames = new List<string>(playerNames);
            }

            // Generate three random sentences and assign to winBarTexts
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, remainingNames.Count);
                string randomPlayer = remainingNames[randomIndex];
                remainingNames.RemoveAt(randomIndex);

                if (Random.Range(0, 2) == 1)
                {
                    randomPlayer = GenerateUsername(randomPlayer);
                }

                string randomGame = gameNames[Random.Range(0, gameNames.Length)];
                int randomAmount = GenerateRandomAmount();

                winBarTexts[i].text = $"{randomPlayer} won {randomAmount}  <sprite=0> in {randomGame}";
                winBarTexts[i].transform.localPosition = new Vector3(-1200f, winBarTexts[i].transform.localPosition.y, 0f); // Reset position
            }

            // Start the animation for all three texts
            for (int i = 0; i < 3; i++)
            {
                Vector3 initialPosition = winBarTexts[i].transform.localPosition; // Store the initial position
                float delay = i * 3.0f; // Increase stagger delay (e.g., 3 seconds between each text)
                StartCoroutine(MoveTextToPosition(winBarTexts[i], initialPosition, 1200f, 12f, delay));
            }

            // Wait for the animations to complete
            yield return new WaitForSeconds(12f + 3.0f); // Match this delay with the animation duration and stagger delay
        }
    }

    IEnumerator MoveTextToPosition(TextMeshProUGUI textElement, Vector3 initialPosition, float targetX, float duration, float delay)
    {
        Vector3 targetPosition = new Vector3(targetX, textElement.transform.localPosition.y, 0f);

        yield return new WaitForSeconds(delay); // Staggered start

        while (true)
        {
            float elapsedTime = 0f;

            // Move the text to the target position over the specified duration
            while (elapsedTime < duration)
            {
                textElement.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final position is set
            textElement.transform.localPosition = targetPosition;

            // Reset to the initial position
            textElement.transform.localPosition = initialPosition;

            // Start the animation again
            initialPosition = textElement.transform.localPosition;
        }
    }

    public int GenerateRandomAmount()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue < 0.8f)
        {
            return UnityEngine.Random.Range(100, 2001);
        }
        else if (randomValue < 0.95f)
        {
            return UnityEngine.Random.Range(2000, 5001);
        }
        else if (randomValue < 0.99f)
        {
            return UnityEngine.Random.Range(5000, 10001);
        }
        else
        {
            return UnityEngine.Random.Range(5, 101);
        }
    }

    string GenerateUsername(string name)
    {
        int randomNumber = Random.Range(100, 10000);
        return $"{name}{randomNumber}";
    }
}
