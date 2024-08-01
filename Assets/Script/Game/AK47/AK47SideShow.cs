using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AK47SideShow : MonoBehaviour
{
    public static AK47SideShow Instance;
    public float startSecond;
    public float secondCount;
    public Text secondTxt;

    public string sendId;
    public string currentId;


    bool isEnter = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        isEnter = false;
        secondCount = 10;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter == false)
        {
            secondCount -= Time.deltaTime;
            secondTxt.text = ((int)secondCount) + "s";
            if (((int)secondCount) == 0 && isEnter == false)
            {
                isEnter = true;
                AK47Manager.Instance.Cancel_SlideShow(sendId, currentId);
                this.gameObject.SetActive(false);
            }
        }
    }

    public void AcceptButtonClick()
    {
        AK47Manager.Instance.Accept_SlideShow(sendId, currentId);
        this.gameObject.SetActive(false);
    }

    public void CancelButtonClick()
    {
        AK47Manager.Instance.Cancel_SlideShow(sendId, currentId);
        this.gameObject.SetActive(false);
    }
}
