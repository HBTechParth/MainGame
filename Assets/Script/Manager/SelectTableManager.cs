using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectTableManager : MonoBehaviour
{

    public TextMeshProUGUI bootText;
    public TextMeshProUGUI potLimitText;
    public float bootValue;
    public float potValue;

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
        MainMenuManager.Instance.JoinButtonClick();
    }
}
