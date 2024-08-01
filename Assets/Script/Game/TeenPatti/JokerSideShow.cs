using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokerSideShow : MonoBehaviour
{
    public static JokerSideShow Instance;
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
                JokerManager.Instance.Cancel_SlideShow(sendId, currentId);
                this.gameObject.SetActive(false);
            }
        }
    }

    public void AcceptButtonClick()
    {
        JokerManager.Instance.Accept_SlideShow(sendId, currentId);
        this.gameObject.SetActive(false);
    }

    public void CancelButtonClick()
    {
        JokerManager.Instance.Cancel_SlideShow(sendId, currentId);
        this.gameObject.SetActive(false);
    }
}
