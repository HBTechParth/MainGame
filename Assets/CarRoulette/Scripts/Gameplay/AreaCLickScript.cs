using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AreaCLickScript : MonoBehaviour
{
    public GameObject GManager;
    public GameObject ChipPrefab;
    public GameObject Player;
    public TextMeshProUGUI TotalText;
    public TextMeshProUGUI MybetText;
    public int AreaIndex;
    public Transform ChipThrowArea;

    GameObject Manager;
    
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("MainManger");
    }


   


    /*public void AreaDisplayer()
    {
        TotalText.text = GManager.GetComponent<CarRouletteScript>().TotalBetAreas[AreaIndex].ToString();
        MybetText.text = GManager.GetComponent<CarRouletteScript>().BetAreas[AreaIndex].ToString();


    }
    public void AreaClick()
    {
        
        if (GManager != null)
        {
            if (GManager.GetComponent<CarRouletteScript>().BettingStarted)
            {
                int coins = GManager.GetComponent<CarRouletteScript>().ChipButtons[GManager.GetComponent<CarRouletteScript>().SelectedChipIndex].GetComponent<ChipButtonScript>().Value;
                if (Manager != null)
                {
                    /*if (Manager.GetComponent<ManagerScript>().UserCoins >= coins)
                    {
                        GManager.GetComponent<CarRouletteScript>().BetAreas[AreaIndex] += coins;
                        GManager.GetComponent<CarRouletteScript>().TotalBetAreas[AreaIndex] += coins;
                        GManager.GetComponent<CarRouletteScript>().UserCoins -= coins;
                        Manager.GetComponent<ManagerScript>().UserCoins -= coins;
                        GManager.GetComponent<BotManagerScript>().ThrowCoinToArea(ChipThrowArea, Player.transform, GManager.GetComponent<CarRouletteScript>().SelectedChipIndex);
                       // SendCoinUpdates(AreaIndex, coins);


                    }#1#
                }
                else
                {
                    if (GManager.GetComponent<CarRouletteScript>().UserCoins >= coins)
                    {
                        GManager.GetComponent<CarRouletteScript>().BetAreas[AreaIndex] += coins;
                        GManager.GetComponent<CarRouletteScript>().TotalBetAreas[AreaIndex] += coins;
                        GManager.GetComponent<CarRouletteScript>().UserCoins -= coins;
                        
                        GManager.GetComponent<BotManagerScript>().ThrowCoinToArea(ChipThrowArea, Player.transform, GManager.GetComponent<CarRouletteScript>().SelectedChipIndex);
                        


                    }
                }
                   
            }
               
                
        }
    }*/

    void SendCoinUpdates(int i,int val)
    {
        if(i==0)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, val, 0, 0, 0, 0, 0, 0, 0);
        }
        else if (i == 1)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, val, 0, 0, 0, 0, 0, 0);
        }
        else if (i == 2)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, 0, val, 0, 0, 0, 0, 0);
        }
        else if (i == 3)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, 0, 0, val, 0, 0, 0, 0);
        }
        else if (i == 4)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, 0, 0, 0, val, 0, 0, 0);
        }
        else if (i == 5)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, 0, 0, 0, 0, val, 0, 0);
        }
        else if (i == 6)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, 0, 0, 0, 0, 0, val, 0);
        }
        else if (i == 7)
        {
            //Manager.GetComponent<ManagerScript>().UpdateCoinsCarRoulette(-val, 0, 0, 0, 0, 0, 0, 0, val);
        }

    }
    // Update is called once per frame
    /*void Update()
    {
        AreaDisplayer();
    }*/
}
