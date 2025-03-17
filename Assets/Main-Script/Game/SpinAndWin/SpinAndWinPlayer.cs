using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpinAndWinPlayer : MonoBehaviour
{
   public GameObject winParticleObj;
       public Image avatarImg;
       public Text playerNameTxt;
       public Text playerBalanceTxt;
       public float balance;
       
       public string avatar;
   
       public int playerNo;
       public string playerId;
       public string tournamentId;
   
       private void Start()
       {
           //balance = 99999f;
           //playerBalanceTxt.text = balance.ToString(CultureInfo.InvariantCulture);
           
       }
   
       public void GetPlayerImage()
       {
           //StartCoroutine(DataManager.Instance.GetImages(avatar, avatarImg));
           DataManager.Instance.LoadProfileImage(avatar, avatarImg);
       }
}
