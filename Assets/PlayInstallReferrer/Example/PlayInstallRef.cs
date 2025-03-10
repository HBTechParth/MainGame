using System.Collections;
using System.Collections.Generic;
using Ugi.PlayInstallReferrerPlugin;
using UnityEngine;

public class PlayInstallRef : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayInRef();
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
                Debug.Log("Install referrer: " + installReferrerDetails.InstallReferrer);
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

