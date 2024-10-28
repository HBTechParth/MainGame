using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectTableManager : MonoBehaviour
{

    public TextMeshProUGUI bootText;
    public TextMeshProUGUI potLimitText;
    public TextMeshProUGUI minBuyText;
    public float bootValue;
    public float potValue;
    public float minBuyIn;
    public float challLimit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickJoinButton()
    {
        MainMenuManager.Instance.selectedValue = bootValue;
        MainMenuManager.Instance.potLimitValue = potValue;
        MainMenuManager.Instance.minBuyINValue = minBuyIn;
        MainMenuManager.Instance.challLimit = challLimit;
        

        MainMenuManager.Instance.JoinButtonClick();
    }
}
