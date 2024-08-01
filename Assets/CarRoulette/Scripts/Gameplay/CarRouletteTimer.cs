using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRouletteTimer : MonoBehaviour
{

    public float time;
    int a = 0;
    public bool starttimer;
    bool CallSpWinner;
    bool SendCoinsSpinner;
    // Start is called before the first frame update
    void Start()
    {
        //time = GetComponent<CarRouletteScript>().TimerTime;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (starttimer)
        {
            time = time - Time.deltaTime;
            a = (int)time;
           
            if (a < 10)
            {
                if (!SendCoinsSpinner)
                {
                    SendCoinsSpinner = true;
                    GetComponent<CarRouletteScript>().SendCoinsNow();
                    print("StopBetting!!!!!");
                    GetComponent<CarRouletteScript>().StopBetPanel.SetActive(true);
                }
                GetComponent<CarRouletteScript>().BettingStarted = false;
                if(a<4)
                {
                    if (!CallSpWinner)
                    {
                        CallSpWinner = true;

                        GetComponent<CarRouletteScript>().SetSpinWinner();
                    }
                }
               

                GetComponent<CarRouletteScript>().TimerText.text = a.ToString();
            }
            else
            {
                GetComponent<CarRouletteScript>().TimerText.text = (a-10).ToString();
            }
            if (a == 0)
            {
                GetComponent<CarRouletteScript>().StopBetPanel.SetActive(false);
                starttimer = false;
                CallSpWinner = false;
                SendCoinsSpinner = false;
                GetComponent<CarRouletteScript>().OnTimerFinished();
                //time = GetComponent<DragonTigerManager>().TimerTime;
            }
        }*/
    }
}
