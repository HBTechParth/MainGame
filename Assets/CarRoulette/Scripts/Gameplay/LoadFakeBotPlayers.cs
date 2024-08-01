using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadFakeBotPlayers : MonoBehaviour
{
    public Image[] botPlayers;
    public Text[] botPlayersName;
    public Text[] botPlayersCoins;

    public void LoadBotPlayers()
    {
        int[] avatars = Enumerable.Range(0, BotManager.Instance.botUser_Profile_URL.Count).ToArray();
        avatars.Shuffle();
        int[] randomAvatars = avatars.Take(botPlayers.Length).ToArray();

        int[] names = Enumerable.Range(0, BotManager.Instance.botUserName.Count).ToArray();
        names.Shuffle();
        int[] randomNames = names.Take(botPlayers.Length).ToArray();

        for (int i = 0; i < botPlayers.Length; i++)
        {
            // Load random avatar
            string avatarURL = BotManager.Instance.botUser_Profile_URL[randomAvatars[i]];
            StartCoroutine(DataManager.Instance.GetImages(avatarURL, botPlayers[i].GetComponent<Image>()));

            // Load random name
            string playerName = BotManager.Instance.botUserName[randomNames[i]];
            botPlayersName[i].text = playerName;

            // Load random balance
            int randomBalance = Random.Range(0, ExtensionMethods.BotPlayerBalance.Length);
            botPlayersCoins[i].text = ExtensionMethods.BotPlayerBalance[randomBalance].ToString();
        }
    }
}
