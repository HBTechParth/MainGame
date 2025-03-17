using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletBotPlayers : MonoBehaviour
{

    public Image avatarImg;
    public Text playerNameTxt;
    public string avatar;


    // Start is called before the first frame update
    void Start()
    {
        GetPlayerImage();
    }
    
    
    public void GetPlayerImage()
    {
        for (int i = 0; i < RouletteManager.Instance.botPlayersList.Count; i++)
        {
            StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
        }
    }
}
