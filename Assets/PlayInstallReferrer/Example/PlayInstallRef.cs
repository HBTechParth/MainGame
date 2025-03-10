using System.Collections;
using System.Collections.Generic;
using Ugi.PlayInstallReferrerPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayInstallRef : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayInstallRef instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayInRef()
    {

        PlayInstallReferrer.GetInstallReferrerInfo((installReferrerDetails) =>
        {
            Debug.Log("Install referrer details received!");

            // check for error
            if (installReferrerDetails.Error != null)
            {
                Debug.LogError("Error occurred!");
                if (installReferrerDetails.Error.Exception != null)
                {
                    Debug.LogError("Exception message: " + installReferrerDetails.Error.Exception.Message);
                }
                Debug.LogError("Response code: " + installReferrerDetails.Error.ResponseCode.ToString());
                return;
            }

            // print install referrer details
            if (installReferrerDetails.InstallReferrer != null)
            {
                string referrer = installReferrerDetails.InstallReferrer;
                Debug.Log("Install Referrer: " + referrer);

                // Check if "gclid" exists in the referrer string
                if (!string.IsNullOrEmpty(referrer) && referrer.Contains("mjhgfy"))
                {
                    Debug.Log(" gclid found in Install Referrer!");
                    Debug.Log("Log  =  >  " + DataManager.Instance.GetLoginValue().ToString());

                    if (DataManager.Instance.GetLoginValue() == "Y")
                    {
                        print("___________________This is called in splash__________");
                        //OpenPinDialog(2);
                        //LudoSignFirstScreen();
                        TestSocketIO.Instace.CallSocket();
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
                else
                {
                    Debug.Log(" gclid NOT found in Install Referrer.");
                    SceneManager.LoadScene("InstallRefFake");
                }
            }
            if (installReferrerDetails.ReferrerClickTimestampSeconds != null)
            {
                Debug.Log("Referrer click timestamp: " + installReferrerDetails.ReferrerClickTimestampSeconds);
            }
            if (installReferrerDetails.InstallBeginTimestampSeconds != null)
            {
                Debug.Log("Install begin timestamp: " + installReferrerDetails.InstallBeginTimestampSeconds);
            }
            if (installReferrerDetails.ReferrerClickTimestampServerSeconds != null)
            {
                Debug.Log("Referrer click timestamp server: " + installReferrerDetails.ReferrerClickTimestampServerSeconds);
            }
            if (installReferrerDetails.InstallBeginTimestampServerSeconds != null)
            {
                Debug.Log("Install begin timestamp server: " + installReferrerDetails.InstallBeginTimestampServerSeconds);
            }
            if (installReferrerDetails.InstallVersion != null)
            {
                Debug.Log("Install version: " + installReferrerDetails.InstallVersion);
            }
            if (installReferrerDetails.GooglePlayInstant != null)
            {
                Debug.Log("Google Play instant: " + installReferrerDetails.GooglePlayInstant);
            }
        });
    }
}

