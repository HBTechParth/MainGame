using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public bool isEnter;
    public Slider fillImg;
    public float waitTime = 10f;
    public double percantage = 0;
    private void Start()
    {
        fillImg.value = 0;
        isEnter = false;
    }

    private void Update()
    {
        fillImg.value += 1.0f / waitTime * Time.deltaTime;

        if (percantage >= 0)
        {
            percantage += Time.deltaTime * 10;

            //perTxt.text = ((int)percantage).ToString() + "%";
            //print("fill Value : " + fillImg.value);
            if (fillImg.value >= 1 && isEnter == false)
            {
                //SceneManager.LoadScene("Login");
                isEnter = true;
                Debug.Log("Log  =  >  " + DataManager.Instance.GetLoginValue().ToString());
                if (DataManager.Instance.GetLoginValue() == "Y")
                {
                    print("___________________This is called in splash__________");
                    //OpenPinDialog(2);
                    //LudoSignFirstScreen();
                    SceneManager.LoadScene("Main");

                    PlayerPrefs.SetInt("OpenReffer", 1);
                    //LoadSceneMainMenu();
                }
                else
                {
                    //PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene("Login");
                    //SceneManager.LoadScene("Splash");
                   // Debug.LogWarning("fill bar to Login");

                }
            }

        }
    }
}
