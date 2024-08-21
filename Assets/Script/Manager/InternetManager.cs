using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetManager : MonoBehaviour
{
    public static InternetManager Instance;
    public GameObject internetObj;
    public GameObject firstInternetObj;
    public GameObject updateObj;

    public bool isCheckUpdate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }

        print("Applicationn Version : " + Application.version);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            FirstCheckInterenet();
        else
            InvokeRepeating(nameof(CheckInterenet), 0, 3);
        CheckUpdate();
    }

    public void CheckUpdate()
    {
        if (Application.version == DataManager.Instance.appVersion) return;
        print("New Version available");
        if (updateObj != null)
        {
            Instantiate(updateObj, transform);
        }
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckInterenet()
    {
        if (InternetPanel.Instance == null && Application.internetReachability == NetworkReachability.NotReachable)
        {
            Instantiate(internetObj, this.transform);
            Time.timeScale = 0;
        }
    }
    void FirstCheckInterenet()
    {
        Instantiate(firstInternetObj, this.transform);
        Time.timeScale = 0;
    }
}
